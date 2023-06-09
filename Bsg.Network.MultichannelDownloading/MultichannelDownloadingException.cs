using System;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelDownloadingException : Exception
{
	public MultichannelDownloadingException(string message)
		: base(message)
	{
	}

	public MultichannelDownloadingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
