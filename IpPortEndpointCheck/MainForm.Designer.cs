namespace IpPortEndpointCheck
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.startStopButton = new System.Windows.Forms.Button();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tcpPortAddButton = new System.Windows.Forms.Button();
            this.addTcpPortTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcpPortsTextBox = new System.Windows.Forms.TextBox();
            this.tcpEmptyButton = new System.Windows.Forms.Button();
            this.tcpCommonButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.udpPortAddButton = new System.Windows.Forms.Button();
            this.addUdpPortTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.udpPortsTextBox = new System.Windows.Forms.TextBox();
            this.udpEmptyButton = new System.Windows.Forms.Button();
            this.udpCommonButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxTcp = new System.Windows.Forms.CheckBox();
            this.checkBoxUdp = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // modeComboBox
            // 
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "Client",
            "Server"});
            this.modeComboBox.Location = new System.Drawing.Point(139, 35);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(219, 21);
            this.modeComboBox.TabIndex = 0;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(401, 33);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 25);
            this.startStopButton.TabIndex = 1;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(62, 18);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(219, 20);
            this.ipTextBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ipTextBox);
            this.groupBox1.Location = new System.Drawing.Point(191, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 50);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target IP";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tcpPortAddButton);
            this.groupBox2.Controls.Add(this.addTcpPortTextBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tcpPortsTextBox);
            this.groupBox2.Controls.Add(this.tcpEmptyButton);
            this.groupBox2.Controls.Add(this.tcpCommonButton);
            this.groupBox2.Location = new System.Drawing.Point(34, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 203);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TCP Port";
            // 
            // tcpPortAddButton
            // 
            this.tcpPortAddButton.Location = new System.Drawing.Point(125, 140);
            this.tcpPortAddButton.Name = "tcpPortAddButton";
            this.tcpPortAddButton.Size = new System.Drawing.Size(94, 25);
            this.tcpPortAddButton.TabIndex = 4;
            this.tcpPortAddButton.Text = "Add";
            this.tcpPortAddButton.UseVisualStyleBackColor = true;
            this.tcpPortAddButton.Click += new System.EventHandler(this.tcpPortAddButton_Click);
            // 
            // addTcpPortTextBox
            // 
            this.addTcpPortTextBox.Location = new System.Drawing.Point(125, 118);
            this.addTcpPortTextBox.Name = "addTcpPortTextBox";
            this.addTcpPortTextBox.Size = new System.Drawing.Size(94, 20);
            this.addTcpPortTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "<<";
            // 
            // tcpPortsTextBox
            // 
            this.tcpPortsTextBox.Location = new System.Drawing.Point(18, 30);
            this.tcpPortsTextBox.Multiline = true;
            this.tcpPortsTextBox.Name = "tcpPortsTextBox";
            this.tcpPortsTextBox.ReadOnly = true;
            this.tcpPortsTextBox.Size = new System.Drawing.Size(76, 131);
            this.tcpPortsTextBox.TabIndex = 2;
            // 
            // tcpEmptyButton
            // 
            this.tcpEmptyButton.Location = new System.Drawing.Point(18, 160);
            this.tcpEmptyButton.Name = "tcpEmptyButton";
            this.tcpEmptyButton.Size = new System.Drawing.Size(76, 25);
            this.tcpEmptyButton.TabIndex = 1;
            this.tcpEmptyButton.Text = "Empty";
            this.tcpEmptyButton.UseVisualStyleBackColor = true;
            this.tcpEmptyButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // tcpCommonButton
            // 
            this.tcpCommonButton.Location = new System.Drawing.Point(125, 34);
            this.tcpCommonButton.Name = "tcpCommonButton";
            this.tcpCommonButton.Size = new System.Drawing.Size(94, 66);
            this.tcpCommonButton.TabIndex = 1;
            this.tcpCommonButton.Text = "22000,22001,\r\n22002,22006,\r\n22010,22048\r\n";
            this.tcpCommonButton.UseVisualStyleBackColor = true;
            this.tcpCommonButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.udpPortAddButton);
            this.groupBox3.Controls.Add(this.addUdpPortTextBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.udpPortsTextBox);
            this.groupBox3.Controls.Add(this.udpEmptyButton);
            this.groupBox3.Controls.Add(this.udpCommonButton);
            this.groupBox3.Location = new System.Drawing.Point(279, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(239, 203);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "UDP Port";
            // 
            // udpPortAddButton
            // 
            this.udpPortAddButton.Enabled = false;
            this.udpPortAddButton.Location = new System.Drawing.Point(125, 140);
            this.udpPortAddButton.Name = "udpPortAddButton";
            this.udpPortAddButton.Size = new System.Drawing.Size(94, 25);
            this.udpPortAddButton.TabIndex = 4;
            this.udpPortAddButton.Text = "Add";
            this.udpPortAddButton.UseVisualStyleBackColor = true;
            this.udpPortAddButton.Click += new System.EventHandler(this.udpPortAddButton_Click);
            // 
            // addUdpPortTextBox
            // 
            this.addUdpPortTextBox.Enabled = false;
            this.addUdpPortTextBox.Location = new System.Drawing.Point(125, 118);
            this.addUdpPortTextBox.Name = "addUdpPortTextBox";
            this.addUdpPortTextBox.Size = new System.Drawing.Size(94, 20);
            this.addUdpPortTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "<<";
            // 
            // udpPortsTextBox
            // 
            this.udpPortsTextBox.Location = new System.Drawing.Point(18, 30);
            this.udpPortsTextBox.Multiline = true;
            this.udpPortsTextBox.Name = "udpPortsTextBox";
            this.udpPortsTextBox.ReadOnly = true;
            this.udpPortsTextBox.Size = new System.Drawing.Size(76, 131);
            this.udpPortsTextBox.TabIndex = 2;
            // 
            // udpEmptyButton
            // 
            this.udpEmptyButton.Enabled = false;
            this.udpEmptyButton.Location = new System.Drawing.Point(18, 160);
            this.udpEmptyButton.Name = "udpEmptyButton";
            this.udpEmptyButton.Size = new System.Drawing.Size(76, 25);
            this.udpEmptyButton.TabIndex = 1;
            this.udpEmptyButton.Text = "Empty";
            this.udpEmptyButton.UseVisualStyleBackColor = true;
            // 
            // udpCommonButton
            // 
            this.udpCommonButton.Enabled = false;
            this.udpCommonButton.Location = new System.Drawing.Point(125, 34);
            this.udpCommonButton.Name = "udpCommonButton";
            this.udpCommonButton.Size = new System.Drawing.Size(94, 66);
            this.udpCommonButton.TabIndex = 1;
            this.udpCommonButton.Text = "22048";
            this.udpCommonButton.UseVisualStyleBackColor = true;
            this.udpCommonButton.Click += new System.EventHandler(this.udpCommonButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 380);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(560, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxUdp);
            this.groupBox4.Controls.Add(this.checkBoxTcp);
            this.groupBox4.Location = new System.Drawing.Point(34, 75);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(147, 50);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Protocol";
            // 
            // checkBoxTcp
            // 
            this.checkBoxTcp.AutoSize = true;
            this.checkBoxTcp.Checked = true;
            this.checkBoxTcp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTcp.Location = new System.Drawing.Point(15, 21);
            this.checkBoxTcp.Name = "checkBoxTcp";
            this.checkBoxTcp.Size = new System.Drawing.Size(47, 17);
            this.checkBoxTcp.TabIndex = 0;
            this.checkBoxTcp.Text = "TCP";
            this.checkBoxTcp.UseVisualStyleBackColor = true;
            this.checkBoxTcp.CheckedChanged += new System.EventHandler(this.checkBoxTcp_CheckedChanged);
            // 
            // checkBoxUdp
            // 
            this.checkBoxUdp.AutoSize = true;
            this.checkBoxUdp.Location = new System.Drawing.Point(82, 21);
            this.checkBoxUdp.Name = "checkBoxUdp";
            this.checkBoxUdp.Size = new System.Drawing.Size(49, 17);
            this.checkBoxUdp.TabIndex = 0;
            this.checkBoxUdp.Text = "UDP";
            this.checkBoxUdp.UseVisualStyleBackColor = true;
            this.checkBoxUdp.CheckedChanged += new System.EventHandler(this.checkBoxUdp_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 402);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.modeComboBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(576, 441);
            this.MinimumSize = new System.Drawing.Size(576, 441);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FindPort";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tcpPortsTextBox;
        private System.Windows.Forms.Button tcpCommonButton;
        private System.Windows.Forms.Button tcpEmptyButton;
        private System.Windows.Forms.Button tcpPortAddButton;
        private System.Windows.Forms.TextBox addTcpPortTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button udpPortAddButton;
        private System.Windows.Forms.TextBox addUdpPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox udpPortsTextBox;
        private System.Windows.Forms.Button udpEmptyButton;
        private System.Windows.Forms.Button udpCommonButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxUdp;
        private System.Windows.Forms.CheckBox checkBoxTcp;
    }
}

