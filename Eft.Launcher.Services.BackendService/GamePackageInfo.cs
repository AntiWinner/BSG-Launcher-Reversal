using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.BackendService;

public class GamePackageInfo
{
	[CompilerGenerated]
	private BsgVersion _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private string _E003;

	[CompilerGenerated]
	private long _E004 = -1L;

	public BsgVersion Version
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

	public string DownloadUri
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

	public string UnpackedUri
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

	public string Hash
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public long RequiredFreeSpace
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		set
		{
			_E004 = value;
		}
	}
}
