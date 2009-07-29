// (c)2009 Rob "N3X15" Nelson. All Rights Reserved.
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

namespace LSLObf
{
	class LSLObfCS
	{
		public static void Main(string[] args)
		{
			switch(args[0])
			{
			case "obfuscate":
			case "compile":
				LSLCompiler compiler = new LSLCompiler();
				for(int i = 1;i<args.Length;i++)
				{
					string arg = args[i];
					if(arg.Substring(0,2).Equals("--"))
					{
						switch(arg)
						{
						case "--help":
							ShowHelp();
							Environment.Exit(0);
							break;
						case "--version":
							ShowVersion();
							Environment.Exit(0);
							break;
						case "--output":
							compiler.Output=args[i+1];
							break;
						}
					}
					else if(arg[0].Equals("-"))
					{
						foreach(char c in arg)
						{
							switch(c)
							{
							case 'V':
								ShowVersion();
								Environment.Exit(0);
								break;
							case 'h':
								ShowHelp();
								Environment.Exit(0);
								break;
							case 'o':
								compiler.Output=args[i+1];
								break;
							}
						}
					}
				}
				break;
			case "help":
			default:
				ShowHelp();
				Environment.Exit(0);
				break;
			}
		}
		
		public static void ShowHelp()
		{
			Console.WriteLine("LSLObf for C# (LSLObf-CS) v1.0.0");
			Console.WriteLine("_________________________________________");
			Console.WriteLine("(c)2009 Rob \"N3X15\" Nelson. All Rights Reserved.");
			Console.WriteLine();
			Console.WriteLine("lslobf obfuscate [-chlotV] [--output outfile] inputfile");
			Console.WriteLine("lslobf compile [-hoV] [--output outfile] inputfile");
		}
		public static void ShowVersion()
		{
			Console.WriteLine("LSLObf for C# (LSLObf-CS) v1.0.0");
			Console.WriteLine("_________________________________________");
			Console.WriteLine(
				"(c)2009 Rob \"N3X15\" Nelson. All Rights Reserved.\n"+
				"\n"+
				"Redistribution and use in source and binary forms, with or without modification,\n"+
				"are permitted provided that the following conditions are met:\n"+
				"\n"+
				" * Redistributions of source code must retain the above copyright notice, this\n"+
				"		list of conditions and the following disclaimer.\n"+
				"\n"+
				" * Redistributions in binary form must reproduce the above copyright notice, this\n"+
				"		list of conditions and the following disclaimer in the documentation and/or\n"+
				"		other materials provided with the distribution.\n"+
				" * Neither the name of the  Gregory Hendrickson nor the names of its contributors\n"+
				"		may be used to endorse or promote products derived from this software without\n"+
				"		specific prior written permission.\n"+
				"\n"+
				"THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" AND\n"+
				"ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED\n"+
				"WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.\n"+
				"IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,\n"+
				"INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT\n"+
				"NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR\n"+
				"PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,\n"+
				"WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)\n"+
				"ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE\n"+
				"+POSSIBILITY OF SUCH DAMAGE."
				                  );
				Console.WriteLine();
				Console.WriteLine("LSL is copyright Linden Lab.  LL hates me, which means I am not affiliated with LL.");
				Console.WriteLine("All other copyrights are (c) their appropriate owners.");
		}
	}
}