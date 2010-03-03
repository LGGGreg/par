/*
 * Copyright (c) 2009, Day Oh & Gregory Hendrickson (LordGregGreg Back) 
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

namespace PubCombN
{
    public class PennyPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private PennyForm1 form;

        public PennyPlugin(PubComb plug)
        {
            plugin = plug;
            //formthread = new Thread(new ThreadStart(delegate()
            //{
                form = new PennyForm1(this);
             //   Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
                
                
            this.frame = plugin.frame;
            this.proxy = plugin.proxy;
            //this.brand = "Penny";
            proxy.AddDelegate(PacketType.AgentSetAppearance, Direction.Outgoing,new PacketDelegate(ApHand));

        }
        public void LoadNow()
        {
            tabInfo t = getInfo();
            plugin.tabform.addATab(t.f, t.s);
        }
        public tabInfo getInfo()
        {
            form.readData();
            return new tabInfo(form, "Penny");
        }
        public Packet ApHand(Packet packet, IPEndPoint sim)
        {
                if (form.getChecked()==false)
                {
                    return packet;
                }
                AgentSetAppearancePacket packet2 = (AgentSetAppearancePacket) packet;
                if ((packet2.ObjectData == null) || (packet2.ObjectData.TextureEntry == null))
                {
                    return packet;
                }
                Primitive.TextureEntry entry = new Primitive.TextureEntry(packet2.ObjectData.TextureEntry, 0, packet2.ObjectData.TextureEntry.Length);
                if (((entry == null) || (entry.FaceTextures == null)) || (entry.FaceTextures.Length <= 0))
                {
                    return packet;
                }
                Console.WriteLine("Penny is replacing textures...");
                UUID uuid =  new UUID("5aa5c70d-d787-571b-0495-4fc1bdef1500");
                for (int i = 0; i <= 7; i++)
                {
                    if (entry.FaceTextures[i] != null)
                    {
                        entry.FaceTextures[i].TextureID = uuid;
                    }
                }
                for (int j = 12; j <= 0x12; j++)
                {
                    if (entry.FaceTextures[j] != null)
                    {
                        entry.FaceTextures[j].TextureID = uuid;
                    }
                }
                if (packet2.ObjectData != null)
                {
                    packet2.ObjectData.TextureEntry = entry.GetBytes();
                }
                Console.WriteLine("OK! Thanks Day!");
                return packet2;
            
        }

       
        

    }
}