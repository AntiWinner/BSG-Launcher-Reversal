using System;

namespace Eft.Launcher.Services.UpdateServices;

public class UpdateServiceException : BsgException
{
	public UpdateServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public UpdateServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
