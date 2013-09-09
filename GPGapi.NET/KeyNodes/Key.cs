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

namespace GpgApi
{
    public sealed class Key
    {
        /// <summary>
        /// Id of the key.
        /// </summary>
        public KeyId Id { get; set; }

        /// <summary>
        /// FingerPrint of the key.
        /// </summary>
        public FingerPrint FingerPrint { get; set; }

        /// <summary>
        /// Type of the key.
        /// </summary>
        public KeyType Type { get; set; }

        /// <summary>
        /// Indicates whether the key is disabled or not.
        /// </summary>
        public Boolean IsDisabled { get; set; }

        /// <summary>
        /// Size of the key.
        /// </summary>
        public UInt32 Size { get; set; }

        /// <summary>
        /// Owner trust of the key.
        /// </summary>
        public KeyOwnerTrust OwnerTrust { get; set; }

        /// <summary>
        /// Trust of the key.
        /// </summary>
        public KeyTrust Trust { get; set; }

        /// <summary>
        /// Creation date of the key.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Expiration date of the key.
        /// The expiration date can also be <see cref="GpgApi.GpgDateTime.IsUnlimited">unlimited</see>.
        /// </summary>
        public GpgDateTime ExpirationDate { get; set; }

        /// <summary>
        /// Encryption algorithm used by this key.
        /// </summary>
        public KeyAlgorithm Algorithm { get; set; }

        /// <summary>
        /// List of user info associated to this key.
        /// </summary>
        public IList<KeyUserInfo> UserInfos { get; private set; }

        /// <summary>
        /// List of photos associated to this key.
        /// </summary>
        public IList<KeyPhoto> Photos { get; private set; }

        /// <summary>
        /// List of subkeys associated to this key.
        /// </summary>
        public IList<KeySub> SubKeys { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.Key"/> class.
        /// </summary>
        public Key()
        {
            Id = null;
            FingerPrint = null;
            Type = KeyType.None;
            IsDisabled = false;
            Size = 0;
            OwnerTrust = KeyOwnerTrust.None;
            Trust = KeyTrust.Unknown;
            CreationDate = DateTime.MinValue;
            ExpirationDate = GpgDateTime.Unlimited;
            Algorithm = KeyAlgorithm.None;

            UserInfos = new List<KeyUserInfo>();
            Photos = new List<KeyPhoto>();
            SubKeys = new List<KeySub>();
        }
    }
}
