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
            if (Math.Abs(temp) < (delta*((float)1/(float)65535)))
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
}
