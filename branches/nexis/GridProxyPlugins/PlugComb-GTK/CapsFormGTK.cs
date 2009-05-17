
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
			Console.WriteLine("Test");
			this.nodEnabled.Model=nsEnabled;
			this.nodDisabled.Model=nsDisabled;
			this.nodEnabled.AppendColumn("Enabled CAPS",new CellRendererText(),"text",0);
			this.nodDisabled.AppendColumn("Disabled CAPS",new CellRendererText(),"text",0);
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
						// Broken as hell, just let the user redo it since this is unsafe anyway.
                        //Move(line, false);                        
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
		
		
		private void updateBlocked()
        {
			try
			{
	            lock (dcp.proxy.BlockCaps)
	            {
	                dcp.proxy.BlockCaps.Clear();
	                foreach (string line in nsDisabled)
	                {
	                    dcp.proxy.BlockCaps.Add(line);
	                }
	                saveData();
	            }
			} catch(Exception e)
			{
				Console.WriteLine(e.ToString());
				//Con
			}
        }

		protected virtual void OnNodEnabledRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			    
			if(o!=null)
			{
		        //Gtk.NodeSelection selection = (Gtk.NodeSelection) o;
		        //TreeNode node = (TreeNode) selection.SelectedNode;
				TreeIter i = new TreeIter();
				if(nsEnabled.GetIter(out i,args.Path))
				{
					string l=(string)nsEnabled.GetValue(i,0);
					nsEnabled.Remove(ref i);
					nsDisabled.AppendValues(new string[]{l});
					saveData();
					updateBlocked();
				}
			}
		}

		protected virtual void OnNodDisabledRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			if(o!=null)
			{
		        //Gtk.NodeSelection selection = (Gtk.NodeSelection) o;
		        //TreeNode node = (TreeNode) selection.SelectedNode;
				TreeIter i = new TreeIter();
				if(nsDisabled.GetIter(out i,args.Path))
				{
					string l=(string)nsDisabled.GetValue(i,0);
					nsDisabled.Remove(ref i);
					nsEnabled.AppendValues(new string[]{l});
					saveData();
					updateBlocked();
				}
			}
		}
	}
}
