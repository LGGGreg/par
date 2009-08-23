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
    public class GregIm
    {
        public string fromname;
        public string ownername;
        public UUID ownerkey;
        public UUID regionkey;
        public ushort regionglobalx;
        public ushort regionglobaly;
        public string regionName;
        public Vector3 regionpos;
        public string slurl;
        public GregIm()
        {
            fromname = ownername = slurl = regionName = "";
        }
        public ImprovedInstantMessagePacket p;
    }
    public class IMLocatePlugin : GTabPlug
    {
        public ProxyFrame frame;
        private Proxy proxy;
        private PubComb plugin;
        private IMHistForm1 form;
        //private string brand;
        private bool enabled = true;
        private List<uint> recSeq = new List<uint>();
        //private ImprovedInstantMessagePacket lastIM = null;
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "IMHistory");
            form.readData();
        }
        public IMLocatePlugin(PubComb plug)
        {

            form = new IMHistForm1(this);
            //    Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
            plugin = plug;
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            //this.brand = "IMLocate";
            proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Incoming, new PacketDelegate(IMs));

        }
        public Packet IMs(Packet p, IPEndPoint sim)
        {
            if (enabled)
            {
                GregIm g = new GregIm();
                ImprovedInstantMessagePacket imm = (ImprovedInstantMessagePacket)p;

                g.fromname = Utils.BytesToString(imm.MessageBlock.FromAgentName);
                g.ownerkey = imm.AgentData.AgentID;
                g.regionkey = imm.MessageBlock.RegionID;
                g.regionpos = imm.MessageBlock.Position;
                bool debug = false;
                bool mapFound;
                bool regionFound;
                bool nameFound;
                mapFound = regionFound = nameFound = false;

                if (g.regionkey != UUID.Zero && imm.MessageBlock.Dialog == (byte)InstantMessageDialog.MessageFromObject)
                {

                    /*if ((imm.MessageBlock.Dialog == 0 && imm.MessageBlock.Offline != 1) || g.ownerkey == frame.AgentID)
                    {
                        imm.MessageBlock.FromAgentName = Utils.StringToBytes(g.fromname.ToString() + " @ " + g.regionpos.ToString());
                        return imm;
                    }*/
                    g.p = imm;
                    if (debug) frame.SayToUser("region key was not zero..:");
                    // request region name
                    RegionHandleRequestPacket rhp = new RegionHandleRequestPacket();
                    rhp.RequestBlock.RegionID = g.regionkey;


                    System.Timers.Timer mygregTimer = new System.Timers.Timer(30000);

                    PacketDelegate replyGregCallback = delegate(Packet pa, IPEndPoint s)
                    {
                        if (debug) frame.SayToUser("got the region handle...");
                        if (!regionFound)
                        {
                            regionFound = true;
                            RegionIDAndHandleReplyPacket rid = (RegionIDAndHandleReplyPacket)pa;
                            ulong handle = rid.ReplyBlock.RegionHandle;
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
                                if (debug) frame.SayToUser("got the map..:");
                                if (!mapFound)
                                {
                                    mapFound = true;
                                    MapBlockReplyPacket reply = (MapBlockReplyPacket)np;
                                    foreach (MapBlockReplyPacket.DataBlock block in reply.Data)
                                    {
                                        if ((block.X == MapX) && (block.Y == MapY))
                                        {

                                            g.regionName = Utils.BytesToString(block.Name);
                                            g.regionglobalx = MapX;
                                            g.regionglobaly = MapY;
                                            g.slurl = "secondlife://" + g.regionName.Replace(" ", "%20") + "/" + g.regionpos.X.ToString() + "/" + g.regionpos.Y.ToString() + "/" + g.regionpos.Z.ToString();
                                            System.Timers.Timer timer = new System.Timers.Timer(10000);
                                            
                                            PacketDelegate replyCallback2 = delegate(Packet replypacket, IPEndPoint blarg)
                                            {
                                                if (debug) frame.SayToUser("got the name");
                                                
                                                    UUIDNameReplyPacket ureply = (UUIDNameReplyPacket)replypacket;
                                                    foreach (UUIDNameReplyPacket.UUIDNameBlockBlock bblock in ureply.UUIDNameBlock)
                                                    {
                                                        if (bblock.ID == g.ownerkey)
                                                        {
                                                            if (!nameFound)
                                                            {
                                                                nameFound = true;

                                                                string firstname = Utils.BytesToString(bblock.FirstName);
                                                                string lastname = Utils.BytesToString(bblock.LastName);
                                                                g.ownername = firstname + " " + lastname;
                                                                g.p.MessageBlock.FromAgentName = Utils.StringToBytes(g.ownername + "'s " + g.fromname + " @ " + g.slurl);
                                                                form.addListItem(g.ownerkey.ToString() + " \t" + g.ownername + " \t" + g.slurl + " \t" + g.fromname + " \t" + Utils.BytesToString(g.p.MessageBlock.Message));
                                                                imm = g.p;
                                                                proxy.InjectPacket(g.p, Direction.Incoming);
                                                            }
                                                            return replypacket;
                                                        }
                                                    }
                                                
                                                return replypacket;
                                            };
                                            proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, replyCallback2);
                                            timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
                                            {

                                                proxy.RemoveDelegate(PacketType.UUIDNameReply, Direction.Incoming, replyCallback2);
                                                timer.Stop();
                                                //proxy.InjectPacket(p, Direction.Incoming);
                                            };
                                            UUIDNameRequestPacket request = new UUIDNameRequestPacket();
                                            request.UUIDNameBlock = new UUIDNameRequestPacket.UUIDNameBlockBlock[1];
                                            request.UUIDNameBlock[0] = new UUIDNameRequestPacket.UUIDNameBlockBlock();
                                            request.UUIDNameBlock[0].ID = g.ownerkey;
                                            request.Header.Reliable = true;
                                            proxy.InjectPacket(request, Direction.Outgoing);
                                            timer.Start();
                                        }
                                    }
                                }
                                return np;
                            };

                            System.Timers.ElapsedEventHandler timerCallback = delegate(object sender, System.Timers.ElapsedEventArgs e)
                            {

                                proxy.RemoveDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                                myTimer2.Stop();
                                //proxy.InjectPacket(p, Direction.Incoming);
                            };

                            proxy.AddDelegate(PacketType.MapBlockReply, Direction.Incoming, replyCallback);
                            myTimer2.Elapsed += timerCallback;
                            myTimer2.Start();

                            proxy.InjectPacket(MapBlockRequest, Direction.Outgoing);
                        }
                        return pa;
                    };

                    System.Timers.ElapsedEventHandler timerGregCallback = delegate(object sender, System.Timers.ElapsedEventArgs e)
                    {

                        proxy.RemoveDelegate(PacketType.RegionIDAndHandleReply, Direction.Incoming, replyGregCallback);
                        mygregTimer.Stop();
                        //proxy.InjectPacket(p, Direction.Incoming);
                    };

                    proxy.AddDelegate(PacketType.RegionIDAndHandleReply, Direction.Incoming, replyGregCallback);
                    mygregTimer.Elapsed += timerGregCallback;
                    mygregTimer.Start();

                    proxy.InjectPacket(rhp, Direction.Outgoing);



                    //----------------------
                    return null;
                }

            }


            return p;
        }

        public void setEnabled(bool b)
        {

            enabled = b;
            string s = "IMLocate is ";
            if (b) s += "Enabled"; else s += "Disabled";
            frame.SendUserAlert(s);
        }

   }
}