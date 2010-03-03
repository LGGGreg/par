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
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
namespace PubCombN
{
    
    public partial class ShadowForm1 : Form
    {
        private ShadowPlugin shadowPlug;
        public bool needSave = false;
        private Thread formthread;
        private ShadowPrefsForm2 form;

        public ShadowForm1(ShadowPlugin shad)
        {

            this.shadowPlug = shad;
            InitializeComponent();
            //webBrowser1.Navigate("about:blank");

            

        }
        public void initializeNames()
        {
            this.Text = shadowPlug.brand;
            saveData();

        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("Shadow.Key");
            tw.WriteLine(this.textBox2key.Text);
            tw.WriteLine(this.shadowPlug.brand);
            tw.WriteLine(this.shadowPlug.indicator);
            tw.WriteLine(this.shadowPlug.trigger);
            tw.Close();
        }
        public void readData()
        {
            //MessageBox.Show("reading encryption data");
            if (File.Exists("Shadow.Key"))
            {
                StreamReader re = File.OpenText("Shadow.Key");
                try
                {
                    this.textBox2key.Text = re.ReadLine();
                    this.shadowPlug.brand= re.ReadLine();
                    this.shadowPlug.indicator=re.ReadLine();
                    this.shadowPlug.trigger = re.ReadLine();
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Error Reading "+"Shadow.Key"+" file, corrupt data found\nReverting to defailt data", "Error reading Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    loadDefaults();
                }
                re.Close();
            }
            else
            {
                loadDefaults();
            }
        }
        private void loadDefaults()
        {
            this.textBox2key.Text = "Enter your key here";
            shadowPlug.indicator = "[E]";
            shadowPlug.brand = "Shadow";
            shadowPlug.trigger = "\\";
            
        }
        public byte[] getKey()
        {
            byte[] temp  =	{0x01,0x23,0x45,0x67,0x89,0xAB,0xCD,0xEF,0xFE,0xDC,0xBA,0x98,0x76,0x54,0x32,0x10,0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88,0x99,0xAA,0xBB,0xCC,0xDD,0xEE,0xFF};
            int count = Utils.StringToBytes(textBox2key.Text).Length;
            if(count>256)count=256;
            
            Buffer.BlockCopy(Utils.StringToBytes(textBox2key.Text), 0, temp, 0, count);
            return temp;
        }

        public void log(string text, Color color)
        {
            richTextBox1.AppendText(text+Environment.NewLine);
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectedText = text;

            if (needSave)
            {
                saveData();
                needSave = false;
            }
            //doc.Write("<span style=\"font-family : Courier;color: "+color+";\">"+text+"</span><p>");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        public void setBox(bool set)
        {
            checkBox1enabled.Checked = set;
        }

        private void textBox2key_TextChanged(object sender, EventArgs e)
        {
           this.needSave = true;
        }

        public void setKey(string t)
        {
            this.textBox2key.Text = t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formthread = new Thread(new ThreadStart(delegate()
            {
                form = new ShadowPrefsForm2(this, this.shadowPlug);
                Application.Run(form);
            }));
            formthread.SetApartmentState(ApartmentState.STA);
            formthread.Start();
        }

        private void button2about_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This program was made by LordGregGreg for the purpose of allowing\n" +
                              "Secure communications in an insecure world.  Messages sent with\n" +
                              "This plugin are encrypted BEFORE they are sent from your computer\n" +
                              "And decrypted AFTER the client computer recives them, (and before\n" +
                              "the client displays them).  This makes it impossible for chat\n" +
                              "interception on any level.\nSpecial Thanks to \"Philip Linden\" (yeah, thats not his actual sl name)\n" +
                        "and the OpenMetaverse project, and all it's contributors.\n" +
                        "Also special thanks to ManyMonkeys for coding the TwoFish  and also the makers of TwoFish!\n\n"+
                 "If you plan to decompile and use this code, that is fine.  But please leave this message and access to it intact, that is all I ask. :)\n\n"+
            "."
            ,"Second Life Encryption - LordGregGreg",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    
        }

        private void checkBox1enabled_CheckedChanged_1(object sender, EventArgs e)
        {
            shadowPlug.setEnabled(checkBox1enabled.Checked);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readData();
            initializeNames();
        }
    }
}
