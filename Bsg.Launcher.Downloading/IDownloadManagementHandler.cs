using System;

namespace Bsg.Launcher.Downloading;

public interface IDownloadManagementHandler
{
	void OnDownloadError(Exception exception, ref bool retry);
}
