// 
// Tokens.cs
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
	public class Utilities
	{
		public static string Dec2Hex(int input)
		{
			return "0x"+input.ToString("X");
		}
		public static string Dec2Hex(string sinput)
		{
			int input=int.Parse(sinput);
			return "0x"+input.ToString("X");
		}
		public static string Hex2Dec(string input)
		{
			return int.Parse(input, System.Globalization.NumberStyles.HexNumber).ToString("D");
		}
	}
	public enum Tokens
	{
		FP_CONSTANT=1,
 		INC_OP,
 		DEC_OP,
 		ADD_ASSIGN,
		SUB_ASSIGN,
		MUL_ASSIGN,
		DIV_ASSIGN,
		MOD_ASSIGN,
		SEMICOLON,
		BEGINBLOCK,
		ENDBLOCK,
		COMMA,
		EQSIGN,
		OPENPAREN,
		CLOSEPAREN,
		MINUS,
		PLUS,
		ASTERISK,
		DIVIDE,
		PERCENT,
		AT,
		COLON,
		GT,
		LT,
		ENDLIST,
		OPENLIST,
		EQ,
		NEQ,
		GEQ,
		LEQ,
		AMPERSAND,
		PIPE,
		EXPONENT,
		TILDE,
		EXCLAMATION,
		BOOLEAN_AND,
		BOOLEAN_OR,
		SHIFT_LEFT,
		SHIFT_RIGHT,
		WHITESPACE,
		LINERETURN,
		QUOTE,
		CHAR,
		IDENTIFIER,
		INTEGER_CONSTANT,
		PERIOD,
		USERFUNC,
		IF,
		DO,
		WHILE,
		STRING,
		LLFUNC,
		FOR,
		STRING_CONSTANT,
		ELSE,
		JUMP,
		INTEGER_CONSTANT_D,
		FP_CONSTANT_D,
		STRING_CONSTANT_D,
		USERFUNC_DECL,
		TYPECAST,
		STATE,
		VAR_DECL,
		RETURN,
		EVENT,
		STATE_DEFAULT,
		ZERO_VECTOR,
		ZERO_ROTATION,
		TOUCH_INVALID_VECTOR,
		TOUCH_INVALID_TEXCOORD,
		NEWLINE
	}
}
