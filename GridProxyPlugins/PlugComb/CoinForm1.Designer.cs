namespace PubCombN
{
    partial class CoinForm1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CoinButton = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(259, 15);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CoinButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(259, 201);
            this.panel2.TabIndex = 1;
            // 
            // CoinButton
            // 
            this.CoinButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CoinButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoinButton.Location = new System.Drawing.Point(0, 0);
            this.CoinButton.Name = "CoinButton";
            this.CoinButton.Size = new System.Drawing.Size(259, 201);
            this.CoinButton.TabIndex = 1;
            this.CoinButton.UseVisualStyleBackColor = true;
            this.CoinButton.Click += new System.EventHandler(this.CoinButton_Click_1);
            // 
            // CoinForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 216);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CoinForm1";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CoinButton;
    }
}