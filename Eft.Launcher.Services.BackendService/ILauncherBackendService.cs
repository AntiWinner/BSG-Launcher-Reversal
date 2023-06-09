using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bsg.Launcher.Services.BackendService;
using Eft.Launcher.Services.SettingsService;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.BackendService;

public interface ILauncherBackendService
{
	Task<LauncherPackageInfo> GetLauncherPackage();

	Task<GamePackageInfo> GetGamePackage(BsgVersion version = default(BsgVersion));

	Task<IReadOnlyCollection<GameUpdateInfo>> GetGameUpdates();

	[Obsolete("Use GetLauncherPackage()")]
	Task<LauncherDistribResponseData> GetLauncherDistrib();

	Task<UserProfile> GetUserProfile();

	Task SendAnalytics(JContainer json);
}
