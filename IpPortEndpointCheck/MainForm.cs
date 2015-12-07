using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace IpPortTest
{
    public partial class MainForm : Form
    {
        private TcpListeners m_tcpListeners = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] tcpPorts = tcpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] udpPorts = udpPortsTextBox.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
            appendTcpPort("22000");
            appendTcpPort("22001");
            appendTcpPort("22002");
            appendTcpPort("22006");
            appendTcpPort("22010");
            appendTcpPort("22048");
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
            if (-1 == tcpPortsTextBox.Text.IndexOf(port))
            {
                tcpPortsTextBox.AppendText(port + "\r\n");
            }
        }
        
        private void appendUdpPort(string port)
        {
            if (-1 == udpPortsTextBox.Text.IndexOf(port))
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
                    m_tcpListeners.StopListen();

                    TcpClients tclis = new TcpClients(IPAddress.Parse("127.0.0.1"));
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

                    m_tcpListeners.WaitListensStop();
                    m_tcpListeners = null;
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
            appendUdpPort("22048");
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

            if (checkBoxTcp.Checked)
            {
                TcpClients tclis = new TcpClients(ip);
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

                if (null != exceptionalTcpPorts && exceptionalTcpPorts.Count > 0)
                {
                    string showText = null;
                    foreach (int item in exceptionalTcpPorts)
                    {
                        showText += item.ToString();
                        showText += " ";
                    }
                    showText += " invalid!";
                    MessageBox.Show(showText);
                }
                else
                {
                    MessageBox.Show("Ports valid.");
                }
            }

            if (checkBoxUdp.Checked)
            {
                //TODO: add udp functions
            }
            

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
                    foreach (string item in tcpPorts)
                    {
                        m_tcpListeners.AddPort(Convert.ToInt32(item));
                    }
                    m_tcpListeners.StartListen();
                }

                if (checkBoxUdp.Checked)
                {
                    //TODO: add udp functions
                }
            
                
            }
            else
            {
                //goto stop
                if (checkBoxTcp.Checked)
                {
                    m_tcpListeners.StopListen();

                    TcpClients tclis = new TcpClients(IPAddress.Parse("127.0.0.1"));
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

                    m_tcpListeners.WaitListensStop();
                    m_tcpListeners = null;
                }

                if (checkBoxUdp.Checked)
                {
                    //TODO: add udp functions
                }
            

                EnableTcpUI(true);
                EnableUdpUI(true);
                checkBoxTcp.Enabled = true;
                checkBoxUdp.Enabled = true;
                modeComboBox.Enabled = true;
                startStopButton.Text = "Start";
            }
        }
    }
}
