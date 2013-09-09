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
    /// <summary>
    /// Specifies the algorithm to be used to encrypt data.
    /// Those algorithms are symmetrics.
    /// </summary>
    /// <remarks>
    /// IDEA is not implemented in GPG because this algorithm is patented; more information here http://www.gnupg.org/faq/why-not-idea.en.html
    /// </remarks>
    public enum CipherAlgorithm
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,

        /// <summary>
        /// Encrypt using the 3DES algorithm.
        /// </summary>
        ThreeDes,

        /// <summary>
        /// Encrypt using the Cast 5 (also named "cast 128") algorithm.
        /// </summary>
        Cast5,

        /// <summary>
        /// Encrypt using the Blowfish algorithm.
        /// </summary>
        BlowFish,

        /// <summary>
        /// Encrypt using the AES algorithm.
        /// </summary>
        Aes,

        /// <summary>
        /// Encrypt using the AES-192 algorithm.
        /// </summary>
        Aes192,

        /// <summary>
        /// Encrypt using the AES-256 algorithm.
        /// </summary>
        Aes256,

        /// <summary>
        /// Encrypt using the TwoFish algorithm.
        /// </summary>
        TwoFish,

        /// <summary>
        /// Encrypt using the Camellia-128 algorithm.
        /// </summary>
        Camellia128,

        /// <summary>
        /// Encrypt using the Camellia-192 algorithm.
        /// </summary>
        Camellia192,

        /// <summary>
        /// Encrypt using the Camellia-256 algorithm.
        /// </summary>
        Camellia256,
    }

    /// <summary>
    /// Specifies the algorithm to be used to hash data (when signing).
    /// </summary>
    public enum DigestAlgorithm
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,

        /// <summary>
        /// Hash using the MD5 algorithm.
        /// </summary>
        MD5,

        /// <summary>
        /// Hash using the SHA1 algorithm.
        /// </summary>
        Sha1,

        /// <summary>
        /// Hash using the RIPEMD-160 algorithm.
        /// </summary>
        Rmd160,

        /// <summary>
        /// Hash using the SHA-256 algorithm.
        /// </summary>
        Sha256,

        /// <summary>
        /// Hash using the SHA-384 algorithm.
        /// </summary>
        Sha384,

        /// <summary>
        /// Hash using the SHA-512 algorithm.
        /// </summary>
        Sha512,

        /// <summary>
        /// Hash using the SHA-224 algorithm.
        /// </summary>
        Sha224
    }

    /// <summary>
    /// Specifies the algorithm to be used to encrypt and/or sign data.
    /// Those algorithms are asymmetrics.
    /// </summary>
    public enum KeyAlgorithm
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,

        /// <summary>
        /// Encrypt and sign data using the RSA algorithm.
        /// </summary>
        RsaRsa,

        /// <summary>
        /// Encrypt data using the RSA algorithm.
        /// </summary>
        RsaEncrypt,

        /// <summary>
        /// Sign data using the RSA algorithm.
        /// </summary>
        RsaSign,

        /// <summary>
        /// Sign data using the DSA algorithm.
        /// </summary>
        Dsa,

        /// <summary>
        /// Encrypt data using the ElGamal algorithm.
        /// </summary>
        ELGamal,

        /// <summary>
        /// Encrypt and sign data using the DSA/ElGamal algorithm.
        /// </summary>
        DsaELGamal
    }

    /// <summary>
    /// Specifies the compression algorithm to be used by GPG when compressing the data.
    /// </summary>
    public enum CompressionAlgorithm
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,

        /// <summary>
        /// Zip algorithm.
        /// </summary>
        Zip,

        /// <summary>
        /// ZLib algorithm.
        /// </summary>
        ZLib,

        /// <summary>
        /// BZip2 algorithm.
        /// </summary>
        BZip2
    }

    /// <summary>
    /// Specifies the trust defined by the user on a public key.
    /// </summary>
    public enum KeyOwnerTrust
    {
        /// <summary>
        /// The user do not trust the key.
        /// </summary>
        None,

        /// <summary>
        /// The user has a marginal trusta on the key.
        /// </summary>
        Marginal,

        /// <summary>
        /// The user trusts the key.
        /// </summary>
        Full,

        /// <summary>
        /// The user trusts the key; this is the maximum trust you can have on a key. This is the default value for the keys created by the user himself.
        /// </summary>
        Ultimate
    }

    public enum KeyTrust
    {
        Unknown,
        Minimum,
        Invalid,
        Revoked,
        Expired,
        Undefined,
        None,
        Marginal,
        Full,
        Ultimate,
        NoKey
    }

    [Flags]
    public enum KeyType
    {
        None = 0,
        Public = 1,
        Secret = 2,
        Pair = Public | Secret
    }

    public enum DataType
    {
        None,
        Binary,
        Text,
        Utf8Text,
    }

    [Flags]
    public enum Import
    {
        None = 0,
        Unchanged = 1,
        NewKey = 2,
        NewUserIds = 4,
        NewSignatures = 8,
        NewSubKeys = 16,
        ContainsPrivateKey = 32
    }
}
