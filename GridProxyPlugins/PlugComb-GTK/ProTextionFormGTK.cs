
using System;

namespace PubComb
{
	
	
	public partial class ProTextionFormGTK : Gtk.Window
	{
		//private ProTextPlug p;
		public ProTextionFormGTK(ProTextPlug pp) : 
				base(Gtk.WindowType.Toplevel)
		{
			//this.p=pp;
			this.Build();
		}
		
		public bool getEnabled()
        {
            return chkEnabled.Active;
        }
	}
}
