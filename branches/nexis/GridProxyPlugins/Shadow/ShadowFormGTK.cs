
using System;
using System.IO;
using Gtk;
using Gdk;
//using Pango; // HTML Webkit.
using OpenMetaverse;

namespace PubComb
{
	public partial class ShadowFormGTK : Gtk.Window
	{
		ShadowPlugin sp;
		
		enum ChatLogPriority
		{
			INFO,
			WARNING,
			ERROR
		}
		public ShadowFormGTK(ShadowPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			sp=p;
			try
			{
			this.Build();
			} catch(Exception)
			{
			}
			Gdk.Color c = new Gdk.Color(0,0,0);
			if(Gdk.Color.Parse("#000000",ref c))
				evtChatLog.ModifyBg(Gtk.StateType.Normal,c);
			this.Title="Shadow for GTK";
			//sp=p;
			readData();
			AddInternalLog("Shadow Plugin Ready!");
		}
		#region Logging crap
		public void AddInternalLog(string msg)
		{
			this.lblChatLog.LabelProp+=GetTimestamp()+"<span fgcolor=\"#ccffcc\"> :: "+msg+"</span>\n";
		}
		private string GetTimestamp()
		{
			string ts="<span fgcolor=\"#ffffff\">[";
			ts+=DateTime.Now.ToShortTimeString();
			return ts+"]</span>";
		}
		public void AddChat(string name,string msg)
		{
			this.lblChatLog.LabelProp+=GetTimestamp()+"<span fgcolor=\"#cccccc\">"+name+": "+msg+"</span>\n";
		}
		public void AddDecryptedChat(string name,string msg)
		{
			this.lblChatLog.LabelProp+=GetTimestamp()+"<span fgcolor=\"orange\">[ENC] "+name+": "+msg+"</span>\n";
		}
		public void AddEncryptedChat(string oldc,string newc)
		{
			this.lblChatLog.LabelProp+=GetTimestamp()+"<span fgcolor=\"red\">[ENC] "+oldc+" -> "+newc+"</span>\n";
		}
#endregion
		
		#region Settings
		private void saveData()
        {
            TextWriter tw = new StreamWriter("Shadow.Key");
            tw.WriteLine(this.txtKey.Text);
            tw.WriteLine(this.sp.brand);
            tw.WriteLine(this.sp.indicator);
            tw.WriteLine(this.sp.trigger);
            tw.Close();
        }
        private void readData()
        {
            if (File.Exists("Shadow.Key"))
            {
                StreamReader re = File.OpenText("Shadow.Key");
                try
                {
                    this.txtKey.Text = re.ReadLine();
                    this.sp.brand= re.ReadLine();
                    this.sp.indicator=re.ReadLine();
                    this.sp.trigger = re.ReadLine();
                }
                catch (System.Exception)
                {
					MessageDialog md = new MessageDialog(
					                                     this, // Parent window
					                                     DialogFlags.DestroyWithParent, // Duh
					                                     MessageType.Error, // Duh
					                                     ButtonsType.Ok, // Duh
					                                     false, // Use pango markup? (<b>, <i>,etc)
					                                     "Error Reading Shadow.Key, corrupt data found\nReverting to default data."
					                                     );
					md.Run();
                    loadDefaults();
                }
                re.Close();
            }
            else
            {
                loadDefaults();
            }
			RefreshSettingsTab();
        }
		private void RefreshSettingsTab()
		{
			txtBrand.Text=this.sp.brand;
			txtTrigger.Text=this.sp.trigger;
		}
        private void loadDefaults()
        {
            this.txtKey.Text = "Enter your key here";
            sp.indicator = "[E]";
            sp.brand = "Shadow";
            sp.trigger = "\\";
        }
#endregion
		
		#region Properties
		public bool ShadowEnabled
		{
			get
			{
				return chkEnabled.Active;
			}
			set
			{
				chkEnabled.Active=value;
			}
		}
		
		public byte[] ShadowKey
		{
			get
			{
				byte[] temp  =	{0x01,0x23,0x45,0x67,0x89,0xAB,0xCD,0xEF,0xFE,0xDC,0xBA,0x98,0x76,0x54,0x32,0x10,0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88,0x99,0xAA,0xBB,0xCC,0xDD,0xEE,0xFF};
	            int count = Utils.StringToBytes(txtKey.Text).Length;
	            if(count>256)count=256;
	            
	            Buffer.BlockCopy(Utils.StringToBytes(txtKey.Text), 0, temp, 0, count);
	            return temp;
			}
		}
		
		public void setKey(string v)
		{
			txtKey.Text=v;
		}

#endregion

		protected virtual void OnCmdOKActivated (object sender, System.EventArgs e)
		{
			saveData();
		}

		protected virtual void OnCmdRevertActivated (object sender, System.EventArgs e)
		{
			readData();
		}
	}
}
