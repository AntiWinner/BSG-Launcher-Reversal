using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services;
using Eft.Launcher.Services.BackendService;

namespace Bsg.Launcher.Downloading;

public interface IDownloadManagementService : IService
{
	Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken);

	Task<string> DownloadTheGamePackageAsync(GamePackageInfo gameInstallationInfo, Action<long, long> onProgress, CancellationToken cancellationToken);

	Task<string[]> DownloadTheGameUpdatesAsync(IReadOnlyCollection<GameUpdateInfo> updates, Action<long, long> onProgress, CancellationToken cancellationToken);

	Task<string[]> DownloadFilesAsync(IReadOnlyList<(string relativeUri, string destinationPath)> downloadSpecification, Action<long, long> onProgress, bool tryUseMetadata = false, bool redownloadIfExist = true, CancellationToken cancellationToken = default(CancellationToken));

	Task DownloadFileAsync(string relativeUri, string destinationPath, Action<long, long> onProgress, bool tryUseMetadata = false, bool redownloadIfExist = true, CancellationToken cancellationToken = default(CancellationToken));
}
