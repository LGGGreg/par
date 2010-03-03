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
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.Imaging;
using GridProxy;

namespace PubCombN
{
    public class ProTextPlug : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        public ProTextionForm1 form;
        public static bool Enabled = true;
        String CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            return Regex.Replace(strIn, @"[^\w\.@-]", "");
        }
        public void LoadNow()
        {
            tabInfo t = getInfo();
            plugin.tabform.addATab(t.f, t.s);
        }
        public tabInfo getInfo()
        {
            return new tabInfo(form, "ProTextion");
        }
        public ProTextPlug(PubComb p)
        {
            plugin = p;
            this.frame = p.frame;
            this.proxy = frame.proxy;
            form = new ProTextionForm1(this);
            this.proxy.AddDelegate(PacketType.ImageData, Direction.Incoming, new PacketDelegate(ImageDataHandler));
        }
        
        public static byte[] ReadBytes(byte [] reader, int fieldSize)
        {
            byte[] bytes = new byte[fieldSize];
            int j = 0;
                for (int i = fieldSize - 1; i > -1; i--)
                    bytes[i] = reader[j++];
                return bytes;
           
        }
        private bool IsPowerOfTwo(uint n)
        {
            return (n & (n - 1)) == 0 && n != 0;
        }
        private Packet ImageDataHandler(Packet packet, IPEndPoint simulator)
        {
            if (form.getEnabled())
            {
                ImageDataPacket data = (ImageDataPacket)packet;

                //(ImageCodec)data.ImageID.Codec;
                byte[] data2 = new byte[(int)data.ImageID.Size];
                Buffer.BlockCopy(data.ImageData.Data, 0, data2, 0, data.ImageData.Data.Length);
                //we now have it in data...
                /*
                 * data += 8;
                    S32 xsiz = ntohl(((U32*)data)[0]);
                    S32 ysiz = ntohl(((U32*)data)[1]);
                    S32 xosiz = ntohl(((U32*)data)[2]);
                    S32 yosiz = ntohl(((U32*)data)[3]);
                    S32 xtsiz = ntohl(((U32*)data)[4]);
                    S32 ytsiz = ntohl(((U32*)data)[5]);
                    //S32 xtosiz = ntohl(((U32*)data)[6]);
                    //S32 ytosiz = ntohl(((U32*)data)[7]);
                    if(xsiz < 16 || xsiz > 2048 || ysiz < 16 || ysiz > 2048) {
                 */
                byte[] four = new byte[4];
                bool flag = false;
                for (int location = 8; location < 30; location += 4)
                {

                    Buffer.BlockCopy(data2, location, four, 0, 4);
                    int size = BitConverter.ToInt32(ReadBytes(four, 4), 0);
                    if (flag)
                        Console.WriteLine("Loc = " + location.ToString() + "| IMAGE SIZE = " + size.ToString());
                    if (location == 16 || location == 20)
                    {
                        //seem to always be zero
                        if (size != 0)
                        {
                            //SendUserAlert("Well, i really don't think you are ever suposed to get this error... so.. remeber this key and send it to me");
                            flag = true;
                        }
                    }
                    else
                    {
                        if (IsPowerOfTwo((uint)size) && size <= 2048 && size >= 1)
                        {
                            ///cool
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                }
                if (flag)
                {
                    //SendUserAlert("We Got a evil Image\nIts key is " + data.ImageID.ID.ToString());
                    frame.SendUserAlert("Possible Image Crash Prevented");
                    ImageNotInDatabasePacket no = new ImageNotInDatabasePacket();
                    no.ImageID.ID = data.ImageID.ID;
                    proxy.InjectPacket(no, Direction.Incoming);
                    return null;
                }

            }
            return packet;
        }
       
        
        
    }

}