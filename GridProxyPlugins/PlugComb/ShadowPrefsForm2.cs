using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PubCombN
{
    public partial class ShadowPrefsForm2 : Form
    {
        private ShadowPlugin shadowPlug;
        private ShadowForm1 form1;

        public ShadowPrefsForm2(ShadowForm1 f, ShadowPlugin shad)
        {
            InitializeComponent();
            this.form1 = f;
            this.shadowPlug = shad;
            this.textBox3Brand.Text = shad.brand;
            this.textBox2Trigger.Text = shad.trigger;
            this.textBox1Indicator.Text = shad.indicator;
            this.Text = shad.brand;
        }

        private void button1prefix_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Warning!\nBy Changing this value you will prevent everyone from decrypting your messages\n" +
                "AND you will not be able to decrypt anyones messages\n" +
                "even if they are using shadow and have the same key!!!\n\n" +
                "They must also have the same brand.  Changing this will remove compatablitiy with older version of shadow 100%\n" +
                "Channging this wil change the commands, the preifx, and even the window titles, use with care\n"+
                "If you are sure you want to continue... press OK", "WARNING!!! Atempting to change brand!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.textBox3Brand.Enabled = true;
            }

        }

        private void button3cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2submit_Click(object sender, EventArgs e)
        {
            this.shadowPlug.brand = this.textBox3Brand.Text;
            this.shadowPlug.trigger =this.textBox2Trigger.Text ;
            this.shadowPlug.indicator = this.textBox1Indicator.Text;
            MessageBox.Show("Setting Saved :D", shadowPlug.brand
            +"-Settings Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form1.initializeNames();
            this.Close();
        }


    }
}
