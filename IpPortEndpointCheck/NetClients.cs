using System;
using System.Collections.Generic;
using System.Text;

namespace IpPortEndpointCheck
{
    class NetClients
    {
        protected List<int> m_portList = new List<int>();
        protected object m_portListMutex = new object();

    }
}
