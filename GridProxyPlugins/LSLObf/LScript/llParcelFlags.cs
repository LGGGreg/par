namespace LSLObf
{
	public partial class LSLLexer
	{
		const uint PF_ALLOW_FLY			= 1 << 0;// Can start flying
		const uint PF_ALLOW_OTHER_SCRIPTS= 1 << 1;// Scripts by others can run.
		const uint PF_FOR_SALE			= 1 << 2;// Can buy this land
		const uint PF_FOR_SALE_OBJECTS	= 1 << 7;// Can buy all objects on this land
		const uint PF_ALLOW_LANDMARK		= 1 << 3;
		const uint PF_ALLOW_TERRAFORM	= 1 << 4;
		const uint PF_ALLOW_DAMAGE		= 1 << 5;
		const uint PF_CREATE_OBJECTS		= 1 << 6;
		// 7 is moved above
		const uint PF_USE_ACCESS_GROUP	= 1 << 8;
		const uint PF_USE_ACCESS_LIST	= 1 << 9;
		const uint PF_USE_BAN_LIST		= 1 << 10;
		const uint PF_USE_PASS_LIST		= 1 << 11;
		const uint PF_SHOW_DIRECTORY		= 1 << 12;
		const uint PF_ALLOW_DEED_TO_GROUP		= 1 << 13;
		const uint PF_CONTRIBUTE_WITH_DEED		= 1 << 14;
		const uint PF_SOUND_LOCAL				= 1 << 15;	// Hear sounds in this parcel only
		const uint PF_SELL_PARCEL_OBJECTS		= 1 << 16;	// Objects on land are included as part of the land when the land is sold
		const uint PF_ALLOW_PUBLISH				= 1 << 17;	// Allow publishing of parcel information on the web
		const uint PF_MATURE_PUBLISH				= 1 << 18;	// The information on this parcel is mature
		const uint PF_URL_WEB_PAGE				= 1 << 19;	// The "media URL" is an HTML page
		const uint PF_URL_RAW_HTML				= 1 << 20;	// The "media URL" is a raw HTML string like <H1>Foo</H1>
		const uint PF_RESTRICT_PUSHOBJECT		= 1 << 21;	// Restrict push object to either on agent or on scripts owned by parcel owner
		const uint PF_DENY_ANONYMOUS				= 1 << 22;	// Deny all non identified/transacted accounts
		// const uint PF_DENY_IDENTIFIED			= 1 << 23;	// Deny identified accounts
		// const uint PF_DENY_TRANSACTED			= 1 << 24;	// Deny identified accounts
		const uint PF_ALLOW_GROUP_SCRIPTS		= 1 << 25;	// Allow scripts owned by group
		const uint PF_CREATE_GROUP_OBJECTS		= 1 << 26;	// Allow object creation by group members or objects
		const uint PF_ALLOW_ALL_OBJECT_ENTRY		= 1 << 27;	// Allow all objects to enter a parcel
		const uint PF_ALLOW_GROUP_OBJECT_ENTRY	= 1 << 28;	// Only allow group (and owner) objects to enter the parcel
		const uint PF_ALLOW_VOICE_CHAT			= 1 << 29;	// Allow residents to use voice chat on this parcel
		const uint PF_USE_ESTATE_VOICE_CHAN      = 1 << 30;
		const uint PF_DENY_AGEUNVERIFIED         = (uint)1 << 31;  // Prevent residents who aren't age-verified 
		// NOTE: At one point we have used all of the bits.
		// We have deprecated two of them in 1.19.0 which *could* be reused,
		// but only after we are certain there are no simstates using those bits.
		
		//const uint PF_RESERVED			= 1U << 31;
		
		// If any of these are true the parcel is restricting access in some maner.
		const uint PF_USE_RESTRICTED_ACCESS = PF_USE_ACCESS_GROUP
												| PF_USE_ACCESS_LIST
												| PF_USE_BAN_LIST
												| PF_USE_PASS_LIST
												| PF_DENY_ANONYMOUS
												| PF_DENY_AGEUNVERIFIED;
		const uint PF_NONE = 0x00000000;
		const uint PF_ALL  = 0xFFFFFFFF;
		const uint PF_DEFAULT =  PF_ALLOW_FLY
								| PF_ALLOW_OTHER_SCRIPTS
								| PF_ALLOW_GROUP_SCRIPTS
								| PF_ALLOW_LANDMARK
								| PF_CREATE_OBJECTS
								| PF_CREATE_GROUP_OBJECTS
								| PF_USE_BAN_LIST
								| PF_ALLOW_ALL_OBJECT_ENTRY
								| PF_ALLOW_GROUP_OBJECT_ENTRY
		                        | PF_ALLOW_VOICE_CHAT
		                        | PF_USE_ESTATE_VOICE_CHAN;
		
		// Access list flags
		const uint AL_ACCESS  = (1 << 0);
		const uint AL_BAN     = (1 << 1);
		//const uint AL_RENTER  = (1 << 2);
		
		// Block access return values. BA_ALLOWED is the only success case
		// since some code in the simulator relies on that assumption. All
		// other BA_ values should be reasons why you are not allowed.
		const int BA_ALLOWED = 0;
		const int BA_NOT_IN_GROUP = 1;
		const int BA_NOT_ON_LIST = 2;
		const int BA_BANNED = 3;
		const int BA_NO_ACCESS_LEVEL = 4;
		const int BA_NOT_AGE_VERIFIED = 5;
		
		// ParcelRelease flags
		const uint PR_NONE		= 0x0;
		const uint PR_GOD_FORCE	= (1 << 0);
		
		enum EObjectCategory
		{
			OC_INVALID = -1,
			OC_NONE = 0,
			OC_TOTAL = 0,	// yes zero, like OC_NONE
			OC_OWNER,
			OC_GROUP,
			OC_OTHER,
			OC_SELECTED,
			OC_TEMP,
			OC_COUNT
		};
		
		const int PARCEL_DETAILS_NAME = 0;
		const int PARCEL_DETAILS_DESC = 1;
		const int PARCEL_DETAILS_OWNER = 2;
		const int PARCEL_DETAILS_GROUP = 3;
		const int PARCEL_DETAILS_AREA = 4;
	}
}