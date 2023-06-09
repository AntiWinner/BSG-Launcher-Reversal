using System.Collections.Generic;

namespace Bsg.Network.MultichannelDownloading;

public interface IChannelProviderFactory
{
	IChannelProvider Create(IChannelMonitor channelQualityMonitor, IReadOnlyCollection<IChannel> channels, MultichannelDownloaderContext context);
}
