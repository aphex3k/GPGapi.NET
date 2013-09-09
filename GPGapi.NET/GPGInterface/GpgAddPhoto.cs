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
using System.IO;
using System.Security;

namespace GpgApi
{
    /// <summary>
    /// Adds a photo to the specified key.
    /// </summary>
    /// <remarks>
    /// The format of the photo must be Jpeg (because GPG accepts only jpeg).
    /// GPG can't download remote files (http://..., ftp://..., etc.); so the path of the file must be a local file.
    /// If the photo is too big (in KB), this action method will NOT ask for a confirmation.
    /// So you can resize the image before adding it to the key.
    /// <br/><br/>
    /// Here is the list of <see cref="GpgApi.GpgInterfaceMessage"/> used by this class.
    /// <list type="bullet">
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.FileNotFound"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.InvalidImageFormat"/></term></item>
    /// </list>
    /// </remarks>
    public sealed class GpgAddPhoto : GpgInterface
    {
        public KeyId KeyId { get; private set; }
        public String PhotoPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgAddPhoto"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="photoPath"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgAddPhoto(KeyId keyId, String photoPath)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            if (photoPath == null)
                throw new ArgumentNullException("photoPath");

            KeyId = keyId;
            PhotoPath = photoPath;
        }

        private Boolean _alreadyAsked1 = false;
        private Boolean _alreadyAsked2 = false;

        // internal AND protected
        internal override String Arguments()
        {
            return "--edit-key " + KeyId + " addphoto save";
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(PhotoPath))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound);

//            if (!Utils.IsJpegImage(PhotoPath))
//                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidImageFormat);

            return GpgInterfaceResult.Success;
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
                        case "keyedit.save.okay": WriteLine("YES"); break;
                        case "keyedit.prompt": WriteLine("save"); break;
                        case "photoid.jpeg.add":
                        {
                            if (_alreadyAsked1)
                                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidImageFormat);

                            _alreadyAsked1 = true;
                            WriteLine(PhotoPath);
                            break;
                        }
                    }

                    break;
                }

                case GpgKeyword.GET_BOOL:
                {
                    if (String.Equals(line, "photoid.jpeg.size", StringComparison.Ordinal))
                    {
                        if (_alreadyAsked2)
                            return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidImageFormat);

                        _alreadyAsked2 = true;
                        WriteLine("YES");
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
