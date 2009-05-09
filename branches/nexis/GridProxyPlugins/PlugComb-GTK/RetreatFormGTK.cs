
using System;
using System.IO;
namespace PubComb
{
	
	
	public partial class RetreatFormGTK : Gtk.Window
	{
		private RetreatPlugin rp;
		public RetreatFormGTK(RetreatPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			rp=p;
			this.Build();
		}
		
		#region Support Functions
		public void readData()
        {
            bool enabled = true;
            if (File.Exists("Retreat.settings"))
            {
                StreamReader re = File.OpenText("Retreat.settings");
                if (re.ReadLine() != "Enabled")
                    enabled = false;


                re.Close();
            }

            chkEnabled.Active = enabled;
        }

        private void saveData()
        {
            TextWriter tw = new StreamWriter("Retreat.settings");
            if (chkEnabled.Active) tw.WriteLine("Enabled");
            else tw.WriteLine("Disabled");
            tw.Close();
        }
		
		// Finally, an appropriately named function.  
		public bool getEnabled()
        {
            return chkEnabled.Active;
        }
#endregion
		
		
	}
}
