using System;

namespace Bsg.Network.MultichannelDownloading;

public interface ILeasedChannel : IChannel, IDisposable
{
	void Dispose(ChannelRevokationReason reason);
}
