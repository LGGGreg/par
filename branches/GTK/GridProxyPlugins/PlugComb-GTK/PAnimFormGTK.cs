// (c)2009 Rob "N3X15" Nelson, Gregory "LordGregGreg" Hendrickson. All Rights Reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this 
// 		list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this 
// 		list of conditions and the following disclaimer in the documentation and/or 
// 		other materials provided with the distribution.
//  * Neither the name of the  Gregory Hendrickson nor the names of its contributors 
// 		may be used to endorse or promote products derived from this software without 
// 		specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
namespace PubComb
{
	public partial class PAnimFormGTK : Gtk.Window
	{
		
		public PAnim pp;
        public string brand;
        public PAnimFormGTK(PAnim p) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.pp=p;
			this.brand="ProtAnim"; // ... Why?  Why not just use static strings?...
			this.Build();
			readData();
		}
		
		public void readData()
        {
            int dex = 5;
            if (File.Exists("ProtAnim.settings"))
            {
                StreamReader re = File.OpenText("ProtAnim.settings");
                dex= int.Parse(re.ReadLine().Trim());
                re.Close();
            }
            cboPLevel.Active = dex;

        }
        
        private void saveData()
        {
            TextWriter tw = new StreamWriter("ProtAnim.settings");
            tw.WriteLine(cboPLevel.Active);
            tw.Close();
        }
		
		// @TODO:  More descriptive function names, these are pissing me off. -- nexis
		public int getChecked()
        {
            return cboPLevel.Active; // Active = ActiveIndex in GTK 
        }
	}
}
