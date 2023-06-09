using System.Collections.Generic;
using Eft.Launcher.Services.BackendService;
using Newtonsoft.Json;

namespace Eft.Launcher.Base.Services.BackendService;

[JsonObject(MemberSerialization.OptIn)]
[JsonConverter(typeof(_E019))]
internal class ListOfServersResponseData : IListOfServersResponseData
{
	public ICollection<IServerData> Servers { get; } = new List<IServerData>();


	[JsonConstructor]
	public ListOfServersResponseData()
	{
	}

	public override string ToString()
	{
		return JsonConvert.SerializeObject(Servers);
	}
}
