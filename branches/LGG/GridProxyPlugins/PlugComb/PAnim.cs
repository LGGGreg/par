/*
 * Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back)
 *All rights reserved.
 *
 *Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

 *THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 * 
 * Copyright (c) 2009, Jeffrey MacLellan(Zwagoth Klaar)
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
using System.Security.Cryptography;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
using System.Threading;
using System.Windows.Forms;

namespace PubComb
{
    public class PAnim : GTabPlug
    {
        
        public bool gdebug = false;
        static class Tools
        {
            static public float clamp(float input, float lower, float upper)
            {
                input = (input < lower) ? lower : input;
                input = (input > upper) ? upper : input;
                return input;
            }

            static public int clamp(int input, int lower, int upper)
            {
                input = (input < lower) ? lower : input;
                input = (input > upper) ? upper : input;
                return input;
            }

            //The way LL uncompresses floats from a U16. Fudge for 0 included;
            static public float U16_to_F32(UInt16 input, float lower, float upper)
            {
                float temp = input * (1 / (float)65535);
                temp *= (upper - lower);
                temp += lower;
                float delta = (upper - lower);
                if (Math.Abs(temp) < (delta * ((float)1 / (float)65535)))
                {
                    temp = 0;
                }
                return temp;
            }

            //The way LL compresses a float into a U16.
            static public UInt16 F32_to_U16(float input, float lower, float upper)
            {
                input = clamp(input, lower, upper);
                input -= lower;
                input /= (upper - lower);
                input *= 65535;
                input = (float)Math.Floor(input);
                return (UInt16)input;
            }

            // Used to normalize angles to their rotational float value of between -1 and 1;
            static public float NormalizeAngle(float Angle)
            {
                if (Angle > 180)
                    Angle -= 360;
                Angle /= 360;
                return clamp(Angle, -1, 1);
            }

            static public float DenormalizeAngle(float NormAngle)
            {
                NormAngle *= 360;
                if (NormAngle < 0)
                    NormAngle += 360;
                return clamp(NormAngle, 0, 360);
            }
        }
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

            public LLConstraint()
            {
                ConstraintType = EConstraintType.PointConstraint;
                //ASCIIEncoding byteconverter;
                SourceVolume = "INVALID"; //byteconverter.GetBytes("INVALID");
                TargetVolume = "INVALID"; //byteconverter.GetBytes("INVALID");
            }

            public LLConstraint(byte chainlen, EConstraintType type, string SourceName, string TargetName, Vector3 sourceoff,
                Vector3 targetoff, Vector3 targetdir, float easeinstart, float easeinstop,
                float easeoutstart, float easeoutstop)
            {
                ChainLength = chainlen;
                ConstraintType = type;
                SourceVolume = SourceName;
                TargetVolume = TargetName;
                SourceOffsetX = sourceoff.X;
                SourceOffsetY = sourceoff.Y;
                SourceOffsetZ = sourceoff.Z;
                TargetOffsetX = targetoff.X;
                TargetOffsetY = targetoff.Y;
                TargetOffsetZ = targetoff.Z;
                TargetDirectionX = targetdir.X;
                TargetDirectionY = targetdir.Y;
                TargetDirectionZ = targetdir.Z;
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

            void SetChainLength(byte newlen, int jcount)
            {
                if (newlen > jcount)
                    newlen = (byte)(jcount - 1);
                if (newlen < 0)
                    newlen = 0;
                ChainLength = newlen;
            }
        }
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
        List<LLJoint> Joints;
        List<LLConstraint> Constraints;
        class gd1
        {
            public gd1()
            {
                size = 0;
                sizedone = 0;
            }
            public int sizedone;
            public byte[] bytes;
            public int size;
        }
        #region joinnames
        public string[] newjoints = 
        {
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
            "Stomach"
        };
        public string[] oldjoints =
        {
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
            "Mouth",
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
            "Stomach"
        };
        #endregion jointnames
        #region llanimsa
        public string[] llanims = {     "05ddbff8-aaa9-92a1-2b74-8fe77a29b445",
                                        "6ed24bd8-91aa-4b12-ccc7-c97c857ab4e0",
                                        "2d6daa51-3192-6794-8e2e-a15f8338ec30",
                                        "2408fe9e-df1d-1d7d-f4ff-1384fa7b350f",
                                        "15468e00-3400-bb66-cecc-646d7c14458e",
                                        "7a4e87fe-de39-6fcb-6223-024b00893244",
                                        "2305bd75-1ca9-b03b-1faa-b176b8a8c49e",
                                        "7a17b059-12b2-41b1-570a-186368b6aa6f",
                                        "201f3fdf-cb1f-dbec-201f-7333e328ae7c",
                                        "56e0ba0d-4a9f-7f27-6117-32f2ebbf6135",
                                        "370f3a20-6ca6-9971-848c-9a01bc42ae3c",
                                        "b5b4a67d-0aee-30d2-72cd-77b333e932ef",
                                        "46bb4359-de38-4ed8-6a22-f1f52fe8f506",
                                        "3147d815-6338-b932-f011-16b56d9ac18b",
                                        "ea633413-8006-180a-c3ba-96dd1d756720",
                                        "5747a48e-073e-c331-f6f3-7c2149613d3e",
                                        "fd037134-85d4-f241-72c6-4f42164fedee",
                                        "c4ca6188-9127-4f31-0158-23c4e2f93304",
                                        "18b3a4b5-b463-bd48-e4b6-71eaac76c515",
                                        "db84829b-462c-ee83-1e27-9bbee66bd624",
                                        "b906c4ba-703b-1940-32a3-0c7f7d791510",
                                        "82e99230-c906-1403-4d9c-3889dd98daba",
                                        "349a3801-54f9-bf2c-3bd0-1ac89772af01",
                                        "efcf670c-2d18-8128-973a-034ebc806b67",
                                        "9b0c1c4e-8ac7-7969-1494-28c874c4f668",
                                        "9ba1c942-08be-e43a-fb29-16ad440efc50",
                                        "47f5f6fb-22e5-ae44-f871-73aaaf4a6022",
                                        "92624d3e-1068-f1aa-a5ec-8244585193ed",
                                        "038fcec9-5ebd-8a8e-0e2e-6e71a0a1ac53",
                                        "6883a61a-b27b-5914-a61e-dda118a9ee2c",
                                        "b68a3d7c-de9e-fc87-eec8-543d787e5b0d",
                                        "928cae18-e31d-76fd-9cc9-2f55160ff818",
                                        "30047778-10ea-1af7-6881-4db7a3a5a114",
                                        "951469f4-c7b2-c818-9dee-ad7eea8c30b7",
                                        "4bd69a1d-1114-a0b4-625f-84e0a5237155",
                                        "cd28b69b-9c95-bb78-3f94-8d605ff1bb12",
                                        "a54d8ee2-28bb-80a9-7f0c-7afbbe24a5d6",
                                        "b0dc417c-1f11-af36-2e80-7e7489fa7cdc",
                                        "57abaae6-1d17-7b1b-5f98-6d11a6411276",
                                        "0f86e355-dd31-a61c-fdb0-3a96b9aad05f",
                                        "514af488-9051-044a-b3fc-d4dbf76377c6",
                                        "aa2df84d-cf8f-7218-527b-424a52de766e",
                                        "1a03b575-9634-b62a-5767-3a679e81f4de",
                                        "214aa6c1-ba6a-4578-f27c-ce7688f61d0d",
                                        "d535471b-85bf-3b4d-a542-93bea4f59d33",
                                        "d4416ff1-09d3-300f-4183-1b68a19b9fc1",
                                        "0b8c8211-d78c-33e8-fa28-c51a9594e424",
                                        "fee3df48-fa3d-1015-1e26-a205810e3001",
                                        "1e8d90cc-a84e-e135-884c-7c82c8b03a14",
                                        "62570842-0950-96f8-341c-809e65110823",
                                        "d63bc1f9-fc81-9625-a0c6-007176d82eb7",
                                        "f76cda94-41d4-a229-2872-e0296e58afe1",
                                        "eb6ebfb2-a4b3-a19c-d388-4dd5c03823f7",
                                        "a351b1bc-cc94-aac2-7bea-a7e6ebad15ef",
                                        "b7c7c833-e3d3-c4e3-9fc0-131237446312",
                                        "728646d9-cc79-08b2-32d6-937f0a835c24",
                                        "835965c6-7f2f-bda2-5deb-2478737f91bf",
                                        "b92ec1a5-e7ce-a76b-2b05-bcdb9311417e",
                                        "da020525-4d94-59d6-23d7-81fdebf33148",
                                        "9c05e5c7-6f07-6ca4-ed5a-b230390c3950",
                                        "666307d9-a860-572d-6fd4-c3ab8865c094",
                                        "f5fc7433-043d-e819-8298-f519a119b688",
                                        "c1bc7f36-3ba0-d844-f93c-93be945d644f",
                                        "7db00ccd-f380-f3ee-439d-61968ec69c8a",
                                        "aec4610c-757f-bc4e-c092-c6e9caf18daf",
                                        "2b5a38b2-5e00-3a97-a495-4c826bc443e6",
                                        "9b29cd61-c45b-5689-ded2-91756b8d76a9",
                                        "ef62d355-c815-4816-2474-b1acc21094a6",
                                        "8b102617-bcba-037b-86c1-b76219f90c88",
                                        "efdc1727-8b8a-c800-4077-975fc27ee2f2",
                                        "3d94bad0-c55b-7dcc-8763-033c59405d33",
                                        "7570c7b5-1f22-56dd-56ef-a9168241bbb6",
                                        "4ae8016b-31b9-03bb-c401-b1ea941db41d",
                                        "20f063ea-8306-2562-0b07-5c853b37b31e",
                                        "62c5de58-cb33-5743-3d07-9e4cd4352864",
                                        "5ea3991f-c293-392e-6860-91dfa01278a3",
                                        "709ea28e-1573-c023-8bf8-520c8bc637fa",
                                        "19999406-3a3a-d58c-a2ac-d72e555dcf51",
                                        "ca5b3f14-3194-7a2b-c894-aa699b718d1f",
                                        "f4f00d6e-b9fe-9292-f4cb-0ae06ea58d57",
                                        "08464f78-3a8e-2944-cba5-0c94aff3af29",
                                        "315c3a41-a5f3-0ba4-27da-f893f769e69b",
                                        "5a977ed9-7f72-44e9-4c4c-6e913df8ae74",
                                        "d83fa0e5-97ed-7eb2-e798-7bd006215cb4",
                                        "f061723d-0a18-754f-66ee-29a44795a32f",
                                        "eefc79be-daae-a239-8c04-890f5d23654a",
                                        "b312b10e-65ab-a0a4-8b3c-1326ea8e3ed9",
                                        "17c024cc-eef2-f6a0-3527-9869876d7752",
                                        "ec952cca-61ef-aa3b-2789-4d1344f016de",
                                        "f3300ad9-3462-1d07-2044-0fef80062da0",
                                        "c8e42d32-7310-6906-c903-cab5d4a34656",
                                        "36f81a92-f076-5893-dc4b-7c3795e487cf",
                                        "49aea43b-5ac3-8a44-b595-96100af0beda",
                                        "35db4f7e-28c2-6679-cea9-3ee108f7fc7f",
                                        "0836b67f-7f7b-f37b-c00a-460dc1521f5a",
                                        "42dd95d5-0bc6-6392-f650-777304946c0f",
                                        "16803a9f-5140-e042-4d7b-d28ba247c325",
                                        "05ddbff8-aaa9-92a1-2b74-8fe77a29b445",
                                        "0eb702e2-cc5a-9a88-56a5-661a55c0676a",
                                        "cd7668a6-7011-d7e2-ead8-fc69eff1a104",
                                        "e04d450d-fdb5-0432-fd68-818aaf5935f8",
                                        "6bd01860-4ebd-127a-bb3d-d1427e8e0c42",
                                        "70ea714f-3a97-d742-1b01-590a8fcd1db5",
                                        "1a5fe8ac-a804-8a5d-7cbd-56bd83184568",
                                        "b1709c8d-ecd3-54a1-4f28-d55ac0840782",
                                        "1c7600d6-661f-b87b-efe2-d7421eb93c86",
                                        "1a2bd58e-87ff-0df8-0b4c-53e047b0bb6e",
                                        "245f3c54-f1c0-bf2e-811f-46d8eeb386e7",
                                        "a8dee56f-2eae-9e7a-05a2-6fb92b97e21e",
                                        "f2bed5f9-9d44-39af-b0cd-257b2a17fe40",
                                        "d2f2ee58-8ad1-06c9-d8d3-3827ba31567a",
                                        "6802d553-49da-0778-9f85-1599a2266526",
                                        "0a9fb970-8b44-9114-d3a9-bf69cfe804d6",
                                        "eae8905b-271a-99e2-4c0e-31106afd100c",
                                        "3da1d753-028a-5446-24f3-9c9b856d9422",
                                        "42b46214-4b44-79ae-deb8-0df61424ff4b",
                                        "f22fed8b-a5ed-2c93-64d5-bdd8b93c889f",
                                        "80700431-74ec-a008-14f8-77575e73693f",
                                        "1cb562b0-ba21-2202-efb3-30f82cdf9595",
                                        "41426836-7437-7e89-025d-0aa4d10f1d69",
                                        "313b9881-4302-73c0-c7d0-0e7a36b6c224",
                                        "85428680-6bf9-3e64-b489-6f81087c24bd",
                                        "5c682a95-6da4-a463-0bf6-0f5b7be129d1",
                                        "11000694-3f41-adc2-606b-eee1d66f3724",
                                        "aa134404-7dac-7aca-2cba-435f9db875ca",
                                        "83ff59fe-2346-f236-9009-4e3608af64c1",
                                        "c541c47f-e0c0-058b-ad1a-d6ae3a4584d9",
                                        "7693f268-06c7-ea71-fa21-2b30d6533f8f",
                                        "b1ed7982-c68e-a982-7561-52a88a5298c0",
                                        "869ecdad-a44b-671e-3266-56aef2e3ac2e",
                                        "c0c4030f-c02b-49de-24ba-2331f43fe41c",
                                        "9f496bd2-589a-709f-16cc-69bf7df1d36c",
                                        "15dd911d-be82-2856-26db-27659b142875",
                                        "b8c8b2a3-9008-1771-3bfc-90924955ab2d",
                                        "42ecd00b-9947-a97c-400a-bbc9174c7aeb"
                                  };
        #endregion llanimsa
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private ProtAnimForm1 form;
        private string brand;
        private Dictionary<UUID, gd1> dls = new Dictionary<UUID, gd1>();
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "PAnim");
            form.readData();
        }
        public PAnim(PubComb plug)
        {
            plugin = plug;
            //formthread = new Thread(new ThreadStart(delegate()
            //{
                form = new ProtAnimForm1(this);
            //    Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
            //    plug.tabform.addATab(form, "ProtAnim");
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.brand = "ProtAnim";
            this.proxy.AddDelegate(PacketType.TransferRequest, Direction.Outgoing, new PacketDelegate(TransferRequestHandler));
            this.proxy.AddDelegate(PacketType.TransferInfo, Direction.Incoming, new PacketDelegate(TransferInfoHandler));
            this.proxy.AddDelegate(PacketType.TransferPacket, Direction.Incoming, new PacketDelegate(TransferPacketHandler));
        }
        public void writethis(string th, ConsoleColor forg, ConsoleColor back)
        {
            ConsoleColor oldback = Console.BackgroundColor;
            ConsoleColor oldfor = Console.ForegroundColor;
            Console.BackgroundColor = back;
            Console.ForegroundColor = forg;
            Console.WriteLine(th);
            Console.BackgroundColor = oldback;
            Console.ForegroundColor = oldfor;
        }
        public bool Parse(byte [] data)
        {
            
            int protectionLevel = form.getChecked();

            //*thanks zwag!!1
            Joints = new List<LLJoint>();
            Constraints = new List<LLConstraint>();
            UInt16 MajorVersion; // Should always be 1
            int BasePriority; // 0-4 Normal. 5-7 valid. Clamp to 4.
            float Duration;
            string Emote; // Variable length string. Up to 127 characters according to LL;
            float LoopInPoint;
            float LoopOutPoint;
            int Loop; // BOOL
            float EaseInDuration;
            uint HandPose; // clamp to 14

            Joints.Clear();
            Constraints.Clear();
            Emote = "";
            //FileStream file = new FileStream(FileName, FileMode.Open);
            byte[] buffer = new byte[256];
            int cof = 0;
            Buffer.BlockCopy(data, cof, buffer, 0, sizeof(ushort)); cof += sizeof(ushort);
            
            MajorVersion = BitConverter.ToUInt16(buffer, 0);
            Buffer.BlockCopy(data, cof, buffer, 0, sizeof(ushort)); cof += sizeof(ushort);
            //MinorVersion =  BitConverter.ToUInt16(buffer, 0);
            //MinorVersion = 0;
            Buffer.BlockCopy(data, cof, buffer, 0, sizeof(Int32)); cof += sizeof(Int32);

            BasePriority = BitConverter.ToInt32(buffer, 0);
            //file.Read(buffer, 0, sizeof(float));
            Buffer.BlockCopy(data, cof, buffer, 0, sizeof(float)); cof += sizeof(float);

            Duration = BitConverter.ToSingle(buffer, 0);
            //HACK: hack to make C# read null terminated scripts of undefined length.
            //TODO: Find a better way to do this.
            
            //file.Read(buffer, 0, 1);
            Buffer.BlockCopy(data, cof, buffer, 0, 1); cof +=1;

            while (buffer[0] != 0)
            {
                Emote += (char)buffer[0];
                //file.Read(buffer, 0, 1);
                Buffer.BlockCopy(data, cof, buffer, 0, 1); cof += 1;
            }
            Buffer.BlockCopy(data, cof, buffer, 0, ((sizeof(float) * 4) + (sizeof(int) * 3))); cof += ((sizeof(float) * 4) + (sizeof(int) * 3));
            //file.Read(buffer, 0, ((sizeof(float) * 4) + (sizeof(int) * 3)));
            LoopInPoint = BitConverter.ToSingle(buffer, 0);
            LoopOutPoint = BitConverter.ToSingle(buffer, sizeof(float));
            Loop = BitConverter.ToInt32(buffer, sizeof(float) * 2);
            EaseInDuration = BitConverter.ToSingle(buffer, ((sizeof(float) * 2) + sizeof(int)));
            EaseInDuration = BitConverter.ToSingle(buffer, ((sizeof(float) * 3) + sizeof(int)));
            HandPose = BitConverter.ToUInt32(buffer, (sizeof(float) * 4) + sizeof(int));
            Int32 JointCount = BitConverter.ToInt32(buffer, ((sizeof(float) * 4) + sizeof(int) * 2));
            if (JointCount > 50) return false;
            for (int curjoint = 0; curjoint < JointCount; ++curjoint)
            {
                string jointName = "";
                //file.Read(buffer, 0, 1);
                Buffer.BlockCopy(data, cof, buffer, 0, 1); cof += 1;
                while (buffer[0] != 0)
                {
                    //HACK: Make sure C# does not try to read things as unicode characters.
                    //Do not use BitConverter.ToChar here.
                    jointName += (char)buffer[0];
                    //file.Read(buffer, 0, 1);
                    Buffer.BlockCopy(data, cof, buffer, 0, 1); cof += 1;
                
                }
                if (form.getChecked() <= 2)
                {
                    if (!new List<string>(oldjoints).Contains(jointName))
                    {
                        if (gdebug) writethis("File Refused because joing wasnt an old joint", ConsoleColor.Gray, ConsoleColor.DarkRed);
                        return false;
                    }
                }
                else if (form.getChecked() <= 3)
                {
                    if (!new List<string>(newjoints).Contains(jointName))
                    {
                        if (gdebug) writethis("File Refused because joigt wasnt innew joints", ConsoleColor.Gray, ConsoleColor.DarkRed);

                        return false;
                    }
                }

                LLJoint newJoint = new LLJoint(jointName);
                //file.Read(buffer, 0, sizeof(int) * 2);
                Buffer.BlockCopy(data, cof, buffer, 0, sizeof(int) * 2); cof += sizeof(int) * 2;
                
                newJoint.SetPriority(BitConverter.ToInt32(buffer, 0));
                int RotCount = BitConverter.ToInt32(buffer, sizeof(int));
                for (int curRot = 0; curRot < RotCount; ++curRot)
                {
                    UInt16 time, x, y, z = 0;
                    Buffer.BlockCopy(data, cof, buffer, 0, sizeof(UInt16) * 4); cof += sizeof(UInt16) * 4;
                    //file.Read(buffer, 0, sizeof(UInt16) * 4);
                    time = BitConverter.ToUInt16(buffer, 0);
                    x = BitConverter.ToUInt16(buffer, sizeof(UInt16));
                    y = BitConverter.ToUInt16(buffer, sizeof(UInt16) * 2);
                    z = BitConverter.ToUInt16(buffer, sizeof(UInt16) * 3);
                    newJoint.AddRot(time, x, y, z);
                }

                //file.Read(buffer, 0, sizeof(int));
                Buffer.BlockCopy(data, cof, buffer, 0, sizeof(int)); cof += sizeof(int);
                    
                int PosCount = BitConverter.ToInt32(buffer, 0);
                for (int curPos = 0; curPos < PosCount; ++curPos)
                {
                    UInt16 time, x, y, z = 0;
                    Buffer.BlockCopy(data, cof, buffer, 0, sizeof(UInt16) * 4); cof += sizeof(UInt16) * 4;
                
                    //file.Read(buffer, 0, sizeof(UInt16) * 4);
                    time = BitConverter.ToUInt16(buffer, 0);
                    x = BitConverter.ToUInt16(buffer, sizeof(UInt16));
                    y = BitConverter.ToUInt16(buffer, sizeof(UInt16) * 2);
                    z = BitConverter.ToUInt16(buffer, sizeof(UInt16) * 3);
                    newJoint.AddPos(time, x, y, z);
                }
                Joints.Add(newJoint);
            }
            //file.Read(buffer, 0, sizeof(int));
            Buffer.BlockCopy(data, cof, buffer, 0, sizeof(int)); cof += sizeof(int);
                
            int ConstraintCount = BitConverter.ToInt32(buffer, 0);
            if (form.getChecked() <= 4)
            {
                if (ConstraintCount > 0)
                {
                    if (gdebug) writethis("File Refused because it had constraints", ConsoleColor.Gray, ConsoleColor.DarkRed);

                    return false;
                }
            }
            if (ConstraintCount > 50) return false;
            for (int curConst = 0; curConst < ConstraintCount; ++curConst)
            {
                Buffer.BlockCopy(data, cof, buffer, 0, 18 + 12 + 16 + 12 + 12 + 4 + 4 + 4 + 4); cof += 18 + 12 + 16 + 12 + 12 + 4 + 4 + 4 + 4;
                //file.Read(buffer, 0, 18 + 12 + 16 + 12 + 12 + 4 + 4 + 4 + 4);
                byte chainlen = buffer[0];
                if (form.getChecked() <= 5)
                {
                    if (chainlen > 2)
                    {
                        if (gdebug) writethis("File Refused because chain greater then 2", ConsoleColor.Gray, ConsoleColor.DarkRed);

                        return false;
                    }
                    if (chainlen > Joints.Count)
                    {
                        if (gdebug) writethis("File Refused because greater than joints count", ConsoleColor.Gray, ConsoleColor.DarkRed);

                        return false;
                    }
                    if (chainlen > JointCount)
                    {
                        if (gdebug) writethis("File Refused because chain biger then jc", ConsoleColor.Gray, ConsoleColor.DarkRed);

                        return false;
                    }
                }
                LLConstraint.EConstraintType ctype = (LLConstraint.EConstraintType)buffer[1];
                byte[] sourceb = new byte[16];
                Buffer.BlockCopy(buffer, 2, sourceb, 0, 16);
                string source = sourceb.ToString();
                Vector3 sourceoffset = new Vector3();
                sourceoffset.X = BitConverter.ToSingle(buffer, 18);
                sourceoffset.Y = BitConverter.ToSingle(buffer, 22);
                sourceoffset.Z = BitConverter.ToSingle(buffer, 26);
                byte[] targetb = new byte[16];
                Buffer.BlockCopy(buffer, 30, targetb, 0, 16);
                string target = targetb.ToString();
                Vector3 targetoffset = new Vector3();
                targetoffset.X = BitConverter.ToSingle(buffer, 46);
                targetoffset.Y = BitConverter.ToSingle(buffer, 50);
                targetoffset.Z = BitConverter.ToSingle(buffer, 54);
                Vector3 targetdir = new Vector3();
                targetdir.X = BitConverter.ToSingle(buffer, 58);
                targetdir.Y = BitConverter.ToSingle(buffer, 62);
                targetdir.Z = BitConverter.ToSingle(buffer, 66);
                float ease_in_start = BitConverter.ToSingle(buffer, 70);
                float ease_in_stop = BitConverter.ToSingle(buffer, 74);
                float ease_out_start = BitConverter.ToSingle(buffer, 78);
                float ease_out_stop = BitConverter.ToSingle(buffer, 82);
                if (ConstraintCount > JointCount)
                {
                    if (gdebug) writethis("File Refused because cons more then joints", ConsoleColor.Gray, ConsoleColor.DarkRed);

                    return false;
                }
                Constraints.Add(new LLConstraint(chainlen, ctype, source, target, sourceoffset, targetoffset, targetdir, ease_in_start, ease_in_stop, ease_out_start, ease_out_stop));
            }
            //file.Close();


            return true;

        }
        private Packet TransferRequestHandler(Packet packet, IPEndPoint simulator)
        {
            TransferRequestPacket trp = (TransferRequestPacket)packet;
            byte[] b = new byte[4];
            byte[] u = new byte[16];
            Buffer.BlockCopy(trp.TransferInfo.Params, 0, u, 0, 16);
            Buffer.BlockCopy(trp.TransferInfo.Params, 16, b, 0, 4);
            UUID uid = new UUID(
                u, 0);
            bool goon = true;


            if (Utils.BytesToInt(b) == 20)
            {
                //its an animation

                if (form.getChecked() <= 0)
                {
                    if (gdebug) writethis("File Refused because no anims are allowed", ConsoleColor.Gray, ConsoleColor.DarkRed);

                    goon = false;
                }
                if (form.getChecked() <= 1)
                {
                    if (!new List<string>(llanims).Contains(uid.ToString()))
                    {
                        if (gdebug) writethis("File Refused because it wasnt a linden one", ConsoleColor.Gray, ConsoleColor.DarkRed);

                        goon = false;
                    }
                }
                if (uid.ToString().ToLower().Equals("a7b1669c-fa67-0242-4136-79b7a8a3daa0"))
                {
                    if (gdebug) writethis("File Refused because its on the ban list", ConsoleColor.Gray, ConsoleColor.DarkRed);

                    goon = false;
                }

                gd1 temp = new gd1();
                temp.bytes = new byte[1024 * 300];

                if (goon)
                {
                    if (new List<string>(llanims).Contains(uid.ToString()))
                    {
                        //linden anim, safe
                        if (gdebug) writethis("File allowed because its a lidnen one ", ConsoleColor.Gray, ConsoleColor.DarkGreen);

                    }
                    else
                    {
                        if (!dls.ContainsKey(trp.TransferInfo.TransferID))
                        {
                            dls.Add(trp.TransferInfo.TransferID, temp);
                        }
                        else
                        {
                            dls[trp.TransferInfo.TransferID] = temp;
                        }
                        if (gdebug) writethis("We got an animation trafer, it was " + uid.ToString(), ConsoleColor.Black, ConsoleColor.Green);

                    }
                }

                if (gdebug) writethis("FYI., checked is " + form.getChecked().ToString(), ConsoleColor.Gray, ConsoleColor.DarkBlue);

            }
            if (goon == false)
            {
                sendNotAnim(trp.TransferInfo.TransferID);
                //send bad packet
                return null;
            }

            return packet;
        }
        public void sendNotAnim(UUID tran)
        {
            TransferInfoPacket tip = new TransferInfoPacket();
            tip.TransferInfo = new TransferInfoPacket.TransferInfoBlock();
            tip.TransferInfo.Params = UUID.Zero.GetBytes();
            tip.TransferInfo.Size = 0;
            tip.TransferInfo.Status = (int)StatusCode.Skip;
            tip.TransferInfo.TargetType = 2;
            tip.TransferInfo.TransferID = tran;
            tip.TransferInfo.ChannelType = 2;
            tip.Header.Reliable = true;
            proxy.InjectPacket(tip, Direction.Incoming);
        }
        private Packet TransferInfoHandler(Packet packet, IPEndPoint simulator)
        {
            TransferInfoPacket info = (TransferInfoPacket)packet;

            if (dls.ContainsKey(info.TransferInfo.TransferID))
            {
                if (gdebug) Console.WriteLine("Got an info packet");

                //this is our requested tranfer, handle it
                dls[info.TransferInfo.TransferID].size = info.TransferInfo.Size;

                if (((StatusCode)info.TransferInfo.Status != StatusCode.OK) || (dls[info.TransferInfo.TransferID].size < 1))
                {

                    //SendUserAlert("Failed to read item "+assetdownloadID.ToString()+" with inv type number "+type.ToString()+" because of status code "+info.TransferInfo.Status.ToString());
                }
                else
                    if (dls[info.TransferInfo.TransferID].sizedone >= dls[info.TransferInfo.TransferID].size)
                    {
                        //Download already completed!
                        return done(packet, info.TransferInfo.TransferID);
                    }
                //intercept packet
                return packet;
            }
            return packet;
        }
        private Packet TransferPacketHandler(Packet packet, IPEndPoint simulator)
        {
            TransferPacketPacket asset = (TransferPacketPacket)packet;
            UUID cid = asset.TransferData.TransferID;
            if (dls.ContainsKey(cid))
            {
                if (gdebug) Console.WriteLine("Got an transpher packet");
                try
                {
                    Buffer.BlockCopy(asset.TransferData.Data, 0, dls[cid].bytes, dls[cid].sizedone,
                        asset.TransferData.Data.Length);
                }
                catch (Exception )
                {
                    return packet;
                }
                dls[cid].sizedone += asset.TransferData.Data.Length;

                // Check if we downloaded the full asset
                if (dls[cid].sizedone >= dls[cid].size)
                {
                    //done
                    return done(packet, cid);

                }
                //Intercept packet
            }
            return packet;
        }
        public Packet done(Packet input, UUID tranferid)
        {
            //writethis("We finished a download", ConsoleColor.Black, ConsoleColor.Green);
            string filename = "TempAnim" + tranferid.ToString() + ".anim";
            byte[] tmp = new byte[dls[tranferid].size];
            Buffer.BlockCopy(dls[tranferid].bytes, 0, tmp, 0, dls[tranferid].size);
            dls[tranferid].bytes = tmp;
            //File.WriteAllBytes(filename, dls[tranferid].bytes);
            bool pass = false;
            try
            {
                pass = Parse(dls[tranferid].bytes);
            }
            catch (Exception )
            {
                return null;
            }
            //File.Delete(filename);
            dls.Remove(tranferid);
            if (pass)
            {

                //writethis("this anim passed the test", ConsoleColor.Green, ConsoleColor.Gray);
                return input;
            }
            writethis("Bad Animation Found", ConsoleColor.Red, ConsoleColor.Gray);
            //SayToUser("An animation download has been blocked");
            //sendNotAnim(tranferid);
            return null;
        }
        
        private void SayToUser(string message)
        {

            ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
            packet.ChatData.FromName = Utils.StringToBytes(this.brand);
            packet.ChatData.SourceID = UUID.Random();
            packet.ChatData.OwnerID = frame.AgentID;
            packet.ChatData.SourceType = (byte)2;
            packet.ChatData.ChatType = (byte)1;
            packet.ChatData.Audible = (byte)1;
            packet.ChatData.Position = new Vector3(0, 0, 0);
            packet.ChatData.Message = Utils.StringToBytes(message);
            proxy.InjectPacket(packet, Direction.Incoming);
        }
        public void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
        private void SendUserDialog(string first, string last, string objectName, string message, string[] buttons)
        {
            Random rand = new Random();
            ScriptDialogPacket packet = new ScriptDialogPacket();
            packet.Data.ObjectID = UUID.Random();
            packet.Data.FirstName = Utils.StringToBytes(first);
            packet.Data.LastName = Utils.StringToBytes(last);
            packet.Data.ObjectName = Utils.StringToBytes(objectName);
            packet.Data.Message = Utils.StringToBytes(message);
            packet.Data.ChatChannel = (byte)rand.Next(1000, 10000);
            packet.Data.ImageID = UUID.Zero;

            ScriptDialogPacket.ButtonsBlock[] temp = new ScriptDialogPacket.ButtonsBlock[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                temp[i] = new ScriptDialogPacket.ButtonsBlock();
                temp[i].ButtonLabel = Utils.StringToBytes(buttons[i]);
            }
            packet.Buttons = temp;
            proxy.InjectPacket(packet, Direction.Incoming);
        }
    }
}