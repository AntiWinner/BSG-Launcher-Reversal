using Newtonsoft.Json;

namespace Eft.Launcher.Services.SettingsService;

public class PlayerProfile
{
	[JsonProperty(PropertyName = "profileLevel")]
	public string ProfileLevel { get; private set; }

	[JsonProperty(PropertyName = "totalInGame")]
	public int TotalTimeInGameSec { get; set; }
}
