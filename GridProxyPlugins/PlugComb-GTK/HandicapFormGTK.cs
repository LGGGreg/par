// HandicapFormGTK.cs created with MonoDevelop
// User: nexis at 1:21 A 07/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
namespace PubComb
{
	
	
	public partial class HandicapFormGTK : Gtk.Window
	{
		//private HandicapPlugin handiPlug;
        private string brand = "Handicap";
		
		public HandicapFormGTK(HandicapPlugin hp) : 
				base(Gtk.WindowType.Toplevel)
		{
			//this.handiPlug=hp;
			this.Build();
			readData();
		}
		
        public void setBox(bool a)
        {
            this.chkEnabled.Active = a;
        }
        public bool getcheck()
        {
            return chkEnabled.Active;
        }
        public void addDebugInfo(string d)
        {
            txtDebug.Buffer.Text += "\n" + d;
        }

        public void readData()
        {
            bool ena = true;
            if (File.Exists(this.brand + ".settings"))
            {
                StreamReader re = File.OpenText(this.brand + ".settings");
                ena  = bool.Parse(re.ReadLine().Trim());
                re.Close();
            }
            chkEnabled.Active = ena;

        }

        private void saveData()
        {
            TextWriter tw = new StreamWriter(this.brand + ".settings");
            tw.WriteLine(chkEnabled.Active);
            tw.Close();
        }

        protected virtual void OnChkEnabledClicked (object sender, System.EventArgs e)
        {
			saveData();
        }
	}
}
