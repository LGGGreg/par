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
using OpenMetaverse;
namespace LSLObf
{
	public class Obfuscator
	{
		public delegate void ProgressEvent(double Progress);
		public event ProgressEvent Progress;
		
		string text;
		string otext;
		
		/*
		 * string derp = functionA(in);
		 * 
		 * Becomes:
		 * // Create variables to run function...
		 * string fo1;
		 * string fi1;
		 * // Function input registration...
		 * fi1=in;
		 * // go to function entrance...
		 * jmp if1;
		 * @af1;
		 * // return from function
		 * string derp=fo;
		 * return;
		 * @f1;
		 * 
		 */
		// In case the first part of a variable or function is a number.
		const string PREFIX="o";
		
		List<char> CharsUsed = new List<char>();
		
		List<String> LiteralStrings = new List<string>();
		List<String> States			= new List<string>();
		List<String> llFunctions	= new List<string>();
		
		Dictionary<String,String> Functions=new Dictionary<string, string>();
		
		string[] IgnoreChars={"\t","\r","\n"};
		string[] NewLineChars={"\n","\r"};
		

		
		public Obfuscator(string input)
		{
			text=input;
		}
		
		public bool Obfuscate()
		{
			bool PreviousWasWhitespace=false;
			bool PrevioysWasFSlash=false;
			bool InComment=false;
			bool InText=false;
			for(int i = 0;i<text.Length;i++)
			{
				string c = text.Substring(i,1);
				
				// Remove linebreaks, tabs, etc.
				//  Replace with space.
				if(Array.IndexOf(IgnoreChars,c)==-1)
					c=" ";
				
				// Remove excessive whitespace.
				if(c.Equals(" ") && otext.EndsWith(" "))
					continue;
				
				// End comment at newline, but keep the space.
				if(Array.IndexOf(NewLineChars,text.Substring(i,1))>-1)
				{
					InComment=false;
				}
				
				// Remove comments.
				if(text.Substring(i,2).Equals("//") || InComment)
				{
					InComment=true;
					continue;
				}
			}
			return true;
		}
		public String Input
		{
			get{return text;}
		}
		
		public string ObfuscatedOutput
		{
			get{return otext;}
		}
	}
}
