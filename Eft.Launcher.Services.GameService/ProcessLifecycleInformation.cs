using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.GameService;

public class ProcessLifecycleInformation
{
	[CompilerGenerated]
	private int _E000;

	[CompilerGenerated]
	private BsgVersion _E001;

	public int ExitCode
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	public BsgVersion AppVersion
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}
}
