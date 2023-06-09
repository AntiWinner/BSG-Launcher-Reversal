using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelLeaseRevokedException : ChannelException
{
	public ChannelLeaseRevokedException(IChannel channel, string message)
		: this(channel, message, null)
	{
	}

	public ChannelLeaseRevokedException(IChannel channel, string message, Exception innerException)
		: base(channel, message, innerException)
	{
	}
}
