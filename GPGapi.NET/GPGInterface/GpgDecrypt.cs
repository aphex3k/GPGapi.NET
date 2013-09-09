#region License
/*
    Copyright (c) 2011 Jimmy Gilles <jimmygilles@gmail.com>
 
    This file is part of GpgApi.

    GpgApi is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, version 3 of the License.

    GpgApi is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with GpgApi. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion License

using System;
using System.Globalization;
using System.IO;
using System.Security;

namespace GpgApi
{
    /// <summary>
    /// Decrypts a file and checks the signature (if signed).
    /// </summary>
    /// <remarks>
    /// Here is the list of <see cref="GpgApi.GpgInterfaceMessage"/> used by this class.
    /// <list type="bullet">
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.BeginDecryption"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.EndDecryption"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.DecryptionOk"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.DecryptionFailed"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.NoSecretKey"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.NoPublicKey"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.DataError"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.InvalidFileName"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.FileNotFound"/></term></item>
    /// </list>
    /// </remarks>
    public sealed class GpgDecrypt : GpgInterface
    {
        public String EncryptedFileName { get; private set; }
        public String DecryptedFileName { get; private set; }
        public Int32 DecryptedDataLength { get; private set; }      // Taille des données décryptées.
        public DataType DecryptedDataType { get; private set; }     // Le type de données décryptées (binaires, texte, etc.)
        public DateTime EncryptionDateTime { get; private set; }    // La date à laquelle les données ont été encryptées
        public String OriginalFileName { get; private set; }        // Le nom original du fichier encrypté. Si les données ne proviennent pas d'un fichier, ce sera null
        public Boolean IsSigned { get; private set; }               // Indique si les données étaient signées avant d'être encryptées
        public Boolean IsGoodSignature { get; private set; }        // Indique si la signature est bonne (message non altéré)
        public DateTime SignatureDateTime { get; private set; }     // Indique la date et heure de la signature
        public KeyId SignatureKeyId { get; private set; }           // KeyId de la clé qui a signé les données.
        public KeyOwnerTrust SignatureTrust { get; private set; }   // Confiance en cette clé (peut-être récupéré à partir du trousseau de clés)

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgDecrypt"/> class.
        /// </summary>
        /// <param name="encryptedFileName"></param>
        /// <param name="decryptedFileName"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgDecrypt(String encryptedFileName, String decryptedFileName)
        {
            if (encryptedFileName == null)
                throw new ArgumentNullException("encryptedFileName");

            if (decryptedFileName == null)
                throw new ArgumentNullException("decryptedFileName");

            EncryptedFileName = encryptedFileName;
            DecryptedFileName = decryptedFileName;
            DecryptedDataLength = 0;
            DecryptedDataType = DataType.None;
            EncryptionDateTime = DateTime.MinValue;
            OriginalFileName = null;
            IsSigned = false;
            IsGoodSignature = false;
            SignatureDateTime = DateTime.MinValue;
            SignatureKeyId = null;
            SignatureTrust = KeyOwnerTrust.None;
        }

        private String _keyId = null;
        private String _last_enc_to = null;
        private Boolean _isSymmetric = false;
        private Boolean _googPassphrase = false;

        // internal AND protected
        internal override String Arguments()
        {
            String args = "";
            args += "--output " + Utils.EscapePath(DecryptedFileName);
            args += " ";
            args += "--decrypt " + Utils.EscapePath(EncryptedFileName);
            return args;
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(EncryptedFileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound);

            if (!Utils.IsValidPath(DecryptedFileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidFileName, DecryptedFileName);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.ENC_TO:
                {
                    String[] parts = line.Split(' ');

                    if (parts[0] != _last_enc_to)
                    {
                        _last_enc_to = parts[0];
                        ResetTries();
                    }

                    break;
                }

                case GpgKeyword.NEED_PASSPHRASE_SYM:
                {
                    _isSymmetric = true;
                    break;
                }

                case GpgKeyword.NEED_PASSPHRASE:
                {
                    String[] parts = line.Split(' ');
                    _keyId = parts[1];
                    _isSymmetric = false;
                    break;
                }

                case GpgKeyword.BEGIN_DECRYPTION:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.BeginDecryption);

                case GpgKeyword.END_DECRYPTION:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.EndDecryption);

                case GpgKeyword.DECRYPTION_OKAY:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.DecryptionOk);

                case GpgKeyword.DECRYPTION_FAILED:
                    return _googPassphrase ? new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.DecryptionFailed) : GpgInterfaceResult.BadPassphrase;

                case GpgKeyword.PLAINTEXT_LENGTH:
                {
                    DecryptedDataLength = Int32.Parse(line, CultureInfo.InvariantCulture);
                    break;
                }

                case GpgKeyword.PLAINTEXT:
                {
                    String[] parts = line.Split(' ');

                    String datatype = parts[0];
                    switch (datatype)
                    {
                        case "62": DecryptedDataType = DataType.Binary; break;
                        case "74": DecryptedDataType = DataType.Text; break;
                        case "75": DecryptedDataType = DataType.Utf8Text; break;
                    }

                    EncryptionDateTime = Utils.ConvertTimestamp(Double.Parse(parts[1], CultureInfo.InvariantCulture));
                    OriginalFileName = Uri.UnescapeDataString(parts[2]);

                    break;
                }

                case GpgKeyword.GET_HIDDEN:
                {
                    if (String.Equals(line, "passphrase.enter", StringComparison.Ordinal))
                    {
                        SecureString password = InternalAskPassphrase(_keyId, false, _isSymmetric);
                        if (IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WritePassword(password);
                    }

                    break;
                }

                case GpgKeyword.GOOD_PASSPHRASE:
                {
                    _googPassphrase = true;
                    break;
                }

                case GpgKeyword.GOODSIG:
                {
                    IsSigned = true;
                    IsGoodSignature = true;
                    SignatureKeyId = new KeyId(line.Split(' ')[0]);
                    break;
                }

                case GpgKeyword.BADSIG:
                {
                    IsSigned = true;
                    IsGoodSignature = false;
                    SignatureKeyId = new KeyId(line.Split(' ')[0]);
                    break;
                }

                case GpgKeyword.VALIDSIG:
                {
                    String[] parts = line.Split(' ');

                    String datetime = parts[2];
                    if (datetime.Contains("T"))
                    {
                        // ISO 8601
                        SignatureDateTime = DateTime.ParseExact(datetime, "s", CultureInfo.InvariantCulture);
                    }
                    else
                        SignatureDateTime = Utils.ConvertTimestamp(Double.Parse(datetime, CultureInfo.InvariantCulture));

                    break;
                }

                case GpgKeyword.NO_PUBKEY:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.NoPublicKey, line);

                case GpgKeyword.NO_SECKEY:
                {
                    if (_googPassphrase)
                        return GpgInterfaceResult.Success;
                    else
                        return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.NoSecretKey, line);
                }

                case GpgKeyword.NODATA:
                case GpgKeyword.UNEXPECTED:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.DataError);

                case GpgKeyword.TRUST_UNDEFINED:
                case GpgKeyword.TRUST_NEVER:
                {
                    SignatureTrust = KeyOwnerTrust.None;
                    break;
                }

                case GpgKeyword.TRUST_MARGINAL:
                {
                    SignatureTrust = KeyOwnerTrust.Marginal;
                    break;
                }

                case GpgKeyword.TRUST_FULLY:
                {
                    SignatureTrust = KeyOwnerTrust.Full;
                    break;
                }

                case GpgKeyword.TRUST_ULTIMATE:
                {
                    SignatureTrust = KeyOwnerTrust.Ultimate;
                    break;
                }

                case GpgKeyword.GET_BOOL:
                {
                    if (String.Equals(line, "openfile.overwrite.okay", StringComparison.Ordinal))
                        WriteLine("YES");
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
