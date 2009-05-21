/*
 * Copyright (c) 2009,  Gregory Hendrickson (LordGregGreg Back)
 * 
 *All rights reserved
 *
 *Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

 *THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Gtk;

namespace PubComb
{
    public partial class TabItem : Gtk.Window
    {
        public PubComb pc;
        public TabItem(PubComb o) : base(Gtk.WindowType.Toplevel)
		{
            pc = o;
            //this.Build();
        }
        public void addATab(Window targform,string s)
        {
			
            NotebookPage newtab = new Gtk.tab;
            
			Frame pp = new Gtk.Frame();
            
            pp.Name = "placehold";

            Frame p = new Gtk.Frame();
 
            p.Name = "panel444";
            foreach(Widget newcontrol in targform.Children)
            {
                p.Add(newcontrol);
            }
	        newtab.Controls.Add(pp);
	        newtab.Controls.Add(p);
	        newtab.BackColor = Color.Black;
	        newtab.ForeColor = Color.Red;
            tabControl1.Controls.Add(newtab);
        }

        private void TabItem_Load(object sender, EventArgs e)
        {
            pc.addMahTabs();
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
			/*
			 * No more of this shit please. -- Fred
            try
            {
                //This line of code will help you to change the apperance like size,name,style.
                Font f;
                //For background color
                Brush backBrush;
                //For forground color
                Brush foreBrush;

                //This construct will hell you to deside which tab page have current focus
                //to change the style.
                if (e.Index == this.tabControl1.SelectedIndex)
                {
                    //This line of code will help you to change the apperance like size,name,style.
                    f = new Font(e.Font, FontStyle.Bold | FontStyle.Bold);
                    f = new Font(e.Font, FontStyle.Bold);

                    backBrush = new System.Drawing.SolidBrush(Color.Black);
                    foreBrush = new LinearGradientBrush(e.Bounds, Color.Red, Color.OrangeRed, LinearGradientMode.Horizontal); 
                    
                }
                else
                {
                    f = e.Font;
                    //backBrush = new SolidBrush(e.BackColor);
                    backBrush = new SolidBrush(Color.Black);
                    backBrush = new LinearGradientBrush(e.Bounds, Color.Red,Color.OrangeRed,LinearGradientMode.Horizontal); 
                    foreBrush = new SolidBrush(Color.Black);
                    //foreBrush = new SolidBrush(e.ForeColor);
                }

                //To set the alignment of the caption.
                string tabName = this.tabControl1.TabPages[e.Index].Text;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;

                //Thsi will help you to fill the interior portion of
                //selected tabpage.
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                Rectangle r = e.Bounds;
                r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
                e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

                sf.Dispose();
                if (e.Index == this.tabControl1.SelectedIndex)
                {
                    f.Dispose();
                    backBrush.Dispose();
                }
                else
                {
                    backBrush.Dispose();
                    foreBrush.Dispose();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
*/
        }


    }

}
public interface GTabPlug
{

    void LoadNow();


}