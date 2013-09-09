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
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace GpgApi
{
    /// <summary>
    /// Encrypt (and sign) a key.
    /// </summary>
    /// <remarks>
    /// Here is the list of <see cref="GpgApi.GpgInterfaceMessage"/> used by this class.
    /// <list type="bullet">
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.FileNotFound"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.BeginSigning"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.SignatureCreated"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.BeginEncryption"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.EndEncryption"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.SignatureKeyExpired"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.InvalidRecipient"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.InvalidFileName"/></term></item>
    /// </list>
    /// </remarks>
    public sealed class GpgEncrypt : GpgInterface
    {
        public String FileName { get; private set; }
        public String EncryptedFileName { get; private set; }
        public Boolean Armored { get; private set; }
        public Boolean HideUserIds { get; private set; }
        public KeyId SignatureKeyId { get; private set; }
        public IEnumerable<KeyId> Recipients { get; private set; }
        public CipherAlgorithm CipherAlgorithm { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgEncrypt"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encryptedFileName"></param>
        /// <param name="armored"></param>
        /// <param name="hideUserIds"></param>
        /// <param name="signatureKeyId"></param>
        /// <param name="recipients"></param>
        /// <param name="cipherAlgorithm"></param>
        public GpgEncrypt(String fileName, String encryptedFileName, Boolean armored, Boolean hideUserIds, KeyId signatureKeyId, IEnumerable<KeyId> recipients, CipherAlgorithm cipherAlgorithm)
        {
            FileName = fileName;
            EncryptedFileName = encryptedFileName;
            Armored = armored;
            HideUserIds = hideUserIds;
            SignatureKeyId = signatureKeyId;
            Recipients = recipients;
            CipherAlgorithm = cipherAlgorithm;
        }

        private Boolean _isSymmetric = false;

        // internal AND protected
        internal override String Arguments()
        {
            String args = "";

            args += "--output " + Utils.EscapePath(EncryptedFileName) + " ";

            if (Armored)
                args += "--armor ";

            if (HideUserIds)
                args += "--throw-keyids ";

            if (SignatureKeyId != null)
            {
                args += "--sign ";
                args += "--local-user " + SignatureKeyId + " ";
            }

            if (CipherAlgorithm != CipherAlgorithm.None)
            {
                args += "--symmetric ";
                args += "--cipher-algo " + GpgConvert.ToName(CipherAlgorithm) + " ";
            }

            if (Recipients != null)
            {
                String recipients = String.Empty;
                foreach (KeyId recipient in Recipients)
                    recipients += "--recipient " + recipient + " ";

                if (recipients.Length > 0)
                {
                    args += "--encrypt ";
                    args += recipients;
                }
            }

            args += Utils.EscapePath(FileName);

            return args;
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(FileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound, FileName);

            if (!Utils.IsValidPath(EncryptedFileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidFileName, EncryptedFileName);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.NEED_PASSPHRASE:
                {
                    _isSymmetric = false;
                    break;
                }

                case GpgKeyword.NEED_PASSPHRASE_SYM:
                {
                    _isSymmetric = true;
                    break;
                }

                case GpgKeyword.BAD_PASSPHRASE:
                {
                    if (IsMaxTries())
                        return GpgInterfaceResult.BadPassphrase;
                    break;
                }

                case GpgKeyword.GET_HIDDEN:
                {
                    if (String.Equals(line, "passphrase.enter", StringComparison.Ordinal))
                    {
                        SecureString password = InternalAskPassphrase(SignatureKeyId, _isSymmetric, _isSymmetric);
                        if (IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WritePassword(password);
                    }
                    break;
                }

                case GpgKeyword.INV_RECP:
                {
                    String[] parts = line.Split(' ');
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidRecipient, parts[0]);
                }

                case GpgKeyword.BEGIN_SIGNING:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.BeginSigning);

                case GpgKeyword.SIG_CREATED:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.SignatureCreated);

                case GpgKeyword.BEGIN_ENCRYPTION:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.BeginEncryption);

                case GpgKeyword.END_ENCRYPTION:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.EndEncryption);

                case GpgKeyword.KEYEXPIRED:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.SignatureKeyExpired);

                case GpgKeyword.GET_BOOL:
                {
                    switch (line)
                    {
                        case "untrusted_key.override":
                        case "openfile.overwrite.okay":
                            WriteLine("YES");
                            break;
                    }
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
