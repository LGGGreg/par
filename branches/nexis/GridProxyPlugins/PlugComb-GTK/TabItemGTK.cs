
using System;
using Gtk;

namespace PubComb
{
	
	
	public partial class TabItemGTK : Gtk.Window
	{
		public PubComb pc;
		
		public TabItemGTK(PubComb o) : 
				base(Gtk.WindowType.Toplevel)
		{
			pc=o;
			this.Build();
		}
		
		public void addATab(Window w,string title)
		{
			Label tabname = new Label(title);
			//Label tabBody = new Label();
			nbTabs.AppendPage(w,tabname);
		}
	}
	public interface GTabPlug
	{
	    void LoadNow();
	}
}
