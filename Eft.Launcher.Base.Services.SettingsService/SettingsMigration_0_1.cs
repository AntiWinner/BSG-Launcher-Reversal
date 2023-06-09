using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.SettingsService;

internal class SettingsMigration_0_1 : ISettingsMigration
{
	public int FromVersion => 0;

	public void Migrate(JObject json)
	{
		JObject jObject = new JObject();
		string text = json[_E05B._E000(20888)]?.Value<string>()?.TrimStart('\\', '?');
		if (text != null)
		{
			jObject[_E05B._E000(20960)] = new JArray(new JObject
			{
				{
					_E05B._E000(20975),
					text
				},
				{
					_E05B._E000(20987),
					true
				},
				{
					_E05B._E000(26459),
					_E05B._E000(20821)
				}
			});
		}
		jObject[_E05B._E000(20928)] = json[_E05B._E000(20928)]?.Value<string>();
		jObject[_E05B._E000(20938)] = json[_E05B._E000(20938)]?.Value<string>();
		jObject[_E05B._E000(20945)] = json[_E05B._E000(20945)]?.Value<string>();
		jObject[_E05B._E000(20950)] = json[_E05B._E000(20950)]?.Value<DateTime>() ?? default(DateTime);
		jObject[_E05B._E000(20953)] = json[_E05B._E000(20953)]?.Value<string>();
		jObject[_E05B._E000(20958)] = json[_E05B._E000(20958)]?.Value<bool>() ?? false;
		jObject[_E05B._E000(20521)] = json[_E05B._E000(20521)]?.Value<bool>() ?? true;
		jObject[_E05B._E000(21759)] = json[_E05B._E000(21759)]?.Value<string>();
		jObject[_E05B._E000(20535)] = 1;
		json.RemoveAll();
		foreach (KeyValuePair<string, JToken> item in jObject)
		{
			json.Add(item.Key, item.Value);
		}
	}
}
