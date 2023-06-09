using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Eft.Launcher.Services.InformationCollectionService;

public class StorageInfo
{
	[CompilerGenerated]
	private uint _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private ulong _E003;

	[CompilerGenerated]
	private uint _E004;

	[CompilerGenerated]
	private string _E005;

	[CompilerGenerated]
	private bool _E006;

	[CompilerGenerated]
	private bool _E007;

	[JsonIgnore]
	public uint Index
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

	public string Model
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

	public string SerialNumber
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

	[JsonIgnore]
	public ulong Size
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

	public uint SizeGb
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

	public string MediaType
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

	public bool IsLauncherInstalled
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

	public bool IsSystemDrive
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

	public override string ToString()
	{
		return Model;
	}
}
