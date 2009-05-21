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
using System.IO;
using Gtk;
namespace PubComb
{
	public partial class RainbowParticlesFormGTK : Gtk.Window
	{
		public Gtk.ListStore ColorStore;
		
		public int xtrafx_Index = 0;
        public RainbowParticlesPlugin rp;
        public static String xtrafxString;
        public xtrafxMode Mode = xtrafxMode.Quad;
		
		public RainbowParticlesFormGTK(RainbowParticlesPlugin r) : 
				base(Gtk.WindowType.Toplevel)
		{
			rp = r;
			this.Build();
			this.ColorStore=new Gtk.ListStore(typeof(String));
			nodColors.AppendColumn("Color",new CellRendererText(),"text",0);
			nodColors.Model=ColorStore;
			/*
			this.ColorStore.AppendValues("red");
			this.ColorStore.AppendValues("orange");
            this.ColorStore.AppendValues("yellow");
            this.ColorStore.AppendValues("lime");
            this.ColorStore.AppendValues("green");
            this.ColorStore.AppendValues("turquoise");
            this.ColorStore.AppendValues("blue");
            this.ColorStore.AppendValues("purple");
            this.ColorStore.AppendValues("pink");
            this.ColorStore.AppendValues("black");
            this.ColorStore.AppendValues("white");
            */
		}
		
		#region Support Functions
		public void setColorFromString(string input)
        {
            string[] colors = input.ToLower().Split(new char[3] { ' ', ',', ':' });
            xtrafxString = input;
            rp.plug.SharedInfo.rainbow= new byte[colors.Length][];
            rp.plug.SayToUser("The colors we got were " + input + " and we got " + colors.Length.ToString() + " colors");
			// Threading is slightly different in GTK, no lock needed.
            //lock (listBox1.Items)
            //{
                ColorStore.Clear();
                for (int i = 0; i < colors.Length; i++)
                {
                    ColorStore.AppendValues(colors[i]);
					// CHANGED BY nexis:
					// ToLower, to facilitate proper capitalization in the GUI.
                    switch (colors[i].ToLower())
                    {
                        case "red":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x00, 0x00, 0xFF };
                            break;
                        case "orange":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x80, 0x00, 0xFF };
                            break;
                        case "yellow":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0xFF, 0x00, 0xFF };
                            break;
                        case "lime":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x80, 0xFF, 0x00, 0xFF };
                            break;
                        case "green":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0x00, 0xFF };
                            break;
                        case "turquoise":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0x80, 0xFF };
                            break;
                        case "cyan":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0xFF, 0xFF, 0xFF };
                            break;
                        case "blue":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0xFF, 0xFF };
                            break;
                        case "purple":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x80, 0x00, 0xFF, 0xFF };
                            break;
                        case "pink":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0x00, 0xFF, 0xFF };
                            break;
                        case "black":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0x00, 0xFF };
                            break;
                        case "white":
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                            break;
                        default:
                            rp.plug.SharedInfo.rainbow[i] = new byte[] { 0x00, 0x00, 0x00, 0xFF };
                            rp.plug.SayToUser("Unknown color: " + colors[i] + " set to black");
                            break;
                    }
                }
            //}
            xtrafx_Index = 0;
            saveData();
        }

        private void saveData()
        {
            TextWriter tw = new StreamWriter("SelectionBeam.settings");
            tw.WriteLine(xtrafxString);
            if (Mode == xtrafxMode.Quad) tw.WriteLine("Quad");
            else if (Mode == xtrafxMode.Quad) tw.WriteLine("Single");
            else if (Mode == xtrafxMode.Speak) tw.WriteLine("Speak");
            else if (Mode == xtrafxMode.Circles) tw.WriteLine("Circles");
            else  tw.WriteLine("Disabled");
            tw.Close();
        }
        public void readData()
        {
            string t = "red orange yellow lime green turquoise blue purple pink";
            Mode = xtrafxMode.Quad;
            if (File.Exists("SelectionBeam.settings"))
            {
                StreamReader re = File.OpenText("SelectionBeam.settings");
                t = re.ReadLine();
                string type = re.ReadLine().ToLower();
                if (type.Equals("quad"))
                    Mode = xtrafxMode.Quad;
                else if (type.Equals("single"))
                    Mode = xtrafxMode.Single;
                else if (type.Equals("speak"))
                    Mode = xtrafxMode.Speak;
                else if (type.Equals("circles"))
                    Mode = xtrafxMode.Circles;
                else if (type.Equals("disabled"))
                    Mode = xtrafxMode.Disabled;
                re.Close();
            }
            setMode(Mode);
            setColorFromString(t);
            cboBeamtype.Active = (int)Mode;
        }
        public void setMode(xtrafxMode m)
        {
            Mode = m;
            
            saveData();
        }
		
		public void setColors(String colors)
        {
            setColorFromString(colors);
        }
        public void doList()
        {
            string temp = "";
            foreach(String c in ColorStore)
            {
                temp += c + " ";
            }
            if(temp!="")
            	setColorFromString(temp.Remove(temp.Length-1));
        }
		public void MoveUp()
        {

			TreeIter t;
			TreeIter tr;
			if(nodColors.Selection.GetSelected(out t))//identify the selected item
            {
	            for (int i = 0; i < ColorStore.IterNChildren(); i++)
	            {
					if(ColorStore.IterNthChild(out tr,i)&&tr.Equals(t))
					{
	                    //swap with the top item(move up)
	                    if (i > 0)
	                    {
	                        string bottom = (string)ColorStore.GetValue(t,0);
	                        ColorStore.Remove(ref tr);
							ColorStore.InsertWithValues(i - 1, bottom);
	                        nodColors.Selection.SelectPath(new TreePath(String.Format("0:{0}",(i - 1))));
	                    }
					}
                }
            }
        }
        //Moves the selected items one level down
        public void MoveDown()
        {
            TreeIter t;
			TreeIter tr;
			if(nodColors.Selection.GetSelected(out t))//identify the selected item
            {
	            for (int i = 0; i < ColorStore.IterNChildren(); i++)
	            {
					if(ColorStore.IterNthChild(out tr,i)&&tr.Equals(t))
					{
	                    //swap with the top item(move up)
	                    if (i < ColorStore.IterNChildren()-1)
	                    {
	                        string bottom = (string)ColorStore.GetValue(t,0);
	                        ColorStore.Remove(ref tr);
							ColorStore.InsertWithValues(i + 1, bottom);
	                        nodColors.Selection.SelectPath(new TreePath(String.Format("0:{0}",(i + 1))));
	                    }
					}
                }
            }
        }

		
#endregion

        protected virtual void OnCmdAddColorActivated (object sender, System.EventArgs e)
        {
			// No need to lock, this runs in the same thread as the colorstore.
			this.ColorStore.AppendValues(cboColors.ActiveText);
            doList();
        }

		// Recoded -- nexis
        protected virtual void OnCmdRemoveActivated (object sender, System.EventArgs e)
		{
			TreeIter ti;
			if(nodColors.Selection.GetSelected(out ti))
			{
				this.ColorStore.Remove(ref ti);
			}
		}

        protected virtual void OnCboBeamtypeChanged (object sender, System.EventArgs e)
		{
			// Pays to be careful.
			if(!String.IsNullOrEmpty(cboBeamtype.ActiveText))
			{
				// Changed the enum's values to correspond to the dropdown list.
				this.setMode((xtrafxMode)Enum.Parse(typeof(xtrafxMode),String.Format("{0}",cboBeamtype.Active)));
			}
		}

        protected virtual void OnCmdMoveUpActivated (object sender, System.EventArgs e)
		{
			MoveUp();
			doList();
		}

        protected virtual void OnCmdMoveDownActivated (object sender, System.EventArgs e)
		{
			MoveDown();
			doList();
		}		
	}
}
