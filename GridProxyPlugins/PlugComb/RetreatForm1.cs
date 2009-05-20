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
    public partial class RetreatForm1 : Form
    {
        RetreatPlugin rp;
        public RetreatForm1(RetreatPlugin r)
        {
            rp = r;
            InitializeComponent();
        }
        public void readData()
        {
            bool enabled = true;
            if (File.Exists("Retreat.settings"))
            {
                StreamReader re = File.OpenText("Retreat.settings");
                if (re.ReadLine() != "Enabled")
                    enabled = false;


                re.Close();
            }

            checkBox1.Checked = enabled;
        }

        private void saveData()
        {
            TextWriter tw = new StreamWriter("Retreat.settings");
            if (checkBox1.Checked) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveData();
            
        }
        public bool getEnabled()
        {
            return checkBox1.Checked;
        }
    }
}
