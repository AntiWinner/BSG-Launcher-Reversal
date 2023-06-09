using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.InformationCollectionService;

public class SmSystemFirmwareInfo
{
	[CompilerGenerated]
	private string _E000;

	public string Version
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
}
