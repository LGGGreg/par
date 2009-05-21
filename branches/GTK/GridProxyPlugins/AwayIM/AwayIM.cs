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
using OpenMetaverse.Utilities;
using GridProxy;
public class awayIM : ProxyPlugin
{
    private ProxyFrame frame;
    
    private Proxy proxy;
    private string awayMessage;
    public static bool Enabled = false;

    public awayIM(ProxyFrame frame)
    {
        this.frame = frame;
        this.proxy = frame.proxy;
        this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
        this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Incoming, new PacketDelegate(RecivedIM));
    }
    public override void Init()
    {
        SayToUser("/me loaded, say \"/away [away_message]\".");
    }
    private void SayToUser(string message)
    {
        
        ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
        packet.ChatData.FromName = Utils.StringToBytes("AutoReply plugin");
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
    private Packet RecivedIM(Packet packet, IPEndPoint sim)
    {
        if (Enabled)
        {
            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)packet;
            Console.WriteLine(im.ToString());
            if (im.MessageBlock.Dialog == (byte)0)
            {
                string message = Utils.BytesToString(im.MessageBlock.Message);
                if (message.ToLower().Contains("typing") )
                {
                    //ignoe
                }else if (message.ToLower().Contains("user not online"))
                {
                }
                else if (im.AgentData.AgentID == frame.AgentID || im.AgentData.AgentID==UUID.Zero )
                {
                    //ignore
                }else if(message.Length>0 )
                {
                    
                    ImprovedInstantMessagePacket send = new ImprovedInstantMessagePacket();
                    send.AgentData.AgentID = frame.AgentID;
                    send.AgentData.SessionID = frame.SessionID;

                    send.MessageBlock.Message = Utils.StringToBytes("Auto Message : " + awayMessage);
                    send.MessageBlock.Dialog = (byte)0;
                    send.MessageBlock.FromAgentName = Utils.StringToBytes("");
                    send.MessageBlock.FromGroup = false;
                    send.MessageBlock.ID = im.MessageBlock.ID;
                    send.MessageBlock.Offline = im.MessageBlock.Offline;
                    send.MessageBlock.ParentEstateID = im.MessageBlock.ParentEstateID;
                    send.MessageBlock.Position = im.MessageBlock.Position;
                    send.MessageBlock.RegionID = im.MessageBlock.RegionID;
                    send.MessageBlock.Timestamp = im.MessageBlock.Timestamp;
                    send.MessageBlock.ToAgentID = im.AgentData.AgentID;
                    send.MessageBlock.BinaryBucket = new Byte[] { };

                    proxy.InjectPacket(send, Direction.Outgoing);
                    proxy.InjectPacket(send, Direction.Incoming);
                }
            }
        }
        return packet;
    }
    private Packet OutChatFromViewerHandler(Packet packet, IPEndPoint sim)
    {
        ChatFromViewerPacket ChatFromViewer = (ChatFromViewerPacket)packet;
        string message = Utils.BytesToString(ChatFromViewer.ChatData.Message);
        if (message.ToLower().Contains("/away"))
        {
            if (!Enabled)
            {
                awayMessage = message.Substring(message.IndexOf("away") + 4).Trim();
                SendUserAlert("Away message active and set to: " + awayMessage);
                SayToUser("Say \"/away\" again to remove away message");
                Enabled = true;
            }
            else
            {
                SendUserAlert("Away message removed");
                Enabled = false;
            }
      
            return null;
        }

        return packet;

    }

}