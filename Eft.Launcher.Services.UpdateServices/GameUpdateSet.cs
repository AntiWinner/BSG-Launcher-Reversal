using System.Collections.Generic;
using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Services.UpdateServices;

public class GameUpdateSet
{
	public UpdateNecessity UpdateNecessity { get; set; }

	public BsgVersion NewVersion { get; set; }

	public IReadOnlyList<GameUpdateInfo> Updates { get; set; }
}
