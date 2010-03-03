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
namespace PubCombN
{
    public class SitAnywherePlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private SitAnywhereForm1 form;
        private PubComb.Aux_SharedInfo shared;
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "SitAnywhere");
        }
        public tabInfo getInfo()
        {
            return new tabInfo(form, "SitAnywhere");
        }
        public SitAnywherePlugin(PubComb plug)
        {
            
            plugin = plug;
            shared = plug.SharedInfo;
            form = new SitAnywhereForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.frame.AddCommand("/sit", new ProxyFrame.CommandDelegate(this.CmdSit));
        
        }
        private void CmdSit(string[] words)
        {
            SitNow();

        }
        public void SitNow()
        {
            plugin.SendUserAlert("Activated Sit");
            
            AgentUpdatePacket a = new AgentUpdatePacket();
            a.Type = PacketType.AgentUpdate;
            a.AgentData = new AgentUpdatePacket.AgentDataBlock();
            a.AgentData.AgentID = frame.AgentID;
            a.AgentData.BodyRotation = Quaternion.Identity;
            a.AgentData.CameraAtAxis = shared.CameraAtAxis;
            a.AgentData.CameraCenter = shared.CameraPosition;
            a.AgentData.CameraLeftAxis = shared.CameraLeftAxis;
            a.AgentData.CameraUpAxis = shared.CameraUpAxis;
            a.AgentData.ControlFlags = 131072;
            a.AgentData.Far = shared.Far;
            a.AgentData.Flags = 0;
            a.AgentData.HeadRotation = Quaternion.Identity;
            a.AgentData.SessionID = frame.SessionID;
            a.AgentData.State = 0;

            proxy.InjectPacket(a, Direction.Outgoing);
        }
    }

}