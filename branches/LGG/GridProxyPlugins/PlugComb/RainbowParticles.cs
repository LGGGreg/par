﻿/*
 * Copyright (c) 2009, Day Oh & Gregory Hendrickson (LordGregGreg Back)
 * All Beams But SL Particles by day oh
 * Gui and settings by LGG
 * 
 *All rights reserved
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
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;

namespace PubComb
{
    public enum xtrafxMode
    {
        Single,
        Quad,
        Speak,
        Circles,
        Disabled
    }
    public class RainbowParticlesPlugin : GTabPlug
    {
        private ProxyFrame frame;
        private Proxy proxy;

        private System.Timers.Timer GregTimer = new System.Timers.Timer(500.0);


        public Vector3d lastVec = Vector3d.One;
        int oldTime = 0;
        bool curentOn = false;
        public PubComb plug;
        public RainbowParticlesForm1 form;


        public void LoadNow()
        {
            plug.tabform.addATab(form, "Selection Beams");
            form.readData();
        }
        public RainbowParticlesPlugin(PubComb plugin)
        {
            plug = plugin;
            form = new RainbowParticlesForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.ScriptDialogReply, Direction.Outgoing, new PacketDelegate(OutDialogFromViewer));
            this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
            this.proxy.AddDelegate(PacketType.ViewerEffect, Direction.Outgoing, new PacketDelegate(OutViewerEffectHandler));
            
            GregTimer.Elapsed += new System.Timers.ElapsedEventHandler(GregTimer_Elapsed);
            GregTimer.Start();
        }
        private void GregTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (curentOn)
            {
                if (System.Environment.TickCount - 1000 > oldTime)
                {
                    curentOn = false;
                    shoutOut(9000, "stop");
                }
            }
        }
        private void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
        public void objdesc(int locid, string desc)
        {
            ObjectDescriptionPacket packet = new ObjectDescriptionPacket();
            packet.AgentData.AgentID = this.frame.AgentID;
            packet.AgentData.SessionID = this.frame.SessionID;
            ObjectDescriptionPacket.ObjectDataBlock[] temp = new ObjectDescriptionPacket.ObjectDataBlock[1];
            temp[0] = new ObjectDescriptionPacket.ObjectDataBlock();
            temp[0].LocalID = (uint)locid;
            temp[0].Description = Utils.StringToBytes(desc);
            packet.ObjectData = temp;
            this.proxy.InjectPacket(packet, Direction.Outgoing);
        }
        private void RequestObjects(byte[] objects)
        {
            RequestMultipleObjectsPacket packet = new RequestMultipleObjectsPacket();
            packet.AgentData.AgentID = frame.AgentID;
            RequestMultipleObjectsPacket.ObjectDataBlock[] temp = new RequestMultipleObjectsPacket.ObjectDataBlock[4];
            for (int i = 0; i < objects.Length; i++)
            {
                temp[i] = new RequestMultipleObjectsPacket.ObjectDataBlock();
                temp[i].ID = objects[i];
                temp[i].CacheMissType = (byte)1;
            }
            packet.ObjectData = temp;
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
        
        private Packet OutDialogFromViewer(Packet packet, IPEndPoint sim)
        {
            ScriptDialogReplyPacket DialogFromViewer = (ScriptDialogReplyPacket)packet;
            string message = Utils.BytesToString(DialogFromViewer.Data.ButtonLabel).Trim().ToLower();
            if (message.Contains("x-") || message.Contains("xm-"))
            {
                viewerInput(message);
                return null;
            }
            return packet;
        }
        private void viewerInput(string message)
        {
            if (message.Contains("x-colors "))
            {
                form.setColors(message.Substring(9).Trim());
                //setColorFromString(message.Substring(9).Trim());
            }
            else if (message.Contains("xm-"))
            {
                xtrafxMode Mode = xtrafxMode.Disabled;
                if (message.Contains("single"))
                {
                    Mode = xtrafxMode.Single;
                    SendUserAlert("XtraFX Mode changed to Single.");
                }
                else if (message.Contains("quad"))
                {
                    Mode = xtrafxMode.Quad;
                    SendUserAlert("XtraFX Mode changed to quad.");
                }
                else if (message.Contains("speak"))
                {
                    Mode = xtrafxMode.Speak;
                    SendUserAlert("XtraFX Mode changed to speaking on chanel 9000.");
                }
                else
                    SendUserAlert("Mode not found.");


                form.setMode(Mode); 
            }
        }
        private Packet OutChatFromViewerHandler(Packet packet, IPEndPoint sim)
        {
            ChatFromViewerPacket ChatFromViewer = (ChatFromViewerPacket)packet;
            string message = Utils.BytesToString(ChatFromViewer.ChatData.Message).ToLower();
            if (message.Contains("x-") || message.Contains("xm-"))
            {
                viewerInput(message);
                return null;
            }

            return packet;

        }
        private Packet OutViewerEffectHandler(Packet packet, IPEndPoint sim)
        {
            if (form.Mode != xtrafxMode.Disabled)
            {
                ViewerEffectPacket ViewerEffect = (ViewerEffectPacket)packet;
                bool inject = false;
                foreach (ViewerEffectPacket.EffectBlock effect in ViewerEffect.Effect)
                {
                    if (effect.Type == (byte)EffectType.Beam)
                    {
                        if (form.Mode == xtrafxMode.Speak)
                        {
                            oldTime = System.Environment.TickCount;
                            Vector3d newVec = new Vector3d(effect.TypeData, 32);

                            if (curentOn == false)
                            {
                                shoutOut(9000, "start");
                                curentOn = true;
                            }
                            if ((lastVec.Equals(newVec) == false))
                            {

                                lastVec = newVec;
                                shoutOut(9000, this.lastVec.ToString());
                            }
                            return packet;

                        }

                        inject = true;
                        Buffer.BlockCopy(form.xtrafxb[form.xtrafx_Index], 0, effect.Color, 0, 4);
                        form.xtrafx_Index++;

                        if (form.xtrafx_Index >= form.xtrafxb.Length)
                        {
                            form.xtrafx_Index = 0;
                        }

                        if (form.Mode == xtrafxMode.Quad)
                        {
                            // trial: multi-beam
                            Vector3d position = new Vector3d(effect.TypeData, 32);

                            ViewerEffectPacket[][][] ve = new ViewerEffectPacket[2][][];
                            for (int x = 0; x < 2; x++)
                            {
                                ve[x] = new ViewerEffectPacket[2][];
                                for (int y = 0; y < 2; y++)
                                {
                                    ve[x][y] = new ViewerEffectPacket[2];
                                    for (int z = 0; z < 2; z++)
                                    {
                                        ve[x][y][z] = new ViewerEffectPacket();
                                        ve[x][y][z].AgentData = new ViewerEffectPacket.AgentDataBlock();
                                        ve[x][y][z].AgentData.AgentID = frame.AgentID;
                                        ve[x][y][z].AgentData.SessionID = frame.SessionID;
                                        ve[x][y][z].Effect = new ViewerEffectPacket.EffectBlock[1];
                                        ve[x][y][z].Effect[0] = new ViewerEffectPacket.EffectBlock();
                                        ve[x][y][z].Effect[0].ID = UUID.Random();
                                        ve[x][y][z].Effect[0].AgentID = effect.AgentID;
                                        ve[x][y][z].Effect[0].Type = effect.Type;
                                        ve[x][y][z].Effect[0].Duration = effect.Duration;
                                        ve[x][y][z].Effect[0].Color = effect.Color;
                                        ve[x][y][z].Effect[0].TypeData = effect.TypeData;
                                        double ox = ((double)x - 0.5f);
                                        double oy = ((double)y - 0.5f);
                                        double oz = ((double)z - 0.5f);
                                        Vector3d pos = new Vector3d(ox + position.X, oy + position.Y, oz + position.Z);
                                        Buffer.BlockCopy(pos.GetBytes(), 0, ve[x][y][z].Effect[0].TypeData, 32, 24);
                                        ve[x][y][z].Header.Reliable = false;
                                        proxy.InjectPacket(ve[x][y][z], Direction.Outgoing);
                                    }
                                }
                            }
                        }


                    }
                    else
                        if (form.Mode == xtrafxMode.Speak)
                        {
                            return null;
                        }
                }
                if (inject)
                {
                    packet = (Packet)ViewerEffect;
                }
            }
            return packet;
        }
        public void shoutOut(int c, string m)
        {

            ChatFromViewerPacket chat = new ChatFromViewerPacket();
            chat.AgentData.AgentID = frame.AgentID;
            chat.AgentData.SessionID = frame.SessionID;
            chat.ChatData.Channel = c;
            chat.ChatData.Type = (byte)2;
            chat.ChatData.Message = Utils.StringToBytes(m);
            proxy.InjectPacket(chat, Direction.Outgoing);
        }
    }
}
