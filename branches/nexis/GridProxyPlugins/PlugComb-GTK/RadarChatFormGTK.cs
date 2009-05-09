/*
 * Copyright (c) 2009,  Gregory Hendrickson (LordGregGreg Back) and Rob Nelson (Fred Rookstown)
 * 
 * All rights reserved
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 *   * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *   * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *   * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
// RadarChatFormGTK.cs created with MonoDevelop
// User: nexis at 11:30 A 07/05/2009
//


using System;
using System.IO;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;
using System.Collections.Generic;

namespace PubComb
{
	public partial class RadarChatFormGTK : Gtk.Window
	{
		private RadarChatPlugin rc;
		
		// ... What
		public System.Timers.Timer singleTimer = new System.Timers.Timer(2000);
        public System.Timers.Timer allTimer = new System.Timers.Timer(9000);
        int colorIndex = 0;
        public DateTime start;
        public DateTime starta;
		
		public RadarChatFormGTK(RadarChatPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			rc=p;
			this.Build();
			
			singleTimer.Elapsed += new System.Timers.ElapsedEventHandler(singleFire);
            allTimer.Elapsed += new System.Timers.ElapsedEventHandler(allFire);
			
			// Set up columns
            nodRadar.AppendColumn ("Name", new Gtk.CellRendererText (), "text", 0);
            nodRadar.AppendColumn ("UUID", new Gtk.CellRendererText (), "text", 1);

		}
		public int RadarChannel
		{
			get { return this.spinChannel.ValueAsInt;}
		}
		public bool IsRadarEnabled
		{
			get { return this.chkEnabled.Active; }
			set { this.chkEnabled.Active=value; }
		}
		public string Delimiter
		{
			get { return this.txtDelimiter.Text; }
		}
		public Gtk.NodeView RadarList
		{
			get
			{
				return this.nodRadar;
			}
			set
			{
				this.nodRadar=value;
			}
		}
        public void incColor()
        {
            colorIndex++;
            if (colorIndex >= rc.plugin.SharedInfo.rainbow.Length)
            {
                colorIndex = 0;
            }
        }
        public void sendParticleCircle(UUID who, float dur)
        {
            ViewerEffectPacket p = new ViewerEffectPacket();
            p.AgentData = new ViewerEffectPacket.AgentDataBlock();
            p.AgentData.AgentID = rc.plugin.frame.AgentID;
            p.AgentData.SessionID = rc.plugin.frame.SessionID;
            p.Effect = new ViewerEffectPacket.EffectBlock[1];
            p.Effect[0] = new ViewerEffectPacket.EffectBlock();
            p.Effect[0].AgentID = rc.plugin.frame.AgentID;
            p.Effect[0].Duration = dur;
            p.Effect[0].ID = UUID.Random();
            p.Effect[0].Type = (byte)7;
            p.Effect[0].TypeData= new byte[56];
            Buffer.BlockCopy(who.GetBytes(),0,p.Effect[0].TypeData,0,16);
            Buffer.BlockCopy(UUID.Zero.GetBytes(),0,p.Effect[0].TypeData,16,16);
            Buffer.BlockCopy(Vector3d.Zero.GetBytes(), 0, p.Effect[0].TypeData, 32, 24);
            p.Header.Reliable = true;
            p.Effect[0].Color = new byte[4];
            
            Buffer.BlockCopy(
                rc.plugin.SharedInfo.rainbow[colorIndex], 0, p.Effect[0].Color, 0, 4);



            rc.plugin.proxy.InjectPacket(p, GridProxy.Direction.Outgoing);
            rc.plugin.proxy.InjectPacket(p, GridProxy.Direction.Outgoing);

        }
        public void sendMassSwirl(List<UUID> u,float dur)
        {
            ViewerEffectPacket p = new ViewerEffectPacket();
            p.AgentData = new ViewerEffectPacket.AgentDataBlock();
            p.AgentData.AgentID = rc.plugin.frame.AgentID;
            p.AgentData.SessionID = rc.plugin.frame.SessionID;
            p.Effect = new ViewerEffectPacket.EffectBlock[u.Count];
            for(int i = 0;i<u.Count;i++)
            {
                p.Effect[i] = new ViewerEffectPacket.EffectBlock();
                p.Effect[i].AgentID = rc.plugin.frame.AgentID;
                p.Effect[i].Duration = dur;
                p.Effect[i].ID = UUID.Random();
                p.Effect[i].Type = (byte)7;
                p.Effect[i].TypeData= new byte[56];
                Buffer.BlockCopy(u[i].GetBytes(),0,p.Effect[i].TypeData,0,16);
                Buffer.BlockCopy(UUID.Zero.GetBytes(),0,p.Effect[i].TypeData,16,16);
                Buffer.BlockCopy(Vector3d.Zero.GetBytes(), 0, p.Effect[i].TypeData, 32, 24);
                //rc.plugin.proxy.writethis("sent in " + u[i].ToString(), ConsoleColor.Black, ConsoleColor.Cyan);
                p.Effect[i].Color = new byte[4];
                 Buffer.BlockCopy(
                rc.plugin.SharedInfo.rainbow[colorIndex], 0, p.Effect[i].Color, 0, 4);
            }
            p.Header.Reliable = true;

            rc.plugin.proxy.InjectPacket(p, GridProxy.Direction.Outgoing);
            rc.plugin.proxy.InjectPacket(p, GridProxy.Direction.Outgoing);

        }
		public string GetSelectedUUID()
		{
			Gtk.TreePath[] tp = nodRadar.Selection.GetSelectedRows();
			if(tp.Length>0)
			{
				AvatarNode an = (AvatarNode)nodRadar.NodeStore.GetNode(tp[0]);
				return an.UUID;
			} else {
				return UUID.Zero.ToString();
			}
		}
        public void singleFire(object sender, System.Timers.ElapsedEventArgs e)
        {
            if ((((TimeSpan)(System.DateTime.Now - start)).TotalSeconds > 60) && (new UUID(GetSelectedUUID()).Equals(rc.plugin.frame.AgentID)==false))
            {
                singleTimer.Stop();
                //checkBox2swilyone.Checked = false;
            }
            sendParticleCircle(new UUID(GetSelectedUUID()),(float)(singleTimer.Interval/1000));
            incColor();
        }
        public void allFire(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (((TimeSpan)(System.DateTime.Now - starta)).TotalSeconds > 60)
            {
                allTimer.Stop();
                //checkBox2swirlall.Checked = false;
            }
            
            List<UUID> uids = new List<UUID>();
            foreach (KeyValuePair<UUID, String> i in rc.plugin.SharedInfo.key2name)
            {
                
               /* uids.Add(i.Key);
                if(uids.Count>2)
                {
                    sendMassSwirl(uids,5.0f);
                    uids.Clear();
                }*/
                sendParticleCircle(i.Key, (float)(allTimer.Interval/1000));
            }
            
            
            /*if (uids.Count > 0)
            {
                sendMassSwirl(uids,5.0f);
            }*/

            incColor();
        }
        public void readData()
        {
            bool enabled = true;
            string delim = ",";
            int chan = -777777777;
            try
            {
                if (File.Exists("radarchat.settings"))
                {
                    StreamReader re = File.OpenText("radarchat.settings");
                    if (re.ReadLine() != "Enabled")
                        enabled = false;
                    chan = int.Parse(re.ReadLine().ToString()); ;
                    delim = re.ReadLine();


                    re.Close();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                chkEnabled.Active = enabled;
                txtDelimiter.Text = delim;
                spinChannel.Value = chan;
            }
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("radarchat.settings");
            if (chkEnabled.Active) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.WriteLine(spinChannel.Value.ToString());
            tw.WriteLine(txtDelimiter.Text);
            tw.Close();
        }

        protected virtual void OnCmdResetChanActivated (object sender, System.EventArgs e)
        {
			spinChannel.Value = -777777777;
        }

        protected virtual void OnChkEnableActivated (object sender, System.EventArgs e)
        {
			saveData();
        }

        protected virtual void OnSpinChannelChanged (object sender, System.EventArgs e)
        {
			saveData();
        }

        protected virtual void OnTxtDelimiterChanged (object sender, System.EventArgs e)
        {
			saveData();
        }
	}
}
