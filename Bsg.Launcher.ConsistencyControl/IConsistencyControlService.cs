using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Bsg.Launcher.ConsistencyControl;

public interface IConsistencyControlService
{
	void EnsureConsistencyByHash(IReadOnlyCollection<(string filePath, string hash)> data, Action<long, long> onProgress, CancellationToken cancellationToken);

	void EnsureConsistencyByHash(IReadOnlyCollection<(Stream stream, string hash)> data, Action<long, long> onProgress, CancellationToken cancellationToken);

	void EnsureConsistencyByHash(string filePath, string hash, Action<long, long> onProgress, CancellationToken cancellationToken);

	void EnsureConsistencyByHash(Stream stream, string hash, Action<long, long> onProgress, CancellationToken cancellationToken);

	bool CheckHash(Stream stream, string hash, Action<long, long> onProgress, CancellationToken cancellationToken);

	byte[] GetHash(Stream stream, Action<long, long> onProgress, CancellationToken cancellationToken);
}
