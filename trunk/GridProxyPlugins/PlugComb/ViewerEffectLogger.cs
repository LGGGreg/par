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
    public class ViewerEffectLogPlugin : GTabPlug
    {
        private PubComb plugin;
        private ProxyFrame frame;
        public Proxy proxy;
        private VELogForm1 form;
        private PubComb.Aux_SharedInfo shared;
        public List<UUID> avs = new List<UUID>();
        public void LoadNow()
        {
            tabInfo t = getInfo();
            plugin.tabform.addATab(t.f, t.s);
        }
        public tabInfo getInfo()
        {
            return new tabInfo(form, "VE Logger");
        }
        public ViewerEffectLogPlugin(PubComb plug)
        {

            plugin = plug;
            shared = plug.SharedInfo;
            form = new VELogForm1(this);
            this.frame = plug.frame;
            this.proxy = plug.proxy;
            this.proxy.AddDelegate(PacketType.ViewerEffect, Direction.Incoming, new PacketDelegate(ve));
        }
        public Packet ve(Packet p, IPEndPoint sim)
        {
            foreach(ViewerEffectPacket.EffectBlock b in ((ViewerEffectPacket)p).Effect)
            {

                if (b.Type == (byte)EffectType.AnimalControls && form.checkBox16animla.Checked)
                {
                    form.textBox16animal.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.AnimationObject && form.checkBox15animation.Checked)
                {
                    form.textBox15animat.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Beam && form.checkBox14beam.Checked)
                {
                    form.textBox14beam.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Cloth && form.checkBox1cloth.Checked)
                {
                    form.textBox1cloth.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Connector && form.checkBox2connector.Checked)
                {
                    form.textBox2connecotr.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Edit && form.checkBox3edit.Checked)
                {
                    form.textBox3edit.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.FlexibleObject && form.checkBox4flexble.Checked)
                {
                    form.textBox4flexable.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Glow && form.checkBox5glow.Checked)
                {
                    form.textBox5glow.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Icon && form.checkBox6icon.Checked)
                {
                    form.textBox6icon.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.LookAt && form.checkBox7lookat.Checked)
                {
                    form.textBox7lookat.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Point && form.checkBox8point.Checked)
                {
                    form.textBox8point.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.PointAt && form.checkBox9pointat.Checked)
                {
                    form.textBox9pointat.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Sphere && form.checkBox10sphere.Checked)
                {
                    form.textBox10sphere.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Spiral && form.checkBox11spiral.Checked)
                {
                    form.textBox11spiral.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Text && form.checkBox12text.Checked)
                {
                    form.textBox12text.Text += b.ToString();
                }else  if (b.Type == (byte)EffectType.Trail && form.checkBox13trail.Checked)
                {
                    form.textBox13trail.Text += b.ToString();
                } 
            }
            return p;
        }


    }
}