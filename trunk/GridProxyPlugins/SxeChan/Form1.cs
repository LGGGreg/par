/*
 * Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back)
 *All rights reserved.
 *
 *Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

 *THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SxeChan
{

    public partial class Form1 : Form
    {
         private int chan = 1337;
         private string chanel = "#PARplugins";
         private string server = "chat.freenode.net";
         private int port = 6667;
         private bool lsn = true;
        private SxePlugin sp;
        public Form1(SxePlugin s)
        {
            InitializeComponent();
            sp = s;
        }
        public int getChan()
        {
            return int.Parse(textBox4slchan.Text);
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        public void setForm(string server, string slch, string ircha, string port,bool b)
        {
            textBox1server.Text = server;
            textBox2irchan.Text = ircha;
            textBox3port.Text = port;
            textBox4slchan.Text = slch;
            listentoChat.Checked = b;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sp.setInfo(this.textBox2irchan.Text,this.textBox1server.Text,
              int.Parse(this.textBox4slchan.Text.Trim()),
              int.Parse(this.textBox3port.Text.Trim()));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readData();
        }
        public void loadDefaults()
        {
            this.chan = 1337;
            this.chanel = "#PARplugins";
            this.server = "chat.freenode.net";
            this.port = 6667;
            this.lsn = true;
        }
        public void readData()
        {
            loadDefaults();
            if (File.Exists("SxeChanNew.settings"))
            {
                StreamReader re = File.OpenText("SxeChanNew.settings");
                lsn = false;
                chan = int.Parse(re.ReadLine().Trim());
                chanel = re.ReadLine();
                server = re.ReadLine();
                port = int.Parse(re.ReadLine().Trim());
                try
                {
                    if (re.ReadLine() == "on")
                        lsn = true;
                }
                catch
                {
                }
                re.Close();
            }
            setForm(server, chan.ToString(), chanel, port.ToString(), lsn);
            sp.start();//server, chan.ToString(), chanel, port.ToString(), lsn);
            //girc.setport(port);
            //girc.setserver(server);
            //girc.setchanel(chanel);

        }
        public bool getCheck()
        {
            return listentoChat.Checked;
        }
        public void setCheck(bool b)
        {
            listentoChat.Checked = b;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.openwindow();
        }

        private void textBox2irchan_TextChanged(object sender, EventArgs e)
        {

        }

    }
}