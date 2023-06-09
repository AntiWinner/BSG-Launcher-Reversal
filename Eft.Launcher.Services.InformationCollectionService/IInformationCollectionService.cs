using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.InformationCollectionService;

public interface IInformationCollectionService
{
	SystemInfo GetSystemInfo();

	string GetHwIdV1();

	Task<IReadOnlyCollection<ServerAvailabilityInfo>> GetServersAvailabilityInfo(Action<int> onProgress = null);
}
