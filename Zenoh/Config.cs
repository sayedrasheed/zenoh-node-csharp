using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zenoh
{
    /// <summary>
    /// Class <c>Config</c> is wrapper around the Zenoh C API Config
    /// </summary>
    public class Config : IDisposable
    {
        internal unsafe ZOwnedConfig* innerConfig;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="network">Network IP and port string in the format of "IPV4:Port"</param>
        public Config(string network)
        {
            unsafe
            {
                ZOwnedConfig config = ZenohC.z_config_default();
                nint p = Marshal.AllocHGlobal(Marshal.SizeOf(config));
                Marshal.StructureToPtr(config, p, false);
                innerConfig = (ZOwnedConfig*)p;
                SetNetwork(network);
            }

            disposed = false;
        }

        /// <summary>
        /// Sets the multicast scouting IP and port for this session class.
        /// </summary>
        /// <param name="network">Network IP and port string in the format of "IPV4:Port"</param>
        public bool SetNetwork(string network)
        {
            if (disposed) return false;

            unsafe
            {
                ZConfig config = ZenohC.z_config_loan(innerConfig);
                sbyte r = ZenohC.zc_config_insert_json(config, ZenohC.zConfigMulticastIpv4AddressKey, network);
                return r == 0;
            }
        }

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            unsafe
            {
                ZenohC.z_config_drop(innerConfig);
                Marshal.FreeHGlobal((nint)innerConfig);
            }

            disposed = true;
        }
    }
}
