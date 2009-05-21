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
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
using System.Threading;
using System.Windows.Forms;

namespace PubComb
{
    public class AwesomeSauce : GTabPlug
    {
        private ProxyFrame frame;
        private Proxy proxy;
        //private PubComb plugin;
        public static bool Enabled = true;
        public AwesomeSauceFormGTK form;
        //private Thread formthread;
        //private Form1 form;
        public string indicator, trigger;
        public Dictionary<UUID, CurrentUploadType> uploadsLookingForXferId = new Dictionary<UUID, CurrentUploadType>();
        public Dictionary<ulong, CurrentUploadType> currentuploads = new Dictionary<ulong, CurrentUploadType>();
        public static Dictionary<ulong, lsotoolXfer> OutgoingXfers = new Dictionary<ulong, lsotoolXfer>();
        public Dictionary<ulong, RequestXferPacket> requests2Process = new Dictionary<ulong, RequestXferPacket>();
        private Dictionary<UUID, TransferInfoPacket> scriptInfos = new Dictionary<UUID, TransferInfoPacket>();
        //public Dictionary<UUID, UUID> trans2Item = new Dictionary<UUID, UUID>();
        public string brand = "asauce";
        //private ulong currentRegion;
        //private List<CurrentUploadType> name2SaveBuffer = new List<CurrentUploadType>();
        public class CurrentUploadType
        {
            public bool proxySending;
            public string name;
            public int size;
            public byte[] data;
            public byte[] oldscriptdata;
            public int curentdatalenght;
            public int oldmaxsize;
            public bool temp;
            public bool local;
            public bool paid;
            //public uint ownerMask;
            public sbyte type;
            public UUID uploadID;
            public ulong xferID;
            public UUID VFileAssetID;
        }
        //public CurrentUploadType curentUpload;
        public class lsotoolXfer
        {
            public UUID VFileID;
            public Dictionary<uint, SendXferPacketPacket> Packets = new Dictionary<uint, SendXferPacketPacket>();
            public int PacketIndex = 0;

            public lsotoolXfer(UUID vfileid)
            {
                VFileID = vfileid;
            }
        }
        public void LoadNow(ref TabItemGTK tabform)
        {
            tabform.addATab(form, "AwesomeSauce");
        }
        public AwesomeSauce(PubComb p)
        {
            //plugin = p;
            //currentRegion = UUID.Zero.GetULong();
            form = new AwesomeSauceFormGTK(this);
            this.frame = p.frame;
            this.proxy = frame.proxy;
            //this.proxy.AddDelegate(PacketType.ScriptDialogReply, Direction.Outgoing, new PacketDelegate(OutDialogFromViewer));
            //this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
            //this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Outgoing, new PacketDelegate(SendingIM));
            this.proxy.AddDelegate(PacketType.AssetUploadRequest, Direction.Outgoing, new PacketDelegate(TryToSendAsset));
            this.proxy.AddDelegate(PacketType.ConfirmXferPacket, Direction.Incoming, new PacketDelegate(InConfirmXferPacketHandler));
            this.proxy.AddDelegate(PacketType.RequestXfer, Direction.Incoming, new PacketDelegate(ServerRequestsMyDataToStart));
            this.proxy.AddDelegate(PacketType.SendXferPacket, Direction.Outgoing, new PacketDelegate(ClientSentThisScript));
            this.proxy.AddDelegate(PacketType.TransferPacket, Direction.Incoming, new PacketDelegate(LoadingUpNewScript));
            this.proxy.AddDelegate(PacketType.TransferInfo, Direction.Incoming, new PacketDelegate(SimWantsToSendUs)); 
            //this.proxy.AddDelegate(PacketType.ReplyTaskInventory, Direction.Incoming, new PacketDelegate(ReplyTask));
            //this.proxy.AddDelegate(PacketType.TransferRequest, Direction.Outgoing, new PacketDelegate(TrasferReq));
            //this.proxy.AddCapsDelegate("UpdateScriptTask", new CapsDelegate(UploadStart));
            //this.proxy.AddCapsDelegate("UpdateScriptAgent", new CapsDelegate(UploadStart));

            //if (!Directory.Exists("(You can Delete me) Expired Scripts Cache"))
            //Directory.CreateDirectory("(You can Delete me) Expired Scripts Cache");
            
            if (!Directory.Exists("Scripts Cache"))
            {
                Directory.CreateDirectory("Scripts Cache");
            }
             
                       // File.Move("./" + f, "./Old Particle Scripts/" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString() +
                       //     " - " + DateTime.Now.TimeOfDay.TotalSeconds.ToString() + "@" + f.Substring(17));
                
        }
        
        /*private bool UploadStart(CapsRequest capReq, CapsStage stage)
        {
            Console.WriteLine(capReq.ToString());
            Console.WriteLine("drrr");
            Console.WriteLine(capReq.Request.ToString());
            
            //RequestXferPacket requestRecived = (RequestXferPacket)Packet.BuildPacket("RequestXfer", (OpenMetaverse.StructuredData.LLSDMap)capReq.Response);
            
            return false;
        }*/
		
		/* NOT USED
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
        */
		
        private void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
        /*private Packet OutDialogFromViewer(Packet packet, IPEndPoint sim)
        {
            ScriptDialogReplyPacket DialogFromViewer = (ScriptDialogReplyPacket)packet;
            if (handeledViewerOutput(Utils.BytesToString(DialogFromViewer.Data.ButtonLabel).Trim().ToLower()) == "die")
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

            if (mssage.ToLower().Contains(this.brand.ToLower() + "-on"))
            {
                Enabled = true;
                //form.setBox(Enabled);

                SendUserAlert("AwesomeSauce Enabled");
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-off"))
            {
                Enabled = false;

                SendUserAlert("AwesomeSauce Disabled");
                //form.setBox(Enabled);
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-help"))
            {
                SayToUser("Thanks for using the AwesomeSauce script text remover system :).\n" + "To make it work, just enable it and upload scripts in no mono\n"+
                         "You have to use DisableCaps.dll to UpdateScriptTask, and UpdateScriptAgent. (put them into the dropped side, and tp or relog)\n"+
                         "Then, just add \"//(COMPILE_PROTECTED)\"at the top of your script, and the rest is automatic.\n"+
                         "Your script wil be saved to your hard drive, and the server script will contain the top three lines of it only"+
                             "(maybe put copywright comments there)");
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-about"))
            {
                SayToUser("This program was made by LordGregGreg for the purpose of allowing\nscripts to be uploaded without their text.\nMight be good for securty if you don't trust sl to keep your perms right");
                SayToUser("Special Thanks to \"Philip Linden\" (yeah, thats not his actual sl name)\n" +
                    "and Darling Brody for ideas and motivation\n"+
                    "and the OpenMetaverse project, and all it's contributors.");

            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-"))
            {
                SendUserDialog(this.brand + "", "AwesomeSauce", "Messenger", "What do you want me to do?", new string[] { this.brand+"-ON", this.brand+"-OFF", this.brand+"-HELP", this.brand+"-ABOUT" });
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

        }*/

         private Packet ClientSentThisScript(Packet packet, IPEndPoint sim)
        {
            if (packet.Header.Resent) return packet;
            SendXferPacketPacket sxpp = (SendXferPacketPacket)packet;
            int start = 0;
            if (currentuploads.ContainsKey(sxpp.XferID.ID))
            {
                if (currentuploads[sxpp.XferID.ID].proxySending)
                {
                    Console.WriteLine("we are already sending this script, dont need its data");
                    return null;
                }
                if (currentuploads[sxpp.XferID.ID].curentdatalenght==0)
                {
                    Console.WriteLine("This was the first packet");
                    byte [] temp = new byte[sxpp.DataPacket.Data.Length];
                    Buffer.BlockCopy(sxpp.DataPacket.Data,4,temp,0,sxpp.DataPacket.Data.Length-4);

                    string script = Utils.BytesToString(temp);

                    string[] lines = script.Split('\n');
                    if(lines[0].Contains("(COMPILE_PROTECTED)"))
                    {
                        
                        form.Log("We got a script meant to be protected!",  System.Drawing.Color.DarkBlue, System.Drawing.Color.Black);
                        SendUserAlert("Uploading Script Protected");
                        start = 4;
                    }
                    else
                    {
                        form.Log("This Script was not meant to be protected ABANDON SHIP!", System.Drawing.Color.DarkBlue, System.Drawing.Color.Black);
                        currentuploads.Remove(sxpp.XferID.ID);
                        if(OutgoingXfers.ContainsKey(sxpp.XferID.ID))
                        {
                            OutgoingXfers.Remove(sxpp.XferID.ID);
                        }
                        return packet;
                    }
                }
                    
                form.Log("We got a script packet sent from client with the actual stuff, get it!", System.Drawing.Color.DarkBlue, System.Drawing.Color.Black);
                Buffer.BlockCopy(sxpp.DataPacket.Data, start, currentuploads[sxpp.XferID.ID].oldscriptdata,
                    currentuploads[sxpp.XferID.ID].curentdatalenght, sxpp.DataPacket.Data.Length-start);
                currentuploads[sxpp.XferID.ID].curentdatalenght += sxpp.DataPacket.Data.Length-start;

                if ((sxpp.XferID.Packet & 0x80000000) != 0)
                {

                    form.Log("we got the final packet sent from viewer to server, going to start upload now", System.Drawing.Color.Blue, System.Drawing.Color.Black);
                    
                    currentuploads[sxpp.XferID.ID].oldmaxsize = currentuploads[sxpp.XferID.ID].curentdatalenght;
                    if (requests2Process.ContainsKey(sxpp.XferID.ID))
                    {
                        ProcessRequest(requests2Process[sxpp.XferID.ID]);
                        requests2Process.Remove(sxpp.XferID.ID);
                    }
                    //last packet
                }
                ConfirmXferPacketPacket fakeConf = new ConfirmXferPacketPacket();
                fakeConf.XferID.ID = sxpp.XferID.ID;
                fakeConf.XferID.Packet = sxpp.XferID.Packet;
                proxy.InjectPacket(fakeConf, Direction.Incoming);
                return null;//block the packets the cliet is sending
            } //Console.WriteLine("Not one we are loking for");
            return packet;
        }
        private Packet ServerRequestsMyDataToStart(Packet packet, IPEndPoint sim)
        {
            RequestXferPacket requestRecived = (RequestXferPacket)packet;
            if (true)
            {

                if (requestRecived.XferID.VFileType == (short)AssetType.LSLText)
                {
                    form.Log("Server requestd us to start something lsl " +
                        requestRecived.XferID.VFileType.ToString()+" and its vfile id was "+requestRecived.XferID.VFileID.ToString()
                        ,System.Drawing.Color.Magenta,System.Drawing.Color.Black);
                
                    if (uploadsLookingForXferId.ContainsKey(requestRecived.XferID.VFileID))
                    {
                        form.Log("We were expecting this! its vfile matches the asset id agreed upon , adding for later processing", System.Drawing.Color.Magenta, System.Drawing.Color.DarkGray);
                        
                        uploadsLookingForXferId[requestRecived.XferID.VFileID].xferID = requestRecived.XferID.ID;
                        if (!currentuploads.ContainsKey(requestRecived.XferID.ID))
                        {
                            currentuploads.Add(requestRecived.XferID.ID, uploadsLookingForXferId[requestRecived.XferID.VFileID]);
                        }
                        else
                        {
                            currentuploads[requestRecived.XferID.ID] = uploadsLookingForXferId[requestRecived.XferID.VFileID];
                        }
                        uploadsLookingForXferId.Remove(requestRecived.XferID.VFileID);
                        
                        //TODO: we need to do this AFTER we get the script;
                        if (requests2Process.ContainsKey(requestRecived.XferID.ID))
                        {
                            requests2Process[requestRecived.XferID.ID] = requestRecived;
                        }
                        else
                        {
                            requests2Process.Add(requestRecived.XferID.ID, requestRecived);
                        }
                        /*
                        if (uploadsLookingForXferId[requestRecived.XferID.VFileID].oldscriptdata.Length > 0)
                        {
                            form.Log("Old Script data finished in first packet", System.Drawing.Color.Blue, System.Drawing.Color.Black);
                            if (requests2Process.ContainsKey(requestRecived.XferID.ID))
                            {
                                ProcessRequest(requests2Process[requestRecived.XferID.ID]);
                                requests2Process.Remove(requestRecived.XferID.ID);
                            }
                        }*/

                        return packet;
                    }
                }
            }
            return packet;
        }
        private Packet LoadingUpNewScript(Packet packet, IPEndPoint sim)
        {
            if (packet.Header.Resent) return packet;
            TransferPacketPacket tpp = (TransferPacketPacket)packet;
            if (tpp.TransferData.ChannelType == 2 && tpp.TransferData.Packet==0 && scriptInfos.ContainsKey(tpp.TransferData.TransferID))
            {
                string data = Utils.BytesToString(tpp.TransferData.Data);
                if (data.Contains("//-->(LGG_PROTECTED)"))
                {
                    string incache = "Scripts Cache\\" + data.Substring(20, 36) + ".lsl";
                    form.Log("Detected a localy saved script, looking.." + incache, System.Drawing.Color.DarkBlue, System.Drawing.Color.Cyan);

                    if (File.Exists(incache))
                    {
                        form.Log("Found !localy saved script, loading..", System.Drawing.Color.Blue, System.Drawing.Color.Cyan);

                        using (StreamReader r = new StreamReader(incache))
                        {
                            byte[] newdata = Utils.StringToBytes(r.ReadToEnd());

                            r.Close();

                            TransferInfoPacket tosend = scriptInfos[tpp.TransferData.TransferID];
                            scriptInfos.Remove(tpp.TransferData.TransferID);

                            tosend.TransferInfo.Size = newdata.Length + (1000 - (newdata.Length % 1000));
                            proxy.InjectPacket(tosend, Direction.Incoming);
                            form.Log("Injecting info with size " + tosend.TransferInfo.Size.ToString(), System.Drawing.Color.Blue, System.Drawing.Color.Cyan);


                            int start = 0;
                            int pnum = 0;
                            byte[] temp = new byte[1000];

                            while (newdata.Length - start > 1000)
                            {
                                Buffer.BlockCopy(newdata, start, temp, 0, 1000);
                                TransferPacketPacket send = tpp;
                                send.TransferData.Packet = pnum++;
                                send.TransferData.Data = temp;
                                send.TransferData.Status = (int)StatusCode.OK;
                                proxy.InjectPacket(send, Direction.Incoming);
                                start += 1000;
                                form.Log("Injecting Packet number " + pnum.ToString(), System.Drawing.Color.Blue, System.Drawing.Color.Cyan);


                            }
                            temp = new byte[1000];
                            Buffer.BlockCopy(newdata, start, temp, 0, newdata.Length - start);
                            tpp.TransferData.Data = temp;
                            tpp.TransferData.Status = (int)StatusCode.Done;
                            tpp.TransferData.Packet = pnum++;
                            form.Log("Injecting Packet number " + pnum.ToString(), System.Drawing.Color.Blue, System.Drawing.Color.Cyan);


                        }



                    }
                    else
                    {
                        form.Log("oh shit.. cant find the script... f*" + incache, System.Drawing.Color.DarkBlue, System.Drawing.Color.Cyan);

                    }
                }
                else
                {
                    TransferInfoPacket tosend = scriptInfos[tpp.TransferData.TransferID];
                    scriptInfos.Remove(tpp.TransferData.TransferID);
                    proxy.InjectPacket(tosend, Direction.Incoming);
                }
            }
            return tpp;
        }
        private Packet SimWantsToSendUs(Packet packet, IPEndPoint sim)
        {
            TransferInfoPacket ti = (TransferInfoPacket)packet;
            /*
                Size: 1000
                Params: F0 33 49 09 28 BC 47 17 AE 0E 18 93 C2 88 1E 3D
                Params: BB 48 FF 01 46 38 4C 1D 95 2B CC D3 39 98 EB 80
                Params: F0 33 49 09 28 BC 47 17 AE 0E 18 93 C2 88 1E 3D
                Params: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                Params: 60 2D 0C 73 95 74 1F 33 56 3F C8 6E EE 42 B2 27
                Params: 3F 13 21 7D 96 A4 8E 65 D6 11 E1 C7 AA 23 08 1D
                Params: 0A 00 00 00
             */
            if (ti.TransferInfo.Size <= 1000)
            {
                sbyte type = 0;
                if (ti.TransferInfo.Params.Length == 20)
                {
                    //download.AssetID = new UUID(info.TransferInfo.Params, 0);
                    type = (sbyte)ti.TransferInfo.Params[16];

                    //Client.DebugLog(String.Format("TransferInfo packet received. AssetID: {0} Type: {1}",
                    //    transfer.AssetID, type));
                }
                else if (ti.TransferInfo.Params.Length == 100)
                {
                    // TODO: Can we use these?
                    //UUID agentID = new UUID(info.TransferInfo.Params, 0);
                    //UUID sessionID = new UUID(info.TransferInfo.Params, 16);
                    //UUID ownerID = new UUID(info.TransferInfo.Params, 32);
                    //UUID taskID = new UUID(info.TransferInfo.Params, 48);
                    //UUID itemID = new UUID(info.TransferInfo.Params, 64);
                    //download.AssetID = new UUID(info.TransferInfo.Params, 80);
                    type = (sbyte)ti.TransferInfo.Params[96];
                    
                    //Client.DebugLog(String.Format("TransferInfo packet received. AgentID: {0} SessionID: {1} " + 
                    //    "OwnerID: {2} TaskID: {3} ItemID: {4} AssetID: {5} Type: {6}", agentID, sessionID, 
                    //    ownerID, taskID, itemID, transfer.AssetID, type));
                }
                if (type == 10)
                {
                    form.Log("we got a incomming scrippt text.. it could be one we need", System.Drawing.Color.Black, System.Drawing.Color.Blue);
                    if (scriptInfos.ContainsKey(ti.TransferInfo.TransferID))
                    {
                        scriptInfos[ti.TransferInfo.TransferID] = ti;
                    }
                    else
                    {
                        scriptInfos.Add(ti.TransferInfo.TransferID, ti);
                    }
                    //return null;


                }
            }

            return packet;
        }
        /*private Packet ReplyTask(Packet packet, IPEndPoint sim)
            {
                Thread mythread = new Thread(new ThreadStart(delegate
                {
                    ManualResetEvent finishevent = new ManualResetEvent(false);
                    string filename = Utils.BytesToString(((ReplyTaskInventoryPacket)packet).InventoryData.Filename);
                    ulong xferid = UUID.Random().GetULong();
                    Dictionary<uint, byte[]> datapackets = new Dictionary<uint, byte[]>();
                    bool datahasend = false;
                    uint lastpacket = 0;
                    PacketDelegate XferHandler = delegate(Packet packet3, IPEndPoint sim3)
                    {
                        SendXferPacketPacket xferpacket = (SendXferPacketPacket)packet3;
                        if (xferpacket.XferID.ID == xferid)
                        {
                            uint packetnumber = xferpacket.XferID.Packet & 0x7FFFFFFF;
                            if (!datapackets.ContainsKey(packetnumber))
                                datapackets[packetnumber] = xferpacket.DataPacket.Data;
                            if ((xferpacket.XferID.Packet & 0x80000000) != 0)
                            {
                                datahasend = true;
                                lastpacket = packetnumber;
                            }
                            if (datahasend)
                            {
                                if (datapackets.Count == (lastpacket + 1))
                                {
                                    int size = 0;
                                    foreach (KeyValuePair<uint, byte[]> kvp in datapackets)
                                        size += kvp.Value.Length;
                                    byte[] data = new byte[size - 4];
                                    int offset = 0;
                                    for (uint i = 0; i <= lastpacket; i++)
                                    {
                                        if (i == 0)
                                        {
                                            Buffer.BlockCopy(datapackets[i], 4, data, offset, datapackets[i].Length - 4);
                                            offset += datapackets[i].Length - 4;
                                        }
                                        else
                                        {
                                            Buffer.BlockCopy(datapackets[i], 0, data, offset, datapackets[i].Length);
                                            offset += datapackets[i].Length;
                                        }
                                    }
                                    ReceiveInventoryFile(Utils.BytesToString(data));
                                    finishevent.Set();
                                }
                            }
                        }
                        return packet3;
                    };
                    frame.proxy.AddDelegate(PacketType.SendXferPacket, Direction.Incoming, XferHandler);
                    ManualResetEvent requestevent = new ManualResetEvent(false);
                    PacketDelegate evtsetter = delegate(Packet packet2, IPEndPoint sim2)
                    {
                        if (Utils.BytesToString(((RequestXferPacket)packet2).XferID.Filename) == filename)
                        {
                            xferid = ((RequestXferPacket)packet2).XferID.ID;
                            requestevent.Set();
                        }
                        return packet2;
                    };
                    frame.proxy.AddDelegate(PacketType.RequestXfer, Direction.Outgoing, evtsetter);
                    bool needrequest = !requestevent.WaitOne(1000, false);
                    frame.proxy.RemoveDelegate(PacketType.RequestXfer, Direction.Outgoing, evtsetter);
                    if (needrequest)
                    {
                        RequestXferPacket request = new RequestXferPacket();
                        request.XferID = new RequestXferPacket.XferIDBlock();
                        request.XferID.ID = xferid;
                        request.XferID.Filename = Utils.StringToBytes(filename);
                        request.XferID.FilePath = 4;
                        request.XferID.DeleteOnCompletion = true;
                        request.XferID.UseBigPackets = false;
                        request.XferID.VFileID = UUID.Zero;
                        request.XferID.VFileType = -1;
                        frame.proxy.InjectPacket(request, Direction.Outgoing);
                    }
                    finishevent.WaitOne(60000, false);
                    frame.proxy.RemoveDelegate(PacketType.SendXferPacket, Direction.Incoming, XferHandler);
                }));
                mythread.Start();
                return packet;
            
        }*/
        /*private void ReceiveInventoryFile(string data)
        {
            Console.Write("--------Received file:--------\n");
            Console.Write(data);

            //UUID magic = new UUID("3c115e51-04f4-523c-9fa6-98aff1034730");
            string[] lines = data.Split('\n');
            UUID creatorid = UUID.Zero;
            string type = "";
            string inv_type = "";
            string name = "";
            string description = "";
            bool got = false;
            foreach (string line in lines)
            {
                string L = line.Trim();
                if (L.StartsWith("inv_item"))
                {
                    if (got)
                    {
                        //ReceiveAssetInfo(name, description, creatorid, type, inv_type);
                        return;
                    }
                    else
                    {
                        got = true;
                        creatorid = UUID.Zero;
                        name = "";
                        inv_type = "";
                        description = "";
                    }
                }
                else if (L.StartsWith("creator_id\t"))
                {
                    creatorid = new UUID(L.Substring(11).Trim());
                }
                else if (L.StartsWith("desc\t"))
                {
                    description = L.Substring(5).Trim().TrimEnd('|');
                }
                else if (L.StartsWith("type\t"))
                {
                    type = L.Substring(5).Trim();
                }
                else if (L.StartsWith("inv_type\t"))
                {
                    inv_type = L.Substring(9).Trim();
                }
                else if (L.StartsWith("name\t"))
                {
                    name = L.Substring(5).Trim().TrimEnd('|');
                }
            }
        }*/
        /*private Packet TrasferReq(Packet packet, IPEndPoint sim)
        {
            TransferRequestPacket trp = (TransferRequestPacket)packet;
            if (trp.TransferInfo.SourceType == 3)
            {
                /*smels 
                Buffer.BlockCopy(frame.AgentID.GetBytes(), 0, paramField, 0, 16);
                Buffer.BlockCopy(frame.SessionID.GetBytes(), 0, paramField, 16, 16);
                Buffer.BlockCopy(item.OwnerID.GetBytes(), 0, paramField, 32, 16);
                Buffer.BlockCopy(UUID.Zero.GetBytes(), 0, paramField, 48, 16);
                Buffer.BlockCopy(item.UUID.GetBytes(), 0, paramField, 64, 16);
                Buffer.BlockCopy(item.AssetUUID.GetBytes(), 0, paramField, 80, 16);
                Buffer.BlockCopy(Helpers.IntToBytes((int)item.AssetType), 0, paramField, 96, 4);
                UUID itemid = new UUID(trp.TransferInfo.Params, 64);
                if (new List<string>(Directory.GetFiles("Scripts Cache")).Contains(itemid.ToString()))
                {

                    using (StreamReader r = new StreamReader("Scripts Cache\\"+itemid.ToString()))
                    {
                        byte[] data = Utils.StringToBytes(r.ReadToEnd());
                        TransferPacketPacket tpp = new TransferPacketPacket();
                        tpp.TransferData.ChannelType = trp.TransferInfo.ChannelType;
                        tpp.TransferData.Status = (int)StatusCode.Done;
                        tpp.TransferData.Packet = 0;
                        tpp.TransferData.TransferID = trp.TransferInfo.TransferID;
                        tpp.TransferData.Data = data;
                        proxy.InjectPacket(tpp, Direction.Incoming); 
                        

                        TransferInfoPacket tip = new TransferInfoPacket();
                        tip.TransferInfo = new TransferInfoPacket.TransferInfoBlock();
                        tip.TransferInfo.Params = trp.TransferInfo.Params;
                        tip.TransferInfo.Size = 0;
                        tip.TransferInfo.Status = (int)StatusCode.OK;
                        tip.TransferInfo.TargetType = 3;
                        tip.TransferInfo.TransferID = trp.TransferInfo.TransferID ;
                        tip.TransferInfo.ChannelType = 2;
                        tip.Header.Reliable = true;
                        proxy.InjectPacket(tip, Direction.Incoming);
                    }
                }
                

            }
            return packet;
        }*/
        private void ProcessRequest(RequestXferPacket requestRecived)
        {
            form.Log("precesing the read request packet now", System.Drawing.Color.Green, System.Drawing.Color.Black);
            
            currentuploads[requestRecived.XferID.ID].proxySending = true;
            byte[][] datapackets = OpenBytecode(requestRecived.XferID.ID);

            SendXferPacketPacket SendXferPacket;

            OutgoingXfers.Add(requestRecived.XferID.ID, new lsotoolXfer(requestRecived.XferID.VFileID));
            for (uint i = 0; i < (this.currentuploads[requestRecived.XferID.ID].size); i++)
            {
                SendXferPacket = new SendXferPacketPacket();
                SendXferPacket.XferID = new SendXferPacketPacket.XferIDBlock();
                this.currentuploads[requestRecived.XferID.ID].xferID = requestRecived.XferID.ID;
                SendXferPacket.XferID.ID = requestRecived.XferID.ID; 
                SendXferPacket.XferID.Packet = i;
                if (i == (this.currentuploads[requestRecived.XferID.ID].size - 1))
                    SendXferPacket.XferID.Packet = i | 0x80000000;
                SendXferPacket.DataPacket = new SendXferPacketPacket.DataPacketBlock();
                SendXferPacket.DataPacket.Data = datapackets[i];
                OutgoingXfers[requestRecived.XferID.ID].Packets.Add(i, SendXferPacket);
            }

            // Start sending the new packets to the sim
            OutgoingXfers[requestRecived.XferID.ID].PacketIndex = 0;
            form.Log("sent first packet with data\n" + Utils.BytesToString(OutgoingXfers[requestRecived.XferID.ID].Packets[0].DataPacket.Data), System.Drawing.Color.Yellow, System.Drawing.Color.DarkGreen);
            proxy.InjectPacket(OutgoingXfers[requestRecived.XferID.ID].Packets[0], Direction.Outgoing);
            //this.Scriptdata = OpenBytecode(fileDest);     
       

        }
        private Packet InConfirmXferPacketHandler(Packet packet, IPEndPoint sim)
        {
            if (Enabled)
            {
                ConfirmXferPacketPacket ConfirmXferPacket = (ConfirmXferPacketPacket)packet;

                ulong XferID = ConfirmXferPacket.XferID.ID;
                if (OutgoingXfers.ContainsKey(XferID))
                { // This is one of our Xfers
                    //SayToUser("sending the script packets...");
                    //OutgoingXfers[XferID].PacketIndex++;
                    //form.setProgress(OutgoingXfers[XferID].PacketIndex++);
                    OutgoingXfers[XferID].PacketIndex++;
                    Console.WriteLine("progress is " + OutgoingXfers[XferID].PacketIndex + " of " + currentuploads[XferID].size.ToString());
                    if (OutgoingXfers[XferID].PacketIndex < (currentuploads[XferID].size))
                    {
                        // Send the next packet
                        proxy.InjectPacket(OutgoingXfers[XferID].Packets[(uint)OutgoingXfers[XferID].PacketIndex], Direction.Outgoing);
                    }
                    else
                    {
                        // Done done done
                        //form.setProgress(curentUpload.size);
                        Console.WriteLine("<>>Upload successful!");
                        /*
                        UpdateInventoryItemPacket p = new UpdateInventoryItemPacket();

                        CreateInventoryItemPacket create = new CreateInventoryItemPacket();
                        create.AgentData = new CreateInventoryItemPacket.AgentDataBlock();
                        create.AgentData.AgentID = frame.AgentID;
                        create.AgentData.SessionID = frame.SessionID;
                        create.InventoryBlock = new CreateInventoryItemPacket.InventoryBlockBlock();
                        create.InventoryBlock.CallbackID = 0; // What's this?             
                        create.InventoryBlock.FolderID = UUID.Zero;
                        create.InventoryBlock.TransactionID = currentuploads[XferID].uploadID;
                        create.InventoryBlock.NextOwnerMask = currentuploads[XferID].ownerMask;
                        create.InventoryBlock.Type = currentuploads[XferID].type;
                        create.InventoryBlock.InvType = currentuploads[XferID].type;
                        create.InventoryBlock.WearableType = (byte)0;
                        string name = "New Text Preview - Delete Me";

                        if (currentuploads[XferID].temp)
                            name += "-temp";
                        if (currentuploads[XferID].local)
                            name += "-local";
                        if (currentuploads[XferID].paid)
                            name += "-paid";

                        //name += "-t=" + curentUpload.type.ToString();


                        create.InventoryBlock.Name = Utils.StringToBytes(name);


                        create.InventoryBlock.Description = Utils.StringToBytes(name);

                        //proxy.InjectPacket(create, Direction.Outgoing);
                        //string filename = "";
                         */
                        /*if (trans2Item.ContainsKey(curentUpload.uploadID))
                        {
                            filename = trans2Item[curentUpload.uploadID].ToString();

                            File.WriteAllBytes("Scripts Cache\\" + filename, curentUpload.oldscriptdata);
                        }
                        else
                        {
                            name2SaveBuffer.Add(curentUpload);
                        }*/


                        form.Log("blocked " + ConfirmXferPacket.XferID.Packet.ToString() + ";", System.Drawing.Color.DarkCyan, System.Drawing.Color.Black);

                        OutgoingXfers.Remove(XferID);
                        currentuploads.Remove(XferID);
                        return null;

                    }
                    // Don't send this to the client
                    //if (currentuploads.ContainsKey(XferID))
                      //  if (currentuploads[XferID].proxySending) 
                    form.Log("blocked " + ConfirmXferPacket.XferID.Packet.ToString() + ";", System.Drawing.Color.DarkCyan, System.Drawing.Color.Black);

                            return null;

                }
            }
            //form.Log("we let this confirm packet go! " + packet.ToString(), System.Drawing.Color.DarkBlue, System.Drawing.Color.Black);
            return packet;
        }
        /*private Packet UpdateInventory(Packet packet, IPEndPoint sim)
        {
            /*
            UpdateInventoryItemPacket uip = (UpdateInventoryItemPacket)packet;
            foreach (UpdateInventoryItemPacket.InventoryDataBlock block in uip.InventoryData)
            {
                if (block.Type == 10)
                {
                    if(trans2Item.ContainsKey(block.TransactionID))
                    {
                        trans2Item[block.TransactionID]=block.ItemID;
                    }else
                    trans2Item.Add(block.TransactionID, block.ItemID);
                }
            }
            foreach (CurrentUploadType cu in name2SaveBuffer)
            {
                if (trans2Item.ContainsKey(cu.uploadID))
                {
                    string filename = trans2Item[curentUpload.uploadID].ToString();

                    File.WriteAllBytes("Scripts Cache\\" + filename, curentUpload.oldscriptdata);
                }
            }
            name2SaveBuffer.Clear();
            return packet;
        }*/
        private Packet TryToSendAsset(Packet packet, IPEndPoint sim)
        {
            if (Enabled)
            {
                AssetUploadRequestPacket ass = (AssetUploadRequestPacket)packet;
                if (ass.AssetBlock.Type == (sbyte)AssetType.LSLText)
                {
                    Console.WriteLine("Atempting to save to inv with asset type " + ass.AssetBlock.Type.ToString());

                    CurrentUploadType curentUpload = new CurrentUploadType();
                    curentUpload.proxySending = false;
                    curentUpload.local = ass.AssetBlock.StoreLocal;
                    curentUpload.temp = ass.AssetBlock.Tempfile;
                    curentUpload.uploadID = ass.AssetBlock.TransactionID;
                    curentUpload.type = ass.AssetBlock.Type;
                    curentUpload.data = new byte[] { };
                    curentUpload.paid = false;
                    curentUpload.oldscriptdata = new byte[300000];
                    /*uint newperms = 0;
                    newperms |= (uint)PermissionMask.Modify;
                    newperms |= (uint)PermissionMask.Copy;
                    newperms |= (uint)PermissionMask.Transfer;
                    
                    curentUpload.ownerMask = newperms;*/


                    curentUpload.curentdatalenght = 0;
                    curentUpload.oldmaxsize = 90000;
                    //Buffer.BlockCopy(ass.AssetBlock.AssetData, 0, curentUpload.oldscriptdata, 0, curentUpload.curentdatalenght);


                    AssetUploadRequestPacket asetUp = new AssetUploadRequestPacket();
                    asetUp.AssetBlock.StoreLocal = curentUpload.local;
                    asetUp.AssetBlock.Tempfile = false;
                    asetUp.AssetBlock.TransactionID = curentUpload.uploadID;
                    asetUp.AssetBlock.Type = (sbyte)AssetType.LSLText;
                    asetUp.AssetBlock.AssetData = new byte[] { };
                    //proxy.InjectPacket(asetUp, Direction.Outgoing);
                    //proxy
                    curentUpload.VFileAssetID = UUID.Combine(curentUpload.uploadID, frame.SessionID);

                    form.Log("We are expecintg a vfile id of " + curentUpload.VFileAssetID.ToString(), System.Drawing.Color.Black, System.Drawing.Color.Magenta);
                    if (this.uploadsLookingForXferId.ContainsKey(curentUpload.VFileAssetID))
                    {
                        this.uploadsLookingForXferId[curentUpload.VFileAssetID] = curentUpload;

                    }
                    else
                        this.uploadsLookingForXferId.Add(curentUpload.VFileAssetID, curentUpload);

                }
            }
            return packet;
        }
        public byte[] getNewScriptText(ulong xid)
        {
            /*GridClient client = new GridClient();
            UUID vfile;
            client.Assets.RequestUpload(out vfile, AssetType.Notecard, Utils.StringToBytes("test notecard body"), false);
            client.Inventory.RequestCreateItem(client.Inventory.FindFolderForType(AssetType.Notecard),
                    "notecard body", "note card desc", AssetType.Notecard, vfile, InventoryType.Notecard, PermissionMask.All, delegate(bool success, InventoryItem item)
                    {
                        if (success) 
                            Console.Write("woot");
                    }
            );*/
            
            
            byte[] tmp = new byte[currentuploads[xid].oldmaxsize];
            Buffer.BlockCopy(currentuploads[xid].oldscriptdata, 0, tmp, 0, currentuploads[xid].oldmaxsize);
            string oldscript = Utils.BytesToString(tmp);

            string randomid = UUID.Random().ToString();
            File.WriteAllBytes("Scripts Cache\\"+randomid+".lsl",tmp);

            string[] lines = oldscript.Split('\n');
            int numberlines = 3;
            if (lines.Length < 3) numberlines = lines.Length;

            string first3lines = "";
            for (int i = 1; i <= numberlines; i++)
            {
                first3lines += lines[i]+'\n';
            }
            //byte [] f3 = Utils.StringToBytes(first3lines);

            oldscript = "//-->(LGG_PROTECTED)" + randomid + "(LGG_PROTECTED)<--\\\\\n";
            //byte[] ol = Utils.StringToBytes(oldscript);

            //byte [] output = new byte[f3.Length+3 + ol.Length];
            //Buffer.BlockCopy(ol, 0, output, 0, ol.Length);
            //Buffer.BlockCopy(tmp, 0, output, ol.Length, f3.Length+3);



            return Utils.StringToBytes(oldscript + first3lines); ;
        }
        public byte[][] OpenBytecode(ulong Xid)
        {
            //[14:45]  Cryogenic Blitz: CoarseUpdate packets
            byte[] source = getNewScriptText(Xid);
            int remainingBytes = source.Length;
            int np = 0;
            byte[][] tempPackets = new byte[200000][];
            
            tempPackets[0] = new byte[1004];
            if (remainingBytes > 1000)
            {
                Buffer.BlockCopy(source, 0, tempPackets[0], 4, 1000);
                remainingBytes -= 1000;
                np++;
                while (remainingBytes > 1000)
                {
                    tempPackets[np] = new byte[1000];
                    Buffer.BlockCopy(source, 1000 * np, tempPackets[np], 0, 1000);
                    np++;
                    remainingBytes -= 1000;
                }
                tempPackets[np] = new byte[remainingBytes];
                Buffer.BlockCopy(source, 1000 * np, tempPackets[np], 0, remainingBytes);
            }
            else
            {
                Buffer.BlockCopy(source, 0, tempPackets[0], 4, remainingBytes);
            }
            np++;
            this.currentuploads[ Xid].size = np;
            //fill in first 4 bytes
            Buffer.BlockCopy(BitConverter.GetBytes(source.Length+4), 0,tempPackets[0], 0, 4);
            
            //rebuild it all the right size
            byte[][] packets = new byte[np][];
            for (int i = 0; i < np; i++)
            {
                packets[i] = tempPackets[i];
            }

            //form.setProgressProps(np);
            return packets;
        }
        

    }
}