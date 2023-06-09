using System;

namespace Eft.Launcher.Services.SettingsService;

public class SettingsServiceException : BsgException
{
	public SettingsServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public SettingsServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
