using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelProviderNoAvailableChannelsException : ChannelProviderException
{
	public ChannelProviderNoAvailableChannelsException(string message)
		: base(message)
	{
	}

	public ChannelProviderNoAvailableChannelsException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
