namespace Eft.Launcher.Gui.Wpf.Views;

public interface IRequestRestartDialogWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }
}
