/*
 * Copyright (c) 2009, Day Oh
 * Modified on Jan 2009 by LordGregGreg Back
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
using System.Net;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;

namespace PubCombN
{
    public class useful : GTabPlug
    {
        private ProxyFrame frame;
        private Proxy proxy;
        private PubComb plugin;

        private ulong RegionHandle = 0;
        private Vector3 CameraCenter = Vector3.Zero;

        private static List<UUID> PeopleWhoIMedMe = new List<UUID>();
        private UsefulForm1 form;
        private PubComb.Aux_SharedInfo shared;

        public List<UUID> pending = new List<UUID>();


        public class SimInfo
        {
            public uint Handle;
            public int x;
            public int y;
        }
        public void LoadNow()
        {
            tabInfo t = getInfo();
            plugin.tabform.addATab(t.f,t.s);
        }
        public tabInfo getInfo()
        {
            return new tabInfo(form, "Useful");
        }
        public useful(PubComb plug)
        {
            plugin = plug;
            shared = plugin.SharedInfo;
            this.frame = plug.frame;
            this.proxy = frame.proxy;
            form = new UsefulForm1(this);

            /*this.proxy.AddDelegate(PacketType.AgentMovementComplete, Direction.Incoming, delegate(Packet packet, IPEndPoint sim)
            {
                RegionHandle = ((AgentMovementCompletePacket)packet).Data.RegionHandle;
                return packet;
            });
            this.proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, delegate(Packet packet, IPEndPoint sim)
            {
                CameraCenter = ((AgentUpdatePacket)packet).AgentData.CameraCenter;
                return packet;
            });
            */

            //this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
            //this.proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, new PacketDelegate(OutAgentUpdateHandler));
            //this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Incoming, new PacketDelegate(InImprovedInstantMessageHandler));
            //this.proxy.AddDelegate(PacketType.ViewerEffect, Direction.Incoming, new PacketDelegate(InViewerEffectHandler));
            this.proxy.AddDelegate(PacketType.AlertMessage, Direction.Incoming, new PacketDelegate(InAlertMessageHandler));
            this.proxy.AddDelegate(PacketType.AvatarPropertiesRequest, Direction.Outgoing, new PacketDelegate(OutAvatarPropertiesRequestHandler));
            this.proxy.AddDelegate(PacketType.GroupProfileReply, Direction.Incoming, new PacketDelegate(GroupProp));
            this.proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, new PacketDelegate(GotName));
            
            //this.proxy.AddDelegate(PacketType.AvatarSitResponse, Direction.Incoming, new PacketDelegate(InAvatarSitResponseHandler));
            //this.proxy.AddDelegate(PacketType.TerminateFriendship, Direction.Incoming, new PacketDelegate(InTerminateFriendshipHandler));
            //this.proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, new PacketDelegate(InObjectUpdateHandler));
        }

        


        private Packet OutAgentUpdateHandler(Packet packet, IPEndPoint sim)
        {

            // slproxy sometimes loses track of which sim you're in...
            proxy.activeCircuit = sim;
            return packet;
        }
        private Packet InImprovedInstantMessageHandler(Packet packet, IPEndPoint sim)
        {
            if (RegionHandle != 0)
            {
                SoundTriggerPacket sound = new SoundTriggerPacket();
                sound.SoundData.SoundID = new UUID(form.textBox1sounduuid.Text);
                sound.SoundData.OwnerID = frame.AgentID;
                sound.SoundData.ObjectID = frame.AgentID;
                sound.SoundData.ParentID = UUID.Zero;
                sound.SoundData.Handle = RegionHandle;
                sound.SoundData.Position = CameraCenter;
                sound.SoundData.Gain = ((float)form.trackBar1volume.Value)/100.0f;
                sound.Header.Reliable = false;
                if (form.checkBox1imsound.Checked)
                    proxy.InjectPacket(sound, Direction.Incoming);
            }

            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)packet;
            //block repeated crap



            if (im.MessageBlock.Dialog == (byte)InstantMessageDialog.StartTyping)
            {
                if (PeopleWhoIMedMe.Contains(im.AgentData.AgentID) == false)
                {
                    PeopleWhoIMedMe.Add(im.AgentData.AgentID);
                    ImprovedInstantMessagePacket im2 = new ImprovedInstantMessagePacket();
                    im2.AgentData = new ImprovedInstantMessagePacket.AgentDataBlock();
                    im2.AgentData.AgentID = im.AgentData.AgentID;
                    im2.AgentData.SessionID = im.AgentData.SessionID;
                    im2.MessageBlock = new ImprovedInstantMessagePacket.MessageBlockBlock();
                    im2.MessageBlock.FromGroup = im.MessageBlock.FromGroup;
                    im2.MessageBlock.ToAgentID = im.MessageBlock.ToAgentID;
                    im2.MessageBlock.ParentEstateID = im.MessageBlock.ParentEstateID;
                    im2.MessageBlock.RegionID = im.MessageBlock.RegionID;
                    im2.MessageBlock.Position = im.MessageBlock.Position;
                    im2.MessageBlock.Offline = im.MessageBlock.Offline;
                    im2.MessageBlock.Dialog = 0;
                    im2.MessageBlock.ID = im.MessageBlock.ID;
                    im2.MessageBlock.Timestamp = im.MessageBlock.Timestamp;
                    im2.MessageBlock.FromAgentName = im.MessageBlock.FromAgentName;
                    im2.MessageBlock.Message = Utils.StringToBytes("/me is typing a message...");
                    im2.MessageBlock.BinaryBucket = im.MessageBlock.BinaryBucket;
                    proxy.InjectPacket(im2, Direction.Incoming);
                }
            }
            else if (im.MessageBlock.Dialog == 22) // teleport lure
            {
                string[] bbfields = Utils.BytesToString(im.MessageBlock.BinaryBucket).Split('|');
                if (bbfields.Length < 5)
                    return packet;
                ushort MapX;
                ushort MapY;
                double RegionX;
                double RegionY;
                double RegionZ;
                try
                {
                    MapX = (ushort)(uint.Parse(bbfields[0]) / 256);
                    MapY = (ushort)(uint.Parse(bbfields[1]) / 256);
                    RegionX = double.Parse(bbfields[2]);
                    RegionY = double.Parse(bbfields[3]);
                    RegionZ = double.Parse(bbfields[4]);
                }
                catch
                {
                    frame.SayToUser("WARNING! " + Utils.BytesToString(im.MessageBlock.FromAgentName) + "'s teleport lure IM seems to have unusual data in its BinaryBucket!");
                    return packet;
                }

                // request region name

                System.Timers.Timer myTimer = new System.Timers.Timer(10000);

                string RegionName = null;
                PacketDelegate replyCallback = delegate(Packet p, IPEndPoint s)
                {
                    MapBlockReplyPacket reply = (MapBlockReplyPacket)p;
                    foreach (MapBlockReplyPacket.DataBlock block in reply.Data)
                    {
                        if ((block.X == MapX) && (block.Y == MapY))
                        {
                            RegionName = Utils.BytesToString(block.Name);
                            StringBuilder sb = new StringBuilder();
                            sb.Append(Utils.BytesToString(im.MessageBlock.FromAgentName) + "'s teleport lure is to ");
                            sb.Append(RegionName + " " + RegionX.ToString() + ", " + RegionY.ToString() + ", " + RegionZ.ToString() + " ");
                            sb.Append("secondlife://" + RegionName.Replace(" ", "%20") + "/" + RegionX.ToString() + "/" + RegionY.ToString() + "/" + RegionZ.ToString());
                            frame.SayToUser(sb.ToString());
                        }
                    }
                    return null;
                };

                System.Timers.ElapsedEventHandler timerCallback = delegate(object sender, System.Timers.ElapsedEventArgs e)
                {
                    if (RegionName == null)
                    {
                        frame.SayToUser("Couldn't resolve the destination of " + Utils.BytesToString(im.MessageBlock.FromAgentName) + "'s teleport lure: " + Utils.BytesToString(im.MessageBlock.BinaryBucket));
                    }
                    proxy.RemoveDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                    myTimer.Stop();
                };

                proxy.AddDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                myTimer.Elapsed += timerCallback;
                myTimer.Start();

                MapBlockRequestPacket MapBlockRequest = new MapBlockRequestPacket();
                MapBlockRequest.Type = PacketType.MapBlockRequest;
                MapBlockRequest.AgentData = new MapBlockRequestPacket.AgentDataBlock();
                MapBlockRequest.AgentData.AgentID = frame.AgentID;
                MapBlockRequest.AgentData.SessionID = frame.SessionID;
                MapBlockRequest.AgentData.Flags = 0;
                MapBlockRequest.AgentData.Godlike = false;
                MapBlockRequest.PositionData = new MapBlockRequestPacket.PositionDataBlock();
                MapBlockRequest.PositionData.MinX = MapX;
                MapBlockRequest.PositionData.MaxX = MapX;
                MapBlockRequest.PositionData.MinY = MapY;
                MapBlockRequest.PositionData.MaxY = MapY;
                proxy.InjectPacket(MapBlockRequest, Direction.Outgoing);
            }
            else if (im.MessageBlock.Dialog == (byte)InstantMessageDialog.InventoryOffered)
            {
                if (im.MessageBlock.BinaryBucket[0] == (byte)AssetType.Simstate)
                {
                    frame.SayToUser(Utils.BytesToString(im.MessageBlock.FromAgentName) + " offered you a SimState :O");
                    return null;
                }
            }
            else if (im.MessageBlock.Dialog == 9)
            {
                if (im.MessageBlock.BinaryBucket[0] == (byte)AssetType.Simstate)
                {
                    frame.SayToUser(im.AgentData.AgentID.ToString() + " offered you a SimState from an object at " + im.MessageBlock.Position.ToString() + " :O Message = " + Utils.BytesToString(im.MessageBlock.Message));
                    return null;
                }
            }
            // Don't get spammed bro
            if (im.MessageBlock.Dialog == 0)
            {
                im.MessageBlock.ID = frame.AgentID.Equals(im.AgentData.AgentID) ? frame.AgentID : im.AgentData.AgentID ^ frame.AgentID;
                packet = (Packet)im;
            }
            return packet;
        }
        private Packet InViewerEffectHandler(Packet packet, IPEndPoint sim)
        {
            ViewerEffectPacket p = (ViewerEffectPacket)packet;
            foreach (ViewerEffectPacket.EffectBlock block in p.Effect)
            {
                if (block.Type == (byte)EffectType.PointAt)
                {
                    if (block.TypeData.Length >= 56)
                    {
                        Vector3d v = new Vector3d(block.TypeData, 32);
                        if ((v.X < -1024f) || (v.X > 1024f) || (v.Y < -1024f) || (v.Y > 1024f) || (v.Z < -1024f) || (v.Z > 1024f))
                        {
                            v = new Vector3d();
                            frame.SayToUser("Possible ViewerEffect crash packet from " + block.AgentID.ToString());
                        }
                        Buffer.BlockCopy(v.GetBytes(), 0, block.TypeData, 32, 24);
                    }
                }
            }
            return (Packet)p;
        }
        private Packet InAlertMessageHandler(Packet packet, IPEndPoint sim)
        {
            if (!form.checkBox1alertmessages.Checked) return packet;
            AlertMessagePacket AlertMessage = (AlertMessagePacket)packet;
            string message = Utils.BytesToString(AlertMessage.AlertData.Message);
            if (message.StartsWith("Can't move object "))
            {
                int begin = message.IndexOf("{");
                if (begin != -1)
                {
                    int end = message.IndexOf("}") - 1;
                    int length = end - begin;
                    string location = message.Substring(begin + 1, length);
                    string[] numbers = location.Split(",".ToCharArray());
                    begin = message.IndexOf("} in region ") + 12;
                    end = message.IndexOf("because your objects are not allowed on this parcel.", begin) - 1;
                    length = end - begin;
                    string region = message.Substring(begin, length);
                    string slurl = "secondlife://" + region.Replace(" ", "%20") + "/" + numbers[0].Trim() + "/" + numbers[1].Trim() + "/" + numbers[2].Trim();
                    message += " " + slurl + " ";
                    if (message.Length > 255)
                    {
                        message = message.Substring(0, 255);
                    }
                    AlertMessage.AlertData.Message = Utils.StringToBytes(message);
                    packet = (Packet)AlertMessage;
                }
            }
            return packet;
        }
        private Packet GroupProp(Packet packet, IPEndPoint sim)
        {
            if (!form.checkBox1groupkey.Checked) return packet;
            
            GroupProfileReplyPacket p = (GroupProfileReplyPacket)packet;
            frame.SayToUser(Utils.BytesToString(p.GroupData.Name) + " = " + p.GroupData.GroupID.ToString());
            return packet;
        }
        private Packet OutAvatarPropertiesRequestHandler(Packet requestpacket, IPEndPoint sim)
        {
            if (!form.checkBox1profilekey.Checked) return requestpacket;
            
            if (requestpacket.Header.Resent)
                return requestpacket;
            AvatarPropertiesRequestPacket packet = (AvatarPropertiesRequestPacket)requestpacket;
            UUID uuid = packet.AgentData.AvatarID;
            if (shared.key2name.ContainsKey(uuid))
            {
                frame.SayToUser(shared.key2name[uuid] + " = " + uuid.ToString());
                 
            }
            else
            {
                plugin.RequestNameFromKey(uuid);
                pending.Add(uuid);
            }
            return packet;

        }
        private Packet GotName(Packet p, IPEndPoint sim)
        {
            UUIDNameReplyPacket n = (UUIDNameReplyPacket)p;
            lock (shared.key2name)
            {
                lock (shared.name2key)
                {
                    foreach (UUIDNameReplyPacket.UUIDNameBlockBlock b in n.UUIDNameBlock)
                    {
                        if (!shared.key2name.ContainsKey(b.ID))
                        {
                            string name = Utils.BytesToString(b.FirstName) + " " + Utils.BytesToString(b.LastName);
                            shared.key2name.Add(b.ID, name);
                            if (!shared.name2key.ContainsKey(name))
                            {
                                shared.name2key.Add(name, b.ID);
                            }                            
                        }
                        lock (pending)
                        {
                            if (pending.Contains(b.ID))
                            {
                                pending.Remove(b.ID);
                                frame.SayToUser(shared.key2name[b.ID] + " = " + b.ID.ToString());

                            }
                        }
                    }

                }
            }
            return p;
        }
        // try to stop autopilot when sit response is received
        //private Packet InAvatarSitResponseHandler(Packet packet, IPEndPoint sim)
        //{
        //    if (LastAgentUpdate != null)
        //    {
        //        AgentUpdatePacket p = LastAgentUpdate;
        //        p.AgentData.ControlFlags = 524288; // (uint)AgentManager.ControlFlags.AGENT_CONTROL_NUDGE_AT_POS;
        //        proxy.InjectPacket(p, Direction.Outgoing);
        //        p.AgentData.ControlFlags = 0;
        //        proxy.InjectPacket(p, Direction.Outgoing);
        //    }
        //    return packet;
        //}

        private Packet InTerminateFriendshipHandler(Packet packet, IPEndPoint sim)
        {
            if (packet.Header.Resent)
                return packet;
            UUID uuid = ((TerminateFriendshipPacket)packet).ExBlock.OtherID;
            string name = "(waiting)";
            ManualResetEvent evt = new ManualResetEvent(false);
            PacketDelegate nameHandler = delegate(Packet packet2, IPEndPoint sim2)
            {
                foreach (UUIDNameReplyPacket.UUIDNameBlockBlock block in ((UUIDNameReplyPacket)packet2).UUIDNameBlock)
                {
                    if (block.ID == uuid)
                    {
                        name = Utils.BytesToString(block.FirstName) + " " + Utils.BytesToString(block.LastName);
                        evt.Set();
                    }
                }
                return packet2;
            };
            Thread myThread = new Thread(new ThreadStart(delegate
            {
                proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, nameHandler);
                UUIDNameRequestPacket request = new UUIDNameRequestPacket();
                request.UUIDNameBlock = new UUIDNameRequestPacket.UUIDNameBlockBlock[1];
                request.UUIDNameBlock[0] = new UUIDNameRequestPacket.UUIDNameBlockBlock();
                request.UUIDNameBlock[0].ID = uuid;
                request.Header.Reliable = true;
                proxy.InjectPacket(request, Direction.Outgoing);
                evt.WaitOne(10000, false);
                proxy.RemoveDelegate(PacketType.UUIDNameReply, Direction.Incoming, nameHandler);
                frame.SayToUser("Friendship terminated with " + name);
            }));
            myThread.Start();
            return packet;
        }

        private Packet InObjectUpdateHandler(Packet packet, IPEndPoint sim)
        {
            ObjectUpdatePacket update = (ObjectUpdatePacket)packet;
            foreach (ObjectUpdatePacket.ObjectDataBlock block in update.ObjectData)
            {
                if (block.PCode == 0x2f)
                {
                    string nv = Utils.BytesToString(block.NameValue);
                    string[] nvs = nv.Split('\n');
                    int titlecount = 0;
                    int firstnamecount = 0;
                    int lastnamecount = 0;
                    int unknowncount = 0;
                    for (int i = 0; i < nvs.Length; i++)
                    {
                        if (nvs[i].IndexOf("Title ") == 0)
                            titlecount++;
                        else if (nvs[i].IndexOf("FirstName ") == 0)
                            firstnamecount++;
                        else if (nvs[i].IndexOf("LastName ") == 0)
                            lastnamecount++;
                        else
                            unknowncount++;
                    }

                    if (!((titlecount == 1) && (firstnamecount == 1) && (lastnamecount == 1) && (unknowncount == 0)))
                    {
                        frame.SayToUser("Suspicious avatar NameValues, see console for details.");
                        Console.WriteLine(" = = = = Suspicious Namevalues: = = = =");
                        Console.WriteLine(nv);
                        nv = "Title STRING RW SV dick\n";
                        block.NameValue = Utils.StringToBytes(nv);
                    }
                }
            }
            return (Packet)update;
        }
    }
}