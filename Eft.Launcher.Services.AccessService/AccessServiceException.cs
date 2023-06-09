using System;

namespace Eft.Launcher.Services.AccessService;

public class AccessServiceException : BsgException
{
	public AccessServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public AccessServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
