using System;

namespace Eft.Launcher.Services.BugReportService;

public class BugReportServiceException : BsgException
{
	public BugReportServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public BugReportServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
