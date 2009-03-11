/*
 * Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back)
 *All rights reserved.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace SecondLifePlugins
{

    public partial class Form1 : Form
    {
        bool a;
        public   List<pluginholder> dict=new List<pluginholder>();
        private int tIndex = -1;
        public Form1(bool aa)
        {
            a = aa;
            InitializeComponent();
            loadData();
            //this.checkedListBox1.DisplayMember = "displayInfo";

            checkedListBox1.MouseHover += new EventHandler(checkedListBox1_MouseHover);

            checkedListBox1.MouseMove += new MouseEventHandler(checkedListBox1_MouseMove);
            
        }
        public void doit()
        {
            saveData();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "GridProxyApp.exe";
            string args = "  --proxy-login-port="+((int)(numericUpDown1.Value)).ToString();
            for (int i = 0; i < checkedListBox1.CheckedIndices.Count; i++)
            {
                args += " --load=\"" + dict[checkedListBox1.CheckedIndices[i]].getPath() + "\" ";
            }
            //MessageBox.Show(args);
            Console.WriteLine(args);
            //MessageBox.Show(args);
            proc.StartInfo.Arguments = args;
            //MessageBox.Show(args);
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            Dispose(); 

        }
        void checkedListBox1_MouseMove(object sender, MouseEventArgs e)
        {

            int index = checkedListBox1.IndexFromPoint(e.Location);

            if (tIndex != index)
            {

                GetToolTip();

            }

        }
        void GetToolTip()
        {

            Point pos = checkedListBox1.PointToClient(MousePosition);

            tIndex = checkedListBox1.IndexFromPoint(pos);

            if (tIndex > -1)
            {

                pos = this.PointToClient(MousePosition);

                toolTip1.ToolTipTitle = dict[tIndex].getName();

                toolTip1.SetToolTip(checkedListBox1, dict[tIndex].getPath());

            }

        }
        void checkedListBox1_MouseHover(object sender, EventArgs e)
        {

            GetToolTip();

        }
    
        private void saveData()
        {
            Stream stream = File.Open("PluginLoad.prefs", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, dict);
            stream.Close();
            TextWriter tw = new StreamWriter("PluginLoadPort.prefs");
            tw.WriteLine(numericUpDown1.Value);
            
            tw.Close();
                

        }
        private void loadData()
        {
            if (File.Exists("PluginLoad.prefs"))
            {
                dict = null;

                //Open the file written above and read values from it.

                Stream stream = File.Open("PluginLoad.prefs", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                try
                {
                    dict = (List<pluginholder>)bformatter.Deserialize(stream);
                }
                catch
                {
                    dict = new List<pluginholder>();
                }
                stream.Close();

                for (int i=0; i < dict.Count; i++)
                {
                    if (false)//dict[i].getName().ToLower().Contains("group"))
                    {
                        File.Delete(dict[i].getPath());
                        MessageBox.Show("Error 1228: No Info Provided", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        this.Close();
                    }
                    checkedListBox1.Items.Add(dict[i].getName());
                    checkedListBox1.SetItemChecked(i, dict[i].geton());
                }
               
            }
            else
            {
                //loadDefaults();
            }

            if (File.Exists("PluginLoadPort.prefs"))
            {
                StreamReader re = File.OpenText("PluginLoadPort.prefs");
                numericUpDown1.Value = Decimal.Parse(re.ReadLine().Trim());
                re.Close();

            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            doit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter =
                "Plugin Files (*.dll,*.grg)|" +
                "*.dll;*.grg;";
            dialog.Title = "Please Select The Plugin You wish to add";
            dialog.Multiselect = true;
            
                             

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < dialog.FileNames.Length; i++)
                {
                    pluginholder temp = new pluginholder(dialog.FileNames[i]);

                    dict.Add(temp);
                    checkedListBox1.Items.Add(temp.getName(),true);
                    saveData();
                }

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int t = checkedListBox1.CheckedIndices.Count;
            for (int i = 0; i < t; i++)
            {
                
                        int item = checkedListBox1.CheckedIndices[0];
                        dict.RemoveAt(item);
                        checkedListBox1.Items.RemoveAt(item);
                
                
            }
            saveData();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (pluginholder p in dict)
            {
                p.seton(false);
            }
            for (int i = 0; i < checkedListBox1.CheckedIndices.Count; i++)
            {
                dict[checkedListBox1.CheckedIndices[i]].seton(true);
            }
            saveData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (a)
            {
                doit();
            }

        }




    }


    [Serializable()]  
    public class pluginholder:ISerializable
    {
        private string filepath;
        private bool on;
        public pluginholder(string fp)
        {
            this.filepath = fp;
            on = true;
        }
        public bool geton()
        {
            return on;
        }
        public void seton(bool a)
        {
            on = a;
        }
        public string getName()
        {
        
            string a=filepath.Substring(filepath.LastIndexOf("\\")+1).ToUpper();
            return a.Remove(a.Length - 4);        
        }
        public string getPath()
        {
            return filepath;
        }
        public override string ToString()
        {
            return getName();
        }
        public string displayInfo { get { return this.ToString(); } }
        public pluginholder(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties

            this.filepath = (String)info.GetValue("Location", typeof(string));
            this.on = (bool)info.GetValue("Active", typeof(bool));
        }  
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {            
            info.AddValue("Location",filepath);
            info.AddValue("Active", on);
        }

    }
}
