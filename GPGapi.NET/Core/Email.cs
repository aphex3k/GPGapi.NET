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
    /// Represents a valid email address.
    /// This class is immutable.
    /// </summary>
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex _regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether an email address is valid or not.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>true if the email address is valid; otherwise false.</returns>
        public static Boolean IsValid(String email)
        {
            return !String.IsNullOrEmpty(email) && _regex.IsMatch(email);
        }

        private readonly String _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.Email"/> class to the specified email.
        /// </summary>
        /// <param name="email">An email address.</param>
        /// <exception cref="InvalidEmailAddressException"><paramref name="email"/> is not a valid email address.</exception>
        public Email(String email)
        {
            if (!Email.IsValid(email))
                throw new InvalidEmailAddressException();
            _email = email;
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current email address as a string.</returns>
        public override String ToString()
        {
            return _email;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="GpgApi.Email"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override Int32 GetHashCode()
        {
            return _email.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> equals the current <see cref="GpgApi.Email"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="GpgApi.Email"/>.</param>
        /// <returns>true if the specified Object equals the current Email; otherwise false.</returns>
        public override Boolean Equals(Object obj)
        {
            return Email.Equals(this, obj as Email);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GpgApi.Email"/> equals the current <see cref="GpgApi.Email"/>.
        /// </summary>
        /// <param name="other">The Email to compare with the current <see cref="GpgApi.Email"/>.</param>
        /// <returns>true if the specified Email equals the current Email; otherwise false.</returns>
        public Boolean Equals(Email other)
        {
            return Email.Equals(this, other);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Email"/>s are equals.
        /// </summary>
        /// <param name="email1">The first Email to compare</param>
        /// <param name="email2">The second Email to compare</param>
        /// <returns>true if the specified Emails are equals; otherwise false.</returns>
        public static Boolean Equals(Email email1, Email email2)
        {
            if (Object.ReferenceEquals(email1, email2))
                return true;

            if ((Object)email1 == null || (Object)email2 == null)
                return false;

            return String.Equals(email1._email, email2._email);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Email"/>s are equals.
        /// </summary>
        /// <param name="email1">The first Email to compare</param>
        /// <param name="email2">The second Email to compare</param>
        /// <returns>true if the specified Emails are equals; otherwise false.</returns>
        public static Boolean operator ==(Email email1, Email email2)
        {
            return Email.Equals(email1, email2);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.Email"/>s are differents.
        /// </summary>
        /// <param name="email1">The first Email to compare</param>
        /// <param name="email2">The second Email to compare</param>
        /// <returns>true if the specified Emails are differents; otherwise false.</returns>
        public static Boolean operator !=(Email email1, Email email2)
        {
            return !Email.Equals(email1, email2);
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current email address as a string.</returns>
        public static implicit operator String(Email email)
        {
            return email == null ? null : email.ToString();
        }
    }
}
