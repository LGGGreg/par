using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PubCombN
{
    public partial class InvFunForm1 : Form
    {
        public InvFunPlugin invf;
        public InvFunForm1(InvFunPlugin iii)
        {
            InitializeComponent();

            invf = iii;
            GregTreeInv.setPlugin(iii);
        }

        private void InvFunForm1_Load(object sender, EventArgs e)
        {

        }

        private void updateinv_Click(object sender, EventArgs e)
        {
            this.GregTreeInv.setRoot(invf.plugin.frame.InventoryRoot);
        }

        private void InvFunForm1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
