using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class TcpClients : NetClients
    {
        
        public TcpClients(IPAddress ip)
        {
            m_ip = ip;
        }

        public IPAddress TargetIP
        {
            get { return m_ip; }
        }

        public bool AddPort(int tcpPort)
        {
            Debug.Assert(tcpPort >= 0 && tcpPort < 65536);

            lock (m_portListMutex)
            {
                m_portList.Add(tcpPort);
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

        public void StartConnect()
        {
            

            int count = m_portList.Count;

            lock (m_connectingMutex)
            {
                m_connecting = count;
            }

            for (int i = 0; i < count; i++)
            {
                Thread threadId = new Thread(TcpClients.TcpConnectThreadProc);
                threadId.Start((object)this);

                m_connectThreadList.Add(threadId);
            }

        }

        public bool DecreaseConnect()
        {
            lock (m_connectingMutex)
            {
                --m_connecting;
            }

            return true;
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

        static private void TcpConnectThreadProc(object data)
        {
            TcpClients tcpClients = (TcpClients)data;
            int port = tcpClients.PopPort();

            TcpClient tcli = new TcpClient();
            tcli.SendTimeout = 1000;

            try
            {
                tcli.Connect(new IPEndPoint(tcpClients.TargetIP, port));
                tcli.Close();
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10061:
                        tcpClients.AddExceptionalPort(port);
                        break;
                    default:
                        tcpClients.AddExceptionalPort(port);
                        break;
                }
            }

            tcpClients.DecreaseConnect();
        }
    }
}
