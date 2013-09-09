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
using System.Text;

namespace GpgApi
{
    /// <summary>
    /// Exports a key to a string.
    /// </summary>
    /// <remarks>
    /// The exported key is "armored", so you can copy/past the export to an email for example.
    /// </remarks>
    public sealed class GpgExportKey : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public Boolean PrivateKey { get; private set; }

        public String ExportedKey
        {
            get { return exported_key.Length == 0 ? null : exported_key.ToString(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgExportKey"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="privateKey"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgExportKey(KeyId keyId, Boolean privateKey)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            KeyId = keyId;
            PrivateKey = privateKey;
        }

        private StringBuilder exported_key = new StringBuilder();

        // internal AND protected
        internal override String Arguments()
        {
            String export = PrivateKey ? "--export-secret-keys " : "--export ";
            return "--armor " + export + KeyId;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            exported_key.AppendLine(line);
            return GpgInterfaceResult.Success;
        }
    }
}
