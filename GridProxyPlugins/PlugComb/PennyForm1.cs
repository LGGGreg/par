/*
 * Copyright (c) 2009,  Day Oh
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

namespace PubCombN
{
    public partial class PennyForm1 : Form
    {
        public PennyPlugin pp;
        public PennyForm1(PennyPlugin p)
        {
            pp = p; 
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //readData();
        }

        public bool getChecked()
        {
            return checkBox1.Checked;
        }

        
        public void readData()
        {
            bool pass = true;
            
            if (File.Exists("penny.settings"))
            {
                StreamReader re = File.OpenText("penny.settings");
                if (re.ReadLine() != "Enabled")
                    pass = false;
                            re.Close();
               
            }
            checkBox1.Checked = pass;

        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("penny.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.Close();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            saveData();
        }
    }
}
