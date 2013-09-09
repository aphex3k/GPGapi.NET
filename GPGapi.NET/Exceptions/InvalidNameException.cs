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
using System.Runtime.Serialization;

namespace GpgApi
{
    /// <summary>
    /// The exception is thrown when a new instance of <see cref="GpgApi.Name"/> is initialized with an invalid name.
    /// </summary>
    /// <remarks>
    /// To avoid this exception you can determine whether the name you want to use is valid with the static method <see cref="GpgApi.Name.IsValid"/>.
    /// </remarks>
    [Serializable]
    public class InvalidNameException : GpgApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.InvalidNameException"/> class.
        /// </summary>
        public InvalidNameException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.InvalidNameException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public InvalidNameException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.InvalidNameException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data. </param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected InvalidNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.InvalidNameException"/> class with a specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidNameException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
