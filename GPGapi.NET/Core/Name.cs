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
    /// Represents a valid name.
    /// This class is immutable.
    /// </summary>
    public sealed class Name : IEquatable<Name>
    {
        private static readonly Regex _regex = new Regex(@"^([^\s<>0-9]+)([^<>]*)[^\s<>]?$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether a name is valid or not.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>true if the name is valid; otherwise false.</returns>
        public static Boolean IsValid(String name)
        {
            return !String.IsNullOrEmpty(name) && _regex.IsMatch(name) && name.Length >= 1 && name.Length <= 255;
        }

        private readonly String _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.Name"/> class to the specified name.
        /// </summary>
        /// <param name="name">A name.</param>
        /// <exception cref="InvalidNameException"><paramref name="name"/> is not a valid name.</exception>
        public Name(String name)
        {
            if (!Name.IsValid(name))
                throw new InvalidNameException();
            _name = name;
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current name as a string.</returns>
        public override String ToString()
        {
            return _name;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="GpgApi.Name"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override Int32 GetHashCode()
        {
            return _name.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> equals the current <see cref="GpgApi.Name"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="GpgApi.Name"/>.</param>
        /// <returns>true if the specified Object equals the current Name; otherwise false.</returns>
        public override Boolean Equals(Object obj)
        {
            return Name.Equals(this, obj as Name);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GpgApi.Name"/> equals the current <see cref="GpgApi.Name"/>.
        /// </summary>
        /// <param name="other">The Name to compare with the current <see cref="GpgApi.Name"/>.</param>
        /// <returns>true if the specified Name equals the current Name; otherwise false.</returns>
        public Boolean Equals(Name other)
        {
            return Name.Equals(this, other);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Name"/>s are equals.
        /// </summary>
        /// <param name="name1">The first Name to compare</param>
        /// <param name="name2">The second Name to compare</param>
        /// <returns>true if the specified Names are equals; otherwise false.</returns>
        public static Boolean Equals(Name name1, Name name2)
        {
            if (Object.ReferenceEquals(name1, name2))
                return true;

            if ((Object)name1 == null || (Object)name2 == null)
                return false;

            return String.Equals(name1._name, name2._name);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Name"/>s are equals.
        /// </summary>
        /// <param name="name1">The first Name to compare</param>
        /// <param name="name2">The second Name to compare</param>
        /// <returns>true if the specified Names are equals; otherwise false.</returns>
        public static Boolean operator ==(Name name1, Name name2)
        {
            return Name.Equals(name1, name2);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Name"/>s are differents.
        /// </summary>
        /// <param name="name1">The first Name to compare</param>
        /// <param name="name2">The second Name to compare</param>
        /// <returns>true if the specified Names are differents; otherwise false.</returns>
        public static Boolean operator !=(Name name1, Name name2)
        {
            return !Name.Equals(name1, name2);
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current name as a string.</returns>
        public static implicit operator String(Name name)
        {
            return name == null ? null : name.ToString();
        }
    }
}
