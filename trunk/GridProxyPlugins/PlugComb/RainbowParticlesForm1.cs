/*
 * Copyright (c) 2009, Day Oh & Gregory Hendrickson (LordGregGreg Back)
 * All Beams But SL Particles by day oh
 * Gui and settings by LGG
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PubComb
{
    public partial class RainbowParticlesForm1 : Form
    {

        public int xtrafx_Index = 0;
        public RainbowParticlesPlugin rp;
        public static String xtrafxString;
        public xtrafxMode Mode = xtrafxMode.Quad;

        public RainbowParticlesForm1(RainbowParticlesPlugin r)
        {
            rp = r;
            InitializeComponent();
        }
        public void setColorFromString(string input)
        {
            string[] colors = input.ToLower().Split(new char[3] { ' ', ',', ':' });
            xtrafxString = input;
            rp.plug.SharedInfo.rainbow= new byte[colors.Length][];
            rp.plug.SayToUser("The colors we got were " + input + " and we got " + colors.Length.ToString() + " colors");
            lock (listBox1.Items)
            {
                listBox1.Items.Clear();
                for (int i = 0; i < colors.Length; i++)
                {
                    listBox1.Items.Add(colors[i]);

                    switch (colors[i])
                    {
                        case "red":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x00, 0x00, 0xFF };
                            break;
                        case "orange":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x80, 0x00, 0xFF };
                            break;
                        case "yellow":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0xFF, 0x00, 0xFF };
                            break;
                        case "lime":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x80, 0xFF, 0x00, 0xFF };
                            break;
                        case "green":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0x00, 0xFF };
                            break;
                        case "turquoise":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0x80, 0xFF };
                            break;
                        case "cyan":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0xFF, 0xFF };
                            break;
                        case "blue":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0xFF, 0xFF };
                            break;
                        case "purple":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x80, 0x00, 0xFF, 0xFF };
                            break;
                        case "pink":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x00, 0xFF, 0xFF };
                            break;
                        case "black":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0x00, 0xFF };
                            break;
                        case "white":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                            break;
                        default:
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0x00, 0xFF };
                            rp.plug.SayToUser("Unknown color: " + colors[i] + " set to black");
                            break;
                    }
                }
            }
            xtrafx_Index = 0;
            saveData();
        }

        private void saveData()
        {
            TextWriter tw = new StreamWriter("SelectionBeam.settings");
            tw.WriteLine(xtrafxString);
            if (Mode == xtrafxMode.Quad) tw.WriteLine("Quad");
            else if (Mode == xtrafxMode.Quad) tw.WriteLine("Single");
            else if (Mode == xtrafxMode.Speak) tw.WriteLine("Speak");
            else if (Mode == xtrafxMode.Circles) tw.WriteLine("Circles");
            else  tw.WriteLine("Disabled");
            tw.Close();
        }
        public void readData()
        {
            string t = "red orange yellow lime green turquoise blue purple pink";
            Mode = xtrafxMode.Quad;
            if (File.Exists("SelectionBeam.settings"))
            {
                StreamReader re = File.OpenText("SelectionBeam.settings");
                t = re.ReadLine();
                string type = re.ReadLine().ToLower();
                if (type.Equals("quad"))
                    Mode = xtrafxMode.Quad;
                else if (type.Equals("single"))
                    Mode = xtrafxMode.Single;
                else if (type.Equals("speak"))
                    Mode = xtrafxMode.Speak;
                else if (type.Equals("circles"))
                    Mode = xtrafxMode.Circles;
                else if (type.Equals("disabled"))
                    Mode = xtrafxMode.Disabled;
                re.Close();
            }
            setMode(Mode);
            setColorFromString(t);
            comboBox1.SelectedIndex = 0;
        }
        public void setMode(xtrafxMode m)
        {
            Mode = m;
            if (Mode == xtrafxMode.Single)
            {
                //radioButton2single.Checked = true;
            }
            else if (Mode == xtrafxMode.Quad)
            {
                //radioButton1quad.Checked = true;
            }
            else if (Mode == xtrafxMode.Circles)
            {
                //radioButton4circles.Checked = true;
            }
            else if (Mode == xtrafxMode.Speak)
            {
                //radioButtonspeak.Checked = true;
            }
            else if (Mode == xtrafxMode.Disabled)
            {
                //radioButton5nothing.Checked = true;
            }
            saveData();
        }
        private void radioButton2single_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Single);
        }
        public void setColors(String colors)
        {
            setColorFromString(colors);
        }
        public void doList()
        {
            string temp = "";
            for(int i =0;i<listBox1.Items.Count;i++)
            {
                temp += listBox1.Items[i].ToString() + " ";
            }
            if(temp!="")
            setColorFromString(temp.Remove(temp.Length-1));
        }


        private void button7delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Remove(listBox1.Items[listBox1.SelectedIndex]);
                
                doList();
            }
        }
        public void MoveUp()
        {

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (i == listBox1.SelectedIndex)//identify the selected item
                {
                    //swap with the top item(move up)
                    if (i > 0)
                    {
                        string bottom = listBox1.Items[i].ToString();
                        listBox1.Items.Remove(bottom);
                        listBox1.Items.Insert(i - 1, bottom);
                        listBox1.SelectedIndex = (i - 1);
                    }
                }
            }
        }
        //Moves the selected items one level down
        public void MoveDown()
        {
            int startindex = listBox1.Items.Count - 1;
            for (int i = startindex; i > -1; i--)
            {
                if (i == listBox1.SelectedIndex)//identify the selected item
                {
                    //swap with the lower item(move down)
                    if (i < startindex)
                    {
                        string bottom = listBox1.Items[i].ToString();
                        listBox1.Items.Remove(bottom);
                        listBox1.Items.Insert(i + 1, bottom);
                        listBox1.SelectedIndex = (i + 1);
                    }

                }
            }
        }
        private void button6moveup_Click(object sender, EventArgs e)
        {
            /*
            lock (listBox1.Items)
            {
                Object a = listBox1.SelectedValue;
                int i = listBox1.SelectedIndex;
                if (i < 1) return;
                listBox1.Items.RemoveAt(i);
                listBox1.Items.Insert(i - 1, a);
            }*/
            MoveUp();
            doList();
        }
        private void button8movedown_Click(object sender, EventArgs e)
        {
            /*
            lock (listBox1.Items)
            {
                Object a = listBox1.SelectedValue;
                int i = listBox1.SelectedIndex;
                if (i == listBox1.Items.Count - 1) return;
                listBox1.Items.RemoveAt(i);
                listBox1.Items.Insert(i + 1, a);
            }*/
            MoveDown();
            doList();
        }
        private void radioButton1quad_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Quad);
        }
        private void radioButtonspeak_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Speak);
        }

        private void radioButton4circles_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Circles);
        }

        private void radioButton5nothing_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Disabled);
        }

        private void radioButton6nada_CheckedChanged(object sender, EventArgs e)
        {
            setMode(xtrafxMode.Disabled);
        }

        private void button5add_Click_1(object sender, EventArgs e)
        {
            lock (listBox1.Items)
            {
                listBox1.Items.Add(comboBox1.Text.ToString());
            }
            doList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
