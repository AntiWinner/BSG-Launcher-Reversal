using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Eft.Launcher.Services.InformationCollectionService;

[JsonObject(MemberSerialization.OptOut)]
public class ServerAvailabilityInfo
{
	[CompilerGenerated]
	private readonly string _E000;

	[CompilerGenerated]
	private readonly int _E001;

	[CompilerGenerated]
	private readonly TracertEntry[] _E002;

	[JsonProperty(PropertyName = "Address")]
	public string HostNameOrIpAddress
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public int Ping
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public TracertEntry[] TraceRoute
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public ServerAvailabilityInfo(string hostNameOrIpAddress, int ping, TracertEntry[] traceRoute)
	{
		_E000 = hostNameOrIpAddress;
		_E001 = ping;
		_E002 = traceRoute;
	}

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
