using System;

namespace Eft.Launcher.Services.UpdateServices;

public class InstallationCompletedEventArgs : EventArgs
{
	public BsgVersion Version { get; }

	public InstallationResult InstallationResult { get; }

	public InstallationCompletedEventArgs(BsgVersion version, InstallationResult applyingResult)
	{
		Version = version;
		InstallationResult = applyingResult;
	}
}
