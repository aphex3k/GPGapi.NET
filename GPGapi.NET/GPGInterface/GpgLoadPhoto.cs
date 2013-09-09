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
using System.Drawing;
using System.IO;

namespace GpgApi
{
    /// <summary>
    /// Retrieves a photo associated to the key.
    /// If the photo cannot be found <see cref="GpgLoadPhoto.Image"/> will be null.
    /// </summary>
    public sealed class GpgLoadPhoto : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public UInt32 Index { get; private set; }
        public System.Net.Mime.MediaTypeNames.Image Image { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgLoadPhoto"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="index"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgLoadPhoto(KeyId keyId, UInt32 index)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            KeyId = keyId;
            Index = index;
            Image = null;
        }

        // internal AND protected
        internal override String Arguments()
        {
            const String output = "\"cmd /C echo %I\"";
            return "--photo-viewer " + output + " --edit-key " + KeyId + " uid " + Index + " showphoto quit";
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (String.IsNullOrEmpty(line) || GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            try
            {
                using (FileStream stream = new FileStream(line, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    Image = Image.FromStream(stream, false, true);
                }
            }
            catch(Exception ex)
            {
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.None, ex);
            }

            try
            {
                File.Delete(line);
            }
            catch
            {
                // Do nothing
            }

            return GpgInterfaceResult.Success;
        }
    }
}
