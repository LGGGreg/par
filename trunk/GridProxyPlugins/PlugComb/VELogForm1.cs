using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PubComb
{
    public partial class VELogForm1 : Form
    {
        ViewerEffectLogPlugin v;
        public VELogForm1(ViewerEffectLogPlugin ve)
        {
            v = ve;
            InitializeComponent();
        }

        private void VELogForm1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage15_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1on_Click(object sender, EventArgs e)
        {
            checkBox10sphere.Checked = checkBox11spiral.Checked = checkBox12text.Checked = checkBox13trail.Checked = checkBox14beam.Checked = checkBox15animation.Checked = checkBox16animla.Checked = checkBox1cloth.Checked = checkBox2connector.Checked = checkBox3edit.Checked = checkBox4flexble.Checked = checkBox5glow.Checked = checkBox6icon.Checked = checkBox7lookat.Checked = checkBox8point.Checked = checkBox9pointat.Checked
                = true;
        }

        private void button1off_Click(object sender, EventArgs e)
        {
            checkBox10sphere.Checked = checkBox11spiral.Checked = checkBox12text.Checked = checkBox13trail.Checked = checkBox14beam.Checked = checkBox15animation.Checked = checkBox16animla.Checked = checkBox1cloth.Checked = checkBox2connector.Checked = checkBox3edit.Checked = checkBox4flexble.Checked = checkBox5glow.Checked = checkBox6icon.Checked = checkBox7lookat.Checked = checkBox8point.Checked = checkBox9pointat.Checked
                = false;
        }
    }
}
