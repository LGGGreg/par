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

namespace PubCombN
{
    public class LeetPlugin : GTabPlug
    {
        private ProxyFrame frame;
        private Proxy proxy;
        public PubComb plugin;
        public static bool Enabled = false;
        public string indicator, brand, trigger;
        private leetForm1 form;
        private String alphabet = "abcdefghijklmnopqrstuvwxyz";
        private String  [,] levels = new String [9,26]{
        {
            "4", "b", "c", "d", "3", "f", "g", "h", "i", "j", 
            "k", "1", "m", "n", "0", "p", "9", "r", "s", "7", 
            "u", "v", "w", "x", "y", "z"
        }, {
            "4", "b", "c", "d", "3", "f", "g", "h", "1", "j", 
            "k", "1", "m", "n", "0", "p", "9", "r", "5", "7", 
            "u", "v", "w", "x", "y", "2"
        }, {
            "4", "8", "c", "d", "3", "f", "6", "h", "'", "j", 
            "k", "1", "m", "n", "0", "p", "9", "r", "5", "7", 
            "u", "v", "w", "x", "'/", "2"
        }, {
            "@", "8", "c", "d", "3", "f", "6", "h", "'", "j", 
            "k", "1", "m", "n", "0", "p", "9", "r", "5", "7", 
            "u", "v", "w", "x", "'/", "2"
        }, {
            "@", "|3", "c", "d", "3", "f", "6", "#", "!", "7", 
            "|<", "1", "m", "n", "0", "|>", "9", "|2", "$", "7", 
            "u", "\\/", "w", "x", "'/", "2"
        }, {
            "@", "|3", "c", "|)", "&", "|=", "6", "#", "!", ",|", 
            "|<", "1", "m", "n", "0", "|>", "9", "|2", "$", "7", 
            "u", "\\/", "w", "x", "'/", "2"
        }, {
            "@", "|3", "[", "|)", "&", "|=", "6", "#", "!", ",|", 
            "|<", "1", "^^", "^/", "0", "|*", "9", "|2", "5", "7", 
            "(_)", "\\/", "\\/\\/", "><", "'/", "2"
        }, {
            "@", "8", "(", "|)", "&", "|=", "6", "|-|", "!", "_|", 
            "|\\(", "1", "|\\/|", "|\\|", "()", "|>", "(,)", "|2", "$", "|", 
            "|_|", "\\/", "\\^/", ")(", "'/", "\"/_"
        }, {
            "@", "8", "(", "|)", "&", "|=", "6", "|-|", "!", "_|", 
            "|\\{", "|_", "/\\/\\", "|\\|", "()", "|>", "(,)", "|2", "$", "|", 
            "|_|", "\\/", "\\^/", ")(", "'/", "\"/_"
        }
    };


        public LeetPlugin(PubComb plug)
        {
            plugin = plug;

            //formthread = new Thread(new ThreadStart(delegate()
            //{
               form = new leetForm1(this);
             //   Application.Run(form);
            //}));
            //formthread.SetApartmentState(ApartmentState.STA);
            //formthread.Start();

            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.brand = "ls";
            this.proxy.AddDelegate(PacketType.ScriptDialogReply, Direction.Outgoing, new PacketDelegate(OutDialogFromViewer));
            this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
            this.proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Outgoing, new PacketDelegate(SendingIM));

        }
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "Leet Speak");
            
        }
        public tabInfo getInfo()
        {
            return new tabInfo(form, "Leet Speak");
        }
         private Packet OutDialogFromViewer(Packet packet, IPEndPoint sim)
        {
            ScriptDialogReplyPacket DialogFromViewer = (ScriptDialogReplyPacket)packet;
            string message = Utils.BytesToString(DialogFromViewer.Data.ButtonLabel).Trim().ToLower();
            if (handeledViewerOutput(message) == "die")
            {
                return null;
            }
            return packet;
        }
        private Packet SendingIM(Packet packet, IPEndPoint sim)
        {
            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)packet;
            string message = Utils.BytesToString(im.MessageBlock.Message);
            if (handeledViewerOutput(message) == "die")
            {
                return null;
            }
            if(Enabled)
            im.MessageBlock.Message = Utils.StringToBytes(leetafy(message));
            return im;
        }
        public string handeledViewerOutput(string mssage)
        {

            if (mssage.ToLower().Contains(this.brand.ToLower() + "-on"))
            {
                Enabled = true;
                form.setBox(Enabled);
                
                frame.SendUserAlert("1337 Enabled");
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-off"))
            {
                Enabled = false;

                frame.SendUserAlert("1337 Disabled");
                form.setBox(Enabled);
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-level"))
            {
                int offset = this.brand.Length + 6;
                form.setLevel(int.Parse(mssage.Substring(mssage.ToLower().IndexOf(this.brand.ToLower() + "-level") + offset).Trim()));
                frame.SendUserAlert("1337 level now set to " + mssage.Substring(mssage.ToLower().IndexOf(this.brand.ToLower() + "-level") + offset).Trim());
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-help"))
            {
                frame.SayToUser("I am too lazy to write help");
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-about"))
            {
                frame.SayToUser("This program was made by LordGregGreg for the purpose ...");
                frame.SayToUser("Special Thanks to \"Philip Linden\" (yeah, thats not his actual sl name)\n" +
                    "and the OpenMetaverse project, and all it's contributors.\n");
                return "die";
            }
            else if (mssage.ToLower().Contains(this.brand.ToLower() + "-"))
            {
                frame.SendUserDialog(this.brand + "", "leet ", "speek", "What do you want me to do?", new string[] { "ls-ON", "ls-OFF", "ls-HELP", "ls-ABOUT" });
                return "die";
            }

            return "go";
        }
        public string leetafy(string s)
        {
            StringBuilder sb = new StringBuilder();
            int i = form.get1337();
            if (s == null || s.Length == 0 || i < 1)
            {
                return s;
            }

            for (int j = 0; j < s.Length; j++)
            {
                char c = (char)s.ToCharArray().GetValue(j);
                int k = alphabet.IndexOf(c);
                if (k > -1)
                {
                    sb.Append(levels[i - 1,k]);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        private Packet OutChatFromViewerHandler(Packet packet, IPEndPoint sim)
        {
            ChatFromViewerPacket ChatFromViewer = (ChatFromViewerPacket)packet;
            string message = Utils.BytesToString(ChatFromViewer.ChatData.Message);
            if (handeledViewerOutput(message) == "die")
            {
                return null;
            }
            if (form.getcheck())
            ChatFromViewer.ChatData.Message = Utils.StringToBytes(leetafy(message)); 
            return ChatFromViewer;

        }
        public void setEnabled(bool en)
        {
            Enabled = en;
            if (Enabled)
            {
                frame.SendUserAlert("1337 Enabled");
            }
            else
            {
                frame.SendUserAlert("1337 Disabled");
            }
        }

    }
}