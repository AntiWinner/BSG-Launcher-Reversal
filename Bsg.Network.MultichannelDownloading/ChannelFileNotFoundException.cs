using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelFileNotFoundException : ChannelException
{
	public string File { get; }

	public ChannelFileNotFoundException(IChannel channel, string file)
		: this(channel, file, null)
	{
	}

	public ChannelFileNotFoundException(IChannel channel, string file, Exception innerException)
		: base(channel, $"{channel}{file} is not found", innerException)
	{
		File = file;
	}
}
