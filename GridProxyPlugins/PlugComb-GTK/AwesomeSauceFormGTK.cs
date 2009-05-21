
using System;

namespace PubComb
{	
	public partial class AwesomeSauceFormGTK : Gtk.Window
	{
		//AwesomeSauce a;
		public AwesomeSauceFormGTK(AwesomeSauce p) : 
				base(Gtk.WindowType.Toplevel)
		{
			//a=p;
			this.Build();
		}
		
		/// <summary>
		/// Log something.
		/// </summary>
		/// <param name="text">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="color">
		/// A <see cref="System.Drawing.Color"/> Not used.
		/// </param>
		/// <param name="bcolor">
		/// A <see cref="System.Drawing.Color"/> Not used.
		/// </param>
		public void Log(string text, System.Drawing.Color color, System.Drawing.Color bcolor)
		{
			txtLog.Buffer.Text=txtLog.Buffer.Text+text+Environment.NewLine;
		}
	}
}
