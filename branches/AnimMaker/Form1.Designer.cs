namespace AnimMaker
{
    partial class Form1
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
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoopCheckbox = new System.Windows.Forms.CheckBox();
            this.AddJointSelect = new System.Windows.Forms.ComboBox();
            this.JointList = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.RotKeyframes = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.PosKeyframes = new System.Windows.Forms.ListBox();
            this.AddJoint = new System.Windows.Forms.Button();
            this.RemoveJoint = new System.Windows.Forms.Button();
            this.RemoveRotKey = new System.Windows.Forms.Button();
            this.AddRotKey = new System.Windows.Forms.Button();
            this.RemovePosKey = new System.Windows.Forms.Button();
            this.AddPosKey = new System.Windows.Forms.Button();
            this.RotX = new System.Windows.Forms.NumericUpDown();
            this.RotY = new System.Windows.Forms.NumericUpDown();
            this.RotZ = new System.Windows.Forms.NumericUpDown();
            this.PosZ = new System.Windows.Forms.NumericUpDown();
            this.PosY = new System.Windows.Forms.NumericUpDown();
            this.PosX = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.MajorVersion = new System.Windows.Forms.NumericUpDown();
            this.MinorVersion = new System.Windows.Forms.NumericUpDown();
            this.Priority = new System.Windows.Forms.NumericUpDown();
            this.Duration = new System.Windows.Forms.NumericUpDown();
            this.LoopInTime = new System.Windows.Forms.NumericUpDown();
            this.LoopOutTime = new System.Windows.Forms.NumericUpDown();
            this.EaseInDuration = new System.Windows.Forms.NumericUpDown();
            this.EaseOutDuration = new System.Windows.Forms.NumericUpDown();
            this.RotTime = new System.Windows.Forms.NumericUpDown();
            this.PosTime = new System.Windows.Forms.NumericUpDown();
            this.JointPriority = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.HandPose = new System.Windows.Forms.NumericUpDown();
            this.MergeButton = new System.Windows.Forms.Button();
            this.MergeOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.RenameButton = new System.Windows.Forms.Button();
            this.Tooltips = new System.Windows.Forms.ToolTip(this.components);
            this.RescaleTimeCheck = new System.Windows.Forms.CheckBox();
            this.HelpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MajorVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinorVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Priority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Duration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopInTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopOutTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EaseInDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EaseOutDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JointPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HandPose)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenDialog
            // 
            this.OpenDialog.Filter = "Animation Files (*.anim)|*.anim";
            this.OpenDialog.RestoreDirectory = true;
            this.OpenDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenDialog_FileOk);
            // 
            // SaveDialog
            // 
            this.SaveDialog.Filter = "Animation Files (*.anim)|*.anim";
            this.SaveDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveDialog_FileOk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 221);
            this.label1.TabIndex = 0;
            this.label1.Text = "Major Version(1):\r\n\r\nMinor Version(0):\r\n\r\nPriority(0-4):\r\n\r\nDuration(0-90):\r\n\r\nLo" +
                "op In Time:(0-90)\r\n\r\nLoop Out Time(0-90):\r\n\r\nEase In Duration(0-90):\r\n\r\nEase Out" +
                " Duration(0-90):\r\n\r\nHand Pose(0-14):";
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(16, 296);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(80, 20);
            this.OpenButton.TabIndex = 8;
            this.OpenButton.Text = "Open File";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(104, 296);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(80, 20);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save File";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoopCheckbox
            // 
            this.LoopCheckbox.AutoSize = true;
            this.LoopCheckbox.Location = new System.Drawing.Point(8, 256);
            this.LoopCheckbox.Name = "LoopCheckbox";
            this.LoopCheckbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoopCheckbox.Size = new System.Drawing.Size(99, 17);
            this.LoopCheckbox.TabIndex = 10;
            this.LoopCheckbox.Text = "Loop Animation";
            this.Tooltips.SetToolTip(this.LoopCheckbox, resources.GetString("LoopCheckbox.ToolTip"));
            this.LoopCheckbox.UseVisualStyleBackColor = true;
            this.LoopCheckbox.CheckedChanged += new System.EventHandler(this.LoopCheckbox_CheckedChanged);
            // 
            // AddJointSelect
            // 
            this.AddJointSelect.FormattingEnabled = true;
            this.AddJointSelect.Items.AddRange(new object[] {
            "Chest",
            "Chin",
            "L Forearm",
            "L Lower Leg",
            "L Upper Arm",
            "L Upper Leg",
            "Left Ear",
            "Left Eyeball",
            "Left Foot",
            "Left Hand",
            "Left Hip",
            "Left Pec",
            "Left Shoulder",
            "mAnkleLeft",
            "mAnkleRight",
            "mChest",
            "mCollarLeft",
            "mCollarRight",
            "mElbowLeft",
            "mElbowRight",
            "mEyeLeft",
            "mEyeRight",
            "mFootLeft",
            "mFootRight",
            "mHead",
            "mHipLeft",
            "mHipRight",
            "mKneeLeft",
            "mKneeRight",
            "mNeck",
            "Mouth",
            "mPelvis",
            "mShoulderLeft",
            "mShoulderRight",
            "mSkull",
            "mToeLeft",
            "mToeRight",
            "mTorso",
            "mWristLeft",
            "mWristRight",
            "Nose",
            "Pelvis",
            "R Forearm",
            "R Lower Leg",
            "R Upper Arm",
            "R Upper Leg",
            "Right Ear",
            "Right Eyeball",
            "Right Foot",
            "Right Hand",
            "Right Hip",
            "Right Pec",
            "Right Shoulder",
            "Skull",
            "Spine",
            "Stomach"});
            this.AddJointSelect.Location = new System.Drawing.Point(200, 240);
            this.AddJointSelect.Name = "AddJointSelect";
            this.AddJointSelect.Size = new System.Drawing.Size(80, 21);
            this.AddJointSelect.Sorted = true;
            this.AddJointSelect.TabIndex = 11;
            this.AddJointSelect.Text = "Joint";
            this.Tooltips.SetToolTip(this.AddJointSelect, resources.GetString("AddJointSelect.ToolTip"));
            // 
            // JointList
            // 
            this.JointList.FormattingEnabled = true;
            this.JointList.Location = new System.Drawing.Point(200, 24);
            this.JointList.Name = "JointList";
            this.JointList.Size = new System.Drawing.Size(152, 212);
            this.JointList.Sorted = true;
            this.JointList.TabIndex = 12;
            this.Tooltips.SetToolTip(this.JointList, resources.GetString("JointList.ToolTip"));
            this.JointList.SelectedIndexChanged += new System.EventHandler(this.JointList_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(200, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Joints:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(368, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Rot Keyframes:";
            // 
            // RotKeyframes
            // 
            this.RotKeyframes.FormattingEnabled = true;
            this.RotKeyframes.Location = new System.Drawing.Point(368, 24);
            this.RotKeyframes.Name = "RotKeyframes";
            this.RotKeyframes.Size = new System.Drawing.Size(152, 108);
            this.RotKeyframes.TabIndex = 14;
            this.Tooltips.SetToolTip(this.RotKeyframes, resources.GetString("RotKeyframes.ToolTip"));
            this.RotKeyframes.SelectedIndexChanged += new System.EventHandler(this.RotKeyframes_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(368, 160);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Pos Keyframes:";
            // 
            // PosKeyframes
            // 
            this.PosKeyframes.FormattingEnabled = true;
            this.PosKeyframes.Location = new System.Drawing.Point(368, 176);
            this.PosKeyframes.Name = "PosKeyframes";
            this.PosKeyframes.Size = new System.Drawing.Size(152, 108);
            this.PosKeyframes.TabIndex = 16;
            this.Tooltips.SetToolTip(this.PosKeyframes, resources.GetString("PosKeyframes.ToolTip"));
            this.PosKeyframes.SelectedIndexChanged += new System.EventHandler(this.PosKeyframes_SelectedIndexChanged);
            // 
            // AddJoint
            // 
            this.AddJoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddJoint.Location = new System.Drawing.Point(312, 240);
            this.AddJoint.Name = "AddJoint";
            this.AddJoint.Size = new System.Drawing.Size(16, 24);
            this.AddJoint.TabIndex = 18;
            this.AddJoint.Text = "+";
            this.Tooltips.SetToolTip(this.AddJoint, "Adds the currently selected joint to the animation from the new joint dropdown.");
            this.AddJoint.UseVisualStyleBackColor = true;
            this.AddJoint.Click += new System.EventHandler(this.AddJoint_Click);
            // 
            // RemoveJoint
            // 
            this.RemoveJoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveJoint.Location = new System.Drawing.Point(336, 240);
            this.RemoveJoint.Name = "RemoveJoint";
            this.RemoveJoint.Size = new System.Drawing.Size(16, 24);
            this.RemoveJoint.TabIndex = 19;
            this.RemoveJoint.Text = "-";
            this.Tooltips.SetToolTip(this.RemoveJoint, "Removes the currently selected joint and all of its data.");
            this.RemoveJoint.UseVisualStyleBackColor = true;
            this.RemoveJoint.Click += new System.EventHandler(this.RemoveJoint_Click);
            // 
            // RemoveRotKey
            // 
            this.RemoveRotKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveRotKey.Location = new System.Drawing.Point(504, 136);
            this.RemoveRotKey.Name = "RemoveRotKey";
            this.RemoveRotKey.Size = new System.Drawing.Size(16, 24);
            this.RemoveRotKey.TabIndex = 21;
            this.RemoveRotKey.Text = "-";
            this.RemoveRotKey.UseVisualStyleBackColor = true;
            this.RemoveRotKey.Click += new System.EventHandler(this.RemoveRotKey_Click);
            // 
            // AddRotKey
            // 
            this.AddRotKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddRotKey.Location = new System.Drawing.Point(480, 136);
            this.AddRotKey.Name = "AddRotKey";
            this.AddRotKey.Size = new System.Drawing.Size(16, 24);
            this.AddRotKey.TabIndex = 20;
            this.AddRotKey.Text = "+";
            this.AddRotKey.UseVisualStyleBackColor = true;
            this.AddRotKey.Click += new System.EventHandler(this.AddRotKey_Click);
            // 
            // RemovePosKey
            // 
            this.RemovePosKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemovePosKey.Location = new System.Drawing.Point(504, 288);
            this.RemovePosKey.Name = "RemovePosKey";
            this.RemovePosKey.Size = new System.Drawing.Size(16, 24);
            this.RemovePosKey.TabIndex = 23;
            this.RemovePosKey.Text = "-";
            this.RemovePosKey.UseVisualStyleBackColor = true;
            this.RemovePosKey.Click += new System.EventHandler(this.RemovePosKey_Click);
            // 
            // AddPosKey
            // 
            this.AddPosKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPosKey.Location = new System.Drawing.Point(480, 288);
            this.AddPosKey.Name = "AddPosKey";
            this.AddPosKey.Size = new System.Drawing.Size(16, 24);
            this.AddPosKey.TabIndex = 22;
            this.AddPosKey.Text = "+";
            this.AddPosKey.UseVisualStyleBackColor = true;
            this.AddPosKey.Click += new System.EventHandler(this.AddPosKey_Click);
            // 
            // RotX
            // 
            this.RotX.DecimalPlaces = 5;
            this.RotX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RotX.Location = new System.Drawing.Point(552, 24);
            this.RotX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotX.Name = "RotX";
            this.RotX.Size = new System.Drawing.Size(64, 20);
            this.RotX.TabIndex = 24;
            this.RotX.ValueChanged += new System.EventHandler(this.RotX_ValueChanged);
            // 
            // RotY
            // 
            this.RotY.DecimalPlaces = 5;
            this.RotY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RotY.Location = new System.Drawing.Point(552, 48);
            this.RotY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotY.Name = "RotY";
            this.RotY.Size = new System.Drawing.Size(64, 20);
            this.RotY.TabIndex = 25;
            this.RotY.ValueChanged += new System.EventHandler(this.RotY_ValueChanged);
            // 
            // RotZ
            // 
            this.RotZ.DecimalPlaces = 5;
            this.RotZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RotZ.Location = new System.Drawing.Point(552, 72);
            this.RotZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotZ.Name = "RotZ";
            this.RotZ.Size = new System.Drawing.Size(64, 20);
            this.RotZ.TabIndex = 26;
            this.RotZ.ValueChanged += new System.EventHandler(this.RotZ_ValueChanged);
            // 
            // PosZ
            // 
            this.PosZ.DecimalPlaces = 5;
            this.PosZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PosZ.Location = new System.Drawing.Point(552, 224);
            this.PosZ.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PosZ.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.PosZ.Name = "PosZ";
            this.PosZ.Size = new System.Drawing.Size(64, 20);
            this.PosZ.TabIndex = 29;
            this.PosZ.ValueChanged += new System.EventHandler(this.PosZ_ValueChanged);
            // 
            // PosY
            // 
            this.PosY.DecimalPlaces = 5;
            this.PosY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PosY.Location = new System.Drawing.Point(552, 200);
            this.PosY.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PosY.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.PosY.Name = "PosY";
            this.PosY.Size = new System.Drawing.Size(64, 20);
            this.PosY.TabIndex = 28;
            this.PosY.ValueChanged += new System.EventHandler(this.PosY_ValueChanged);
            // 
            // PosX
            // 
            this.PosX.DecimalPlaces = 5;
            this.PosX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PosX.Location = new System.Drawing.Point(552, 176);
            this.PosX.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PosX.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.PosX.Name = "PosX";
            this.PosX.Size = new System.Drawing.Size(64, 20);
            this.PosX.TabIndex = 27;
            this.PosX.ValueChanged += new System.EventHandler(this.PosX_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(528, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 91);
            this.label12.TabIndex = 30;
            this.label12.Text = "X:\r\n\r\nY:\r\n\r\nZ:\r\n\r\nTI:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(528, 176);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 91);
            this.label13.TabIndex = 31;
            this.label13.Text = "X:\r\n\r\nY:\r\n\r\nZ:\r\n\r\nTI:";
            // 
            // MajorVersion
            // 
            this.MajorVersion.Enabled = false;
            this.MajorVersion.Location = new System.Drawing.Point(136, 24);
            this.MajorVersion.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MajorVersion.Name = "MajorVersion";
            this.MajorVersion.Size = new System.Drawing.Size(56, 20);
            this.MajorVersion.TabIndex = 32;
            this.Tooltips.SetToolTip(this.MajorVersion, "The animation files major version number.\r\n\r\nThe only valid option for this value" +
                    " is 1.");
            this.MajorVersion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MinorVersion
            // 
            this.MinorVersion.Enabled = false;
            this.MinorVersion.Location = new System.Drawing.Point(136, 49);
            this.MinorVersion.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinorVersion.Name = "MinorVersion";
            this.MinorVersion.Size = new System.Drawing.Size(56, 20);
            this.MinorVersion.TabIndex = 33;
            this.Tooltips.SetToolTip(this.MinorVersion, "The animation files minor version.\r\n\r\nThe only valid option for this value is 0.");
            // 
            // Priority
            // 
            this.Priority.Location = new System.Drawing.Point(136, 74);
            this.Priority.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.Priority.Name = "Priority";
            this.Priority.Size = new System.Drawing.Size(56, 20);
            this.Priority.TabIndex = 34;
            this.Tooltips.SetToolTip(this.Priority, resources.GetString("Priority.ToolTip"));
            this.Priority.ValueChanged += new System.EventHandler(this.Priority_ValueChanged);
            // 
            // Duration
            // 
            this.Duration.DecimalPlaces = 4;
            this.Duration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Duration.Location = new System.Drawing.Point(136, 99);
            this.Duration.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.Duration.Name = "Duration";
            this.Duration.Size = new System.Drawing.Size(56, 20);
            this.Duration.TabIndex = 35;
            this.Tooltips.SetToolTip(this.Duration, resources.GetString("Duration.ToolTip"));
            this.Duration.ValueChanged += new System.EventHandler(this.Duration_ValueChanged);
            // 
            // LoopInTime
            // 
            this.LoopInTime.DecimalPlaces = 4;
            this.LoopInTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LoopInTime.Location = new System.Drawing.Point(136, 124);
            this.LoopInTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.LoopInTime.Name = "LoopInTime";
            this.LoopInTime.Size = new System.Drawing.Size(56, 20);
            this.LoopInTime.TabIndex = 36;
            this.Tooltips.SetToolTip(this.LoopInTime, resources.GetString("LoopInTime.ToolTip"));
            this.LoopInTime.ValueChanged += new System.EventHandler(this.LoopInTime_ValueChanged);
            // 
            // LoopOutTime
            // 
            this.LoopOutTime.DecimalPlaces = 4;
            this.LoopOutTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LoopOutTime.Location = new System.Drawing.Point(136, 149);
            this.LoopOutTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.LoopOutTime.Name = "LoopOutTime";
            this.LoopOutTime.Size = new System.Drawing.Size(56, 20);
            this.LoopOutTime.TabIndex = 37;
            this.Tooltips.SetToolTip(this.LoopOutTime, resources.GetString("LoopOutTime.ToolTip"));
            this.LoopOutTime.ValueChanged += new System.EventHandler(this.LoopOutTime_ValueChanged);
            // 
            // EaseInDuration
            // 
            this.EaseInDuration.DecimalPlaces = 4;
            this.EaseInDuration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.EaseInDuration.Location = new System.Drawing.Point(136, 174);
            this.EaseInDuration.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.EaseInDuration.Name = "EaseInDuration";
            this.EaseInDuration.Size = new System.Drawing.Size(56, 20);
            this.EaseInDuration.TabIndex = 38;
            this.Tooltips.SetToolTip(this.EaseInDuration, resources.GetString("EaseInDuration.ToolTip"));
            this.EaseInDuration.ValueChanged += new System.EventHandler(this.EaseInDuration_ValueChanged);
            // 
            // EaseOutDuration
            // 
            this.EaseOutDuration.DecimalPlaces = 4;
            this.EaseOutDuration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.EaseOutDuration.Location = new System.Drawing.Point(136, 199);
            this.EaseOutDuration.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.EaseOutDuration.Name = "EaseOutDuration";
            this.EaseOutDuration.Size = new System.Drawing.Size(56, 20);
            this.EaseOutDuration.TabIndex = 39;
            this.Tooltips.SetToolTip(this.EaseOutDuration, resources.GetString("EaseOutDuration.ToolTip"));
            this.EaseOutDuration.ValueChanged += new System.EventHandler(this.EaseOutDuration_ValueChanged);
            // 
            // RotTime
            // 
            this.RotTime.DecimalPlaces = 5;
            this.RotTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RotTime.Location = new System.Drawing.Point(552, 96);
            this.RotTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.RotTime.Name = "RotTime";
            this.RotTime.Size = new System.Drawing.Size(64, 20);
            this.RotTime.TabIndex = 40;
            this.Tooltips.SetToolTip(this.RotTime, resources.GetString("RotTime.ToolTip"));
            this.RotTime.ValueChanged += new System.EventHandler(this.RotTime_ValueChanged);
            // 
            // PosTime
            // 
            this.PosTime.DecimalPlaces = 5;
            this.PosTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PosTime.Location = new System.Drawing.Point(552, 248);
            this.PosTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.PosTime.Name = "PosTime";
            this.PosTime.Size = new System.Drawing.Size(64, 20);
            this.PosTime.TabIndex = 41;
            this.Tooltips.SetToolTip(this.PosTime, resources.GetString("PosTime.ToolTip"));
            this.PosTime.ValueChanged += new System.EventHandler(this.PosTime_ValueChanged);
            // 
            // JointPriority
            // 
            this.JointPriority.Location = new System.Drawing.Point(296, 272);
            this.JointPriority.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.JointPriority.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.JointPriority.Name = "JointPriority";
            this.JointPriority.Size = new System.Drawing.Size(56, 20);
            this.JointPriority.TabIndex = 42;
            this.Tooltips.SetToolTip(this.JointPriority, "Determines the priority of the joint in the animation.\r\n\r\n0: Normal\r\n1: High\r\n2: " +
                    "Higher\r\n3: Very High\r\n4: Highest\r\n-1:Use animations base priority");
            this.JointPriority.ValueChanged += new System.EventHandler(this.JointPriority_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Joint Priority:";
            // 
            // HandPose
            // 
            this.HandPose.Location = new System.Drawing.Point(136, 224);
            this.HandPose.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.HandPose.Name = "HandPose";
            this.HandPose.Size = new System.Drawing.Size(56, 20);
            this.HandPose.TabIndex = 44;
            this.Tooltips.SetToolTip(this.HandPose, "Determines the hand position used when the \r\n     animation is played.\r\n\r\nThe def" +
                    "ault is 1.\r\n\r\nCustom hand poses are not currently possible.");
            this.HandPose.ValueChanged += new System.EventHandler(this.HandPose_ValueChanged);
            // 
            // MergeButton
            // 
            this.MergeButton.Location = new System.Drawing.Point(192, 296);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(80, 20);
            this.MergeButton.TabIndex = 45;
            this.MergeButton.Text = "Merge Open";
            this.Tooltips.SetToolTip(this.MergeButton, resources.GetString("MergeButton.ToolTip"));
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // MergeOpenDialog
            // 
            this.MergeOpenDialog.Filter = "Animation Files (*.anim)|*.anim";
            this.MergeOpenDialog.RestoreDirectory = true;
            this.MergeOpenDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.MergeOpenDialog_FileOk);
            // 
            // RenameButton
            // 
            this.RenameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RenameButton.Location = new System.Drawing.Point(288, 240);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(16, 24);
            this.RenameButton.TabIndex = 46;
            this.RenameButton.Text = "R";
            this.Tooltips.SetToolTip(this.RenameButton, "Renames the currently selected joint with the joint selected in the new joint dro" +
                    "pdown.");
            this.RenameButton.UseVisualStyleBackColor = true;
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // Tooltips
            // 
            this.Tooltips.Active = false;
            // 
            // RescaleTimeCheck
            // 
            this.RescaleTimeCheck.AutoSize = true;
            this.RescaleTimeCheck.Location = new System.Drawing.Point(280, 296);
            this.RescaleTimeCheck.Name = "RescaleTimeCheck";
            this.RescaleTimeCheck.Size = new System.Drawing.Size(96, 17);
            this.RescaleTimeCheck.TabIndex = 47;
            this.RescaleTimeCheck.Text = "Rescale Times";
            this.Tooltips.SetToolTip(this.RescaleTimeCheck, resources.GetString("RescaleTimeCheck.ToolTip"));
            this.RescaleTimeCheck.UseVisualStyleBackColor = true;
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(592, 296);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(24, 24);
            this.HelpButton.TabIndex = 48;
            this.HelpButton.Text = "?";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 328);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.RescaleTimeCheck);
            this.Controls.Add(this.RenameButton);
            this.Controls.Add(this.MergeButton);
            this.Controls.Add(this.HandPose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.JointPriority);
            this.Controls.Add(this.PosTime);
            this.Controls.Add(this.RotTime);
            this.Controls.Add(this.EaseOutDuration);
            this.Controls.Add(this.EaseInDuration);
            this.Controls.Add(this.LoopOutTime);
            this.Controls.Add(this.LoopInTime);
            this.Controls.Add(this.Duration);
            this.Controls.Add(this.Priority);
            this.Controls.Add(this.MinorVersion);
            this.Controls.Add(this.MajorVersion);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.PosZ);
            this.Controls.Add(this.PosY);
            this.Controls.Add(this.PosX);
            this.Controls.Add(this.RotZ);
            this.Controls.Add(this.RotY);
            this.Controls.Add(this.RotX);
            this.Controls.Add(this.RemovePosKey);
            this.Controls.Add(this.AddPosKey);
            this.Controls.Add(this.RemoveRotKey);
            this.Controls.Add(this.AddRotKey);
            this.Controls.Add(this.RemoveJoint);
            this.Controls.Add(this.AddJoint);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.PosKeyframes);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.RotKeyframes);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.JointList);
            this.Controls.Add(this.AddJointSelect);
            this.Controls.Add(this.LoopCheckbox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "AniMaker";
            ((System.ComponentModel.ISupportInitialize)(this.RotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MajorVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinorVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Priority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Duration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopInTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopOutTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EaseInDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EaseOutDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JointPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HandPose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox LoopCheckbox;
        private System.Windows.Forms.ComboBox AddJointSelect;
        private System.Windows.Forms.ListBox JointList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox RotKeyframes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox PosKeyframes;
        private System.Windows.Forms.Button AddJoint;
        private System.Windows.Forms.Button RemoveJoint;
        private System.Windows.Forms.Button RemoveRotKey;
        private System.Windows.Forms.Button AddRotKey;
        private System.Windows.Forms.Button RemovePosKey;
        private System.Windows.Forms.Button AddPosKey;
        private System.Windows.Forms.NumericUpDown RotX;
        private System.Windows.Forms.NumericUpDown RotY;
        private System.Windows.Forms.NumericUpDown RotZ;
        private System.Windows.Forms.NumericUpDown PosZ;
        private System.Windows.Forms.NumericUpDown PosY;
        private System.Windows.Forms.NumericUpDown PosX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown MajorVersion;
        private System.Windows.Forms.NumericUpDown MinorVersion;
        private System.Windows.Forms.NumericUpDown Priority;
        private System.Windows.Forms.NumericUpDown Duration;
        private System.Windows.Forms.NumericUpDown LoopInTime;
        private System.Windows.Forms.NumericUpDown LoopOutTime;
        private System.Windows.Forms.NumericUpDown EaseInDuration;
        private System.Windows.Forms.NumericUpDown EaseOutDuration;
        private System.Windows.Forms.NumericUpDown RotTime;
        private System.Windows.Forms.NumericUpDown PosTime;
        private System.Windows.Forms.NumericUpDown JointPriority;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown HandPose;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.OpenFileDialog MergeOpenDialog;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.ToolTip Tooltips;
        private System.Windows.Forms.CheckBox RescaleTimeCheck;
        private System.Windows.Forms.Button HelpButton;
    }
}

