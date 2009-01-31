namespace PubComb
{
    partial class HandiForm1
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
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1enabled = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.checkBox1enabled);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(374, 106);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "No Clue Who Made this first, thanks to Day for crazy binnary parsing code!\r\nLGG:G" +
                "ui";
            // 
            // checkBox1enabled
            // 
            this.checkBox1enabled.AutoSize = true;
            this.checkBox1enabled.Checked = true;
            this.checkBox1enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1enabled.Location = new System.Drawing.Point(13, 13);
            this.checkBox1enabled.Name = "checkBox1enabled";
            this.checkBox1enabled.Size = new System.Drawing.Size(82, 17);
            this.checkBox1enabled.TabIndex = 0;
            this.checkBox1enabled.Text = "TP Enabled";
            this.checkBox1enabled.UseVisualStyleBackColor = true;
            this.checkBox1enabled.CheckedChanged += new System.EventHandler(this.checkBox1enabled_CheckedChanged);
            // 
            // HandiForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(374, 147);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "HandiForm1";
            this.Text = "l";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1enabled;
        private System.Windows.Forms.Label label2;
    }
}