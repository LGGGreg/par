/* Copyright (c) 2009, Jeffrey MacLellan(Zwagoth Klaar)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the Jeffrey MacLellan nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnimMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AddJointSelect.SelectedIndex = 0;
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            WorkingFile.file.Parse(OpenDialog.FileName);
            UpdateJoints();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveDialog.ShowDialog();
        }

        private void SaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            WorkingFile.file.Save(SaveDialog.FileName);
            UpdateJoints();
        }

        private void LoopCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            WorkingFile.file.SetLoop(LoopCheckbox.Checked);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenDialog.ShowDialog();
        }

        private void UpdateJoints()
        {
            JointList.Items.Clear();
            foreach (LLJoint joint in WorkingFile.file.Joints)
            {
                JointList.Items.Add(joint.Name);
            }
            UpdateControls();
        }

        private void UpdateControls()
        {
            int jointb = GetSelectedJoint();
            if (jointb != -1)
            {
                JointPriority.ValueChanged -= JointPriority_ValueChanged;
                JointPriority.Value = WorkingFile.file.Joints[jointb].GetPriority();
                JointPriority.ValueChanged += JointPriority_ValueChanged;
                RotKeyframes.Items.Clear();
                PosKeyframes.Items.Clear();
                foreach (LLJointPosition pos in WorkingFile.file.Joints[jointb].Positions)
                {
                    PosKeyframes.Items.Add(pos.GetTime(WorkingFile.file.GetDuration()).ToString());
                }
                foreach (LLJointRotation rot in WorkingFile.file.Joints[jointb].Rotations)
                {
                    RotKeyframes.Items.Add(rot.GetTime(WorkingFile.file.GetDuration()).ToString());
                }
            }
            else
            {
                PosKeyframes.Items.Clear();
                RotKeyframes.Items.Clear();
            }
            MajorVersion.Value = WorkingFile.file.MajorVersion;
            MinorVersion.Value = WorkingFile.file.MinorVersion;
            Priority.Value = (decimal)WorkingFile.file.GetPriority();
            Duration.Value = (decimal)WorkingFile.file.GetDuration();
            LoopInTime.Value = (decimal)WorkingFile.file.LoopInPoint;
            LoopOutTime.Value = (decimal)WorkingFile.file.LoopOutPoint;
            EaseInDuration.Value = (decimal)WorkingFile.file.EaseInDuration;
            EaseOutDuration.Value = (decimal)WorkingFile.file.EaseOutDuration;
            HandPose.Value = WorkingFile.file.GetHandPose();
            LoopCheckbox.Checked = WorkingFile.file.GetLoop();
        }

        private void JointList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
                UpdateControls();
        }

        private void AddJoint_Click(object sender, EventArgs e)
        {
            string jointname = (string)AddJointSelect.SelectedItem;
            WorkingFile.file.AddJoint(jointname);
            UpdateJoints();
        }

        private void RemoveJoint_Click(object sender, EventArgs e)
        {
            string jointname = (string)JointList.SelectedItem;
            WorkingFile.file.RemoveJoint(jointname);
            UpdateJoints();
        }

        private void AddRotKey_Click(object sender, EventArgs e)
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                WorkingFile.file.Joints[joint].AddRot(0, 0, 0, 0);
                UpdateRotList();
            }
        }

        private void AddPosKey_Click(object sender, EventArgs e)
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                WorkingFile.file.Joints[joint].AddPos(0, 32767, 32767, 32767);
                UpdatePosList();
            }
        }

        private void UpdateRotList()
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                RotKeyframes.Items.Clear();
                foreach (LLJointRotation rot in WorkingFile.file.Joints[joint].Rotations)
                {
                    RotKeyframes.Items.Add(rot.GetTime(WorkingFile.file.GetDuration()).ToString());
                }
            }
        }

        private void UpdatePosList()
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                PosKeyframes.Items.Clear();
                foreach (LLJointPosition pos in WorkingFile.file.Joints[joint].Positions)
                {
                    PosKeyframes.Items.Add(pos.GetTime(WorkingFile.file.GetDuration()).ToString());
                }
            }
        }

        private void RemoveRotKey_Click(object sender, EventArgs e)
        {
            if (RotKeyframes.SelectedIndex != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].RemoveRot(RotKeyframes.SelectedIndex);
                    UpdateRotList();
                }
            }
        }

        private void RemovePosKey_Click(object sender, EventArgs e)
        {
            if (PosKeyframes.SelectedIndex != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].RemovePos(PosKeyframes.SelectedIndex);
                    UpdatePosList();
                }
            }
        }

        private void RotX_ValueChanged(object sender, EventArgs e)
        {
            int index = RotKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Rotations[index].SetX((float)RotX.Value);
                }
            }
        }

        private int GetSelectedJoint()
        {
            string jointname = (string)JointList.SelectedItem;
            return WorkingFile.file.GetJoint(jointname);
        }

        private void RotKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = RotKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    RotX.ValueChanged -= RotX_ValueChanged;
                    RotY.ValueChanged -= RotY_ValueChanged;
                    RotZ.ValueChanged -= RotZ_ValueChanged;
                    RotTime.ValueChanged -= RotTime_ValueChanged;
                    RotX.Value = (decimal)WorkingFile.file.Joints[joint].Rotations[index].GetX();
                    RotY.Value = (decimal)WorkingFile.file.Joints[joint].Rotations[index].GetY();
                    RotZ.Value = (decimal)WorkingFile.file.Joints[joint].Rotations[index].GetZ();
                    RotTime.Value = (decimal)WorkingFile.file.Joints[joint].Rotations[index].GetTime(WorkingFile.file.GetDuration());
                    RotX.ValueChanged += RotX_ValueChanged;
                    RotY.ValueChanged += RotY_ValueChanged;
                    RotZ.ValueChanged += RotZ_ValueChanged;
                    RotTime.ValueChanged += RotTime_ValueChanged;
                }
            }
        }

        private void PosKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = PosKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    PosX.ValueChanged -= PosX_ValueChanged;
                    PosY.ValueChanged -= PosY_ValueChanged;
                    PosZ.ValueChanged -= PosZ_ValueChanged;
                    PosTime.ValueChanged -= PosTime_ValueChanged;
                    PosX.Value = (decimal)WorkingFile.file.Joints[joint].Positions[index].GetX();
                    PosY.Value = (decimal)WorkingFile.file.Joints[joint].Positions[index].GetY();
                    PosZ.Value = (decimal)WorkingFile.file.Joints[joint].Positions[index].GetZ();
                    PosTime.Value = (decimal)WorkingFile.file.Joints[joint].Positions[index].GetTime(WorkingFile.file.GetDuration());
                    PosX.ValueChanged += PosX_ValueChanged;
                    PosY.ValueChanged += PosY_ValueChanged;
                    PosZ.ValueChanged += PosZ_ValueChanged;
                    PosTime.ValueChanged += PosTime_ValueChanged;
                }
            }
        }

        private void Priority_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.SetPriority((int)Priority.Value);
        }

        private void Duration_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.SetDuration((float)Duration.Value);
            PosTime.Maximum = Duration.Value;
            RotTime.Maximum = Duration.Value;
            LoopInTime.Maximum = Duration.Value;
            LoopOutTime.Maximum = Duration.Value;
            UpdateControls();
        }

        private void LoopInTime_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.LoopInPoint = (float)LoopInTime.Value;
        }

        private void LoopOutTime_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.LoopOutPoint = (float)LoopOutTime.Value;
        }

        private void EaseInDuration_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.EaseInDuration = (float)EaseInDuration.Value;
        }

        private void EaseOutDuration_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.EaseOutDuration = (float)EaseOutDuration.Value;
        }

        private void RotY_ValueChanged(object sender, EventArgs e)
        {
            int index = RotKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Rotations[index].SetY((float)RotY.Value);
                }
            }
        }

        private void RotZ_ValueChanged(object sender, EventArgs e)
        {
            int index = RotKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Rotations[index].SetZ((float)RotZ.Value);
                }
            }
        }

        private void RotTime_ValueChanged(object sender, EventArgs e)
        {
            int index = RotKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Rotations[index].SetTime((float)RotTime.Value, WorkingFile.file.GetDuration());
                    UpdateControls();
                }
            }
        }

        private void PosX_ValueChanged(object sender, EventArgs e)
        {
            int index = PosKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Positions[index].SetX((float)PosX.Value);
                }
            }
        }

        private void PosY_ValueChanged(object sender, EventArgs e)
        {
            int index = PosKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Positions[index].SetY((float)PosY.Value);
                }
            }
        }

        private void PosZ_ValueChanged(object sender, EventArgs e)
        {
            int index = PosKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Positions[index].SetZ((float)PosZ.Value);
                }
            }
        }

        private void PosTime_ValueChanged(object sender, EventArgs e)
        {
            int index = PosKeyframes.SelectedIndex;
            if (index != -1)
            {
                int joint = GetSelectedJoint();
                if (joint != -1)
                {
                    WorkingFile.file.Joints[joint].Positions[index].SetTime((float)PosTime.Value, WorkingFile.file.GetDuration());
                    UpdateControls();
                }
            }
        }

        private void JointPriority_ValueChanged(object sender, EventArgs e)
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                WorkingFile.file.Joints[joint].SetPriority((int)JointPriority.Value);
            }
        }

        private void HandPose_ValueChanged(object sender, EventArgs e)
        {
            WorkingFile.file.SetHandPose((UInt32)HandPose.Value);
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            int joint = GetSelectedJoint();
            if (joint != -1)
            {
                if (AddJointSelect.SelectedIndex != -1)
                {
                    //Duplicate joints are bad. :<
                    if (WorkingFile.file.GetJoint((string)AddJointSelect.SelectedItem) == -1)
                    {
                        WorkingFile.file.Joints[joint].Name = (string)AddJointSelect.SelectedItem;
                        UpdateJoints();
                    }
                }
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            MergeOpenDialog.ShowDialog();
        }

        private void MergeOpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            LLAnimationFile temp = new LLAnimationFile();
            temp.Parse(MergeOpenDialog.FileName);
            float tempdur = temp.GetDuration();
            float newDuration = WorkingFile.file.GetDuration();
            foreach (LLJoint joint in temp.Joints)
            {
                //Not currently possible to add duplicate joints.
                WorkingFile.file.AddJoint(joint.Name);
                int workjoint = WorkingFile.file.GetJoint(joint.Name);
                if (workjoint == -1)
                    throw (new Exception("[MERGE]: Added a joint but it was not added. What?"));
                foreach (LLJointRotation rot in joint.Rotations)
                {
                    UInt16 rotTI = Tools.F32_to_U16(rot.GetTime(tempdur), 0, newDuration);
                    WorkingFile.file.Joints[workjoint].AddRot(rotTI, rot.X, rot.Y, rot.Z);
                }
                foreach (LLJointPosition pos in joint.Positions)
                {
                    UInt16 posTI = Tools.F32_to_U16(pos.GetTime(tempdur), 0, newDuration);
                    WorkingFile.file.Joints[workjoint].AddPos(posTI, pos.X, pos.Y, pos.Z);
                }
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            Tooltips.Active = !Tooltips.Active;
            if (Tooltips.Active)
                HelpButton.Text = "!?";
            else
                HelpButton.Text = "?";
        }
    }
}