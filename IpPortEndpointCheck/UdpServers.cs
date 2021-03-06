﻿using System;
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

                    string msg = "(UDP Server: Received) LocalEndPoint {" + localEndpoint.ToString() + "}, RemoteEndPoint {"
                        + RemoteIpEndPoint.ToString() + "}, msg-->{" + returnData.ToString() + "}";
                    Debug.WriteLine(msg);
                    udpServer.AppendMessage(msg);
                    
                    if (returnData.ToString().Contains(UdpServers.kAskQuestion))
                    {
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(UdpServers.kAnswer);
                        ucli.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint);

                        string sendMsg = "(UDP Server: AckSent) LocalEndPoint {" + localEndpoint.ToString() + "}, RemoteEndPoint {"
                        + RemoteIpEndPoint.ToString() + "}, msg-->{" + UdpServers.kAnswer + "}";
                        Debug.WriteLine(sendMsg);
                        udpServer.AppendMessage(sendMsg);
                    }
                }
                ucli.Close();
            }
            catch (SocketException ex)
            {
                Debug.WriteLine("SocketErrorcode " + ex.ErrorCode + ", " + ex.ToString());
                switch (ex.SocketErrorCode)
                {
                    case SocketError.AddressAlreadyInUse:
                        {
                            string msg = "(UDP Server: AddressAlreadyInUse) LocalEndPoint {" + localEndpoint.ToString() + "}";
                            Debug.WriteLine(msg);
                            udpServer.AppendMessage(msg);

                            udpServer.AddExceptionalPort(localEndpoint.Port);
                        }
                        break;
                    default:
                        {
                            Debug.WriteLine(ex.ToString());
                            Debug.Assert(false);
                            //string msg = "(UDP Server: Error) LocalEndPoint {" + localEndpoint.ToString() + "}, Error Code "
                            //    + ex.ErrorCode.ToString() + " Error Msg: " + ex.Message;
                            //Debug.WriteLine(msg);
                            //udpServer.AppendMessage(msg);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Debug.Assert(false);
            }

            
            udpServer.DoDecrease();
        }
    }
}
