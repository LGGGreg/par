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
//Intelectual Rights Copyright to LordGregGreg Back
namespace PubComb
{
    public class RadarChatPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private RadarChatForm1 form;
        public DateTime start;
        private PubComb.Aux_SharedInfo shared;
        public List<UUID> avs = new List<UUID>();
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Radar Chat");
            form.readData();
        }
        public RadarChatPlugin(PubComb plug)
        {
            start = System.DateTime.Now;
            plugin = plug;
            shared = plug.SharedInfo;
            form = new RadarChatForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.CoarseLocationUpdate, Direction.Incoming, new PacketDelegate(LocationIN));
            this.proxy.AddDelegate(PacketType.SoundTrigger, Direction.Incoming, new PacketDelegate(inSound));
        }
        private Packet inSound(Packet p, IPEndPoint sim)
        {
            if (form.checkBox1.Checked)
            {
                SoundTriggerPacket s = (SoundTriggerPacket)p;
                if (s.SoundData.OwnerID == frame.AgentID && s.SoundData.SoundID == new UUID("76c78607-93f9-f55a-5238-e19b1a181389"))
                {
                    sendUpdate();
                }
                
            }
            return p;
        }
        private Packet LocationIN(Packet p, IPEndPoint sim)
        {
            if (form.checkBox1.Checked)
            {
                //if (sim.Address != shared.ip || sim.Port != shared.port) return p;
                if (sim.ToString() != proxy.activeCircuit.ToString()) return null;
                CoarseLocationUpdatePacket c = (CoarseLocationUpdatePacket)p;
                
                List<UUID> temp = new List<UUID>();
                bool say = false;
                if (c.AgentData.Length < 1) return p;
                lock (avs)
                {
                    foreach (CoarseLocationUpdatePacket.AgentDataBlock a in c.AgentData)
                    {
                        if (!avs.Contains(a.AgentID))
                        {
                            say = true;
                            form.listBox1.Items.Add(a.AgentID.ToString());
                        }

                        temp.Add(a.AgentID);
                    }
                    foreach (UUID u in avs)
                    {
                        if (!temp.Contains(u))
                        {
                            say = true;
                            form.listBox1.Items.Remove(u.ToString());
                        }
                    }
                }
                avs=temp;

                if(say)
                {
                    sendUpdate();
                }
                    
            }
            return p;
        }
        public void sendUpdate()
        {
            TimeSpan ts = (System.DateTime.Now - start);
            string prefix = (ts.TotalMilliseconds + 39840.0f).ToString() + form.textBox1.Text + avs.Count.ToString() + form.textBox1.Text;
            
            string whattosay = prefix;
            foreach (UUID uid in avs)
            {
                string id = uid.ToString();
                whattosay += id + form.textBox1.Text;
                if (whattosay.Length > 200)
                {
                    DiagReply(whattosay.Remove(whattosay.Length-1), form.numericUpDown1.Value);
                    whattosay = prefix;
                }
            }
            if(whattosay!=prefix)
                DiagReply(whattosay.Remove(whattosay.Length-1), form.numericUpDown1.Value);
        }
        public void DiagReply(string s, decimal chan)
        {
            ScriptDialogReplyPacket p = new ScriptDialogReplyPacket();
            p.AgentData = new ScriptDialogReplyPacket.AgentDataBlock();
            p.AgentData.AgentID = frame.AgentID;
            p.AgentData.SessionID = frame.SessionID;
            p.Data = new ScriptDialogReplyPacket.DataBlock();
            p.Data.ButtonIndex = 1;
            p.Data.ButtonLabel = Utils.StringToBytes(s);
            p.Data.ChatChannel = (int)chan;
            p.Data.ObjectID = frame.AgentID;
            proxy.InjectPacket(p, Direction.Outgoing);
        }
    }
     

}