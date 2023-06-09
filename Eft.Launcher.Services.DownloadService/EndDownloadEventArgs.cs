using System;

namespace Eft.Launcher.Services.DownloadService;

public class EndDownloadEventArgs
{
	public Uri DownloadUri { get; }

	public string DownloadLocation { get; }

	public DownloadCategory DownloadCategory { get; }

	public bool IsDownloadWasResumed { get; }

	public int AverageDownloadSpeedMbitSec { get; }

	public DownloadState DownloadState { get; }

	public EndDownloadEventArgs(Uri downloadUri, string downloadLocation, DownloadCategory downloadCategory, bool isDownloadWasResumed, int averageDownloadSpeedMbitSec, DownloadState downloadState)
	{
		DownloadUri = downloadUri;
		DownloadLocation = downloadLocation;
		DownloadCategory = downloadCategory;
		IsDownloadWasResumed = isDownloadWasResumed;
		AverageDownloadSpeedMbitSec = averageDownloadSpeedMbitSec;
		DownloadState = downloadState;
	}
}
