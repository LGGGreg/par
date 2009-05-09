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

namespace PubComb
{
    public class SpamBlocker : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private SpamBlockFormGTK form;
        private PubComb.Aux_SharedInfo shared;
        class rights
        {
            public rights(ChangeUserRightsPacket pp)
            {
                p = pp;
                time =System.DateTime.Now ;
            }
            public ChangeUserRightsPacket p;
            public DateTime time;
        }
        class ims
        {
            public ims(ImprovedInstantMessagePacket im)
            {
                i = im; time = System.DateTime.Now ;
            }
            public ImprovedInstantMessagePacket i;
            public DateTime time;
        }
        class diags
        {
            public diags(ScriptDialogPacket ss)
            {
                s = ss; time = System.DateTime.Now;
            }
            public override string ToString()
            {
                return time.ToLongTimeString();
            }
            public ScriptDialogPacket s;
            public DateTime time;
        }
        class sounds
        {
            public sounds(UUID sound, UUID r)
            {
                soundUUID = sound;
                owner = r;
                time = System.DateTime.Now;
            }
            public UUID soundUUID;
            public UUID owner;
            public DateTime time;
        }
        class maps
        {
            public maps(ScriptTeleportRequestPacket p)
            {
                m = p;
                time = System.DateTime.Now;
            }
            public ScriptTeleportRequestPacket m;
            public DateTime time;
        }

        private List<rights> lastrights = new List<rights>();
        private List<ims> lastIM = new List<ims>();
        private List<diags> lastDialogs = new List<diags>();
        private List<sounds> lastSounds = new List<sounds>();
        private List<maps> lastMaps = new List<maps>();

        private List<uint> recSeq = new List<uint>();
		
		public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
	            tabform.addATab(form, "SpamBlock");
	            //form.readData();
			} else {
				Console.WriteLine("[LYRA] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
		
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "SpamBlock");
        }
        public SpamBlocker(PubComb plug)
        {

            plugin = plug;
            shared = plug.SharedInfo;
            form = new SpamBlockFormGTK(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.ChangeUserRights, Direction.Incoming, new PacketDelegate(this.UserRights));
            this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Incoming, new PacketDelegate(this.InIM));
            this.proxy.AddDelegate(PacketType.ScriptDialog, Direction.Incoming, new PacketDelegate(this.Dialogs));
            this.proxy.AddDelegate(PacketType.SoundTrigger, Direction.Incoming, new PacketDelegate(this.InTSound));
            this.proxy.AddDelegate(PacketType.AttachedSound, Direction.Incoming, new PacketDelegate(this.InASound));
            this.proxy.AddDelegate(PacketType.ScriptTeleportRequest, Direction.Incoming, new PacketDelegate(this.InMap));
        }
        private Packet InTSound(Packet packet, IPEndPoint sim)
        {
            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.RemoveAt(0);
                    }

                    if (form.CheckSound)
                    {

                        lock (lastSounds)
                        {
                            lastSounds.Add(new sounds(((SoundTriggerPacket)packet).SoundData.SoundID,((SoundTriggerPacket)packet).SoundData.OwnerID));
                            if (lastSounds.Count > 3)
                            {
                                lastSounds.RemoveAt(0);
                            }
                        }
                        List<UUID> sids = new List<UUID>();
                        List<UUID> whos = new List<UUID>();
                        lock (lastSounds)
                        {
                            foreach (sounds pa in lastSounds)
                            {
                                if (!sids.Contains(pa.soundUUID))
                                    sids.Add(pa.soundUUID);

                                UUID who = pa.owner;
                                if (!whos.Contains(who))
                                    whos.Add(who);
                            }

                        }
                        if (whos.Count == 1 && sids.Count ==1 && lastSounds.Count == 3)
                        {
                            TimeSpan duration = lastSounds[2].time - lastSounds[0].time;
                            if (duration.TotalMilliseconds < 1000)
                            {
                                form.Debug += "DS";
                                //proxy.writeinthis("DS", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        private Packet InASound(Packet packet, IPEndPoint sim)
        {
            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.RemoveAt(0);
                    }

                    if (form.CheckSound)
                    {

                        lock (lastSounds)
                        {
                            lastSounds.Add(new sounds(((AttachedSoundPacket)packet).DataBlock.SoundID, ((AttachedSoundPacket)packet).DataBlock.OwnerID));
                            if (lastSounds.Count > 3)
                            {
                                lastSounds.RemoveAt(0);
                            }
                        }
                        List<UUID> sids = new List<UUID>();
                        List<UUID> whos = new List<UUID>();
                        lock (lastSounds)
                        {
                            foreach (sounds pa in lastSounds)
                            {
                                if (!sids.Contains(pa.soundUUID))
                                    sids.Add(pa.soundUUID);

                                UUID who = pa.owner;
                                if (!whos.Contains(who))
                                    whos.Add(who);
                            }

                        }
                        if (whos.Count == 1 && sids.Count ==1 && lastSounds.Count == 3)
                        {
                            TimeSpan duration = lastSounds[2].time - lastSounds[0].time;
                            if (duration.TotalMilliseconds < 1000)
                            {
                                form.Debug += "DAS";
                                //proxy.writeinthis("DAS", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        
        private Packet InIM(Packet packet, IPEndPoint sim)
        {
            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.RemoveAt(0);
                    }

                    if (form.CheckIM)
                    {

                        lock (lastIM)
                        {
                            lastIM.Add(new ims((ImprovedInstantMessagePacket)packet));
                            if (lastIM.Count > 5)
                            {
                                lastIM.RemoveAt(0);
                            }
                        }

                        List<string> msgs = new List<string>();
                        List<UUID> whos = new List<UUID>();
                        List<int> types = new List<int>();
                        lock (lastIM)
                        {
                            foreach (ims pa in lastIM)
                            {
                                ImprovedInstantMessagePacket p = pa.i;
                                string m = Utils.BytesToString(p.MessageBlock.Message);
                                if (!msgs.Contains(m))
                                    msgs.Add(m);

                                UUID who = p.AgentData.AgentID;
                                if (!whos.Contains(who))
                                    whos.Add(who);

                                int type = p.MessageBlock.Dialog;
                                if (!types.Contains(type))
                                    types.Add(type);
                            }

                        }
                        if (whos.Count == 1 && types.Count < 3 && msgs.Count < 4 && lastIM.Count == 5)
                        {
                            TimeSpan duration = lastIM[4].time - lastIM[0].time;


                            if (duration.TotalMilliseconds < 2000)
                            {
                                form.Debug += "DIM";
                                //proxy.writeinthis("DIM", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        private Packet UserRights(Packet packet, IPEndPoint sim)
        {

            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.RemoveAt(0);
                    }
                    if (form.CheckMod)
                    {
                        ChangeUserRightsPacket cur = (ChangeUserRightsPacket)packet;
                        //if (cur.AgentData.AgentID == lastrights.AgentData.AgentID) return null;

                        lock (lastrights)
                        {
                            lastrights.Add(new rights((ChangeUserRightsPacket)packet));
                            if (lastrights.Count > 3)
                            {
                                lastrights.RemoveAt(0);
                            }
                        }

                        List<UUID> whos = new List<UUID>();
                        lock (lastIM)
                        {
                            foreach (rights r in lastrights)
                            {
                                ChangeUserRightsPacket p = r.p;
                                UUID who = p.AgentData.AgentID;
                                if (!whos.Contains(who))
                                    whos.Add(who);
                            }

                        }
                        if (whos.Count == 1 && lastrights.Count == 3)
                        {
                            TimeSpan duration = this.lastrights[2].time - lastrights[0].time;

                            if (duration.TotalMilliseconds < 2000)
                            {
                                form.Debug += "DR";
                                //proxy.writeinthis("DR", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        private Packet Dialogs(Packet packet, IPEndPoint sim)
        {
            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.Clear();
                    }
                    if (form.CheckDiag)
                    {
                        ScriptDialogPacket s = (ScriptDialogPacket)packet;

                        lock (lastDialogs)
                        {
                            
                            lastDialogs.Add(new diags((ScriptDialogPacket)packet));
                            if (lastDialogs.Count > 4)
                            {
                                lastDialogs.RemoveAt(0);
                            }
                        }

                        List<UUID> whos = new List<UUID>();
                        lock (lastDialogs)
                        {
                            //proxy.writethis("new", ConsoleColor.Black, ConsoleColor.Blue);
                            foreach (diags d in lastDialogs)
                            {
                                ScriptDialogPacket p = d.s;
                                UUID who = p.Data.ObjectID;
                                if (!whos.Contains(who))
                                    whos.Add(who);

                                //proxy.writethis(d.ToString(), ConsoleColor.Black, ConsoleColor.Cyan);
                            }

                        }
                        
                        if (lastDialogs.Count == 4)
                        {
                            TimeSpan duration = lastDialogs[3].time - lastDialogs[0].time;
                            //proxy.writethis(durationToString(), ConsoleColor.Black, ConsoleColor.DarkCyan);
                            if (duration.TotalMilliseconds < 400)
                            {
                                form.Debug += "DD";
                                //proxy.writeinthis("DD", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        private Packet InMap(Packet packet, IPEndPoint sim)
        {

            lock (recSeq)
            {
                if (!recSeq.Contains(packet.Header.Sequence))
                {
                    recSeq.Add(packet.Header.Sequence);
                    if (recSeq.Count > 200)
                    {
                        recSeq.RemoveAt(0);
                    }
                    if (form.CheckMap)
                    {
                        ScriptTeleportRequestPacket stp = (ScriptTeleportRequestPacket)packet;
                        lock (lastMaps)
                        {
                            lastMaps.Add(new maps(stp));
                            if (lastMaps.Count > 3)
                            {
                                lastMaps.RemoveAt(0);
                            }
                        }

                        List<String> whos = new List<string>();
                        List<Vector3> whares = new List<Vector3>();
                        lock (lastMaps)
                        {
                            foreach (maps m in lastMaps)
                            {
                                ScriptTeleportRequestPacket mp =m.m;
                                String who = Utils.BytesToString(mp.Data.ObjectName);
                                Vector3 whare = mp.Data.SimPosition;
                                if (!whos.Contains(who))
                                    whos.Add(who);
                                if (!whares.Contains(whare))
                                    whares.Add(whare);
                            }

                        }
                        if (whos.Count == 1 && whares.Count <=3 && lastMaps.Count == 3)
                        {
                            TimeSpan duration = this.lastMaps[2].time - lastMaps[0].time;
                            
                            if (duration.TotalMilliseconds < 3500)
                            {
                                form.Debug += "DM";
                                //proxy.writeinthis("DM", ConsoleColor.Black, ConsoleColor.Red);
                                return null;
                            }
                        }
                    }
                }
                else return null;
            }
            return packet;
        }
        
        
        
    }

}