using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Gtk;
using OpenMetaverse;

namespace PubComb
{
	[Gtk.TreeNode (ListOnly=true)]
	public class AvatarNode:Gtk.TreeNode
	{
	
		public bool Broken=false;
		
		[Gtk.TreeNodeValue (Column=0)]
		public string UUID="-";
		
		[Gtk.TreeNodeValue (Column=1)]
		public string Name="[Waiting]";
		
		[Gtk.TreeNodeValue (Column=2)]
		public string SimIP = "0.0.0.0";
		
		[Gtk.TreeNodeValue (Column=3)]
		public string LocalID="0";
		
		[Gtk.TreeNodeValue (Column=4)]
		public string Pos="<128,128,0>";
		
		
		public AvatarNode(string uuid,string name,string simIP,string localID,string pos)
		{
			this.UUID=uuid;
			this.Name=name;
			this.SimIP=simIP;
			this.LocalID=localID;
			this.Pos=pos;
		}
		
		public AvatarNode(PubComb.Aux_Avatar avatar)
		{
			this.Name=avatar.Name;
			this.UUID=avatar.UUID.ToString();
			this.SimIP=avatar.sim_IP.ToString();
			this.LocalID=avatar.LocalID.ToString();
			this.Pos=avatar.Position.ToString();
		}
	}
    public partial class FormAvatars : Window
    {
        public AvatarTracker a;
		public NodeStore AvatarList; 
        public FormAvatars(AvatarTracker at) : base(Gtk.WindowType.Toplevel)
        {
            a = at;
            InitializeComponent();
			//Build ();
			dataGridView1.NodeStore=new NodeStore(typeof(AvatarNode));
			int i =0;
			
			dataGridView1.AppendColumn ("UUID", new Gtk.CellRendererText(), "text", i++);
			dataGridView1.AppendColumn ("Name", new Gtk.CellRendererText(), "text", i++);
			dataGridView1.AppendColumn ("SimIP", new Gtk.CellRendererText(), "text", i++);
			dataGridView1.AppendColumn ("LocalID", new Gtk.CellRendererText(), "text", i++);
			dataGridView1.AppendColumn ("Pos", new Gtk.CellRendererText(), "text", i++);
	        dataGridView1.ShowAll ();
        }

        public delegate void AddAvatarCallback(PubComb.Aux_Avatar avatar);
        public void AddAvatar1(PubComb.Aux_Avatar avatar)
        {
			this.dataGridView1.NodeStore.AddNode(new AvatarNode(avatar));
        }
        public void AddAvatar(PubComb.Aux_Avatar avatar)
        {
            this.dataGridView1.NodeStore.AddNode(new AvatarNode(avatar));
        }
        public delegate void RemAvatarCallback(PubComb.Aux_Avatar avatar);
        public void RemoveAvatar(PubComb.Aux_Avatar avatar)
        {
			AvatarNode an = new AvatarNode(avatar);
			this.dataGridView1.NodeStore.RemoveNode(an);
            
        }
        public void RemoveAvatar1(PubComb.Aux_Avatar avatar)
        {
			// I don't think this needs a lock{}, probably threadsafe but I'm thread-stupid.
            AvatarNode an = new AvatarNode(avatar);
			this.dataGridView1.NodeStore.RemoveNode(an);
        }

        public delegate void UpdatevatarCallback(PubComb.Aux_Avatar avatar);
        public void UpdateAvatar(PubComb.Aux_Avatar avatar)
        {
			foreach(Gtk.ITreeNode n in dataGridView1.NodeStore)
			{
				AvatarNode an = (AvatarNode)n;
				if(an!=null)
				{
					if(an.UUID.Equals(avatar.UUID.ToString()))
					{
						an=new AvatarNode(avatar);
					}
				}
			}
        }
        public void UpdateAvatar1(PubComb.Aux_Avatar avatar)
        {
            lock (dataGridView1.NodeStore)
            {
				foreach(Gtk.ITreeNode n in dataGridView1.NodeStore)
				{
					AvatarNode an = (AvatarNode)n;
					if(an!=null)
					{
						if(an.UUID.Equals(avatar.UUID.ToString()))
						{
							an=new AvatarNode(avatar);
						}
					}
				}
            }
        }


    }
}
