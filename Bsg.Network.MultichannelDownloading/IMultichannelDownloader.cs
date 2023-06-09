using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bsg.Network.MultichannelDownloading;

public interface IMultichannelDownloader : IDisposable
{
	int Id { get; }

	IChannelMonitor Monitor { get; }

	event Action<IMultichannelDownloader> OnDisposing;

	Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken = default(CancellationToken));

	Task DownloadAsync(string relativeUri, Stream destinationStream, Action<long, long> onProgress = null, bool tryUseMetadata = false, CancellationToken cancellationToken = default(CancellationToken));

	void ResetErrors();
}
