/*
 * Copyright (c) 2007-2008, openmetaverse.org
 * All rights reserved.
 * 
 * BRAZENLY STOLEN AND MODIFIED BY ROB "N3X15" NELSON TO ALLOW FOR
 * TRACKING OBJECTS OVER A PROXIED CONNECTION
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.org nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Net;
using System.Collections.Generic;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
namespace PubComb
{
	/// <summary>
	/// For tracking objects via proxy.
	/// </summary>
	public class NexObjectTracker
	{
		ProxyFrame frame;
		public bool OBJECT_TRACKING=true;
		public bool AVATAR_TRACKING=true;
		public bool ALWAYS_REQUEST_OBJECTS=true;
		
		// Same system used in LibOMV's simulator class.
		public PCInternalDictionary<uint, Primitive> ObjectsPrimitives = new PCInternalDictionary<uint, Primitive>();
		public PCInternalDictionary<uint, Avatar> ObjectsAvatars = new PCInternalDictionary<uint, Avatar>();
		
		public NexObjectTracker(ProxyFrame frame)
		{
			this.frame=frame;
			RegisterMyCallbacks();
		}
		
		protected void RegisterMyCallbacks()
        {
			frame.proxy.AddDelegate(PacketType.ObjectUpdate, 				Direction.Incoming,	new PacketDelegate(UpdateHandler));
            frame.proxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, 	Direction.Incoming,	new PacketDelegate(TerseUpdateHandler));
            frame.proxy.AddDelegate(PacketType.ObjectUpdateCompressed,		Direction.Incoming, new PacketDelegate(CompressedUpdateHandler));
            frame.proxy.AddDelegate(PacketType.ObjectUpdateCached, 			Direction.Incoming,	new PacketDelegate(CachedUpdateHandler));
            frame.proxy.AddDelegate(PacketType.KillObject,					Direction.Incoming,	new PacketDelegate(KillObjectHandler));
            frame.proxy.AddDelegate(PacketType.ObjectPropertiesFamily,		Direction.Incoming,	new PacketDelegate(ObjectPropertiesFamilyHandler));
            frame.proxy.AddDelegate(PacketType.ObjectProperties, 			Direction.Incoming,	new PacketDelegate(ObjectPropertiesHandler));

            // If the callbacks aren't registered there's not point in doing client-side path prediction,
            // so we set it up here
            //InterpolationTimer = new Timer(new TimerCallback(InterpolationTimer_Elapsed), null, Settings.INTERPOLATION_INTERVAL,
            //    Settings.INTERPOLATION_INTERVAL);
        }
		
        #region Action Methods

        /// <summary>
        /// Request object information from the sim, primarily used for stale 
        /// or missing cache entries
        /// </summary>
        /// <param name="simulator">The simulator containing the object you're 
        /// looking for</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        public void RequestObject(uint localID)
        {
            RequestMultipleObjectsPacket request = new RequestMultipleObjectsPacket();
            request.AgentData.AgentID = frame.AgentID;
            request.AgentData.SessionID = frame.SessionID;
            request.ObjectData = new RequestMultipleObjectsPacket.ObjectDataBlock[1];
            request.ObjectData[0] = new RequestMultipleObjectsPacket.ObjectDataBlock();
            request.ObjectData[0].ID = localID;
            request.ObjectData[0].CacheMissType = 0;

            frame.proxy.InjectPacket(request, Direction.Outgoing);
        }

        /// <summary>
        /// Request object information for multiple objects all contained in
        /// the same sim, primarily used for stale or missing cache entries
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to request</param>
        public void RequestObjects(List<uint> localIDs)
        {
            int i = 0;

            RequestMultipleObjectsPacket request = new RequestMultipleObjectsPacket();
            request.AgentData.AgentID = frame.AgentID;
            request.AgentData.SessionID = frame.SessionID;
            request.ObjectData = new RequestMultipleObjectsPacket.ObjectDataBlock[localIDs.Count];

            foreach (uint localID in localIDs)
            {
                request.ObjectData[i] = new RequestMultipleObjectsPacket.ObjectDataBlock();
                request.ObjectData[i].ID = localID;
                request.ObjectData[i].CacheMissType = 0;

                i++;
            }

            frame.proxy.InjectPacket(request, Direction.Outgoing);
        }

        /// <summary>
        /// Attempt to purchase an original object, a copy, or the contents of
        /// an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>        
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="saleType">Whether the original, a copy, or the object
        /// contents are on sale. This is used for verification, if the this
        /// sale type is not valid for the object the purchase will fail</param>
        /// <param name="price">Price of the object. This is used for 
        /// verification, if it does not match the actual price the purchase
        /// will fail</param>
        /// <param name="groupID">Group ID that will be associated with the new
        /// purchase</param>
        /// <param name="categoryID">Inventory folder UUID where the object or objects 
        /// purchased should be placed</param>
        /// <example>
        /// <code>
        /// BuyObject(Client.Network.CurrentSim, 500, SaleType.Copy, 
        /// 100, UUID.Zero, Client.Self.InventoryRootFolderUUID);
        /// </code> 
        ///</example>
        public void BuyObject(uint localID, SaleType saleType, int price, UUID groupID,
            UUID categoryID)
        {
            ObjectBuyPacket buy = new ObjectBuyPacket();

            buy.AgentData.AgentID = frame.AgentID;
            buy.AgentData.SessionID = frame.SessionID;
            buy.AgentData.GroupID = groupID;
            buy.AgentData.CategoryID = categoryID;

            buy.ObjectData = new ObjectBuyPacket.ObjectDataBlock[1];
            buy.ObjectData[0] = new ObjectBuyPacket.ObjectDataBlock();
            buy.ObjectData[0].ObjectLocalID = localID;
            buy.ObjectData[0].SaleType = (byte)saleType;
            buy.ObjectData[0].SalePrice = price;

            frame.proxy.InjectPacket(buy, Direction.Outgoing);
        }

        /// <summary>
        /// Select a single object. This will trigger the simulator to send us back 
        /// an ObjectProperties packet so we can get the full information for
        /// this object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="automaticDeselect">Should objects be deselected immediately after selection</param>
        public void SelectObject(uint localID, bool automaticDeselect)
        {
            ObjectSelectPacket select = new ObjectSelectPacket();

            select.AgentData.AgentID = frame.AgentID;
            select.AgentData.SessionID = frame.SessionID;

            select.ObjectData = new ObjectSelectPacket.ObjectDataBlock[1];
            select.ObjectData[0] = new ObjectSelectPacket.ObjectDataBlock();
            select.ObjectData[0].ObjectLocalID = localID;

            frame.proxy.InjectPacket(select, Direction.Outgoing);
            if (automaticDeselect)
            {
                DeselectObject(localID);
            }
        }

        /// <summary>
        /// Select a single object. This will trigger the simulator to send us back 
        /// an ObjectProperties packet so we can get the full information for
        /// this object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        public void SelectObject(uint localID)
        {
            SelectObject(localID, true);
        }

        /// <summary>
        /// Select multiple objects. This will trigger the simulator to send us
        /// back ObjectProperties for each object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to select</param>
        /// <param name="automaticDeselect">Should objects be deselected immediately after selection</param>
        public void SelectObjects(uint[] localIDs, bool automaticDeselect)
        {
            ObjectSelectPacket select = new ObjectSelectPacket();

            select.AgentData.AgentID = frame.AgentID;
            select.AgentData.SessionID = frame.SessionID;

            select.ObjectData = new ObjectSelectPacket.ObjectDataBlock[localIDs.Length];

            for (int i = 0; i < localIDs.Length; i++)
            {
                select.ObjectData[i] = new ObjectSelectPacket.ObjectDataBlock();
                select.ObjectData[i].ObjectLocalID = localIDs[i];
            }

            frame.proxy.InjectPacket(select, Direction.Outgoing);
            if (automaticDeselect)
            {
                DeselectObjects(localIDs);
            }
        }

        /// <summary>
        /// Select multiple objects. This will trigger the simulator to send us
        /// back ObjectProperties for each object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to select</param>
        public void SelectObjects(uint[] localIDs)
        {
            SelectObjects(localIDs, true);
        }


        /// <summary>
        /// Sets and object's flags (physical, temporary, phantom, casts shadow)
        /// </summary>
        /// <param name="localID"></param>
        /// <param name="physical"></param>
        /// <param name="temporary"></param>
        /// <param name="phantom"></param>
        /// <param name="castsShadow"></param>
        public void SetFlags(uint localID, bool physical, bool temporary, bool phantom, bool castsShadow)
        {
            ObjectFlagUpdatePacket flags = new ObjectFlagUpdatePacket();
            flags.AgentData.AgentID = frame.AgentID;
            flags.AgentData.SessionID = frame.SessionID;
            flags.AgentData.ObjectLocalID = localID;
            flags.AgentData.UsePhysics = physical;
            flags.AgentData.IsTemporary = temporary;
            flags.AgentData.IsPhantom = phantom;
            flags.AgentData.CastsShadows = castsShadow;

            frame.proxy.InjectPacket(flags,Direction.Outgoing);
        }

        /// <summary>
        /// Sets an object's sale information
        /// </summary>
        /// <param name="localID"></param>
        /// <param name="saleType"></param>
        /// <param name="price"></param>
        public void SetSaleInfo(uint localID, SaleType saleType, int price)
        {
            ObjectSaleInfoPacket sale = new ObjectSaleInfoPacket();
            sale.AgentData.AgentID = frame.AgentID;
            sale.AgentData.SessionID = frame.SessionID;
            sale.ObjectData = new ObjectSaleInfoPacket.ObjectDataBlock[1];
            sale.ObjectData[0] = new ObjectSaleInfoPacket.ObjectDataBlock();
            sale.ObjectData[0].LocalID = localID;
            sale.ObjectData[0].SalePrice = price;
            sale.ObjectData[0].SaleType = (byte)saleType;

            frame.proxy.InjectPacket(sale,Direction.Outgoing);
        }

        /// <summary>
        /// Sets sale info for multiple objects
        /// </summary>
        /// <param name="localIDs"></param>
        /// <param name="saleType"></param>
        /// <param name="price"></param>
        public void SetSaleInfo(List<uint> localIDs, SaleType saleType, int price)
        {
            ObjectSaleInfoPacket sale = new ObjectSaleInfoPacket();
            sale.AgentData.AgentID = frame.AgentID;
            sale.AgentData.SessionID = frame.SessionID;
            sale.ObjectData = new ObjectSaleInfoPacket.ObjectDataBlock[localIDs.Count];
            for (int i = 0; i < localIDs.Count; i++)
            {
                sale.ObjectData[i] = new ObjectSaleInfoPacket.ObjectDataBlock();
                sale.ObjectData[i].LocalID = localIDs[i];
                sale.ObjectData[i].SalePrice = price;
                sale.ObjectData[i].SaleType = (byte)saleType;
            }

            frame.proxy.InjectPacket(sale,Direction.Outgoing);
        }

        /// <summary>
        /// Deselect an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        public void DeselectObject(uint localID)
        {
            ObjectDeselectPacket deselect = new ObjectDeselectPacket();

            deselect.AgentData.AgentID = frame.AgentID;
            deselect.AgentData.SessionID = frame.SessionID;

            deselect.ObjectData = new ObjectDeselectPacket.ObjectDataBlock[1];
            deselect.ObjectData[0] = new ObjectDeselectPacket.ObjectDataBlock();
            deselect.ObjectData[0].ObjectLocalID = localID;

            frame.proxy.InjectPacket(deselect,Direction.Outgoing);
        }

        /// <summary>
        /// Deselect multiple objects.
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="libsecondlife.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to select</param>
        public void DeselectObjects(uint[] localIDs)
        {
            ObjectDeselectPacket deselect = new ObjectDeselectPacket();

            deselect.AgentData.AgentID = frame.AgentID;
            deselect.AgentData.SessionID = frame.SessionID;

            deselect.ObjectData = new ObjectDeselectPacket.ObjectDataBlock[localIDs.Length];

            for (int i = 0; i < localIDs.Length; i++)
            {
                deselect.ObjectData[i] = new ObjectDeselectPacket.ObjectDataBlock();
                deselect.ObjectData[i].ObjectLocalID = localIDs[i];
            }

            frame.proxy.InjectPacket(deselect,Direction.Outgoing);
        }

        /// <summary>
        /// Perform a click action on an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        public void ClickObject(uint localID)
        {
            ObjectGrabPacket grab = new ObjectGrabPacket();
            grab.AgentData.AgentID = frame.AgentID;
            grab.AgentData.SessionID = frame.SessionID;
            grab.ObjectData.GrabOffset = Vector3.Zero;
            grab.ObjectData.LocalID = localID;

            frame.proxy.InjectPacket(grab,Direction.Outgoing);

            // TODO: If these hit the server out of order the click will fail 
            // and we'll be grabbing the object

            ObjectDeGrabPacket degrab = new ObjectDeGrabPacket();
            degrab.AgentData.AgentID = frame.AgentID;
            degrab.AgentData.SessionID = frame.SessionID;
            degrab.ObjectData.LocalID = localID;

            frame.proxy.InjectPacket(degrab,Direction.Outgoing);
        }

        /// <summary>
        /// Create, or "rez" a new prim object in a simulator
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object to place the object in</param>
        /// <param name="prim">Data describing the prim object to rez</param>
        /// <param name="groupID">Group ID that this prim will be set to, or UUID.Zero if you
        /// do not want the object to be associated with a specific group</param>
        /// <param name="position">An approximation of the position at which to rez the prim</param>
        /// <param name="scale">Scale vector to size this prim</param>
        /// <param name="rotation">Rotation quaternion to rotate this prim</param>
        /// <remarks>Due to the way client prim rezzing is done on the server,
        /// the requested position for an object is only close to where the prim
        /// actually ends up. If you desire exact placement you'll need to 
        /// follow up by moving the object after it has been created. This
        /// function will not set textures, light and flexible data, or other 
        /// extended primitive properties</remarks>
        public void AddPrim(Primitive.ConstructionData prim, UUID groupID, Vector3 position,
            Vector3 scale, Quaternion rotation)
        {
            AddPrim(prim, groupID, position, scale, rotation, PrimFlags.CreateSelected);
        }

        /// <summary>
        /// Create, or "rez" a new prim object in a simulator
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object to place the object in</param>
        /// <param name="prim">Data describing the prim object to rez</param>
        /// <param name="groupID">Group ID that this prim will be set to, or UUID.Zero if you
        /// do not want the object to be associated with a specific group</param>
        /// <param name="position">An approximation of the position at which to rez the prim</param>
        /// <param name="scale">Scale vector to size this prim</param>
        /// <param name="rotation">Rotation quaternion to rotate this prim</param>
        /// <param name="createFlags">Specify the <seealso cref="PrimFlags"/></param>
        /// <remarks>Due to the way client prim rezzing is done on the server,
        /// the requested position for an object is only close to where the prim
        /// actually ends up. If you desire exact placement you'll need to 
        /// follow up by moving the object after it has been created. This
        /// function will not set textures, light and flexible data, or other 
        /// extended primitive properties</remarks>
        public void AddPrim(Primitive.ConstructionData prim, UUID groupID, Vector3 position,
            Vector3 scale, Quaternion rotation, PrimFlags createFlags)
        {
            ObjectAddPacket packet = new ObjectAddPacket();

            packet.AgentData.AgentID = frame.AgentID;
            packet.AgentData.SessionID = frame.SessionID;
            packet.AgentData.GroupID = groupID;

            packet.ObjectData.State = prim.State;
            packet.ObjectData.AddFlags = (uint)createFlags;
            packet.ObjectData.PCode = (byte)PCode.Prim;

            packet.ObjectData.Material = (byte)prim.Material;
            packet.ObjectData.Scale = scale;
            packet.ObjectData.Rotation = rotation;

            packet.ObjectData.PathCurve = (byte)prim.PathCurve;
            packet.ObjectData.PathBegin = Primitive.PackBeginCut(prim.PathBegin);
            packet.ObjectData.PathEnd = Primitive.PackEndCut(prim.PathEnd);
            packet.ObjectData.PathRadiusOffset = Primitive.PackPathTwist(prim.PathRadiusOffset);
            packet.ObjectData.PathRevolutions = Primitive.PackPathRevolutions(prim.PathRevolutions);
            packet.ObjectData.PathScaleX = Primitive.PackPathScale(prim.PathScaleX);
            packet.ObjectData.PathScaleY = Primitive.PackPathScale(prim.PathScaleY);
            packet.ObjectData.PathShearX = (byte)Primitive.PackPathShear(prim.PathShearX);
            packet.ObjectData.PathShearY = (byte)Primitive.PackPathShear(prim.PathShearY);
            packet.ObjectData.PathSkew = Primitive.PackPathTwist(prim.PathSkew);
            packet.ObjectData.PathTaperX = Primitive.PackPathTaper(prim.PathTaperX);
            packet.ObjectData.PathTaperY = Primitive.PackPathTaper(prim.PathTaperY);
            packet.ObjectData.PathTwist = Primitive.PackPathTwist(prim.PathTwist);
            packet.ObjectData.PathTwistBegin = Primitive.PackPathTwist(prim.PathTwistBegin);

            packet.ObjectData.ProfileCurve = prim.profileCurve;
            packet.ObjectData.ProfileBegin = Primitive.PackBeginCut(prim.ProfileBegin);
            packet.ObjectData.ProfileEnd = Primitive.PackEndCut(prim.ProfileEnd);
            packet.ObjectData.ProfileHollow = Primitive.PackProfileHollow(prim.ProfileHollow);

            packet.ObjectData.RayStart = position;
            packet.ObjectData.RayEnd = position;
            packet.ObjectData.RayEndIsIntersection = 0;
            packet.ObjectData.RayTargetID = UUID.Zero;
            packet.ObjectData.BypassRaycast = 1;

            frame.proxy.InjectPacket(packet,Direction.Outgoing);
        }

        /// <summary>
        /// Rez a Linden tree
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="scale">The size of the tree</param>
        /// <param name="rotation">The rotation of the tree</param>
        /// <param name="position">The position of the tree</param>
        /// <param name="treeType">The Type of tree</param>
        /// <param name="groupOwner">The <seealso cref="UUID"/> of the group to set the tree to, 
        /// or UUID.Zero if no group is to be set</param>
        /// <param name="newTree">true to use the "new" Linden trees, false to use the old</param>
        public void AddTree(Vector3 scale, Quaternion rotation, Vector3 position,
            Tree treeType, UUID groupOwner, bool newTree)
        {
            ObjectAddPacket add = new ObjectAddPacket();

            add.AgentData.AgentID = frame.AgentID;
            add.AgentData.SessionID = frame.SessionID;
            add.AgentData.GroupID = groupOwner;
            add.ObjectData.BypassRaycast = 1;
            add.ObjectData.Material = 3;
            add.ObjectData.PathCurve = 16;
            add.ObjectData.PCode = newTree ? (byte)PCode.NewTree : (byte)PCode.Tree;
            add.ObjectData.RayEnd = position;
            add.ObjectData.RayStart = position;
            add.ObjectData.RayTargetID = UUID.Zero;
            add.ObjectData.Rotation = rotation;
            add.ObjectData.Scale = scale;
            add.ObjectData.State = (byte)treeType;

            frame.proxy.InjectPacket(add,Direction.Outgoing);
        }

        /// <summary>
        /// Rez grass and ground cover
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="scale">The size of the grass</param>
        /// <param name="rotation">The rotation of the grass</param>
        /// <param name="position">The position of the grass</param>
        /// <param name="grassType">The type of grass from the <seealso cref="Grass"/> enum</param>
        /// <param name="groupOwner">The <seealso cref="UUID"/> of the group to set the tree to, 
        /// or UUID.Zero if no group is to be set</param>
        public void AddGrass(Vector3 scale, Quaternion rotation, Vector3 position,
            Grass grassType, UUID groupOwner)
        {
            ObjectAddPacket add = new ObjectAddPacket();

            add.AgentData.AgentID = frame.AgentID;
            add.AgentData.SessionID = frame.SessionID;
            add.AgentData.GroupID = groupOwner;
            add.ObjectData.BypassRaycast = 1;
            add.ObjectData.Material = 3;
            add.ObjectData.PathCurve = 16;
            add.ObjectData.PCode = (byte)PCode.Grass;
            add.ObjectData.RayEnd = position;
            add.ObjectData.RayStart = position;
            add.ObjectData.RayTargetID = UUID.Zero;
            add.ObjectData.Rotation = rotation;
            add.ObjectData.Scale = scale;
            add.ObjectData.State = (byte)grassType;

            frame.proxy.InjectPacket(add,Direction.Outgoing);
        }

        /// <summary>
        /// Set the textures to apply to the faces of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="textures">The texture data to apply</param>
        public void SetTextures(uint localID, Primitive.TextureEntry textures)
        {
            SetTextures(localID, textures, String.Empty);
        }

        /// <summary>
        /// Set the textures to apply to the faces of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="textures">The texture data to apply</param>
        /// <param name="mediaUrl">A media URL (not used)</param>
        public void SetTextures(uint localID, Primitive.TextureEntry textures, string mediaUrl)
        {
            ObjectImagePacket image = new ObjectImagePacket();

            image.AgentData.AgentID = frame.AgentID;
            image.AgentData.SessionID = frame.SessionID;
            image.ObjectData = new ObjectImagePacket.ObjectDataBlock[1];
            image.ObjectData[0] = new ObjectImagePacket.ObjectDataBlock();
            image.ObjectData[0].ObjectLocalID = localID;
            image.ObjectData[0].TextureEntry = textures.GetBytes();
            image.ObjectData[0].MediaURL = Utils.StringToBytes(mediaUrl);

            frame.proxy.InjectPacket(image,Direction.Outgoing);
        }

        /// <summary>
        /// Set the Light data on an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="light">A <seealso cref="Primitive.LightData"/> object containing the data to set</param>
        public void SetLight(uint localID, Primitive.LightData light)
        {
            ObjectExtraParamsPacket extra = new ObjectExtraParamsPacket();

            extra.AgentData.AgentID = frame.AgentID;
            extra.AgentData.SessionID = frame.SessionID;
            extra.ObjectData = new ObjectExtraParamsPacket.ObjectDataBlock[1];
            extra.ObjectData[0] = new ObjectExtraParamsPacket.ObjectDataBlock();
            extra.ObjectData[0].ObjectLocalID = localID;
            extra.ObjectData[0].ParamType = (byte)ExtraParamType.Light;
            if (light.Intensity == 0.0f)
            {
                // Disables the light if intensity is 0
                extra.ObjectData[0].ParamInUse = false;
            }
            else
            {
                extra.ObjectData[0].ParamInUse = true;
            }
            extra.ObjectData[0].ParamData = light.GetBytes();
            extra.ObjectData[0].ParamSize = (uint)extra.ObjectData[0].ParamData.Length;

            frame.proxy.InjectPacket(extra,Direction.Outgoing);
        }

        /// <summary>
        /// Set the flexible data on an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="flexible">A <seealso cref="Primitive.FlexibleData"/> object containing the data to set</param>
        public void SetFlexible(uint localID, Primitive.FlexibleData flexible)
        {
            ObjectExtraParamsPacket extra = new ObjectExtraParamsPacket();

            extra.AgentData.AgentID = frame.AgentID;
            extra.AgentData.SessionID = frame.SessionID;
            extra.ObjectData = new ObjectExtraParamsPacket.ObjectDataBlock[1];
            extra.ObjectData[0] = new ObjectExtraParamsPacket.ObjectDataBlock();
            extra.ObjectData[0].ObjectLocalID = localID;
            extra.ObjectData[0].ParamType = (byte)ExtraParamType.Flexible;
            extra.ObjectData[0].ParamInUse = true;
            extra.ObjectData[0].ParamData = flexible.GetBytes();
            extra.ObjectData[0].ParamSize = (uint)extra.ObjectData[0].ParamData.Length;

            frame.proxy.InjectPacket(extra,Direction.Outgoing);
        }

        /// <summary>
        /// Set the sculptie texture and data on an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="sculpt">A <seealso cref="Primitive.SculptData"/> object containing the data to set</param>
        public void SetSculpt(uint localID, Primitive.SculptData sculpt)
        {
            ObjectExtraParamsPacket extra = new ObjectExtraParamsPacket();

            extra.AgentData.AgentID = frame.AgentID;
            extra.AgentData.SessionID = frame.SessionID;

            extra.ObjectData = new ObjectExtraParamsPacket.ObjectDataBlock[1];
            extra.ObjectData[0] = new ObjectExtraParamsPacket.ObjectDataBlock();
            extra.ObjectData[0].ObjectLocalID = localID;
            extra.ObjectData[0].ParamType = (byte)ExtraParamType.Sculpt;
            extra.ObjectData[0].ParamInUse = true;
            extra.ObjectData[0].ParamData = sculpt.GetBytes();
            extra.ObjectData[0].ParamSize = (uint)extra.ObjectData[0].ParamData.Length;

            frame.proxy.InjectPacket(extra,Direction.Outgoing);

            // Not sure why, but if you don't send this the sculpted prim disappears
            ObjectShapePacket shape = new ObjectShapePacket();

            shape.AgentData.AgentID = frame.AgentID;
            shape.AgentData.SessionID = frame.SessionID;

            shape.ObjectData = new OpenMetaverse.Packets.ObjectShapePacket.ObjectDataBlock[1];
            shape.ObjectData[0] = new OpenMetaverse.Packets.ObjectShapePacket.ObjectDataBlock();
            shape.ObjectData[0].ObjectLocalID = localID;
            shape.ObjectData[0].PathScaleX = 100;
            shape.ObjectData[0].PathScaleY = 150;
            shape.ObjectData[0].PathCurve = 32;

            frame.proxy.InjectPacket(shape,Direction.Outgoing);
        }

        /// <summary>
        /// Set additional primitive parameters on an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="type">The extra parameters to set</param>
        public void SetExtraParamOff(uint localID, ExtraParamType type)
        {
            ObjectExtraParamsPacket extra = new ObjectExtraParamsPacket();

            extra.AgentData.AgentID = frame.AgentID;
            extra.AgentData.SessionID = frame.SessionID;
            extra.ObjectData = new ObjectExtraParamsPacket.ObjectDataBlock[1];
            extra.ObjectData[0] = new ObjectExtraParamsPacket.ObjectDataBlock();
            extra.ObjectData[0].ObjectLocalID = localID;
            extra.ObjectData[0].ParamType = (byte)type;
            extra.ObjectData[0].ParamInUse = false;
            extra.ObjectData[0].ParamData = Utils.EmptyBytes;
            extra.ObjectData[0].ParamSize = 0;

            frame.proxy.InjectPacket(extra,Direction.Outgoing);
        }

        /// <summary>
        /// Link multiple prims into a linkset
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to link</param>
        /// <remarks>The last object in the array will be the root object of the linkset TODO: Is this true?</remarks>
        public void LinkPrims(List<uint> localIDs)
        {
            ObjectLinkPacket packet = new ObjectLinkPacket();

            packet.AgentData.AgentID = frame.AgentID;
            packet.AgentData.SessionID = frame.SessionID;

            packet.ObjectData = new ObjectLinkPacket.ObjectDataBlock[localIDs.Count];

            int i = 0;
            foreach (uint localID in localIDs)
            {
                packet.ObjectData[i] = new ObjectLinkPacket.ObjectDataBlock();
                packet.ObjectData[i].ObjectLocalID = localID;

                i++;
            }

            frame.proxy.InjectPacket(packet,Direction.Outgoing);
        }

        /// <summary>
        /// Change the rotation of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="rotation">The new rotation of the object</param>
        public void SetRotation(uint localID, Quaternion rotation)
        {
            ObjectRotationPacket objRotPacket = new ObjectRotationPacket();
            objRotPacket.AgentData.AgentID = frame.AgentID;
            objRotPacket.AgentData.SessionID = frame.SessionID;

            objRotPacket.ObjectData = new ObjectRotationPacket.ObjectDataBlock[1];

            objRotPacket.ObjectData[0] = new ObjectRotationPacket.ObjectDataBlock();
            objRotPacket.ObjectData[0].ObjectLocalID = localID;
            objRotPacket.ObjectData[0].Rotation = rotation;
            frame.proxy.InjectPacket(objRotPacket,Direction.Outgoing);
        }

        /// <summary>
        /// Set the name of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="name">A string containing the new name of the object</param>
        public void SetName(uint localID, string name)
        {
            SetNames(new uint[] { localID }, new string[] { name });
        }

        /// <summary>
        /// Set the name of multiple objects
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to change the name of</param>
        /// <param name="names">An array which contains the new names of the objects</param>
        public void SetNames(uint[] localIDs, string[] names)
        {
            ObjectNamePacket namePacket = new ObjectNamePacket();
            namePacket.AgentData.AgentID = frame.AgentID;
            namePacket.AgentData.SessionID = frame.SessionID;

            namePacket.ObjectData = new ObjectNamePacket.ObjectDataBlock[localIDs.Length];

            for (int i = 0; i < localIDs.Length; ++i)
            {
                namePacket.ObjectData[i] = new ObjectNamePacket.ObjectDataBlock();
                namePacket.ObjectData[i].LocalID = localIDs[i];
                namePacket.ObjectData[i].Name = Utils.StringToBytes(names[i]);
            }

            frame.proxy.InjectPacket(namePacket,Direction.Outgoing);
        }

        /// <summary>
        /// Set the description of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="description">A string containing the new description of the object</param>
        public void SetDescription(uint localID, string description)
        {
            SetDescriptions(new uint[] { localID }, new string[] { description });
        }

        /// <summary>
        /// Set the descriptions of multiple objects
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to change the description of</param>
        /// <param name="descriptions">An array which contains the new descriptions of the objects</param>
        public void SetDescriptions(uint[] localIDs, string[] descriptions)
        {
            ObjectDescriptionPacket descPacket = new ObjectDescriptionPacket();
            descPacket.AgentData.AgentID = frame.AgentID;
            descPacket.AgentData.SessionID = frame.SessionID;

            descPacket.ObjectData = new ObjectDescriptionPacket.ObjectDataBlock[localIDs.Length];

            for (int i = 0; i < localIDs.Length; ++i)
            {
                descPacket.ObjectData[i] = new ObjectDescriptionPacket.ObjectDataBlock();
                descPacket.ObjectData[i].LocalID = localIDs[i];
                descPacket.ObjectData[i].Description = Utils.StringToBytes(descriptions[i]);
            }

            frame.proxy.InjectPacket(descPacket,Direction.Outgoing);
        }

        /// <summary>
        /// Attach an object to this avatar
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="attachPoint">The point on the avatar the object will be attached</param>
        /// <param name="rotation">The rotation of the attached object</param>
        public void AttachObject(uint localID, AttachmentPoint attachPoint, Quaternion rotation)
        {
            ObjectAttachPacket attach = new ObjectAttachPacket();
            attach.AgentData.AgentID = frame.AgentID;
            attach.AgentData.SessionID = frame.SessionID;
            attach.AgentData.AttachmentPoint = (byte)attachPoint;

            attach.ObjectData = new ObjectAttachPacket.ObjectDataBlock[1];
            attach.ObjectData[0] = new ObjectAttachPacket.ObjectDataBlock();
            attach.ObjectData[0].ObjectLocalID = localID;
            attach.ObjectData[0].Rotation = rotation;

            frame.proxy.InjectPacket(attach,Direction.Outgoing);
        }

        /// <summary>
        /// Drop an attached object from this avatar
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/>
        /// object where the objects reside. This will always be the simulator the avatar is currently in
        /// </param>
        /// <param name="localID">The object's ID which is local to the simulator the object is in</param>
        public void DropObject(uint localID)
        {
            ObjectDropPacket dropit = new ObjectDropPacket();
            dropit.AgentData.AgentID = frame.AgentID;
            dropit.AgentData.SessionID = frame.SessionID;
            dropit.ObjectData = new ObjectDropPacket.ObjectDataBlock[1];
            dropit.ObjectData[0] = new ObjectDropPacket.ObjectDataBlock();
            dropit.ObjectData[0].ObjectLocalID = localID;

            frame.proxy.InjectPacket(dropit,Direction.Outgoing);
        }

        /// <summary>
        /// Detach an object from yourself
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> 
        /// object where the objects reside
        /// 
        /// This will always be the simulator the avatar is currently in
        /// </param>
        /// <param name="localIDs">An array which contains the IDs of the objects to detach</param>
        public void DetachObjects(List<uint> localIDs)
        {
            ObjectDetachPacket detach = new ObjectDetachPacket();
            detach.AgentData.AgentID = frame.AgentID;
            detach.AgentData.SessionID = frame.SessionID;
            detach.ObjectData = new ObjectDetachPacket.ObjectDataBlock[localIDs.Count];

            int i = 0;
            foreach (uint localid in localIDs)
            {
                detach.ObjectData[i] = new ObjectDetachPacket.ObjectDataBlock();
                detach.ObjectData[i].ObjectLocalID = localid;
                i++;
            }

            frame.proxy.InjectPacket(detach,Direction.Outgoing);
        }

        /// <summary>
        /// Change the position of an object, Will change position of entire linkset
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="position">The new position of the object</param>
        public void SetPosition(uint localID, Vector3 position)
        {
            UpdateObject(localID, position, UpdateType.Position | UpdateType.Linked);
        }

        /// <summary>
        /// Change the position of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="position">The new position of the object</param>
        /// <param name="childOnly">if true, will change position of (this) child prim only, not entire linkset</param>
        public void SetPosition(uint localID, Vector3 position, bool childOnly)
        {
            UpdateType type = UpdateType.Position;

            if (!childOnly)
                type |= UpdateType.Linked;

            UpdateObject(localID, position, type);
        }

        /// <summary>
        /// Change the Scale (size) of an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="scale">The new scale of the object</param>
        /// <param name="childOnly">If true, will change scale of this prim only, not entire linkset</param>
        /// <param name="uniform">True to resize prims uniformly</param>
        public void SetScale(uint localID, Vector3 scale, bool childOnly, bool uniform)
        {
            UpdateType type = UpdateType.Scale;

            if (!childOnly)
                type |= UpdateType.Linked;

            if (uniform)
                type |= UpdateType.Uniform;

            UpdateObject(localID, scale, type);
        }

        /// <summary>
        /// Change the Rotation of an object that is either a child or a whole linkset
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="quat">The new scale of the object</param>
        /// <param name="childOnly">If true, will change rotation of this prim only, not entire linkset</param>
        public void SetRotation(uint localID, Quaternion quat, bool childOnly)
        {
            UpdateType type = UpdateType.Rotation;

            if (!childOnly)
                type |= UpdateType.Linked;

            MultipleObjectUpdatePacket multiObjectUpdate = new MultipleObjectUpdatePacket();
            multiObjectUpdate.AgentData.AgentID = frame.AgentID;
            multiObjectUpdate.AgentData.SessionID = frame.SessionID;

            multiObjectUpdate.ObjectData = new MultipleObjectUpdatePacket.ObjectDataBlock[1];

            multiObjectUpdate.ObjectData[0] = new MultipleObjectUpdatePacket.ObjectDataBlock();
            multiObjectUpdate.ObjectData[0].Type = (byte)type;
            multiObjectUpdate.ObjectData[0].ObjectLocalID = localID;
            multiObjectUpdate.ObjectData[0].Data = quat.GetBytes();

            frame.proxy.InjectPacket(multiObjectUpdate,Direction.Outgoing);
        }

        /// <summary>
        /// Send a Multiple Object Update packet to change the size, scale or rotation of a primitive
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="data">The new rotation, size, or position of the target object</param>
        /// <param name="type">The flags from the <seealso cref="UpdateType"/> Enum</param>
        public void UpdateObject(uint localID, Vector3 data, UpdateType type)
        {
            MultipleObjectUpdatePacket multiObjectUpdate = new MultipleObjectUpdatePacket();
            multiObjectUpdate.AgentData.AgentID = frame.AgentID;
            multiObjectUpdate.AgentData.SessionID = frame.SessionID;

            multiObjectUpdate.ObjectData = new MultipleObjectUpdatePacket.ObjectDataBlock[1];

            multiObjectUpdate.ObjectData[0] = new MultipleObjectUpdatePacket.ObjectDataBlock();
            multiObjectUpdate.ObjectData[0].Type = (byte)type;
            multiObjectUpdate.ObjectData[0].ObjectLocalID = localID;
            multiObjectUpdate.ObjectData[0].Data = data.GetBytes();

            frame.proxy.InjectPacket(multiObjectUpdate,Direction.Outgoing);
        }

        /// <summary>
        /// Deed an object (prim) to a group, Object must be shared with group which
        /// can be accomplished with SetPermissions()
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localID">The objects ID which is local to the simulator the object is in</param>
        /// <param name="groupOwner">The <seealso cref="UUID"/> of the group to deed the object to</param>
        public void DeedObject(uint localID, UUID groupOwner)
        {
            ObjectOwnerPacket objDeedPacket = new ObjectOwnerPacket();
            objDeedPacket.AgentData.AgentID = frame.AgentID;
            objDeedPacket.AgentData.SessionID = frame.SessionID;

            // Can only be use in God mode
            objDeedPacket.HeaderData.Override = false;
            objDeedPacket.HeaderData.OwnerID = UUID.Zero;
            objDeedPacket.HeaderData.GroupID = groupOwner;

            objDeedPacket.ObjectData = new ObjectOwnerPacket.ObjectDataBlock[1];
            objDeedPacket.ObjectData[0] = new ObjectOwnerPacket.ObjectDataBlock();

            objDeedPacket.ObjectData[0].ObjectLocalID = localID;

            frame.proxy.InjectPacket(objDeedPacket,Direction.Outgoing);
        }

        /// <summary>
        /// Deed multiple objects (prims) to a group, Objects must be shared with group which
        /// can be accomplished with SetPermissions()
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to deed</param>
        /// <param name="groupOwner">The <seealso cref="UUID"/> of the group to deed the object to</param>
        public void DeedObjects(List<uint> localIDs, UUID groupOwner)
        {
            ObjectOwnerPacket packet = new ObjectOwnerPacket();
            packet.AgentData.AgentID = frame.AgentID;
            packet.AgentData.SessionID = frame.SessionID;

            // Can only be use in God mode
            packet.HeaderData.Override = false;
            packet.HeaderData.OwnerID = UUID.Zero;
            packet.HeaderData.GroupID = groupOwner;

            packet.ObjectData = new ObjectOwnerPacket.ObjectDataBlock[localIDs.Count];

            for (int i = 0; i < localIDs.Count; i++)
            {
                packet.ObjectData[i] = new ObjectOwnerPacket.ObjectDataBlock();
                packet.ObjectData[i].ObjectLocalID = localIDs[i];
            }
            frame.proxy.InjectPacket(packet,Direction.Outgoing);
        }

        /// <summary>
        /// Set the permissions on multiple objects
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the objects reside</param>
        /// <param name="localIDs">An array which contains the IDs of the objects to set the permissions on</param>
        /// <param name="who">The new Who mask to set</param>
        /// <param name="permissions">The new Permissions mark to set</param>
        /// <param name="set">TODO: What does this do?</param>
        public void SetPermissions(List<uint> localIDs, PermissionWho who,
            PermissionMask permissions, bool set)
        {
            ObjectPermissionsPacket packet = new ObjectPermissionsPacket();

            packet.AgentData.AgentID = frame.AgentID;
            packet.AgentData.SessionID = frame.SessionID;

            // Override can only be used by gods
            packet.HeaderData.Override = false;

            packet.ObjectData = new ObjectPermissionsPacket.ObjectDataBlock[localIDs.Count];

            for (int i = 0; i < localIDs.Count; i++)
            {
                packet.ObjectData[i] = new ObjectPermissionsPacket.ObjectDataBlock();

                packet.ObjectData[i].ObjectLocalID = localIDs[i];
                packet.ObjectData[i].Field = (byte)who;
                packet.ObjectData[i].Mask = (uint)permissions;
                packet.ObjectData[i].Set = Convert.ToByte(set);
            }

            frame.proxy.InjectPacket(packet,Direction.Outgoing);
        }

        /// <summary>
        /// Request additional properties for an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="objectID"></param>
        public void RequestObjectPropertiesFamily(UUID objectID)
        {
            RequestObjectPropertiesFamily(objectID, true);
        }

        /// <summary>
        /// Request additional properties for an object
        /// </summary>
        /// <param name="simulator">A reference to the <seealso cref="OpenMetaverse.Simulator"/> object where the object resides</param>
        /// <param name="objectID">Absolute UUID of the object</param>
        /// <param name="reliable">Whether to require server acknowledgement of this request</param>
        public void RequestObjectPropertiesFamily(UUID objectID, bool reliable)
        {
            RequestObjectPropertiesFamilyPacket properties = new RequestObjectPropertiesFamilyPacket();
            properties.AgentData.AgentID = frame.AgentID;
            properties.AgentData.SessionID = frame.SessionID;
            properties.ObjectData.ObjectID = objectID;
            // TODO: RequestFlags is typically only for bug report submissions, but we might be able to
            // use it to pass an arbitrary uint back to the callback
            properties.ObjectData.RequestFlags = 0;

            properties.Header.Reliable = reliable;

            frame.proxy.InjectPacket(properties,Direction.Outgoing);
        }

        #endregion
		
		#region Delegates

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="prim"></param>
        /// <param name="regionHandle"></param>
        /// <param name="timeDilation"></param>
        public delegate void NewPrimCallback(Primitive prim, ulong regionHandle,
            ushort timeDilation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="prim"></param>
        /// <param name="regionHandle"></param>
        /// <param name="timeDilation"></param>
        public delegate void NewAttachmentCallback(Primitive prim, ulong regionHandle,
            ushort timeDilation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="props"></param>
        public delegate void ObjectPropertiesCallback(Primitive.ObjectProperties props);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="props"></param>
        /// <param name="type"></param>
        public delegate void ObjectPropertiesFamilyCallback(Primitive.ObjectProperties props,
            ReportType type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="avatar"></param>
        /// <param name="regionHandle"></param>
        /// <param name="timeDilation"></param>
        public delegate void NewAvatarCallback(Avatar avatar, ulong regionHandle,
            ushort timeDilation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="foliage"></param>
        /// <param name="regionHandle"></param>
        /// <param name="timeDilation"></param>
        public delegate void NewFoliageCallback(Primitive foliage, ulong regionHandle,
            ushort timeDilation);
        /// <summary>
        /// Called whenever an object disappears
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="update"></param>
        /// <param name="regionHandle"></param>
        /// <param name="timeDilation"></param>
        public delegate void ObjectUpdatedCallback(ObjectUpdate update, ulong regionHandle,
            ushort timeDilation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="objectID"></param>
        public delegate void KillObjectCallback(uint objectID);
        /// <summary>
        /// Called whenever the client avatar sits down or stands up
        /// </summary>
        /// <param name="simulator">Simulator the packet was received from</param>
        /// <param name="avatar"></param>
        /// <param name="sittingOn">The local ID of the object that is being sat
        /// <param name="oldSeat"></param>
        /// on. If this is zero the avatar is not sitting on an object</param>
        public delegate void AvatarSitChanged(Avatar avatar, uint sittingOn, uint oldSeat);

        #endregion Delegates

        #region Events

        /// <summary>
        /// This event will be raised for every ObjectUpdate block that 
        /// contains a prim that isn't attached to an avatar.
        /// </summary>
        /// <remarks>Depending on the circumstances a client could 
        /// receive two or more of these events for the same object, if you 
        /// or the object left the current sim and returned for example. Client
        /// applications are responsible for tracking and storing objects.
        /// </remarks>
        public event NewPrimCallback OnNewPrim;
        /// <summary>
        /// This event will be raised for every ObjectUpdate block that 
        /// contains an avatar attachment.
        /// </summary>
        /// <remarks>Depending on the circumstances a client could 
        /// receive two or more of these events for the same object, if you 
        /// or the object left the current sim and returned for example. Client
        /// applications are responsible for tracking and storing objects.
        /// </remarks>
        public event NewAttachmentCallback OnNewAttachment;
        /// <summary>
        /// This event will be raised for every ObjectUpdate block that 
        /// contains a new avatar.
        /// </summary>
        /// <remarks>Depending on the circumstances a client 
        /// could receive two or more of these events for the same avatar, if 
        /// you or the other avatar left the current sim and returned for 
        /// example. Client applications are responsible for tracking and 
        /// storing objects.
        /// </remarks>
        public event NewAvatarCallback OnNewAvatar;
        /// <summary>
        /// This event will be raised when a terse object update packet is 
        /// received, containing the updated position, rotation, and 
        /// movement-related vectors
        /// </summary>
        public event ObjectUpdatedCallback OnObjectUpdated;
        /// <summary>
        /// This event will be raised when an avatar sits on an object
        /// or stands up, with a local ID of the current seat or zero.
        /// </summary>
        public event AvatarSitChanged OnAvatarSitChanged;
        /// <summary>
        /// This event will be raised when an object is removed from a 
        /// simulator.
        /// </summary>
        public event KillObjectCallback OnObjectKilled;
        /// <summary>
        /// This event will be raised when an objects properties are received
        /// from the simulator
        /// </summary>
        public event ObjectPropertiesCallback OnObjectProperties;
        /// <summary>
        /// Thie event will be raised when an objects properties family 
        /// information is recieved from the simulator. ObjectPropertiesFamily
        /// is a subset of the fields found in ObjectProperties
        /// </summary>
        public event ObjectPropertiesFamilyCallback OnObjectPropertiesFamily;

        #endregion
		
		#region Packet Handlers

        /// <summary>
        /// Used for new prims, or significant changes to existing prims
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="simulator"></param>
        protected Packet UpdateHandler(Packet packet, IPEndPoint IP)
        {
            ObjectUpdatePacket update = (ObjectUpdatePacket)packet;
            //UpdateDilation(update.RegionData.TimeDilation);

            for (int b = 0; b < update.ObjectData.Length; b++)
            {
                ObjectUpdatePacket.ObjectDataBlock block = update.ObjectData[b];

                Vector4 collisionPlane = Vector4.Zero;
                Vector3 position;
                Vector3 velocity;
                Vector3 acceleration;
                Quaternion rotation;
                Vector3 angularVelocity;
                NameValue[] nameValues;
                bool attachment = false;
                PCode pcode = (PCode)block.PCode;

                #region Relevance check

                // Check if we are interested in this object
				// HACK I'm lazy
                if (true)
                {
                    switch (pcode)
                    {
                        case PCode.Grass:
                        case PCode.Tree:
                        case PCode.NewTree:
                        case PCode.Prim:
                            if (OnNewPrim == null) continue;
                            break;
                        case PCode.Avatar:
                            // Make an exception for updates about our own agent
                            //if (block.FullID != frame.AgentID && OnNewAvatar == null) continue;
                            break;
                        case PCode.ParticleSystem:
                            continue; // TODO: Do something with these
                    }
                }

                #endregion Relevance check

                #region NameValue parsing

                string nameValue = Utils.BytesToString(block.NameValue);
                if (nameValue.Length > 0)
                {
                    string[] lines = nameValue.Split('\n');
                    nameValues = new NameValue[lines.Length];

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(lines[i]))
                        {
                            NameValue nv = new NameValue(lines[i]);
                            if (nv.Name == "AttachItemID") attachment = true;
                            nameValues[i] = nv;
                        }
                    }
                }
                else
                {
                    nameValues = new NameValue[0];
                }

                #endregion NameValue parsing

                #region Decode Object (primitive) parameters
                Primitive.ConstructionData data = new Primitive.ConstructionData();
                data.State = block.State;
                data.Material = (Material)block.Material;
                data.PathCurve = (PathCurve)block.PathCurve;
                data.profileCurve = block.ProfileCurve;
                data.PathBegin = Primitive.UnpackBeginCut(block.PathBegin);
                data.PathEnd = Primitive.UnpackEndCut(block.PathEnd);
                data.PathScaleX = Primitive.UnpackPathScale(block.PathScaleX);
                data.PathScaleY = Primitive.UnpackPathScale(block.PathScaleY);
                data.PathShearX = Primitive.UnpackPathShear((sbyte)block.PathShearX);
                data.PathShearY = Primitive.UnpackPathShear((sbyte)block.PathShearY);
                data.PathTwist = Primitive.UnpackPathTwist(block.PathTwist);
                data.PathTwistBegin = Primitive.UnpackPathTwist(block.PathTwistBegin);
                data.PathRadiusOffset = Primitive.UnpackPathTwist(block.PathRadiusOffset);
                data.PathTaperX = Primitive.UnpackPathTaper(block.PathTaperX);
                data.PathTaperY = Primitive.UnpackPathTaper(block.PathTaperY);
                data.PathRevolutions = Primitive.UnpackPathRevolutions(block.PathRevolutions);
                data.PathSkew = Primitive.UnpackPathTwist(block.PathSkew);
                data.ProfileBegin = Primitive.UnpackBeginCut(block.ProfileBegin);
                data.ProfileEnd = Primitive.UnpackEndCut(block.ProfileEnd);
                data.ProfileHollow = Primitive.UnpackProfileHollow(block.ProfileHollow);
                data.PCode = pcode;
                #endregion

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
                        Console.Write("Got an ObjectUpdate block with ObjectUpdate field length of " +
                            block.ObjectData.Length.ToString());

                        continue;
                }
                #endregion

                // Determine the object type and create the appropriate class
                switch (pcode)
                {
                    #region Prim and Foliage
                    case PCode.Grass:
                    case PCode.Tree:
                    case PCode.NewTree:
                    case PCode.Prim:
                        Primitive prim = GetPrimitive(block.ID, block.FullID);

                        #region Update Prim Info with decoded data
                        prim.Flags = (PrimFlags)block.UpdateFlags;

                        if ((prim.Flags & PrimFlags.ZlibCompressed) != 0)
                        {
                            //Logger.Log("Got a ZlibCompressed ObjectUpdate, implement me!",
                            //    Helpers.LogLevel.Warning, Client);
                            continue;
                        }

                        // Automatically request ObjectProperties for prim if it was rezzed selected.
                        if ((prim.Flags & PrimFlags.CreateSelected) != 0)
                        {
                            SelectObject(prim.LocalID);
                        }

                        prim.NameValues = nameValues;
                        prim.LocalID = block.ID;
                        prim.ID = block.FullID;
                        prim.ParentID = block.ParentID;
                        prim.RegionHandle = update.RegionData.RegionHandle;
                        prim.Scale = block.Scale;
                        prim.ClickAction = (ClickAction)block.ClickAction;
                        prim.OwnerID = block.OwnerID;
                        prim.MediaURL = Utils.BytesToString(block.MediaURL);
                        prim.Text = Utils.BytesToString(block.Text);
                        prim.TextColor = new Color4(block.TextColor, 0, false, true);

                        // Sound information
                        prim.Sound = block.Sound;
                        prim.SoundFlags = (SoundFlags)block.Flags;
                        prim.SoundGain = block.Gain;
                        prim.SoundRadius = block.Radius;

                        // Joint information
                        prim.Joint = (JointType)block.JointType;
                        prim.JointPivot = block.JointPivot;
                        prim.JointAxisOrAnchor = block.JointAxisOrAnchor;

                        // Object parameters
                        prim.PrimData = data;

                        // Textures, texture animations, particle system, and extra params
                        prim.Textures = new Primitive.TextureEntry(block.TextureEntry, 0,
                            block.TextureEntry.Length);

                        prim.TextureAnim = new Primitive.TextureAnimation(block.TextureAnim, 0);
                        prim.ParticleSys = new Primitive.ParticleSystem(block.PSBlock, 0);
                        prim.SetExtraParamsFromBytes(block.ExtraParams, 0);

                        // PCode-specific data
                        switch (pcode)
                        {
                            case PCode.Grass:
                            case PCode.Tree:
                            case PCode.NewTree:
                                if (block.Data.Length == 1)
                                    prim.TreeSpecies = (Tree)block.Data[0];
                                else
                                    Logger.Log("Got a foliage update with an invalid TreeSpecies field", Helpers.LogLevel.Warning);
                                prim.ScratchPad = Utils.EmptyBytes;
                                break;
                            default:
                                prim.ScratchPad = new byte[block.Data.Length];
                                if (block.Data.Length > 0)
                                    Buffer.BlockCopy(block.Data, 0, prim.ScratchPad, 0, prim.ScratchPad.Length);
                                break;
                        }

                        // Packed parameters
                        prim.CollisionPlane = collisionPlane;
                        prim.Position = position;
                        prim.Velocity = velocity;
                        prim.Acceleration = acceleration;
                        prim.Rotation = rotation;
                        prim.AngularVelocity = angularVelocity;
                        #endregion

                        if (attachment)
                            FireOnNewAttachment(prim, update.RegionData.RegionHandle,
                                update.RegionData.TimeDilation);
                        else
                            FireOnNewPrim(prim, update.RegionData.RegionHandle,
                                update.RegionData.TimeDilation);

                        break;
                    #endregion Prim and Foliage
                    #region Avatar
                    case PCode.Avatar:

                        // Update some internals if this is our avatar
					/*
                        if (block.FullID == frame.AgentID)
                        {
                            #region Update Client.Self

                            // We need the local ID to recognize terse updates for our agent
                            Client.Self.localID = block.ID;

                            // Packed parameters
                            Client.Self.collisionPlane = collisionPlane;
                            Client.Self.relativePosition = position;
                            Client.Self.velocity = velocity;
                            Client.Self.acceleration = acceleration;
                            Client.Self.relativeRotation = rotation;
                            Client.Self.angularVelocity = angularVelocity;

                            #endregion
                        }
*/
                        #region Create an Avatar from the decoded data

                        Avatar avatar = GetAvatar(block.ID, block.FullID);
                        uint oldSeatID = avatar.ParentID;

                        avatar.ID = block.FullID;
                        avatar.LocalID = block.ID;
                        avatar.CollisionPlane = collisionPlane;
                        avatar.Position = position;
                        avatar.Velocity = velocity;
                        avatar.Acceleration = acceleration;
                        avatar.Rotation = rotation;
                        avatar.AngularVelocity = angularVelocity;
                        avatar.NameValues = nameValues;
                        avatar.PrimData = data;
                        if (block.Data.Length > 0) Logger.Log("Unexpected Data field for an avatar update, length " + block.Data.Length, Helpers.LogLevel.Warning);
                        avatar.ParentID = block.ParentID;
                        avatar.RegionHandle = update.RegionData.RegionHandle;

                        //SetAvatarSittingOn(avatar, block.ParentID, oldSeatID);

                        // Textures
                        avatar.Textures = new Primitive.TextureEntry(block.TextureEntry, 0,
                            block.TextureEntry.Length);

                        #endregion Create an Avatar from the decoded data

                        FireOnNewAvatar(avatar, update.RegionData.RegionHandle,
                            update.RegionData.TimeDilation);
                       
						break;
                    #endregion Avatar
                    case PCode.ParticleSystem:
                        DecodeParticleUpdate(block);
                        // TODO: Create a callback for particle updates
                        break;
                    default:
                        //Logger.DebugLog("Got an ObjectUpdate block with an unrecognized PCode " + pcode.ToString(), Client);
                        break;
                }
            }
			return packet;
        }

        protected void DecodeParticleUpdate(ObjectUpdatePacket.ObjectDataBlock block)
        {
            // TODO: Handle ParticleSystem ObjectUpdate blocks

            // float bounce_b
            // Vector4 scale_range
            // Vector4 alpha_range
            // Vector3 vel_offset
            // float dist_begin_fadeout
            // float dist_end_fadeout
            // UUID image_uuid
            // long flags
            // byte createme
            // Vector3 diff_eq_alpha
            // Vector3 diff_eq_scale
            // byte max_particles
            // byte initial_particles
            // float kill_plane_z
            // Vector3 kill_plane_normal
            // float bounce_plane_z
            // Vector3 bounce_plane_normal
            // float spawn_range
            // float spawn_frequency
            // float spawn_frequency_range
            // Vector3 spawn_direction
            // float spawn_direction_range
            // float spawn_velocity
            // float spawn_velocity_range
            // float speed_limit
            // float wind_weight
            // Vector3 current_gravity
            // float gravity_weight
            // float global_lifetime
            // float individual_lifetime
            // float individual_lifetime_range
            // float alpha_decay
            // float scale_decay
            // float distance_death
            // float damp_motion_factor
            // Vector3 wind_diffusion_factor
        }

        /// <summary>
        /// A terse object update, used when a transformation matrix or
        /// velocity/acceleration for an object changes but nothing else
        /// (scale/position/rotation/acceleration/velocity)
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="simulator"></param>
        protected Packet TerseUpdateHandler(Packet packet, IPEndPoint IP)
        {
            ImprovedTerseObjectUpdatePacket terse = (ImprovedTerseObjectUpdatePacket)packet;
            //UpdateDilation(terse.RegionData.TimeDilation);

            for (int i = 0; i < terse.ObjectData.Length; i++)
            {
                ImprovedTerseObjectUpdatePacket.ObjectDataBlock block = terse.ObjectData[i];

                try
                {
                    int pos = 4;
                    uint localid = Utils.BytesToUInt(block.Data, 0);

                    // Check if we are interested in this update
                    if (OnObjectUpdated == null)
                        continue;

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
                    // Angular velocity (omega)
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

                    Primitive obj = (update.Avatar) ?
                        (Primitive)GetAvatar(update.LocalID, UUID.Zero) :
                        (Primitive)GetPrimitive(update.LocalID, UUID.Zero);

                    #region Update Client.Self
                    /*if (update.LocalID == Client.Self.localID)
                    {
                        Client.Self.collisionPlane = update.CollisionPlane;
                        Client.Self.relativePosition = update.Position;
                        Client.Self.velocity = update.Velocity;
                        Client.Self.acceleration = update.Acceleration;
                        Client.Self.relativeRotation = update.Rotation;
                        Client.Self.angularVelocity = update.AngularVelocity;
                    }*/
                    #endregion Update Client.Self

                    obj.Acceleration = update.Acceleration;
                    obj.AngularVelocity = update.AngularVelocity;
                    obj.CollisionPlane = update.CollisionPlane;
                    obj.Position = update.Position;
                    obj.Rotation = update.Rotation;
                    obj.Velocity = update.Velocity;
                    if (update.Textures != null)
                        obj.Textures = update.Textures;

                    // Fire the callback
                    FireOnObjectUpdated(update, terse.RegionData.RegionHandle, terse.RegionData.TimeDilation);
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message, Helpers.LogLevel.Warning, e);
                }
            }
			return packet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="simulator"></param>
        protected Packet CompressedUpdateHandler(Packet packet, IPEndPoint IP)
        {
            ObjectUpdateCompressedPacket update = (ObjectUpdateCompressedPacket)packet;

            for (int b = 0; b < update.ObjectData.Length; b++)
            {
                ObjectUpdateCompressedPacket.ObjectDataBlock block = update.ObjectData[b];
                int i = 0;

                try
                {
                    // UUID
                    UUID FullID = new UUID(block.Data, 0);
                    i += 16;
                    // Local ID
                    uint LocalID = (uint)(block.Data[i++] + (block.Data[i++] << 8) +
                        (block.Data[i++] << 16) + (block.Data[i++] << 24));
                    // PCode
                    PCode pcode = (PCode)block.Data[i++];

                    #region Relevance check

                    if (true)
                    {
                        switch (pcode)
                        {
                            case PCode.Grass:
                            case PCode.Tree:
                            case PCode.NewTree:
                            case PCode.Prim:
                                if (OnNewPrim == null) continue;
                                break;
                        }
                    }

                    #endregion Relevance check

                    Primitive prim = GetPrimitive(LocalID, FullID);

                    prim.LocalID = LocalID;
                    prim.ID = FullID;
                    prim.Flags = (PrimFlags)block.UpdateFlags;
                    prim.PrimData.PCode = pcode;

                    #region Decode block and update Prim

                    // State
                    prim.PrimData.State = block.Data[i++];
                    // CRC
                    i += 4;
                    // Material
                    prim.PrimData.Material = (Material)block.Data[i++];
                    // Click action
                    prim.ClickAction = (ClickAction)block.Data[i++];
                    // Scale
                    prim.Scale = new Vector3(block.Data, i);
                    i += 12;
                    // Position
                    prim.Position = new Vector3(block.Data, i);
                    i += 12;
                    // Rotation
                    prim.Rotation = new Quaternion(block.Data, i, true);
                    i += 12;
                    // Compressed flags
                    CompressedFlags flags = (CompressedFlags)Utils.BytesToUInt(block.Data, i);
                    i += 4;

                    prim.OwnerID = new UUID(block.Data, i);
                    i += 16;

                    // Angular velocity
                    if ((flags & CompressedFlags.HasAngularVelocity) != 0)
                    {
                        prim.AngularVelocity = new Vector3(block.Data, i);
                        i += 12;
                    }

                    // Parent ID
                    if ((flags & CompressedFlags.HasParent) != 0)
                    {
                        prim.ParentID = (uint)(block.Data[i++] + (block.Data[i++] << 8) +
                        (block.Data[i++] << 16) + (block.Data[i++] << 24));
                    }
                    else
                    {
                        prim.ParentID = 0;
                    }

                    // Tree data
                    if ((flags & CompressedFlags.Tree) != 0)
                    {
                        prim.TreeSpecies = (Tree)block.Data[i++];
                        prim.ScratchPad = Utils.EmptyBytes;
                    }
                    // Scratch pad
                    else if ((flags & CompressedFlags.ScratchPad) != 0)
                    {
                        prim.TreeSpecies = (Tree)0;

                        int size = block.Data[i++];
                        prim.ScratchPad = new byte[size];
                        Buffer.BlockCopy(block.Data, i, prim.ScratchPad, 0, size);
                        i += size;
                    }

                    // Floating text
                    if ((flags & CompressedFlags.HasText) != 0)
                    {
                        string text = String.Empty;
                        while (block.Data[i] != 0)
                        {
                            text += (char)block.Data[i];
                            i++;
                        }
                        i++;

                        // Floating text
                        prim.Text = text;

                        // Text color
                        prim.TextColor = new Color4(block.Data, i, false);
                        i += 4;
                    }
                    else
                    {
                        prim.Text = String.Empty;
                    }

                    // Media URL
                    if ((flags & CompressedFlags.MediaURL) != 0)
                    {
                        string text = String.Empty;
                        while (block.Data[i] != 0)
                        {
                            text += (char)block.Data[i];
                            i++;
                        }
                        i++;

                        prim.MediaURL = text;
                    }

                    // Particle system
                    if ((flags & CompressedFlags.HasParticles) != 0)
                    {
                        prim.ParticleSys = new Primitive.ParticleSystem(block.Data, i);
                        i += 86;
                    }

                    // Extra parameters
                    i += prim.SetExtraParamsFromBytes(block.Data, i);

                    //Sound data
                    if ((flags & CompressedFlags.HasSound) != 0)
                    {
                        prim.Sound = new UUID(block.Data, i);
                        i += 16;

                        prim.SoundGain = Utils.BytesToFloat(block.Data, i);
                        i += 4;
                        prim.SoundFlags = (SoundFlags)block.Data[i++];
                        prim.SoundRadius = Utils.BytesToFloat(block.Data, i);
                        i += 4;
                    }

                    // Name values
                    if ((flags & CompressedFlags.HasNameValues) != 0)
                    {
                        string text = String.Empty;
                        while (block.Data[i] != 0)
                        {
                            text += (char)block.Data[i];
                            i++;
                        }
                        i++;

                        // Parse the name values
                        if (text.Length > 0)
                        {
                            string[] lines = text.Split('\n');
                            prim.NameValues = new NameValue[lines.Length];

                            for (int j = 0; j < lines.Length; j++)
                            {
                                if (!String.IsNullOrEmpty(lines[j]))
                                {
                                    NameValue nv = new NameValue(lines[j]);
                                    prim.NameValues[j] = nv;
                                }
                            }
                        }
                    }

                    prim.PrimData.PathCurve = (PathCurve)block.Data[i++];
                    ushort pathBegin = Utils.BytesToUInt16(block.Data, i); i += 2;
                    prim.PrimData.PathBegin = Primitive.UnpackBeginCut(pathBegin);
                    ushort pathEnd = Utils.BytesToUInt16(block.Data, i); i += 2;
                    prim.PrimData.PathEnd = Primitive.UnpackEndCut(pathEnd);
                    prim.PrimData.PathScaleX = Primitive.UnpackPathScale(block.Data[i++]);
                    prim.PrimData.PathScaleY = Primitive.UnpackPathScale(block.Data[i++]);
                    prim.PrimData.PathShearX = Primitive.UnpackPathShear((sbyte)block.Data[i++]);
                    prim.PrimData.PathShearY = Primitive.UnpackPathShear((sbyte)block.Data[i++]);
                    prim.PrimData.PathTwist = Primitive.UnpackPathTwist((sbyte)block.Data[i++]);
                    prim.PrimData.PathTwistBegin = Primitive.UnpackPathTwist((sbyte)block.Data[i++]);
                    prim.PrimData.PathRadiusOffset = Primitive.UnpackPathTwist((sbyte)block.Data[i++]);
                    prim.PrimData.PathTaperX = Primitive.UnpackPathTaper((sbyte)block.Data[i++]);
                    prim.PrimData.PathTaperY = Primitive.UnpackPathTaper((sbyte)block.Data[i++]);
                    prim.PrimData.PathRevolutions = Primitive.UnpackPathRevolutions(block.Data[i++]);
                    prim.PrimData.PathSkew = Primitive.UnpackPathTwist((sbyte)block.Data[i++]);

                    prim.PrimData.profileCurve = block.Data[i++];
                    ushort profileBegin = Utils.BytesToUInt16(block.Data, i); i += 2;
                    prim.PrimData.ProfileBegin = Primitive.UnpackBeginCut(profileBegin);
                    ushort profileEnd = Utils.BytesToUInt16(block.Data, i); i += 2;
                    prim.PrimData.ProfileEnd = Primitive.UnpackEndCut(profileEnd);
                    ushort profileHollow = Utils.BytesToUInt16(block.Data, i); i += 2;
                    prim.PrimData.ProfileHollow = Primitive.UnpackProfileHollow(profileHollow);

                    // TextureEntry
                    int textureEntryLength = (int)Utils.BytesToUInt(block.Data, i);
                    i += 4;
                    prim.Textures = new Primitive.TextureEntry(block.Data, i, textureEntryLength);
                    i += textureEntryLength;

                    // Texture animation
                    if ((flags & CompressedFlags.TextureAnimation) != 0)
                    {
                        //int textureAnimLength = (int)Utils.BytesToUIntBig(block.Data, i);
                        i += 4;
                        prim.TextureAnim = new Primitive.TextureAnimation(block.Data, i);
                    }

                    #endregion

                    #region Fire Events

                    // Fire the appropriate callback
                    if ((flags & CompressedFlags.HasNameValues) != 0 && prim.ParentID != 0)
                        FireOnNewAttachment(prim, update.RegionData.RegionHandle,
                            update.RegionData.TimeDilation);
                    else
                        FireOnNewPrim(prim,update.RegionData.RegionHandle,
                            update.RegionData.TimeDilation);

                    #endregion
                }
                catch (IndexOutOfRangeException e)
                {
                    Logger.Log("Error decoding an ObjectUpdateCompressed packet", Helpers.LogLevel.Warning, e);
                    Logger.Log(block, Helpers.LogLevel.Warning);
                }
            }
			return packet;
        }

        /// <summary>
        /// Handles cached object update packets from the simulator
        /// </summary>
        /// <param name="packet">The packet containing the object data</param>
        /// <param name="simulator">The simulator sending the data</param>
        protected Packet CachedUpdateHandler(Packet packet, IPEndPoint IP)
        {
            if (ALWAYS_REQUEST_OBJECTS)
            {
                ObjectUpdateCachedPacket update = (ObjectUpdateCachedPacket)packet;
                List<uint> ids = new List<uint>(update.ObjectData.Length);

                // No object caching implemented yet, so request updates for all of these objects
                for (int i = 0; i < update.ObjectData.Length; i++)
                {
                    ids.Add(update.ObjectData[i].ID);
                }

                RequestObjects(ids);
            }
			return packet;
        }

        /// <summary>
        /// Handle KillObject packets from the simulator
        /// </summary>
        /// <param name="packet">The packet containing the object data</param>
        /// <param name="simulator">The simulator sending the data</param>
        protected Packet KillObjectHandler(Packet packet, IPEndPoint IP)
        {
            KillObjectPacket kill = (KillObjectPacket)packet;

            // Notify first, so that handler has a chance to get a
            // reference from the ObjectTracker to the object being killed
            for (int i = 0; i < kill.ObjectData.Length; i++)
                FireOnObjectKilled(kill.ObjectData[i].ID);

            lock (ObjectsPrimitives.Dictionary)
            {
                List<uint> removeAvatars = new List<uint>();
                List<uint> removePrims = new List<uint>();

                if (OBJECT_TRACKING)
                {
                    uint localID;
                    for (int i = 0; i < kill.ObjectData.Length; i++)
                    {
                        localID = kill.ObjectData[i].ID;

                        if (ObjectsPrimitives.Dictionary.ContainsKey(localID))
                            removePrims.Add(localID);

                        foreach (KeyValuePair<uint, Primitive> prim in ObjectsPrimitives.Dictionary)
                        {
                            if (prim.Value.ParentID == localID)
                            {
                                FireOnObjectKilled(prim.Key);
                                removePrims.Add(prim.Key);
                            }
                        }
                    }
                }

                if (AVATAR_TRACKING)
                {
                    lock (ObjectsAvatars.Dictionary)
                    {
                        uint localID;
                        for (int i = 0; i < kill.ObjectData.Length; i++)
                        {
                            localID = kill.ObjectData[i].ID;

                            if (ObjectsAvatars.Dictionary.ContainsKey(localID))
                                removeAvatars.Add(localID);

                            List<uint> rootPrims = new List<uint>();

                            foreach (KeyValuePair<uint, Primitive> prim in ObjectsPrimitives.Dictionary)
                            {
                                if (prim.Value.ParentID == localID)
                                {
                                    FireOnObjectKilled(prim.Key);
                                    removePrims.Add(prim.Key);
                                    rootPrims.Add(prim.Key);
                                }
                            }

                            foreach (KeyValuePair<uint, Primitive> prim in ObjectsPrimitives.Dictionary)
                            {
                                if (rootPrims.Contains(prim.Value.ParentID))
                                {
                                    FireOnObjectKilled(prim.Key);
                                    removePrims.Add(prim.Key);
                                }
                            }
                        }

                        //Do the actual removing outside of the loops but still inside the lock.
                        //This safely prevents the collection from being modified during a loop.
                        foreach (uint removeID in removeAvatars)
                            ObjectsAvatars.Dictionary.Remove(removeID);
                    }
                }

                foreach (uint removeID in removePrims)
                    ObjectsPrimitives.Dictionary.Remove(removeID);
            }
			return packet;
        }

        protected Packet ObjectPropertiesHandler(Packet p, IPEndPoint IP)
        {
            ObjectPropertiesPacket op = (ObjectPropertiesPacket)p;
            ObjectPropertiesPacket.ObjectDataBlock[] datablocks = op.ObjectData;

            for (int i = 0; i < datablocks.Length; ++i)
            {
                ObjectPropertiesPacket.ObjectDataBlock objectData = datablocks[i];
                Primitive.ObjectProperties props = new Primitive.ObjectProperties();

                props.ObjectID = objectData.ObjectID;
                props.AggregatePerms = objectData.AggregatePerms;
                props.AggregatePermTextures = objectData.AggregatePermTextures;
                props.AggregatePermTexturesOwner = objectData.AggregatePermTexturesOwner;
                props.Permissions = new Permissions(objectData.BaseMask, objectData.EveryoneMask, objectData.GroupMask,
                    objectData.NextOwnerMask, objectData.OwnerMask);
                props.Category = (ObjectCategory)objectData.Category;
                props.CreationDate = Utils.UnixTimeToDateTime((uint)objectData.CreationDate);
                props.CreatorID = objectData.CreatorID;
                props.Description = Utils.BytesToString(objectData.Description);
                props.FolderID = objectData.FolderID;
                props.FromTaskID = objectData.FromTaskID;
                props.GroupID = objectData.GroupID;
                props.InventorySerial = objectData.InventorySerial;
                props.ItemID = objectData.ItemID;
                props.LastOwnerID = objectData.LastOwnerID;
                props.Name = Utils.BytesToString(objectData.Name);
                props.OwnerID = objectData.OwnerID;
                props.OwnershipCost = objectData.OwnershipCost;
                props.SalePrice = objectData.SalePrice;
                props.SaleType = (SaleType)objectData.SaleType;
                props.SitName = Utils.BytesToString(objectData.SitName);
                props.TouchName = Utils.BytesToString(objectData.TouchName);

                int numTextures = objectData.TextureID.Length / 16;
                props.TextureIDs = new UUID[numTextures];
                for (int j = 0; j < numTextures; ++j)
                    props.TextureIDs[j] = new UUID(objectData.TextureID, j * 16);

                if (OBJECT_TRACKING)
                {
                    Primitive findPrim = ObjectsPrimitives.Find(
                        delegate(Primitive prim) { return prim.ID == props.ObjectID; });

                    if (findPrim != null)
                    {
						
                        lock (ObjectsPrimitives.Dictionary)
                        {
                            if (ObjectsPrimitives.Dictionary.ContainsKey(findPrim.LocalID))
                                ObjectsPrimitives.Dictionary[findPrim.LocalID].Properties = props;
                        }
                    }
                }

                FireOnObjectProperties(props);
            }
			return p;
        }

        protected Packet ObjectPropertiesFamilyHandler(Packet p, IPEndPoint ip)
        {
            ObjectPropertiesFamilyPacket op = (ObjectPropertiesFamilyPacket)p;
            Primitive.ObjectProperties props = new Primitive.ObjectProperties();

            ReportType requestType = (ReportType)op.ObjectData.RequestFlags;

            props.ObjectID = op.ObjectData.ObjectID;
            props.Category = (ObjectCategory)op.ObjectData.Category;
            props.Description = Utils.BytesToString(op.ObjectData.Description);
            props.GroupID = op.ObjectData.GroupID;
            props.LastOwnerID = op.ObjectData.LastOwnerID;
            props.Name = Utils.BytesToString(op.ObjectData.Name);
            props.OwnerID = op.ObjectData.OwnerID;
            props.OwnershipCost = op.ObjectData.OwnershipCost;
            props.SalePrice = op.ObjectData.SalePrice;
            props.SaleType = (SaleType)op.ObjectData.SaleType;
            props.Permissions.BaseMask = (PermissionMask)op.ObjectData.BaseMask;
            props.Permissions.EveryoneMask = (PermissionMask)op.ObjectData.EveryoneMask;
            props.Permissions.GroupMask = (PermissionMask)op.ObjectData.GroupMask;
            props.Permissions.NextOwnerMask = (PermissionMask)op.ObjectData.NextOwnerMask;
            props.Permissions.OwnerMask = (PermissionMask)op.ObjectData.OwnerMask;

            if (OBJECT_TRACKING)
            {
                Primitive findPrim = ObjectsPrimitives.Find(
                        delegate(Primitive prim) { return prim.ID == op.ObjectData.ObjectID; });

                if (findPrim != null)
                {
                    lock (ObjectsPrimitives.Dictionary)
                    {
                        if (ObjectsPrimitives.Dictionary.ContainsKey(findPrim.LocalID))
                        {
                            if (ObjectsPrimitives.Dictionary[findPrim.LocalID].Properties == null)
                                ObjectsPrimitives.Dictionary[findPrim.LocalID].Properties = new Primitive.ObjectProperties();
                            ObjectsPrimitives.Dictionary[findPrim.LocalID].Properties.SetFamilyProperties(props);
                        }
                    }
                }
            }

            FireOnObjectPropertiesFamily(props, requestType);
			return p;
        }
#endregion
		
        #region Event Notification

        protected void FireOnObjectProperties(Primitive.ObjectProperties props)
        {
            if (OnObjectProperties != null)
            {
                try { OnObjectProperties(props); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        protected void FireOnObjectPropertiesFamily(Primitive.ObjectProperties props,
            ReportType type)
        {
            if (OnObjectPropertiesFamily != null)
            {
                try { OnObjectPropertiesFamily(props, type); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
			}
        }

        protected void FireOnObjectKilled(uint localid)
        {
            if (OnObjectKilled != null)
            {
                try { OnObjectKilled(localid); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        protected void FireOnNewPrim(Primitive prim, ulong RegionHandle, ushort TimeDilation)
        {
            if (OnNewPrim != null)
            {
                try { OnNewPrim(prim, RegionHandle, TimeDilation); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        protected void FireOnNewAttachment(Primitive prim, ulong RegionHandle, ushort TimeDilation)
        {
            if (OnNewAttachment != null)
            {
                try { OnNewAttachment(prim, RegionHandle, TimeDilation); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        protected void FireOnNewAvatar(Avatar avatar, ulong RegionHandle, ushort TimeDilation)
        {
            if (OnNewAvatar != null)
            {
                try { OnNewAvatar(avatar, RegionHandle, TimeDilation); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        protected void FireOnObjectUpdated(ObjectUpdate update, ulong RegionHandle, ushort TimeDilation)
        {
            if (OnObjectUpdated != null)
            {
                try { OnObjectUpdated(update, RegionHandle, TimeDilation); }
                catch (Exception e) { Logger.Log(e.Message, Helpers.LogLevel.Error, e); }
            }
        }

        #endregion
		
        #region Object Tracking Link

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="localID"></param>
        /// <param name="fullID"></param>
        /// <returns></returns>
        protected Primitive GetPrimitive(uint localID, UUID fullID)
        {
            if (OBJECT_TRACKING)
            {
                Primitive prim;

                if (ObjectsPrimitives.TryGetValue(localID, out prim))
                {
                    return prim;
                }
                else
                {
                    prim = new Primitive();
                    prim.LocalID = localID;
                    prim.ID = fullID;
                    lock (ObjectsPrimitives.Dictionary)
                        ObjectsPrimitives.Dictionary[localID] = prim;

                    return prim;
                }
            }
            else
            {
                return new Primitive();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="localID"></param>
        /// <param name="fullID"></param>
        /// <returns></returns>
        protected Avatar GetAvatar(uint localID, UUID fullID)
        {
            if (AVATAR_TRACKING)
            {
                Avatar avatar;

                if (ObjectsAvatars.TryGetValue(localID, out avatar))
                {
                    return avatar;
                }
                else
                {
                    avatar = new Avatar();
                    avatar.LocalID = localID;
                    avatar.ID = fullID;
                    lock (ObjectsAvatars.Dictionary)
                        ObjectsAvatars.Dictionary[localID] = avatar;

                    return avatar;
                }
            }
            else
            {
                return new Avatar();
            }
        }

        #endregion Object Tracking Link
	}
}
