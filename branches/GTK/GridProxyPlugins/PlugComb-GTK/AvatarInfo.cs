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
    public class AvatarTracker:GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;

        public PubComb.Aux_SharedInfo SharedInfo;

        private AvatarFormGTK form;
        public Thread formthread;
		
		public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
	            tabform.addATab(form, "Avatar Loc");
	            //form.readData();
			} else {
				Console.WriteLine("[AvatarInfo] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
		
        public void LoadNow()
        {
            plugin.tabform.addATab(form,"Avatar Loc"); 
        }
        public AvatarTracker(PubComb plugin)
        {
            this.plugin = plugin;
            this.frame = plugin.frame;
            this.proxy = plugin.frame.proxy;
            this.SharedInfo = plugin.SharedInfo;

            this.proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, new PacketDelegate(InObjectUpdateHandler));
            this.proxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, Direction.Incoming, new PacketDelegate(InImprovedTerseObjectUpdateHandler));
            this.proxy.AddDelegate(PacketType.KillObject, Direction.Incoming, new PacketDelegate(InKillObjectHandler));
            this.proxy.AddDelegate(PacketType.DisableSimulator, Direction.Incoming, new PacketDelegate(InDisableSimulatorHandler));
            form = new AvatarFormGTK(this);
            
            //formthread = new Thread(new ThreadStart(showform));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
        }

        

        private Packet InObjectUpdateHandler(Packet packet, IPEndPoint sim)
        {
            string simIP = sim.ToString();
            ObjectUpdatePacket ObjectUpdate = (ObjectUpdatePacket)packet;
            foreach (ObjectUpdatePacket.ObjectDataBlock block in ObjectUpdate.ObjectData)
            {
                if (block.PCode == 0x2f) // Avatars only
                {
                    // If this simulator doesn't exist, create it
                    if (!SharedInfo.Aux_Simulators.ContainsKey(simIP))
                    {
                        SharedInfo.Aux_Simulators.Add(simIP, new PubComb.Aux_Simulator(simIP));
                    }


                    Vector3 position = new Vector3();
                    #region decode position
                    switch (block.ObjectData.Length)
                    {
                        case 76:
                            position = new Vector3(block.ObjectData, 16);
                            break;
                        case 60:
                            position = new Vector3(block.ObjectData, 0);
                            break;
                        case 32:
                            // The data is an array of unsigned shorts
                            position = new Vector3(
                                Utils.UInt16ToFloat(block.ObjectData, 0, -0.5f * 256.0f, 1.5f * 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, 2, -0.5f * 256.0f, 1.5f * 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, 4, -256.0f, 3.0f * 256.0f));
                            break;
                        case 16:
                            // The data is an array of single bytes (8-bit numbers)
                            position = new Vector3(
                                Utils.ByteToFloat(block.ObjectData, 0, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, 1, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, 2, -256.0f, 256.0f));
                            break;
                        default:
                            plugin.SayToUser("Couldn't decode position, the data was " + block.ObjectData.Length.ToString() + " bytes long.");
                            break;
                    }
                    #endregion

                    string FirstName = null;
                    string LastName = null;
                    #region decode name
                    if (block.NameValue != null)
                    {
                        string[] namevalues = Utils.BytesToString(block.NameValue).Split("\r\n".ToCharArray());
                        foreach (string line in namevalues)
                        {
                            NameValue namevalue = new NameValue(line);
                            if (namevalue.Value != null)
                            {
                                if (namevalue.Name == "FirstName")
                                {
                                    FirstName = Convert.ToString(namevalue.Value);
                                }
                                else if (namevalue.Name == "LastName")
                                {
                                    LastName = Convert.ToString(namevalue.Value);
                                }
                            }
                        }
                    }
                    string name = "";
                    if (FirstName != null) name += FirstName;
                    else name += "(Waiting)";
                    name += " ";
                    if (LastName != null) name += LastName;
                    else name += "(Waiting)";
                    #endregion

                    PubComb.Aux_Avatar avatar = new PubComb.Aux_Avatar(block.FullID, simIP, block.ID, name, position);
                    if (!SharedInfo.Aux_Simulators[simIP].Avatars.ContainsKey(block.FullID))
                    {
                        // Add avatar to simulator
                        SharedInfo.Aux_Simulators[simIP].Avatars.Add(block.FullID, avatar);

                         form.AddAvatar(avatar);
                    }
                    else
                    {
                        // Just update the avatar's position
                        SharedInfo.Aux_Simulators[simIP].Avatars[block.FullID].Position = position;
                         form.UpdateAvatar(avatar); 
                    }
                    if (avatar.UUID == frame.AgentID)
                    {
                        SharedInfo.AgentName = name;
                        SharedInfo.AvPosition = position;
                    }
                }
            }
            return packet;
        }

        private Packet InImprovedTerseObjectUpdateHandler(Packet packet, IPEndPoint sim)
        {
            string simIP = sim.ToString();
            // If the sim doesn't exist, add it
            if (!SharedInfo.Aux_Simulators.ContainsKey(simIP))
            {
                SharedInfo.Aux_Simulators.Add(simIP, new PubComb.Aux_Simulator(simIP));
            }

            ImprovedTerseObjectUpdatePacket update = (ImprovedTerseObjectUpdatePacket)packet;
            foreach (ImprovedTerseObjectUpdatePacket.ObjectDataBlock block in update.ObjectData)
            {
                // Is it an avatar?
                if (block.Data[5] != 0)
                {
                    uint localid = Utils.BytesToUInt(block.Data, 0);
                    PubComb.Aux_Avatar avatar = SharedInfo.Aux_Simulators[simIP].AvatarByLocalID(localid);
                    if (avatar != null)
                    {
                        // update position
                        avatar.Position = new Vector3(block.Data, 22);
                        SharedInfo.Aux_Simulators[simIP].Avatars[avatar.UUID].Position = avatar.Position;
                        if (avatar.UUID == frame.AgentID)
                        {
                            SharedInfo.AvPosition = avatar.Position;
                        }
                         form.UpdateAvatar(avatar); 
                    }
                }
            }

            return packet;
        }

        private Packet InKillObjectHandler(Packet packet, IPEndPoint sim)
        {
            string simIP = sim.ToString();
            if (SharedInfo.Aux_Simulators.ContainsKey(simIP))
            {
                KillObjectPacket KillObject = (KillObjectPacket)packet;
                List<UUID> ToBeRemoved = new List<UUID>();
                foreach (KillObjectPacket.ObjectDataBlock block in KillObject.ObjectData)
                {
                    PubComb.Aux_Avatar avatar = SharedInfo.Aux_Simulators[simIP].AvatarByLocalID(block.ID);
                    if (avatar != null)
                    {
                         form.RemoveAvatar(avatar); 
                        ToBeRemoved.Add(avatar.UUID);
                    }
                }
                foreach (UUID uuid in ToBeRemoved)
                {
                    SharedInfo.Aux_Simulators[simIP].Avatars.Remove(uuid);
                }
            }
            return packet;
        }

        private Packet InDisableSimulatorHandler(Packet packet, IPEndPoint sim)
        {
            string simIP = sim.ToString();
            if (SharedInfo.Aux_Simulators.ContainsKey(simIP))
            {
                if (SharedInfo.Aux_Simulators[simIP].Avatars.Count > 0)
                {
                    lock (SharedInfo.Aux_Simulators[sim.ToString()].Avatars)
                    {
                        foreach (KeyValuePair<UUID, PubComb.Aux_Avatar> UUIDandAvatar in SharedInfo.Aux_Simulators[simIP].Avatars)
                        {
                            form.RemoveAvatar(UUIDandAvatar.Value); 
                        }
                    }
                }
                SharedInfo.Aux_Simulators.Remove(simIP);
            }
            return packet;
        }
    }
}
