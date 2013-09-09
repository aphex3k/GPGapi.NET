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
    /// Can be used to delete either a photo or a userinfo.
    /// </summary>
    public class GpgDeleteUserId : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public UInt32 Index { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgDeleteUserId"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="index"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgDeleteUserId(KeyId keyId, UInt32 index)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            KeyId = keyId;
            Index = index;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--edit-key " + KeyId + " uid " + Index + " deluid save";
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.GET_BOOL:
                {
                    if (String.Equals(line, "keyedit.remove.uid.okay", StringComparison.Ordinal))
                        WriteLine("YES");
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
