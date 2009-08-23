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

    public class RetreatPlugin : GTabPlug
    {
        private ProxyFrame frame;
        private Proxy proxy;
        private PubComb plugin;
        private RetreatForm1 form;

        private UUID currentBastard = UUID.Zero;
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Retreat");
            form.readData();
        }
        public RetreatPlugin(PubComb plug)
        {

            form = new RetreatForm1(this);
            //    Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();
            plugin = plug;
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.TerminateFriendship, Direction.Incoming, new PacketDelegate(ItsOver));
            this.proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, new PacketDelegate(GotTheirName));
        

        }

        public Packet GotTheirName(Packet packet, IPEndPoint sim)
        {
            if (currentBastard != UUID.Zero)
            {
                UUIDNameReplyPacket nr = (UUIDNameReplyPacket)packet;
                for (int i = 0; i < nr.UUIDNameBlock.Length; i++)
                {
                    if (nr.UUIDNameBlock[i].ID == currentBastard)
                    {
                        string whatsup = Utils.BytesToString(nr.UUIDNameBlock[i].FirstName) + " " + Utils.BytesToString(nr.UUIDNameBlock[i].LastName) + " has just removed you from their freinds list!\n" +
                            "This was at " + System.DateTime.Now.ToShortDateString() + " : " + System.DateTime.Now.ToShortTimeString();

                        frame.SayToUser(whatsup);
                        frame.SendUserAlert(whatsup);
                        frame.SendUserDialog("Freindship", "Over", "Alert!", whatsup, new String[] { "DAMN!" });
                    }
                }
            }

            return packet;
        }
        public Packet ItsOver(Packet packet, IPEndPoint sim)
        {
            if (form.getEnabled())
            {
                TerminateFriendshipPacket tp = (TerminateFriendshipPacket)packet;
                //UUIDNameRequest = 65771,
                //UUIDNameReply = 65772,
                currentBastard = tp.ExBlock.OtherID;
                UUIDNameRequestPacket nr = new UUIDNameRequestPacket();
                UUIDNameRequestPacket.UUIDNameBlockBlock[] temp = new UUIDNameRequestPacket.UUIDNameBlockBlock[1];
                temp[0] = new UUIDNameRequestPacket.UUIDNameBlockBlock();
                temp[0].ID = currentBastard;
                nr.UUIDNameBlock = temp;
                proxy.InjectPacket(nr, Direction.Outgoing);

                
            }
            return packet;

        }
        

    }
}
    