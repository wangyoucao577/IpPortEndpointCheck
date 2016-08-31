using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IpPortEndpointCheck
{
    class TcpListeners : NetCheckServers
    {

        public override void StartListen()
        {
            base.StartListen();

            DoStart(TcpListeners.TcpListenerThreadProc);
        }

        static private void TcpListenerThreadProc(object data)
        {
            TcpListeners tcpListener = (TcpListeners)data;
            IPEndPoint localEndpoint = tcpListener.PopEndpoint();
            
            TcpListener listener = new TcpListener(localEndpoint);
            

            try
            {
                listener.Start();

                while (tcpListener.GoonListen)
                {
                    TcpClient cli = listener.AcceptTcpClient();

                    string msg = "(TCP Accepted) LocalEndPoint {" + cli.Client.LocalEndPoint.ToString() + "}, RemoteEndPoint {"
                        + cli.Client.RemoteEndPoint.ToString() + "}";
                    Trace.WriteLine(msg);
                    tcpListener.AppendMessage(msg);

                    cli.Close();
                }

                listener.Stop();
            }
            catch (SocketException ex)
            {
                switch (ex.SocketErrorCode)
                {
                    case SocketError.AddressAlreadyInUse:
                        tcpListener.AddExceptionalPort(localEndpoint.Port);
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            tcpListener.DoDecrease();
        }
    }
}
