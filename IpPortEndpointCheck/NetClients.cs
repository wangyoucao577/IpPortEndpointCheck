using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace IpPortEndpointCheck
{
    class NetClients : Observer.Subject
    {
        protected const string kAskQuestion = "AreYouOk?";
        protected const string kAnswer = "ImOk!";

        protected List<IPEndPoint> m_portList = new List<IPEndPoint>();
        protected object m_portListMutex = new object();

        protected List<int> m_exceptionalPortList = new List<int>();
        protected object m_exceptionalPortListMutex = new object();

        protected List<string> m_messageList = new List<string>();
        protected object m_messageListMutex = new object();

        protected List<Thread> m_threadList = new List<Thread>();

        protected int m_connecting = 0;
        protected object m_connectingMutex = new object();

        protected IPAddress m_ip = null;
        protected IPAddress TargetIP
        {
            get { return m_ip; }
        }

        public List<int> ExceptionPortList
        {
            get
            {
                lock (m_exceptionalPortListMutex)
                {
                    return m_exceptionalPortList;
                }
            }
        }
    
        public string GetNextMessage()
        {
            lock (m_messageListMutex)
            {
                string msg = m_messageList[0];
                m_messageList.RemoveAt(0);
                return msg;
            }
        }

        public virtual void AppendMessage(string msg)
        {
            lock (m_messageListMutex)
            {
                m_messageList.Add(msg);
            }
            
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
                if (Socket.SupportsIPv4)
                {
                    m_portList.Add(new IPEndPoint(IPAddress.Any, port));
                }
                if (Socket.OSSupportsIPv6)
                {
                    m_portList.Add(new IPEndPoint(IPAddress.IPv6Any, port));
                }
            }

            return true;
        }

        public IPEndPoint PopEndpoint()
        {
            IPEndPoint port;
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
