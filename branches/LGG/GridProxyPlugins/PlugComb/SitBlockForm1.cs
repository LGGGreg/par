using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PubComb
{
    public partial class SitBlockForm1 : Form
    {
        SitBlockPlugin s;
        public SitBlockForm1(SitBlockPlugin sb)
        {
            s = sb;
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
        }
        public void readData()
        {
            bool pass = false;
            
            if (File.Exists("sitBlock.settings"))
            {
                StreamReader re = File.OpenText("sitBlock.settings");
                if (re.ReadLine() == "Enabled")
                    pass = true;
                
                re.Close();

            }
            checkBox1.Checked = pass;
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("sitBlock.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            
            tw.Close();
        }
    }
}
