namespace Eft.Launcher.Services.UpdateServices;

public class UpdateCompletedEventArgs : InstallationCompletedEventArgs
{
	public BsgVersion FromVersion { get; }

	public UpdateCompletedEventArgs(BsgVersion fromVersion, BsgVersion version, InstallationResult applyingResult)
		: base(version, applyingResult)
	{
		FromVersion = fromVersion;
	}
}
