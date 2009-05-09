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

/** HEAVY RECODE IN PROGRESS, AS LISTBOXES ARE NOT AVAILABLE IN GTK, AND NODEVIEWS ARE A BIT MORE COMPLEX THAN WINFORM LISTBOXES. **/
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
using Gtk;
//Intelectual Rights Copyright to LordGregGreg Back
namespace PubComb
{
    public class RadarChatPlugin : GTabPlug
    {
		public class AvatarBlip : Gtk.TreeNode
		{
			[TreeNodeValue (Column=0)]
			public string Name="[Fetching]";
			
			[TreeNodeValue (Column=1)]
			public string UUID="-";
			
			public AvatarBlip(string UUID)
			{
				this.UUID=UUID;
			}
			
			public AvatarBlip(UUID ID,string Name)
			{
				this.UUID=ID.ToString();
				this.Name=Name;
			}
		}
        public PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private RadarChatFormGTK form;
        public DateTime start;
        public PubComb.Aux_SharedInfo shared;
		public List<AvatarBlip> Blips = new List<AvatarBlip>();
        public List<UUID> avs = new List<UUID>();
        public List<UUID> pending = new List<UUID>();
		
		public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
	            tabform.addATab(form, "Radar Chat");
	            form.readData();
			} else {
				Console.WriteLine("[RadarChat] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
		
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
            form = new RadarChatFormGTK(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, new PacketDelegate(GotName));
            this.proxy.AddDelegate(PacketType.CoarseLocationUpdate, Direction.Incoming, new PacketDelegate(LocationIN));
            this.proxy.AddDelegate(PacketType.SoundTrigger, Direction.Incoming, new PacketDelegate(inSound));
        }
        private Packet GotName(Packet p, IPEndPoint sim)
        {
            UUIDNameReplyPacket n = (UUIDNameReplyPacket)p;
			// WHy lock this twice?!
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
                            lock (pending)
                            {
                                if (pending.Contains(b.ID))
                                {
                                    pending.Remove(b.ID);
									bool Found=false;
                                    lock (form.RadarList.NodeStore)
                                    {
										foreach(ITreeNode tn in form.RadarList.NodeStore)
										{
											AvatarBlip ab = (AvatarBlip)tn;
											if(shared.key2name[b.ID].Equals(ab.Name))
												Found=true;
										}
										if(!Found)
											form.RadarList.NodeStore.AddNode(new AvatarBlip(b.ID,shared.key2name[b.ID]));
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return p;
        }
        private Packet inSound(Packet p, IPEndPoint sim)
        {
            if (form.IsRadarEnabled)
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
            
                //if (sim.Address != shared.ip || sim.Port != shared.port) return p;
                if (sim.ToString() != proxy.activeCircuit.ToString()) return p;
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
                            //form.RadarList.NodeStore.AddNode(new AvatarBlip(a.AgentID.ToString()));
                            if (shared.key2name.ContainsKey(a.AgentID))
                            {
                                form.RadarList.NodeStore.AddNode(new AvatarBlip(a.AgentID,shared.key2name[a.AgentID]));
                            }
                            else
                            {
                                plugin.RequestNameFromKey(a.AgentID);
                                pending.Add(a.AgentID);
                            }
                        }

                        temp.Add(a.AgentID);
                    }
                    foreach (UUID u in avs)
                    {
                        if (!temp.Contains(u))
                        {
                            say = true;
              
                            if(shared.key2name.ContainsKey(u))
                            {
							    bool Found=false;
								foreach(ITreeNode tn in form.RadarList.NodeStore)
								{
									AvatarBlip ab = (AvatarBlip)tn;
									if(shared.key2name[u].Equals(ab.Name))
										Found=true;
								}
								if(Found)
									form.RadarList.NodeStore.RemoveNode(new AvatarBlip(u,shared.key2name[u]));                            
                            }
                        }
                    }
                }
                avs=temp;

                if (say)
                {
                    if (form.IsRadarEnabled)
                    {
                        sendUpdate();
                    }
                }
                    
            
    
            return p;
        }
        public void sendUpdate()
        {
            TimeSpan ts = (System.DateTime.Now - start);
            string prefix = (ts.TotalMilliseconds + 39840.0f).ToString() + form.Delimiter + avs.Count.ToString() + form.Delimiter;
            
            string whattosay = prefix;
            foreach (UUID uid in avs)
            {
                string id = uid.ToString();
                whattosay += id + form.Delimiter;
                if (whattosay.Length > 200)
                {
                    DiagReply(whattosay.Remove(whattosay.Length-1), form.RadarChannel);
                    whattosay = prefix;
                }
            }
            if(whattosay!=prefix)
                DiagReply(whattosay.Remove(whattosay.Length-1), form.RadarChannel);
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
        public void EstateOwnerMessage(string method, string param)
        {
            List<string> listParams = new List<string>();
            listParams.Add(param);
            EstateOwnerMessage(method, listParams);
        }

        /// <summary>
        /// Used for setting and retrieving various estate panel settings
        /// </summary>
        /// <param name="method">EstateOwnerMessage Method field</param>
        /// <param name="listParams">List of parameters to include</param>
        public void EstateOwnerMessage(string method, List<string> listParams)
        {
            EstateOwnerMessagePacket estate = new EstateOwnerMessagePacket();
            estate.AgentData.AgentID = frame.AgentID;
            estate.AgentData.SessionID = frame.SessionID;
            estate.AgentData.TransactionID = UUID.Zero;
            estate.MethodData.Invoice = UUID.Random();
            estate.MethodData.Method = Utils.StringToBytes(method);
            estate.ParamList = new EstateOwnerMessagePacket.ParamListBlock[listParams.Count];
            for (int i = 0; i < listParams.Count; i++)
            {
                estate.ParamList[i] = new EstateOwnerMessagePacket.ParamListBlock();
                estate.ParamList[i].Parameter = Utils.StringToBytes(listParams[i]);
            }
            proxy.InjectPacket(estate, Direction.Outgoing);
        }

        /// <summary>
        /// Kick an avatar from an estate
        /// </summary>
        /// <param name="userID">Key of Agent to remove</param>
        public void KickUser(UUID userID)
        {
            EstateOwnerMessage("kickestate", userID.ToString());
        }
        /// <summary>Used by EstateOwnerMessage packets</summary>
        public enum EstateAccessDelta
        {
            BanUser = 64,
            BanUserAllEstates = 66,
            UnbanUser = 128,
            UnbanUserAllEstates = 130
        }
        public void EjectUser(UUID targetID, bool ban)
        {
            EjectUserPacket eject = new EjectUserPacket();
            eject.AgentData.AgentID = frame.AgentID;
            eject.AgentData.SessionID = frame.SessionID;
            eject.Data.TargetID = targetID;
            if (ban) eject.Data.Flags = 1;
            else eject.Data.Flags = 0;

            proxy.InjectPacket(eject, Direction.Outgoing);
        }

        /// <summary>
        /// Freeze or unfreeze an avatar over your land
        /// </summary>
        /// <param name="targetID">target key to freeze</param>
        /// <param name="freeze">true to freeze, false to unfreeze</param>
        public void FreezeUser(UUID targetID, bool freeze)
        {
            FreezeUserPacket frz = new FreezeUserPacket();
            frz.AgentData.AgentID = frame.AgentID;
            frz.AgentData.SessionID = frame.SessionID;
            frz.Data.TargetID = targetID;
            if (freeze) frz.Data.Flags = 0;
            else frz.Data.Flags = 1;

            proxy.InjectPacket(frz, Direction.Outgoing);
        }

        /// <summary>
        /// Ban an avatar from an estate</summary>
        /// <param name="userID">Key of Agent to remove</param>
        /// <param name="allEstates">Ban user from this estate and all others owned by the estate owner</param>
        public void BanUser(UUID userID, bool allEstates)
        {
            List<string> listParams = new List<string>();
            uint flag = allEstates ? (uint)EstateAccessDelta.BanUserAllEstates : (uint)EstateAccessDelta.BanUser;
            listParams.Add(frame.AgentID.ToString());
            listParams.Add(flag.ToString());
            listParams.Add(userID.ToString());
            EstateOwnerMessage("estateaccessdelta", listParams);
        }

        

        /// <summary>
        /// Send an avatar back to their home location
        /// </summary>
        /// <param name="pest">Key of avatar to send home</param>
        public void TeleportHomeUser(UUID pest)
        {
            List<string> listParams = new List<string>();
            listParams.Add(frame.AgentID.ToString());
            listParams.Add(pest.ToString());
            EstateOwnerMessage("teleporthomeuser", listParams);
        }
    }
     

}