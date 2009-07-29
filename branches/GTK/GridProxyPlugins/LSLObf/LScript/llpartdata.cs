/** 
 * @file llpartdata.h
 * @brief Particle system data packing
 *
 * $LicenseInfo:firstyear=2003&license=viewergpl$
 * 
 * Copyright (c) 2003-2009, Linden Research, Inc.
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

namespace LSLObf
{
	public partial class LSLLexer
	{

		const int PS_CUR_VERSION = 18;
		
		//
		// These constants are used by the script code, not by the particle system itself
		//
		
		enum LLPSScriptFlags
		{
			// Flags for the different parameters of individual particles
			LLPS_PART_FLAGS,
			LLPS_PART_START_COLOR,
			LLPS_PART_START_ALPHA,
			LLPS_PART_END_COLOR,
			LLPS_PART_END_ALPHA,
			LLPS_PART_START_SCALE,
			LLPS_PART_END_SCALE,
			LLPS_PART_MAX_AGE,
		
			// Flags for the different parameters of the particle source
			LLPS_SRC_ACCEL,
			LLPS_SRC_PATTERN,
			LLPS_SRC_INNERANGLE,
			LLPS_SRC_OUTERANGLE,
			LLPS_SRC_TEXTURE,
			LLPS_SRC_BURST_RATE,
			LLPS_SRC_BURST_DURATION,
			LLPS_SRC_BURST_PART_COUNT,
			LLPS_SRC_BURST_RADIUS,
			LLPS_SRC_BURST_SPEED_MIN,
			LLPS_SRC_BURST_SPEED_MAX,
			LLPS_SRC_MAX_AGE,
			LLPS_SRC_TARGET_UUID,
			LLPS_SRC_OMEGA,
			LLPS_SRC_ANGLE_BEGIN,
			LLPS_SRC_ANGLE_END
		};
		
		
		enum LLPartData :long
		{
				LL_PART_INTERP_COLOR_MASK =		0x01,
				LL_PART_INTERP_SCALE_MASK =		0x02,
				LL_PART_BOUNCE_MASK =			0x04,
				LL_PART_WIND_MASK =				0x08,
				LL_PART_FOLLOW_SRC_MASK =		0x10,		// Follows source, no rotation following (expensive!)
				LL_PART_FOLLOW_VELOCITY_MASK =	0x20,		// Particles orient themselves with velocity
				LL_PART_TARGET_POS_MASK =		0x40,
				LL_PART_TARGET_LINEAR_MASK =	0x80,		// Particle uses a direct linear interpolation
				LL_PART_EMISSIVE_MASK =			0x100,		// Particle is "emissive", instead of being lit
				LL_PART_BEAM_MASK =				0x200,		// Particle is a "beam" connecting source and target
		
				// Not implemented yet!
				//LL_PART_RANDOM_ACCEL_MASK =		0x100,		// Particles have random acceleration
				//LL_PART_RANDOM_VEL_MASK =		0x200,		// Particles have random velocity shifts"
				//LL_PART_TRAIL_MASK =			0x400,		// Particles have historical "trails"
		
				// Viewer side use only!
				LL_PART_HUD =					0x40000000,
				LL_PART_DEAD_MASK =				0x80000000,
		};
		
		
		enum LLPartSysData
		{
				LL_PART_SRC_OBJ_REL_MASK				= 0x01,		// Accel and velocity for particles relative object rotation
				LL_PART_USE_NEW_ANGLE					= 0x02,		// Particles uses new 'correct' angle parameters.
				LL_PART_SRC_PATTERN_DROP 				= 0x01,
				LL_PART_SRC_PATTERN_EXPLODE 			= 0x02,
				// Not implemented fully yet
				LL_PART_SRC_PATTERN_ANGLE =				0x04,
				LL_PART_SRC_PATTERN_ANGLE_CONE =		0x08,
				LL_PART_SRC_PATTERN_ANGLE_CONE_EMPTY =	0x10,
		};
	}
}