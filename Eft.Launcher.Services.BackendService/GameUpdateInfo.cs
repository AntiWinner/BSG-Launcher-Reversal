using System;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.BackendService;

public class GameUpdateInfo : IComparable<GameUpdateInfo>
{
	[CompilerGenerated]
	private BsgVersion _E000;

	[CompilerGenerated]
	private BsgVersion _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private string _E003;

	[CompilerGenerated]
	private bool _E004;

	[CompilerGenerated]
	private bool _E005;

	[CompilerGenerated]
	private bool _E006;

	[CompilerGenerated]
	private bool _E007;

	public BsgVersion FromVersion
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

	public BsgVersion Version
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

	public string Hash
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

	public string DownloadUri
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

	public bool FullConsistencyCheck
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

	public bool ClearCache
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		set
		{
			_E005 = value;
		}
	}

	public bool DeleteLocalIni
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		set
		{
			_E006 = value;
		}
	}

	public bool DeleteSharedIni
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		set
		{
			_E007 = value;
		}
	}

	public int CompareTo(GameUpdateInfo other)
	{
		int num = FromVersion.CompareTo(other.FromVersion);
		if (num != 0)
		{
			return num;
		}
		return Version.CompareTo(other.Version);
	}
}
