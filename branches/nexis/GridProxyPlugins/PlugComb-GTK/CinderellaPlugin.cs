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
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
using System.Threading;
using System.Windows.Forms;

namespace PubComb
{

    public class CinderellaPlugin : GTabPlug
    {
        public class CurrentUploadType
        {
            public string name;
            public int size;
            public byte[] data;
            public bool temp;
            public bool local;
            public bool paid;
            public uint ownerMask;
            public sbyte type;
        }
        public ProxyFrame frame;
        public Proxy proxy;
        private PubComb plugin;
        public Dictionary<UUID, Primitive.ParticleSystem> buffer = new Dictionary<UUID, Primitive.ParticleSystem>();
        UUID uploadID;
		UUID specialID;
        public CurrentUploadType curentUpload;
        public class lsotoolXfer
        {
            public UUID VFileID;
            public Dictionary<uint, SendXferPacketPacket> Packets = new Dictionary<uint, SendXferPacketPacket>();
            public int PacketIndex = 0;

            public lsotoolXfer(UUID vfileid)
            {
                VFileID = vfileid;
            }
        }
        public enum AssetType : sbyte
        {
            /// <summary>Unknown asset type</summary>
            Unknown = -1,
            /// <summary>Texture asset, stores in JPEG2000 J2C stream format</summary>
            Texture = 0,
            /// <summary>Sound asset</summary>
            Sound = 1,
            /// <summary>Calling card for another avatar</summary>
            CallingCard = 2,
            /// <summary>Link to a location in world</summary>
            Landmark = 3,
            /// <summary>Legacy script asset, you should never see one of these</summary>
            //[Obsolete]
            //Script = 4,
            /// <summary>Collection of textures and parameters that can be 
            /// worn by an avatar</summary>
            Clothing = 5,
            /// <summary>Primitive that can contain textures, sounds, 
            /// scripts and more</summary>
            Object = 6,
            /// <summary>Notecard asset</summary>
            Notecard = 7,
            /// <summary>Holds a collection of inventory items</summary>
            Folder = 8,
            /// <summary>Root inventory folder</summary>
            RootFolder = 9,
            /// <summary>Linden scripting language script</summary>
            LSLText = 10,
            /// <summary>LSO bytecode for a script</summary>
            LSLBytecode = 11,
            /// <summary>Uncompressed TGA texture</summary>
            TextureTGA = 12,
            /// <summary>Collection of textures and shape parameters that can
            /// be worn</summary>
            Bodypart = 13,
            /// <summary>Trash folder</summary>
            TrashFolder = 14,
            /// <summary>Snapshot folder</summary>
            SnapshotFolder = 15,
            /// <summary>Lost and found folder</summary>
            LostAndFoundFolder = 16,
            /// <summary>Uncompressed sound</summary>
            SoundWAV = 17,
            /// <summary>Uncompressed TGA non-square image, not to be used as a
            /// texture</summary>
            ImageTGA = 18,
            /// <summary>Compressed JPEG non-square image, not to be used as a
            /// texture</summary>
            ImageJPEG = 19,
            /// <summary>Animation</summary>
            Animation = 20,
            /// <summary>Sequence of animations, sounds, chat, and pauses</summary>
            Gesture = 21,
            /// <summary>Simstate file</summary>
            Simstate = 22,
        }
        public static Dictionary<ulong, lsotoolXfer> OutgoingXfers = new Dictionary<ulong, lsotoolXfer>();
        private CinderForm1 form;
        private int grabbed = 200;

        public CinderellaPlugin(PubComb plug)
        {
            plugin = plug;
            this.frame = plug.frame;
            this.proxy = plug.proxy;

            //this.proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, new PacketDelegate(ObjectUpdateHandler));
            //this.proxy.AddDelegate(PacketType.ObjectProperties, Direction.Incoming, new PacketDelegate(ObjectPropertiesHandler));
            this.proxy.AddDelegate(PacketType.ConfirmXferPacket, Direction.Incoming, new PacketDelegate(InConfirmXferPacketHandler));
            this.proxy.AddDelegate(PacketType.RequestXfer, Direction.Incoming, new PacketDelegate(ServerRequestsMyDataToStart));
            
            //formthread = new Thread(new ThreadStart(delegate()
            //{
                form = new CinderForm1(this);
              //  Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();

        }
		
		public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
	            tabform.addATab(form, "Cinderella");
			} else {
				Console.WriteLine("[CinderellaPlugin] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
		
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Cinderella");
            
        }
        public void payMoney(int amount)
        {
            MoneyTransferRequestPacket p = new MoneyTransferRequestPacket();
            p.AgentData.AgentID = frame.AgentID;
            p.AgentData.SessionID = frame.SessionID;
            p.MoneyData.AggregatePermInventory = 0;
            p.MoneyData.AggregatePermNextOwner = 0;
            p.MoneyData.Amount = amount;
            p.MoneyData.Flags = 0;
            p.MoneyData.SourceID = frame.AgentID;
            p.MoneyData.TransactionType = 1101;
            p.MoneyData.DestID = UUID.Zero;
            p.MoneyData.Description = Utils.StringToBytes("Upload Fee");

            this.proxy.InjectPacket(p, Direction.Outgoing);

        }
        private Packet InConfirmXferPacketHandler(Packet packet, IPEndPoint sim)
        {
            ConfirmXferPacketPacket ConfirmXferPacket = (ConfirmXferPacketPacket)packet;

            ulong XferID = ConfirmXferPacket.XferID.ID;
            if (OutgoingXfers.ContainsKey(XferID))
            { // This is one of our Xfers
                //SayToUser("sending the script packets...");
                //OutgoingXfers[XferID].PacketIndex++;
                form.setProgress(OutgoingXfers[XferID].PacketIndex++);
                Console.WriteLine("progress is " + OutgoingXfers[XferID].PacketIndex + " of " + curentUpload.size.ToString());
                if (OutgoingXfers[XferID].PacketIndex < (curentUpload.size))
                {
                    // Send the next packet
                    proxy.InjectPacket(OutgoingXfers[XferID].Packets[(uint)OutgoingXfers[XferID].PacketIndex], Direction.Outgoing);
                }
                else
                {
                    // Done done done
                    form.setProgress(curentUpload.size);
                    Console.WriteLine("<>>Upload successful!");
                    if (grabbed == 1)
                    {
                        grabbed += 200;
                        CreateInventoryItemPacket create = new CreateInventoryItemPacket();
                        create.AgentData = new CreateInventoryItemPacket.AgentDataBlock();
                        create.AgentData.AgentID = frame.AgentID;
                        create.AgentData.SessionID = frame.SessionID;
                        create.InventoryBlock = new CreateInventoryItemPacket.InventoryBlockBlock();
                        create.InventoryBlock.CallbackID = 0; // What's this?             
                        create.InventoryBlock.FolderID = UUID.Zero;
                        create.InventoryBlock.TransactionID = uploadID;
                        create.InventoryBlock.NextOwnerMask = curentUpload.ownerMask;
                        create.InventoryBlock.Type = curentUpload.type;
                        create.InventoryBlock.InvType = curentUpload.type;
                        create.InventoryBlock.WearableType = (byte)0;
                        string name = curentUpload.name;

                        if (curentUpload.temp)
                            name += "-temp";
                        if (curentUpload.local)
                            name += "-local";
                        if (curentUpload.paid)
                            name += "-paid";

                        //name += "-t=" + curentUpload.type.ToString();


                        create.InventoryBlock.Name = Utils.StringToBytes(name);


                        create.InventoryBlock.Description = Utils.StringToBytes(name);
                        proxy.InjectPacket(create, Direction.Outgoing);
                    }


                    OutgoingXfers.Remove(XferID);

                }
                // Don't send this to the client
                return null;
            }
            return packet;
        }
        private Packet ServerRequestsMyDataToStart(Packet packet, IPEndPoint sim)
        {
            RequestXferPacket requestRecived = (RequestXferPacket)packet;
            /*SayToUser("vfile id is " + requestRecived.XferID.VFileID.UUID +
                " and id is " + requestRecived.XferID.ID.ToString()
                +" and uploadid is "+ uploadID 
                + " and special id is "+specialID
                +" and filename is "+Utils.BytesToString(requestRecived.XferID.Filename)
                +" and filepath is "+requestRecived.XferID.FilePath.ToString());
          
            */

            if (grabbed == 0)
            {
                grabbed++;
                Console.WriteLine("Server tells me to start data");

                byte[][] datapackets = OpenBytecode(curentUpload.data);


                SendXferPacketPacket SendXferPacket;
                OutgoingXfers.Add(requestRecived.XferID.ID, new lsotoolXfer(requestRecived.XferID.VFileID));
                for (uint i = 0; i < (curentUpload.size); i++)
                {
                    SendXferPacket = new SendXferPacketPacket();
                    SendXferPacket.XferID = new SendXferPacketPacket.XferIDBlock();
                    SendXferPacket.XferID.ID = requestRecived.XferID.ID;
                    SendXferPacket.XferID.Packet = i;
                    if (i == (curentUpload.size - 1))
                        SendXferPacket.XferID.Packet = i | 0x80000000;
                    SendXferPacket.DataPacket = new SendXferPacketPacket.DataPacketBlock();
                    SendXferPacket.DataPacket.Data = datapackets[i];
                    OutgoingXfers[requestRecived.XferID.ID].Packets.Add(i, SendXferPacket);
                }

                // Start sending the new packets to the sim
                OutgoingXfers[requestRecived.XferID.ID].PacketIndex = 0;
                proxy.InjectPacket(OutgoingXfers[requestRecived.XferID.ID].Packets[0], Direction.Outgoing);
                //this.Scriptdata = OpenBytecode(fileDest);            

                return null;
            }
            return packet;
        }
        public void SaveToSLInventory(byte[] uploaddata, String nname, sbyte type, bool copy, bool mod, bool trans, bool pay, bool local, bool temp)
        {
            Console.WriteLine("Atempting to save to inv");
            uploadID = UUID.Random();
            AssetUploadRequestPacket asetUp = new AssetUploadRequestPacket();
            asetUp.AssetBlock.StoreLocal = local;

            asetUp.AssetBlock.Tempfile = temp;
            asetUp.AssetBlock.TransactionID = uploadID;
            specialID = UUID.Combine(uploadID, frame.SessionID);
            asetUp.AssetBlock.Type = (sbyte)type;

            asetUp.AssetBlock.AssetData = new byte[] { };
            proxy.InjectPacket(asetUp, Direction.Outgoing);

            this.curentUpload = new CurrentUploadType();
            curentUpload.name = nname;
            curentUpload.data = uploaddata;
            curentUpload.temp = temp;
            curentUpload.type = (sbyte)type;
            curentUpload.local = local;
            curentUpload.paid = pay;

            grabbed = 0;
            if (pay)
                payMoney(10);
            uint newperms = 0;
            if (mod) newperms |= (uint)PermissionMask.Modify;
            if (copy) newperms |= (uint)PermissionMask.Copy;
            if (trans) newperms |= (uint)PermissionMask.Transfer;

            curentUpload.ownerMask = newperms;

            //generate owner mask


        }
        /*public byte[][] OpenBytecode2(byte[] source)
        {

            //byte[][] packets = new byte[17][];
            byte[][] packets = new byte[curentUpload.size][];
            int bysize = int.Parse(source.Length.ToString(), System.Globalization.NumberStyles.HexNumber) + 4;
            //int bysize =BitConverter.GetBytes(source.Length).Length+4;
            byte[] bytecode = new byte[bysize];
            byte[] a = BitConverter.GetBytes(bysize - 4);
            Console.WriteLine("The bytes are " + BitConverter.ToString(a, 0) + " and the source length is " + source.Length.ToString() +
                " and the bysize is " + bysize.ToString() + " so wtf ?");
            Buffer.BlockCopy(a, 0, bytecode, 0, 4);
            Buffer.BlockCopy(source, 0, bytecode, 4, source.Length);

            packets[0] = new byte[1004];
            Buffer.BlockCopy(bytecode, 0, packets[0], 0, 1004);
            for (int i = 1; i < (curentUpload.size - 1); i++)
            {
                packets[i] = new byte[1000];
                Buffer.BlockCopy(bytecode, 4 + (i * 1000), packets[i], 0, 1000);
            }
            packets[(curentUpload.size - 1)] = new byte[384];
            Buffer.BlockCopy(bytecode, bysize - 384, packets[(curentUpload.size - 1)], 0, 384);

            return packets;
        }*/
        public byte[][] OpenBytecode(byte[] source)
        {
            int remainingBytes = source.Length;
            int np = 0;
            byte[][] tempPackets = new byte[200000][];

            tempPackets[0] = new byte[1004];
            if (remainingBytes > 1000)
            {
                Buffer.BlockCopy(source, 0, tempPackets[0], 4, 1000);
                remainingBytes -= 1000;
                np++;
                while (remainingBytes > 1000)
                {
                    tempPackets[np] = new byte[1000];
                    Buffer.BlockCopy(source, 1000 * np, tempPackets[np], 0, 1000);
                    np++;
                    remainingBytes -= 1000;
                }
                tempPackets[np] = new byte[remainingBytes];
                Buffer.BlockCopy(source, 1000 * np, tempPackets[np], 0, remainingBytes);
            }
            else
            {
                Buffer.BlockCopy(source, 0, tempPackets[0], 4, remainingBytes);
            }
            np++;
            curentUpload.size = np;
            //fill in first 4 bytes
            Buffer.BlockCopy(BitConverter.GetBytes(source.Length + 4), 0, tempPackets[0], 0, 4);

            //rebuild it all the right size
            byte[][] packets = new byte[np][];
            for (int i = 0; i < np; i++)
            {
                packets[i] = tempPackets[i];
            }

            form.setProgressProps(np);
            return packets;
        }
        
        public void SayToUser(string message)
        {
            ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
            packet.ChatData.FromName = Utils.StringToBytes("Cinderella Plugin");
            packet.ChatData.SourceID = UUID.Random();
            packet.ChatData.OwnerID = frame.AgentID;
            packet.ChatData.SourceType = (byte)2;
            packet.ChatData.ChatType = (byte)1;
            packet.ChatData.Audible = (byte)1;
            packet.ChatData.Position = new Vector3(0, 0, 0);
            packet.ChatData.Message = Utils.StringToBytes(message);
            proxy.InjectPacket(packet, Direction.Incoming);
        }
        public void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
    }
}