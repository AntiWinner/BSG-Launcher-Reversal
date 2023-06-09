namespace Eft.Launcher.Gui.Wpf.Views;

public interface IFeedbackWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }
}
