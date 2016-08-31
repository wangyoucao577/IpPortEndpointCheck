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

        public override void StartConnect()
        {
            DoStart(TcpClients.TcpConnectThreadProc);
        }

        static private void TcpConnectThreadProc(object data)
        {
            TcpClients tcpClients = (TcpClients)data;
            IPEndPoint peerEndpoint = tcpClients.PopEndpoint();
            if (peerEndpoint.AddressFamily != tcpClients.TargetIP.AddressFamily)
            {   
                //ip stack not match
                tcpClients.DoDecrease();
                return;
            }

            TcpClient tcli = new TcpClient(tcpClients.TargetIP.AddressFamily);
            tcli.SendTimeout = 1000;

            try
            {
                tcli.Connect(new IPEndPoint(tcpClients.TargetIP, peerEndpoint.Port));
                tcli.Close();
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10061:
                        tcpClients.AddExceptionalPort(peerEndpoint.Port);
                        break;
                    default:
                        tcpClients.AddExceptionalPort(peerEndpoint.Port);
                        break;
                }
            }

            tcpClients.DoDecrease();
        }
    }
}
