namespace PubCombN
{
    partial class UsefulForm1
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
            this.checkBox1profilekey = new System.Windows.Forms.CheckBox();
            this.checkBox1alertmessages = new System.Windows.Forms.CheckBox();
            this.checkBox1groupkey = new System.Windows.Forms.CheckBox();
            this.checkBox1imsound = new System.Windows.Forms.CheckBox();
            this.checkBox1teleportinfo = new System.Windows.Forms.CheckBox();
            this.textBox1sounduuid = new System.Windows.Forms.TextBox();
            this.trackBar1volume = new System.Windows.Forms.TrackBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1volume)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.trackBar1volume);
            this.panel1.Controls.Add(this.textBox1sounduuid);
            this.panel1.Controls.Add(this.checkBox1teleportinfo);
            this.panel1.Controls.Add(this.checkBox1imsound);
            this.panel1.Controls.Add(this.checkBox1profilekey);
            this.panel1.Controls.Add(this.checkBox1alertmessages);
            this.panel1.Controls.Add(this.checkBox1groupkey);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(366, 303);
            this.panel1.TabIndex = 0;
            // 
            // checkBox1profilekey
            // 
            this.checkBox1profilekey.AutoSize = true;
            this.checkBox1profilekey.ForeColor = System.Drawing.Color.Red;
            this.checkBox1profilekey.Location = new System.Drawing.Point(38, 16);
            this.checkBox1profilekey.Name = "checkBox1profilekey";
            this.checkBox1profilekey.Size = new System.Drawing.Size(106, 17);
            this.checkBox1profilekey.TabIndex = 2;
            this.checkBox1profilekey.Text = "Show Profile Key";
            this.checkBox1profilekey.UseVisualStyleBackColor = true;
            // 
            // checkBox1alertmessages
            // 
            this.checkBox1alertmessages.AutoSize = true;
            this.checkBox1alertmessages.Checked = true;
            this.checkBox1alertmessages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1alertmessages.ForeColor = System.Drawing.Color.Red;
            this.checkBox1alertmessages.Location = new System.Drawing.Point(38, 74);
            this.checkBox1alertmessages.Name = "checkBox1alertmessages";
            this.checkBox1alertmessages.Size = new System.Drawing.Size(161, 17);
            this.checkBox1alertmessages.TabIndex = 1;
            this.checkBox1alertmessages.Text = "Show Better Allert Messages";
            this.checkBox1alertmessages.UseVisualStyleBackColor = true;
            // 
            // checkBox1groupkey
            // 
            this.checkBox1groupkey.AutoSize = true;
            this.checkBox1groupkey.ForeColor = System.Drawing.Color.Red;
            this.checkBox1groupkey.Location = new System.Drawing.Point(38, 39);
            this.checkBox1groupkey.Name = "checkBox1groupkey";
            this.checkBox1groupkey.Size = new System.Drawing.Size(106, 17);
            this.checkBox1groupkey.TabIndex = 0;
            this.checkBox1groupkey.Text = "Show Group Key";
            this.checkBox1groupkey.UseVisualStyleBackColor = true;
            // 
            // checkBox1imsound
            // 
            this.checkBox1imsound.AutoSize = true;
            this.checkBox1imsound.ForeColor = System.Drawing.Color.Red;
            this.checkBox1imsound.Location = new System.Drawing.Point(38, 114);
            this.checkBox1imsound.Name = "checkBox1imsound";
            this.checkBox1imsound.Size = new System.Drawing.Size(176, 17);
            this.checkBox1imsound.TabIndex = 3;
            this.checkBox1imsound.Text = "Play Sound When IM Recieved";
            this.checkBox1imsound.UseVisualStyleBackColor = true;
            // 
            // checkBox1teleportinfo
            // 
            this.checkBox1teleportinfo.AutoSize = true;
            this.checkBox1teleportinfo.Checked = true;
            this.checkBox1teleportinfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1teleportinfo.ForeColor = System.Drawing.Color.Red;
            this.checkBox1teleportinfo.Location = new System.Drawing.Point(38, 194);
            this.checkBox1teleportinfo.Name = "checkBox1teleportinfo";
            this.checkBox1teleportinfo.Size = new System.Drawing.Size(247, 17);
            this.checkBox1teleportinfo.TabIndex = 4;
            this.checkBox1teleportinfo.Text = "Provide Aditional Info About Teleport Requests";
            this.checkBox1teleportinfo.UseVisualStyleBackColor = true;
            // 
            // textBox1sounduuid
            // 
            this.textBox1sounduuid.Location = new System.Drawing.Point(38, 146);
            this.textBox1sounduuid.Name = "textBox1sounduuid";
            this.textBox1sounduuid.Size = new System.Drawing.Size(212, 20);
            this.textBox1sounduuid.TabIndex = 5;
            this.textBox1sounduuid.Text = "4c366008-65da-2e84-9b74-f58a392b94c6";
            // 
            // trackBar1volume
            // 
            this.trackBar1volume.LargeChange = 10;
            this.trackBar1volume.Location = new System.Drawing.Point(252, 106);
            this.trackBar1volume.Maximum = 100;
            this.trackBar1volume.Name = "trackBar1volume";
            this.trackBar1volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1volume.Size = new System.Drawing.Size(42, 68);
            this.trackBar1volume.TabIndex = 6;
            this.trackBar1volume.Value = 50;
            // 
            // UsefulForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 303);
            this.Controls.Add(this.panel1);
            this.Name = "UsefulForm1";
            this.Text = "UsefulForm1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1volume)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox checkBox1groupkey;
        public System.Windows.Forms.CheckBox checkBox1alertmessages;
        public System.Windows.Forms.CheckBox checkBox1profilekey;
        public System.Windows.Forms.TextBox textBox1sounduuid;
        public System.Windows.Forms.CheckBox checkBox1teleportinfo;
        public System.Windows.Forms.CheckBox checkBox1imsound;
        public System.Windows.Forms.TrackBar trackBar1volume;
    }
}