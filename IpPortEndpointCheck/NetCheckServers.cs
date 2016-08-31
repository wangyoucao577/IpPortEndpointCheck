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

        protected bool GoonListen
        {
            get
            {
                lock (m_goonListenMutex)
                {
                    return m_goonListen;
                }
            }

            set
            {
                lock (m_goonListenMutex)
                {
                    m_goonListen = value;
                }
            }
        }

        public virtual void StartListen()
        {
            GoonListen = true;
        }

        public virtual void StopListen()
        {
            GoonListen = false;
        }

        public override void AppendMessage(string msg)
        {
            base.AppendMessage(msg);

            if (GoonListen)
            {
                Notify();
            }
        }

        public void WaitListensStop()
        {
            foreach (Thread item in m_threadList)
            {
                item.Join();
            }
        }

    }
}
