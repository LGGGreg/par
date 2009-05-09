// PennyFormGTK.cs created with MonoDevelop
// User: nexis at 10:37 A 07/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;

namespace PubComb
{
	
	
	public partial class PennyFormGTK : Gtk.Window
	{
		private PennyPlugin pp;
		
		public PennyFormGTK(PennyPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			pp=p;
			this.Build();
		}
		
		// TODO:  Refactor to be GetEnabled() or something.  MonoDevelop doesn't have refactoring :/
		public bool getChecked()
        {
            return chkEnabled.Active;
        }

        
        public void readData()
        {
            bool pass = true;
            
            if (File.Exists("penny.settings"))
            {
                StreamReader re = File.OpenText("penny.settings");
                if (re.ReadLine() != "Enabled")
                    pass = false;
                re.Close();
               
            }
            chkEnabled.Active = pass;

        }
        private void saveData()
        {
            TextWriter tw = new StreamWriter("penny.settings");
            if (chkEnabled.Active) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.Close();
        }
	}        
}
