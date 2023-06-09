using System;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelDownloaderFactoryException : MultichannelDownloadingException
{
	public MultichannelDownloaderFactoryException(string message)
		: base(message)
	{
	}

	public MultichannelDownloaderFactoryException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
