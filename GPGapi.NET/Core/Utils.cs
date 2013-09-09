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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace GpgApi
{
    internal static class Utils
    {
        /// <summary>
        /// Kills a process and all processes started by this process (complete subtree).
        /// </summary>
        /// <param name="processId">The PID of the process to kill.</param>
        public static void KillByProcessId(Int32 processId)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "taskkill.exe",
                Arguments = String.Concat("/F /T /PID ", processId.ToString(CultureInfo.InvariantCulture)),
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
        }

        public static Boolean IsValidPath(String path)
        {
            try
            {
                new FileInfo(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Escapes a path before using it as argument in a command line.
        /// </summary>
        /// <param name="path">The path to escape.</param>
        /// <returns>The escaped path.</returns>
        public static String EscapePath(String path)
        {
            return "\"" + path + "\"";
        }

        public static String UnescapeGpgString(String s)
        {
            s = s.Replace("\\x3a", ":");
            s = s.Replace("\\x5c", "\\");
            return s;
        }

//        /// <summary>
//        /// Checks whether an image is a jpeg image or not.
//        /// </summary>
//        /// <param name="filename">The name of the image to checks.</param>
//        /// <returns>True if the image is jpeg; otherwise it returns false.</returns>
//        public static Boolean IsJpegImage(String filename)
//        {
//            try
//            {
//                using (Image image = Image.FromFile(filename))
//                {
//                    return image.RawFormat.Equals(ImageFormat.Jpeg);
//                }
//            }
//            catch (OutOfMemoryException)
//            {
//                // Image.FromFile throws an OutOfMemoryException 
//                // if the file does not have a valid image format or
//                // GDI+ does not support the pixel format of the file.
//                return false;
//            }
//        }

        /// <summary>
        /// Method for converting a UNIX timestamp to a regular
        /// System.DateTime value (and also to the current local time)
        /// </summary>
        /// <param name="timestamp">value to be converted</param>
        /// <returns>converted DateTime in string format</returns>
        public static DateTime ConvertTimestamp(Double timestamp)
        {
            //create a new DateTime value based on the Unix Epoch
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            //add the timestamp to the value
            DateTime newDateTime = converted.AddSeconds(timestamp);

            //return the value in string format
            return newDateTime.ToLocalTime();
        }

        /// <summary>
        /// Gpg uses this format:
        /// "User Name (user comment) &lt;username@username.com&gt;"
        /// This method splits this string and returns each parts separately.
        /// </summary>
        public static void SplitUserInfo(String userInfo, out Name name, out Email email, out String comment)
        {
            name = null;
            email = null;
            comment = null;

            String tmp_name = null;
            String tmp_email = null;
            String tmp_comment = null;

            // ----------------------------------

            if (userInfo.EndsWith(">", StringComparison.Ordinal))
            {
                Int32 pos = userInfo.LastIndexOf('<');
                tmp_email = userInfo.Substring(pos + 1, userInfo.Length - pos - 2);
                userInfo = userInfo.Substring(0, pos).Trim();
            }

            if (userInfo.EndsWith(")", StringComparison.Ordinal))
            {
                Int32 pos = userInfo.LastIndexOf('(');
                tmp_comment = userInfo.Substring(pos + 1, userInfo.Length - pos - 2);
                userInfo = userInfo.Substring(0, pos).Trim();
            }

            tmp_name = userInfo.Trim();

            // ----------------------------------

            if (!Email.IsValid(tmp_email))
            {
                tmp_comment = tmp_email + " " + tmp_comment;
                tmp_email = null;
            }

            if (!Name.IsValid(tmp_name))
            {
                tmp_comment = tmp_name + " " + tmp_comment;
                tmp_name = null;
            }

            // ----------------------------------

            if (tmp_name != null)
                name = new Name(tmp_name);

            if (tmp_email != null)
                email = new Email(tmp_email);

            comment = tmp_comment;
        }
    }
}
