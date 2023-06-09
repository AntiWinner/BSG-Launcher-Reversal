using System;
using System.Threading.Tasks;
using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Services.UpdateServices;

public interface IGameUpdateService
{
	GameUpdateServiceState State { get; }

	event EventHandler<GameUpdateServiceState> OnStateChanged;

	event Action<long, long> OnProgress;

	event EventHandler<GameUpdateSet> OnUpdateChecked;

	event EventHandler<UpdateCompletedEventArgs> OnUpdateCompleted;

	event EventHandler<InstallationCompletedEventArgs> OnInstallationCompleted;

	event Action<BsgVersion> OnInstallationStarted;

	Task<GameUpdateSet> CheckForGameUpdate(bool fastChecking = false);

	Task<InstallationResult> DownloadAndInstallGame(string installationPath, GamePackageInfo distribResponseData);

	Task<InstallationResult> DownloadAndApplyGameUpdateSet(GameUpdateSet gameUpdateSet);

	void CreateUpdate(BsgVersion fromVersion, BsgVersion toVersion, string oldDir, string newDir, string updatePath);

	void PauseInstallation();

	Task ResumeInstallation();

	Task StopInstallation(bool showCancellationDialog);

	Task<bool> TryResumeLastGameUpdateSetInstallation();

	Task<bool> TryResumeLastGameInstallation();
}
