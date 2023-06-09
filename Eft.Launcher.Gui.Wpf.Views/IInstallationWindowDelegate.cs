using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface IInstallationWindowDelegate : IWindowDelegate
{
	GamePackageInfo DistribResponseData { get; set; }

	string InstallationPath { get; set; }

	bool? DialogResult { get; set; }

	void UpdateInstallationInfo(long requiredFreeSpace, long availableSpace);
}
