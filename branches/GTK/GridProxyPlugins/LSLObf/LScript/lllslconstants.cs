/** 
 * @file lllslconstants.cs
 * @author James Cook
 * @brief Constants used in lsl.
 *
 * $LicenseInfo:firstyear=2006&license=viewergpl$
 * 
 * Copyright (c) 2006-2009, Linden Research, Inc.
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

namespace LSLObf
{
	public partial class LSLLexer
	{
		// LSL: Return flags for llGetAgentInfo
		const uint AGENT_FLYING		= 0x0001;
		const uint AGENT_ATTACHMENTS= 0x0002;
		const uint AGENT_SCRIPTED	= 0x0004;
		const uint AGENT_MOUSELOOK	= 0x0008;
		const uint AGENT_SITTING	= 0x0010;
		const uint AGENT_ON_OBJECT	= 0x0020;
		const uint AGENT_AWAY		= 0x0040;
		const uint AGENT_WALKING	= 0x0080;
		const uint AGENT_IN_AIR		= 0x0100;
		const uint AGENT_TYPING		= 0x0200;
		const uint AGENT_CROUCHING	= 0x0400;
		const uint AGENT_BUSY		= 0x0800;
		const uint AGENT_ALWAYS_RUN	= 0x1000;

		const int LSL_REMOTE_DATA_CHANNEL		= 1;
		const int LSL_REMOTE_DATA_REQUEST		= 2;
		const int LSL_REMOTE_DATA_REPLY			= 3;
		
		// Constants used in extended LSL primitive setter and getters
		const int LSL_PRIM_TYPE_LEGACY	= 1; // No longer supported.
		const int LSL_PRIM_MATERIAL		= 2;
		const int LSL_PRIM_PHYSICS		= 3;
		const int LSL_PRIM_TEMP_ON_REZ	= 4;
		const int LSL_PRIM_PHANTOM		= 5;
		const int LSL_PRIM_POSITION		= 6;
		const int LSL_PRIM_SIZE			= 7;
		const int LSL_PRIM_ROTATION		= 8;
		const int LSL_PRIM_TYPE			= 9; // Replacement for LSL_PRIM_TYPE_LEGACY
		const int LSL_PRIM_TEXTURE		= 17;
		const int LSL_PRIM_COLOR		= 18;
		const int LSL_PRIM_BUMP_SHINY	= 19;
		const int LSL_PRIM_FULLBRIGHT	= 20;
		const int LSL_PRIM_FLEXIBLE		= 21;
		const int LSL_PRIM_TEXGEN		= 22;
		const int LSL_PRIM_POINT_LIGHT	= 23;
		const int LSL_PRIM_CAST_SHADOWS	= 24;
		const int LSL_PRIM_GLOW     	= 25;
		
		const int LSL_PRIM_TYPE_BOX		= 0;
		const int LSL_PRIM_TYPE_CYLINDER= 1;
		const int LSL_PRIM_TYPE_PRISM	= 2;
		const int LSL_PRIM_TYPE_SPHERE	= 3;
		const int LSL_PRIM_TYPE_TORUS	= 4;
		const int LSL_PRIM_TYPE_TUBE	= 5;
		const int LSL_PRIM_TYPE_RING	= 6;
		const int LSL_PRIM_TYPE_SCULPT  = 7;
		
		const int LSL_PRIM_HOLE_DEFAULT = 0x00;
		const int LSL_PRIM_HOLE_CIRCLE	= 0x10;
		const int LSL_PRIM_HOLE_SQUARE  = 0x20;
		const int LSL_PRIM_HOLE_TRIANGLE= 0x30;
		
		const int LSL_PRIM_MATERIAL_STONE	= 0;
		const int LSL_PRIM_MATERIAL_METAL	= 1;
		const int LSL_PRIM_MATERIAL_GLASS	= 2;
		const int LSL_PRIM_MATERIAL_WOOD	= 3;
		const int LSL_PRIM_MATERIAL_FLESH	= 4;
		const int LSL_PRIM_MATERIAL_PLASTIC	= 5;
		const int LSL_PRIM_MATERIAL_RUBBER	= 6;
		const int LSL_PRIM_MATERIAL_LIGHT	= 7;
		
		const int LSL_PRIM_SHINY_NONE		= 0;
		const int LSL_PRIM_SHINY_LOW		= 1;
		const int LSL_PRIM_SHINY_MEDIUM		= 2;
		const int LSL_PRIM_SHINY_HIGH		= 3;
		
		const int LSL_PRIM_TEXGEN_DEFAULT	= 0;
		const int LSL_PRIM_TEXGEN_PLANAR	= 1;
		
		const int LSL_PRIM_BUMP_NONE		= 0;
		const int LSL_PRIM_BUMP_BRIGHT		= 1;
		const int LSL_PRIM_BUMP_DARK		= 2;
		const int LSL_PRIM_BUMP_WOOD		= 3;
		const int LSL_PRIM_BUMP_BARK		= 4;
		const int LSL_PRIM_BUMP_BRICKS		= 5;
		const int LSL_PRIM_BUMP_CHECKER		= 6;
		const int LSL_PRIM_BUMP_CONCRETE	= 7;
		const int LSL_PRIM_BUMP_TILE		= 8;
		const int LSL_PRIM_BUMP_STONE		= 9;
		const int LSL_PRIM_BUMP_DISKS		= 10;
		const int LSL_PRIM_BUMP_GRAVEL		= 11;
		const int LSL_PRIM_BUMP_BLOBS		= 12;
		const int LSL_PRIM_BUMP_SIDING		= 13;
		const int LSL_PRIM_BUMP_LARGETILE	= 14;
		const int LSL_PRIM_BUMP_STUCCO		= 15;
		const int LSL_PRIM_BUMP_SUCTION		= 16;
		const int LSL_PRIM_BUMP_WEAVE		= 17;
		
		const int LSL_PRIM_SCULPT_TYPE_SPHERE   = 1;
		const int LSL_PRIM_SCULPT_TYPE_TORUS    = 2;
		const int LSL_PRIM_SCULPT_TYPE_PLANE    = 3;
		const int LSL_PRIM_SCULPT_TYPE_CYLINDER = 4;
		const int LSL_PRIM_SCULPT_TYPE_MASK     = 7;
		const int LSL_PRIM_SCULPT_FLAG_INVERT   = 64;
		const int LSL_PRIM_SCULPT_FLAG_MIRROR   = 128;
		
		const int LSL_ALL_SIDES				= -1;
		const int LSL_LINK_ROOT				= 1;
		const int LSL_LINK_FIRST_CHILD		= 2;
		const int LSL_LINK_SET				= -1;
		const int LSL_LINK_ALL_OTHERS		= -2;
		const int LSL_LINK_ALL_CHILDREN		= -3;
		const int LSL_LINK_THIS				= -4;
		
		// LSL constants for llSetForSell
		const int SELL_NOT			= 0;
		const int SELL_ORIGINAL		= 1;
		const int SELL_COPY			= 2;
		const int SELL_CONTENTS		= 3;
		
		// LSL constants for llSetPayPrice
		const int PAY_PRICE_HIDE = -1;
		const int PAY_PRICE_DEFAULT = -2;
		const int MAX_PAY_BUTTONS = 4;
		const int PAY_BUTTON_DEFAULT_0 = 1;
		const int PAY_BUTTON_DEFAULT_1 = 5;
		const int PAY_BUTTON_DEFAULT_2 = 10;
		const int PAY_BUTTON_DEFAULT_3 = 20;
		
		// lsl email registration.
		const int EMAIL_REG_SUBSCRIBE_OBJECT	= 0x01;
		const int EMAIL_REG_UNSUBSCRIBE_OBJECT	= 0x02;
		const int EMAIL_REG_UNSUBSCRIBE_SIM		= 0x04;
		
		const int LIST_STAT_RANGE = 0;
		const int LIST_STAT_MIN		= 1;
		const int LIST_STAT_MAX		= 2;
		const int LIST_STAT_MEAN	= 3;
		const int LIST_STAT_MEDIAN	= 4;
		const int LIST_STAT_STD_DEV	= 5;
		const int LIST_STAT_SUM = 6;
		const int LIST_STAT_SUM_SQUARES = 7;
		const int LIST_STAT_NUM_COUNT = 8;
		const int LIST_STAT_GEO_MEAN = 9;
		
		const int STRING_TRIM_HEAD = 0x01;
		const int STRING_TRIM_TAIL = 0x02;
		const int STRING_TRIM = STRING_TRIM_HEAD | STRING_TRIM_TAIL;
		
		// llGetObjectDetails
		const int OBJECT_UNKNOWN_DETAIL = -1;
		const int OBJECT_NAME = 1;
		const int OBJECT_DESC = 2;
		const int OBJECT_POS = 3;
		const int OBJECT_ROT = 4;
		const int OBJECT_VELOCITY = 5;
		const int OBJECT_OWNER = 6;
		const int OBJECT_GROUP = 7;
		const int OBJECT_CREATOR = 8;

		// llTextBox() magic token string - yes this is a hack.  sue me.
		const string TEXTBOX_MAGIC_TOKEN = "!!llTextBox!!";

		// changed() event flags
		const uint	CHANGED_NONE = 0x0;
		const uint	CHANGED_INVENTORY = 0x1;
		const uint	CHANGED_COLOR = 0x2;
		const uint	CHANGED_SHAPE = 0x4;
		const uint	CHANGED_SCALE = 0x8;
		const uint	CHANGED_TEXTURE = 0x10;
		const uint	CHANGED_LINK = 0x20;
		const uint	CHANGED_ALLOWED_DROP = 0x40;
		const uint	CHANGED_OWNER = 0x80;
		const uint	CHANGED_REGION = 0x100;
		const uint	CHANGED_TELEPORT = 0x200;
		const uint	CHANGED_REGION_START = 0x400;
		
		const uint CLICK_ACTION_NONE = 0;
		const uint CLICK_ACTION_TOUCH = 0;
		const uint CLICK_ACTION_SIT = 1;
		const uint CLICK_ACTION_BUY = 2;
		const uint CLICK_ACTION_PAY = 3;
		const uint CLICK_ACTION_OPEN = 4;
		const uint CLICK_ACTION_PLAY = 5;
		const uint CLICK_ACTION_OPEN_MEDIA = 6;
		
		const string URL_REQUEST_DENIED = "URL_REQUEST_DENIED";
		const string URL_REQUEST_GRANTED = "URL_REQUEST_GRANTED";
	}
}