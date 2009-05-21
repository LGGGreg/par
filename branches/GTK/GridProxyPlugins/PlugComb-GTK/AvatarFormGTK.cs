// (c)2009 Rob "N3X15" Nelson, Gregory "LordGregGreg" Hendrickson. All Rights Reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this 
// 		list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this 
// 		list of conditions and the following disclaimer in the documentation and/or 
// 		other materials provided with the distribution.
//  * Neither the name of the  Gregory Hendrickson nor the names of its contributors 
// 		may be used to endorse or promote products derived from this software without 
// 		specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.


using System;
using Gtk;
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
	
	public partial class AvatarFormGTK : Gtk.Window
	{
		public AvatarTracker a;
		public NodeStore AvatarList; 
		
		public AvatarFormGTK(AvatarTracker p) : 
				base(Gtk.WindowType.Toplevel)
		{
			a=p;
			this.Build();
			nodAvatars.NodeStore=new NodeStore(typeof(AvatarNode));
			int i =0;
			
			nodAvatars.AppendColumn ("UUID", new Gtk.CellRendererText(), "text", i++);
			nodAvatars.AppendColumn ("Name", new Gtk.CellRendererText(), "text", i++);
			nodAvatars.AppendColumn ("SimIP", new Gtk.CellRendererText(), "text", i++);
			nodAvatars.AppendColumn ("LocalID", new Gtk.CellRendererText(), "text", i++);
			nodAvatars.AppendColumn ("Pos", new Gtk.CellRendererText(), "text", i++);
	        nodAvatars.ShowAll ();
		}
		
        public void AddAvatar(PubComb.Aux_Avatar avatar)
        {
            this.nodAvatars.NodeStore.AddNode(new AvatarNode(avatar));
        }
		
		public void RemoveAvatar(PubComb.Aux_Avatar avatar)
        {
			AvatarNode an = new AvatarNode(avatar);
			this.nodAvatars.NodeStore.RemoveNode(an);
        }
		
		public void UpdateAvatar(PubComb.Aux_Avatar avatar)
        {
			foreach(Gtk.ITreeNode n in nodAvatars.NodeStore)
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
            lock (nodAvatars.NodeStore)
            {
				foreach(Gtk.ITreeNode n in nodAvatars.NodeStore)
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
