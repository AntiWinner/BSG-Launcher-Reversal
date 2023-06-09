using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelProviderException : MultichannelDownloaderException
{
	public ChannelProviderException(string message)
		: base(message)
	{
	}

	public ChannelProviderException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
