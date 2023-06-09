using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.UpdateServices;

namespace Eft.Launcher.Services.DownloadService;

[Obsolete]
public interface IDownloadService
{
	event EventHandler<BeginDownloadEventArgs> OnBeginDownload;

	event EventHandler<EndDownloadEventArgs> OnEndDownload;

	event Action<SlowConnectionEventArgs> OnSlowConnectionDetected;

	Task<string> DownloadFile(Uri fileUri, string fileName, string hash, DownloadCategory downloadCategory, CancellationToken cancellationToken, Action<IProgressReport> onProgress = null, Action<IProgressReport> onCheckingHashProgress = null);

	Task<long> GetFileSize(Uri fileUri);

	void Cleanup();

	void Cleanup(GamePackageInfo distribResponseData);

	void Cleanup(GameUpdateSet gameUpdateSet);

	void CleanFileFragments();

	IEnumerable<string> GetFileFragments(DownloadCategory downloadCategory);

	IEnumerable<string> GetFiles(DownloadCategory downloadCategory);
}
