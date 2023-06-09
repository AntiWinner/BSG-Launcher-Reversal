namespace Eft.Launcher.Services.UpdateServices;

public enum InstallationResult
{
	Unknown,
	Succeded,
	HasSkippedFiles,
	Stopped,
	Paused,
	DownloadError,
	ConsistencyError,
	InstallationError
}
