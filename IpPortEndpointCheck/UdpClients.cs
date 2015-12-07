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

            UdpClient ucli = new UdpClient();
            Socket uSocket = ucli.Client;
            uSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);

            //Send
            Byte[] sendBytes = Encoding.ASCII.GetBytes("AreYouOk?");
            try
            {
                ucli.Send(sendBytes, sendBytes.Length, new IPEndPoint(udpClients.TargetIP, port));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Debug.Assert(false);
            }

            //Receive
            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = ucli.Receive(ref RemoteIpEndPoint);
                ucli.Close();

                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Trace.WriteLine("This is the message you received :" +
                                             returnData.ToString());
                Trace.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());

                if (returnData.ToString().Contains("ImOk!"))
                {

                }
                else
                {
                    udpClients.AddExceptionalPort(port);
                }

            }
            catch (SocketException ex)
            {
                Trace.WriteLine("SocketErrorcode " + ex.ErrorCode + ", " + ex.ToString());
                switch (ex.ErrorCode)
                {
                    case 10054:
                        break;
                    default:
                        break;
                }

                udpClients.AddExceptionalPort(port);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Debug.Assert(false);
            }

            udpClients.DecreaseConnect();
        }

    }
}
