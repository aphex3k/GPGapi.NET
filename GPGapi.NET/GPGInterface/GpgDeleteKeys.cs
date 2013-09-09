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
    /// <summary>
    /// Removes one or multiple keys from the secret and public keyring.
    /// </summary>
    /// <remarks>
    /// Be carefull, this method will execute without confirmation.
    /// </remarks>
    public sealed class GpgDeleteKeys : GpgInterface
    {
        public IEnumerable<KeyId> KeyIds { get; private set; }
        public Boolean SecretOnly { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgDeleteKeys"/> class.
        /// </summary>
        /// <param name="keyIds"></param>
        /// <param name="secretOnly"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgDeleteKeys(IEnumerable<KeyId> keyIds, Boolean secretOnly)
        {
            if (keyIds == null)
                throw new ArgumentNullException("keyIds");

            KeyIds = keyIds;
            SecretOnly = secretOnly;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return (SecretOnly ? "--delete-secret-key " : "--delete-secret-and-public-key ") + String.Join(" ", KeyIds);
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
                    switch (line)
                    {
                        case "delete_key.secret.okay":
                        case "delete_key.okay":
                            WriteLine("YES");
                            break;
                    }

                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
