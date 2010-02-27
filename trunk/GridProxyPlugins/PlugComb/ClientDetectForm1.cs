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
            cl.uid2name.Add(new UUID("0bcd5f5d-a4ce-9ea4-f9e8-15132653b3d8"), "MoyMix");
            cl.uid2name.Add(new UUID("0f6723d2-5b23-6b58-08ab-308112b33786"), "CryoLife");
            cl.uid2name.Add(new UUID("11ad2452-ce54-8d65-7c23-05589b59f516"), "VerticalLife");
            cl.uid2name.Add(new UUID("1c29480c-c608-df87-28bb-964fb64c5366"), "Gemini");
            cl.uid2name.Add(new UUID("2a9a406c-f448-68f2-4e38-878f8c46c190"), "Meerkat");
            cl.uid2name.Add(new UUID("2c9c1e0b-e5d1-263e-16b1-7fc6d169f3d6"), "PhoxSL");
            cl.uid2name.Add(new UUID("3ab7e2fa-9572-ef36-1a30-d855dbea4f92"), "VerticalLife");
            cl.uid2name.Add(new UUID("3da8a69a-58ca-023f-2161-57f2ab3b5702"), "Operator");
            cl.uid2name.Add(new UUID("4da16427-d81e-e816-f346-aaf4741b8056"), "iLife");
            cl.uid2name.Add(new UUID("4e8dcf80-336b-b1d8-ef3e-08dacf015a0f"), "Sapphire");
            cl.uid2name.Add(new UUID("5262d71a-88f7-ef40-3b15-00ea148ab4b5"), "Gemini.Bot");
            cl.uid2name.Add(new UUID("58a8b7ec-1455-7162-5d96-d3c3ead2ed71"), "VerticalLife");
            cl.uid2name.Add(new UUID("5aa5c70d-d787-571b-0495-4fc1bdef1500"), "LGGproxy");
            cl.uid2name.Add(new UUID("77662f23-c77a-9b4d-5558-26b757b2144c"), "PSL");
            cl.uid2name.Add(new UUID("7c4d47a3-0c51-04d1-fa47-e4f3ac12f59b"), "CryoLife");
            cl.uid2name.Add(new UUID("8183e823-c443-2142-6eb6-2ab763d4f81c"), "DayOhproxy");
            cl.uid2name.Add(new UUID("81b3e921-ee31-aa57-ff9b-ec1f28e41da1"), "Infinity");
            cl.uid2name.Add(new UUID("841ef25b-3b90-caf9-ea3d-5649e755db65"), "VerticalLife");
            cl.uid2name.Add(new UUID("872c0005-3095-0967-866d-11cd71115c22"), "Copybotter");
            cl.uid2name.Add(new UUID("9422e9d7-7b11-83e4-6262-4a8db4716a3b"), "BetaLife");
            cl.uid2name.Add(new UUID("adcbe893-7643-fd12-f61c-0b39717e2e32"), "tyk3n");
            cl.uid2name.Add(new UUID("b6820989-bf42-ff59-ddde-fd3fd3a74fe4"), "Meerkat");
            cl.uid2name.Add(new UUID("c252d89d-6f7c-7d90-f430-d140d2e3fbbe"), "VLife");
            cl.uid2name.Add(new UUID("c5b570ca-bb7e-3c81-afd1-f62646b20014"), "KungFu");
            cl.uid2name.Add(new UUID("ccb509cf-cc69-e569-38f1-5086c687afd1"), "Ruby");
            cl.uid2name.Add(new UUID("ccda2b3b-e72c-a112-e126-fee238b67218"), "Emerald");
            cl.uid2name.Add(new UUID("d3eb4a5f-aec5-4bcb-b007-cce9efe89d37"), "rivlife");
            cl.uid2name.Add(new UUID("e52d21f7-3c8b-819f-a3db-65c432295dac"), "CryoLife");
            cl.uid2name.Add(new UUID("e734563e-1c31-2a35-3ed5-8552c807439f"), "VerticalLife");
            cl.uid2name.Add(new UUID("f12457b5-762e-52a7-efad-8f17f3b022ee"), "Anti-Life");
            cl.uid2name.Add(new UUID("f3fd74a6-fee7-4b2f-93ae-ddcb5991da04"), "PSL");
            cl.uid2name.Add(new UUID("f5a48821-9a98-d09e-8d6a-50cc08ba9a47"), "NeilLife");
            cl.uid2name.Add(new UUID("f5feab57-bde5-2074-97af-517290213eaa"), "Onyx");
            cl.uid2name.Add(new UUID("ffce04ff-5303-4909-a044-d37af7ab0b0e"), "Corgi");
            
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
