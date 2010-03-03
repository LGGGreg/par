namespace PubCombN
{
    partial class CinderForm1
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
            this.checkBox1trans = new System.Windows.Forms.CheckBox();
            this.checkBox1mod = new System.Windows.Forms.CheckBox();
            this.checkBox1copy = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2free = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1lossless = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.checkBox1lossless);
            this.panel1.Controls.Add(this.checkBox1trans);
            this.panel1.Controls.Add(this.checkBox1mod);
            this.panel1.Controls.Add(this.checkBox1copy);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.ForeColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 46);
            this.panel1.TabIndex = 1;
            // 
            // checkBox1trans
            // 
            this.checkBox1trans.AutoSize = true;
            this.checkBox1trans.Checked = true;
            this.checkBox1trans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1trans.Location = new System.Drawing.Point(121, 26);
            this.checkBox1trans.Name = "checkBox1trans";
            this.checkBox1trans.Size = new System.Drawing.Size(53, 17);
            this.checkBox1trans.TabIndex = 7;
            this.checkBox1trans.Text = "Trans";
            this.checkBox1trans.UseVisualStyleBackColor = true;
            // 
            // checkBox1mod
            // 
            this.checkBox1mod.AutoSize = true;
            this.checkBox1mod.Checked = true;
            this.checkBox1mod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1mod.Location = new System.Drawing.Point(68, 26);
            this.checkBox1mod.Name = "checkBox1mod";
            this.checkBox1mod.Size = new System.Drawing.Size(47, 17);
            this.checkBox1mod.TabIndex = 6;
            this.checkBox1mod.Text = "Mod";
            this.checkBox1mod.UseVisualStyleBackColor = true;
            // 
            // checkBox1copy
            // 
            this.checkBox1copy.AutoSize = true;
            this.checkBox1copy.Checked = true;
            this.checkBox1copy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1copy.Location = new System.Drawing.Point(12, 27);
            this.checkBox1copy.Name = "checkBox1copy";
            this.checkBox1copy.Size = new System.Drawing.Size(50, 17);
            this.checkBox1copy.TabIndex = 5;
            this.checkBox1copy.Text = "Copy";
            this.checkBox1copy.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.ForeColor = System.Drawing.Color.Red;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(247, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2free);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.buttonLoad);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(247, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 46);
            this.panel2.TabIndex = 3;
            // 
            // button2free
            // 
            this.button2free.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2free.Location = new System.Drawing.Point(44, 0);
            this.button2free.Name = "button2free";
            this.button2free.Size = new System.Drawing.Size(80, 46);
            this.button2free.TabIndex = 3;
            this.button2free.Text = "Save to SL!";
            this.button2free.UseVisualStyleBackColor = true;
            this.button2free.Click += new System.EventHandler(this.button2free_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(124, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "About/Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLoad.Location = new System.Drawing.Point(0, 0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(44, 46);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click_1);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 40);
            this.panel3.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(447, 261);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // checkBox1lossless
            // 
            this.checkBox1lossless.AutoSize = true;
            this.checkBox1lossless.Checked = true;
            this.checkBox1lossless.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1lossless.Location = new System.Drawing.Point(180, 26);
            this.checkBox1lossless.Name = "checkBox1lossless";
            this.checkBox1lossless.Size = new System.Drawing.Size(70, 17);
            this.checkBox1lossless.TabIndex = 8;
            this.checkBox1lossless.Text = "LossLess";
            this.checkBox1lossless.UseVisualStyleBackColor = true;
            // 
            // CinderForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 347);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "CinderForm1";
            this.Text = "Cinderella - Textures That Only Last Till Midnight";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1trans;
        private System.Windows.Forms.CheckBox checkBox1mod;
        private System.Windows.Forms.CheckBox checkBox1copy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2free;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBox1lossless;

    }
}