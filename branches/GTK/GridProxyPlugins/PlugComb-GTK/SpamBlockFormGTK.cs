
using System;
using System.IO;
namespace PubComb
{
	
	
	public partial class SpamBlockFormGTK : Gtk.Window
	{
		private SpamBlocker sb;
		
		public SpamBlockFormGTK(SpamBlocker p) : 
				base(Gtk.WindowType.Toplevel)
		{
			sb=p;
			this.Build();
		}
		
#region Support Functions
		public bool CheckMod
        {
            get{return this.chkModRights.Active;}
        }
        public bool CheckIM
        {
            get{return this.chkIM.Active;}
        }
        public bool CheckDiag
        {
            get{return chkDialog.Active;}
        }
		public bool CheckSound
		{
			get{return chkSound.Active;}
		}
		public bool CheckMap
		{
			get{return chkMap.Active;}
		}
		public string Debug
		{
			get{return txtDebug.Buffer.Text;}
			set{txtDebug.Buffer.Text=value;}
		}
#endregion
		// Why, Greg, Why?!
        protected virtual void OnChkSoundToggled (object sender, System.EventArgs e)
    	{
			if (File.Exists("mutesound.on"))
            {
                
                File.Delete("mutesound.on");
            }
            else
            {
                File.Create("mutesound.on");
            }
    	}	
	}
}
