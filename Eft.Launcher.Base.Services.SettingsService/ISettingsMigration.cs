using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.SettingsService;

internal interface ISettingsMigration
{
	int FromVersion { get; }

	void Migrate(JObject json);
}
