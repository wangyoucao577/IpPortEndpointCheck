using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;


namespace IpPortEndpointCheck
{
    class UdpClients : NetClients
    {
        public UdpClients(IPAddress ip) : base(ip)
        {
        }

        public override void StartConnect()
        {
            DoStart(UdpClients.UdpConnectThreadProc);
        }

        static private void UdpConnectThreadProc(object data)
        {
            UdpClients udpClients = (UdpClients)data;
            int port = udpClients.PopPort();

            UdpClient ucli = new UdpClient();
            Socket uSocket = ucli.Client;
            uSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);

            //Send
            Byte[] sendBytes = Encoding.ASCII.GetBytes(UdpClients.kAskQuestion);
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

                if (returnData.ToString().Contains(UdpClients.kAnswer))
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
                switch (ex.SocketErrorCode)
                {
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

            udpClients.DoDecrease();
        }

    }
}
