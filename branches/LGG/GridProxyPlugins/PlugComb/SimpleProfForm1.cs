/*
 * Copyright (c) 2009,  Gregory Hendrickson (LordGregGreg Back)
 * 
 *All rights reserved
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

namespace PubComb
{
    public partial class SimpleProfForm1 : Form
    {
        public ProfileFunPlugin pf;
        public SimpleProfForm1(ProfileFunPlugin p)
        {
            pf = p;
            InitializeComponent();
        }
        public void readData()
        {
            bool t = false;
            string web = "http://the-diy-life.com/super1337hax/LGGRedirect.php?loc=";
            if (File.Exists("profileweb.settings"))
            {
                try
                {
                    StreamReader re = File.OpenText("profileweb.settings");
                    if (re.ReadLine() == "Enabled")
                        t = true;
                
                    string at = re.ReadLine();
                    if (!string.IsNullOrEmpty(at))
                        web = at;
                }
                catch(Exception a)
                {
                    web = "http://the-diy-life.com/super1337hax/LGGRedirect.php?loc=";
                }


            }
            textBox1.Text = web;
            checkBox1.Checked=t;
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("profileweb.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.WriteLine(textBox1.Text);
            tw.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        public void setStuff(string img, string rlimg, string about, string rlabout, string web)
        {
            textBox1about.Text = about;
            textBox1image.Text = img;
            textBox1rlabout.Text = rlabout;
            textBox1rlimage.Text = rlimg;
            textBox1website.Text = web;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1website.Enabled = false;
            }
            else
            {
                textBox1website.Enabled = true;
            }
        }
        public bool autoupdate()
        {
            return checkBox1.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pf.updateProfile(textBox1image.Text, textBox1rlimage.Text, textBox1about.Text, textBox1rlabout.Text, textBox1website.Text);
            saveData();

        }
        public void updateWeb(string web)
        {
            pf.updateProfile(textBox1image.Text, textBox1rlimage.Text, textBox1about.Text, textBox1rlabout.Text, web);

        }
        public string getBase()
        {
            return textBox1.Text;
        }

        private void SimpleProfForm1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
    }

     
}
