using System.Net;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.BackendService;

public class DatacenterDto
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private bool _E002;

	[CompilerGenerated]
	private int _E003;

	[CompilerGenerated]
	private int _E004;

	[CompilerGenerated]
	private IPAddress[] _E005;

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

	public string Region
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

	public bool IsSelected
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

	public int MaxPingTime
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

	public int AvgWaitTime
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

	public IPAddress[] IpAddresses
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
}
