using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace IpPortEndpointCheck
{
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();

            //Print Local Machine IP Address
            IPAddress[] localIP = Dns.GetHostAddresses("");
            PushNewMessage("---------- Local Machine IP Address ----------");
            foreach (IPAddress ip in localIP)
            {
                PushNewMessage(ip.AddressFamily.ToString() + "   { " + ip.ToString() + " }");
            }
            PushNewMessage("---------- Local Machine IP Address ----------\r\n\r\n\r\n");
            
            
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            messageTextBox.Text = "";
        }

        delegate void PushNewMessageCallback(string msg);

        public void PushNewMessage(string msg)
        {
            if (!msg.EndsWith("\r\n"))
            {
                msg += "\r\n";
            }

            if (messageTextBox.InvokeRequired)
            {
                PushNewMessageCallback d = new PushNewMessageCallback(PushNewMessage);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                messageTextBox.Text += msg;
            }
        }
    }
}
