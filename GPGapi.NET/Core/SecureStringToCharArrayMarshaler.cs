using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace GpgApi
{
    /// http://stackoverflow.com/questions/1800695/c-sharp-securestring-question
    /// Copyright (C) 2010 Douglas Day
    /// All rights reserved.
    /// MIT-licensed: http://www.opensource.org/licenses/mit-license.php

    internal class SecureStringToCharArrayMarshaler : IDisposable
    {
        #region Private Fields

        private Char[] _charArray = null;
        private SecureString _secureString = null;
        private GCHandle _GCH;

        #endregion

        #region Public Properties

        public SecureString SecureValue
        {
            get { return _secureString; }
            set
            {
                _secureString = value;
                UpdateStringValue();
            }
        }

        public Char[] Value
        {
            get { return _charArray; }
        }

        #endregion

        #region Constructors

        public SecureStringToCharArrayMarshaler()
        {
        }

        public SecureStringToCharArrayMarshaler(SecureString ss)
        {
            SecureValue = ss;
        }

        #endregion

        #region Private Methods

        private void UpdateStringValue()
        {
            Deallocate();

            if (SecureValue != null)
            {
                Int32 length = SecureValue.Length;
                _charArray = new Char[length];

                for (Int32 i = 0; i < length; ++i)
                    _charArray[i] = '\0';

                _GCH = new GCHandle();

                // Create a CER (Contrained Execution Region)
                RuntimeHelpers.PrepareConstrainedRegions();
                try { }
                finally
                {
                    // Pin our char array, disallowing the garbage collector from
                    // moving it around.
                    _GCH = GCHandle.Alloc(_charArray, GCHandleType.Pinned);
                }

                IntPtr stringPtr = IntPtr.Zero;
                RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(
                    delegate
                    {
                        // Create a CER (Contrained Execution Region)
                        RuntimeHelpers.PrepareConstrainedRegions();
                        try { }
                        finally
                        {
                            stringPtr = Marshal.SecureStringToBSTR(SecureValue);
                        }

                        // Copy the SecureString content to our pinned string
                        IntPtr charArrayPtr = _GCH.AddrOfPinnedObject();
                        for (Int32 index = 0; index < length * 2; index++)
                            Marshal.WriteByte(charArrayPtr, index, Marshal.ReadByte(stringPtr, index));
                    },
                    delegate
                    {
                        if (stringPtr != IntPtr.Zero)
                        {
                            // Free the SecureString BSTR that was generated
                            Marshal.ZeroFreeBSTR(stringPtr);
                        }
                    },
                    null);
            }
        }

        private void Deallocate()
        {
            if (_GCH.IsAllocated)
            {
                // Determine the length of the string (* 2 because the string is unicode)
                Int32 length = Value.Length * 2;

                // Zero each character of the string.
                IntPtr charArrayPtr = _GCH.AddrOfPinnedObject();
                for (Int32 index = 0; index < length; ++index)
                    Marshal.WriteByte(charArrayPtr, index, 0);

                // Free the handle so the garbage collector
                // can dispose of it properly.
                _GCH.Free();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Deallocate();
        }

        #endregion
    }
}
