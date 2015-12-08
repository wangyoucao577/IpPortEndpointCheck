using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace IpPortEndpointCheck
{
    class NetClients
    {
        protected const string kAskQuestion = "AreYouOk?";
        protected const string kAnswer = "ImOk!";

        protected List<int> m_portList = new List<int>();
        protected object m_portListMutex = new object();

        protected List<int> m_exceptionalPortList = new List<int>();
        protected object m_exceptionalPortListMutex = new object();

        protected List<Thread> m_threadList = new List<Thread>();

        protected int m_connecting = 0;
        protected object m_connectingMutex = new object();

        protected IPAddress m_ip = null;
        protected IPAddress TargetIP
        {
            get { return m_ip; }
        }

        protected NetClients(IPAddress ip)
        {
            m_ip = ip;
        }

        public bool AddPort(int port)
        {
            Debug.Assert(port >= 0 && port < 65536);

            lock (m_portListMutex)
            {
                m_portList.Add(port);
            }

            return true;
        }

        public int PopPort()
        {
            int port = 0;
            lock (m_portListMutex)
            {
                port = m_portList[0];
                m_portList.RemoveAt(0);
            }

            return port;
        }

        public bool IsConnectFinished(out List<int> exceptionalPorts)
        {
            bool finish = false;
            lock (m_connectingMutex)
            {
                finish = m_connecting <= 0 ? true : false;
            }

            if (finish)
            {
                lock (m_exceptionalPortListMutex)
                {
                    exceptionalPorts = m_exceptionalPortList;
                }
            }
            else
            {
                exceptionalPorts = null;
            }

            return finish;
        }

        public bool AddExceptionalPort(int port)
        {
            lock (m_exceptionalPortListMutex)
            {
                m_exceptionalPortList.Add(port);
            }

            return true;
        }

        protected bool DoDecrease()
        {
            lock (m_connectingMutex)
            {
                --m_connecting;
            }

            return true;
        }

        protected void DoStart(ParameterizedThreadStart start)
        {
            int count = m_portList.Count;

            lock (m_connectingMutex)
            {
                m_connecting = count;
            }

            for (int i = 0; i < count; i++)
            {
                Thread threadId = new Thread(start);
                threadId.Start((object)this);

                m_threadList.Add(threadId);
            }
        }

        public virtual void StartConnect()
        {
            //Nobody will invoke this virtual method
            Debug.Assert(false);
        }
    }
}
