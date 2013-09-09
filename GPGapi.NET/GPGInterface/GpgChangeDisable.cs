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
    /// Changes the status of a key from enable to disable and vice versa.
    /// </summary>
    public sealed class GpgChangeDisable : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public Boolean Disable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgChangeDisable"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="disable"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgChangeDisable(KeyId keyId, Boolean disable)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            KeyId = keyId;
            Disable = disable;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--edit-key " + KeyId + (Disable ? " disable" : " enable") + " save";
        }
    }
}
