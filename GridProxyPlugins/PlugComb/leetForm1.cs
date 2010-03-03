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


namespace PubCombN
{
    public partial class leetForm1 : Form
    {
        LeetPlugin lp;
        
        public leetForm1(LeetPlugin a)
        {
            lp = a;
            InitializeComponent();
            listBox1.SelectedIndex = 0;
        }

        public void setBox(bool a)
        {
            checkBox1.Checked = a;
        }

       
        public void setLevel(int l)
        {
            listBox1.SelectedIndex = l;
        }
        public int get1337()
        {
            return listBox1.SelectedIndex;
        }
        public bool getcheck()
        {
            return checkBox1.Checked;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            //if (checkBox1.Checked && i > 0)
            //    lp.SendUserAlert("1337 level set to " + i.ToString());
            //else lp.SendUserAlert("1337 speak disabled");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lp.setEnabled(checkBox1.Checked);
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

       
    }
}
