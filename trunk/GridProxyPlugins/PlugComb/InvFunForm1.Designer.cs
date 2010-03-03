namespace PubCombN
{
    partial class InvFunForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.GregTreeInv = new GregInventoryTree();
            this.panel3 = new System.Windows.Forms.Panel();
            this.updateinv = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 10);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 254);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.GregTreeInv);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 57);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(284, 197);
            this.panel4.TabIndex = 1;
            // 
            // GregTreeInv
            // 
            this.GregTreeInv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GregTreeInv.Location = new System.Drawing.Point(0, 0);
            this.GregTreeInv.Name = "GregTreeInv";
            this.GregTreeInv.Size = new System.Drawing.Size(284, 197);
            this.GregTreeInv.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.updateinv);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(284, 57);
            this.panel3.TabIndex = 0;
            // 
            // updateinv
            // 
            this.updateinv.Location = new System.Drawing.Point(13, 18);
            this.updateinv.Name = "updateinv";
            this.updateinv.Size = new System.Drawing.Size(169, 23);
            this.updateinv.TabIndex = 0;
            this.updateinv.Text = "Update Inventory";
            this.updateinv.UseVisualStyleBackColor = true;
            this.updateinv.Click += new System.EventHandler(this.updateinv_Click);
            // 
            // InvFunForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "InvFunForm1";
            this.Text = "InvFunForm1";
            this.Load += new System.EventHandler(this.InvFunForm1_Load);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        public GregInventoryTree GregTreeInv;
        private System.Windows.Forms.Button updateinv;
    }
}