namespace PubComb
{
    partial class CinderForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Gtk.Fixed();
            this.checkBox1trans = new Gtk.CheckButton();
            this.checkBox1mod = new Gtk.CheckButton();
            this.checkBox1copy = new Gtk.CheckButton();
            this.progressBar1 = new Gtk.ProgressBar();
            this.panel2 = new Gtk.Fixed();
            this.button2free = new Gtk.Button();
            this.button1 = new Gtk.Button();
            this.buttonLoad = new Gtk.Button();
            this.panel3 = new Gtk.Fixed();
			// BROKEN BROKEN
            //this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1lossless = new Gtk.CheckButton();
            
            // 
            // panel1
            // 
            
            this.panel1.Add(this.checkBox1lossless);
            this.panel1.Add(this.checkBox1trans);
            this.panel1.Add(this.checkBox1mod);
            this.panel1.Add(this.checkBox1copy);
            this.panel1.Add(this.progressBar1);
            this.panel1.Add(this.panel2);
            
            this.panel1.SetUposition(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.SetUsize(447, 46);
            
            // 
            // checkBox1trans
            // 
            //this.checkBox1trans.AutoSize = true;
            this.checkBox1trans.Active = true;
            this.checkBox1trans.SetUposition(121, 26);
            this.checkBox1trans.Name = "checkBox1trans";
            this.checkBox1trans.SetUsize(53, 17);
            // = 7;
            this.checkBox1trans.Label = "Trans";
            
            // 
            // checkBox1mod
            // 
            ////this.checkBox1mod.AutoSize = true;
            this.checkBox1mod.Active = true;
            
            this.checkBox1mod.SetUposition(68, 26);
            this.checkBox1mod.Name = "checkBox1mod";
            this.checkBox1mod.SetUsize(47, 17);
            
            this.checkBox1mod.Label = "Mod";
            // 
            // checkBox1copy
            // 
            //this.checkBox1copy.AutoSize = true;
            this.checkBox1copy.Active = true;
            
            this.checkBox1copy.SetUposition(12, 27);
            this.checkBox1copy.Name = "checkBox1copy";
            this.checkBox1copy.SetUsize(50, 17);
            
            this.checkBox1copy.Label = "Copy";
            
            // 
            // progressBar1
            // 
            this.progressBar1.SetUposition(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.SetUsize(247, 23);
            //this.progressBar1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Add(this.button2free);
            this.panel2.Add(this.button1);
            this.panel2.Add(this.buttonLoad);
            
            this.panel2.SetUposition(247, 0);
            this.panel2.Name = "panel2";
            this.panel2.SetUsize(200, 46);
            
            // 
            // button2free
            // 
            
            this.button2free.SetUposition(44, 0);
            this.button2free.Name = "button2free";
            this.button2free.SetUsize(80, 46);
            
            this.button2free.Label = "Save to SL!";
            
            this.button2free.Activated += new System.EventHandler(this.button2free_Click);
            // 
            // button1
            // 
            
            this.button1.SetUposition(124, 0);
            this.button1.Name = "button1";
            this.button1.SetUsize(76, 46);
            
            this.button1.Label = "About/Help";
            
            this.button1.Activated += new System.EventHandler(this.button1_Click);
            // 
            // buttonLoad
            // 
            
            this.buttonLoad.SetUposition(0, 0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.SetUsize(44, 46);
            
            this.buttonLoad.Label = "Load";
            
            this.buttonLoad.Activated += new System.EventHandler(this.buttonLoad_Click_1);
            // 
            // panel3
            // 
            
            this.panel3.SetUposition(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.SetUsize(447, 40);
            
            // 
            // pictureBox1
            // 
            
//            this.pictureBox1.SetUposition(0, 40);
//            this.pictureBox1.Name = "pictureBox1";
//            this.pictureBox1.SetUsize(447, 261);
//            this.pictureBox1.TabIndex = 3;
//            this.pictureBox1.TabStop = false;
//            // 
            // checkBox1lossless
            // 
            
            this.checkBox1lossless.Active = true;
            
            this.checkBox1lossless.SetUposition(180, 26);
            this.checkBox1lossless.Name = "checkBox1lossless";
            this.checkBox1lossless.SetUsize(70, 17);
           
            this.checkBox1lossless.Label = "LossLess";
           
            // 
            // CinderForm1
            // 
            //this.Add(this.pictureBox1);
            this.Add(this.panel3);
            this.Add(this.panel1);
            this.Name = "CinderForm1";
            this.Title = "Cinderella - Textures That Only Last Till Midnight";
            //this.Load += new System.EventHandler(this.Form1_Load);
            
        }

        #endregion

        private Gtk.Fixed panel1;
        private Gtk.Fixed panel2;
        private Gtk.Button buttonLoad;
        private Gtk.ProgressBar progressBar1;
        private Gtk.CheckButton checkBox1trans;
        private Gtk.CheckButton checkBox1mod;
        private Gtk.CheckButton checkBox1copy;
        private Gtk.Button button1;
        private Gtk.Button button2free;
        private Gtk.Fixed panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Gtk.CheckButton checkBox1lossless;

    }
}