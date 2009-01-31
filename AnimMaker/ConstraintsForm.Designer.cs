namespace AnimMaker
{
    partial class ConstraintsForm
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
            this.ConstraintList = new System.Windows.Forms.ListBox();
            this.SourceText = new System.Windows.Forms.ComboBox();
            this.TargetText = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ConstraintList
            // 
            this.ConstraintList.FormattingEnabled = true;
            this.ConstraintList.Location = new System.Drawing.Point(8, 24);
            this.ConstraintList.Name = "ConstraintList";
            this.ConstraintList.Size = new System.Drawing.Size(120, 225);
            this.ConstraintList.Sorted = true;
            this.ConstraintList.TabIndex = 0;
            this.ConstraintList.SelectedIndexChanged += new System.EventHandler(this.ConstraintList_SelectedIndexChanged);
            // 
            // SourceText
            // 
            this.SourceText.FormattingEnabled = true;
            this.SourceText.Items.AddRange(new object[] {
            "PELVIS",
            "BELLY",
            "CHEST",
            "NECK",
            "HEAD",
            "L_CLAVICLE",
            "L_UPPER_ARM",
            "L_LOWER_ARM",
            "L_HAND",
            "R_CLAVICLE",
            "R_UPPER_ARM",
            "R_LOWER_ARM",
            "R_HAND",
            "R_UPPER_LEG",
            "R_LOWER_LEG",
            "R_FOOT",
            "L_UPPER_LEG",
            "L_LOWER_LEG",
            "L_FOOT"});
            this.SourceText.Location = new System.Drawing.Point(144, 40);
            this.SourceText.Name = "SourceText";
            this.SourceText.Size = new System.Drawing.Size(104, 21);
            this.SourceText.TabIndex = 1;
            // 
            // TargetText
            // 
            this.TargetText.FormattingEnabled = true;
            this.TargetText.Items.AddRange(new object[] {
            "GROUND",
            "PELVIS",
            "BELLY",
            "CHEST",
            "NECK",
            "HEAD",
            "L_CLAVICLE",
            "L_UPPER_ARM",
            "L_LOWER_ARM",
            "L_HAND",
            "R_CLAVICLE",
            "R_UPPER_ARM",
            "R_LOWER_ARM",
            "R_HAND",
            "R_UPPER_LEG",
            "R_LOWER_LEG",
            "R_FOOT",
            "L_UPPER_LEG",
            "L_LOWER_LEG",
            "L_FOOT"});
            this.TargetText.Location = new System.Drawing.Point(144, 80);
            this.TargetText.Name = "TargetText";
            this.TargetText.Size = new System.Drawing.Size(104, 21);
            this.TargetText.TabIndex = 2;
            // 
            // ConstraintsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 270);
            this.Controls.Add(this.TargetText);
            this.Controls.Add(this.SourceText);
            this.Controls.Add(this.ConstraintList);
            this.Name = "ConstraintsForm";
            this.Text = "ConstraintsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ConstraintList;
        private System.Windows.Forms.ComboBox SourceText;
        private System.Windows.Forms.ComboBox TargetText;
    }
}