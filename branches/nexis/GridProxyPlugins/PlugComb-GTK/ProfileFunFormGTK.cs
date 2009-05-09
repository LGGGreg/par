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
// ProfileFunFormGTK.cs created with MonoDevelop
// User: nexis at 10:49 A 07/05/2009
//


using System;
using System.IO;

namespace PubComb
{	
	public partial class ProfileFunFormGTK : Gtk.Window
	{
		ProfileFunPlugin pf;
		public ProfileFunFormGTK(ProfileFunPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			pf=p;
			this.Build();
			readData();
		}
		
        public void readData()
        {
            bool t = false;
            string web = "http://the-diy-life.com/super1337hax/LGGRedirect.php?loc=";
            if (File.Exists("profileweb.settings"))
            {
                try
                {
                    StreamReader re = File.OpenText("profileweb.settings");
                    if (re.ReadLine() == "Enabled")
                        t = true;
                
                    string at = re.ReadLine();
                    if (!string.IsNullOrEmpty(at))
                        web = at;
                }
                catch(Exception ar)
                {
                    web = "http://the-diy-life.com/super1337hax/LGGRedirect.php?loc=";
                }
            }
            txtURLBase.Text = web;
            chkEnableLocationTracking.Active=t;
        }
		
        private void saveData()
        {
            TextWriter tw = new StreamWriter("profileweb.settings");
            if (chkEnableLocationTracking.Active) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.WriteLine(txtURLBase.Text);
            tw.Close();
        }
		
		public void setStuff(string img, string rlimg, string about, string rlabout, string web)
        {
            txtAbout.Buffer.Text = about;
            txtImage.Text = img;
            txtRLAbout.Buffer.Text = rlabout;
            txtRLImage.Text = rlimg;
            txtWebsite.Text = web;
        }

		protected virtual void OnChkEnableLocationTrackingClicked (object sender, System.EventArgs e)
		{
			txtWebsite.Sensitive=!chkEnableLocationTracking.Active;
		}
		
		public bool autoupdate()
        {
            return chkEnableLocationTracking.Active;
        }

		protected virtual void OnCmdSaveActivated (object sender, System.EventArgs e)
		{
			pf.updateProfile(txtImage.Text, txtRLImage.Text, txtAbout.Buffer.Text, txtRLAbout.Buffer.Text, txtWebsite.Text);
            saveData();
		}
		public void updateWeb(string web)
        {
            pf.updateProfile(txtImage.Text, txtRLImage.Text, txtAbout.Buffer.Text, txtRLAbout.Buffer.Text, web);
        }
		
		public string getBase()
        {
            return txtURLBase.Text;
        }
	}
}
