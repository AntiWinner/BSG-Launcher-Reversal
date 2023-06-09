using System;

namespace Eft.Launcher.Services.DownloadService;

public class DownloadServiceException : BsgException
{
	public DownloadServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public DownloadServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
