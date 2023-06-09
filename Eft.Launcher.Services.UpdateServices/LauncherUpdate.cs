using System;

namespace Eft.Launcher.Services.UpdateServices;

public class LauncherUpdate
{
	public bool UpdateIsRequired { get; set; }

	public Version Version { get; set; }

	public string Hash { get; set; }

	public Uri DownloadUri { get; set; }
}
