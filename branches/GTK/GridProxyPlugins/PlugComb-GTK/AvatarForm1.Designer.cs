using System.Drawing;
using System.Text;

using System.Windows.Forms;
namespace PubComb
{
    partial class FormAvatars
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
            this.panel2 = new Gtk.Fixed();
            this.panel4 = new Gtk.Fixed();
            this.dataGridView1 = new Gtk.NodeView();
            this.panel3 = new Gtk.Fixed();
            this.label1 = new Gtk.Label();
            // 
            // panel1
          
            this.panel1.Name = "panel1";
			this.panel1.SetUsize(945, 10);
			
     
            // 
            // panel2
            // 
            this.panel2.Add(this.panel4);
            this.panel2.Add(this.panel3);
   
            this.panel2.SetUposition(0, 10);
            this.panel2.Name = "panel2";
            this.panel2.SetUsize(945, 256);
            // 
            // panel4
            // 
            this.panel4.Add(this.dataGridView1);
            this.panel4.SetUposition(0, 29);
            this.panel4.Name = "panel4";
            this.panel4.SetUsize(945, 227);
            // 
            // dataGridView1
			//
			
            //this.dataGridView1.BackgroundColor = System.Drawing.Color.Black;
            this.dataGridView1.SetUposition(0, 0);
            this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.SetUsize(945, 227);
            //this.dataGridView1.CellContentClick += new Gtk.NodeViewCellEventHandler(this.dataGridView1_CellContentClick);
			
            // 
            // panel3
            // 
            this.panel3.Add(this.label1);
            
            this.panel3.SetUposition(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.SetUsize(945, 29);
            
            // 
            // label1
            // 
            
            this.label1.SetUposition(22, 7);
            this.label1.Name = "label1";
            this.label1.SetUsize(105, 13);
            this.label1.Text = "Day Oh\'s Avatar Info";
            // 
            // FormAvatars
            // 
			
			
            this.Add(this.panel1);
            this.Add(this.panel2);

            this.Name = "FormAvatars";
            //this.Text = "Avatars";
        }

        #endregion

        private Gtk.Fixed panel1;
        private Gtk.Fixed panel2;
        private Gtk.Fixed panel4;
        private Gtk.NodeView dataGridView1;
        private Gtk.Fixed panel3;
        private Gtk.Label label1;


    }
}
