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
         public int chan = 1337;
         public string chanel = "#IRCPLUG";
         public string server = "us.undernet.org";
         public int port = 6667;
         public bool lsn = true;
        private SxePlugin sp;
        public Form1(SxePlugin s)
        {
            InitializeComponent();
            sp = s;
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
            this.chanel = "#IRCPLUG";
            this.server = "us.undernet.org";
            this.port = 6667;
            this.lsn = true;
        }
        public void readData()
        {
            loadDefaults();
            if (File.Exists("SxeChan.settings"))
            {
                StreamReader re = File.OpenText("SxeChan.settings");
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

    }
}