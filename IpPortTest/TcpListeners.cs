using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortTest
{
    class TcpListeners
    {
        private List<int> m_tcpPortList = new List<int>();
        private object m_portListMutex = new object();

        private List<Thread> m_listenThreadList = new List<Thread>();

        private bool m_goonListen = true;
        private object m_goonListenMutex = new object();

        public bool AddPort(int tcpPort)
        {
            Debug.Assert(tcpPort >= 0 && tcpPort < 65536);

            lock (m_portListMutex)
            {
                m_tcpPortList.Add(tcpPort);
            }

            return true;
        }

        public int PopPort()
        {
            int port = 0;
            lock (m_portListMutex)
            {
                port = m_tcpPortList[0];
                m_tcpPortList.RemoveAt(0);
            }

            return port;
        }

        public bool StartListen()
        {
            lock (m_goonListenMutex)
            {
                m_goonListen = true;
            }

            int count = m_tcpPortList.Count;

            for (int i = 0; i < count; i++)
            {
                Thread threadId = new Thread(TcpListeners.TcpListenerThreadProc);
                threadId.Start((object)this);

                m_listenThreadList.Add(threadId);
            }

            return true;
        }
        public void StopListen()
        {
            lock (m_goonListenMutex)
            {
                m_goonListen = false;
            }
        }

        public void WaitListensStop()
        {
            foreach (Thread item in m_listenThreadList)
            {
                item.Join();
            }
        }

        public bool GoonListening()
        {
            lock (m_goonListenMutex)
            {
                return m_goonListen;
            }
        }

        static private void TcpListenerThreadProc(object data)
        {
            TcpListeners tcpListener = (TcpListeners)data;
            int port = tcpListener.PopPort();

            
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                listener.Start();

                while (tcpListener.GoonListening())
                {
                    TcpClient cli = listener.AcceptTcpClient();
                    cli.Close();
                }

                listener.Stop();
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10048:
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

        }
    }
}
