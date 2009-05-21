// MainWindow.cs created with MonoDevelop
// User: nexis at 10:20 A 14/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using System.IO;
using System.Xml;
public partial class MainWindow: Gtk.Window
{	
	[Gtk.TreeNode (ListOnly=true)]
	public class PluginNode:Gtk.TreeNode
	{
		public bool Broken=false;
		
		[Gtk.TreeNodeValue (Column=0)]
		public bool Enable=false;
		
		[Gtk.TreeNodeValue (Column=1)]
		public string Name="Unknown";
		
		[Gtk.TreeNodeValue (Column=2)]
		public string Filename = "brokenplugin.dll";
		
		[Gtk.TreeNodeValue (Column=3)]
		public string Author="Broken Plugin Author";
		
		[Gtk.TreeNodeValue (Column=4)]
		public string Version="0.0.0.0";
		
		public string Description = "This is not actually a plugin.  Seriously.";
		
		public PluginNode(string name,string filename,string author,string version,string desc)
		{
			this.Name=name;
			this.Filename=filename;
			this.Author=author;
			this.Version=version;
			this.Description=desc;
		}
	}
	//protected NodeView PluginList;
	//protected NodeStore PlugStore=new NodeStore();
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		PluginList.NodeStore=new NodeStore(typeof(PluginNode));
		int i =0;
		CellRendererToggle crt = new Gtk.CellRendererToggle();
		crt.Activatable=true;
		crt.Mode=CellRendererMode.Activatable;
		crt.Toggled+=crt_toggled;
		PluginList.AppendColumn ("Enable", crt, "active", i++);
		PluginList.AppendColumn ("Name", new Gtk.CellRendererText(), "text", i++);
		PluginList.AppendColumn ("Filename", new Gtk.CellRendererText(), "text", i++);
		PluginList.AppendColumn ("Author", new Gtk.CellRendererText(), "text", i++);
		PluginList.AppendColumn ("Version", new Gtk.CellRendererText(), "text", i++);
        PluginList.ShowAll ();
		PluginList.NodeSelection.Changed += new System.EventHandler(OnSelectionChanged);
		//PluginList.
		//actRefresh.Visible=false;
		this.LoadPluginMetadata();
	}
	
	void crt_toggled(object o, ToggledArgs args)
	{
		PluginNode p = (PluginNode)PluginList.NodeStore.GetNode(new TreePath(args.Path));
		if (p!=null)
		{
			p.Enable=!p.Enable;
		}
	}
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void LoadPluginMetadata()
	{
		try
		{
			string sep = Directory.GetCurrentDirectory();
			sep+="/Plugins/";
			XmlReader r= XmlReader.Create("plugindata.xml");             
			// Parse the XML document.  ReadString is used to 
			//read the text content of the elements.
			while(r.Read())
			{
				switch(r.NodeType)
				{
				case XmlNodeType.Element:
					if(r.Name.Equals("Plugin"))
					{
						string filename=r.GetAttribute("Filename");
						// Broken?  Skippit.
						if(r.GetAttribute("Broken")!=null)
							continue;
						PluginNode pn = new PluginNode(
							                                   r.GetAttribute("Name"),
							                                   System.IO.Path.Combine(sep,filename),
							                                   r.GetAttribute("Author"),
							                                   r.GetAttribute("Version"),
							                                   ""
							                                   );
						//r.MoveToContent();
						pn.Description=r.ReadElementContentAsString();
						this.PluginList.NodeStore.AddNode(pn);
					}
					break;
				}
			}
		} catch(System.Exception e)
		{
			MessageDialog md = new MessageDialog (this, 
			                                      DialogFlags.DestroyWithParent,
			                                      MessageType.Error, 
			                                      ButtonsType.Close, 
			                                      "Exception occurred: {0}",e.ToString());
	
			ResponseType result = (ResponseType)md.Run ();

			if (result == ResponseType.Close)
				Application.Quit();
			else
				md.Destroy();
		}

	}
    void OnSelectionChanged (object o, System.EventArgs args)
    {
		if(o!=null)
		{
            Gtk.NodeSelection selection = (Gtk.NodeSelection) o;
            PluginNode node = (PluginNode) selection.SelectedNode;
            
			
			if(node!=null)
			{
				lblPluginData.Text = "Name: "+node.Name+"\n"
				+"Author: " + node.Author + "\n"
				+"Version: " + node.Version+"\n\n"+node.Description;
				
				if(node.Enable)
				{
					actEnable.Visible=false;
					actDisable.Visible=true;
				} else {
					actEnable.Visible=false;
					actDisable.Visible=true;
				}
			}
		} else {
			actEnable.Visible=false;
			actDisable.Visible=false;
		}
    }

    protected virtual void OnActLaunchActivated (object sender, System.EventArgs e)
    {
		string cmdline="GridProxyApp.exe --proxy-login-port=8080";
		int numloaded=0;
		foreach(PluginNode n in PluginList.NodeStore)
		{
			if(n.Enable)
			{
				cmdline+=" --load="+n.Filename;
				numloaded++;
			}
		}
		if(numloaded==0)
		{
			MessageDialog md = new MessageDialog(
			                  this,
			                  DialogFlags.DestroyWithParent,
			                  MessageType.Error,
			                  ButtonsType.Ok,
			                  "No plugins selected.  Please try again."
			                  );
			md.Run();
			return;
		} else {
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.Arguments=cmdline;
			proc.StartInfo.FileName="mono";
			string wp = String.Format("{0}/ShadowFiles/",Directory.GetCurrentDirectory());
			proc.StartInfo.WorkingDirectory=wp;
			Console.WriteLine("Executing mono "+cmdline);
			proc.Start();
		}
    }

    protected virtual void OnExit (object sender, System.EventArgs e)
    {
		Application.Quit();
    }

}