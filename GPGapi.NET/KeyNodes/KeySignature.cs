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
    public sealed class KeySignature
    {
        public KeyId KeyId { get; set; }
        public DateTime SignatureDate { get; set; }
        public Boolean Revoked { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.KeySignature"/> class.
        /// </summary>
        public KeySignature()
        {
            KeyId = null;
            SignatureDate = DateTime.MinValue;
            Revoked = false;
        }

        internal KeySignature(String keyid, DateTime signaturedate, Boolean revoked)
        {
            KeyId = new KeyId(keyid);
            SignatureDate = signaturedate;
            Revoked = revoked;
        }
    }
}
