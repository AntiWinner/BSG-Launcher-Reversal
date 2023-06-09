namespace Eft.Launcher.Network.Http;

public static class ApiResponseCode
{
	public const int OK = 0;

	public const int ERR_BASE = 200;

	public const int ERR_CLIENT_NOT_AUTHORIZED = 201;

	public const int ERR_PASS_RESTORE_WRONG_EMAIL = 202;

	public const int ERR_PASS_RESTORE_FAIL_TO_SEND = 203;

	public const int ERR_WRONG_BACKEND_VERSION = 204;

	public const int ERR_BAD_ACCOUNT = 205;

	public const int ERR_WRONG_EMAIL_OR_PASS = 206;

	public const int ERR_WRONG_PARAMETERS = 207;

	public const int ERR_BAD_USER_REGION = 208;

	public const int ERR_RECEIVED_NEW_HARDWARE = 209;

	public const int ERR_GAME_VERSION_NOT_FOUND = 210;

	public const int ERR_WRONG_ACTIVATION_CODE = 211;

	public const int ERR_SESSION_EXPIRED = 213;

	public const int ERR_MAX_PROFILE_CREATED = 224;

	public const int ERR_NICKNAME_NOT_UNIQUE = 225;

	public const int ERR_NICKNAME_NOT_VALID = 226;

	public const int ERR_MOVEITEMS_BAD_REQUEST = 227;

	public const int ERR_MOVEITEMS_MOVE_ERROR = 228;

	public const int ERR_PROFILE_IS_BANNED = 229;

	public const int ERR_MAX_LOGIN_COUNT = 230;

	public const int ERR_WRONG_TAXONOMY_VERSION = 231;

	public const int ERR_WRONG_MAJOR_VERSION = 232;

	public const int ERR_NO_ACCESS_TO_SERVER = 233;

	public const int ERR_PHONE_NUMBER_ACTIVATION_REQUIRED = 240;

	public const int ERR_PHONE_VALIDATION_CODE_REQUIRED = 243;

	public const int ERR_PHONE_CAN_NOT_BE_USED = 245;

	public const int ERR_INVALID_PRONE_VALIDATION_CODE = 246;

	public const int ERR_AUTH_LOCKED_TEMP = 248;

	public const int ERR_AUTH_LOCKED_PERMANENT = 249;

	public const int ERR_PROFILE_IN_MATCH = 299;

	public const int NO_UPDATES = 300;

	public const int VERSION_OBSOLETE = 301;

	public const int Unauthorized = 401001;
}
