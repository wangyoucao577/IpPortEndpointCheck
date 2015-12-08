using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class NetCheckServers : NetClients
    {
        protected bool m_goonListen = true;
        protected object m_goonListenMutex = new object();

        public NetCheckServers() : base(null) { }

        public void StopListen()
        {
            lock (m_goonListenMutex)
            {
                m_goonListen = false;
            }
        }

        public void WaitListensStop()
        {
            foreach (Thread item in m_threadList)
            {
                item.Join();
            }
        }


        protected bool GoonListening()
        {
            lock (m_goonListenMutex)
            {
                return m_goonListen;
            }
        }

    }
}
