using System;

namespace Eft.Launcher.Services.BackendService;

public class BackendServiceException : BsgException
{
	public BackendServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public BackendServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
