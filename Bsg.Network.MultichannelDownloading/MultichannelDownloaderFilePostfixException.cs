using System;

namespace Bsg.Network.MultichannelDownloading;

internal class MultichannelDownloaderFilePostfixException : MultichannelDownloadingException
{
	public MultichannelDownloaderFilePostfixException(string message)
		: base(message)
	{
	}

	public MultichannelDownloaderFilePostfixException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
