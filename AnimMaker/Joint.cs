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

namespace AnimMaker
{
    public class LLJoint
    {
        public string Name; // Variable length string. Up to 127 characters according to LL. null term
        int Priority;
        //public long RotationCount;
        public List<LLJointRotation> Rotations;
        //public long PositionCount;
        public List<LLJointPosition> Positions;
        public LLJoint(string jointName)
        {
            Name = jointName;
            Priority = 0;
            Rotations = new List<LLJointRotation>();
            Positions = new List<LLJointPosition>();
        }
        public void SetDuration(float NewDuration, float OldDuration)
        {
            foreach (LLJointPosition i in Positions)
            {
                i.SetTime(i.GetTime(OldDuration), NewDuration);
            }
            foreach (LLJointRotation i in Rotations)
            {
                i.SetTime(i.GetTime(OldDuration), NewDuration);
            }
        }

        public void SetPriority(int NewPriority)
        {
            Priority = Tools.clamp(NewPriority, -1, 4);
        }

        public int GetPriority()
        {
            return Priority;
        }

        public void AddPos(UInt16 intime, UInt16 inx, UInt16 iny, UInt16 inz)
        {
            Positions.Add(new LLJointPosition(intime, inx, iny, inz));
        }

        public void AddRot(UInt16 intime, UInt16 inx, UInt16 iny, UInt16 inz)
        {
            Rotations.Add(new LLJointRotation(intime, inx, iny, inz));
        }

        public void AddRot(float intime, float indur, float inx, float iny, float inz)
        {
            Rotations.Add(new LLJointRotation(intime, indur, inx, iny, inz));
        }

        public void AddPos(float intime, float indur, float inx, float iny, float inz)
        {
            Positions.Add(new LLJointPosition(intime, indur, inx, iny, inz));
        }

        public void RemovePos(int Index)
        {
            Positions.RemoveAt(Index);
        }

        public void RemoveRot(int Index)
        {
            Rotations.RemoveAt(Index);
        }
    }

    public class LLJointPosition
    {
        public UInt16 Time;
        public UInt16 X;
        public UInt16 Y;
        public UInt16 Z;

        public LLJointPosition(float intime, float indur, float inx, float iny, float inz)
        {
            SetTime(intime, indur);
            SetX(inx);
            SetY(iny);
            SetZ(inz);
        }

        public LLJointPosition(UInt16 intime, UInt16 inx, UInt16 iny, UInt16 inz)
        {
            Time = intime;
            X = inx;
            Y = iny;
            Z = inz;
        }

        public float GetTime(float Duration)
        {
            return Tools.U16_to_F32(Time, 0, Duration);
        }

        public void SetTime(float newTime, float Duration)
        {
            Time = Tools.F32_to_U16(newTime, 0, Duration);
        }

        public float GetX()
        {
            return Tools.U16_to_F32(X, -5, 5);
        }
        public float GetY()
        {
            return Tools.U16_to_F32(Y, -5, 5);
        }

        public float GetZ()
        {
            return Tools.U16_to_F32(Z, -5, 5);
        }

        public void SetX(float Pos)
        {
            X = Tools.F32_to_U16(Tools.clamp(Pos, -5, 5), -5, 5);
        }

        public void SetY(float Pos)
        {
            Y = Tools.F32_to_U16(Tools.clamp(Pos, -5, 5), -5, 5);
        }

        public void SetZ(float Pos)
        {
            Z = Tools.F32_to_U16(Tools.clamp(Pos, -5, 5), -5, 5);
        }
    }

    /*
     * Rotations within animation files are expressed as three U16 values converted from floats.
     * Floats have a range of -1 to 1 with -1 representing 360 degrees and 1 180 degrees.
     * Any angle > 180 is represented as its value > 180 added to -180.
     * Each U16 represents the XYZ rotation axii, in order.
     */
    public class LLJointRotation
    {
        public UInt16 Time;
        public UInt16 X;
        public UInt16 Y;
        public UInt16 Z;

        public LLJointRotation(float intime, float indur, float inx, float iny, float inz)
        {
            SetTime(intime, indur);
            SetX(inx);
            SetY(iny);
            SetZ(inz);
        }

        public LLJointRotation(UInt16 intime, UInt16 inx, UInt16 iny, UInt16 inz)
        {
            Time = intime;
            X = inx;
            Y = iny;
            Z = inz;
        }

        public float GetTime(float Duration)
        {
            return Tools.U16_to_F32(Time, 0, Duration);
        }

        public void SetTime(float newTime, float Duration)
        {
            Time = Tools.F32_to_U16(newTime, 0, Duration);
        }

        public float GetX()
        {
            return Tools.DenormalizeAngle(Tools.U16_to_F32(X, -1, 1));
        }
        public float GetY()
        {
            return Tools.DenormalizeAngle(Tools.U16_to_F32(Y, -1, 1));
        }

        public float GetZ()
        {
            return Tools.DenormalizeAngle(Tools.U16_to_F32(Z, -1, 1));
        }

        public void SetX(float Angle)
        {
            X = Tools.F32_to_U16(Tools.NormalizeAngle(Angle), -1, 1);
        }

        public void SetY(float Angle)
        {
            Y = Tools.F32_to_U16(Tools.NormalizeAngle(Angle), -1, 1);
        }

        public void SetZ(float Angle)
        {
            Z = Tools.F32_to_U16(Tools.NormalizeAngle(Angle), -1, 1);
        }
    }
}
