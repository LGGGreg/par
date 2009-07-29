
/** 
 * @file indra_constants.h
 * @brief some useful short term constants for Indra
 *
 * $LicenseInfo:firstyear=2001&license=viewergpl$
 * 
 * Copyright (c) 2001-2009, Linden Research, Inc.
 * 
 * Second Life Viewer Source Code
 * The source code in this file ("Source Code") is provided by Linden Lab
 * to you under the terms of the GNU General Public License, version 2.0
 * ("GPL"), unless you have obtained a separate licensing agreement
 * ("Other License"), formally executed by you and Linden Lab.  Terms of
 * the GPL can be found in doc/GPL-license.txt in this distribution, or
 * online at http://secondlifegrid.net/programs/open_source/licensing/gplv2
 * 
 * There are special exceptions to the terms and conditions of the GPL as
 * it is applied to this Source Code. View the full text of the exception
 * in the file doc/FLOSS-exception.txt in this software distribution, or
 * online at
 * http://secondlifegrid.net/programs/open_source/licensing/flossexception
 * 
 * By copying, modifying or distributing this software, you acknowledge
 * that you have read and understood your obligations described above,
 * and agree to abide by those obligations.
 * 
 * ALL LINDEN LAB SOURCE CODE IS PROVIDED "AS IS." LINDEN LAB MAKES NO
 * WARRANTIES, EXPRESS, IMPLIED OR OTHERWISE, REGARDING ITS ACCURACY,
 * COMPLETENESS OR PERFORMANCE.
 * $/LicenseInfo$
 */
using System;
using OpenMetaverse;

namespace LSLObf
{
	public partial class LSLLexer
	{

// At 45 Hz collisions seem stable and objects seem
// to settle down at a reasonable rate.
// JC 3/18/2003

// const double PHYSICS_TIMESTEP = 1.f / 45.0;
// This must be a #define due to anal retentive restrictions on const expressions
// CG 2008-06-05
const double PHYSICS_TIMESTEP = (1.0 / 45.0);

const double COLLISION_TOLERANCE = 0.1;
const double HALF_COLLISION_TOLERANCE = COLLISION_TOLERANCE * 0.5;

// Time constants
const uint HOURS_PER_LINDEN_DAY		= 4;	
const uint DAYS_PER_LINDEN_YEAR		= 11;

const uint SEC_PER_LINDEN_DAY		= HOURS_PER_LINDEN_DAY * 60 * 60;
const uint SEC_PER_LINDEN_YEAR		= DAYS_PER_LINDEN_YEAR * SEC_PER_LINDEN_DAY;

const double REGION_WIDTH_METERS = 256.0;
const int REGION_WIDTH_UNITS = 256;
const uint REGION_WIDTH_uint = 256;

const double REGION_HEIGHT_METERS = 4096.0;

// Bits for simulator performance query flags
enum LAND_STAT_FLAGS :uint
{
	STAT_FILTER_BY_PARCEL	= 0x00000001,
	STAT_FILTER_BY_OWNER	= 0x00000002,
	STAT_FILTER_BY_OBJECT	= 0x00000004,
	STAT_REQUEST_LAST_ENTRY	= 0x80000000,
};

enum LAND_STAT_REPORT_TYPE
{
	STAT_REPORT_TOP_SCRIPTS = 0,
	STAT_REPORT_TOP_COLLIDERS
};

const uint STAT_FILTER_MASK	= 0x1FFFFFFF;

// Region absolute limits
const int		REGION_AGENT_COUNT_MIN = 1;
const int		REGION_AGENT_COUNT_MAX = 200;			// Must fit in uint for the moment (RegionInfo msg)
const int		REGION_PRIM_COUNT_MIN = 0;
const int		REGION_PRIM_COUNT_MAX = 40000;
const double		REGION_PRIM_BONUS_MIN = 1.0;
const double		REGION_PRIM_BONUS_MAX = 10.0;

// Default maximum number of tasks/prims per region.
const uint DEFAULT_MAX_REGION_WIDE_PRIM_COUNT = 15000;

const 	double 	MIN_AGENT_DEPTH			= 0.30;
const 	double 	DEFAULT_AGENT_DEPTH 	= 0.45;
const 	double 	MAX_AGENT_DEPTH			= 0.60;

const 	double 	MIN_AGENT_WIDTH 		= 0.40;
const 	double 	DEFAULT_AGENT_WIDTH 	= 0.60;
const 	double 	MAX_AGENT_WIDTH 		= 0.80;

const 	double 	MIN_AGENT_HEIGHT		= 1.3 - 2.0 * COLLISION_TOLERANCE;
const 	double 	DEFAULT_AGENT_HEIGHT	= 1.9;
const 	double 	MAX_AGENT_HEIGHT		= 2.65 - 2.0 * COLLISION_TOLERANCE;

// For linked sets
const int MAX_CHILDREN_PER_TASK = 255;
const int MAX_CHILDREN_PER_PHYSICAL_TASK = 32;

const int MAX_JOINTS_PER_OBJECT = 1;	// limiting to 1 until Havok 2.x

const string	DEFAULT_DMZ_SPACE_SERVER	= "192.168.0.140";
const string	DEFAULT_DMZ_USER_SERVER		= "192.168.0.140";
const string	DEFAULT_DMZ_DATA_SERVER		= "192.168.0.140";
const string	DEFAULT_DMZ_ASSET_SERVER	= "http://asset.dmz.lindenlab.com:80";

const string	DEFAULT_AGNI_SPACE_SERVER	= "63.211.139.100";
const string	DEFAULT_AGNI_USER_SERVER	= "63.211.139.100";
const string	DEFAULT_AGNI_DATA_SERVER	= "63.211.139.100";
const string	DEFAULT_AGNI_ASSET_SERVER	= "http://asset.agni.lindenlab.com:80";

// Information about what ports are for what services is in the wiki Name Space Ports page
// https://wiki.lindenlab.com/wiki/Name_Space_Ports
const string DEFAULT_LOCAL_ASSET_SERVER	= "http://localhost:12041/asset/tmp";
const string	LOCAL_ASSET_URL_FORMAT		= "http://%s:12041/asset";

const	uint		DEFAULT_LAUNCHER_PORT			= 12029;
//const	uint		DEFAULT_BIGBOARD_PORT			= 12030; // Deprecated
//const	uint		DEFAULT_QUERYSIM_PORT			= 12031; // Deprecated
const	uint		DEFAULT_DATA_SERVER_PORT		= 12032;
const	uint		DEFAULT_SPACE_SERVER_PORT		= 12033;
const	uint		DEFAULT_VIEWER_PORT				= 12034;
const	uint		DEFAULT_SIMULATOR_PORT			= 12035;
const	uint		DEFAULT_USER_SERVER_PORT		= 12036;
const	uint		DEFAULT_RPC_SERVER_PORT			= 12037;
const	uint		DEFAULT_LOG_DATA_SERVER_PORT	= 12039;
const	uint		DEFAULT_BACKBONE_PORT			= 12040;
const   uint		DEFAULT_LOCAL_ASSET_PORT		= 12041;
//const   uint		DEFAULT_BACKBONE_CAP_PORT		= 12042; // Deprecated
const   uint		DEFAULT_CAP_PROXY_PORT			= 12043;
const   uint		DEFAULT_INV_DATA_SERVER_PORT	= 12044;
const	uint		DEFAULT_CGI_SERVICES_PORT		= 12045;

// Mapserver uses ports 12124 - 12139 to allow multiple mapservers to run
// on a single host for map tile generation. JC
const	uint		DEFAULT_MAPSERVER_PORT			= 12124;

// For automatic port discovery when running multiple viewers on one host
const	uint		PORT_DISCOVERY_RANGE_MIN		= 13000;
const	uint		PORT_DISCOVERY_RANGE_MAX		= PORT_DISCOVERY_RANGE_MIN + 50;

const	char	LAND_LAYER_CODE					= 'L';
const	char	WATER_LAYER_CODE				= 'W';
const	char	WIND_LAYER_CODE					= '7';
const	char	CLOUD_LAYER_CODE				= '8';


// keys
// Bit masks for various keyboard modifier keys.
const uint MASK_NONE =			0x0000;
const uint MASK_CONTROL =		0x0001;		// Mapped to cmd on Macs
const uint MASK_ALT =			0x0002;
const uint MASK_SHIFT =			0x0004;
const uint MASK_NORMALKEYS =    0x0007;     // A real mask - only get the bits for normal modifier keys
const uint MASK_MAC_CONTROL =	0x0008;		// Un-mapped Ctrl key on Macs, not used on Windows
const uint MASK_MODIFIERS =		MASK_CONTROL|MASK_ALT|MASK_SHIFT|MASK_MAC_CONTROL;

// Special keys go into >128
const uint KEY_SPECIAL = 0x80;	// special keys start here
const uint KEY_RETURN =	0x81;
const uint KEY_LEFT =	0x82;
const uint KEY_RIGHT =	0x83;
const uint KEY_UP =		0x84;
const uint KEY_DOWN =	0x85;
const uint KEY_ESCAPE =	0x86;
const uint KEY_BACKSPACE =0x87;
const uint KEY_DELETE =	0x88;
const uint KEY_SHIFT =	0x89;
const uint KEY_CONTROL =	0x8A;
const uint KEY_ALT =		0x8B;
const uint KEY_HOME =	0x8C;
const uint KEY_END =		0x8D;
const uint KEY_PAGE_UP = 0x8E;
const uint KEY_PAGE_DOWN = 0x8F;
const uint KEY_HYPHEN = 0x90;
const uint KEY_EQUALS = 0x91;
const uint KEY_INSERT = 0x92;
const uint KEY_CAPSLOCK = 0x93;
const uint KEY_TAB =		0x94;
const uint KEY_ADD = 	0x95;
const uint KEY_SUBTRACT =0x96;
const uint KEY_MULTIPLY =0x97;
const uint KEY_DIVIDE =	0x98;
const uint KEY_F1		= 0xA1;
const uint KEY_F2		= 0xA2;
const uint KEY_F3		= 0xA3;
const uint KEY_F4		= 0xA4;
const uint KEY_F5		= 0xA5;
const uint KEY_F6		= 0xA6;
const uint KEY_F7		= 0xA7;
const uint KEY_F8		= 0xA8;
const uint KEY_F9		= 0xA9;
const uint KEY_F10		= 0xAA;
const uint KEY_F11		= 0xAB;
const uint KEY_F12		= 0xAC;

const uint KEY_PAD_UP		= 0xC0;
const uint KEY_PAD_DOWN		= 0xC1;
const uint KEY_PAD_LEFT		= 0xC2;
const uint KEY_PAD_RIGHT		= 0xC3;
const uint KEY_PAD_HOME		= 0xC4;
const uint KEY_PAD_END		= 0xC5;
const uint KEY_PAD_PGUP		= 0xC6;
const uint KEY_PAD_PGDN		= 0xC7;
const uint KEY_PAD_CENTER	= 0xC8; // the 5 in the middle
const uint KEY_PAD_INS		= 0xC9;
const uint KEY_PAD_DEL		= 0xCA;
const uint KEY_PAD_RETURN	= 0xCB;
const uint KEY_PAD_ADD		= 0xCC; // not used
const uint KEY_PAD_SUBTRACT	= 0xCD; // not used
const uint KEY_PAD_MULTIPLY  = 0xCE; // not used
const uint KEY_PAD_DIVIDE	= 0xCF; // not used

const uint KEY_BUTTON0	= 0xD0;
const uint KEY_BUTTON1	= 0xD1;
const uint KEY_BUTTON2	= 0xD2;
const uint KEY_BUTTON3	= 0xD3;
const uint KEY_BUTTON4	= 0xD4;
const uint KEY_BUTTON5	= 0xD5;
const uint KEY_BUTTON6	= 0xD6;
const uint KEY_BUTTON7	= 0xD7;
const uint KEY_BUTTON8	= 0xD8;
const uint KEY_BUTTON9	= 0xD9;
const uint KEY_BUTTON10	= 0xDA;
const uint KEY_BUTTON11	= 0xDB;
const uint KEY_BUTTON12	= 0xDC;
const uint KEY_BUTTON13	= 0xDD;
const uint KEY_BUTTON14	= 0xDE;
const uint KEY_BUTTON15	= 0xDF;

const uint KEY_NONE =	0xFF; // not sent from keyboard.  For internal use only.

const int KEY_COUNT = 256;



const double DEFAULT_WATER_HEIGHT 	= 20.0;

// Maturity ratings for simulators
const uint SIM_ACCESS_MIN 	= 0;		// Treated as 'unknown', usually ends up being SIM_ACCESS_PG
const uint SIM_ACCESS_PG		= 13;
const uint SIM_ACCESS_MATURE	= 21;
const uint SIM_ACCESS_ADULT	= 42;		// Seriously Adult Only
const uint SIM_ACCESS_DOWN	= 254;
const uint SIM_ACCESS_MAX 	= SIM_ACCESS_ADULT;

// group constants
const int MAX_AGENT_GROUPS = 25;

// god levels
const uint GOD_MAINTENANCE = 250;
const uint GOD_FULL = 200;
const uint GOD_LIAISON = 150;
const uint GOD_CUSTOMER_SERVICE = 100;
const uint GOD_LIKE = 1;
const uint GOD_NOT = 0;

// "agent id" for things that should be done to ALL agents
const string LL_UUID_ALL_AGENTS="44e87126-e794-4ded-05b3-7c42da3d5cdb";

// Governor Linden's agent id.
const string ALEXANDRIA_LINDEN_ID="ba2a564a-f0f1-4b82-9c61-b7520bfcd09f";
const string GOVERNOR_LINDEN_ID="3d6181b0-6a4b-97ef-18d8-722652995cf1";
const string REALESTATE_LINDEN_ID="3d6181b0-6a4b-97ef-18d8-722652995cf1";
// Maintenance's group id.
const string MAINTENANCE_GROUP_ID="dc7b21cd-3c89-fcaa-31c8-25f9ffd224cd";

// Flags for kick message
const uint KICK_FLAGS_DEFAULT	= 0x0;
const uint KICK_FLAGS_FREEZE		= 1 << 0;
const uint KICK_FLAGS_UNFREEZE	= 1 << 1;

const uint UPD_NONE      		= 0x00;
const uint UPD_POSITION  		= 0x01;
const uint UPD_ROTATION  		= 0x02;
const uint UPD_SCALE     		= 0x04;
const uint UPD_LINKED_SETS 	= 0x08;
const uint UPD_UNIFORM 		= 0x10;	// used with UPD_SCALE

// Agent Update Flags (uint)
const uint AU_FLAGS_NONE      		= 0x00;
const uint AU_FLAGS_HIDETITLE      	= 0x01;

// start location constants
const uint START_LOCATION_ID_LAST 		= 0;
const uint START_LOCATION_ID_HOME 		= 1;
const uint START_LOCATION_ID_DIRECT	 	= 2;	// for direct teleport
const uint START_LOCATION_ID_PARCEL	 	= 3;	// for teleports to a parcel
const uint START_LOCATION_ID_TELEHUB 	= 4;	// for teleports to a spawnpoint
const uint START_LOCATION_ID_URL			= 5;
const uint START_LOCATION_ID_COUNT 		= 6;

// group constants
const uint GROUP_MIN_SIZE = 2;

// radius within which a chat message is fully audible
const double CHAT_WHISPER_RADIUS = 10.0;
const double CHAT_NORMAL_RADIUS = 20.0;
const double CHAT_SHOUT_RADIUS = 100.0;
const double CHAT_MAX_RADIUS = CHAT_SHOUT_RADIUS;
const double CHAT_MAX_RADIUS_BY_TWO = CHAT_MAX_RADIUS / 2.0;

// this times above gives barely audible radius
const double CHAT_BARELY_AUDIBLE_FACTOR = 2.0;

// distance in front of speaking agent the sphere is centered
const double CHAT_WHISPER_OFFSET = 5.0;
const double CHAT_NORMAL_OFFSET = 10.0;
const double CHAT_SHOUT_OFFSET = 50.0;

// first clean starts at 3 AM
const int SANDBOX_FIRST_CLEAN_HOUR = 3;
// clean every <n> hours
const int SANDBOX_CLEAN_FREQ = 12;

const double WIND_SCALE_HACK		= 2.0;	// hack to make wind speeds more realistic

enum ETerrainBrushType
{
	// the valid brush numbers cannot be reordered, because they 
	// are used in the binary LSL format as arguments to llModifyLand()
	E_LANDBRUSH_LEVEL	= 0,
	E_LANDBRUSH_RAISE	= 1,
	E_LANDBRUSH_LOWER	= 2,
	E_LANDBRUSH_SMOOTH 	= 3,
	E_LANDBRUSH_NOISE	= 4,
	E_LANDBRUSH_REVERT 	= 5,
	E_LANDBRUSH_INVALID = 6
}

// media commands
const uint PARCEL_MEDIA_COMMAND_STOP  = 0;
const uint PARCEL_MEDIA_COMMAND_PAUSE = 1;
const uint PARCEL_MEDIA_COMMAND_PLAY  = 2;
const uint PARCEL_MEDIA_COMMAND_LOOP  = 3;
const uint PARCEL_MEDIA_COMMAND_TEXTURE = 4;
const uint PARCEL_MEDIA_COMMAND_URL = 5;
const uint PARCEL_MEDIA_COMMAND_TIME = 6;
const uint PARCEL_MEDIA_COMMAND_AGENT = 7;
const uint PARCEL_MEDIA_COMMAND_UNLOAD = 8;
const uint PARCEL_MEDIA_COMMAND_AUTO_ALIGN = 9;
const uint PARCEL_MEDIA_COMMAND_TYPE = 10;
const uint PARCEL_MEDIA_COMMAND_SIZE = 11;
const uint PARCEL_MEDIA_COMMAND_DESC = 12;
const uint PARCEL_MEDIA_COMMAND_LOOP_SET = 13;

// map item types
const uint MAP_ITEM_TELEHUB = 0x01;
const uint MAP_ITEM_PG_EVENT = 0x02;
const uint MAP_ITEM_MATURE_EVENT = 0x03;
const uint MAP_ITEM_POPULAR = 0x04;
//const uint MAP_ITEM_AGENT_COUNT = 0x05;
const uint MAP_ITEM_AGENT_LOCATIONS = 0x06;
const uint MAP_ITEM_LAND_FOR_SALE = 0x07;
const uint MAP_ITEM_CLASSIFIED = 0x08;
const uint MAP_ITEM_ADULT_EVENT = 0x09;
const uint MAP_ITEM_LAND_FOR_SALE_ADULT = 0x0a;

// Crash reporter behavior
const string CRASH_SETTINGS_FILE = "settings_crash_behavior.xml";
const string CRASH_BEHAVIOR_SETTING = "CrashSubmitBehavior";
const int CRASH_BEHAVIOR_ASK = 0;
const int CRASH_BEHAVIOR_ALWAYS_SEND = 1;
const int CRASH_BEHAVIOR_NEVER_SEND = 2;

// Export/Import return values
const int EXPORT_SUCCESS = 0;
const int EXPORT_ERROR_PERMISSIONS = -1;
const int EXPORT_ERROR_UNKNOWN = -2;

// This is how long the sim will try to teleport you before giving up.
const double TELEPORT_EXPIRY = 15.0;
// Additional time (in seconds) to wait per attachment
const double TELEPORT_EXPIRY_PER_ATTACHMENT = 3.0;

// The maximum size of an object extra parameters binary (packed) block


const int CHAT_CHANNEL_DEBUG = Int32.MaxValue;

// PLEASE don't add constants here.  Every dev will have to do
// a complete rebuild.  Try to find another shared header file,
// like llregionflags.h, lllslconstants.h, llagentconstants.h,
// or create a new one.  JC
		
const double	GRAVITY			= -9.8f;

// mathematical constants
const double	F_PI		= 3.1415926535897932384626433832795f;
const double	F_TWO_PI	= 6.283185307179586476925286766559f;
const double	F_PI_BY_TWO	= 1.5707963267948966192313216916398f;
const double	F_SQRT_TWO_PI = 2.506628274631000502415765284811f;
const double	F_E			= 2.71828182845904523536f;
const double	F_SQRT2		= 1.4142135623730950488016887242097f;
const double	F_SQRT3		= 1.73205080756888288657986402541f;
const double	OO_SQRT2	= 0.7071067811865475244008443621049f;
const double	DEG_TO_RAD	= 0.017453292519943295769236907684886f;
const double	RAD_TO_DEG	= 57.295779513082320876798154814105f;
const double	F_APPROXIMATELY_ZERO = 0.00001f;
const double	F_LN2		= 0.69314718056f;
const double	OO_LN2		= 1.4426950408889634073599246810019f;

const double	F_ALMOST_ZERO	= 0.0001f;
const double	F_ALMOST_ONE	= 1.0f - F_ALMOST_ZERO;

// BUG: Eliminate in favor of F_APPROXIMATELY_ZERO above?
const double FP_MAG_THRESHOLD = 0.0000001f;
	}
}