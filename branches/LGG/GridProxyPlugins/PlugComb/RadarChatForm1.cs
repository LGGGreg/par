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
    public partial class RadarChatForm1 : Form
    {
        RadarChatPlugin rc;
        public RadarChatForm1(RadarChatPlugin r)
        {
            rc = r;

            InitializeComponent();
        }
        public void readData()
        {
            bool enabled = true;
            string delim = ",";
            int chan = -777777777;
            try
            {
                if (File.Exists("radarchat.settings"))
                {
                    StreamReader re = File.OpenText("radarchat.settings");
                    if (re.ReadLine() != "Enabled")
                        enabled = false;
                    chan = int.Parse(re.ReadLine().ToString()); ;
                    delim = re.ReadLine();


                    re.Close();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                checkBox1.Checked = enabled;
                textBox1.Text = delim;
                numericUpDown1.Value = chan;
            }
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("radarchat.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.WriteLine(numericUpDown1.Value.ToString());
            tw.WriteLine(textBox1.Text);
            tw.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            saveData(); 
        }
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
