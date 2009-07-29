// 
// Utility.cs
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

namespace LSLObf
{
	public class Utility
	{
		public static void assert(bool expr)
		{ 
			if (false == expr)
				throw new ApplicationException("Error: Assertion failed.");
		}
	  
		private static String[] errorMsg = new String[]
		{
			"Error: Unmatched end-of-comment punctuation.",
			"Error: Unmatched start-of-comment punctuation.",
			"Error: Unclosed string.",
			"Error: Illegal character."
		};
	  
		public const int E_ENDCOMMENT = 0; 
		public const int E_STARTCOMMENT = 1; 
		public const int E_UNCLOSEDSTR = 2; 
		public const int E_UNMATCHED = 3; 
	
		public static void error(int code)
		{
			Console.WriteLine(errorMsg[code]);
		}
		
		public static string MD5(string Value)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
			data = x.ComputeHash(data);
			return System.Text.Encoding.ASCII.GetString(data);
		}
	}
}