using System;
using System.Windows.Shell;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.UpdateServices;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface IMainWindowDelegate : IWindowDelegate
{
	bool StartedFromLoginPage { get; }

	void SetGameUpdateState(GameUpdateServiceState newState);

	void SetGameUpdateProgress(long bytesDownloaded, long totalBytesToDownload);

	void SetGameUpdateVersion(BsgVersion version);

	void SetGameEdition(string edition);

	void SetGameVersion(BsgVersion version);

	void SetGameRegion(string region);

	void SetGameState(GameState newState);

	void SetTaskbarState(TaskbarItemProgressState taskbarState);

	void SetLauncherVersion(Version version);

	void CollapseToTray();

	void SetCurrentGameServers(string ipRegion, DatacenterDto[] datacenters);

	void SetQueueValues(int queuePosition, int estimatedTimeSec);

	void PlaySound(string soundFileName);

	void DrawMainButtonCountdown(double seconds, bool disableMainButton);
}
