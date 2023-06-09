namespace Eft.Launcher.Services.UpdateServices;

public enum LauncherUpdateServiceState
{
	Idle,
	CheckingForLauncherUpdate,
	DownloadingLauncherUpdate,
	ApplyingLauncherUpdate,
	LauncherUpdateError
}
