using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PubComb
{
    public partial class ProTextionForm1 : Form
    {
        public ProTextPlug p;
        public ProTextionForm1(ProTextPlug pp)
        {
            p = pp;
            InitializeComponent();
        }
        public bool getEnabled()
        {
            return checkBox1.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
