/**
 * @file TabItemGTK.cs
 * @brief Form that displays the GTK-ified plugin forms.
 * @version $Id$
 * 
 * (c)2009 Rob "N3X15" Nelson, Gregory "LordGregGreg" Hendrickson
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 *  * Redistributions of source code must retain the above copyright notice, this 
 * 		list of conditions and the following disclaimer.
 * 
 *  * Redistributions in binary form must reproduce the above copyright notice, this 
 * 		list of conditions and the following disclaimer in the documentation and/or 
 * 		other materials provided with the distribution.
 *  * Neither the name of the  Gregory Hendrickson nor the names of its contributors 
 * 		may be used to endorse or promote products derived from this software without 
 * 		specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using Gtk;

namespace PubComb
{
	
	
	public partial class TabItemGTK : Gtk.Window
	{
		public PubComb pc;
		
		public TabItemGTK(PubComb o) : 
				base(Gtk.WindowType.Toplevel)
		{
			pc=o;
			this.Build();
			this.pc.addMahTabs(this);
		}
		
		/// <summary>
		/// Add a tab to the main form
		/// </summary>
		/// <param name="w">
		/// A <see cref="Window"/>.
		/// </param>
		/// <param name="title">
		/// A <see cref="System.String"/> with the title in it.
		/// </param>
		public void addATab(Window w,string title)
		{
			try
			{
				Widget wc = w.Child;
				
				Label tabname = new Label(title);
				wc.Reparent(nbTabs);
				nbTabs.SetTabLabel(wc,tabname);
				w.Hide();
				w.Dispose();
				
				//Console.WriteLine("Disposing of "+title+".");
				
				nbTabs.ShowAll();
			}
			catch(Exception)
			{
				Console.WriteLine("[TabItemGTK] Failed to load "+title+".");
			}
		}
	}
	public interface GTabPlug
	{
	    void LoadNow(ref TabItemGTK tabform);
	}
}
