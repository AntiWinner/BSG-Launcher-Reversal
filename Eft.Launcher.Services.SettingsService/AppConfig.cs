using System;

namespace Eft.Launcher.Services.SettingsService;

public class AppConfig
{
	public const string ProductionConfigurationName = "Production";

	public static AppConfig Instance { get; } = new AppConfig();


	public string Configuration { get; } = _E05B._E000(60951);


	public string AppId { get; } = _E05B._E000(60956);


	public string SubfolderName => AppShortName + ((Configuration == _E05B._E000(60951)) ? "" : (_E05B._E000(20912) + Configuration + _E05B._E000(24948)));

	public string AppPublisher { get; } = _E05B._E000(61001);


	public string AppShortName { get; } = _E05B._E000(26103);


	public string AppFullName { get; } = _E05B._E000(61023);


	public string GameShortName { get; } = _E05B._E000(61108);


	public string GameFullName { get; } = _E05B._E000(61067);


	public bool LongPathHandlingEnabled { get; }

	public string SettingsFileName { get; } = _E05B._E000(61080);


	private AppConfig()
	{
		if (Environment.OSVersion.Version.Major <= 6)
		{
			LongPathHandlingEnabled = true;
		}
	}
}
