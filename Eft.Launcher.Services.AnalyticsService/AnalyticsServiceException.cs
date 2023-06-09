using System;

namespace Eft.Launcher.Services.AnalyticsService;

public class AnalyticsServiceException : BsgException
{
	public AnalyticsServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public AnalyticsServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
