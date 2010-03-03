using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PubCombN
{
    public partial class UsefulForm1 : Form
    {
        private useful use;
        public UsefulForm1(useful inuse)
        {
            use = inuse;
            InitializeComponent();
        }
    }
}
