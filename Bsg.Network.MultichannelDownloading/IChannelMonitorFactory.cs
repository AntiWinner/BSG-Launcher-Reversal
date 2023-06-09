using System.Collections.Generic;

namespace Bsg.Network.MultichannelDownloading;

public interface IChannelMonitorFactory
{
	IChannelMonitor Create(IReadOnlyCollection<IChannel> channels, MultichannelDownloaderContext context);
}
