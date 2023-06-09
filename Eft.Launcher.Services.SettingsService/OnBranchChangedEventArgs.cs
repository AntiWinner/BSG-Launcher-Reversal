namespace Eft.Launcher.Services.SettingsService;

public class OnBranchChangedEventArgs
{
	public IBranch OldBranch { get; }

	public IBranch NewBranch { get; }

	public OnBranchChangedEventArgs(IBranch oldBranch, IBranch newBranch)
	{
		OldBranch = oldBranch;
		NewBranch = newBranch;
	}
}
