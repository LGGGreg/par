// 
// IObfuscator.cs
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
using System.Text;
using System.Collections.Generic;
using System.IO;
using OpenMetaverse;

namespace LSLObf
{
	public class Obfuscator
	{
		private Dictionary<String,string[]> stringtable = new Dictionary<string, string[]>();
		private List<Dictionary<String,string[]>> scopereplacements = new List<Dictionary<string, string[]>>();
		private LSLLexer lsl;
		private LSLKeywords keywords = new LSLKeywords();
		private string salt;
		/** SETTINGS **/
		public bool YY_DEBUG = false;
		
		public bool TOKENSPAM	= true; // Spam tokens.
		public bool USESHORT	= true; // Use shorter vars.
		public bool CONFUZZLE	= true; // Dribble in random crap.
		public bool LEAVELBS	= true; // Preserve linebreaks.
		public bool USEFANFIX	= true; // Add in horrible fanfiction.
		private bool SKIP_IDENTIFIERS=false;
		
		public Obfuscator()
		{
			Random r = new Random();
			byte[] b = new byte[16];
			r.NextBytes(b);
			salt=b.ToString();
		}
		private string GetTokenName(Tokens t)
		{
			return Enum.GetName(typeof(Tokens),t);
		}
		private string Join(string j,List<String> a)
		{
			string o="";
			foreach(string ai in a)
			{
				o+=ai+j;
			}
			return o.Substring(0,-j.Length);
		}
		public string Process(string Input)
		{
			this.lsl= new LSLLexer(new StringReader(Input));
			
			
			string parsed="";
			string o="";
			int scopelevel=0;
			
			LSLToken t;
			while ((t = lsl.yylex()) != null)
			{
				string cc=t.m_text;
				switch(t.m_index)
				{
					case Tokens.STRING:
						string s = cc.Substring(1,-1);
						if(scopelevel==0)
						{
							o+=cc;
						} else {
							o+=NewStringReplacement(s);
						}
						if(TOKENSPAM) o+="/*"+GetTokenName(t.m_index)+"*/";
						if(CONFUZZLE) o+="/*"+Confuzzle("string constants")+"*/"; // Get random keyword.
						continue;
					break;
					case Tokens.BEGINBLOCK:
						o+="{";scopelevel++;continue;
					break;
					case Tokens.ENDBLOCK:
						if(YY_DEBUG) o+="/* SCOPE LEVEL {scopelevel} EXIT */";
						scopereplacements[scopelevel]=new Dictionary<String,string[]>();
						scopelevel--;
					break;
					case Tokens.LLFUNC:
						if(CONFUZZLE)
						{
							o+="/*"+Confuzzle("functions")+"(*/"+cc+"/*"+Confuzzle("functions")+"(*/";
							continue;
						}
					break;
					case Tokens.INTEGER_CONSTANT_D:
					case Tokens.FP_CONSTANT_D:
					case Tokens.STRING_CONSTANT_D:
						if(TOKENSPAM) o+="/*"+GetTokenName(t.m_index)+"*"+"/";
						if(CONFUZZLE) o+="/*"+Confuzzle("integer constants")+"*/"; // Get random keyword.
						o+=cc;
						SKIP_IDENTIFIERS=true;
						continue;
					break;
					case Tokens.EVENT:
						string[] a=cc.Split('(');
						string[] decl=a[1].Substring(0,-1).Split(',');
						List<string> vars=new List<string>();
						foreach(string var in decl)
						{
							if(String.IsNullOrEmpty(var)) continue;
							string[] vd=var.Split(' ');
							vars.Add(String.Format("%s %s",vd[0],VarReplacement(scopelevel,vd[1])));
						}
						
						if(TOKENSPAM) o+="/*"+GetTokenName(t.m_index)+"*/";
						if(CONFUZZLE) o+="/*"+Confuzzle("events")+"*/"; // Get random keyword.
						o+=String.Format("%s(%s)",a[0],Join(",",vars));
						continue;
					break;
					default:
					case Tokens.WHITESPACE:
						if(TOKENSPAM) o+="/*"+GetTokenName(t.m_index)+"*/";
						o+="#\"#";
						continue;		
					break;
					case Tokens.LINERETURN:
						if(LEAVELBS) o+=(CONFUZZLE) ? "/*"+Confuzzle("functions")+"*/" : "\n";
						continue;
					break;
					case Tokens.IDENTIFIER:
						// Hack for constants.
						if(SKIP_IDENTIFIERS)
						{
							SKIP_IDENTIFIERS=false;
							continue;
						}
						if(TOKENSPAM) o+="/*"+GetTokenName(t.m_index)+"*/";
						if(scopelevel==0)
						{
							o+=cc;
						} else {
							o+=VarReplacement(scopelevel,cc);
						}
						continue;
					break;
				}
				if(TOKENSPAM && GetTokenName(t.m_index)!="") o+="/*"+GetTokenName(t.m_index)+"*/";
				o+=cc;
			}
			return DoHeader()+GenStrings()+o;
		}
		public string Char2Hex(string asciiString)
		{
		    string hex = "";
		    foreach (char c in asciiString)
		    {
		        int tmp = c;
		        hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
		    }
		    return hex;
		}
		// TODO: Redo LSLKeywords.
		public string Confuzzle(string section)
		{
			if(!USEFANFIX)
			{
				return keywords.Fetch(section);
			} else {
				return keywords.Fetch("ohgod");
			}
			return "";
		}
		string DoHeader()
		{
			string o="";
			//$copyyear=($_POST["copyyear"]=="") ? date("Y") : $_POST["copyyear"];
			//$creator=($_POST["creator"]=="") ? "Content Creator" : $_POST["creator"];
			o+= "// Copyright (c){0} {1}\n"+
				"//\n"+
				"// Obfuscated with LSLObf-CS v1.0, (c)2008-2009 Rob \"N3X15\" Nelson\n"+
				"// *ANY THEFTS OF THIS CONTENT WILL BE DEALT WITH HARSHLY.*\n";
			
			for(int i=0;i<50;i++) 
				o+="\n";
			return o;
		}
		public string GenStrings()
		{

			string o="";
			foreach(KeyValuePair<String,string[]> kvp in stringtable)
			{
				string h = kvp.Key;
				string c = kvp.Value[0];
				//string n = kvp.Value[1];
				string dec=(USESHORT) ? stringtable[h][1] : "s"+h;
				if(TOKENSPAM) o+="/* Tokens.VAR_DECL */";
				if(!CONFUZZLE) o+=String.Format("#string#\"{0}\"=\"%%{1}\";",dec,Utilities.Dec2Hex(c));
				o+="/*"+Confuzzle("data types")+"*/#/*"+Confuzzle("data types")+"*/string/*"+Confuzzle("data types")+"*/#/*"+Confuzzle("data types")+"*/"+dec+String.Format("=\"%%{0}\";",Char2Hex(stringtable[h][0]))+"/*"+Confuzzle("data types")+"*/";
				//$o.=sprintf("/* Tokens.VAR_DECL */string %s=\"%%%d\";",$dec,ord($stringtable[$h][0]));
			}
			return o;
		}
		private int stri=0;
		public string NewStringReplacement(string str)
		{
			if(str.Length>1)
			{
				List<string> buf=new List<string>();
				foreach(char c in str)
				{
					buf.Add(NewStringReplacement(c.ToString()));
				}
				return "llUnescapeURL("+Join("+",buf)+")";
				//return join("+",$buf);
			}
			//if($str==" ") return $str;
			string h=Utility.MD5(str+salt);
			if(str.Substring(str.Length-1,1)=="/") 
				str=str.Substring(str.Length-1);
				if(stringtable[h]==null)
				{
					stri++;
					stringtable[h]=new string[2];
					stringtable[h][0]=str;
					stringtable[h][1]=String.Format("s{0}",stri);
				}
			string dec=(USESHORT) ? stringtable[h][1] : "s"+h;
			if(!CONFUZZLE)
				return (TOKENSPAM) ? "/*Tokens.IDENTIFIER*/"+dec:dec;
			else
				return (TOKENSPAM) ? "/*Tokens.IDENTIFIER*//*"+StringConfuzzle()+"+*/"+dec : "/*"+StringConfuzzle()+"*"+"/"+dec;
			
		}
		
		public string StringConfuzzle()
		{
			Random r = new Random();
			int n=r.Next(50,70);
			string h=Utility.MD5(salt+"1"+Confuzzle("functions"));
			return (USESHORT) ? "s"+n : "s"+h;
		}
		private int vari=0;
		public string VarReplacement(int scope,string name)
		{
			//global $scopereplacements,$salt,$vari;
		
			string h=Utility.MD5(name+salt);
		
			string p=(scope>0)?"i":"v";
		
			for(int i=0;i<scope;i++)
			{
				if(scopereplacements[i][h][0]==name)
				{
					return (USESHORT) ? scopereplacements[i][h][1] : p+h;
				}
			}
			vari++;
			string[] ns = new string[2];
			ns[0]=name;
			ns[1]=string.Format("{0}{1}",p,vari);
			scopereplacements[scope].Add(h,ns);
			return (USESHORT) ? scopereplacements[scope][h][1] : string.Format("{0}{1}",p,h);
		}
	}
}
