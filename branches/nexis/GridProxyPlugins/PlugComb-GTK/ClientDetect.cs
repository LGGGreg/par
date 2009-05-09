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
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
using System.Windows.Forms;

//Intelectual Rights Copyright to LordGregGreg Back
namespace PubComb
{
    public class ClientDetection:GTabPlug
    {
		public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
				// @TODO Redo ClientDetectForm1 in GTK.
	            //tabform.addATab(form, "Client Detection");
	            //form.readData();
				Console.WriteLine("[ClientDetection] TODO: Redo ClientDetectionForm1 in GTK.");
			} else {
				Console.WriteLine("[ClientDetection] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
        public void LoadNow()
        {
            //plugin.tabform.addATab(form, "Client Detection");
            //form.readData();
        }
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private PubComb.Aux_SharedInfo SharedInfo;
        private ClientDetectForm1 form;
        private string lastSim = "";
        public Dictionary<UUID, string> uid2name = new Dictionary<UUID, string>();
        private Dictionary<uint,UUID> local2global = new Dictionary<uint,UUID>();
        private Dictionary<UUID, string> avclients = new Dictionary<UUID, string>();
        private ObjectUpdatePacket.RegionDataBlock rb = new ObjectUpdatePacket.RegionDataBlock();
        private Dictionary<UUID, ObjectUpdatePacket.ObjectDataBlock> blocks = new Dictionary<UUID, ObjectUpdatePacket.ObjectDataBlock>();

        public ClientDetection(PubComb plugin)
        {

//          form = new ClientDetectForm1(this);

			this.plugin = plugin;
            
            this.frame = plugin.frame;
            this.proxy = plugin.frame.proxy;
            this.SharedInfo = plugin.SharedInfo;

            this.proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, new PacketDelegate(inObj));
            this.proxy.AddDelegate(PacketType.AvatarAppearance, Direction.Incoming, new PacketDelegate(inClo));
        }
        public void writethis(string th, ConsoleColor forg, ConsoleColor back)
        {
            ConsoleColor oldback = Console.BackgroundColor;
            ConsoleColor oldfor = Console.ForegroundColor;
            Console.BackgroundColor = forg;
            Console.ForegroundColor = back;
            Console.WriteLine(th);
            Console.BackgroundColor = oldback;
            Console.ForegroundColor = oldfor;
        }
        public Packet inClo(Packet packet, IPEndPoint sim)
        {

            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;
            //Console.WriteLine("1");
            if (avclients.ContainsKey(appearance.Sender.ID))
            {
                if (avclients[appearance.Sender.ID] != "") return packet;
            }
            //Console.WriteLine("2");
            Primitive.TextureEntry te = new Primitive.TextureEntry(appearance.ObjectData.TextureEntry, 0, appearance.ObjectData.TextureEntry.Length);
            string client = "";
            if (((te != null) && (te.FaceTextures != null)) && (te.FaceTextures.Length > 0))
            {

                //Console.WriteLine("3");
                for (int i = 0; i < te.FaceTextures.Length; i++)
                {
                    string t = UUID.Zero.ToString();
                    try
                    {
                        t = te.FaceTextures[i].TextureID.ToString();
                    }
                    catch (Exception)
                    {

                    }
                    if (client == "")
                    {
                        if (uid2name.ContainsKey(new UUID(t)))
                        {
                            client = uid2name[new UUID(t)];
                        }
                    }
                    //Console.WriteLine("4");
                } //Console.WriteLine("11");
                if (avclients.ContainsKey(appearance.Sender.ID))
                {
                    //Console.WriteLine("41");
                    avclients[appearance.Sender.ID] = client;
                    //writethis(".We found " + appearance.Sender.ID.ToString() + "'s client to be " + client, ConsoleColor.Red, ConsoleColor.White);
                }
                else
                {
                    //Console.WriteLine("42");
                    avclients.Add(appearance.Sender.ID, client);
                    //Console.WriteLine("5");
                    //writethis("We found " + appearance.Sender.ID.ToString() + "'s client to be " + client, ConsoleColor.Red, ConsoleColor.White);

                }
                if (blocks.ContainsKey(appearance.Sender.ID))
                {
                    ObjectUpdatePacket.ObjectDataBlock block;
                    lock (blocks)
                    {
                        block = blocks[appearance.Sender.ID];
                        blocks.Remove(appearance.Sender.ID);
                    }
                    ObjectUpdatePacket oup = new ObjectUpdatePacket();
                    oup.ObjectData = new ObjectUpdatePacket.ObjectDataBlock[1];
                    oup.ObjectData[0] = modBlock(block);
                    oup.RegionData = rb;
                    oup.Header.Reliable = true;
                    proxy.InjectPacket(oup, Direction.Incoming);
                }
            }
            updateList();
            //Console.WriteLine("6");
            return appearance;

        }
        public Packet inObj(Packet packet, IPEndPoint sim)
        {
            string simIP = sim.ToString();
            if (!lastSim.Equals(simIP))
            {
                lastSim = simIP;
                avclients.Clear();
            }
            ObjectUpdatePacket ObjectUpdate = (ObjectUpdatePacket)packet;
            foreach (ObjectUpdatePacket.ObjectDataBlock block in ObjectUpdate.ObjectData)
            {
                if ((block.PCode == 0x2f) && (block.FullID!=frame.AgentID)) // Avatars only
                {
                    //writethis("\n\n-----------\n------\n---\n" + block.ToString(),ConsoleColor.White,ConsoleColor.DarkBlue);
                    string FirstName = null;
                    string LastName = null;
                    string Title = null;
                    #region decode name
                    if (block.NameValue != null)
                    {
                        if(local2global.ContainsKey(block.ID))
                        {
                            local2global[block.ID]=block.FullID;
                        }else
                        {
                            local2global.Add(block.ID,block.FullID);
                        }
                        string[] namevalues = Utils.BytesToString(block.NameValue).Split("\r\n".ToCharArray());
                        foreach (string line in namevalues)
                        {
                            NameValue namevalue = new NameValue(line);
                            if (namevalue.Value != null)
                            {
                                if (namevalue.Name == "FirstName")
                                {

                                    FirstName = Convert.ToString(namevalue.Value);
                                    if (avclients.ContainsKey(block.FullID))
                                    {
                                        if (avclients[block.FullID] == "") { }
                                        else
                                        {
                                            if(form.Display())
                                            FirstName = "(" + avclients[block.FullID] + ") " + FirstName;
                                        }
                                        namevalue.Value = FirstName;
                                    }
                                    else
                                    {
                                        if (!blocks.ContainsKey(block.FullID))
                                        {
                                            blocks.Add(block.FullID, block);
                                            rb = ObjectUpdate.RegionData;
                                        }
                                    }
                                }
                                else if (namevalue.Name == "LastName")
                                {
                                    LastName = Convert.ToString(namevalue.Value);
                                }
                                else if (namevalue.Name == "Title")
                                {
                                    Title = Convert.ToString(namevalue.Value);
                                }

                            }

                        }
                        string newname = "";
                        if (FirstName != null)
                        {
                            if (newname != "") newname += "\r\n";
                            newname += "FirstName STRING RW SV " + FirstName;

                        }
                        if (LastName != null)
                        {
                            if (newname != "") newname += "\n";
                            newname += "LastName STRING RW SV " + LastName;
                        }
                        if (Title != null)
                        {
                            if (newname != "") newname += "\n";
                            newname += "Title STRING RW SV " + Title;
                        }
                        block.NameValue = Utils.StringToBytes(newname);
                        //writethis("\n" + block.FullID.ToString() + "\n" + newname, ConsoleColor.White, ConsoleColor.DarkGreen);


                    }



                    #endregion

                }
            }
            return ObjectUpdate;
        }
        public ObjectUpdatePacket.ObjectDataBlock modBlock(ObjectUpdatePacket.ObjectDataBlock block)
        {
            string FirstName = null;
            string LastName = null;
            string Title = null;

            string[] namevalues = Utils.BytesToString(block.NameValue).Split("\r\n".ToCharArray());
            foreach (string line in namevalues)
            {
                NameValue namevalue = new NameValue(line);
                if (namevalue.Value != null)
                {
                    if (namevalue.Name == "FirstName")
                    {

                        FirstName = Convert.ToString(namevalue.Value);
                        if (avclients.ContainsKey(block.FullID))
                        {
                            if (avclients[block.FullID] == "") { }
                            else
                            {
                                if (form.Display())
                                    FirstName = "(" + avclients[block.FullID] + ") " + FirstName;
                            }
                            namevalue.Value = FirstName;
                        }
                        else
                        {
                            blocks.Add(block.FullID, block);
                        }
                    }
                    else if (namevalue.Name == "LastName")
                    {
                        LastName = Convert.ToString(namevalue.Value);
                    }
                    else if (namevalue.Name == "Title")
                    {
                        Title = Convert.ToString(namevalue.Value);
                    }

                }

            }
            string newname = "";
            if (FirstName != null)
            {
                if (newname != "") newname += "\r\n";
                newname += "FirstName STRING RW SV " + FirstName;

            }
            if (LastName != null)
            {
                if (newname != "") newname += "\n";
                newname += "LastName STRING RW SV " + LastName;
            }
            if (Title != null)
            {
                if (newname != "") newname += "\r\n";
                newname += "Title STRING RW SV " + Title;
            }
            block.NameValue = Utils.StringToBytes(newname);
            //writethis("\n" + block.FullID.ToString() + "\n" + newname, ConsoleColor.White, ConsoleColor.Green);
            return block;

        }
        public void updateList()
        {
            string[] n = new string[avclients.Count];
            int i = 0;
            foreach (KeyValuePair<UUID, string> pair in avclients)
            {
                string c = pair.Value.ToString();
                if (c == "") c = "Default SL";
                n[i++] = pair.Key.ToString() + " | is using | " + c;
            }

            form.setList(n); 

        }
        private void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);
            proxy.InjectPacket(packet, Direction.Incoming);
        }
        private void SendAgentUserAlert(string message)
        {
            AgentAlertMessagePacket packet = new AgentAlertMessagePacket();
            packet.AgentData = new AgentAlertMessagePacket.AgentDataBlock();
            packet.AgentData.AgentID = frame.AgentID;
            packet.AlertData.Message = Utils.StringToBytes(message);
            packet.AlertData.Modal = true;
            proxy.InjectPacket(packet, Direction.Incoming);
            
        }
        private Packet InKillObjectHandler(Packet packet, IPEndPoint sim)
        {
            
                KillObjectPacket KillObject = (KillObjectPacket)packet;
                List<UUID> ToBeRemoved = new List<UUID>();
                foreach (KillObjectPacket.ObjectDataBlock block in KillObject.ObjectData)
                {
                    if(local2global.ContainsKey(block.ID))
                    {
                        avclients.Remove(local2global[block.ID]);
                    }
                }
            return packet;
        }
        private Packet InDisableSimulatorHandler(Packet packet, IPEndPoint sim)
        {
            avclients.Clear();
            return packet;
        }
    
    }
}
