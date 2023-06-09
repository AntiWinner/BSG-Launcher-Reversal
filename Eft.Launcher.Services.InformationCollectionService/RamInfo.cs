using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.InformationCollectionService;

public class RamInfo
{
	[CompilerGenerated]
	private uint _E000;

	[CompilerGenerated]
	private IEnumerable<RamModuleInfo> _E001;

	public uint TotalSizeGb
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

	public IEnumerable<RamModuleInfo> Modules
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
