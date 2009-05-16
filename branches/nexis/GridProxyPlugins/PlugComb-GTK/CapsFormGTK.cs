
using System;
using Gtk;
using System.IO;
using GridProxy;
namespace PubComb
{
	
	
	public partial class CapsFormGTK : Gtk.Window
	{
		public DisableCapsPlugin dcp;
		public Gtk.ListStore nsEnabled;
		public Gtk.ListStore nsDisabled;
		public CapsFormGTK(DisableCapsPlugin l) : 
				base(Gtk.WindowType.Toplevel)
		{
			nsEnabled=new Gtk.ListStore(typeof(String));
			nsDisabled=new Gtk.ListStore(typeof(String));
			dcp=l;
			this.Build();
			this.nodEnabled.Model=nsEnabled;
			this.nodDisabled.Model=nsDisabled;
			
			string[] caps = {
				"ChatSessionRequest",
	            "CopyInventoryFromNotecard",
	            "DispatchRegionInfo",
	            "EventQueueGet",
	            "GroupProposalBallot",
	            "MapLayer",
	            "MapLayerGod",
	            "NewFileAgentInventory",
	            "ParcelGodReserveForNewbie",
	            "ParcelPropertiesUpdate",
	            "ParcelVoiceInfoRequest",
	            "ProvisionVoiceAccountRequest",
	            "RemoteParcelRequest",
	            "RequestTextureDownload",
	            "SearchStatRequest",
	            "SearchStatTracking",
	            "SendPostcard",
	            "SendUserReport",
	            "SendUserReportWithScreenshot",
	            "StartGroupProposal",
	            "ServerReleaseNotes",
	            "UpdateAgentLanguage",
	            "UpdateGestureAgentInventory",
	            "UpdateGestureTaskInventory",
	            "UpdateNotecardAgentInventory",
	            "UpdateNotecardTaskInventory",
	            "UpdateScriptAgent",
	            "ViewerStartAuction",
	            "UntrustedSimulatorMessage",
	            "ViewerStats",
	            "DispatchRegionInfo",
	            "StartGroupProposal",
	            "ServerReleaseNotes",
	            "SendUserReportWithScreenShot",
	            "UpdateScriptTask"
			};
			foreach(string cap in caps)
			{
				nsEnabled.AppendValues(cap);
			}
			readData();
		}
		
		public void readData()
        {
            
            if (File.Exists("DisableCaps.settings"))
            {
                using (StreamReader r = new StreamReader("DisableCaps.settings"))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        Move(line, false);                        
                    }
                    r.Close();
                }
            }
            updateBlocked();
        }
		private void saveData()
        {
            TextWriter tw = new StreamWriter("DisableCaps.settings");
            foreach (string line in nsDisabled)
            {
                tw.WriteLine(line);
            }
            tw.Close();
        }
		private void Move(string item, bool enabled)
	    {
			if(String.IsNullOrEmpty(item))
				return;
			if(enabled)
			{
				TreeIter i=new TreeIter();
				while(nsEnabled.IterNext(ref i))
				{
					string line=(string)nsEnabled.GetValue(i,0);
					if(line==item)
					{
						nsEnabled.Remove(ref i);
						nsDisabled.AppendValues(line);
					}
				}
			} else {
				TreeIter i=new TreeIter();
				while(nsDisabled.IterNext(ref i))
				{
					string line=(string)nsDisabled.GetValue(i,0);
					if(line==item)
					{
						nsDisabled.Remove(ref i);
						nsEnabled.AppendValues(line);
					}
				}
			}
		}
		
		private void updateBlocked()
        {
			Proxy p = new Proxy(new ProxyConfig("Derp","Derp"));
            lock (dcp.proxy.BlockCaps)
            {
                dcp.proxy.BlockCaps.Clear();
                foreach (string line in nsDisabled)
                {
                    dcp.proxy.BlockCaps.Add(line);
                }
                saveData();
            }
        }

		protected virtual void OnCmdMoveLeftActivated (object sender, System.EventArgs e)
		{
			TreeIter i;
			if(nodDisabled.Selection.GetSelected(out i))
			{
				string sel=(string)nsDisabled.GetValue(i,0);
				if(!String.IsNullOrEmpty(sel))
					Move(sel,false);
			}
		}

		protected virtual void OnCmdMoveRightActivated (object sender, System.EventArgs e)
		{
			TreeIter i;
			if(nodEnabled.Selection.GetSelected(out i))
			{
				string sel=(string)nsEnabled.GetValue(i,0);
				if(!String.IsNullOrEmpty(sel))
					Move(sel,true);
			}
		}
	}
}
