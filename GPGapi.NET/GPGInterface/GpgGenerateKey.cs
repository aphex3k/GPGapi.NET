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
using System.Security;

namespace GpgApi
{
    /// <summary>
    /// Generates a new key pair and adds it to the user's keyring.
    /// </summary>
    /// <remarks>
    /// Here is the list of <see cref="GpgApi.GpgInterfaceMessage"/> used by this class.
    /// <list type="bullet">
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.GeneratingPrimeNumbers"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.GeneratingDsaKey"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.GeneratingELGamalKey"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.NeedEntropy"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.SizeTooSmall"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.SizeTooBig"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.KeyNotCreated"/></term></item>
    /// </list>
    /// </remarks>
    public sealed class GpgGenerateKey : GpgInterface
    {
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public String Comment { get; private set; }
        public KeyAlgorithm Algorithm { get; private set; }
        public UInt32 Size { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public String FingerPrint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgGenerateKey"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="comment"></param>
        /// <param name="algorithm"></param>
        /// <param name="size"></param>
        /// <param name="expirationDate"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgGenerateKey(Name name, Email email, String comment, KeyAlgorithm algorithm, UInt32 size, DateTime expirationDate)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (email == null)
                throw new ArgumentNullException("email");

            Name = name;
            Email = email;
            Comment = comment;
            Algorithm = algorithm;
            Size = size;
            ExpirationDate = expirationDate;
            FingerPrint = null;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--gen-key --allow-freeform-uid";
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            Int32 minsize = 1024;
            Int32 maxsize = 4096;

            switch (Algorithm)
            {
                case KeyAlgorithm.RsaRsa:
                    minsize = 1024;
                    maxsize = 4096;
                    break;
                case KeyAlgorithm.DsaELGamal:
                    minsize = 1024;
                    maxsize = 3072;
                    break;
            }

            if (Size < minsize)
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.SizeTooSmall, minsize);

            if (Size > maxsize)
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.SizeTooBig, maxsize);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.GET_LINE:
                {
                    switch (line)
                    {
                        case "keygen.algo": WriteLine(GpgConvert.ToId(Algorithm)); break;
                        case "keygen.size": WriteLine(Size); break;
                        case "keygen.valid": WriteLine(GpgConvert.ToDays(ExpirationDate)); break;
                        case "keygen.name": WriteLine(EncodeString(Name)); break;
                        case "keygen.email": WriteLine(EncodeString(Email)); break;
                        case "keygen.comment": WriteLine(EncodeString(Comment)); break;
                    }

                    break;
                }

                case GpgKeyword.PROGRESS:
                {
                    String[] parts = line.Split(' ');

                    if (parts.Length >= 4)
                    {
                        switch (parts[0])
                        {
                            case "primegen": return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.GeneratingPrimeNumbers);
                            case "pk_dsa": return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.GeneratingDsaKey);
                            case "pk_elg": return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.GeneratingELGamalKey);
                            case "need_entropy": return new GpgInterfaceResult(GpgInterfaceStatus.Processing, GpgInterfaceMessage.NeedEntropy);
                        }
                    }

                    break;
                }

                case GpgKeyword.KEY_CREATED:
                {
                    String[] parts = line.Split(' ');
                    FingerPrint = parts[1];

                    break;
                }

                case GpgKeyword.KEY_NOT_CREATED:
                {
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.KeyNotCreated);
                }

                case GpgKeyword.GET_HIDDEN:
                {
                    if (String.Equals(line, "passphrase.enter", StringComparison.Ordinal))
                    {
                        SecureString password = InternalAskPassphrase(Name, true);
                        if (IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WritePassword(password);
                    }

                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
