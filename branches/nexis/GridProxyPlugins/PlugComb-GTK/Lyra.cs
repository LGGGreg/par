/*
 * Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back) & Day Oh
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
    public class LyraPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        private LyraFormGTK form;
        private string brand;
        private bool pass = true;
        public void LoadNow(ref TabItemGTK tabform)
        {
			if(tabform!=null)
			{
	            tabform.addATab(form, "Lyra");
	            form.readData();
			} else {
				Console.WriteLine("[LYRA] BUG:  tabform is NULL.  Cannot add tab.");
			}
        }
        public LyraPlugin(PubComb plug)
        {
            //formthread = new Thread(new ThreadStart(delegate()
            //{
            //    form = new LyraFormGTK(this);
            //    Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
            plugin = plug;
            form = new LyraFormGTK(this);
            //plug.tabform.addATab(form, "LYRA");
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.brand = "Lyra";
            proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(SimChat));
            proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, new PacketDelegate(Age));
        }
        public void show(bool p)
        {
            
            if (p)
            {
                SendUserAlert("Avatar Phantom Lock Disabled");
            }
            else SendUserAlert("Avatar Phantom Lock Enabled");
            
            form.setCheck(!pass);
        }
        public Packet SimChat(Packet p, IPEndPoint s)
        {
            ChatFromViewerPacket ch = (ChatFromViewerPacket)p;
            if(Utils.BytesToString(ch.ChatData.Message).ToLower().Contains(form.getbox().ToLower()))
            {
                if(pass)pass=false;
                else pass=true;
                show(pass);
                return null;
                

                
            }
            return p;
        }
        public Packet Age(Packet p, IPEndPoint s)
        {
            if (form.getCheck() == true) return null;
            return p;
        }
        
        private void SayToUser(string message)
        {

            ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
            packet.ChatData.FromName = Utils.StringToBytes(this.brand);
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
        private void SendUserDialog(string first, string last, string objectName, string message, string[] buttons)
        {
            Random rand = new Random();
            ScriptDialogPacket packet = new ScriptDialogPacket();
            packet.Data.ObjectID = UUID.Random();
            packet.Data.FirstName = Utils.StringToBytes(first);
            packet.Data.LastName = Utils.StringToBytes(last);
            packet.Data.ObjectName = Utils.StringToBytes(objectName);
            packet.Data.Message = Utils.StringToBytes(message);
            packet.Data.ChatChannel = (byte)rand.Next(1000, 10000);
            packet.Data.ImageID = UUID.Zero;

            ScriptDialogPacket.ButtonsBlock[] temp = new ScriptDialogPacket.ButtonsBlock[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                temp[i] = new ScriptDialogPacket.ButtonsBlock();
                temp[i].ButtonLabel = Utils.StringToBytes(buttons[i]);
            }
            packet.Buttons = temp;
            proxy.InjectPacket(packet, Direction.Incoming);
        }
    }

}