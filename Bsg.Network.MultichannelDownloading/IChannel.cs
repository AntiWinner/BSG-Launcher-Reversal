using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bsg.Network.MultichannelDownloading;

public interface IChannel : IDisposable
{
	bool IsSpare { get; }

	bool IsBusy { get; }

	Uri Endpoint { get; }

	event Action OnDataReadingStarted;

	event Action<int> OnDataRead;

	event Action OnDataReadingCompleted;

	bool CanDownloadFile(string relativeUri);

	Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken);

	void FillChunk(string relativeUri, IChunk chunk, CancellationToken cancellationToken);

	void DownloadFile(string relativeUri, Stream destinationStream, CancellationToken cancellationToken);

	void ResetErrors();

	void AddError(Exception exception);
}
