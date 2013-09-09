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
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace GpgApi
{
    /// <summary>
    /// Represents a key id.
    /// This class is immutable.
    /// </summary>
    public sealed class KeyId : IEquatable<KeyId>
    {
        private static readonly Regex _regex = new Regex(@"^[A-Za-z0-9]+$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether a key id is valid or not.
        /// </summary>
        /// <param name="keyId">The keyd id to check.</param>
        /// <returns>true if the key id is valid; otherwise false.</returns>
        public static Boolean IsValid(String keyId)
        {
            return keyId != null && keyId.Length == 16 && _regex.IsMatch(keyId);
        }

        private readonly String _keyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.KeyId"/> class to the specified keyid.
        /// </summary>
        /// <param name="keyId">A key id.</param>
        /// <exception cref="InvalidKeyIdException"><paramref name="keyId"/> is not a valid key id.</exception>
        public KeyId(String keyId)
        {
            if (!KeyId.IsValid(keyId))
                throw new InvalidKeyIdException();
            _keyId = keyId.ToUpperInvariant();
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current key id as a string.</returns>
        public override String ToString()
        {
            return _keyId;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="GpgApi.KeyId"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override Int32 GetHashCode()
        {
            return _keyId.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> equals the current <see cref="GpgApi.KeyId"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="GpgApi.KeyId"/>.</param>
        /// <returns>true if the specified Object equals the current KeyId; otherwise false.</returns>
        public override Boolean Equals(Object obj)
        {
            return KeyId.Equals(this, obj as KeyId);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GpgApi.KeyId"/> equals the current <see cref="GpgApi.KeyId"/>.
        /// </summary>
        /// <param name="other">The KeyId to compare with the current <see cref="GpgApi.KeyId"/>.</param>
        /// <returns>true if the specified KeyId equals the current KeyId; otherwise false.</returns>
        public Boolean Equals(KeyId other)
        {
            return KeyId.Equals(this, other);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.KeyId"/>s are equals.
        /// </summary>
        /// <param name="keyId1">The first KeyId to compare</param>
        /// <param name="keyId2">The second KeyId to compare</param>
        /// <returns>true if the specified KeyIds are equals; otherwise false.</returns>
        public static Boolean Equals(KeyId keyId1, KeyId keyId2)
        {
            if (Object.ReferenceEquals(keyId1, keyId2))
                return true;

            if ((Object)keyId1 == null || (Object)keyId2 == null)
                return false;

            return String.Equals(keyId1._keyId, keyId2._keyId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.KeyId"/>s are equals.
        /// </summary>
        /// <param name="keyId1">The first KeyId to compare</param>
        /// <param name="keyId2">The second KeyId to compare</param>
        /// <returns>true if the specified KeyIds are equals; otherwise false.</returns>
        public static Boolean operator ==(KeyId keyId1, KeyId keyId2)
        {
            return KeyId.Equals(keyId1, keyId2);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.KeyId"/>s are differents.
        /// </summary>
        /// <param name="keyId1">The first KeyId to compare</param>
        /// <param name="keyId2">The second KeyId to compare</param>
        /// <returns>true if the specified KeyIds are differents; otherwise false.</returns>
        public static Boolean operator !=(KeyId keyId1, KeyId keyId2)
        {
            return !KeyId.Equals(keyId1, keyId2);
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current key id as a string.</returns>
        public static implicit operator String(KeyId keyId)
        {
            return keyId == null ? null : keyId.ToString();
        }
    }
}
