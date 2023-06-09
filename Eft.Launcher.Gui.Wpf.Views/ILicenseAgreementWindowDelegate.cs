namespace Eft.Launcher.Gui.Wpf.Views;

public interface ILicenseAgreementWindowDelegate : IWindowDelegate
{
	string Document { get; }

	bool? DialogResult { get; set; }
}
