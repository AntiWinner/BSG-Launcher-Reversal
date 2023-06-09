namespace Eft.Launcher.Gui.Wpf.Views;

public interface IDialogWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }
}
