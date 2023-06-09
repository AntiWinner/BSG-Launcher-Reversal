using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bsg.Network.MultichannelDownloading;

public interface IChannelProvider : IDisposable
{
	int ChannelsCount { get; }

	int LeasedChannelsCount { get; }

	event Action OnChannelForLeaseAppeared;

	ILeasedChannel WaitForLeaseChannel(CancellationToken cancellationToken, string relativeUri, object tenant, bool isImportant = false);

	Task<ILeasedChannel> WaitForLeaseChannelAsync(CancellationToken cancellationToken, string relativeUri, object tenant, bool isImportant = false);

	bool TryLeaseChannel(out ILeasedChannel leasedChannel, string relativeUri, object tenant, bool isImportant = false);

	void ResetErrors();
}
