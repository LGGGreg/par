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
    public class FileProtectPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        public Proxy proxy;
        private FileProtectForm1 form;
        private PubComb.Aux_SharedInfo shared;
        public List<UUID> avs = new List<UUID>();
        public void LoadNow()
        {
            plugin.tabform.addATab(form, "File Protect");
            form.readData();
        }
        public FileProtectPlugin(PubComb plug)
        {

            plugin = plug;
            shared = plug.SharedInfo;
            form = new FileProtectForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;

        }
        public Packet disbale(Packet p, IPEndPoint sim)
        {
            if (form.getChecked())
            {
                string f = Utils.BytesToString(((RequestXferPacket)p).XferID.Filename);
                if (f.Contains(".db2") || f.Contains(".inv")) return null;
            }
            return p;
        }


    }
}