namespace LSLObf
{
	public partial class LSLLexer
	{
		
const uint  CONTROL_AT_POS_INDEX				= 0;
const uint  CONTROL_AT_NEG_INDEX				= 1;
const uint  CONTROL_LEFT_POS_INDEX			= 2;
const uint  CONTROL_LEFT_NEG_INDEX			= 3;
const uint  CONTROL_UP_POS_INDEX				= 4;
const uint  CONTROL_UP_NEG_INDEX				= 5;
const uint  CONTROL_PITCH_POS_INDEX			= 6;
const uint  CONTROL_PITCH_NEG_INDEX			= 7;
const uint  CONTROL_YAW_POS_INDEX				= 8;
const uint  CONTROL_YAW_NEG_INDEX				= 9;
const uint  CONTROL_FAST_AT_INDEX				= 10;
const uint  CONTROL_FAST_LEFT_INDEX			= 11;
const uint  CONTROL_FAST_UP_INDEX				= 12;
const uint  CONTROL_FLY_INDEX					= 13;
const uint  CONTROL_STOP_INDEX				= 14;
const uint  CONTROL_FINISH_ANIM_INDEX			= 15;
const uint  CONTROL_STAND_UP_INDEX			= 16;
const uint  CONTROL_SIT_ON_GROUND_INDEX		= 17;
const uint  CONTROL_MOUSELOOK_INDEX			= 18;
const uint  CONTROL_NUDGE_AT_POS_INDEX		= 19;
const uint  CONTROL_NUDGE_AT_NEG_INDEX		= 20;
const uint  CONTROL_NUDGE_LEFT_POS_INDEX		= 21;
const uint  CONTROL_NUDGE_LEFT_NEG_INDEX		= 22;
const uint  CONTROL_NUDGE_UP_POS_INDEX		= 23;
const uint  CONTROL_NUDGE_UP_NEG_INDEX		= 24;
const uint  CONTROL_TURN_LEFT_INDEX			= 25;
const uint  CONTROL_TURN_RIGHT_INDEX			= 26;
const uint  CONTROL_AWAY_INDEX				= 27;
const uint  CONTROL_LBUTTON_DOWN_INDEX		= 28;
const uint  CONTROL_LBUTTON_UP_INDEX			= 29;
const uint  CONTROL_ML_LBUTTON_DOWN_INDEX		= 30;
const uint  CONTROL_ML_LBUTTON_UP_INDEX		= 31;
const uint  TOTAL_CONTROLS					= 32;

const uint  AGENT_CONTROL_AT_POS              = 0x00000001;
const uint  AGENT_CONTROL_AT_NEG              = 0x00000002;
const uint  AGENT_CONTROL_LEFT_POS            = 0x00000004;
const uint  AGENT_CONTROL_LEFT_NEG            = 0x00000008;
const uint  AGENT_CONTROL_UP_POS              = 0x00000010;
const uint  AGENT_CONTROL_UP_NEG              = 0x00000020;
const uint  AGENT_CONTROL_PITCH_POS           = 0x00000040;
const uint  AGENT_CONTROL_PITCH_NEG           = 0x00000080;
const uint  AGENT_CONTROL_YAW_POS             = 0x00000100;
const uint  AGENT_CONTROL_YAW_NEG             = 0x00000200;

const uint  AGENT_CONTROL_FAST_AT             = 0x00000400;
const uint  AGENT_CONTROL_FAST_LEFT           = 0x00000800;
const uint  AGENT_CONTROL_FAST_UP             = 0x00001000;

const uint  AGENT_CONTROL_FLY				= 0x00002000;
const uint  AGENT_CONTROL_STOP				= 0x00004000;
const uint  AGENT_CONTROL_FINISH_ANIM		= 0x00008000;
const uint  AGENT_CONTROL_STAND_UP			= 0x00010000;
const uint  AGENT_CONTROL_SIT_ON_GROUND		= 0x00020000;
const uint  AGENT_CONTROL_MOUSELOOK			= 0x00040000;

const uint  AGENT_CONTROL_NUDGE_AT_POS      = 0x00080000;
const uint  AGENT_CONTROL_NUDGE_AT_NEG      = 0x00100000;
const uint  AGENT_CONTROL_NUDGE_LEFT_POS    = 0x00200000;
const uint  AGENT_CONTROL_NUDGE_LEFT_NEG    = 0x00400000;
const uint  AGENT_CONTROL_NUDGE_UP_POS      = 0x00800000;
const uint  AGENT_CONTROL_NUDGE_UP_NEG      = 0x01000000;
const uint  AGENT_CONTROL_TURN_LEFT	        = 0x02000000;
const uint  AGENT_CONTROL_TURN_RIGHT	    = 0x04000000;

const uint  AGENT_CONTROL_AWAY				= 0x08000000;

const uint  AGENT_CONTROL_LBUTTON_DOWN		= 0x10000000;
const uint  AGENT_CONTROL_LBUTTON_UP		= 0x20000000;
const uint  AGENT_CONTROL_ML_LBUTTON_DOWN	= 0x40000000;
const uint  AGENT_CONTROL_ML_LBUTTON_UP		= 0x80000000;

const uint  AGENT_CONTROL_AT 		= AGENT_CONTROL_AT_POS 
								  | AGENT_CONTROL_AT_NEG 
								  | AGENT_CONTROL_NUDGE_AT_POS 
								  | AGENT_CONTROL_NUDGE_AT_NEG;

const uint  AGENT_CONTROL_LEFT 	= AGENT_CONTROL_LEFT_POS 
								  | AGENT_CONTROL_LEFT_NEG 
								  | AGENT_CONTROL_NUDGE_LEFT_POS 
								  | AGENT_CONTROL_NUDGE_LEFT_NEG;

const uint  AGENT_CONTROL_UP 		= AGENT_CONTROL_UP_POS 
								  | AGENT_CONTROL_UP_NEG 
								  | AGENT_CONTROL_NUDGE_UP_POS 
								  | AGENT_CONTROL_NUDGE_UP_NEG;

const uint  AGENT_CONTROL_HORIZONTAL = AGENT_CONTROL_AT 
									 | AGENT_CONTROL_LEFT;

const uint  AGENT_CONTROL_NOT_USED_BY_LSL = AGENT_CONTROL_FLY 
										  | AGENT_CONTROL_STOP 
										  | AGENT_CONTROL_FINISH_ANIM 
										  | AGENT_CONTROL_STAND_UP 
										  | AGENT_CONTROL_SIT_ON_GROUND 
										  | AGENT_CONTROL_MOUSELOOK 
										  | AGENT_CONTROL_AWAY;

const uint  AGENT_CONTROL_MOVEMENT = AGENT_CONTROL_AT 
								   | AGENT_CONTROL_LEFT 
								   | AGENT_CONTROL_UP;

const uint  AGENT_CONTROL_ROTATION = AGENT_CONTROL_PITCH_POS 
								   | AGENT_CONTROL_PITCH_NEG 
								   | AGENT_CONTROL_YAW_POS 
								   | AGENT_CONTROL_YAW_NEG;

const uint  AGENT_CONTROL_NUDGE = AGENT_CONTROL_NUDGE_AT_POS
								| AGENT_CONTROL_NUDGE_AT_NEG
								| AGENT_CONTROL_NUDGE_LEFT_POS
								| AGENT_CONTROL_NUDGE_LEFT_NEG;


// move these up so that we can hide them in "State" for object updates 
// (for now)
const uint  AGENT_ATTACH_OFFSET				= 4;
const uint  AGENT_ATTACH_MASK				= 0xf << (int)AGENT_ATTACH_OFFSET;
const uint  AGENT_ATTACH_CLEAR				= 0x00;

// RN: this method swaps the upper and lower nibbles to maintain backward 
// compatibility with old objects that only used the upper nibble


// test state for use in testing grabbing the camera
const uint  AGENT_CAMERA_OBJECT				= ((uint)0x1) << 3;

const double MAX_ATTACHMENT_DIST = 3.5f; // meters?
		
const uint HTTP_METHOD=0;
		
		

//Ventrella Follow Cam Script Stuff
enum EFollowCamAttributes {
	FOLLOWCAM_PITCH = 0,
	FOLLOWCAM_FOCUS_OFFSET,
	FOLLOWCAM_FOCUS_OFFSET_X, //this HAS to come after FOLLOWCAM_FOCUS_OFFSET in this list
	FOLLOWCAM_FOCUS_OFFSET_Y,
	FOLLOWCAM_FOCUS_OFFSET_Z,
	FOLLOWCAM_POSITION_LAG,	
	FOLLOWCAM_FOCUS_LAG,	
	FOLLOWCAM_DISTANCE,	
	FOLLOWCAM_BEHINDNESS_ANGLE,
	FOLLOWCAM_BEHINDNESS_LAG,
	FOLLOWCAM_POSITION_THRESHOLD,
	FOLLOWCAM_FOCUS_THRESHOLD,	
	FOLLOWCAM_ACTIVE,			
	FOLLOWCAM_POSITION,			
	FOLLOWCAM_POSITION_X, //this HAS to come after FOLLOWCAM_POSITION in this list
	FOLLOWCAM_POSITION_Y,
	FOLLOWCAM_POSITION_Z,
	FOLLOWCAM_FOCUS,
	FOLLOWCAM_FOCUS_X, //this HAS to come after FOLLOWCAM_FOCUS in this list
	FOLLOWCAM_FOCUS_Y,
	FOLLOWCAM_FOCUS_Z,
	FOLLOWCAM_POSITION_LOCKED,
	FOLLOWCAM_FOCUS_LOCKED,
	NUM_FOLLOWCAM_ATTRIBUTES
};

//end Ventrella
}
}