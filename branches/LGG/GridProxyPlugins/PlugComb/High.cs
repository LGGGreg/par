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
    public class HighPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private HighForm1 form;
        private PubComb.Aux_SharedInfo shared;
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "SoHigh");
            form.readData();
        }
        public HighPlugin(PubComb plug)
        {

            plugin = plug;
            shared = plug.SharedInfo;
            form = new HighForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.frame.AddCommand("/high", new ProxyFrame.CommandDelegate(this.CmdHigh));
            this.proxy.AddDelegate(PacketType.ObjectSelect, Direction.Outgoing, new PacketDelegate(this.ToHigh));
            this.proxy.AddDelegate(PacketType.ObjectDeselect, Direction.Outgoing, new PacketDelegate(this.ToHigh));
        }
        private void CmdHigh(string[] words)
        {
            if (words.Length >= 2)
            {
                try
                {
                    doHigh(int.Parse(words[1]));
                }
                catch
                {
                    string s = "";
                    foreach (string w in words)
                    {
                        s += "|" + w;
                    }
                    plugin.SayToUser("value not a number+"+s);
                    plugin.SayToUser("Useage: /high 50000");
                    

                }
            }
            else
                doHigh((int)form.getHigh());

        }
        private Packet ToHigh(Packet p, IPEndPoint s)
        {
            if (shared.Position.Z > 4096 + shared.Far) return null;
            return p;
        }
        public void doHigh(int c)
        {
            if (form.getplat())
            {
                platHigh(c);
            }
            System.Timers.Timer myTimer = new System.Timers.Timer(400);
            System.Timers.ElapsedEventHandler timerCallback = delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                if (form.getTp())
                {
                    tpHigh(c);
                    myTimer.Stop();
                }
            };
            myTimer.Elapsed += timerCallback;
            myTimer.Start();
            
            
        }
        public void tpHigh(int c)
        {
            Vector3 where = new Vector3(shared.Position.X, shared.Position.Y, (float)c);
            TeleportLocationRequestPacket tp = new TeleportLocationRequestPacket();
            tp.AgentData = new TeleportLocationRequestPacket.AgentDataBlock();
            tp.AgentData.AgentID = frame.AgentID;
            tp.AgentData.SessionID = frame.SessionID;
            tp.Info = new TeleportLocationRequestPacket.InfoBlock();
            tp.Info.RegionHandle = shared.RegionHandle;
            tp.Info.Position = where;
            tp.Info.LookAt = where;
            proxy.InjectPacket(tp, Direction.Outgoing);
        }
        public void platHigh(int c)
        {
            Vector3 where = new Vector3(shared.Position.X, shared.Position.Y, (float)(c-5));
            System.Timers.Timer myTimer = new System.Timers.Timer(10000);
            PacketDelegate replyCallback = delegate(Packet p, IPEndPoint s)
            {
                return null;
            };
            System.Timers.ElapsedEventHandler timerCallback = delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                proxy.RemoveDelegate(PacketType.ObjectSelect, Direction.Outgoing, replyCallback);
                proxy.RemoveDelegate(PacketType.ObjectDeselect, Direction.Outgoing, replyCallback);
                myTimer.Stop();
            };



            proxy.AddDelegate(PacketType.ObjectDeselect, Direction.Outgoing, replyCallback);
            proxy.AddDelegate(PacketType.ObjectSelect, Direction.Outgoing, replyCallback);
            myTimer.Elapsed += timerCallback;
            myTimer.Start();

            
            ObjectAddPacket a = new ObjectAddPacket();
            a.AgentData = new ObjectAddPacket.AgentDataBlock();
            a.AgentData.AgentID = frame.AgentID;
            a.AgentData.GroupID = UUID.Zero;
            a.AgentData.SessionID = frame.SessionID;
            a.ObjectData = new ObjectAddPacket.ObjectDataBlock();
            a.ObjectData.PCode = 9;
            a.ObjectData.Material = 4;
            a.ObjectData.AddFlags = 2;
            a.ObjectData.PathCurve = 0x10;
            a.ObjectData.ProfileCurve = 1;
            a.ObjectData.PathBegin = 0;
            a.ObjectData.PathEnd = 0;
            a.ObjectData.PathScaleX = 100;
            a.ObjectData.PathScaleY = 100;
            a.ObjectData.PathShearX = 0;
            a.ObjectData.PathShearY = 0;
            a.ObjectData.PathTwist = 0;
            a.ObjectData.PathTwistBegin = 0;
            a.ObjectData.PathRadiusOffset = 0;
            a.ObjectData.PathTaperX = 0;
            a.ObjectData.PathTaperY = 0;
            a.ObjectData.PathRevolutions = 0;
            a.ObjectData.PathSkew = 0;
            a.ObjectData.ProfileBegin = 0;
            a.ObjectData.ProfileEnd = 0;
            a.ObjectData.ProfileHollow = 0;
            a.ObjectData.BypassRaycast = 1;
            a.ObjectData.RayStart = where;
            a.ObjectData.RayEnd = where;
            a.ObjectData.RayTargetID = UUID.Zero;
            a.ObjectData.RayEndIsIntersection = 0;
            a.ObjectData.Scale = new Vector3((float)1,(float)1,(float)1);
            a.ObjectData.Rotation = Quaternion.Identity;
            a.ObjectData.State = 0;
            a.Header.Reliable = true;

            proxy.InjectPacket(a, Direction.Outgoing);

            a.ObjectData.RayEnd = a.ObjectData.RayStart = new Vector3(where.X - 5, where.Y - 5, where.Z);
            a.ObjectData.Scale = new Vector3((float)10, (float)10, (float).4);
            proxy.InjectPacket(a, Direction.Outgoing);

            a.ObjectData.RayEnd = a.ObjectData.RayStart = new Vector3(where.X - 5, where.Y + 5, where.Z);
            proxy.InjectPacket(a, Direction.Outgoing);

            a.ObjectData.RayEnd = a.ObjectData.RayStart = new Vector3(where.X + 5, where.Y + 5, where.Z);
            proxy.InjectPacket(a, Direction.Outgoing);

            a.ObjectData.RayEnd = a.ObjectData.RayStart = new Vector3(where.X + 5, where.Y - 5, where.Z);
            proxy.InjectPacket(a, Direction.Outgoing);

        }

    }
}