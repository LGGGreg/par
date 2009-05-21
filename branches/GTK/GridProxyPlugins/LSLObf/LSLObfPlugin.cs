// 
// LSLObfPlugin.cs
//  
// Author:
//       Rob "N3X15" Nelson <nexisentertainment@gmail.com>
// 
// Copyright (c) 2009 Nexis Entertainment
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.StructuredData;
using System;
using GridProxy;

namespace LSLObf
{
	public class LSLObfPlugin : ProxyPlugin
	{
		private Proxy proxy;
		private ProxyFrame frame;
		private Obfuscator o;
		private Nini.Config.ConfigBase cfg;
		
		public LSLObfPlugin(ProxyFrame frame)
		{
			cfg=new Nini.Config.ConfigBase("LSLObf",new Nini.Config.IniConfigSource("LSLObf.ini"));
			this.frame=frame;
			this.proxy=frame.proxy;
		}
		
		public override void Init()
		{
			this.frame.AddCommand("/lslobf",new ProxyFrame.CommandDelegate(this.ParseCommand));
			this.frame.proxy.AddCapsDelegate("NewFileAgentInventory",new CapsDelegate(OnNewFile));
			
			this.frame.SayToUser(
				"LSL Obfuscator ready.\n"+
			    " * /lslobf on - Enables the obfuscator."+
			    " * /lslobf off - Turns it off."
			);
		}
		
		public void ParseCommand(string[] words)
		{
		}
		public bool OnNewFile(CapsRequest req,CapsStage stage)
		{
			if(!stage.Equals(CapsStage.Request))
				return false;
			
			OSDMap data = (OSDMap)OSDParser.DeserializeLLSDXml(req.RawRequest);
			if(Utils.StringToAssetType(data["asset_type"].AsString()).Equals(AssetType.LSLText))
			{
				Console.WriteLine("!!! NewFileAgentInventory Received !!!");
				Console.WriteLine(data.ToString());
			}
			return false;
		}
	}
}
