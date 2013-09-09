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
using System.Globalization;
using System.IO;

namespace GpgApi
{
    /// <summary>
    /// Imports a key from either a file or from a keyserver.
    /// If a filename is specified then the key will be imported from that file;
    /// otherwise it is imported from the keyserver (the keyid must be specified in that case).
    /// </summary>
    /// <remarks>
    /// Here is the list of <see cref="GpgApi.GpgInterfaceMessage"/> used by this class.
    /// <list type="bullet">
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.FileNotFound"/></term></item>
    ///     <item><term><see cref="GpgApi.GpgInterfaceMessage.DataError"/></term></item>
    /// </list>
    /// </remarks>
    public class GpgImportKey : GpgInterface
    {
        public String FileName { get; private set; }
        public KeyId KeyId { get; private set; }
        public IEnumerable<Uri> Servers { get; private set; }
        public FingerPrint FingerPrint { get; private set; }
        public Import Import { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgImportKey"/> class.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="servers"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgImportKey(KeyId keyId, IEnumerable<Uri> servers)
        {
            if (keyId == null)
                throw new ArgumentNullException("keyId");

            if (servers == null)
                throw new ArgumentNullException("servers");

            FileName = null;
            KeyId = keyId;
            Servers = servers;
            FingerPrint = null;
            Import = Import.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgImportKey"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        public GpgImportKey(String fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;
            KeyId = null;
            Servers = null;
            FingerPrint = null;
            Import = Import.None;
        }

        // internal AND protected
        internal override String Arguments()
        {
            if (FileName != null)
                return "--import " + Utils.EscapePath(FileName);

            return "--keyserver " + String.Join(",", Servers) + " --recv-keys " + KeyId;
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(FileName))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound, FileName);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.NODATA:
                    return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.DataError);

                case GpgKeyword.IMPORT_OK:
                {
                    String[] parts = line.Split(new Char[] { ' ' });

                    Int32 flag = Int32.Parse(parts[0], CultureInfo.InvariantCulture);
                    FingerPrint = new FingerPrint(parts[1]);

                    if (flag == 0)
                        Import = Import.Unchanged;
                    if ((flag & 1) == 1)
                        Import |= Import.NewKey;
                    if ((flag & 2) == 2)
                        Import |= Import.NewUserIds;
                    if ((flag & 4) == 4)
                        Import |= Import.NewSignatures;
                    if ((flag & 8) == 8)
                        Import |= Import.NewSubKeys;
                    if ((flag & 16) == 16)
                        Import |= Import.ContainsPrivateKey;

                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
