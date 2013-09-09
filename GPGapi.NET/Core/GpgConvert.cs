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

namespace GpgApi
{
    internal static class GpgConvert
    {
        public static CipherAlgorithm ToCipherAlgorithm(Int32 i)
        {
            switch (i)
            {
                //case 1 : return CipherAlgorithm.IDEA;     // See comment in Enums.cs
                case 2 : return CipherAlgorithm.ThreeDes;
                case 3 : return CipherAlgorithm.Cast5;
                case 4 : return CipherAlgorithm.BlowFish;
                case 7 : return CipherAlgorithm.Aes;
                case 8 : return CipherAlgorithm.Aes192;
                case 9 : return CipherAlgorithm.Aes256;
                case 10 : return CipherAlgorithm.TwoFish;
                case 11 : return CipherAlgorithm.Camellia128;
                case 12 : return CipherAlgorithm.Camellia192;
                case 13 : return CipherAlgorithm.Camellia256;
                default : return CipherAlgorithm.None;
            }
        }

        public static DigestAlgorithm ToDigestAlgorithm(Int32 i)
        {
            switch (i)
            {
                case 1 : return DigestAlgorithm.MD5;
                case 2 : return DigestAlgorithm.Sha1;
                case 3 : return DigestAlgorithm.Rmd160;
                case 8 : return DigestAlgorithm.Sha256;
                case 9 : return DigestAlgorithm.Sha384;
                case 10 : return DigestAlgorithm.Sha512;
                case 11 : return DigestAlgorithm.Sha224;
                default : return DigestAlgorithm.None;
            }
        }

        public static CompressionAlgorithm ToCompressionAlgorithm(Int32 i)
        {
            switch (i)
            {
                case 1 : return CompressionAlgorithm.Zip;
                case 2 : return CompressionAlgorithm.ZLib;
                case 3 : return CompressionAlgorithm.BZip2;
                default : return CompressionAlgorithm.None;
            }
        }

        public static KeyAlgorithm ToKeyAlgorithm(Int32 i)
        {
            switch (i)
            {
                case 1 : return KeyAlgorithm.RsaRsa;
                case 2 : return KeyAlgorithm.RsaEncrypt;
                case 3 : return KeyAlgorithm.RsaSign;
                case 16 : return KeyAlgorithm.ELGamal;
                case 17 : return KeyAlgorithm.Dsa;
                case 20 : return KeyAlgorithm.DsaELGamal;
                default : return KeyAlgorithm.None;
            }
        }

        public static KeyTrust ToTrust(Char c)
        {
            c = Char.ToUpperInvariant(c);

            switch (c)
            {
                case 'O': return KeyTrust.Unknown;
                case 'I': return KeyTrust.Invalid;
                case 'R': return KeyTrust.Revoked;
                case 'E': return KeyTrust.Expired;
                case 'Q': return KeyTrust.Undefined;
                case 'N': return KeyTrust.None;
                case 'M': return KeyTrust.Marginal;
                case 'F': return KeyTrust.Full;
                case 'U': return KeyTrust.Ultimate;
                default: return KeyTrust.Unknown;
            }
        }

        public static KeyOwnerTrust ToOwnerTrust(Char c)
        {
            c = Char.ToUpperInvariant(c);

            switch (c)
            {
                case 'N': return KeyOwnerTrust.None;
                case 'M': return KeyOwnerTrust.Marginal;
                case 'U': return KeyOwnerTrust.Ultimate;
                case 'F': return KeyOwnerTrust.Full;
                default: return KeyOwnerTrust.None;
            }
        }

        public static KeyAlgorithm ToKeyAlgorithm(String s)
        {
            Int32 i;
            return Int32.TryParse(s, out i) ? GpgConvert.ToKeyAlgorithm(i) : KeyAlgorithm.None;
        }

        public static KeyTrust ToTrust(String s)
        {
            return String.IsNullOrEmpty(s) ? KeyTrust.Unknown : GpgConvert.ToTrust(s[0]);
        }

        public static KeyOwnerTrust ToOwnerTrust(String s)
        {
            return String.IsNullOrEmpty(s) ? KeyOwnerTrust.None : GpgConvert.ToOwnerTrust(s[0]);
        }

        /// <summary>
        /// For more information, see gpg source code, file keygen.c line 1518.
        /// </summary>
        /// <remarks>
        /// GPG does not accept a "None" algorithm, so RSA is used by default.
        /// But you should not call this function with the "None" algorithm.
        /// </remarks>
        public static Int32 ToId(KeyAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case KeyAlgorithm.None: return 1;
                case KeyAlgorithm.RsaRsa: return 1;
                case KeyAlgorithm.DsaELGamal: return 2;
                case KeyAlgorithm.Dsa: return 3;
                case KeyAlgorithm.RsaSign: return 4;
                case KeyAlgorithm.ELGamal: return 5;
                case KeyAlgorithm.RsaEncrypt: return 6;
                default: return 1;
            }
        }

        public static Int32 ToId(KeyOwnerTrust trust)
        {
            switch (trust)
            {
                case KeyOwnerTrust.Ultimate: return 5;
                case KeyOwnerTrust.Full: return 4;
                case KeyOwnerTrust.Marginal: return 3;
                case KeyOwnerTrust.None: return 2;
                default: return 2;
            }
        }

        public static String ToName(CipherAlgorithm algorithm)
        {
            switch (algorithm)
            {
                //case CipherAlgorithm.IDEA: return "IDEA";     // See comment in Enums.cs
                case CipherAlgorithm.None: return String.Empty;
                case CipherAlgorithm.ThreeDes: return "3DES";
                case CipherAlgorithm.Cast5: return "CAST5";
                case CipherAlgorithm.BlowFish: return "BLOWFISH";
                case CipherAlgorithm.Aes: return "AES";
                case CipherAlgorithm.Aes192: return "AES192";
                case CipherAlgorithm.Aes256: return "AES256";
                case CipherAlgorithm.TwoFish: return "TWOFISH";
                case CipherAlgorithm.Camellia128: return "CAMELLIA128";
                case CipherAlgorithm.Camellia192: return "CAMELLIA192";
                case CipherAlgorithm.Camellia256: return "CAMELLIA256";
                default: return String.Empty;
            }
        }

        public static DateTime ToDate(String s)
        {
            if (String.IsNullOrEmpty(s))
                return GpgDateTime.Unlimited.DateTime;

            UInt32 i;
            if (!UInt32.TryParse(s, out i))
                throw new GpgApiException("Invalid date time: " + s);

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(i);
        }

        public static Int32 ToDays(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return (date - DateTime.Today).Days;
        }
    }
}
