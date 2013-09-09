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
using System.Text.RegularExpressions;

namespace GpgApi
{
    /// <summary>
    /// Represents a fingerprint.
    /// This class is immutable.
    /// </summary>
    public sealed class FingerPrint : IEquatable<FingerPrint>
    {
        private static readonly Regex _regex = new Regex(@"^[A-Za-z0-9]+$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether a fingerprint is valid or not.
        /// </summary>
        /// <param name="fingerPrint">The fingerprint to check.</param>
        /// <returns>true if the fingerprint is valid; otherwise false.</returns>
        public static Boolean IsValid(String fingerPrint)
        {
            return !String.IsNullOrEmpty(fingerPrint) && _regex.IsMatch(fingerPrint);
        }

        private readonly String _fingerPrint;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.FingerPrint"/> class to the specified fingerprint.
        /// </summary>
        /// <param name="fingerPrint">A fingerprint</param>
        /// <exception cref="InvalidFingerPrintException"><paramref name="fingerPrint"/> is not a valid fingerprint.</exception>
        public FingerPrint(String fingerPrint)
        {
            if (!FingerPrint.IsValid(fingerPrint))
                throw new InvalidFingerPrintException();
            _fingerPrint = fingerPrint.ToUpperInvariant();
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current fingerprint as a string.</returns>
        public override String ToString()
        {
            return _fingerPrint;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="GpgApi.FingerPrint"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override Int32 GetHashCode()
        {
            return _fingerPrint.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> equals the current <see cref="GpgApi.FingerPrint"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="GpgApi.FingerPrint"/>.</param>
        /// <returns>true if the specified Object equals the current FingerPrint; otherwise false.</returns>
        public override Boolean Equals(Object obj)
        {
            return FingerPrint.Equals(this, obj as FingerPrint);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GpgApi.FingerPrint"/> equals the current <see cref="GpgApi.FingerPrint"/>.
        /// </summary>
        /// <param name="other">The fingerprint to compare with the current <see cref="GpgApi.FingerPrint"/>.</param>
        /// <returns>true if the specified FingerPrint equals the current FingerPrint; otherwise false.</returns>
        public Boolean Equals(FingerPrint other)
        {
            return FingerPrint.Equals(this, other);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.FingerPrint"/>s are equals.
        /// </summary>
        /// <param name="fingerPrint1">The first FingerPrint to compare</param>
        /// <param name="fingerPrint2">The second FingerPrint to compare</param>
        /// <returns>true if the specified FingerPrints are equals; otherwise false.</returns>
        public static Boolean Equals(FingerPrint fingerPrint1, FingerPrint fingerPrint2)
        {
            if (Object.ReferenceEquals(fingerPrint1, fingerPrint2))
                return true;

            if ((Object)fingerPrint1 == null || (Object)fingerPrint2 == null)
                return false;

            return String.Equals(fingerPrint1._fingerPrint, fingerPrint2._fingerPrint, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.FingerPrint"/>s are equals.
        /// </summary>
        /// <param name="fingerPrint1">The first FingerPrint to compare</param>
        /// <param name="fingerPrint2">The second FingerPrint to compare</param>
        /// <returns>true if the specified FingerPrints are equals; otherwise false.</returns>
        public static Boolean operator ==(FingerPrint fingerPrint1, FingerPrint fingerPrint2)
        {
            return FingerPrint.Equals(fingerPrint1, fingerPrint2);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.FingerPrint"/>s are differents.
        /// </summary>
        /// <param name="fingerPrint1">The first FingerPrint to compare</param>
        /// <param name="fingerPrint2">The second FingerPrint to compare</param>
        /// <returns>true if the specified FingerPrints are differents; otherwise false.</returns>
        public static Boolean operator !=(FingerPrint fingerPrint1, FingerPrint fingerPrint2)
        {
            return !FingerPrint.Equals(fingerPrint1, fingerPrint2);
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current fingerprint as a string.</returns>
        public static implicit operator String(FingerPrint fingerPrint)
        {
            return fingerPrint == null ? null : fingerPrint.ToString();
        }
    }
}
