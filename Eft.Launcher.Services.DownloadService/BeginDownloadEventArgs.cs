using System;

namespace Eft.Launcher.Services.DownloadService;

public class BeginDownloadEventArgs
{
	public Uri DownloadUri { get; }

	public string DownloadLocation { get; }

	public DownloadCategory DownloadCategory { get; }

	public bool IsResumeDownload { get; }

	public BeginDownloadEventArgs(Uri downloadUri, string downloadLocation, DownloadCategory downloadCategory, bool isResumeDownload)
	{
		DownloadUri = downloadUri;
		DownloadLocation = downloadLocation;
		DownloadCategory = downloadCategory;
		IsResumeDownload = isResumeDownload;
	}
}
