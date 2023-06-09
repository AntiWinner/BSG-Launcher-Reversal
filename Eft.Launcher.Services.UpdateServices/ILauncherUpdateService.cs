using System;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.UpdateServices;

public interface ILauncherUpdateService
{
	ILauncherMetadata Metadata { get; }

	LauncherUpdateServiceState State { get; }

	event EventHandler<LauncherUpdateServiceState> OnStateChanged;

	event EventHandler<IProgressReport> OnProgress;

	event Action<Version, InstallationResult> OnLauncherInstallationCompleted;

	void CheckForStartAfterUpdate();

	Task<LauncherUpdate> CheckForLauncherUpdate();

	Task BeginUpdateLauncher(LauncherUpdate launcherUpdate);

	void EndUpdateLauncher(LauncherUpdate launcherUpdate);
}
