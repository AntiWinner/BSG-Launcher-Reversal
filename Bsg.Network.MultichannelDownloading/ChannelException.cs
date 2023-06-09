using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelException : MultichannelDownloadingException
{
	public IChannel Channel { get; }

	public ChannelException(IChannel channel, string message)
		: this(channel, message, null)
	{
	}

	public ChannelException(IChannel channel, string message, Exception innerException)
		: base(message, innerException)
	{
		Channel = channel;
	}
}
