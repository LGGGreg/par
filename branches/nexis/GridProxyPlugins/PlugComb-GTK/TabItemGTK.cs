
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
			//this.parLogo.ImageProp=
			this.pc.addMahTabs(this);
		}
		
		public void addATab(Window w,string title)
		{
			try
			{
				// HACK
				//w.HideAll();
				Widget wc = w.Child;
				
				Label tabname = new Label(title);
				wc.Reparent(nbTabs);
				nbTabs.SetTabLabel(wc,tabname);
				w.Dispose();
				
				Console.WriteLine("Disposing of "+title+".");
				
				nbTabs.ShowAll();
			}
			catch(Exception)
			{
				Console.WriteLine("Failed to load "+title+".");
			}
		}
	}
	public interface GTabPlug
	{
	    void LoadNow(ref TabItemGTK tabform);
	}
}
