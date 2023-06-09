using System;

namespace Eft.Launcher.Services.BackendService;

[Obsolete("Use LauncherPackageInfo")]
public class LauncherDistribResponseData
{
	public Version Version { get; set; }

	public string Hash { get; set; }

	public Uri DownloadUri { get; set; }

	public long RequiredFreeSpace { get; set; }
}
