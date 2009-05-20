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
    public partial class CapsForm1 : Form
    {
        public DisableCapsPlugin dcp;
        public CapsForm1(DisableCapsPlugin l)
        {
            dcp = l;
            InitializeComponent();
            
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readData();
            Console.WriteLine("edding data");


        }
        public void readData()
        {
            
            if (File.Exists("DisableCaps.settings"))
            {
                using (StreamReader r = new StreamReader("DisableCaps.settings"))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        move(line, true);                        
                    }
                    r.Close();
                }
                


            }
            updateBlocked();
            
        }
        private void move(object item, bool inllist1)
        {
           
            lock (listBox1.Items)
            {
                lock (listBox2.Items)
                {

                    if (inllist1)
                    {
                        //listBox1.Items.Remove(item);
                        if(listBox1.Items.Contains(item))
                        {

                            //Console.WriteLine("start remove ok");
                            listBox1.Items.Remove(item);
                            //Console.WriteLine("remove went ok");
                        
                            listBox2.Items.Add(item);

                            //Console.WriteLine("add went ok");
                        }
                    }
                    else
                    {
                        if (listBox2.Items.Contains(item))
                        {
                            listBox1.Items.Add(item);
                            listBox2.Items.Remove(item);
                        }
                    }
                }
            }
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("DisableCaps.settings");
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                tw.WriteLine(listBox2.Items[i].ToString());
            }
            tw.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
        } 

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void updateBlocked()
        {
            lock (dcp.proxy.BlockCaps)
            {
                dcp.proxy.BlockCaps.Clear();
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    dcp.proxy.BlockCaps.Add(listBox2.Items[i].ToString());
                }
                saveData();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                move(listBox1.SelectedItem, true);
                updateBlocked();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                move(listBox2.SelectedItem, false);
                updateBlocked();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



    }
}
