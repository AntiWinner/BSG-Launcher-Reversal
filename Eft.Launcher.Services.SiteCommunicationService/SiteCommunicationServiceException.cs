using System;

namespace Eft.Launcher.Services.SiteCommunicationService;

public class SiteCommunicationServiceException : BsgException
{
	public SiteCommunicationServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public SiteCommunicationServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
