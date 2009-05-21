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
    public partial class IMHistForm1 : Form
    {
        private IMLocatePlugin imp;
        public IMHistForm1(IMLocatePlugin im)
        {
            imp = im;
            InitializeComponent();
        }
        public void show(bool b)
        {
            checkBox1enabled.Checked = b;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readData();
        }
        public void readData()
        {
            bool enabled = true;
            if (File.Exists("IMLocate.settings"))
            {
                StreamReader re = File.OpenText("IMLocate.settings");
                if (re.ReadLine() != "Enabled")
                    enabled = false;


                re.Close();
            }

            show(enabled);
        }
        
        private void saveData()
        {
            TextWriter tw = new StreamWriter("IMLocate.settings");
            if (checkBox1enabled.Enabled) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.Close();
        }
        public void addListItem(string s)
        {
            listBox1.Items.Add(s);
            
        }


        private void checkBox1enabled_CheckedChanged_1(object sender, EventArgs e)
        {
            saveData();
            imp.setEnabled(checkBox1enabled.Checked);
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Clipboard.SetText(listBox1.SelectedItem.ToString());
            imp.SendUserAlert(listBox1.SelectedItem.ToString());
        }
    }
}
