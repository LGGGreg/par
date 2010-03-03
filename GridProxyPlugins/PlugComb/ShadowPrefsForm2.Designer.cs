namespace PubCombN
{
    partial class ShadowPrefsForm2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1prefix = new System.Windows.Forms.Button();
            this.textBox1Indicator = new System.Windows.Forms.TextBox();
            this.textBox2Trigger = new System.Windows.Forms.TextBox();
            this.textBox3Brand = new System.Windows.Forms.TextBox();
            this.button2submit = new System.Windows.Forms.Button();
            this.button3cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Preferences";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Encrpytion Indicator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fast Encryptionn Mode Switch Trigger";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Encryption Branding";
            // 
            // button1prefix
            // 
            this.button1prefix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1prefix.Location = new System.Drawing.Point(119, 108);
            this.button1prefix.Name = "button1prefix";
            this.button1prefix.Size = new System.Drawing.Size(19, 24);
            this.button1prefix.TabIndex = 4;
            this.button1prefix.Text = "?";
            this.button1prefix.UseVisualStyleBackColor = true;
            this.button1prefix.Click += new System.EventHandler(this.button1prefix_Click);
            // 
            // textBox1Indicator
            // 
            this.textBox1Indicator.BackColor = System.Drawing.Color.Black;
            this.textBox1Indicator.ForeColor = System.Drawing.Color.OrangeRed;
            this.textBox1Indicator.Location = new System.Drawing.Point(208, 50);
            this.textBox1Indicator.Name = "textBox1Indicator";
            this.textBox1Indicator.Size = new System.Drawing.Size(233, 20);
            this.textBox1Indicator.TabIndex = 5;
            this.textBox1Indicator.Text = "[E]";
            // 
            // textBox2Trigger
            // 
            this.textBox2Trigger.BackColor = System.Drawing.Color.Black;
            this.textBox2Trigger.ForeColor = System.Drawing.Color.OrangeRed;
            this.textBox2Trigger.Location = new System.Drawing.Point(208, 84);
            this.textBox2Trigger.Name = "textBox2Trigger";
            this.textBox2Trigger.Size = new System.Drawing.Size(233, 20);
            this.textBox2Trigger.TabIndex = 6;
            this.textBox2Trigger.Text = "\\";
            // 
            // textBox3Brand
            // 
            this.textBox3Brand.BackColor = System.Drawing.Color.Black;
            this.textBox3Brand.Enabled = false;
            this.textBox3Brand.ForeColor = System.Drawing.Color.OrangeRed;
            this.textBox3Brand.Location = new System.Drawing.Point(208, 114);
            this.textBox3Brand.Name = "textBox3Brand";
            this.textBox3Brand.Size = new System.Drawing.Size(233, 20);
            this.textBox3Brand.TabIndex = 7;
            this.textBox3Brand.Text = "Shadow";
            // 
            // button2submit
            // 
            this.button2submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2submit.Location = new System.Drawing.Point(31, 152);
            this.button2submit.Name = "button2submit";
            this.button2submit.Size = new System.Drawing.Size(149, 24);
            this.button2submit.TabIndex = 8;
            this.button2submit.Text = "Submit";
            this.button2submit.UseVisualStyleBackColor = true;
            this.button2submit.Click += new System.EventHandler(this.button2submit_Click);
            // 
            // button3cancel
            // 
            this.button3cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3cancel.Location = new System.Drawing.Point(253, 152);
            this.button3cancel.Name = "button3cancel";
            this.button3cancel.Size = new System.Drawing.Size(149, 24);
            this.button3cancel.TabIndex = 9;
            this.button3cancel.Text = "Cancel";
            this.button3cancel.UseVisualStyleBackColor = true;
            this.button3cancel.Click += new System.EventHandler(this.button3cancel_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.button3cancel;
            this.ClientSize = new System.Drawing.Size(450, 188);
            this.Controls.Add(this.button3cancel);
            this.Controls.Add(this.button2submit);
            this.Controls.Add(this.textBox3Brand);
            this.Controls.Add(this.textBox2Trigger);
            this.Controls.Add(this.textBox1Indicator);
            this.Controls.Add(this.button1prefix);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "Form2";
            this.Text = "Shadow-Preferences";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1prefix;
        private System.Windows.Forms.TextBox textBox1Indicator;
        private System.Windows.Forms.TextBox textBox2Trigger;
        private System.Windows.Forms.TextBox textBox3Brand;
        private System.Windows.Forms.Button button2submit;
        private System.Windows.Forms.Button button3cancel;
    }
}