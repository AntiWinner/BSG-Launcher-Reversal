using System;

namespace Eft.Launcher.Services.InformationCollectionService;

public class InformationCollectionServiceException : BsgException
{
	public InformationCollectionServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public InformationCollectionServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
