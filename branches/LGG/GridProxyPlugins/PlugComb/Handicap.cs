/*
 * Copyright (c) 2009, Day Oh & Gregory Hendrickson (LordGregGreg Back)
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
    public class HandicapPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        private Proxy proxy;
        public static bool Enabled = true;
        private HandiForm1 form;
        public string indicator, trigger;
        public string brand = "handi";

        public void LoadNow()
        {
            plugin.tabform.addATab(form, "HandiCap");
            form.readData();
        }
        public HandicapPlugin(PubComb plug)
        {
            //formthread = new Thread(new ThreadStart(delegate()
            //{
                form = new HandiForm1(this);
            //    Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
                plugin = plug;
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            //plug.tabform.addATab(form, "Handicap");
            this.proxy.AddDelegate(PacketType.GenericMessage, Direction.Outgoing, new PacketDelegate(OutAutoPilot));
            this.proxy.AddDelegate(PacketType.GenericMessage, Direction.Incoming, new PacketDelegate(OutAutoPilot));
            this.proxy.AddDelegate(PacketType.ScriptDialogReply, Direction.Outgoing, new PacketDelegate(OutDialogFromViewer));
            this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
            this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Outgoing, new PacketDelegate(SendingIM));
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
        private void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
        private Packet OutDialogFromViewer(Packet packet, IPEndPoint sim)
        {
            ScriptDialogReplyPacket DialogFromViewer = (ScriptDialogReplyPacket)packet;
            if(handeledViewerOutput(Utils.BytesToString(DialogFromViewer.Data.ButtonLabel).Trim().ToLower())=="die")
                return null;
            
            return packet;
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
        private Packet SendingIM(Packet packet, IPEndPoint sim)
        {
            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)packet;
            string message = Utils.BytesToString(im.MessageBlock.Message);
            if (handeledViewerOutput(message) == "die")
            {
                return null;
            }
            return im;
        }
        
        public string handeledViewerOutput(string mssage)
        {

            if (mssage.ToLower().Contains(this.brand.ToLower()+"-on"))
            {
                Enabled = true;
                form.setBox(Enabled);

                SendUserAlert("Handicap TP Enabled");
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower()+"-off"))
            {
                Enabled = false;

                SendUserAlert("Handicap TP Disabled");
                form.setBox(Enabled);
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-help"))
            {
               SayToUser("Thanks for using the handicap teleportation system :).\n"+                   "To make it work, you must first go to the about menu at the top of your screen\n"+
                        "If you dont see it, press ctrl+alt+shift+D to enable it.\n"+
                        "Go to \"Advanced\" -> \"UI\" and then click \"Double Click Auto Pilot\""+
                        "\nNow just type in chat \"handi-on\" and double click the ground :)");
               return "die";

            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-about"))
            {
                    SayToUser("This program was made by LordGregGreg for the purpose of allowing\nAvatars who can't walk to get around better.");
                    SayToUser("Special Thanks to \"Philip Linden\" (yeah, thats not his actual sl name)\n" +
                        "and the OpenMetaverse project, and all it's contributors.");
                    return "die";
                    
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower()+"-"))
            {
                SendUserDialog(this.brand+"", "Teleportation", "Messenger", "What do you want me to do?", new string[] { "handi-ON", "handi-OFF", "handi-HELP", "handi-ABOUT" });
                return "die";
            }

            return "go";
        }
        private Packet OutChatFromViewerHandler(Packet packet, IPEndPoint sim)
        {
            ChatFromViewerPacket ChatFromViewer = (ChatFromViewerPacket)packet;
            string message = Utils.BytesToString(ChatFromViewer.ChatData.Message);
            if (handeledViewerOutput(message) == "die")
            {
                return null;
            }
            return packet;

        }
        public void setEnabled(bool en)
        {
            Enabled = en;
            if (Enabled)
            {
                SendUserAlert("Handicap Enabled");
            }
            else
            {
                SendUserAlert("Handicap Disabled");
            }
        }
        public Packet OutAutoPilot(Packet packet , IPEndPoint sim)
        {
            GenericMessagePacket genMsg = (GenericMessagePacket)packet;
            if (form.getcheck())
            {
                if (Utils.BytesToString(genMsg.MethodData.Method).ToLower().Contains("autopilot"))
                {
                    {
                        try
                        {
                            uint GlobalX = uint.Parse(Utils.BytesToString(genMsg.ParamList[0].Parameter));
                            uint GlobalY = uint.Parse(Utils.BytesToString(genMsg.ParamList[1].Parameter));
                            form.addDebugInfo(genMsg.ParamList[2].Parameter.ToString());
                            form.addDebugInfo(Utils.BytesToHexString(genMsg.ParamList[2].Parameter,"Version43").Trim());
                            //float Z = float.Parse(Utils.BytesToString(genMsg.ParamList[2].Parameter).Trim()) + 2.0f;
                            //float Z = (float)(System.Convert.ToDouble(genMsg.ParamList[2].Parameter));
                            string before = System.Text.Encoding.GetEncoding("utf-8").GetString(genMsg.ParamList[2].Parameter);
                            if (!before.Contains("."))
                            {
                                string hex = Utils.BytesToHexString(genMsg.ParamList[2].Parameter, "Durr").Trim();
                                string[] parts = hex.Split('\n');
                                int where = 0;
                                for (int i = 0; i < parts.Length; i++)
                                {
                                    if (parts[i] == "2E")
                                    {
                                        where = i;
                                    }
                                }
                                before.Insert(where - 1, ".");
                                form.addDebugInfo("i hate c# " + before);
                            }
                            float Z = (float)Double.Parse(before, System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("en-us").NumberFormat)+2.0f;
        

                            //float Z =Utils.TryParseDouble float.Parse(before);  
                            form.addDebugInfo(Z.ToString());
                            form.addDebugInfo("\n");

                            uint MapX = (uint)(GlobalX & 0xFFFFFF00);
                            uint MapY = (uint)(GlobalY & 0xFFFFFF00);
                            ulong RegionHandle = (((ulong)MapX) << 32) | ((ulong)MapY);

                            TeleportLocationRequestPacket tp = new TeleportLocationRequestPacket();
                            tp.AgentData = new TeleportLocationRequestPacket.AgentDataBlock();
                            tp.AgentData.AgentID = frame.AgentID;
                            tp.AgentData.SessionID = frame.SessionID;
                            tp.Info = new TeleportLocationRequestPacket.InfoBlock();
                            tp.Info.RegionHandle = RegionHandle;
                            tp.Info.Position = new Vector3((float)(GlobalX & 0xFF), (float)(GlobalY & 0xFF), Z);
                            tp.Info.LookAt = tp.Info.Position;
                            Console.Write(tp.ToString());
                            proxy.InjectPacket(tp, Direction.Outgoing);
                            return null;
                        }
                        catch(Exception e)
                        {
                            form.addDebugInfo("Fail"+e.ToString());
                        }
                    }
                }
            }
            return packet;
            

        }

    }
}