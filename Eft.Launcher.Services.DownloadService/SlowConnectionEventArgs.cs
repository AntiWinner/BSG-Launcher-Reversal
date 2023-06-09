using System;

namespace Eft.Launcher.Services.DownloadService;

public class SlowConnectionEventArgs
{
	public Uri Uri { get; }

	public bool ResetConnection { get; set; } = true;


	public SlowConnectionEventArgs(Uri uri)
	{
		Uri = uri;
	}
}
