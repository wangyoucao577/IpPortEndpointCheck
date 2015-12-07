using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class TcpClients : NetClients
    {
        
        public TcpClients(IPAddress ip) : base(ip)
        {
        }

        static public void TcpConnectThreadProc(object data)
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
