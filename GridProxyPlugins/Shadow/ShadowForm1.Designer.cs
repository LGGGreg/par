namespace PubComb
{
    partial class ShadowForm1
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
            this.textBox2key = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2about = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1enabled = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2key
            // 
            this.textBox2key.BackColor = System.Drawing.Color.Red;
            this.textBox2key.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox2key.Location = new System.Drawing.Point(0, 244);
            this.textBox2key.Name = "textBox2key";
            this.textBox2key.Size = new System.Drawing.Size(284, 20);
            this.textBox2key.TabIndex = 2;
            this.textBox2key.Text = "Enter your key here";
            this.textBox2key.TextChanged += new System.EventHandler(this.textBox2key_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(284, 244);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.button2about);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.checkBox1enabled);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 21);
            this.panel1.TabIndex = 4;
            // 
            // button2about
            // 
            this.button2about.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2about.ForeColor = System.Drawing.Color.Red;
            this.button2about.Location = new System.Drawing.Point(121, 0);
            this.button2about.Name = "button2about";
            this.button2about.Size = new System.Drawing.Size(55, 21);
            this.button2about.TabIndex = 3;
            this.button2about.Text = "About";
            this.button2about.UseVisualStyleBackColor = true;
            this.button2about.Click += new System.EventHandler(this.button2about_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(67, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "Prefs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1enabled
            // 
            this.checkBox1enabled.AutoSize = true;
            this.checkBox1enabled.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBox1enabled.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox1enabled.ForeColor = System.Drawing.Color.Red;
            this.checkBox1enabled.Location = new System.Drawing.Point(0, 0);
            this.checkBox1enabled.Name = "checkBox1enabled";
            this.checkBox1enabled.Size = new System.Drawing.Size(65, 21);
            this.checkBox1enabled.TabIndex = 1;
            this.checkBox1enabled.Text = "Enabled";
            this.checkBox1enabled.UseVisualStyleBackColor = false;
            this.checkBox1enabled.CheckedChanged += new System.EventHandler(this.checkBox1enabled_CheckedChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox2key);
            this.HelpButton = true;
            this.Name = "Form1";
            this.Text = "Shadow";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox2key;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1enabled;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2about;
    }
}