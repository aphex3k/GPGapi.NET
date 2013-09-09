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
    /// Adds a new user info to a key.
    /// </summary>
    public sealed class GpgAddUserInfo : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public String Comment { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgAddUserInfo"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="comment"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgAddUserInfo(KeyId keyId, Name name, Email email, String comment)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            if (name == null)
                throw new ArgumentNullException("name");

            if (email == null)
                throw new ArgumentNullException("email");

            KeyId = keyId;
            Name = name;
            Email = email;
            Comment = comment;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--edit-key " + KeyId + " adduid save";
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
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

                case GpgKeyword.GET_LINE:
                {
                    switch (line)
                    {
                        case "keygen.name": WriteLine(EncodeString(Name)); break;
                        case "keygen.email": WriteLine(EncodeString(Email)); break;
                        case "keygen.comment": WriteLine(EncodeString(Comment)); break;
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
