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

namespace GpgApi
{
    /// <summary>
    /// Verifies a file's signature.
    /// </summary>
    public sealed class GpgVerifySignature : GpgInterface
    {
        public String FileName { get; private set; }

        public KeyOwnerTrust SignatureTrust { get; private set; }
        public Boolean IsSigned { get; private set; }
        public Boolean IsGoodSignature { get; private set; }
        public KeyId SignatureKeyId { get; private set; }
        public DateTime SignatureDateTime { get; private set; }

        public GpgVerifySignature(String fileName)
        {
            FileName = fileName;
            SignatureTrust = KeyOwnerTrust.None;
            IsSigned = false;
            IsGoodSignature = false;
            SignatureKeyId = null;
            SignatureDateTime = DateTime.MinValue;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--verify " + Utils.EscapePath(FileName);
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(FileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound, FileName);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
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
            }

            return GpgInterfaceResult.Success;
        }
    }
}
