/*
 * Copyright (c) 2009,  Gregory Hendrickson (LordGregGreg Back) 
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
 * 
 * Rewritten by nexis to run on GTK.  Old license applies.
 */

using System;
using System.IO;
using Gtk;

namespace PubComb
{
	public partial class LyraFormGTK : Gtk.Window
	{
		//LyraPlugin lp;
		public LyraFormGTK(LyraPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			//lp=p;
			this.Build();
		}
		
        public string getbox()
        {
            return this.txtChatTrig.Text;
        }
        public bool getCheck()
        {
            return this.chkEnabled.Active;
        }
        public void setBox(string s)
        {
            this.txtChatTrig.Text = s;
        }
        public void setCheck(bool a)
        {
            this.chkEnabled.Active = a;
        }
		
		public void readData()
        {
            bool pass = false;
            setBox("phantom1");
            if (File.Exists("lyra.settings"))
            {
                StreamReader re = File.OpenText("lyra.settings");
                if (re.ReadLine() == "Enabled")
                  pass = false;
                string t = re.ReadLine();
                setBox(t);

                re.Close();
                
            }
            setCheck(pass);//turn off
        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("lyra.settings");
            if (this.chkEnabled.Active) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.WriteLine(getbox());
            tw.Close();
        }

        protected virtual void OnChkEnabledClicked (object sender, System.EventArgs e)
        {
			saveData();
        }

        protected virtual void OnTxtChatTrigChanged (object sender, System.EventArgs e)
        {
			saveData();
        }
	}
}
