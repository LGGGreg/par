/**
* LSL Lexer
*  Converted to CsLex Syntax by Rob "N3X15/Fred Rookstown" Nelson.
*
* Insert GPL+FLOSS here. Original syntax by Linden Lab.
*/

// Constants.
//require('lllslconstants.php');
//require('lscript_byteformat.php');
//require('indra_constants.php');
//require('SLFilter/uuid.php');

using System;
using System.Text;
using OpenMetaverse;

public class Sample
{
  public static void Main(String[] argv)
  {
  String [] args = Environment.GetCommandLineArgs();
  System.IO.FileStream f = new System.IO.FileStream(args[1], System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 8192);
  LSLLexer yy = new LSLLexer(f);
  LSLToken t;
  while ((t = yy.yylex()) != null)
    Console.WriteLine(t);
  }
}

public class LSLToken  
{
	public LSLToken(Tokens index,String text,int line,int charBegin,int charEnd)
	{
		m_index = index;
		m_text = text;
		m_line = line;
		m_charBegin = charBegin;
		m_charEnd = charEnd;
	}

	public Tokens m_index;
	public String m_text;
	public int m_line;
	public int m_charBegin;
	public int m_charEnd;

	public override String ToString() 
	{
		return "Token #"+ m_index + ": " + m_text  + " (line "+ m_line + ")";
	}
}

%%

%{
private static int comment_count = 0;
%} 
%line
%char
%state COMMENT,INQUOTE,INLINECOMMENT,BLOCKCOMMENT
%namespace LSLObf
%class LSLLexer
%type LSLToken
%public

N = [0-9]
NPLUS= [0-9]+
L = [a-zA-Z_]
H = [a-fA-F0-9]
HEXIDECIMAL=0[xX][a-fA-F0-9]+
E = [\-+]?[0-9]*\.?[0-9]+
TOK_IDENTIFIER = [a-zA-Z_]([a-zA-Z_]|[0-9])*
TOK_LL_IDENTIFIER = ll([a-zA-Z_]|[0-9])*
TOK_FP_A = [0-9]+[\-+]?[0-9]*\.?[0-9]+
TOK_FP_B = [0-9]*"."[0-9]+([\-+]?[0-9]*\.?[0-9]+)?(f|F)?
TOK_FP_C = [0-9]+"."[0-9]*([\-+]?[0-9]*\.?[0-9]+)?(f|F)?
TOK_STRING = .+
WHITESPACE = [ \t\v\f]+
WS = [ \t\v\f]+
WSLR = [ \t\r\n\v\f]+
LINERETURN = [\n\r]+
LINEBREAK = [\n\r]+
TOK_OTHER = .
FUNC_HEADER = \(([a-zA-Z0-9 _,]*)\)

%%

<YYINITIAL> "//"				{ yybegin(INLINECOMMENT); return null;}
<YYINITIAL> "/*"				{ yybegin(BLOCKCOMMENT);return null;}
<YYINITIAL> \"{TOK_STRING}\"	{
					int sz=yytext().Length-2;
					return (new LSLToken(Tokens.STRING,yytext(),yyline,yychar,yychar+sz));
				}
<YYINITIAL> {TOK_LL_IDENTIFIER}\(		{ return (new LSLToken(Tokens.LLFUNC,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "integer"{WHITESPACE}{TOK_IDENTIFIER}	{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "float"{WHITESPACE}{TOK_IDENTIFIER}		{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "string"{WHITESPACE}{TOK_IDENTIFIER}	{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "key"{WHITESPACE}{TOK_IDENTIFIER}		{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "vector"{WHITESPACE}{TOK_IDENTIFIER}	{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "quaternion"{WHITESPACE}{TOK_IDENTIFIER}	{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "rotation"{WHITESPACE}{TOK_IDENTIFIER}	{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "list"{WHITESPACE}{TOK_IDENTIFIER}		{ return (new LSLToken(Tokens.VAR_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(integer)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(float)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(string)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(key)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(vector)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(quaternion)"	{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(rotation)"	{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "(list)"		{ return (new LSLToken(Tokens.TYPECAST,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "default" 			{ return (new LSLToken(Tokens.STATE_DEFAULT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "state_entry"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "state_exit"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "state"			{ return (new LSLToken(Tokens.STATE,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "event"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "jump"			{ return (new LSLToken(Tokens.JUMP,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "return"			{ return (new LSLToken(Tokens.RETURN,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "if"			{ return (new LSLToken(Tokens.IF,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "else"			{ return (new LSLToken(Tokens.ELSE,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "for"			{ return (new LSLToken(Tokens.FOR,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "do"			{ return (new LSLToken(Tokens.DO,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "while"			{ return (new LSLToken(Tokens.WHILE,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "touch_start"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "touch"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "touch_end"{FUNC_HEADER}			{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "collision_start"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "collision"{FUNC_HEADER}			{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "collision_end"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "land_collision_start"{FUNC_HEADER} { return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "land_collision"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "land_collision_end"{FUNC_HEADER} 	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "timer"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "listen"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "sensor"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "no_sensor"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "control"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "print"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "at_target"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "not_at_target"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "at_rot_target"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "not_at_rot_target"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "money"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "email"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "run_time_permissions"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "changed"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "attach"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "dataserver"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "moving_start"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "moving_end"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "link_message"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "on_rez"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "object_rez"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "remote_data"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "http_response"{FUNC_HEADER}	{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "http_request"{FUNC_HEADER}		{ return (new LSLToken(Tokens.EVENT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_IDENTIFIER}{FUNC_HEADER}{WSLR}"{"	{ return (new LSLToken(Tokens.USERFUNC_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_IDENTIFIER}"()"{WSLR}"{"		{ return (new LSLToken(Tokens.USERFUNC_DECL,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_IDENTIFIER}"(" 		{ return (new LSLToken(Tokens.USERFUNC,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "."					{ return (new LSLToken(Tokens.PERIOD,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {HEXIDECIMAL}			{ return (new LSLToken(Tokens.INTEGER_CONSTANT,Utilities.Hex2Dec(yytext()),yyline,yychar,yychar+1)); }
<YYINITIAL> {NPLUS}				{ return (new LSLToken(Tokens.INTEGER_CONSTANT,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "TRUE"				{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"1",yyline,yychar,yychar+1)); }
<YYINITIAL> "FALSE"				{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_PHYSICS"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x1",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_ROTATE_X"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x2",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_ROTATE_Y"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x4",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_ROTATE_Z"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x8",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_PHANTOM"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x10",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_SANDBOX"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x20",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_BLOCK_GRAB"			{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x40",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_DIE_AT_EDGE"		{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x80",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_RETURN_AT_EDGE"		{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x100",yyline,yychar,yychar+1)); }
<YYINITIAL> "STATUS_CAST_SHADOWS"		{ return (new LSLToken(Tokens.INTEGER_CONSTANT_D,"0x200",yyline,yychar,yychar+1)); }
<YYINITIAL> "AGENT_FLYING"			{ String v = String.Format("{0}",AGENT_FLYING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "AGENT_ATTACHMENTS"			{ String v = String.Format("{0}",AGENT_ATTACHMENTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_SCRIPTED"			{ String v = String.Format("{0}",AGENT_SCRIPTED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_MOUSELOOK"			{ String v = String.Format("{0}",AGENT_MOUSELOOK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_SITTING"			{ String v = String.Format("{0}",AGENT_SITTING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_ON_OBJECT"			{ String v = String.Format("{0}",AGENT_ON_OBJECT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_AWAY"			{ String v = String.Format("{0}",AGENT_AWAY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_WALKING"			{ String v = String.Format("{0}",AGENT_WALKING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_IN_AIR"			{ String v = String.Format("{0}",AGENT_IN_AIR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_TYPING"			{ String v = String.Format("{0}",AGENT_TYPING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_CROUCHING"			{ String v = String.Format("{0}",AGENT_CROUCHING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_BUSY"			{ String v = String.Format("{0}",AGENT_BUSY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT_ALWAYS_RUN"			{ String v = String.Format("{0}",AGENT_ALWAYS_RUN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_PITCH"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_PITCH); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_FOCUS_OFFSET"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_FOCUS_OFFSET); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_POSITION_LAG"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_POSITION_LAG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_FOCUS_LAG"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_FOCUS_LAG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_DISTANCE"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_DISTANCE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_BEHINDNESS_ANGLE"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_BEHINDNESS_ANGLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_BEHINDNESS_LAG"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_BEHINDNESS_LAG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_POSITION_THRESHOLD"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_POSITION_THRESHOLD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_FOCUS_THRESHOLD"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_FOCUS_THRESHOLD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_ACTIVE"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_ACTIVE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_POSITION"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_POSITION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_FOCUS"			{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_FOCUS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_POSITION_LOCKED"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_POSITION_LOCKED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CAMERA_FOCUS_LOCKED"		{ String v = String.Format("{0}",EFollowCamAttributes.FOLLOWCAM_FOCUS_LOCKED);  return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ANIM_ON"				{ String v = String.Format("{0}",0x1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LOOP"				{ String v = String.Format("{0}",0x2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REVERSE"				{ String v = String.Format("{0}",0x4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PING_PONG"				{ String v = String.Format("{0}",0x8); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "SMOOTH"				{ String v = String.Format("{0}",0x10); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ROTATE"				{ String v = String.Format("{0}",0x20); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "SCALE"				{ String v = String.Format("{0}",0x40); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ALL_SIDES"				{ String v = String.Format("{0}",LSL_ALL_SIDES); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LINK_ROOT"				{ String v = String.Format("{0}",LSL_LINK_ROOT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LINK_SET"				{ String v = String.Format("{0}",LSL_LINK_SET); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LINK_ALL_OTHERS"			{ String v = String.Format("{0}",LSL_LINK_ALL_OTHERS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LINK_ALL_CHILDREN"			{ String v = String.Format("{0}",LSL_LINK_ALL_CHILDREN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LINK_THIS"				{ String v = String.Format("{0}",LSL_LINK_THIS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "AGENT"				{ String v = String.Format("{0}",0x1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ACTIVE"				{ String v = String.Format("{0}",0x2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PASSIVE"				{ String v = String.Format("{0}",0x4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "SCRIPTED"				{ String v = String.Format("{0}",0x8); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_FWD"			{ String v = String.Format("{0}",AGENT_CONTROL_AT_POS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_BACK"			{ String v = String.Format("{0}",AGENT_CONTROL_AT_NEG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_LEFT"			{ String v = String.Format("{0}",AGENT_CONTROL_LEFT_POS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_RIGHT"			{ String v = String.Format("{0}",AGENT_CONTROL_LEFT_NEG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_ROT_LEFT"			{ String v = String.Format("{0}",AGENT_CONTROL_YAW_POS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_ROT_RIGHT"			{ String v = String.Format("{0}",AGENT_CONTROL_YAW_NEG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_UP"			{ String v = String.Format("{0}",AGENT_CONTROL_UP_POS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_DOWN"			{ String v = String.Format("{0}",AGENT_CONTROL_UP_NEG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_LBUTTON"			{ String v = String.Format("{0}",AGENT_CONTROL_LBUTTON_DOWN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CONTROL_ML_LBUTTON"		{ String v = String.Format("{0}",AGENT_CONTROL_ML_LBUTTON_DOWN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_DEBIT"			{ String v = String.Format("{0}",SCRIPT_PERMISSION_DEBIT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_TAKE_CONTROLS"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_TAKE_CONTROLS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_REMAP_CONTROLS"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_REMAP_CONTROLS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_TRIGGER_ANIMATION"	{ String v = String.Format("{0}",SCRIPT_PERMISSION_TRIGGER_ANIMATION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_ATTACH"			{ String v = String.Format("{0}",SCRIPT_PERMISSION_ATTACH); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_RELEASE_OWNERSHIP"	{ String v = String.Format("{0}",SCRIPT_PERMISSION_RELEASE_OWNERSHIP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_CHANGE_LINKS"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_CHANGE_LINKS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_CHANGE_JOINTS"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_CHANGE_JOINTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_CHANGE_PERMISSIONS"	{ String v = String.Format("{0}",SCRIPT_PERMISSION_CHANGE_PERMISSIONS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_TRACK_CAMERA"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_TRACK_CAMERA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERMISSION_CONTROL_CAMERA"		{ String v = String.Format("{0}",SCRIPT_PERMISSION_CONTROL_CAMERA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_TEXTURE"			{ String v = String.Format("{0}",LLAssetType.AT_TEXTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_SOUND"			{ String v = String.Format("{0}",LLAssetType.AT_SOUND); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_OBJECT"			{ String v = String.Format("{0}",LLAssetType.AT_OBJECT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_SCRIPT"			{ String v = String.Format("{0}",LLAssetType.AT_LSL_TEXT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_LANDMARK"		{ String v = String.Format("{0}",LLAssetType.AT_LANDMARK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_CLOTHING"		{ String v = String.Format("{0}",LLAssetType.AT_CLOTHING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_NOTECARD"		{ String v = String.Format("{0}",LLAssetType.AT_NOTECARD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_BODYPART"		{ String v = String.Format("{0}",LLAssetType.AT_BODYPART); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_ANIMATION"		{ String v = String.Format("{0}",LLAssetType.AT_ANIMATION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_GESTURE"			{ String v = String.Format("{0}",LLAssetType.AT_GESTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_ALL"			{ String v = String.Format("{0}",LLAssetType.AT_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "INVENTORY_NONE"			{ String v = String.Format("{0}",LLAssetType.AT_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CHANGED_INVENTORY"			{ String v = String.Format("{0}",CHANGED_INVENTORY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_COLOR"			{ String v = String.Format("{0}",CHANGED_COLOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_SHAPE"			{ String v = String.Format("{0}",CHANGED_SHAPE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_SCALE"			{ String v = String.Format("{0}",CHANGED_SCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_TEXTURE"			{ String v = String.Format("{0}",CHANGED_TEXTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_LINK"			{ String v = String.Format("{0}",CHANGED_LINK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_ALLOWED_DROP"		{ String v = String.Format("{0}",CHANGED_ALLOWED_DROP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_OWNER"			{ String v = String.Format("{0}",CHANGED_OWNER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_REGION"			{ String v = String.Format("{0}",CHANGED_REGION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_TELEPORT"			{ String v = String.Format("{0}",CHANGED_TELEPORT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "CHANGED_REGION_START"		{ String v = String.Format("{0}",CHANGED_REGION_START); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "OBJECT_UNKNOWN_DETAIL"		{ String v = String.Format("{0}",OBJECT_UNKNOWN_DETAIL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_NAME"			{ String v = String.Format("{0}",OBJECT_NAME); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_DESC"			{ String v = String.Format("{0}",OBJECT_DESC); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_POS"			{ String v = String.Format("{0}",OBJECT_POS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_ROT"			{ String v = String.Format("{0}",OBJECT_ROT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_VELOCITY"			{ String v = String.Format("{0}",OBJECT_VELOCITY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_OWNER"			{ String v = String.Format("{0}",OBJECT_OWNER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_GROUP"			{ String v = String.Format("{0}",OBJECT_GROUP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "OBJECT_CREATOR"			{ String v = String.Format("{0}",OBJECT_CREATOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TYPE_INTEGER"			{ String v = String.Format("{0}",LScriptType.LST_INTEGER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_FLOAT"			{ String v = String.Format("{0}",LScriptType.LST_FLOATINGPOINT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_STRING"			{ String v = String.Format("{0}",LScriptType.LST_STRING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_KEY"				{ String v = String.Format("{0}",LScriptType.LST_KEY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_VECTOR"			{ String v = String.Format("{0}",LScriptType.LST_VECTOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_ROTATION"			{ String v = String.Format("{0}",LScriptType.LST_QUATERNION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "TYPE_INVALID"			{ String v = String.Format("{0}",LScriptType.LST_NULL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "NULL_KEY"				{ String v = String.Format("{0}","00000000000000000000000000000000"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "EOF"				{ String v = String.Format("{0}","\n\n\n"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "URL_REQUEST_GRANTED"		{ String v = String.Format("{0}",URL_REQUEST_GRANTED); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "URL_REQUEST_DENIED"		{ String v = String.Format("{0}",URL_REQUEST_DENIED); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PI"				{ String v = String.Format("{0}",F_PI); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TWO_PI"				{ String v = String.Format("{0}",F_TWO_PI); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PI_BY_TWO"				{ String v = String.Format("{0}",F_PI_BY_TWO); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DEG_TO_RAD"			{ String v = String.Format("{0}",DEG_TO_RAD); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "RAD_TO_DEG"			{ String v = String.Format("{0}",RAD_TO_DEG); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "SQRT2"				{ String v = String.Format("{0}",F_SQRT2); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DEBUG_CHANNEL"			{ String v = String.Format("{0}",CHAT_CHANNEL_DEBUG); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "PUBLIC_CHANNEL"			{ String v = String.Format("{0}",0); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }	
<YYINITIAL> "ZERO_VECTOR"			{ return (new LSLToken(Tokens.ZERO_VECTOR,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "ZERO_ROTATION"			{ return (new LSLToken(Tokens.ZERO_ROTATION,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "ATTACH_CHEST"			{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HEAD"			{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LSHOULDER"			{ String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RSHOULDER"			{ String v = String.Format("{0}",4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LHAND"			{ String v = String.Format("{0}",5); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RHAND"			{ String v = String.Format("{0}",6); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LFOOT"			{ String v = String.Format("{0}",7); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RFOOT"			{ String v = String.Format("{0}",8); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_BACK"			{ String v = String.Format("{0}",9); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_PELVIS"			{ String v = String.Format("{0}",10); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_MOUTH"			{ String v = String.Format("{0}",11); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_CHIN"			{ String v = String.Format("{0}",12); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LEAR"			{ String v = String.Format("{0}",13); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_REAR"			{ String v = String.Format("{0}",14); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LEYE"			{ String v = String.Format("{0}",15); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_REYE"			{ String v = String.Format("{0}",16); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_NOSE"			{ String v = String.Format("{0}",17); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RUARM"			{ String v = String.Format("{0}",18); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RLARM"			{ String v = String.Format("{0}",19); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LUARM"			{ String v = String.Format("{0}",20); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LLARM"			{ String v = String.Format("{0}",21); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RHIP"			{ String v = String.Format("{0}",22); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RULEG"			{ String v = String.Format("{0}",23); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RLLEG"			{ String v = String.Format("{0}",24); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LHIP"			{ String v = String.Format("{0}",25); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LULEG"			{ String v = String.Format("{0}",26); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LLLEG"			{ String v = String.Format("{0}",27); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_BELLY"			{ String v = String.Format("{0}",28); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_RPEC"			{ String v = String.Format("{0}",29); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_LPEC"			{ String v = String.Format("{0}",30); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_CENTER_2"		{ String v = String.Format("{0}",31); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_TOP_RIGHT"		{ String v = String.Format("{0}",32); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_TOP_CENTER"		{ String v = String.Format("{0}",33); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_TOP_LEFT"		{ String v = String.Format("{0}",34); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_CENTER_1"		{ String v = String.Format("{0}",35); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_BOTTOM_LEFT" 		{ String v = String.Format("{0}",36); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_BOTTOM"			{ String v = String.Format("{0}",37); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "ATTACH_HUD_BOTTOM_RIGHT"		{ String v = String.Format("{0}",38); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_LEVEL"			{ String v = String.Format("{0}",0); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_RAISE"			{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_LOWER"			{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_SMOOTH"			{ String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_NOISE"			{ String v = String.Format("{0}",4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_REVERT"			{ String v = String.Format("{0}",5); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_SMALL_BRUSH"			{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_MEDIUM_BRUSH"			{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LAND_LARGE_BRUSH"			{ String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_ONLINE"			{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_NAME"				{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_BORN"				{ String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_RATING"			{ String v = String.Format("{0}",4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_SIM_POS"			{ String v = String.Format("{0}",5); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_SIM_STATUS"			{ String v = String.Format("{0}",6); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_SIM_RATING"			{ String v = String.Format("{0}",7); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "DATA_PAYINFO"			{ String v = String.Format("{0}",8); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PAYMENT_INFO_ON_FILE" 		{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PAYMENT_INFO_USED"  		{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REMOTE_DATA_CHANNEL"		{ String v = String.Format("{0}",LSL_REMOTE_DATA_CHANNEL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REMOTE_DATA_REQUEST"		{ String v = String.Format("{0}",LSL_REMOTE_DATA_REQUEST); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REMOTE_DATA_REPLY"			{ String v = String.Format("{0}",LSL_REMOTE_DATA_REPLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_FLAGS"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_FLAGS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_START_COLOR"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_START_COLOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_START_ALPHA"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_START_ALPHA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_START_SCALE"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_START_SCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_END_COLOR"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_END_COLOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_END_ALPHA"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_END_ALPHA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_END_SCALE"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_END_SCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_MAX_AGE"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_PART_MAX_AGE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_WIND_MASK"		{ String v = String.Format("{0}",LLPartData::LL_PART_WIND_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_INTERP_COLOR_MASK"	{ String v = String.Format("{0}",LLPartData::LL_PART_INTERP_COLOR_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_INTERP_SCALE_MASK"	{ String v = String.Format("{0}",LLPartData::LL_PART_INTERP_SCALE_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_BOUNCE_MASK"		{ String v = String.Format("{0}",LLPartData::LL_PART_BOUNCE_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_FOLLOW_SRC_MASK"		{ String v = String.Format("{0}",LLPartData::LL_PART_FOLLOW_SRC_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_FOLLOW_VELOCITY_MASK"	{ String v = String.Format("{0}",LLPartData::LL_PART_FOLLOW_VELOCITY_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_TARGET_POS_MASK"		{ String v = String.Format("{0}",LLPartData::LL_PART_TARGET_POS_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_EMISSIVE_MASK"		{ String v = String.Format("{0}",LLPartData::LL_PART_EMISSIVE_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_PART_TARGET_LINEAR_MASK"	{ String v = String.Format("{0}",LLPartData::LL_PART_TARGET_LINEAR_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_MAX_AGE"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_MAX_AGE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_PATTERN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_INNERANGLE"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_INNERANGLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_OUTERANGLE"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_OUTERANGLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_ANGLE_BEGIN"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_ANGLE_BEGIN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_ANGLE_END"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_ANGLE_END); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_BURST_RATE"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_BURST_RATE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_BURST_PART_COUNT"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_BURST_PART_COUNT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_BURST_RADIUS"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_BURST_RADIUS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_BURST_SPEED_MIN"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_BURST_SPEED_MIN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_BURST_SPEED_MAX"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_BURST_SPEED_MAX); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_ACCEL"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_ACCEL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_TEXTURE"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_TEXTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_TARGET_KEY"		{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_TARGET_UUID); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_OMEGA"			{ String v = String.Format("{0}",LLPSScriptFlags.LLPS_SRC_OMEGA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_OBJ_REL_MASK"		{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_OBJ_REL_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN_DROP"		{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_PATTERN_DROP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN_EXPLODE"		{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_PATTERN_EXPLODE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN_ANGLE"		{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_PATTERN_ANGLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN_ANGLE_CONE"	{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_PATTERN_ANGLE_CONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PSYS_SRC_PATTERN_ANGLE_CONE_EMPTY"	{ String v = String.Format("{0}",LLPartSysData::LL_PART_SRC_PATTERN_ANGLE_CONE_EMPTY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_NONE"			{ String v = String.Format("{0}",VEHICLE_TYPE_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_SLED"			{ String v = String.Format("{0}",VEHICLE_TYPE_SLED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_CAR"			{ String v = String.Format("{0}",VEHICLE_TYPE_CAR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_BOAT"			{ String v = String.Format("{0}",VEHICLE_TYPE_BOAT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_AIRPLANE"		{ String v = String.Format("{0}",VEHICLE_TYPE_AIRPLANE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_TYPE_BALLOON"		{ String v = String.Format("{0}",VEHICLE_TYPE_BALLOON); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_REFERENCE_FRAME"		{ String v = String.Format("{0}",VEHICLE_REFERENCE_FRAME); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_FRICTION_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_LINEAR_FRICTION_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_FRICTION_TIMESCALE" { String v = String.Format("{0}",VEHICLE_ANGULAR_FRICTION_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_MOTOR_DIRECTION"	{ String v = String.Format("{0}",VEHICLE_LINEAR_MOTOR_DIRECTION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_MOTOR_DIRECTION"	{ String v = String.Format("{0}",VEHICLE_ANGULAR_MOTOR_DIRECTION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_MOTOR_OFFSET"	{ String v = String.Format("{0}",VEHICLE_LINEAR_MOTOR_OFFSET); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_HOVER_HEIGHT"		{ String v = String.Format("{0}",VEHICLE_HOVER_HEIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_HOVER_EFFICIENCY"		{ String v = String.Format("{0}",VEHICLE_HOVER_EFFICIENCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_HOVER_TIMESCALE"		{ String v = String.Format("{0}",VEHICLE_HOVER_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_BUOYANCY"			{ String v = String.Format("{0}",VEHICLE_BUOYANCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_DEFLECTION_EFFICIENCY"	{ String v = String.Format("{0}",VEHICLE_LINEAR_DEFLECTION_EFFICIENCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_DEFLECTION_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_LINEAR_DEFLECTION_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_MOTOR_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_LINEAR_MOTOR_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_LINEAR_MOTOR_DECAY_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_LINEAR_MOTOR_DECAY_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_DEFLECTION_EFFICIENCY" { String v = String.Format("{0}",VEHICLE_ANGULAR_DEFLECTION_EFFICIENCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_DEFLECTION_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_ANGULAR_DEFLECTION_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_MOTOR_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_ANGULAR_MOTOR_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_ANGULAR_MOTOR_DECAY_TIMESCALE"	{ String v = String.Format("{0}",VEHICLE_ANGULAR_MOTOR_DECAY_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_VERTICAL_ATTRACTION_EFFICIENCY"	{ String v = String.Format("{0}",VEHICLE_VERTICAL_ATTRACTION_EFFICIENCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_VERTICAL_ATTRACTION_TIMESCALE"		{ String v = String.Format("{0}",VEHICLE_VERTICAL_ATTRACTION_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_BANKING_EFFICIENCY"	{ String v = String.Format("{0}",VEHICLE_BANKING_EFFICIENCY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_BANKING_MIX"			{ String v = String.Format("{0}",VEHICLE_BANKING_MIX); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_BANKING_TIMESCALE"		{ String v = String.Format("{0}",VEHICLE_BANKING_TIMESCALE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_NO_FLY_UP"			{ String v = String.Format("{0}",VEHICLE_FLAG_NO_DEFLECTION_UP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_NO_DEFLECTION_UP"			{ String v = String.Format("{0}",VEHICLE_FLAG_NO_DEFLECTION_UP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_LIMIT_ROLL_ONLY"		{ String v = String.Format("{0}",VEHICLE_FLAG_LIMIT_ROLL_ONLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_HOVER_WATER_ONLY"		{ String v = String.Format("{0}",VEHICLE_FLAG_HOVER_WATER_ONLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_HOVER_TERRAIN_ONLY"	{ String v = String.Format("{0}",VEHICLE_FLAG_HOVER_TERRAIN_ONLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_HOVER_GLOBAL_HEIGHT"	{ String v = String.Format("{0}",VEHICLE_FLAG_HOVER_GLOBAL_HEIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_HOVER_UP_ONLY"		{ String v = String.Format("{0}",VEHICLE_FLAG_HOVER_UP_ONLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_LIMIT_MOTOR_UP"		{ String v = String.Format("{0}",VEHICLE_FLAG_LIMIT_MOTOR_UP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_MOUSELOOK_STEER"		{ String v = String.Format("{0}",VEHICLE_FLAG_MOUSELOOK_STEER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_MOUSELOOK_BANK"		{ String v = String.Format("{0}",VEHICLE_FLAG_MOUSELOOK_BANK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "VEHICLE_FLAG_CAMERA_DECOUPLED"		{ String v = String.Format("{0}",VEHICLE_FLAG_CAMERA_DECOUPLED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE"				{ String v = String.Format("{0}",LSL_PRIM_TYPE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL"			{ String v = String.Format("{0}",LSL_PRIM_MATERIAL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_PHYSICS"			{ String v = String.Format("{0}",LSL_PRIM_PHYSICS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_FLEXIBLE"			{ String v = String.Format("{0}",LSL_PRIM_FLEXIBLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_POINT_LIGHT"		{ String v = String.Format("{0}",LSL_PRIM_POINT_LIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TEMP_ON_REZ"		{ String v = String.Format("{0}",LSL_PRIM_TEMP_ON_REZ); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_PHANTOM"			{ String v = String.Format("{0}",LSL_PRIM_PHANTOM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_CAST_SHADOWS"		{ String v = String.Format("{0}",LSL_PRIM_CAST_SHADOWS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_POSITION"			{ String v = String.Format("{0}",LSL_PRIM_POSITION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SIZE"				{ String v = String.Format("{0}",LSL_PRIM_SIZE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_ROTATION"			{ String v = String.Format("{0}",LSL_PRIM_ROTATION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TEXTURE"			{ String v = String.Format("{0}",LSL_PRIM_TEXTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_COLOR"			{ String v = String.Format("{0}",LSL_PRIM_COLOR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_SHINY"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_SHINY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_FULLBRIGHT"		{ String v = String.Format("{0}",LSL_PRIM_FULLBRIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TEXGEN"			{ String v = String.Format("{0}",LSL_PRIM_TEXGEN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_GLOW"	     		{ String v = String.Format("{0}",LSL_PRIM_GLOW); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_BOX"			{ String v = String.Format("{0}",LSL_PRIM_TYPE_BOX); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_CYLINDER"	{ String v = String.Format("{0}",LSL_PRIM_TYPE_CYLINDER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_PRISM"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_PRISM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_SPHERE"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_SPHERE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_TORUS"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_TORUS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_TUBE"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_TUBE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_RING"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_RING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TYPE_SCULPT"		{ String v = String.Format("{0}",LSL_PRIM_TYPE_SCULPT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_HOLE_DEFAULT"		{ String v = String.Format("{0}",LSL_PRIM_HOLE_DEFAULT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_HOLE_CIRCLE"		{ String v = String.Format("{0}",LSL_PRIM_HOLE_CIRCLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_HOLE_SQUARE"		{ String v = String.Format("{0}",LSL_PRIM_HOLE_SQUARE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_HOLE_TRIANGLE"	{ String v = String.Format("{0}",LSL_PRIM_HOLE_TRIANGLE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_STONE"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_STONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_METAL"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_METAL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_GLASS"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_GLASS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_WOOD"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_WOOD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_FLESH"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_FLESH); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_PLASTIC"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_PLASTIC); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_RUBBER"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_RUBBER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_MATERIAL_LIGHT"	{ String v = String.Format("{0}",LSL_PRIM_MATERIAL_LIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SHINY_NONE"		{ String v = String.Format("{0}",LSL_PRIM_SHINY_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SHINY_LOW"		{ String v = String.Format("{0}",LSL_PRIM_SHINY_LOW); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SHINY_MEDIUM"		{ String v = String.Format("{0}",LSL_PRIM_SHINY_MEDIUM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SHINY_HIGH"		{ String v = String.Format("{0}",LSL_PRIM_SHINY_HIGH); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_NONE"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_BRIGHT"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_BRIGHT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_DARK"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_DARK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_WOOD"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_WOOD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_BARK"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_BARK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_BRICKS"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_BRICKS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_CHECKER"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_CHECKER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_CONCRETE"	{ String v = String.Format("{0}",LSL_PRIM_BUMP_CONCRETE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_TILE"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_TILE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_STONE"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_STONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_DISKS"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_DISKS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_GRAVEL"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_GRAVEL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_BLOBS"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_BLOBS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_SIDING"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_SIDING); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_LARGETILE"	{ String v = String.Format("{0}",LSL_PRIM_BUMP_LARGETILE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_STUCCO"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_STUCCO); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_SUCTION"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_SUCTION); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_BUMP_WEAVE"		{ String v = String.Format("{0}",LSL_PRIM_BUMP_WEAVE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TEXGEN_DEFAULT"	{ String v = String.Format("{0}",LSL_PRIM_TEXGEN_DEFAULT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_TEXGEN_PLANAR"	{ String v = String.Format("{0}",LSL_PRIM_TEXGEN_PLANAR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_TYPE_SPHERE"	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_TYPE_SPHERE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_TYPE_TORUS"	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_TYPE_TORUS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_TYPE_PLANE"	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_TYPE_PLANE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_TYPE_CYLINDER"	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_TYPE_CYLINDER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_TYPE_MASK" 	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_TYPE_MASK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_FLAG_MIRROR" 	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_FLAG_MIRROR); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PRIM_SCULPT_FLAG_INVERT" 	{ String v = String.Format("{0}",LSL_PRIM_SCULPT_FLAG_INVERT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "MASK_BASE"				{ String v = String.Format("{0}",0); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "MASK_OWNER"			{ String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "MASK_GROUP"			{ String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "MASK_EVERYONE"			{ String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "MASK_NEXT"				{ String v = String.Format("{0}",4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERM_TRANSFER"			{ String v = String.Format("{0}",PERM_TRANSFER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERM_MODIFY"			{ String v = String.Format("{0}",PERM_MODIFY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERM_COPY"				{ String v = String.Format("{0}",PERM_COPY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERM_MOVE"				{ String v = String.Format("{0}",PERM_MOVE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PERM_ALL"				{ String v = String.Format("{0}",PERM_ALL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_STOP"		{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_STOP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_PAUSE"	{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_PAUSE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_PLAY"		{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_PLAY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_LOOP"		{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_LOOP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_TEXTURE"	{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_TEXTURE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_URL"		{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_URL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_TIME"		{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_TIME); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_AGENT"	{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_AGENT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_UNLOAD"	{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_UNLOAD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_AUTO_ALIGN"	{ String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_AUTO_ALIGN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_TYPE"     { String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_TYPE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_SIZE"     { String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_SIZE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_DESC"     { String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_DESC); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_MEDIA_COMMAND_LOOP_SET" { String v = String.Format("{0}",PARCEL_MEDIA_COMMAND_LOOP_SET); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_MAX"			{ String v = String.Format("{0}",LIST_STAT_MAX); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_MIN"			{ String v = String.Format("{0}",LIST_STAT_MIN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_MEAN"		{ String v = String.Format("{0}",LIST_STAT_MEAN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_MEDIAN"		{ String v = String.Format("{0}",LIST_STAT_MEDIAN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_STD_DEV"		{ String v = String.Format("{0}",LIST_STAT_STD_DEV); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_SUM"		{ String v = String.Format("{0}",LIST_STAT_SUM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_SUM_SQUARES"		{ String v = String.Format("{0}",LIST_STAT_SUM_SQUARES); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_NUM_COUNT"		{ String v = String.Format("{0}",LIST_STAT_NUM_COUNT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_GEOMETRIC_MEAN"		{ String v = String.Format("{0}",LIST_STAT_GEO_MEAN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "LIST_STAT_RANGE"		{ String v = String.Format("{0}",LIST_STAT_RANGE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PAY_HIDE"		{ String v = String.Format("{0}",PAY_PRICE_HIDE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PAY_DEFAULT"	{ String v = String.Format("{0}",PAY_PRICE_DEFAULT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_FLY"		{ String v = String.Format("{0}",PF_ALLOW_FLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_GROUP_SCRIPTS"		{ String v = String.Format("{0}",PF_ALLOW_GROUP_SCRIPTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_SCRIPTS"		{ String v = String.Format("{0}",PF_ALLOW_OTHER_SCRIPTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_LANDMARK"		{ String v = String.Format("{0}",PF_ALLOW_LANDMARK); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_TERRAFORM"		{ String v = String.Format("{0}",PF_ALLOW_TERRAFORM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_DAMAGE"		{ String v = String.Format("{0}",PF_ALLOW_DAMAGE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_CREATE_OBJECTS"		{ String v = String.Format("{0}",PF_CREATE_OBJECTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_CREATE_GROUP_OBJECTS"		{ String v = String.Format("{0}",PF_CREATE_GROUP_OBJECTS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_USE_ACCESS_GROUP"		{ String v = String.Format("{0}",PF_USE_ACCESS_GROUP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_USE_ACCESS_LIST"		{ String v = String.Format("{0}",PF_USE_ACCESS_LIST); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_USE_BAN_LIST"		{ String v = String.Format("{0}",PF_USE_BAN_LIST); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_USE_LAND_PASS_LIST"		{ String v = String.Format("{0}",PF_USE_PASS_LIST); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_LOCAL_SOUND_ONLY"		{ String v = String.Format("{0}",PF_SOUND_LOCAL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_RESTRICT_PUSHOBJECT"		{ String v = String.Format("{0}",PF_RESTRICT_PUSHOBJECT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_GROUP_OBJECT_ENTRY"		{ String v = String.Format("{0}",PF_ALLOW_GROUP_OBJECT_ENTRY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_FLAG_ALLOW_ALL_OBJECT_ENTRY"		{ String v = String.Format("{0}",PF_ALLOW_ALL_OBJECT_ENTRY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_ALLOW_DAMAGE"		{ String v = String.Format("{0}",REGION_FLAGS_ALLOW_DAMAGE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_FIXED_SUN"		{ String v = String.Format("{0}",REGION_FLAGS_SUN_FIXED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_BLOCK_TERRAFORM"		{ String v = String.Format("{0}",REGION_FLAGS_BLOCK_TERRAFORM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_SANDBOX"		{ String v = String.Format("{0}",REGION_FLAGS_SANDBOX); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_DISABLE_COLLISIONS"		{ String v = String.Format("{0}",REGION_FLAGS_SKIP_COLLISIONS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_DISABLE_PHYSICS"		{ String v = String.Format("{0}",REGION_FLAGS_SKIP_PHYSICS); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_BLOCK_FLY"		{ String v = String.Format("{0}",REGION_FLAGS_BLOCK_FLY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_ALLOW_DIRECT_TELEPORT"		{ String v = String.Format("{0}",REGION_FLAGS_ALLOW_DIRECT_TELEPORT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "REGION_FLAG_RESTRICT_PUSHOBJECT"		{ String v = String.Format("{0}",REGION_FLAGS_RESTRICT_PUSHOBJECT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "HTTP_METHOD" { String v = String.Format("{0}",HTTP_METHOD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "HTTP_MIMETYPE" { String v = String.Format("{0}",1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "HTTP_BODY_MAXLENGTH" { String v = String.Format("{0}",2); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "HTTP_BODY_TRUNCATED" { String v = String.Format("{0}",3); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "HTTP_VERIFY_CERT" { String v = String.Format("{0}",4); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_TOTAL"		{ String v = String.Format("{0}",OC_TOTAL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_OWNER"		{ String v = String.Format("{0}",OC_OWNER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_GROUP"		{ String v = String.Format("{0}",OC_GROUP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_OTHER"		{ String v = String.Format("{0}",OC_OTHER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_SELECTED"	{ String v = String.Format("{0}",OC_SELECTED); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_COUNT_TEMP"		{ String v = String.Format("{0}",OC_TEMP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_DETAILS_NAME"	{ String v = String.Format("{0}",PARCEL_DETAILS_NAME); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_DETAILS_DESC"	{ String v = String.Format("{0}",PARCEL_DETAILS_DESC); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_DETAILS_OWNER"	{ String v = String.Format("{0}",PARCEL_DETAILS_OWNER); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_DETAILS_GROUP"	{ String v = String.Format("{0}",PARCEL_DETAILS_GROUP); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "PARCEL_DETAILS_AREA"	{ String v = String.Format("{0}",PARCEL_DETAILS_AREA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "STRING_TRIM_HEAD"	{ String v = String.Format("{0}",STRING_TRIM_HEAD); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "STRING_TRIM_TAIL"	{ String v = String.Format("{0}",STRING_TRIM_TAIL); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "STRING_TRIM"	{ String v = String.Format("{0}",STRING_TRIM); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_NONE"       { String v = String.Format("{0}",CLICK_ACTION_NONE); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_TOUCH"      { String v = String.Format("{0}",CLICK_ACTION_TOUCH); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_SIT"        { String v = String.Format("{0}",CLICK_ACTION_SIT); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_BUY"        { String v = String.Format("{0}",CLICK_ACTION_BUY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_PAY"        { String v = String.Format("{0}",CLICK_ACTION_PAY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_OPEN"       { String v = String.Format("{0}",CLICK_ACTION_OPEN); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_PLAY"       { String v = String.Format("{0}",CLICK_ACTION_PLAY); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "CLICK_ACTION_OPEN_MEDIA" { String v = String.Format("{0}",CLICK_ACTION_OPEN_MEDIA); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TEXTURE_BLANK"           { String v = String.Format("{0}","5748decc-f629-461c-9a36-a35a221fe21f"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TEXTURE_DEFAULT"         { String v = String.Format("{0}","89556747-24cb-43ed-920b-47caed15465f"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TEXTURE_MEDIA"           { String v = String.Format("{0}","8b5fec65-8d8d-9dc5-cda8-8fdf2716e361"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TEXTURE_PLYWOOD"         { String v = String.Format("{0}","89556747-24cb-43ed-920b-47caed15465f"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TEXTURE_TRANSPARENT"     { String v = String.Format("{0}","8dcd4a48-2d37-4909-9f78-f7a9eb4ef903"); return (new LSLToken(Tokens.STRING_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TOUCH_INVALID_FACE"	  { String v = String.Format("{0}",-1); return (new LSLToken(Tokens.INTEGER_CONSTANT_D,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "TOUCH_INVALID_VECTOR"	  { return (new LSLToken(Tokens.TOUCH_INVALID_VECTOR,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> "TOUCH_INVALID_TEXCOORD"  	{ return (new LSLToken(Tokens.TOUCH_INVALID_TEXCOORD,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_IDENTIFIER} 		{  return (new LSLToken(Tokens.IDENTIFIER,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_FP_A}			{ String v = yytext(); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> {TOK_FP_B}			{ String v = yytext(); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> {TOK_FP_C}			{ String v = yytext(); return (new LSLToken(Tokens.FP_CONSTANT,v,yyline,yychar,yychar+v.Length)); }
<YYINITIAL> "++"			{ return (new LSLToken(Tokens.INC_OP,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "--"			{ return (new LSLToken(Tokens.DEC_OP,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "+="			{ return (new LSLToken(Tokens.ADD_ASSIGN,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "-="			{ return (new LSLToken(Tokens.SUB_ASSIGN,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "*="			{ return (new LSLToken(Tokens.MUL_ASSIGN,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "/="			{ return (new LSLToken(Tokens.DIV_ASSIGN,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "%="			{ return (new LSLToken(Tokens.MOD_ASSIGN,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> ";"			{ return (new LSLToken(Tokens.SEMICOLON,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "{"			{ return (new LSLToken(Tokens.BEGINBLOCK,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "}"			{ return (new LSLToken(Tokens.ENDBLOCK,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> ","			{ return (new LSLToken(Tokens.COMMA,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "="			{ return (new LSLToken(Tokens.EQSIGN,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "("			{ return (new LSLToken(Tokens.OPENPAREN,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> ")"			{ return (new LSLToken(Tokens.CLOSEPAREN,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "-"			{ return (new LSLToken(Tokens.MINUS,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "+"			{ return (new LSLToken(Tokens.PLUS,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "*"			{ return (new LSLToken(Tokens.ASTERISK,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "/"			{ return (new LSLToken(Tokens.DIVIDE,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "%"			{ return (new LSLToken(Tokens.PERCENT,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "@"			{ return (new LSLToken(Tokens.AT,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> ":"			{ return (new LSLToken(Tokens.COLON,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> ">"			{ return (new LSLToken(Tokens.GT,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "<"			{ return (new LSLToken(Tokens.LT,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "]"			{ return (new LSLToken(Tokens.ENDLIST,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "["			{ return (new LSLToken(Tokens.OPENLIST,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "=="			{ return (new LSLToken(Tokens.EQ,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "!="			{ return (new LSLToken(Tokens.NEQ,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> ">="			{ return (new LSLToken(Tokens.GEQ,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "<="			{ return (new LSLToken(Tokens.LEQ,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "&"			{ return (new LSLToken(Tokens.AMPERSAND,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "|"			{ return (new LSLToken(Tokens.PIPE,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "^"			{ return (new LSLToken(Tokens.EXPONENT,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "~"			{ return (new LSLToken(Tokens.TILDE,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "!"			{ return (new LSLToken(Tokens.EXCLAMATION,yytext(),yyline,yychar,yychar+1)); }
<YYINITIAL> "&&"			{ return (new LSLToken(Tokens.BOOLEAN_AND,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "||"			{ return (new LSLToken(Tokens.BOOLEAN_OR,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> "<<"			{ return (new LSLToken(Tokens.SHIFT_LEFT,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> ">>"			{ return (new LSLToken(Tokens.SHIFT_RIGHT,yytext(),yyline,yychar,yychar+2)); }
<YYINITIAL> {WHITESPACE}		{ return (new LSLToken(Tokens.WHITESPACE,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {LINERETURN}		{ return (new LSLToken(Tokens.LINERETURN,yytext(),yyline,yychar,yychar+yytext().Length)); }
<YYINITIAL> {TOK_OTHER}			{ return null; }

<INQUOTE> "\"" 			{ yybegin(YYINITIAL); return new LSLToken(Tokens.QUOTE,yytext(),yyline,yychar,yychar+1); }
<INQUOTE> {TOK_OTHER} 		{ return new LSLToken(Tokens.CHAR,yytext(),yyline,yychar,yychar+1); }

<INLINECOMMENT>	"\n"		{ yybegin(YYINITIAL); return new LSLToken(Tokens.NEWLINE,yytext(),yyline,yychar,yychar+1); }
<INLINECOMMENT> {TOK_OTHER}	{ return null; }

<BLOCKCOMMENT> "*/"		{ yybegin(YYINITIAL); return new LSLToken(Tokens.ENDBLOCK,yytext(),yyline,yychar,yychar+2); }
<BLOCKCOMMENT> {TOK_OTHER}	{ return null; }
