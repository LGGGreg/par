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
    public class InvFunPlugin : GTabPlug
    {
        public PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private InvFunForm1 form;
        private PubComb.Aux_SharedInfo shared;
        private Inventory _Store = new Inventory();
        private List<InventorySearch> _Searches = new List<InventorySearch>();
        protected struct InventorySearch
        {
            public UUID Folder;
            public UUID Owner;
            public string[] Path;
            public int Level;
        }

        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Inv Fun");
        }
        public InvFunPlugin(PubComb plug)
        {

            plugin = plug;
            shared = plug.SharedInfo;
            form = new InvFunForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;

        }
        public void RequestFolderContents(UUID folder, UUID owner, bool folders, bool items,
            InventorySortOrder order)
        {
            FetchInventoryDescendentsPacket fetch = new FetchInventoryDescendentsPacket();
            fetch.AgentData.AgentID = frame.AgentID;
            fetch.AgentData.SessionID = frame.SessionID;

            fetch.InventoryData.FetchFolders = folders;
            fetch.InventoryData.FetchItems = items;
            fetch.InventoryData.FolderID = folder;
            fetch.InventoryData.OwnerID = owner;
            fetch.InventoryData.SortOrder = (int)order;

            proxy.InjectPacket(fetch, Direction.Outgoing);
        }
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

        private Packet InventoryDescendentsHandler(Packet packet, IPEndPoint simulator)
        {
            InventoryDescendentsPacket reply = (InventoryDescendentsPacket)packet;

            if (reply.AgentData.Descendents > 0)
            {
                // InventoryDescendantsReply sends a null folder if the parent doesnt contain any folders
                if (reply.FolderData[0].FolderID != UUID.Zero)
                {
                    // Iterate folders in this packet
                    for (int i = 0; i < reply.FolderData.Length; i++)
                    {
                        InventoryFolder folder = new InventoryFolder(reply.FolderData[i].FolderID);
                        folder.ParentUUID = reply.FolderData[i].ParentID;
                        folder.Name = Utils.BytesToString(reply.FolderData[i].Name);
                        folder.PreferredType = (AssetType)reply.FolderData[i].Type;
                        folder.OwnerID = reply.AgentData.OwnerID;

                        _Store[folder.UUID] = folder;
                    }
                }

                // InventoryDescendantsReply sends a null item if the parent doesnt contain any items.
                if (reply.ItemData[0].ItemID != UUID.Zero)
                {
                    // Iterate items in this packet
                    for (int i = 0; i < reply.ItemData.Length; i++)
                    {
                        if (reply.ItemData[i].ItemID != UUID.Zero)
                        {
                            InventoryItem item;
                            /* 
                             * Objects that have been attached in-world prior to being stored on the 
                             * asset server are stored with the InventoryType of 0 (Texture) 
                             * instead of 17 (Attachment) 
                             * 
                             * This corrects that behavior by forcing Object Asset types that have an 
                             * invalid InventoryType with the proper InventoryType of Attachment.
                             */
                            if ((AssetType)reply.ItemData[i].Type == AssetType.Object
                                && (InventoryType)reply.ItemData[i].InvType == InventoryType.Texture)
                            {
                                item = CreateInventoryItem(InventoryType.Attachment, reply.ItemData[i].ItemID);
                                item.InventoryType = InventoryType.Attachment;
                            }
                            else
                            {
                                item = CreateInventoryItem((InventoryType)reply.ItemData[i].InvType, reply.ItemData[i].ItemID);
                                item.InventoryType = (InventoryType)reply.ItemData[i].InvType;
                            }

                            item.ParentUUID = reply.ItemData[i].FolderID;
                            item.CreatorID = reply.ItemData[i].CreatorID;
                            item.AssetType = (AssetType)reply.ItemData[i].Type;
                            item.AssetUUID = reply.ItemData[i].AssetID;
                            item.CreationDate = Utils.UnixTimeToDateTime((uint)reply.ItemData[i].CreationDate);
                            item.Description = Utils.BytesToString(reply.ItemData[i].Description);
                            item.Flags = reply.ItemData[i].Flags;
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

                            _Store[item.UUID] = item;
                        }
                    }
                }
            }

            InventoryFolder parentFolder = null;

            if (_Store.Contains(reply.AgentData.FolderID) &&
                _Store[reply.AgentData.FolderID] is InventoryFolder)
            {
                parentFolder = _Store[reply.AgentData.FolderID] as InventoryFolder;
            }
            else
            {
                Console.WriteLine("Don't have a reference to FolderID " + reply.AgentData.FolderID.ToString() +
                    " or it is not a folder");
                return packet;
            }

            if (reply.AgentData.Version < parentFolder.Version)
            {
                Console.WriteLine("Got an outdated InventoryDescendents packet for folder " + parentFolder.Name +
                    ", this version = " + reply.AgentData.Version + ", latest version = " + parentFolder.Version);
                return packet;
            }

            parentFolder.Version = reply.AgentData.Version;
            // FIXME: reply.AgentData.Descendants is not parentFolder.DescendentCount if we didn't 
            // request items and folders
            parentFolder.DescendentCount = reply.AgentData.Descendents;

            #region FindObjectsByPath Handling

            if (_Searches.Count > 0)
            {
                lock (_Searches)
                {
                StartSearch:

                    // Iterate over all of the outstanding searches
                    for (int i = 0; i < _Searches.Count; i++)
                    {
                        InventorySearch search = _Searches[i];
                        List<InventoryBase> folderContents = _Store.GetContents(search.Folder);

                        // Iterate over all of the inventory objects in the base search folder
                        for (int j = 0; j < folderContents.Count; j++)
                        {
                            // Check if this inventory object matches the current path node
                            if (folderContents[j].Name == search.Path[search.Level])
                            {
                                if (search.Level == search.Path.Length - 1)
                                {
                                    Console.WriteLine("Finished path search of " + String.Join("/", search.Path));



                                    // Remove this entry and restart the loop since we are changing the collection size
                                    _Searches.RemoveAt(i);
                                    goto StartSearch;
                                }
                                else
                                {
                                    // We found a match but it is not the end of the path, request the next level
                                    Console.WriteLine(String.Format("Matched level {0}/{1} in a path search of {2}",
                                        search.Level, search.Path.Length - 1, String.Join("/", search.Path)));

                                    search.Folder = folderContents[j].UUID;
                                    search.Level++;
                                    _Searches[i] = search;

                                    RequestFolderContents(search.Folder, search.Owner, true, true,
                                        InventorySortOrder.ByName);
                                }
                            }
                        }
                    }
                }
            }

            #endregion FindObjectsByPath Handling
            //OnFolderUpdated(parentFolder.UUID);
            form.GregTreeInv.UpdateFolder(parentFolder.UUID);
            return packet;
        }

    }


}

/// <summary>
/// Responsible for maintaining inventory structure. Inventory constructs nodes
/// and manages node children as is necessary to maintain a coherant hirarchy.
/// Other classes should not manipulate or create InventoryNodes explicitly. When
/// A node's parent changes (when a folder is moved, for example) simply pass
/// Inventory the updated InventoryFolder and it will make the appropriate changes
/// to its internal representation.
/// </summary>
public class Inventory
{
    

    /// <summary>
    /// The root folder of this avatars inventory
    /// </summary>
    public InventoryFolder RootFolder
    {
        get { return RootNode.Data as InventoryFolder; }
        set
        {
            UpdateNodeFor(value);
            _RootNode = Items[value.UUID];
        }
    }

    /// <summary>
    /// The default shared library folder
    /// </summary>
    public InventoryFolder LibraryFolder
    {
        get { return LibraryRootNode.Data as InventoryFolder; }
        set
        {
            UpdateNodeFor(value);
            _LibraryRootNode = Items[value.UUID];
        }
    }

    private InventoryNode _LibraryRootNode;
    private InventoryNode _RootNode;

    /// <summary>
    /// The root node of the avatars inventory
    /// </summary>
    public InventoryNode RootNode
    {
        get
        {
            if (_RootNode == null)
                throw new InventoryException("Root node unknown. Are you completely logged in?");
            return _RootNode;
        }
    }

    /// <summary>
    /// The root node of the default shared library
    /// </summary>
    public InventoryNode LibraryRootNode
    {
        get
        {
            if (_LibraryRootNode == null)
                throw new InventoryException("Library Root node unknown. Are you completely logged in?");
            return _LibraryRootNode;
        }
    }


    //private InventoryManager Manager;
    private Dictionary<UUID, InventoryNode> Items = new Dictionary<UUID, InventoryNode>();

    public Inventory()
    {

        //Manager = manager;
        Items = new Dictionary<UUID, InventoryNode>();
    }

    public List<InventoryBase> GetContents(InventoryFolder folder)
    {
        return GetContents(folder.UUID);
    }

    /// <summary>
    /// Returns the contents of the specified folder
    /// </summary>
    /// <param name="folder">A folder's UUID</param>
    /// <returns>The contents of the folder corresponding to <code>folder</code></returns>
    /// <exception cref="InventoryException">When <code>folder</code> does not exist in the inventory</exception>
    public List<InventoryBase> GetContents(UUID folder)
    {
        InventoryNode folderNode;
        if (!Items.TryGetValue(folder, out folderNode))
            throw new InventoryException("Unknown folder: " + folder);
        lock (folderNode.Nodes.SyncRoot)
        {
            List<InventoryBase> contents = new List<InventoryBase>(folderNode.Nodes.Count);
            foreach (InventoryNode node in folderNode.Nodes.Values)
            {
                contents.Add(node.Data);
            }
            return contents;
        }
    }

    /// <summary>
    /// Updates the state of the InventoryNode and inventory data structure that
    /// is responsible for the InventoryObject. If the item was previously not added to inventory,
    /// it adds the item, and updates structure accordingly. If it was, it updates the 
    /// InventoryNode, changing the parent node if <code>item.parentUUID</code> does 
    /// not match <code>node.Parent.Data.UUID</code>.
    /// 
    /// You can not set the inventory root folder using this method
    /// </summary>
    /// <param name="item">The InventoryObject to store</param>
    public void UpdateNodeFor(InventoryBase item)
    {
        lock (Items)
        {
            InventoryNode itemParent = null;
            if (item.ParentUUID != UUID.Zero && !Items.TryGetValue(item.ParentUUID, out itemParent))
            {
                // OK, we have no data on the parent, let's create a fake one.
                InventoryFolder fakeParent = new InventoryFolder(item.ParentUUID);
                fakeParent.DescendentCount = 1; // Dear god, please forgive me.
                itemParent = new InventoryNode(fakeParent);
                Items[item.ParentUUID] = itemParent;
                // Unfortunately, this breaks the nice unified tree
                // while we're waiting for the parent's data to come in.
                // As soon as we get the parent, the tree repairs itself.
                Console.WriteLine("Attempting to update inventory child of " +
                    item.ParentUUID.ToString() + " when we have no local reference to that folder");

                if (true)
                {
                    // Fetch the parent
                    List<UUID> fetchreq = new List<UUID>(1);
                    fetchreq.Add(item.ParentUUID);
                    //Manager.FetchInventory(fetchreq); // we cant fetch folder data! :-O
                }
            }

            InventoryNode itemNode;
            if (Items.TryGetValue(item.UUID, out itemNode)) // We're updating.
            {
                InventoryNode oldParent = itemNode.Parent;
                // Handle parent change
                if (oldParent == null || itemParent == null || itemParent.Data.UUID != oldParent.Data.UUID)
                {
                    if (oldParent != null)
                    {
                        lock (oldParent.Nodes.SyncRoot)
                            oldParent.Nodes.Remove(item.UUID);
                    }
                    if (itemParent != null)
                    {
                        lock (itemParent.Nodes.SyncRoot)
                            itemParent.Nodes[item.UUID] = itemNode;
                    }
                }

                itemNode.Parent = itemParent;



                itemNode.Data = item;
            }
            else // We're adding.
            {
                itemNode = new InventoryNode(item, itemParent);
                Items.Add(item.UUID, itemNode);
            }
        }
    }

    public InventoryNode GetNodeFor(UUID uuid)
    {
        return Items[uuid];
    }

    /// <summary>
    /// Removes the InventoryObject and all related node data from Inventory.
    /// </summary>
    /// <param name="item">The InventoryObject to remove.</param>
    public void RemoveNodeFor(InventoryBase item)
    {
        lock (Items)
        {
            InventoryNode node;
            if (Items.TryGetValue(item.UUID, out node))
            {
                if (node.Parent != null)
                    lock (node.Parent.Nodes.SyncRoot)
                        node.Parent.Nodes.Remove(item.UUID);
                Items.Remove(item.UUID);
            }

            // In case there's a new parent:
            InventoryNode newParent;
            if (Items.TryGetValue(item.ParentUUID, out newParent))
            {
                lock (newParent.Nodes.SyncRoot)
                    newParent.Nodes.Remove(item.UUID);
            }
        }
    }

    /// <summary>
    /// Used to find out if Inventory contains the InventoryObject
    /// specified by <code>uuid</code>.
    /// </summary>
    /// <param name="uuid">The UUID to check.</param>
    /// <returns>true if inventory contains uuid, false otherwise</returns>
    public bool Contains(UUID uuid)
    {
        return Items.ContainsKey(uuid);
    }

    public bool Contains(InventoryBase obj)
    {
        return Contains(obj.UUID);
    }

    #region Operators

    /// <summary>
    /// By using the bracket operator on this class, the program can get the 
    /// InventoryObject designated by the specified uuid. If the value for the corresponding
    /// UUID is null, the call is equivelant to a call to <code>RemoveNodeFor(this[uuid])</code>.
    /// If the value is non-null, it is equivelant to a call to <code>UpdateNodeFor(value)</code>,
    /// the uuid parameter is ignored.
    /// </summary>
    /// <param name="uuid">The UUID of the InventoryObject to get or set, ignored if set to non-null value.</param>
    /// <returns>The InventoryObject corresponding to <code>uuid</code>.</returns>
    public InventoryBase this[UUID uuid]
    {
        get
        {
            InventoryNode node = Items[uuid];
            return node.Data;
        }
        set
        {
            if (value != null)
            {
                // Log a warning if there is a UUID mismatch, this will cause problems
                //if (value.UUID != uuid)
                Console.WriteLine("Inventory[uuid]: uuid " + uuid.ToString() + " is not equal to value.UUID " +
                    value.UUID.ToString());

                UpdateNodeFor(value);
            }
            else
            {
                InventoryNode node;
                if (Items.TryGetValue(uuid, out node))
                {
                    RemoveNodeFor(node.Data);
                }
            }
        }
    }

    #endregion Operators


}
