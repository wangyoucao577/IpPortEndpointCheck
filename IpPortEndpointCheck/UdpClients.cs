using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace IpPortEndpointCheck
{
    class UdpClients
    {
        private List<int> m_portList = new List<int>();
        private object m_portListMutex = new object();

        private List<int> m_exceptionalPortList = new List<int>();
        private object m_exceptionalPortListMutex = new object();

        private List<Thread> m_connectThreadList = new List<Thread>();

        private int m_connecting = 0;
        private object m_connectingMutex = new object();

        private IPAddress m_ip = null;
        public UdpClients(IPAddress ip)
        {
            m_ip = ip;
        }

        public IPAddress TargetIP
        {
            get { return m_ip; }
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

        public void StartConnect()
        {
            int count = m_portList.Count;

            lock (m_connectingMutex)
            {
                m_connecting = count;
            }

            for (int i = 0; i < count; i++)
            {
                Thread threadId = new Thread(UdpClients.UdpConnectThreadProc);
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

        static private void UdpConnectThreadProc(object data)
        {
            UdpClients udpClients = (UdpClients)data;
            int port = udpClients.PopPort();

            //TODO: add actually functions here

            //UdpClient tcli = new TcpClient();
            //tcli.SendTimeout = 1000;

            //try
            //{
            //    tcli.Connect(new IPEndPoint(tcpClients.TargetIP, port));
            //    tcli.Close();
            //}
            //catch (SocketException ex)
            //{
            //    switch (ex.ErrorCode)
            //    {
            //        case 10061:
            //            tcpClients.AddExceptionalPort(port);
            //            break;
            //        default:
            //            tcpClients.AddExceptionalPort(port);
            //            break;
            //    }
            //}

            //tcpClients.DecreaseConnect();
        }

    }
}
