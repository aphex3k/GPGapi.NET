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

namespace GpgApi
{
    public enum GpgInterfaceStatus
    {
        /// <summary>
        /// Process started
        /// </summary>
        Started,

        /// <summary>
        /// Process is currently processing, see <see cref="GpgApi.GpgInterfaceMessage"/> for more information
        /// </summary>
        Processing,

        Success,
        Error
    }

    public enum GpgInterfaceMessage
    {
        /// <summary>
        /// This is the default value.
        /// </summary>
        None,

        /// <summary>
        /// The user aborted the operation by using the method <see cref="GpgApi.GpgInterface.Abort"/>.
        /// </summary>
        Aborted,

        /// <summary>
        /// The user did not enter a good passphrase.
        /// </summary>
        BadPassphrase,

        /// <summary>
        /// The specified file was not found.
        /// </summary>
        FileNotFound,

        /// <summary>
        /// The file's path is not valid.
        /// </summary>
        InvalidFileName,

        /// <summary>
        /// The image is not a jpeg image (the only format supported by GPG)
        /// </summary>
        InvalidImageFormat,

        /// <summary>
        /// The decryption started.
        /// </summary>
        BeginDecryption,

        /// <summary>
        /// The decryption finished
        /// </summary>
        EndDecryption,

        DecryptionOk,               // GpgDecrypt
        DecryptionFailed,           // GpgDecrypt
        NoSecretKey,                // GpgDecrypt
        NoPublicKey,                // GpgDecrypt
        DataError,                  // DataError, GpgImportKey

        BeginSigning,               // GpgEncrypt
        SignatureCreated,           // GpgEncrypt
        BeginEncryption,            // GpgEncrypt
        EndEncryption,              // GpgEncrypt
        SignatureKeyExpired,        // GpgEncrypt
        InvalidRecipient,           // GpgEncrypt

        /// <summary>
        /// The generate key method is generating prime numbers
        /// </summary>
        GeneratingPrimeNumbers,

        /// <summary>
        /// The generate key method is generating a DSA key
        /// </summary>
        GeneratingDsaKey,

        /// <summary>
        /// The generate key method is generating a ElGamal key
        /// </summary>
        GeneratingELGamalKey,

        /// <summary>
        /// The generate key method is waiting for entropy
        /// </summary>
        NeedEntropy,

        /// <summary>
        /// The size of the key is too small
        /// </summary>
        SizeTooSmall,

        /// <summary>
        /// The size of the key is too big
        /// </summary>
        SizeTooBig,

        /// <summary>
        /// The key has not been created because of an error
        /// </summary>
        KeyNotCreated
    }
}
