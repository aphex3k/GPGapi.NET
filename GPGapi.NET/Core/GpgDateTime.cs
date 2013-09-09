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
    /// Represents a gpg datetime.
    /// This class is immutable.
    /// 
    /// Every public keys has an expiration date.
    /// This date can be either a datetime or "unlimited".
    /// Unlimited means that the key will never expire.
    /// </summary>
    public sealed class GpgDateTime : IEquatable<GpgDateTime>
    {
        /// <summary>
        /// Represents a "unlimited" datetime.
        /// </summary>
        public static readonly GpgDateTime Unlimited = new GpgDateTime(null);

        /// <summary>
        /// Gets the DateTime component of this instance.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the boolean that indicates whether the datetime represented by this instance is "unlimited".
        /// </summary>
        public Boolean IsUnlimited { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgDateTime"/> class.
        /// By default, the new instance is an Unlimited datetime.
        /// </summary>
        public GpgDateTime()
        {
            DateTime = GpgDateTime.Unlimited.DateTime;
            IsUnlimited = GpgDateTime.Unlimited.IsUnlimited;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgDateTime"/> class to the specified datetime.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        public GpgDateTime(DateTime datetime)
        {
            DateTime = datetime;
            IsUnlimited = DateTime == DateTime.MaxValue;
        }

        internal GpgDateTime(String datetime)
        {
            if (String.IsNullOrEmpty(datetime))
            {
                DateTime = DateTime.MaxValue;
                IsUnlimited = true;
            }
            else
            {
                UInt32 i;
                if (!UInt32.TryParse(datetime, out i))
                    throw new GpgApiException("Invalid date time: " + datetime);

                DateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(i);
                IsUnlimited = DateTime == DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The current datetime as a string.</returns>
        public override String ToString()
        {
            return DateTime.ToString();
        }

        /// <summary>
        /// Returns the hash code for this <see cref="GpgApi.GpgDateTime"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override Int32 GetHashCode()
        {
            return DateTime.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> equals the current <see cref="GpgApi.GpgDateTime"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="GpgApi.GpgDateTime"/>.</param>
        /// <returns>true if the specified Object equals the current GpgDateTime; otherwise false.</returns>
        public override Boolean Equals(Object obj)
        {
            return GpgDateTime.Equals(this, obj as GpgDateTime);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GpgApi.GpgDateTime"/> equals the current <see cref="GpgApi.GpgDateTime"/>.
        /// </summary>
        /// <param name="other">The GpgDateTime to compare with the current <see cref="GpgApi.GpgDateTime"/>.</param>
        /// <returns>true if the specified GpgDateTime equals the current GpgDateTime; otherwise false.</returns>
        public Boolean Equals(GpgDateTime other)
        {
            return GpgDateTime.Equals(this, other);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.GpgDateTime"/>s are equals.
        /// </summary>
        /// <param name="gpgDateTime1">The first GpgDateTime to compare</param>
        /// <param name="gpgDateTime2">The second GpgDateTime to compare</param>
        /// <returns>true if the specified GpgDateTimes are equals; otherwise false.</returns>
        public static Boolean Equals(GpgDateTime gpgDateTime1, GpgDateTime gpgDateTime2)
        {
            if (Object.ReferenceEquals(gpgDateTime1, gpgDateTime2))
                return true;

            if ((Object)gpgDateTime1 == null || (Object)gpgDateTime2 == null)
                return false;

            return DateTime.Equals(gpgDateTime1.DateTime, gpgDateTime2.DateTime);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.GpgDateTime"/>s are equals.
        /// </summary>
        /// <param name="gpgDateTime1">The first GpgDateTime to compare</param>
        /// <param name="gpgDateTime2">The second GpgDateTime to compare</param>
        /// <returns>true if the specified GpgDateTimes are equals; otherwise false.</returns>
        public static Boolean operator ==(GpgDateTime gpgDateTime1, GpgDateTime gpgDateTime2)
        {
            return GpgDateTime.Equals(gpgDateTime1, gpgDateTime2);
        }

        /// <summary>
        /// Determines whether two <see cref="GpgApi.GpgDateTime"/>s are differents.
        /// </summary>
        /// <param name="gpgDateTime1">The first GpgDateTime to compare</param>
        /// <param name="gpgDateTime2">The second GpgDateTime to compare</param>
        /// <returns>true if the specified GpgDateTimes are differents; otherwise false.</returns>
        public static Boolean operator !=(GpgDateTime gpgDateTime1, GpgDateTime gpgDateTime2)
        {
            return !GpgDateTime.Equals(gpgDateTime1, gpgDateTime2);
        }
    }
}
