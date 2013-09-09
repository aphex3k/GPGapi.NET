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
    /// Signs a file.
    /// </summary>
    public sealed class GpgSign : GpgInterface
    {
        public KeyId SignatureKeyId { get; private set; }
        public String FileName { get; private set; }
        public String SignedFileName { get; private set; }
        public Boolean Armored { get; private set; }

        public Boolean Signed { get; private set; }
        public DigestAlgorithm DigestAlgorithm { get; private set; }
        public KeyAlgorithm KeyAlgorithm { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signatureKeyId"></param>
        /// <param name="fileName"></param>
        /// <param name="signedFileName"></param>
        /// <param name="armored"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgSign(KeyId signatureKeyId, String fileName, String signedFileName, Boolean armored)
        {
            if (signatureKeyId == null)
                throw new ArgumentNullException("signatureKeyId");

            SignatureKeyId = signatureKeyId;
            FileName = fileName;
            SignedFileName = signedFileName;
            Armored = armored;
            Signed = false;
            KeyAlgorithm = KeyAlgorithm.None;
            DigestAlgorithm = DigestAlgorithm.None;
        }

        // internal AND protected
        internal override String Arguments()
        {
            String args = "";

            args += "--output " + Utils.EscapePath(SignedFileName) + " ";
            args += Armored ? "--clearsign " : "--sign ";
            args += "--local-user " + SignatureKeyId + " ";
            args += Utils.EscapePath(FileName);

            return args;
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(FileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound, FileName);

            if (!Utils.IsValidPath(SignedFileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidFileName, SignedFileName);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
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
                        SecureString password = InternalAskPassphrase(SignatureKeyId);
                        if (IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WritePassword(password);
                    }
                    break;
                }

                case GpgKeyword.SIG_CREATED:
                {
                    String[] parts = line.Split(' ');
                    Signed = true;
                    KeyAlgorithm = GpgConvert.ToKeyAlgorithm(Int32.Parse(parts[1], CultureInfo.InvariantCulture));
                    DigestAlgorithm = GpgConvert.ToDigestAlgorithm(Int32.Parse(parts[2], CultureInfo.InvariantCulture));
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
