using System.Threading.Tasks;
using Bsg.Launcher.Models;
using Eft.Launcher.Services.SettingsService;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.BackendService;

public interface IGameBackendService
{
	Task<IListOfServersResponseData> GetListOfServers();

	Task<DatacenterDto[]> GetDatacenters();

	Task SetMatchingConfiguration(string matchingConfiguration);

	Task<PlayerProfile> GetPlayerProfile();

	Task<GameSessionInfo> GetGameSession(BsgVersion gameVersion, string branchName);

	Task CancelQueueWaiting();

	Task<JToken> ActivateCode(string activationCode);
}
