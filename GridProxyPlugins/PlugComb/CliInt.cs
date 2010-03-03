/*
 * Copyright (c) 2009, Gregory Maurer(SL)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the Gregory Maurer nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * CliInt Plugin for PAR.
 * Developed by Gregory Maurer. Using code from OpenMetaverse, other PAR plugins, and ClientAO.
 * Used Lyra as a template
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
//using libsecondlife;
using GridProxy;
using System.Threading;
using System.Windows.Forms;

namespace PubCombN
{
    public struct ObjectUpdate
    {
        /// <summary></summary>
        public bool Avatar;
        /// <summary></summary>
        public Vector4 CollisionPlane;
        /// <summary></summary>
        public byte State;
        /// <summary></summary>
        public uint LocalID;
        /// <summary></summary>
        public Vector3 Position;
        /// <summary></summary>
        public Vector3 Velocity;
        /// <summary></summary>
        public Vector3 Acceleration;
        /// <summary></summary>
        public Quaternion Rotation;
        /// <summary></summary>
        public Vector3 AngularVelocity;
        /// <summary></summary>
        public Primitive.TextureEntry Textures;
    }
    public class CliIntPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private CliIntForm1 form;
        private Vector3 mpos = new Vector3();
        private uint mid = new uint();
        public InventoryItem interceptor = new InventoryItem(UUID.Zero);
        public DateTime lastRez;

        public void LoadNow()
        {
            tabInfo t = getInfo();
            plugin.tabform.addATab(t.f, t.s);
        }
        public tabInfo getInfo()
        {
            form.readData();
            return new tabInfo(form, "CliInt");
            
        }

        public CliIntPlugin(PubComb plug)
        {           
            plugin = plug;
            form = new CliIntForm1(this);
            //plug.tabform.addATab(form, "CliInt");
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            proxy.AddDelegate(PacketType.AlertMessage, Direction.Incoming, new PacketDelegate(this.InAlertMessageHandler));
            proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, new PacketDelegate(this.UpdateHandler));
            proxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, Direction.Incoming, new PacketDelegate(this.TerseUpdateHandler));
            interceptor.Name = "idontexist";
        }

        private Packet InAlertMessageHandler(Packet packet, IPEndPoint sim)
        {
            AlertMessagePacket AlertMessage = (AlertMessagePacket)packet;
            string message = Utils.BytesToString(AlertMessage.AlertData.Message);
            if (message.StartsWith("Can't rez object '" + interceptor.Name + "'" ))
            {
                return null; // spare the user the spam
            }
            return packet;
        }

        //Process commands from the user
        public void ScanForInterceptor()
        {
            //SayToUser("Looking for the specified interceptor.");
            if (form.getCheck1() == true)
            FindIntercept(form.getbox());
        }

        private void ObjectWithVel(Vector3 detpos, Vector3 detvel)
        {
            //Console.WriteLine("ObjectWithVel Called.");
            if (form.getCheck1() == true)
            {
                //Console.WriteLine("Found obj with vel.");
                if (interceptor.Name != "idontexist")
                {
                    //Console.WriteLine("Interceptor exists.");
                    Vector3 detnorm = Vector3.Normalize(detvel);
                    float detdist = Vector3.Distance(plugin.SharedInfo.AvPosition, detpos);
                    Vector3 detdest = (detnorm*detdist)+detpos;
                    float detdestrange =Vector3.Distance(detdest,plugin.SharedInfo.AvPosition);
                   
                    if (detdestrange<=2.5&&Vector3.Mag(detvel)>=5.0)
                    {
                        if (
                        ((TimeSpan)(System.DateTime.Now - lastRez)).TotalMilliseconds > 300)
                        {
                            lastRez = System.DateTime.Now;
                            //Console.WriteLine("Object is within range. Rez teh interceptor.");
                            PseudoLLRezObject(interceptor, (Vector3.Normalize((detpos - plugin.SharedInfo.AvPosition)) * 2.0f)+plugin.SharedInfo.AvPosition);
                        }
                        //Console.WriteLine("We rezzed teh interceptor.");
                        //LogPacket((Packet)rez, Direction.Outgoing);
                    }
                    //else Console.WriteLine("Object out of range.");
                }
                //else Console.WriteLine("Interceptor does not exist.");
            }
            //else Console.WriteLine("Found object, but button is unchecked.");
        }

        /// <summary>
        /// A function to rez name from inventory.
        /// </summary>
        /// <param name="name">InventoryItem to rez</param>
        /// <param name="pos">Local sim position</param>
        private void PseudoLLRezObject(InventoryItem name, Vector3 pos)
        {
            RezObjectPacket rez = new RezObjectPacket();
            rez.AgentData = new RezObjectPacket.AgentDataBlock();
            rez.AgentData.AgentID = frame.AgentID;
            rez.AgentData.SessionID = frame.SessionID;
            rez.AgentData.GroupID = UUID.Zero;
            rez.RezData = new RezObjectPacket.RezDataBlock();
            rez.RezData.FromTaskID = UUID.Zero;
            rez.RezData.BypassRaycast = 1;
            rez.RezData.RayStart = pos;
            rez.RezData.RayEnd = pos;
            rez.RezData.RayTargetID = UUID.Zero;
            rez.RezData.RayEndIsIntersection = false;
            rez.RezData.RezSelected = false;
            rez.RezData.RemoveItem = false;
            rez.RezData.ItemFlags = 0;
            rez.RezData.GroupMask = 0;
            rez.RezData.EveryoneMask = 0;
            rez.RezData.NextOwnerMask = 0;
            rez.InventoryData = new RezObjectPacket.InventoryDataBlock();
            rez.InventoryData.ItemID = name.UUID;
            rez.InventoryData.FolderID = name.ParentUUID;
            rez.InventoryData.CreatorID = name.CreatorID;
            rez.InventoryData.OwnerID = name.OwnerID;
            rez.InventoryData.GroupID = name.GroupID;
            rez.InventoryData.BaseMask = 0;
            rez.InventoryData.OwnerMask = 0;
            rez.InventoryData.GroupMask = 0;
            rez.InventoryData.EveryoneMask = 0;
            rez.InventoryData.NextOwnerMask = 0;
            rez.InventoryData.GroupOwned = false;
            rez.InventoryData.TransactionID = UUID.Zero;
            rez.InventoryData.Type = 6;
            rez.InventoryData.InvType = 6;
            rez.InventoryData.Flags = 0;
            rez.InventoryData.SaleType = 0;
            rez.InventoryData.SalePrice = 10;
            rez.InventoryData.Name = Utils.StringToBytes(name.Name);
            rez.InventoryData.Description = Utils.StringToBytes(name.Description);
            rez.InventoryData.CreationDate = 0x7fffffff; //name.CreationDate;
            rez.InventoryData.CRC = 0;
            rez.Header.Reliable = true;
            proxy.InjectPacket((Packet)rez, Direction.Outgoing);
        }

        /// <summary>
        /// Used for new prims, or significant changes to existing prims (From OpenMetaverse, with edits)
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="simulator"></param>
        private Packet UpdateHandler(Packet packet, IPEndPoint simulator)
        {
            ObjectUpdatePacket update = (ObjectUpdatePacket)packet;
			//UpdateDilation(simulator, update.RegionData.TimeDilation);
            for (int b = 0; b < update.ObjectData.Length; b++)
            {
                ObjectUpdatePacket.ObjectDataBlock block = update.ObjectData[b];
                if (block.FullID == frame.AgentID)
                {
                    Vector4 collisionPlane = Vector4.Zero;
                    Vector3 position;
                    Vector3 velocity;
                    Vector3 acceleration;
                    Quaternion rotation;
                    Vector3 angularVelocity;
                    //NameValue[] nameValues;
                    //bool attachment = false;
                    PCode pcode = (PCode)block.PCode;
                    #region Decode Additional packed parameters in ObjectData
                    int pos = 0;
                    switch (block.ObjectData.Length)
                    {
                        case 76:
                            // Collision normal for avatar
                            collisionPlane = new Vector4(block.ObjectData, pos);
                            pos += 16;

                            goto case 60;
                        case 60:
                            // Position
                            position = new Vector3(block.ObjectData, pos);
                            pos += 12;
                            // Velocity
                            velocity = new Vector3(block.ObjectData, pos);
                            pos += 12;
                            // Acceleration
                            acceleration = new Vector3(block.ObjectData, pos);
                            pos += 12;
                            // Rotation (theta)
                            rotation = new Quaternion(block.ObjectData, pos, true);
                            pos += 12;
                            // Angular velocity (omega)
                            angularVelocity = new Vector3(block.ObjectData, pos);
                            pos += 12;

                            break;
                        case 48:
                            // Collision normal for avatar
                            collisionPlane = new Vector4(block.ObjectData, pos);
                            pos += 16;

                            goto case 32;
                        case 32:
                            // The data is an array of unsigned shorts

                            // Position
                            position = new Vector3(
                                Utils.UInt16ToFloat(block.ObjectData, pos, -0.5f * 256.0f, 1.5f * 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 2, -0.5f * 256.0f, 1.5f * 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 4, -256.0f, 3.0f * 256.0f));
                            pos += 6;
                            // Velocity
                            velocity = new Vector3(
                                Utils.UInt16ToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 4, -256.0f, 256.0f));
                            pos += 6;
                            // Acceleration
                            acceleration = new Vector3(
                                Utils.UInt16ToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 4, -256.0f, 256.0f));
                            pos += 6;
                            // Rotation (theta)
                            rotation = new Quaternion(
                                Utils.UInt16ToFloat(block.ObjectData, pos, -1.0f, 1.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 2, -1.0f, 1.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 4, -1.0f, 1.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 6, -1.0f, 1.0f));
                            pos += 8;
                            // Angular velocity (omega)
                            angularVelocity = new Vector3(
                                Utils.UInt16ToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f),
                                Utils.UInt16ToFloat(block.ObjectData, pos + 4, -256.0f, 256.0f));
                            pos += 6;

                            break;
                        case 16:
                            // The data is an array of single bytes (8-bit numbers)

                            // Position
                            position = new Vector3(
                                Utils.ByteToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 1, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f));
                            pos += 3;
                            // Velocity
                            velocity = new Vector3(
                                Utils.ByteToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 1, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f));
                            pos += 3;
                            // Accleration
                            acceleration = new Vector3(
                                Utils.ByteToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 1, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f));
                            pos += 3;
                            // Rotation
                            rotation = new Quaternion(
                                Utils.ByteToFloat(block.ObjectData, pos, -1.0f, 1.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 1, -1.0f, 1.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 2, -1.0f, 1.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 3, -1.0f, 1.0f));
                            pos += 4;
                            // Angular Velocity
                            angularVelocity = new Vector3(
                                Utils.ByteToFloat(block.ObjectData, pos, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 1, -256.0f, 256.0f),
                                Utils.ByteToFloat(block.ObjectData, pos + 2, -256.0f, 256.0f));
                            pos += 3;

                            break;
                        default:
                            //Logger.Log("Got an ObjectUpdate block with ObjectUpdate field length of " +
                                //block.ObjectData.Length, Helpers.LogLevel.Warning, Client);

                            continue;
                    }
                    #endregion
                    mpos = position;
                    //Console.WriteLine("mpos = " + mpos.ToString());
                    mid = block.ID;
                }
            }
            return packet;
        }

        /// <summary>
        /// A terse object update, used when a transformation matrix or
        /// velocity/acceleration for an object changes but nothing else
        /// (scale/position/rotation/acceleration/velocity). (From OpenMetaverse, with edits)
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="simulator"></param>
        private Packet TerseUpdateHandler(Packet packet, IPEndPoint simulator)
        {
            ImprovedTerseObjectUpdatePacket terse = (ImprovedTerseObjectUpdatePacket)packet;
            //UpdateDilation(simulator, terse.RegionData.TimeDilation);

            for (int i = 0; i < terse.ObjectData.Length; i++)
            {
                ImprovedTerseObjectUpdatePacket.ObjectDataBlock block = terse.ObjectData[i];

                try
                {
                    int pos = 4;
                    uint localid = Utils.BytesToUInt(block.Data, 0);

                    // Check if we are interested in this update
                    //if (!Client.Settings.ALWAYS_DECODE_OBJECTS && localid != Client.Self.localID && OnObjectUpdated == null)
                        //continue;

                    #region Decode update data
                    
                    ObjectUpdate update = new ObjectUpdate();

                    // LocalID
                    update.LocalID = localid;
                    // State
                    update.State = block.Data[pos++];
                    // Avatar boolean
                    update.Avatar = (block.Data[pos++] != 0);
                    // Collision normal for avatar
                    if (update.Avatar)
                    {
                        update.CollisionPlane = new Vector4(block.Data, pos);
                        pos += 16;
                    }
                    // Position
                    update.Position = new Vector3(block.Data, pos);
                    pos += 12;
                    // Velocity
                    update.Velocity = new Vector3(
                        Utils.UInt16ToFloat(block.Data, pos, -128.0f, 128.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 2, -128.0f, 128.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 4, -128.0f, 128.0f));
                    pos += 6;
                    // Acceleration
                    update.Acceleration = new Vector3(
                        Utils.UInt16ToFloat(block.Data, pos, -64.0f, 64.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 2, -64.0f, 64.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 4, -64.0f, 64.0f));
                    pos += 6;
                    // Rotation (theta)
                    update.Rotation = new Quaternion(
                        Utils.UInt16ToFloat(block.Data, pos, -1.0f, 1.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 2, -1.0f, 1.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 4, -1.0f, 1.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 6, -1.0f, 1.0f));
                    pos += 8;
                    // Angular velocity
                    update.AngularVelocity = new Vector3(
                        Utils.UInt16ToFloat(block.Data, pos, -64.0f, 64.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 2, -64.0f, 64.0f),
                        Utils.UInt16ToFloat(block.Data, pos + 4, -64.0f, 64.0f));
                    pos += 6;

                    // Textures
                    // FIXME: Why are we ignoring the first four bytes here?
                    if (block.TextureEntry.Length != 0)
                        update.Textures = new Primitive.TextureEntry(block.TextureEntry, 4, block.TextureEntry.Length - 4);

                    #endregion Decode update data

                    //Primitive obj = (update.Avatar) ?
                        //(Primitive)GetAvatar(simulator, update.LocalID, UUID.Zero) :
                        //(Primitive)GetPrimitive(simulator, update.LocalID, UUID.Zero);

                    #region Update Client.Self
                    /*
                    if (update.LocalID == Client.Self.localID)
                    {
                        Client.Self.collisionPlane = update.CollisionPlane;
                        Client.Self.relativePosition = update.Position;
                        Client.Self.velocity = update.Velocity;
                        Client.Self.acceleration = update.Acceleration;
                        Client.Self.relativeRotation = update.Rotation;
                        Client.Self.angularVelocity = update.AngularVelocity;
                    }
                     * */
                    #endregion Update Client.Self
                    if (update.LocalID == mid) mpos = update.Position;
                    else ObjectWithVel(update.Position, update.Velocity);
                }
                catch
                {
                    //Logger.Log(e.Message, Helpers.LogLevel.Warning, Client, e);
                }
            }
            return packet;
        }

        // Interceptory finding code is mainly from ClientAO with some edits.
        /// <summary>
        /// Begins the process to find the interceptor from a path.
        /// </summary>
        /// <param name="path"></param>
        private void FindIntercept(string path)
        {
            //Load notecard from path
            //exemple: /ao Objects/My AOs/wetikon/config.txt
            // add a delegate to monitor inventory infos
            proxy.AddDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
            RequestFindObjectByPath(frame.InventoryRoot, path);
        }

        //Current inventory path search
        string[] searchPath;
        //Search level
        int searchLevel;
        //Current folder
        UUID currentFolder;
        // Number of directory descendents received
        int nbdescendantsreceived;
        //List of items in the current folder
        Dictionary<string, InventoryItem> currentFolderItems;

        /* Unused Variables for inventory search
        //Asset download request ID
        UUID assetdownloadID;
        //Downloaded bytes so far
        int downloadedbytes;
        //size of download
        int downloadsize;
        //data buffer
        byte[] buffer; */

        private PacketDelegate _inventoryPacketDelegate;
        private PacketDelegate inventoryPacketDelegate
        {
            get
            {
                if (_inventoryPacketDelegate == null)
                {
                    _inventoryPacketDelegate = new PacketDelegate(InventoryDescendentsHandler);
                }
                return _inventoryPacketDelegate;
            }
        }

        #region Inventory Searchers
        /// <summary>
        /// Start a request to find an item by its inventory path.
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="path"></param>
        public void RequestFindObjectByPath(UUID baseFolder, string path)
        {
            if (path == null || path.Length == 0)
                return;// throw new ArgumentException("Empty path is not supported");
            currentFolder = baseFolder;
            //split path by '/'
            searchPath = path.Split('/');
            //search for first element in the path
            searchLevel = 0;

            // Start the search
            RequestFolderContents(baseFolder,
                true,
                (searchPath.Length == 1) ? true : false,
                InventorySortOrder.ByName);
        }

        /// <summary>
        /// Request a folder's content.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="folders"></param>
        /// <param name="items"></param>
        /// <param name="order"></param>
        public void RequestFolderContents(UUID folder, bool folders, bool items, InventorySortOrder order)
        {
            //empty the dictionnary containing current folder items by name
            currentFolderItems = new Dictionary<string, InventoryItem>();
            //reset the number of descendants received
            nbdescendantsreceived = 0;
            //build a packet to request the content
            FetchInventoryDescendentsPacket fetch = new FetchInventoryDescendentsPacket();
            fetch.AgentData.AgentID = frame.AgentID;
            fetch.AgentData.SessionID = frame.SessionID;

            fetch.InventoryData.FetchFolders = folders;
            fetch.InventoryData.FetchItems = items;
            fetch.InventoryData.FolderID = folder;
            fetch.InventoryData.OwnerID = frame.AgentID; //is it correct?
            fetch.InventoryData.SortOrder = (int)order;

            //send packet to SL
            proxy.InjectPacket(fetch, Direction.Outgoing);
        }

        /// <summary>
        /// Creates a new InventoryItem for a type.
        /// </summary>
        /// <param name="type">InventoryType of the item.</param>
        /// <param name="id">UUID of the item.</param>
        /// <returns></returns>
        public static InventoryItem CreateInventoryItem(InventoryType type, UUID id)
        {
            switch (type)
            {
                case InventoryType.Texture: return new InventoryTexture(id);
                case InventoryType.Sound: return new InventorySound(id);
                case InventoryType.CallingCard: return new InventoryCallingCard(id);
                case InventoryType.Landmark: return new InventoryLandmark(id);
                case InventoryType.Object: return new InventoryObject(id);
                case InventoryType.Notecard: return new InventoryNotecard(id);
                case InventoryType.Category: return new InventoryCategory(id);
                case InventoryType.LSL: return new InventoryLSL(id);
                case InventoryType.Snapshot: return new InventorySnapshot(id);
                case InventoryType.Attachment: return new InventoryAttachment(id);
                case InventoryType.Wearable: return new InventoryWearable(id);
                case InventoryType.Animation: return new InventoryAnimation(id);
                case InventoryType.Gesture: return new InventoryGesture(id);
                default: return new InventoryItem(type, id);
            }
        }

        /// <summary>
        /// Process the Inventory reply from SL.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        private Packet InventoryDescendentsHandler(Packet packet, IPEndPoint sim)
        {
            bool intercept = false;
            InventoryDescendentsPacket reply = (InventoryDescendentsPacket)packet;

            if (reply.AgentData.Descendents > 0
                && reply.AgentData.FolderID == currentFolder)
            {
                //SayToUser("nb descendents: " + reply.AgentData.Descendents);
                //this packet concerns the folder we asked for            
                if (reply.FolderData[0].FolderID != UUID.Zero
                    && searchLevel < searchPath.Length - 1)
                {
                    nbdescendantsreceived += reply.FolderData.Length;
                    //SayToUser("nb received: " + nbdescendantsreceived);
                    //folders are present, and we are not at end of path.
                    //look at them
                    for (int i = 0; i < reply.FolderData.Length; i++)
                    {
                        //SayToUser("Folder: " + Utils.BytesToString(reply.FolderData[i].Name));
                        if (searchPath[searchLevel] == Utils.BytesToString(reply.FolderData[i].Name))
                        {
                            //We found the next folder in the path                        
                            currentFolder = reply.FolderData[i].FolderID;
                            if (searchLevel < searchPath.Length - 1)
                            {
                                // ask for next item in path
                                searchLevel++;
                                RequestFolderContents(currentFolder,
                                    true,
                                    (searchLevel < searchPath.Length - 1) ? false : true,
                                    InventorySortOrder.ByName);
                                //Jump to end
                                goto End;
                            }
                        }
                    }
                    if (nbdescendantsreceived >= reply.AgentData.Descendents)
                    {
                        //We have not found the folder. The user probably mistyped it
                        Console.WriteLine("Didn't find folder " + searchPath[searchLevel] + ". Check spelling.");
                        //Stop looking at packets
                        proxy.RemoveDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
                    }
                }
                else if (searchLevel < searchPath.Length - 1)
                {
                    //There are no folders in the packet ; but we are looking for one!
                    //We have not found the folder. The user probably mistyped it
                    Console.WriteLine("Didn't find folder " + searchPath[searchLevel] + ". Check spelling.");
                    //Stop looking at packets
                    proxy.RemoveDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
                }
                else
                {
                    //There are folders in the packet. And we are at the end of 
                    //the path, count their number in nbdescendantsreceived
                    nbdescendantsreceived += reply.FolderData.Length;
                    //SayToUser("nb received: " + nbdescendantsreceived);                
                }
                if (reply.ItemData[0].ItemID != UUID.Zero
                    && searchLevel == searchPath.Length - 1)
                {
                    //there are items returned and we are looking for one 
                    //(end of search path)                
                    //count them
                    nbdescendantsreceived += reply.ItemData.Length;
                    //SayToUser("nb received: " + nbdescendantsreceived);
                    for (int i = 0; i < reply.ItemData.Length; i++)
                    {
                        //we are going to store info on all items. we'll need
                        //it to get the asset ID of animations refered to by the
                        //configuration notecard
                        if (reply.ItemData[i].ItemID != UUID.Zero)
                        {
                            InventoryItem item = CreateInventoryItem((InventoryType)reply.ItemData[i].InvType, reply.ItemData[i].ItemID);
                            item.ParentUUID = reply.ItemData[i].FolderID;
                            item.CreatorID = reply.ItemData[i].CreatorID;
                            item.AssetType = (AssetType)reply.ItemData[i].Type;
                            item.AssetUUID = reply.ItemData[i].ItemID;//reply.ItemData[i].AssetID;
                            item.CreationDate = Utils.UnixTimeToDateTime((uint)reply.ItemData[i].CreationDate);
                            item.Description = Utils.BytesToString(reply.ItemData[i].Description);
                            item.Flags = (uint)reply.ItemData[i].Flags;
                            item.Name = Utils.BytesToString(reply.ItemData[i].Name);
                            item.GroupID = reply.ItemData[i].GroupID;
                            item.GroupOwned = reply.ItemData[i].GroupOwned;
                            item.Permissions = new Permissions(
                                reply.ItemData[i].BaseMask,
                                reply.ItemData[i].EveryoneMask,
                                reply.ItemData[i].GroupMask,
                                reply.ItemData[i].NextOwnerMask,
                                reply.ItemData[i].OwnerMask);
                            item.SalePrice = reply.ItemData[i].SalePrice;
                            item.SaleType = (SaleType)reply.ItemData[i].SaleType;
                            item.OwnerID = reply.AgentData.OwnerID;

                            Console.WriteLine("item in folder: " + item.Name + "/" + item.AssetUUID.ToString());

                            //Add the item to the name -> item hash
                            currentFolderItems.Add(item.Name, item);
                        }
                    }
                    if (nbdescendantsreceived >= reply.AgentData.Descendents)
                    {
                        //We have received all the items in the last folder
                        //Let's look for the item we are looking for
                        if (currentFolderItems.ContainsKey(searchPath[searchLevel]) == true)
                        {
                            //We found what we where looking for
                            //Stop looking at packets
                            proxy.RemoveDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
                            //Download the notecard
                            //assetdownloadID = RequestInventoryAsset(currentFolderItems[searchPath[searchLevel]]);
                            InventoryItem temp;
                            //temp.Description = "idontexist";
                            if (currentFolderItems.TryGetValue(searchPath[searchLevel], out temp) == true)
                            {
                                interceptor = temp;
                                Console.WriteLine("SUCCESS. We found the interceptor. Full value is: " + interceptor.Name + "/" + 
                                            interceptor.AssetUUID.ToString() + "/" + temp.AssetUUID.ToString());
                                
                                PseudoLLRezObject(interceptor,mpos);
                            }
                            else Console.WriteLine("Something went wrong with TryGetValue.");
                        }
                        else
                        {
                            //We didnt find the item, the user probably mistyped its name
                            //SayToUser("Didn't find object " + searchPath[searchLevel] + ". Check the spelling." );
                            //TODO: keep looking for a moment, or else reply packets may still
                            //come in case of a very large inventory folder
                            //Stop looking at packets
                            proxy.RemoveDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
                        }
                    }
                }
                else if (searchLevel == searchPath.Length - 1 && nbdescendantsreceived >= reply.AgentData.Descendents)
                {
                    //There are no items in the packet, but we are looking for one!
                    //We didnt find the item, the user probably mistyped its name
                    //SayToUser("Didn't find object " + searchPath[searchLevel] + ". It is also possible that the folder is" +
                                //" very large." );
                    //TODO: keep looking for a moment, or else reply packets may still
                    //come in case of a very large inventory folder
                    //Stop looking at packets
                    proxy.RemoveDelegate(PacketType.InventoryDescendents, Direction.Incoming, this.inventoryPacketDelegate);
                }
                //Intercept the packet, it was a reply to our request. No need
                //to confuse the actual SL client
                intercept = true;
            }
            End:
            if (intercept)
            {
                //stop packet
                return packet;
            }
            else
            {
                //let packet go to client
                return packet;
            }
        }
        #endregion InventorySearchers

        
    }
}