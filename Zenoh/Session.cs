using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zenoh
{
    /// <summary>
    /// Class <c>Session</c> is wrapper around the Zenoh C API Session
    /// </summary>
    public class Session : IDisposable
    {

        internal readonly unsafe ZOwnedSession* innerSession;
        private bool disposed;
        private bool isSessionValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="config">Zenoh config</param>
        public unsafe Session(Config config) 
        {
            ZOwnedSession session = ZenohC.z_open(config.innerConfig);
            if (ZenohC.z_session_check(&session) != 1)
            {
                isSessionValid = false;
            }

            nint p = Marshal.AllocHGlobal(Marshal.SizeOf(session));
            Marshal.StructureToPtr(session, p, false);

            isSessionValid = true;
            innerSession = (ZOwnedSession*)p;
        }

        /// <summary>
        /// Closes the Zenoh session
        /// </summary>
        public void Close()
        {
            if (disposed) return;

            unsafe
            {
                ZenohC.z_close(innerSession);
                Marshal.FreeHGlobal((nint)innerSession);
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            Close();
        }
    }
}
