using System;

namespace Eft.Launcher.Services.ConsistencyService;

public class ConsistencyServiceException : BsgException
{
	public ConsistencyServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public ConsistencyServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
