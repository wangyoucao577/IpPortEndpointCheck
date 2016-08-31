using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace IpPortEndpointCheck
{
    public partial class MainForm : Form, Observer.IObserver
    {
        private TcpListeners m_tcpListeners = null;
        private UdpServers m_udpServers = null;
        private List<int> m_udpServersExceptionPortList = null;

        public MainForm()
        {
            InitializeComponent();
            this.Text = ProductName + "  " + ProductVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] tcpPorts = tcpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] udpPorts = udpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            toolStripStatusLabel.Text = ""; //Clear the tooltip

            if (checkBoxTcp.Checked && tcpPorts.Length <= 0)
            {
                MessageBox.Show("Please add some ports for TCP check...");
                return;
            }

            if (checkBoxUdp.Checked && udpPorts.Length <= 0)
            {
                MessageBox.Show("Please add some ports for UDP check...");
                return;
            }

            if (!modeComboBox.Text.Equals("Server") && !modeComboBox.Text.Equals("Client"))
            {
                MessageBox.Show("Unknown Mode.");
                Debug.Assert(false);
                return;
            }

            if (modeComboBox.Text.Equals("Server"))
            {
                CheckAsServer(tcpPorts, udpPorts);
            }
            else if (modeComboBox.Text.Equals("Client"))
            {
                IPAddress ip;
                if (!IPAddress.TryParse(ipTextBox.Text, out ip))
                {
                    MessageBox.Show("Ip Address Invalid, Please Edit...");
                    return;
                }

                CheckAsClient(ip, tcpPorts, udpPorts);
            }
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tcpPortsTextBox.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            appendTcpPort("80");
            appendTcpPort("3389");
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modeComboBox.Text.Equals("Server"))
            {
                ipTextBox.Enabled = false;
            }
            else if (modeComboBox.Text.Equals("Client"))
            {
                ipTextBox.Enabled = true;
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void tcpPortAddButton_Click(object sender, EventArgs e)
        {
            int port;
            if (    null == addTcpPortTextBox.Text 
                ||  string.Empty == addTcpPortTextBox.Text
                ||  !Int32.TryParse(addTcpPortTextBox.Text, out port)
                ||  port <= 0
                ||  port >= 65536)
            {
                string showText = "Invalid Port " + addTcpPortTextBox.Text;
                MessageBox.Show(showText);
                return;
            }

            appendTcpPort(addTcpPortTextBox.Text);
        }

        private void appendTcpPort(string port)
        {
            if (-1 == tcpPortsTextBox.Text.IndexOf(port + "\r\n"))
            {
                tcpPortsTextBox.AppendText(port + "\r\n");
            }
        }
        
        private void appendUdpPort(string port)
        {
            if (-1 == udpPortsTextBox.Text.IndexOf(port + "\r\n"))
            {
                udpPortsTextBox.AppendText(port + "\r\n");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modeComboBox.Text.Equals("Server"))
            {
                if (startStopButton.Text.Equals("Stop"))
                {
                    string[] tcpPorts = tcpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] udpPorts = udpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //goto stop
                    CheckAsServer(tcpPorts, udpPorts);
                }
            }
        }

        private void checkBoxTcp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTcp.Checked)
            {
                EnableTcpUI(true);

                startStopButton.Enabled = true;
            }
            else
            {
                EnableTcpUI(false);
            }

            if (!checkBoxTcp.Checked && !checkBoxUdp.Checked)
            {
                startStopButton.Enabled = false;
            }
        }

        private void EnableTcpUI(bool value)
        {
            tcpCommonButton.Enabled = value;
            tcpPortAddButton.Enabled = value;
            tcpEmptyButton.Enabled = value;
            addTcpPortTextBox.Enabled = value;
        }

        private void EnableUdpUI(bool value)
        {
            udpCommonButton.Enabled = value;
            udpPortAddButton.Enabled = value;
            udpEmptyButton.Enabled = value;
            addUdpPortTextBox.Enabled = value;
        }

        private void checkBoxUdp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUdp.Checked)
            {
                EnableUdpUI(true);

                startStopButton.Enabled = true;
            }
            else
            {
                EnableUdpUI(false);
            }

            if (!checkBoxTcp.Checked && !checkBoxUdp.Checked)
            {
                startStopButton.Enabled = false;
            }
        }

        private void udpCommonButton_Click(object sender, EventArgs e)
        {
            appendUdpPort("5060");
        }

        private void udpPortAddButton_Click(object sender, EventArgs e)
        {
            int port;
            if (null == addUdpPortTextBox.Text
                || string.Empty == addUdpPortTextBox.Text
                || !Int32.TryParse(addUdpPortTextBox.Text, out port)
                || port <= 0
                || port >= 65536)
            {
                string showText = "Invalid Port " + addUdpPortTextBox.Text;
                MessageBox.Show(showText);
                return;
            }

            appendUdpPort(addUdpPortTextBox.Text);
        }

        private void CheckAsClient(IPAddress ip, string[] tcpPorts, string[] udpPorts)
        {
            EnableTcpUI(false);
            EnableUdpUI(false);
            checkBoxTcp.Enabled = false;
            checkBoxUdp.Enabled = false;
            ipTextBox.Enabled = false;
            modeComboBox.Enabled = false;

            string showText = null;
            if (checkBoxTcp.Checked)
            {
                TcpClients tclis = new TcpClients(ip);
                tclis.Attach(this);
                foreach (string item in tcpPorts)
                {
                    tclis.AddPort(Convert.ToInt32(item));
                }
                tclis.StartConnect();

                List<int> exceptionalTcpPorts;
                while (!tclis.IsConnectFinished(out exceptionalTcpPorts))
                {
                    Thread.Sleep(500);
                }

                showText += "TCP Ports ";
                if (null != exceptionalTcpPorts && exceptionalTcpPorts.Count > 0)
                {
                    foreach (int item in exceptionalTcpPorts)
                    {
                        showText += item.ToString();
                        showText += " ";
                    }
                    showText += " invalid!";
                    //MessageBox.Show(showText);
                }
                else
                {
                    //MessageBox.Show(" valid.");
                    showText += "ALL valid.";
                }
            }

            if (checkBoxUdp.Checked)
            {
                showText += "\nUDP Ports ";

                UdpClients tclis = new UdpClients(ip);
                tclis.Attach(this);
                foreach (string item in udpPorts)
                {
                    tclis.AddPort(Convert.ToInt32(item));
                }
                tclis.StartConnect();

                List<int> exceptionalPorts;
                while (!tclis.IsConnectFinished(out exceptionalPorts))
                {
                    Thread.Sleep(500);
                }

                if (null != exceptionalPorts && exceptionalPorts.Count > 0)
                {
                    foreach (int item in exceptionalPorts)
                    {
                        showText += item.ToString();
                        showText += " ";
                    }
                    showText += " invalid!";
                    //MessageBox.Show(showText);
                }
                else
                {
                    //MessageBox.Show("Ports valid.");
                    showText += "ALL valid.";
                }
            }

            MessageBox.Show(showText);

            EnableTcpUI(true);
            EnableUdpUI(true);
            checkBoxTcp.Enabled = true;
            checkBoxUdp.Enabled = true;
            ipTextBox.Enabled = true;
            modeComboBox.Enabled = true;
        }

        private void CheckAsServer(string[] tcpPorts, string[] udpPorts)
        {
            if (startStopButton.Text.Equals("Start"))
            {
                //goto start

                startStopButton.Text = "Stop";

                EnableTcpUI(false);
                EnableUdpUI(false);
                checkBoxTcp.Enabled = false;
                checkBoxUdp.Enabled = false;
                modeComboBox.Enabled = false;

                if (checkBoxTcp.Checked)
                {
                    Debug.Assert(null == m_tcpListeners);
                    m_tcpListeners = new TcpListeners();
                    m_tcpListeners.Attach(this);
                    foreach (string item in tcpPorts)
                    {
                        m_tcpListeners.AddPort(Convert.ToInt32(item));
                    }
                    m_tcpListeners.StartListen();
                }

                if (checkBoxUdp.Checked)
                {
                    Debug.Assert(null == m_udpServers);
                    m_udpServers = new UdpServers();
                    m_udpServers.Attach(this);
                    foreach (string item in udpPorts)
                    {
                        m_udpServers.AddPort(Convert.ToInt32(item));
                    }
                    m_udpServers.StartListen();
                }

                m_udpServersExceptionPortList = null;
                timer2CheckServerException.Enabled = true;
            }
            else
            {
                //goto stop
                timer2CheckServerException.Enabled = false;
                m_udpServersExceptionPortList = null;

                if (checkBoxTcp.Checked)
                {
                    m_tcpListeners.StopListen();

                    TcpClients tclis = new TcpClients(IPAddress.Loopback);
                    TcpClients tclisv6 = new TcpClients(IPAddress.IPv6Loopback);
                    foreach (string item in tcpPorts)
                    {
                        tclis.AddPort(Convert.ToInt32(item));
                        tclisv6.AddPort(Convert.ToInt32(item));
                    }
                    tclis.StartConnect();
                    tclisv6.StartConnect();

                    List<int> exceptionalTcpPorts, exceptionalTcpPortsv6;
                    while (!tclis.IsConnectFinished(out exceptionalTcpPorts) || !tclisv6.IsConnectFinished(out exceptionalTcpPortsv6))
                    {
                        Thread.Sleep(500);
                    }

                    m_tcpListeners.WaitListensStop();
                    m_tcpListeners = null;
                }

                if (checkBoxUdp.Checked)
                {
                    m_udpServers.StopListen();

                    UdpClients uclis = new UdpClients(IPAddress.Loopback);
                    UdpClients uclisv6 = new UdpClients(IPAddress.IPv6Loopback);
                    foreach (string item in udpPorts)
                    {
                        uclis.AddPort(Convert.ToInt32(item));
                        uclisv6.AddPort(Convert.ToInt32(item));
                    }
                    uclis.StartConnect();
                    uclisv6.StartConnect();

                    List<int> exceptionalUdpPorts, exceptionalUdpPortsv6;
                    while (!uclis.IsConnectFinished(out exceptionalUdpPorts) || !uclisv6.IsConnectFinished(out exceptionalUdpPortsv6))
                    {
                        Thread.Sleep(500);
                    }

                    m_udpServers.WaitListensStop();
                    m_udpServers = null;
                }
            

                EnableTcpUI(true);
                EnableUdpUI(true);
                checkBoxTcp.Enabled = true;
                checkBoxUdp.Enabled = true;
                modeComboBox.Enabled = true;
                startStopButton.Text = "Start";
            }
        }

        private void timer2CheckServerException_Tick(object sender, EventArgs e)
        {
            //TCP is not important to do a tip because it will not interfere the result

            //UDP
            if (checkBoxUdp.Checked)
            {
                List<int> udpExceptionPortList = m_udpServers.ExceptionPortList;
                if (udpExceptionPortList.Count > 0)
                {
                    bool exceptionPortsUpdate = false;
                    if (m_udpServersExceptionPortList == null || udpExceptionPortList.Count > m_udpServersExceptionPortList.Count)
                    {
                        m_udpServersExceptionPortList = udpExceptionPortList;
                        exceptionPortsUpdate = true;
                    }

                    if (exceptionPortsUpdate)
                    {
                        string udpPortsTipText = "Warning: UDP Port ";
                        foreach (int item in udpExceptionPortList)
                        {
                            udpPortsTipText += item.ToString();
                            udpPortsTipText += " ";
                        }
                        udpPortsTipText += "Already In Use, so that can NOT be checked!";

                        toolStripStatusLabel.Text = udpPortsTipText;
                    }
                }
            }
        }

        private void udpEmptyButton_Click(object sender, EventArgs e)
        {
            udpPortsTextBox.Text = null;
        }

        public void Update(object sub)
        {
            NetClients cl = (NetClients)sub;
            Trace.WriteLine("Observer : " + cl.GetNextMessage());
        }
    }
}
