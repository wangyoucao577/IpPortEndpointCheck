using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class NetClients
    {
        protected List<int> m_portList = new List<int>();
        protected object m_portListMutex = new object();

        protected List<int> m_exceptionalPortList = new List<int>();
        protected object m_exceptionalPortListMutex = new object();

        protected List<Thread> m_connectThreadList = new List<Thread>();

        protected int m_connecting = 0;
        protected object m_connectingMutex = new object();

        protected IPAddress m_ip = null;

    }
}
