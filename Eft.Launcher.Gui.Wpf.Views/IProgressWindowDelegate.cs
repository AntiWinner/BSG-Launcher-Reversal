namespace Eft.Launcher.Gui.Wpf.Views;

public interface IProgressWindowDelegate : IWindowDelegate
{
	void SetMessage(ProgressWindowMessage message);

	void SetProgress(int progress);
}
