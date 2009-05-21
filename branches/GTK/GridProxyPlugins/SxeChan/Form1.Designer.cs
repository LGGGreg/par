namespace SxeChan
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1server = new System.Windows.Forms.TextBox();
            this.textBox2irchan = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox4slchan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listentoChat = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // textBox1server
            // 
            this.textBox1server.BackColor = System.Drawing.Color.Black;
            this.textBox1server.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1server.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBox1server.Location = new System.Drawing.Point(92, 22);
            this.textBox1server.Name = "textBox1server";
            this.textBox1server.Size = new System.Drawing.Size(180, 13);
            this.textBox1server.TabIndex = 1;
            // 
            // textBox2irchan
            // 
            this.textBox2irchan.BackColor = System.Drawing.Color.Black;
            this.textBox2irchan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2irchan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBox2irchan.Location = new System.Drawing.Point(92, 41);
            this.textBox2irchan.Name = "textBox2irchan";
            this.textBox2irchan.Size = new System.Drawing.Size(180, 13);
            this.textBox2irchan.TabIndex = 3;
            this.textBox2irchan.TextChanged += new System.EventHandler(this.textBox2irchan_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Channel";
            // 
            // textBox3port
            // 
            this.textBox3port.BackColor = System.Drawing.Color.Black;
            this.textBox3port.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3port.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBox3port.Location = new System.Drawing.Point(92, 60);
            this.textBox3port.Name = "textBox3port";
            this.textBox3port.Size = new System.Drawing.Size(180, 13);
            this.textBox3port.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port";
            // 
            // textBox4slchan
            // 
            this.textBox4slchan.BackColor = System.Drawing.Color.Black;
            this.textBox4slchan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4slchan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBox4slchan.Location = new System.Drawing.Point(92, 79);
            this.textBox4slchan.Name = "textBox4slchan";
            this.textBox4slchan.Size = new System.Drawing.Size(180, 13);
            this.textBox4slchan.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "SL Channel";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(16, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save and Reconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listentoChat
            // 
            this.listentoChat.AutoSize = true;
            this.listentoChat.Checked = true;
            this.listentoChat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.listentoChat.Location = new System.Drawing.Point(13, 136);
            this.listentoChat.Name = "listentoChat";
            this.listentoChat.Size = new System.Drawing.Size(95, 17);
            this.listentoChat.TabIndex = 9;
            this.listentoChat.Text = "Listen To Chat";
            this.listentoChat.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(114, 132);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Open Chat Window";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 165);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listentoChat);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox4slchan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3port);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2irchan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1server);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "Form1";
            this.Text = "SxeChan - IRC Chatter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1server;
        private System.Windows.Forms.TextBox textBox2irchan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox4slchan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox listentoChat;
        private System.Windows.Forms.Button button2;
    }
}