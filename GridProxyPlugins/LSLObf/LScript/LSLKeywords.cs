// 
// LSLKeywords.cs
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

using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using OpenMetaverse.StructuredData; // LLSD

namespace LSLObf
{
	public class LSLKeywords
	{
		const string FILEPATH="./keywords.dat";
		
		const string KeywordsIni="http://svn.secondlife.com/trac/linden/export/2521/trunk/indra/newview/app_settings/keywords.ini";
		const string LScriptLib="http://svn.secondlife.com/trac/linden/browser/trunk/indra/lscript/lscript_library/lscript_library.cpp?format=txt";
		
		private OSDMap Keywords = new OSDMap();
		public LSLKeywords()
		{
			if(!File.Exists(FILEPATH))
				Regenerate();
			else
				Load();
		}
		public void Load()
		{
			Keywords=(OSDMap)OSDParser.DeserializeLLSDBinary(File.ReadAllBytes(FILEPATH));
		}
		public void Regenerate()
		{
			Keywords=new OSDMap();
			Keywords.Add("functions",new OSDArray());
			Keywords.Add("ohgod",new OSDArray());
			GetKeywordsIni();
			GetLScriptLib();
			GetFanfics();
			File.WriteAllBytes(FILEPATH,OSDParser.SerializeLLSDBinary(Keywords));
		}
		private string WGET(string uri)
		{
			WebClient wc = new WebClient();
			wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
			Stream data = wc.OpenRead(uri);
	        StreamReader reader = new StreamReader (data);
	        string s = reader.ReadToEnd ();
	        Console.WriteLine (s);
	        data.Close ();
	        reader.Close ();
			return s;
		}
		private string[] Lines(string uri)
		{
			return WGET(uri).Replace("\r","").Split('\n');
		}
		public void GetKeywordsIni()
		{
			string csection="";
			foreach(string line in Lines(KeywordsIni))
			{
				string l=line.Trim().Replace("\t"," ");
				string[] lw=l.Split(' ');
				switch(lw[0])
				{
					case "#":
						string nc=l.Trim().Substring(2);
						if(nc.IndexOf("PERMISSION")>-1) continue;
						if(nc.Equals("some vehicle params")) continue;
						csection=nc;
						Keywords.Add(nc,new OSDArray());
						continue;
					break;
					case "[":
					case " ":
					case "":
					case "llkeywords":
					case "[word":
						continue;
					break;
					default:
						((OSDArray)Keywords[csection]).Add(lw[0].Trim());
					break;
				}
			}
			
		}
		public void GetLScriptLib()
		{
			
			foreach(string line in Lines(KeywordsIni))
			{
				if(line.IndexOf("addFunction")>-1)
				{
					string l=line.Replace("\t","");
					l=l.Replace(",","");
					string[] ld=l.Trim().Split(' ');
					string cf=ld[4].Replace("\"","").Trim();
					if(cf.IndexOf("ll")==-1) continue;
					if(cf.Length>2)
						((OSDArray)Keywords["functions"]).Add(cf);
				}
			}
		}
		public void GetFanfics()
		{
			string[] files = Directory.GetFiles("ohgod/");
			foreach(string file in files)
			{
				foreach(string line in File.ReadAllLines(file))
				{
					string l=line.Replace("\t"," ").Replace("/"+"*"," ").Replace("*"+"/"," ").Trim();
					if(l.Length>10)
						((OSDArray)Keywords["ohgod"]).Add(l);
				}
			}
		}
		
		public string Fetch(string section)
		{
			OSDArray s = (OSDArray)Keywords[section];
			Random r = new Random();
			int i = r.Next(0,s.Count-1);
			return s[i].AsString();
		}
	}
}
