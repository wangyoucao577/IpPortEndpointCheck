﻿using System;
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
            IPEndPoint peerEndpoint = udpClients.PopEndpoint();
            if (peerEndpoint.AddressFamily != udpClients.TargetIP.AddressFamily)
            {
                //ip stack not match
                udpClients.DoDecrease();
                return;
            }


            UdpClient ucli = new UdpClient(udpClients.TargetIP.AddressFamily);
            Socket uSocket = ucli.Client;
            uSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(UdpClients.kAskQuestion);

            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                //Send
                ucli.Send(sendBytes, sendBytes.Length, new IPEndPoint(udpClients.TargetIP, peerEndpoint.Port));

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = ucli.Receive(ref RemoteIpEndPoint);

                string returnData = Encoding.ASCII.GetString(receiveBytes);

                string msg = "(UDP Client Received) LocalEndPoint {" + ucli.Client.LocalEndPoint.ToString() + "}, RemoteEndPoint {"
                    + RemoteIpEndPoint.ToString() + "}, msg-->{" + returnData.ToString() + "}";
                Trace.WriteLine(msg);

                if (returnData.ToString().Contains(UdpClients.kAnswer))
                {

                }
                else
                {
                    udpClients.AddExceptionalPort(peerEndpoint.Port);
                }

            }
            catch (SocketException ex)
            {
                Trace.WriteLine("SocketErrorcode " + ex.ErrorCode + ", " + ex.ToString());
                switch (ex.SocketErrorCode)
                {
                    case SocketError.TimedOut:
                    case SocketError.ConnectionReset:
                        break;
                    default:
                        break;
                }
                udpClients.AddExceptionalPort(peerEndpoint.Port);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Debug.Assert(false);
            }
            finally
            {
                ucli.Close();
            }

            udpClients.DoDecrease();
        }
    }
}
