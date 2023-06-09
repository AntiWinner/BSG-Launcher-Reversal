namespace Eft.Launcher.Gui.Wpf.Views;

public interface ICaptchaWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }
}
