using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eft.Launcher.Services.InformationCollectionService;

[JsonObject]
public class TracertEntry
{
	[CompilerGenerated]
	private int _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private long _E003;

	[CompilerGenerated]
	private IPStatus _E004;

	public int HopID
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

	public string Address
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

	public string Hostname
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

	public long ReplyTime
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

	[JsonConverter(typeof(StringEnumConverter))]
	public IPStatus ReplyStatus
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
		return string.Format(_E05B._E000(61166), HopID, string.IsNullOrEmpty(Hostname) ? Address : (Hostname + _E05B._E000(15316) + Address + _E05B._E000(15327)), (ReplyStatus == IPStatus.TimedOut) ? _E05B._E000(61122) : (ReplyTime + _E05B._E000(61182)));
	}
}
