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
        
        public TcpClients(IPAddress ip) : base(ip)
        {
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
