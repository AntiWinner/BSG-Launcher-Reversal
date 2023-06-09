using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Eft.Launcher.Services.InformationCollectionService;

public class NicInfo
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private bool _E002;

	[CompilerGenerated]
	private bool _E003;

	[CompilerGenerated]
	private uint _E004;

	public string Name
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

	public string MacAddress
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

	public bool IsCurrent
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
	public bool PhysicalAdapter
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

	[JsonIgnore]
	public uint InterfaceIndex
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

	public override string ToString()
	{
		return Name;
	}
}
