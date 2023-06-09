using System.Collections.Generic;

namespace Eft.Launcher.Services.BackendService;

public interface IListOfServersResponseData
{
	ICollection<IServerData> Servers { get; }
}
