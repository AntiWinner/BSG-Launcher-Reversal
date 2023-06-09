using Eft.Launcher.Services.BugReportService;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface IBugReportWindowDelegate : IWindowDelegate
{
	void ShowBugReportSendingProgress();

	void HideBugReportSendingProgress();

	void UpdateBugReportSendingProgress(BugReportSendingState state, int progress);
}
