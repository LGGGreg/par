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

    public class PubComb : ProxyPlugin
    {
        public ProxyFrame frame;
        public Proxy proxy;

        public Aux_SharedInfo SharedInfo;

        
        public TabItem tabform;
        private Thread tabformthread;
        List<GTabPlug> plugins = new List<GTabPlug>();

        public PubComb(ProxyFrame frame)
        {
            this.frame = frame;
            this.proxy = frame.proxy;
            SharedInfo = new Aux_SharedInfo(this);

            //plugins.Add(new SpamBlocker(this));
            //plugins.Add(new ClientDetection(this));
            plugins.Add(new LyraPlugin(this));
            plugins.Add(new HandicapPlugin(this));
            plugins.Add(new PennyPlugin(this));
            //plugins.Add(new PAnim(this));
            //plugins.Add(new DisableCapsPlugin(this));
            //plugins.Add(new IMLocatePlugin(this));
            plugins.Add(new LeetPlugin(this));
            //plugins.Add(new CinderellaPlugin(this));
            plugins.Add(new ProfileFunPlugin(this));
            plugins.Add(new SitAnywherePlugin(this));
            plugins.Add(new HighPlugin(this));
            plugins.Add(new RadarChatPlugin(this));
            plugins.Add(new FileProtectPlugin(this));
            //plugins.Add(new InvFunPlugin(this));
            plugins.Add(new RainbowParticlesPlugin(this));
            //causing problems :(
            //plugins.Add(new coin(this));
            plugins.Add(new RetreatPlugin(this));
            //plugins.Add(new AwesomeSauce(this));
            //plugins.Add(new ProTextPlug(this));
            //plugins.Add(new ViewerEffectLogPlugin(this));
            plugins.Add(new AvatarTracker(this));
            //plugins.Add(new CliIntPlugin(this));
            plugins.Add(new SitBlockPlugin(this));

            tabformthread = new Thread(new ThreadStart(delegate()
            {
                tabform = new TabItem(this);
                Application.Run(tabform);
            }));
            tabformthread.SetApartmentState(ApartmentState.STA);
            tabformthread.Start();

            
        }
        public void addMahTabs()
        {

            foreach (GTabPlug p in plugins)
              p.LoadNow();
            
        }

        public override void Init()
        {
            SayToUser("Mega plugin loaded");
        }

        public class Aux_SharedInfo
        {
            public String AgentName;
            public ProxyFrame frame;
            public IPAddress ip;
            public byte[][] rainbow;
            public int port;
            public ulong RegionHandle = 0;
            public Vector3 AvPosition = new Vector3();
            public Vector3 CameraPosition = new Vector3();
            public Vector3 CameraAtAxis;
            public Vector3 CameraLeftAxis;
            public Vector3 CameraUpAxis;
            public Dictionary<string, Aux_Simulator> Aux_Simulators = new Dictionary<string, Aux_Simulator>();
            public Dictionary<UUID, String> key2name = new Dictionary<UUID, string>();
            public Dictionary<String, UUID> name2key = new Dictionary<string, UUID>();
            public float Far;

            public Aux_SharedInfo(PubComb plugin)
            {
                this.frame = plugin.frame;
            }
        }
        public class Aux_Simulator
        {
            public string IP;
            public List<Aux_CoarseLocation> CoarseLocations = new List<Aux_CoarseLocation>();
            public Dictionary<UUID, Aux_Avatar> Avatars = new Dictionary<UUID, Aux_Avatar>();

            public Aux_Simulator(string ip)
            {
                IP = ip;
            }

            public Aux_Avatar AvatarByLocalID(UInt32 localid)
            {
                foreach (KeyValuePair<UUID, Aux_Avatar> UUIDandAvatar in Avatars)
                {
                    if (UUIDandAvatar.Value.LocalID == localid)
                    {
                        return UUIDandAvatar.Value;
                    }
                }
                return null;
            }
        }

        public class Aux_CoarseLocation
        {
            public int X;
            public int Y;
            public int Z;
            public UUID UUID;

            public Aux_CoarseLocation(int x, int y, UUID uuid)
            {
                X = x;
                Y = y;
                UUID = uuid;
            }
        }

        public class Aux_Avatar
        {
            public string sim_IP;
            public UUID UUID;
            public uint LocalID;
            public string Name;
            public Vector3 Position;

            public Aux_Avatar(UUID uuid, string sim, uint localid, string name, Vector3 position)
            {
                sim_IP = sim;
                UUID = uuid;
                LocalID = localid;
                Name = name;
                Position = position;
            }
        }
        public void sendDialog(UUID what, String msg, int chan)
        {
            ScriptDialogReplyPacket sdrp = new ScriptDialogReplyPacket();
            sdrp.Type = PacketType.ScriptDialogReply;
            sdrp.AgentData = new ScriptDialogReplyPacket.AgentDataBlock();
            sdrp.AgentData.AgentID = frame.AgentID;
            sdrp.AgentData.SessionID = frame.SessionID;
            sdrp.Data = new ScriptDialogReplyPacket.DataBlock();
            sdrp.Data.ButtonIndex = 1;
            sdrp.Data.ButtonLabel = Utils.StringToBytes(msg);
            sdrp.Data.ChatChannel = (int)chan;
            sdrp.Data.ObjectID = what;
            proxy.InjectPacket(sdrp, Direction.Outgoing);
        }
        public void RequestNameFromKey(UUID key)
        {
            UUIDNameRequestPacket nrp = new UUIDNameRequestPacket();
            nrp.Type = PacketType.UUIDNameRequest;
            nrp.UUIDNameBlock = new UUIDNameRequestPacket.UUIDNameBlockBlock[1];
            nrp.UUIDNameBlock[0] = new UUIDNameRequestPacket.UUIDNameBlockBlock();
            nrp.UUIDNameBlock[0].ID = key;
            proxy.InjectPacket(nrp, Direction.Outgoing);

        }
        public void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);
            proxy.InjectPacket(packet, Direction.Incoming);
        }
        public void SendAgentUserAlert(string message)
        {
            frame.SendUserAlert(message);
            

        }
        
        public void SayToUser(string message)
        {
            frame.SayToUser(message);
        }
    }
}