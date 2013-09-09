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
    public sealed class KeySub : AbstractKeySignable
    {
        public KeyId Id { get; set; }
        public Boolean IsDisabled { get; set; }
        public KeyAlgorithm Algorithm { get; set; }
        public UInt32 Size { get; set; }
        public DateTime CreationDate { get; set; }
        public GpgDateTime ExpirationDate { get; set; }
        public KeyTrust Trust { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.KeySub"/> class.
        /// </summary>
        public KeySub()
        {
            Id = null;
            IsDisabled = false;
            Algorithm = KeyAlgorithm.None;
            Size = 0;
            CreationDate = DateTime.MinValue;
            ExpirationDate = GpgDateTime.Unlimited;
            Trust = KeyTrust.Unknown;
        }
    }
}
