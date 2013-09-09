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
    /// Gets the version of GPG.
    /// </summary>
    public sealed class GpgVersion : GpgInterface
    {
        public Version Version { get; private set; }

        public GpgVersion()
        {
            Version = null;
        }

        // internal AND protected
        internal override String Arguments()
        {
            return "--version";
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (Version == null)
            {
                Int32 pos = line.LastIndexOf(' ');
                if (pos != -1)
                    Version = new Version(line.Substring(pos + 1));
            }

            return GpgInterfaceResult.Success;
        }
    }
}
