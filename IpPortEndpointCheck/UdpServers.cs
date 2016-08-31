using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IpPortEndpointCheck
{
    class UdpServers : NetCheckServers
    {
        public override void StartListen()
        {
            base.StartListen();

            DoStart(UdpServers.UdpReceiveThreadProc);
        }

        static private void UdpReceiveThreadProc(object data)
        {

            UdpServers udpServer = (UdpServers)data;
            IPEndPoint localEndpoint = udpServer.PopEndpoint();

            //Receive
            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                UdpClient ucli = new UdpClient(localEndpoint);

                while (udpServer.GoonListen)
                {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = ucli.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Trace.WriteLine("(UDP Received) LocalEndPoint {" + localEndpoint.ToString() + "}, RemoteEndPoint {"
                        + RemoteIpEndPoint.ToString() + "}, msg-->{" + returnData.ToString() + "}");
                    //Trace.WriteLine("This is the message you received :" +
                    //                            returnData.ToString());

                    //Trace.WriteLine("This message was sent from " +
                    //                            RemoteIpEndPoint.Address.ToString() +
                    //                            " on their port number " +
                    //                            RemoteIpEndPoint.Port.ToString());

                    if (returnData.ToString().Contains(UdpServers.kAskQuestion))
                    {
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(UdpServers.kAnswer);
                        ucli.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint);
                    }
                    
                }

                ucli.Close();
            }
            catch (SocketException ex)
            {
                Trace.WriteLine("SocketErrorcode " + ex.ErrorCode + ", " + ex.ToString());
                switch (ex.SocketErrorCode)
                {
                    case SocketError.AddressAlreadyInUse:
                        udpServer.AddExceptionalPort(localEndpoint.Port);
                        break;
                    default:
                        Trace.WriteLine(ex.ToString());
                        Debug.Assert(false);
                        break;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Debug.Assert(false);
            }

            udpServer.DoDecrease();
        }
    }
}
