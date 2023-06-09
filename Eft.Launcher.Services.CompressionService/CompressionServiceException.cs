using System;

namespace Eft.Launcher.Services.CompressionService;

public class CompressionServiceException : BsgException
{
	public CompressionServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public CompressionServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
