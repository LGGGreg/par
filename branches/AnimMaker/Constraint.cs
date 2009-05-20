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
    public struct Vector
    {
        public float x;
        public float y;
        public float z;
    };

    public class LLConstraint
    {
        public enum EConstraintType
        {
            PointConstraint,
            PlaneConstraint
        };
        byte ChainLength;
        public EConstraintType ConstraintType;
        public string SourceVolume; //byte[16]
        public float SourceOffsetX;
        public float SourceOffsetY;
        public float SourceOffsetZ;
        public string TargetVolume; //byte[16]
        public float TargetOffsetX;
        public float TargetOffsetY;
        public float TargetOffsetZ;
        public float TargetDirectionX;
        public float TargetDirectionY;
        public float TargetDirectionZ;
        public float EaseInStart;
        public float EaseInStop;
        public float EaseOutStart;
        public float EaseOutStop;

        LLConstraint()
        {
            ConstraintType = EConstraintType.PointConstraint;
            //ASCIIEncoding byteconverter;
            SourceVolume = "INVALID"; //byteconverter.GetBytes("INVALID");
            TargetVolume = "INVALID"; //byteconverter.GetBytes("INVALID");
        }

        LLConstraint(byte chainlen, EConstraintType type, string SourceName, string TargetName, Vector sourceoff, 
            Vector targetoff, Vector targetdir, float easeinstart, float easeinstop,
            float easeoutstart, float easeoutstop)
        {
            ChainLength = chainlen;
            ConstraintType = type;
            SourceVolume = SourceName;
            TargetVolume = TargetName;
            SourceOffsetX = sourceoff.x;
            SourceOffsetY = sourceoff.y;
            SourceOffsetZ = sourceoff.z;
            TargetOffsetX = targetoff.x;
            TargetOffsetY = targetoff.y;
            TargetOffsetZ = targetoff.z;
            TargetDirectionX = targetdir.x;
            TargetDirectionY = targetdir.y;
            TargetDirectionZ = targetdir.z;
            EaseInStart = easeinstart;
            EaseInStop = easeinstop;
            EaseOutStart = easeoutstart;
            EaseOutStop = easeoutstop;
        }

        byte[] GetSourceBytes()
        {
            byte[] output = new byte[16];
            ASCIIEncoding converter = new ASCIIEncoding();
            output = converter.GetBytes(SourceVolume);
            return output;
        }

        byte[] GetTargetBytes()
        {
            byte[] output = new byte[16];
            ASCIIEncoding converter = new ASCIIEncoding();
            output = converter.GetBytes(TargetVolume);
            return output;
        }

        byte GetChainLength()
        {
            return ChainLength;
        }

        void SetChainLength(byte newlen)
        {
            if(newlen > WorkingFile.file.Joints.Count)
                newlen = (byte)(WorkingFile.file.Joints.Count - 1);
            if (newlen < 0)
                newlen = 0;
            ChainLength = newlen;
        }
    }
}
