using System;
using System.Collections.Generic;
using System.Text;

namespace IpPortEndpointCheck
{
    class UdpServer : NetCheckServers
    {
        public override void StartListen()
        {
            base.StartListen();

            DoStart(UdpServer.UdpReceiveThreadProc);
        }

        static private void UdpReceiveThreadProc(object data)
        {
            //TODO: add udp function here

            //TcpListeners tcpListener = (TcpListeners)data;
            //int port = tcpListener.PopPort();


            //TcpListener listener = new TcpListener(IPAddress.Any, port);

            //try
            //{
            //    listener.Start();

            //    while (tcpListener.GoonListen)
            //    {
            //        TcpClient cli = listener.AcceptTcpClient();
            //        cli.Close();
            //    }

            //    listener.Stop();
            //}
            //catch (SocketException ex)
            //{
            //    switch (ex.ErrorCode)
            //    {
            //        case 10048:
            //            break;
            //        default:
            //            Debug.Assert(false);
            //            break;
            //    }
            //}

            //tcpListener.DoDecrease();
        }
    }
}
