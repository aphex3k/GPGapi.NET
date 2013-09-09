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
    /// This exception is thrown when the method <see cref="GpgApi.GpgInterface.Execute"/> is called more than one time for an instance of <see cref="GpgApi.GpgInterface"/>.
    /// </summary>
    [Serializable]
    public class GpgInterfaceAlreadyUsed : GpgApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgInterfaceAlreadyUsed"/> class.
        /// </summary>
        public GpgInterfaceAlreadyUsed()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgInterfaceAlreadyUsed"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public GpgInterfaceAlreadyUsed(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgInterfaceAlreadyUsed"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data. </param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected GpgInterfaceAlreadyUsed(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgApi.GpgInterfaceAlreadyUsed"/> class with a specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public GpgInterfaceAlreadyUsed(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
