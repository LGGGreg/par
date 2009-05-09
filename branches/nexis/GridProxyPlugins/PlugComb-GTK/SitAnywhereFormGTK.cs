
using System;
using Gtk;
namespace PubComb
{
	public partial class SitAnywhereFormGTK : Gtk.Window
	{
		private SitAnywherePlugin sit;

		
		public SitAnywhereFormGTK(SitAnywherePlugin sap) : 
				base(Gtk.WindowType.Toplevel)
		{
			sit=sap;
			this.Build();
		}
		
#region Support Functions
		// None, thankfully.
#endregion
		
		protected virtual void OnCmdSitActivated (object sender, System.EventArgs e)
		{
			sit.SitNow();
		}		
	}
}
