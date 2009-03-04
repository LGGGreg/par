using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenMetaverse;

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
        private void loadDefaults()
        {
            cl.uid2name.Add(new UUID("c252d89d-6f7c-7d90-f430-d140d2e3fbbe"), "VLife");
            cl.uid2name.Add(new UUID("adcbe893-7643-fd12-f61c-0b39717e2e32"), "tyk3n");
            cl.uid2name.Add(new UUID("f3fd74a6-fee7-4b2f-93ae-ddcb5991da04"), "PSL");
            cl.uid2name.Add(new UUID("77662f23-c77a-9b4d-5558-26b757b2144c"), "PSL");
            cl.uid2name.Add(new UUID("5aa5c70d-d787-571b-0495-4fc1bdef1500"), "LGG");
            cl.uid2name.Add(new UUID("8183e823-c443-2142-6eb6-2ab763d4f81c"), "Day Oh");
            cl.uid2name.Add(new UUID("0f6723d2-5b23-6b58-08ab-308112b33786"), "Cryo");
            cl.uid2name.Add(new UUID("1635887e-bffc-5e1d-ac98-909d5a583c53"), "NoobLife");
            cl.uid2name.Add(new UUID("0bcd5f5d-a4ce-9ea4-f9e8-15132653b3d8"), "MoyMix");
            cl.uid2name.Add(new UUID("c228d1cf-4b5d-4ba8-84f4-899a0796aa97"), "Emerald");
            
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
            loadDefaults();
            bool a = true;
            if (File.Exists("ClientDetection.settings"))
            {
                using (StreamReader r = new StreamReader("ClientDetection.settings"))
                {
                    if (r.ReadLine() == "Disabled")
                    {
                        a = false;
                    }
                    Dictionary<UUID, string> temp = new Dictionary<UUID, string>();
                    foreach(KeyValuePair<UUID,string> k in cl.uid2name)
                    {
                        
                        if (!r.EndOfStream)
                        {
                            temp.Add(k.Key,r.ReadLine());
                        }
                        listBox2.Items.Add(k.Key);
                    }
                    if (temp.Count == cl.uid2name.Count)
                    {
                        cl.uid2name = temp;
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
            foreach (KeyValuePair<UUID, string> k in cl.uid2name)
            {
                tw.WriteLine(k.Value.ToString());
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

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                textBox1uuid.Text = listBox2.SelectedItem.ToString();
                textBox2display.Text = cl.uid2name[new UUID(textBox1uuid.Text)];
            }
        }

        private void textBox1uuid_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2display_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UUID a;
            if (UUID.TryParse(textBox1uuid.Text, out a))
            {
                cl.uid2name[a] = textBox2display.Text;
            }
            saveData();
        }
    }
}
