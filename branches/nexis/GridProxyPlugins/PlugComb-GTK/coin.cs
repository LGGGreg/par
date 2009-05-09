/*
 * Copyright (c) 2009,  Day Oh
 *All rights reserved.
 *
 *Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Day Oh nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

 *THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GridProxy;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.Net;

namespace PubComb
{
    public class coin : GTabPlug
    {
        //private Thread formthread;
        private CoinForm1 form;
        public ProxyFrame frame;
        public PubComb plugin;
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Coin");
            form.CorrectImage();
        }
        public CoinState state = CoinState.Heads;

        public AgentSetAppearancePacket appearance = null;
        public uint serialnum = 0;

        public enum CoinState
        {
            Heads = 0,
            Tails = 1,
            Lols = 2,
            Count = 3
        }

        public UUID[] lols = new UUID[]
        {
            new UUID("8183e823-c443-2142-6eb6-2ab763d4f81c"), // Day Oh Proxy
            new UUID("5aa5c70d-d787-571b-0495-4fc1bdef1500"), // LGG
            new UUID("c252d89d-6f7c-7d90-f430-d140d2e3fbbe"), // Vlife
            new UUID("f3fd74a6-fee7-4b2f-93ae-ddcb5991da04"), // PSL
            new UUID("0bcd5f5d-a4ce-9ea4-f9e8-15132653b3d8"), // MoyMix
            new UUID("77662f23-c77a-9b4d-5558-26b757b2144c"), // PSL
            new UUID("abbca853-30ba-49c1-a1e7-2a5b9a70573f"), // CryoLife
            new UUID("adcbe893-7643-fd12-f61c-0b39717e2e32"), // tyk3n

            new UUID("d5a124d6-9166-a6dd-39aa-434463162f46") // black black
        };
        int lolcounter = 0;
        public AgentSetAppearancePacket cloneASA(AgentSetAppearancePacket p)
        {
            AgentSetAppearancePacket o = new AgentSetAppearancePacket();
            o.AgentData = new AgentSetAppearancePacket.AgentDataBlock();
            o.AgentData.AgentID = p.AgentData.AgentID;
            o.AgentData.SessionID = p.AgentData.SessionID;
            o.AgentData.SerialNum = p.AgentData.SerialNum;
            o.AgentData.Size = p.AgentData.Size;
            o.WearableData = new AgentSetAppearancePacket.WearableDataBlock[p.WearableData.Length];
            for (int i = 0; i < o.WearableData.Length; i++)
                o.WearableData[i] = p.WearableData[i];
            o.ObjectData = new AgentSetAppearancePacket.ObjectDataBlock();
            o.ObjectData.TextureEntry = p.ObjectData.TextureEntry;
            o.VisualParam = new AgentSetAppearancePacket.VisualParamBlock[p.VisualParam.Length];
            for (int i = 0; i < o.VisualParam.Length; i++)
                o.VisualParam[i] = p.VisualParam[i];
            return o;
        }

        public AgentSetAppearancePacket replacer_tails(AgentSetAppearancePacket packet)
        {
            AgentSetAppearancePacket p = cloneASA(packet);
            if (p.ObjectData != null)
            {
                if (p.ObjectData.TextureEntry != null)
                {
                    Primitive.TextureEntry te = new Primitive.TextureEntry(p.ObjectData.TextureEntry, 0, p.ObjectData.TextureEntry.Length);
                    if (te != null)
                    {
                        if (te.FaceTextures != null)
                        {
                            if (te.FaceTextures.Length > 0)
                            {
                                //Console.WriteLine("Coin is replacing textures...");
                                UUID replace = new UUID("8183e823-c443-2142-6eb6-2ab763d4f81c");
                                for (int i = 0; i <= 7; i++)
                                {
                                    if (te.FaceTextures[i] != null)
                                        te.FaceTextures[i].TextureID = replace;
                                }
                                for (int i = 12; i <= 18; i++)
                                {
                                    if (te.FaceTextures[i] != null)
                                        te.FaceTextures[i].TextureID = replace;
                                }

                                if (p.ObjectData != null)
                                    p.ObjectData.TextureEntry = te.ToBytes();
                                //Console.WriteLine("OK!");
                            }
                        }
                    }
                }
            }
            return p;
        }

        public AgentSetAppearancePacket replacer_lols(AgentSetAppearancePacket packet)
        {
            AgentSetAppearancePacket p = cloneASA(packet);
            if (p.ObjectData != null)
            {
                if (p.ObjectData.TextureEntry != null)
                {
                    Primitive.TextureEntry te = new Primitive.TextureEntry(p.ObjectData.TextureEntry, 0, p.ObjectData.TextureEntry.Length);
                    if (te != null)
                    {
                        if (te.FaceTextures != null)
                        {
                            if (te.FaceTextures.Length > 0)
                            {
                                //Console.WriteLine("Coin is replacing textures...");
                                UUID replace = lols[lolcounter];
                                lolcounter++;
                                if (lolcounter >= lols.Length)
                                    lolcounter = 0;
                                for (int i = 0; i <= 7; i++)
                                {
                                    if (te.FaceTextures[i] != null)
                                        te.FaceTextures[i].TextureID = replace;
                                }
                                for (int i = 12; i <= 18; i++)
                                {
                                    if (te.FaceTextures[i] != null)
                                        te.FaceTextures[i].TextureID = replace;
                                }

                                if (p.ObjectData != null)
                                    p.ObjectData.TextureEntry = te.ToBytes();
                                //Console.WriteLine("OK!");
                            }
                        }
                    }
                }
            }
            return p;
        }

        public coin(PubComb plug)
        {
            plugin = plug;
            this.frame = plug.frame;

            frame.proxy.AddDelegate(PacketType.AgentSetAppearance, Direction.Outgoing, coinage);

            //formthread = new Thread(new ThreadStart(delegate()
            //{
                form = new CoinForm1(this);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
        }

        public Packet coinage(Packet packet, IPEndPoint sim)
        {
            AgentSetAppearancePacket p = cloneASA((AgentSetAppearancePacket)packet);
            appearance = cloneASA(p);

            if (state != CoinState.Heads)
            {
                if (state == CoinState.Tails)
                    p = replacer_tails(p);
                else if (state == CoinState.Lols)
                    p = replacer_lols(p);
            }
            p.AgentData.SerialNum = 1;
            //frame.proxy.InjectPacket(p, Direction.Outgoing);
            return p;
        }

       
    }
}
