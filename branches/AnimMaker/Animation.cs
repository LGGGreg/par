/* Copyright (c) 2009, Jeffrey MacLellan(Zwagoth Klaar)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the Jeffrey MacLellan nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnimMaker
{
    public class LLAnimationFile
    {
        public UInt16 MajorVersion; // Should always be 1
        public UInt16 MinorVersion; // Should always be 0
        int BasePriority; // 0-4 Normal. 5-7 valid. Clamp to 4.
        float Duration;
        public string Emote; // Variable length string. Up to 127 characters according to LL;
        public float LoopInPoint;
        public float LoopOutPoint;
        int Loop; // BOOL
        public float EaseInDuration;
        public float EaseOutDuration;
        uint HandPose; // clamp to 14
        //ulong JointCount;
        public List<LLJoint> Joints;
        public List<LLConstraint> Constraints;

        public LLAnimationFile()
        {
            Joints = new List<LLJoint>();
            Constraints = new List<LLConstraint>();
            Emote = "";
        }
        public void SetDuration(float NewDuration)
        {
            foreach (LLJoint i in Joints)
            {
                i.SetDuration(NewDuration, Duration);
            }
            Duration = NewDuration;
        }

        public float GetDuration()
        {
            return Duration;
        }

        public void SetHandPose(ulong pose)
        {
            HandPose = (uint)Tools.clamp((int)pose, 0, 14);
        }

        public ulong GetHandPose()
        {
            return HandPose;
        }

        public void SetPriority(int NewPriority)
        {
            BasePriority = Tools.clamp(NewPriority, 0, 4);
        }

        public int GetPriority()
        {
            return BasePriority;
        }

        public void SetLoop(bool loop)
        {
            if (loop)
                Loop = 1;
            else
                Loop = 0;
        }

        public bool GetLoop()
        {
            if (Loop != 0)
                return true;
            return false;
        }

        public int GetJoint(string name)
        {
            foreach (LLJoint joint in Joints)
            {
                if (joint.Name == name)
                    return Joints.IndexOf(joint);
            }
            return -1;
        }

        //TODO: Error handling.
        public void Parse(string FileName)
        {
            Joints.Clear();
            Constraints.Clear();
            Emote = "";
            FileStream file = new FileStream(FileName, FileMode.Open);
            byte[] buffer = new byte[256];
            file.Read(buffer, 0, sizeof(ushort));
            MajorVersion = BitConverter.ToUInt16(buffer, 0);
            file.Read(buffer, 0, sizeof(ushort));
            MinorVersion = BitConverter.ToUInt16(buffer, 0);
            file.Read(buffer, 0, sizeof(Int32));
            BasePriority = BitConverter.ToInt32(buffer, 0);
            file.Read(buffer, 0, sizeof(float));
            Duration = BitConverter.ToSingle(buffer, 0);
            //HACK: hack to make C# read null terminated scripts of undefined length.
            //TODO: Find a better way to do this.
            file.Read(buffer, 0 , 1);
            while (buffer[0] != 0)
            {
                Emote += (char)buffer[0];
                file.Read(buffer, 0, 1);
            }
            file.Read(buffer, 0, ((sizeof(float)*4)+(sizeof(int)*3)));
            LoopInPoint = BitConverter.ToSingle(buffer, 0);
            LoopOutPoint = BitConverter.ToSingle(buffer, sizeof(float));
            Loop = BitConverter.ToInt32(buffer, sizeof(float)*2);
            EaseInDuration = BitConverter.ToSingle(buffer, ((sizeof(float)*2)+sizeof(int)));
            EaseInDuration = BitConverter.ToSingle(buffer, ((sizeof(float)*3)+sizeof(int)));
            HandPose = BitConverter.ToUInt32(buffer, (sizeof(float) * 4) + sizeof(int));
            Int32 JointCount = BitConverter.ToInt32(buffer, ((sizeof(float)*4)+sizeof(int)*2));
            for(int curjoint = 0; curjoint < JointCount; ++curjoint)
            {
                string jointName = "";
                file.Read(buffer, 0 , 1);
                while (buffer[0] != 0)
                {
                    //HACK: Make sure C# does not try to read things as unicode characters.
                    //Do not use BitConverter.ToChar here.
                    jointName += (char)buffer[0];
                    file.Read(buffer, 0, 1);
                }
                LLJoint newJoint = new LLJoint(jointName);
                file.Read(buffer, 0, sizeof(int) * 2);
                newJoint.SetPriority(BitConverter.ToInt32(buffer, 0));
                int RotCount = BitConverter.ToInt32(buffer, sizeof(int));
                for(int curRot = 0; curRot < RotCount; ++curRot)
                {
                    UInt16 time, x, y, z = 0;
                    file.Read(buffer, 0, sizeof(UInt16)*4);
                    time = BitConverter.ToUInt16(buffer, 0);
                    x = BitConverter.ToUInt16(buffer, sizeof(UInt16));
                    y = BitConverter.ToUInt16(buffer, sizeof(UInt16)*2);
                    z = BitConverter.ToUInt16(buffer, sizeof(UInt16)*3);
                    newJoint.AddRot(time, x, y, z);
                }
                file.Read(buffer, 0, sizeof(int));
                int PosCount = BitConverter.ToInt32(buffer, 0);
                for(int curPos = 0; curPos < PosCount; ++curPos)
                {
                    UInt16 time, x, y, z = 0;
                    file.Read(buffer, 0, sizeof(UInt16)*4);
                    time = BitConverter.ToUInt16(buffer, 0);
                    x = BitConverter.ToUInt16(buffer, sizeof(UInt16));
                    y = BitConverter.ToUInt16(buffer, sizeof(UInt16)*2);
                    z = BitConverter.ToUInt16(buffer, sizeof(UInt16)*3);
                    newJoint.AddPos(time, x, y, z);
                }
                Joints.Add(newJoint);    
            }
            //TODO: Read constraints here instead of ignoring them.
            file.Close();
        }

        //TODO: Error handling.
        public void Save(string FileName)
        {
            FileStream file = new FileStream(FileName, FileMode.Create);
            file.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            file.Write(BitConverter.GetBytes((ushort)0), 0, 2);
            file.Write(BitConverter.GetBytes(BasePriority), 0, 4);
            file.Write(BitConverter.GetBytes(Duration), 0, 4);
            //HACK: C# does not like writing strings with null termination(or in ASCII).
            byte[] emote = StrToByteArray(Emote);
            file.Write(emote, 0, Emote.Length);
            file.Write(new byte[1], 0, 1);
            file.Write(BitConverter.GetBytes(LoopInPoint), 0, 4);
            file.Write(BitConverter.GetBytes(LoopOutPoint), 0, 4);
            file.Write(BitConverter.GetBytes(Loop), 0, 4);
            file.Write(BitConverter.GetBytes(EaseInDuration), 0, 4);
            file.Write(BitConverter.GetBytes(EaseOutDuration), 0, 4);
            file.Write(BitConverter.GetBytes(HandPose), 0, 4);
            file.Write(BitConverter.GetBytes(Joints.Count), 0, 4);
            foreach(LLJoint joint in Joints)
            {
                //Joint name.
                //$J#& C#, null terminating was obnoxious.
                byte[] name = StrToByteArray(joint.Name);
                file.Write(name, 0, joint.Name.Length);
                file.Write(new byte[1], 0, 1);
                file.Write(BitConverter.GetBytes(joint.GetPriority()), 0, 4);
                //Number of rotation keyframes.
                file.Write(BitConverter.GetBytes(joint.Rotations.Count), 0, 4);
                foreach(LLJointRotation rot in joint.Rotations)
                {
                    file.Write(BitConverter.GetBytes(rot.Time), 0, 2);
                    file.Write(BitConverter.GetBytes(rot.X), 0, 2);
                    file.Write(BitConverter.GetBytes(rot.Y), 0, 2);
                    file.Write(BitConverter.GetBytes(rot.Z), 0, 2);
                }
                //Number of position keyframes.
                file.Write(BitConverter.GetBytes(joint.Positions.Count), 0, 4);
                foreach(LLJointPosition pos in joint.Positions)
                {
                    file.Write(BitConverter.GetBytes(pos.Time), 0, 2);
                    file.Write(BitConverter.GetBytes(pos.X), 0, 2);
                    file.Write(BitConverter.GetBytes(pos.Y), 0, 2);
                    file.Write(BitConverter.GetBytes(pos.Z), 0, 2);
                }
            }
            //TODO: Constraints go here.
            //Number of constraints.
            file.Write(BitConverter.GetBytes((int)0), 0, 4);
            file.Close();
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public void AddJoint(string name)
        {
            if(!JointExists(name))
            {
                Joints.Add(new LLJoint(name));
            }
        }

        public void RemoveJoint(string name)
        {
            foreach (LLJoint joint in Joints)
            {
                if (joint.Name == name)
                {
                    Joints.Remove(Joints[Joints.IndexOf(joint)]);
                    return;
                }
            }
        }

        private bool JointExists(string name)
        {
            foreach (LLJoint joint in Joints)
            {
                if (joint.Name == name)
                    return true;
            }
            return false;
        }
    }
}
