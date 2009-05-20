using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PubComb
{
    public partial class AweseomeSauceForm1 : Form
    {
        AwesomeSauce a;
        public AweseomeSauceForm1(AwesomeSauce aw)
        {
            a = aw;
            InitializeComponent();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        public void log(string text, Color color, Color bcolor)
        {
            richTextBox1.AppendText(text + Environment.NewLine);
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectionBackColor = bcolor;
            richTextBox1.SelectedText = text;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
