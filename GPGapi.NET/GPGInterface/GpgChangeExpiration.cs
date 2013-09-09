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
using System.Security;

namespace GpgApi
{
    /// <summary>
    /// Changes the expiration date of a key.
    /// TODO, tester cette méthode et voir si donner une date dans le passé est autorisé.
    /// </summary>
    public sealed class GpgChangeExpiration : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgChangeExpiration"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="expirationDate"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgChangeExpiration(KeyId keyId, DateTime expirationDate)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            KeyId = keyId;
            ExpirationDate = expirationDate;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--edit-key " + KeyId + " expire save";
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.GET_LINE:
                {
                    if (String.Equals(line, "keygen.valid", StringComparison.Ordinal))
                        WriteLine(GpgConvert.ToDays(ExpirationDate));
                    break;
                }

                case GpgKeyword.GET_HIDDEN:
                {
                    if (String.Equals(line, "passphrase.enter", StringComparison.Ordinal))
                    {
                        SecureString password = InternalAskPassphrase(KeyId);
                        if (IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WritePassword(password);
                    }
                    break;
                }

                case GpgKeyword.BAD_PASSPHRASE:
                {
                    if (IsMaxTries())
                        return GpgInterfaceResult.BadPassphrase;
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
