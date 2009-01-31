using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PubComb
{
    public partial class ClientDetectForm1 : Form
    {
        ClientDetection cl;
        public ClientDetectForm1(ClientDetection c)
        {
            cl = c;
            InitializeComponent();
        }
        public void setList(string[] n)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(n);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public bool Display()
        {
            return checkBox1.Checked;
        }
        public void readData()
        {
            bool a = true;
            if (File.Exists("ClientDetection.settings"))
            {
                using (StreamReader r = new StreamReader("ClientDetection.settings"))
                {
                    if (r.ReadLine() == "Disabled")
                    {
                        a = false;
                    }
                    r.Close();
                }
            }
            checkBox1.Checked=a;

        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("ClientDetection.settings");
            if (checkBox1.Checked)
            {
                tw.WriteLine("Enabled");
            }
            else
            {
                tw.WriteLine("Disabled");
            }
            tw.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
