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
    public sealed class GpgInterfaceResult
    {
        public GpgInterfaceStatus Status { get; private set; }
        public GpgInterfaceMessage Message { get; private set; }
        public Object Data { get; private set; }

        internal static readonly GpgInterfaceResult Started = new GpgInterfaceResult(GpgInterfaceStatus.Started);
        internal static readonly GpgInterfaceResult Success = new GpgInterfaceResult(GpgInterfaceStatus.Success);
        internal static readonly GpgInterfaceResult UserAbort = new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.Aborted);
        internal static readonly GpgInterfaceResult BadPassphrase = new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.BadPassphrase);

        internal GpgInterfaceResult(GpgInterfaceStatus status, GpgInterfaceMessage message = GpgInterfaceMessage.None, Object data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
