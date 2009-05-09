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
    public class ProfileFunPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private ProfileFunFormGTK form;
        private PubComb.Aux_SharedInfo shared;
        private bool setData = false;
        private System.Timers.Timer RefreshDownloadsTimer = new System.Timers.Timer(5000.0);
        private Dictionary<ulong, string> regionNames = new Dictionary<ulong, string>();
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Profile Fun");
            form.readData();
            getProfile();
        }
        public ProfileFunPlugin(PubComb plug)
        {

            plugin = plug;
            form = new ProfileFunFormGTK(this);
            this.frame = plug.frame;
            shared = plugin.SharedInfo;
            this.proxy = plug.proxy;
            proxy.AddDelegate(PacketType.AvatarPropertiesReply, Direction.Incoming, new PacketDelegate(inAvatar));
            RefreshDownloadsTimer.Elapsed += new System.Timers.ElapsedEventHandler(RefreshDownloadsTimer_Elapsed);
            RefreshDownloadsTimer.Start();
            proxy.AddDelegate(PacketType.AgentMovementComplete, Direction.Incoming, delegate(Packet packet, IPEndPoint sim)
            {
                shared.RegionHandle = ((AgentMovementCompletePacket)packet).Data.RegionHandle;
                return packet;
            });
            proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, delegate(Packet packet, IPEndPoint sim)
            {
                AgentUpdatePacket.AgentDataBlock p = ((AgentUpdatePacket)packet).AgentData;
                shared.CameraPosition = p.CameraCenter;
                shared.CameraAtAxis = p.CameraAtAxis;
                shared.CameraLeftAxis = p.CameraLeftAxis;
                shared.CameraUpAxis = p.CameraUpAxis;
                shared.Far = p.Far;
                shared.ip = sim.Address;
                shared.port = sim.Port;
                return packet;
            });
        }
        public void getProfile()
        {
            AvatarPropertiesRequestPacket arp = new AvatarPropertiesRequestPacket();
            arp.AgentData = new AvatarPropertiesRequestPacket.AgentDataBlock();
            arp.AgentData.AgentID = frame.AgentID;
            arp.AgentData.AvatarID = frame.AgentID;
            arp.AgentData.SessionID = frame.SessionID;
            proxy.InjectPacket(arp, Direction.Outgoing);
        }
        private Packet inAvatar(Packet packet, IPEndPoint sim)
        {
            AvatarPropertiesReplyPacket avr = (AvatarPropertiesReplyPacket)packet;
            if (avr.AgentData.AvatarID == frame.AgentID)
            {
                setData = true;
                form.setStuff(avr.PropertiesData.ImageID.ToString(),
                    avr.PropertiesData.FLImageID.ToString(),
                    Utils.BytesToString(avr.PropertiesData.AboutText),
                    Utils.BytesToString(avr.PropertiesData.FLAboutText),
                    Utils.BytesToString(avr.PropertiesData.ProfileURL));
            }
            return packet;
        }
        public void updateProfile(string img, string rlimg, string about, string rlabout, string web)
        {
            AvatarPropertiesUpdatePacket up = new AvatarPropertiesUpdatePacket();
            up.AgentData = new AvatarPropertiesUpdatePacket.AgentDataBlock();
            up.AgentData.AgentID = frame.AgentID;
            up.AgentData.SessionID = frame.SessionID;
            up.PropertiesData = new AvatarPropertiesUpdatePacket.PropertiesDataBlock();
            up.PropertiesData.ImageID = UUID.Parse(img);
            up.PropertiesData.FLImageID = UUID.Parse(rlimg);
            up.PropertiesData.AboutText = Utils.StringToBytes(about);
            up.PropertiesData.FLAboutText = Utils.StringToBytes(rlabout);
            up.PropertiesData.ProfileURL = Utils.StringToBytes(web);
            up.PropertiesData.MaturePublish = false;
            up.PropertiesData.AllowPublish = true;
            proxy.InjectPacket(up, Direction.Outgoing);
        }
        private void RefreshDownloadsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (frame.proxy.KnownCaps.Count > 2)
            {
                if (form.autoupdate())
                {
                    //buiding req
                    if (setData)
                    {
                        if (!regionNames.ContainsKey(shared.RegionHandle))
                        {
                            ulong handle = shared.RegionHandle;
                            ushort MapX = (ushort)((uint)(handle >> 32) / 256);
                            ushort MapY = (ushort)((uint)(handle & 0x00000000FFFFFFFF) / 256);
                            MapBlockRequestPacket MapBlockRequest = new MapBlockRequestPacket();
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

                            System.Timers.Timer myTimer2 = new System.Timers.Timer(20000);

                            PacketDelegate replyCallback = delegate(Packet np, IPEndPoint ss)
                            {
                                MapBlockReplyPacket reply = (MapBlockReplyPacket)np;
                                foreach (MapBlockReplyPacket.DataBlock block in reply.Data)
                                {
                                    if ((block.X == MapX) && (block.Y == MapY))
                                    {

                                        string regionName = Utils.BytesToString(block.Name);
                                        ushort regionglobalx = MapX;
                                        ushort regionglobaly = MapY;
                                        if (!regionNames.ContainsKey(shared.RegionHandle))
                                        {
                                            regionNames.Add(shared.RegionHandle, regionName);
                                        }
                                        form.updateWeb(form.getBase() + "secondlife://" + regionName.Replace(" ", "%20") + "/" + shared.CameraPosition.X.ToString() + "/" + shared.CameraPosition.Y.ToString() + "/" + shared.CameraPosition.Z.ToString());

                                    }
                                }
                                return null;
                            };

                            System.Timers.ElapsedEventHandler timerCallback = delegate(object asender, System.Timers.ElapsedEventArgs ee)
                            {

                                proxy.RemoveDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                                myTimer2.Stop();
                            };

                            proxy.AddDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                            myTimer2.Elapsed += timerCallback;
                            myTimer2.Start();



                            proxy.InjectPacket(MapBlockRequest, Direction.Outgoing);


                        }

                        else
                        {
                            form.updateWeb(form.getBase() + "secondlife://" + this.regionNames[this.shared.RegionHandle].Replace(" ", "%20") + "/" + shared.CameraPosition.X.ToString() + "/" + shared.CameraPosition.Y.ToString() + "/" + shared.CameraPosition.Z.ToString());

                        }
                    }
                    else
                    {
                        getProfile();
                    }
                }
            }



        }





    }

}