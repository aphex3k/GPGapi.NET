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
    public sealed class AskPassphraseInfo
    {
        public String UserId { get; private set; }
        public Boolean IsNewPassphrase { get; private set; }
        public Boolean IsSymmetric { get; private set; }
        public Int32 Tries { get; private set; }

        internal AskPassphraseInfo(String userId, Boolean isNewPassphrase, Boolean isSymmetric, Int32 tries)
        {
            UserId = userId;
            IsNewPassphrase = isNewPassphrase;
            IsSymmetric = isSymmetric;
            Tries = tries;
        }
    }
}
