using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PubComb
{
    public partial class FileProtectForm1 : Form
    {
        
        //FileProtectPlugin fp;
        public FileProtectForm1(FileProtectPlugin p)
        {
          //  fp = p;
            InitializeComponent();
        }
        public void readData()
        {
            bool enabled = true;
            try
            {
                if (File.Exists("fileProtect.settings"))
                {
                    StreamReader re = File.OpenText("fileProtect.settings");
                    if (re.ReadLine() != "Enabled")
                        enabled = false;
                    

                    re.Close();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                checkBox1.Checked = enabled;
                myUpdate();
            }
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("fileProtect.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            
            tw.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public bool getChecked()
        {
            return checkBox1.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
            myUpdate();
        }
        public void myUpdate()
        {
            
          
        }
    }
}
