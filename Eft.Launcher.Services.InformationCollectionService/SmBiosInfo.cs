using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.InformationCollectionService;

public class SmBiosInfo
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	public string Vendor
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

	public string Version
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

	public string ReleaseDate
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}
}
