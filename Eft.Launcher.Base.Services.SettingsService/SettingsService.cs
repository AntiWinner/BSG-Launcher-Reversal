using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Bsg.Launcher.Json.Converters;
using Bsg.Network.MultichannelDownloading;
using Eft.Launcher.Services;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.SettingsService;

[JsonObject(MemberSerialization.OptIn)]
public class SettingsService : ISettingsService, IService
{
	private const int SettingsVersion = 1;

	private readonly ISettingsMigration[] _migrations = new ISettingsMigration[1]
	{
		new SettingsMigration_0_1()
	};

	private readonly ILogger _logger;

	private readonly AppConfig _appConfig;

	private readonly ILoggerFactory _loggerFactory;

	private readonly IAccessService _accessService;

	private readonly MultichannelDownloaderOptions _multichannelDownloaderOptions;

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "tempFolder")]
	private string _launcherTempDir;

	private bool _keepLoggedIn;

	private bool _saveLogin;

	public bool IsBackendSettingsLoaded { get; private set; }

	public bool IsFirstStart { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "configuration")]
	public string Configuration => _appConfig.Configuration;

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "sysInfCheck")]
	public string SystemInfoChecksum { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "branches")]
	public ICollection<IBranch> Branches { get; set; }

	public IBranch SelectedBranch
	{
		get
		{
			return Branches.FirstOrDefault((IBranch b) => b.IsSelected) ?? throw new Exception(_E05B._E000(20487));
		}
		set
		{
			IBranch branch = Branches.FirstOrDefault((IBranch b) => b.IsSelected);
			if (branch != value)
			{
				Branches.ForEach(delegate(IBranch b)
				{
					b.IsSelected = b == value;
				});
				this.OnBranchChanged?.Invoke(this, new OnBranchChangedEventArgs(branch, value));
				RaiseOnSettingsUpdated();
			}
		}
	}

	public string AppDataDir { get; set; }

	public string LocalAppDataDir { get; set; }

	public string ProgramDataDir { get; set; }

	public string LauncherTempDir
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_launcherTempDir) || !_appConfig.LongPathHandlingEnabled)
			{
				return _launcherTempDir;
			}
			return _E05B._E000(20503) + _launcherTempDir;
		}
		set
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (value.StartsWith(_E05B._E000(20503)))
				{
					value = value.Substring(4).NormalizePath();
				}
				if (!(_launcherTempDir == value))
				{
					_launcherTempDir = value;
					Directory.CreateDirectory(LauncherTempDir);
					RaiseOnSettingsUpdated();
				}
			}
		}
	}

	public string P2pDir => Path.Combine(LauncherTempDir, _E05B._E000(20506));

	public Uri LauncherBackendUri { get; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "authCenterUri")]
	public Uri AuthCenterUri { get; }

	public Uri GuiUri { get; set; }

	public Uri MainPageUri { get; set; }

	public Uri LoginPageUri { get; set; }

	public Uri RequestRestartPageUri { get; set; }

	public Uri DialogPageUri { get; set; }

	public Uri ErrorPageUri { get; set; }

	public Uri ProgressBarPageUri { get; set; }

	public Uri SelectLanguagePageUri { get; set; }

	public Uri BugReportPageUri { get; set; }

	public Uri InstallationPageUri { get; set; }

	public Uri LicenseAgreementPageUri { get; set; }

	public Uri CodeActivationPageUri { get; set; }

	public Uri MatchingConfigurationPageUri { get; set; }

	public Uri FeedbackPageUri { get; set; }

	public Uri CaptchaPageUri { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "aid")]
	public string AccountId { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "nickname")]
	public string Nickname { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "login")]
	public string LoginOrEmail { get; set; }

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "at")]
	[JsonConverter(typeof(_E014))]
	public string AccessToken { get; set; }

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "atet")]
	public DateTime AccessTokenExpirationTimeUtc { get; set; }

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "rt")]
	[JsonConverter(typeof(_E014))]
	public string RefreshToken { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "keepLoggedIn")]
	public bool KeepLoggedIn
	{
		get
		{
			return _keepLoggedIn;
		}
		set
		{
			if (value)
			{
				_saveLogin = true;
			}
			_keepLoggedIn = value;
		}
	}

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "saveLogin")]
	public bool SaveLogin
	{
		get
		{
			return _saveLogin;
		}
		set
		{
			if (!value)
			{
				_keepLoggedIn = false;
			}
			_saveLogin = value;
		}
	}

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "loginLanguage")]
	public string LoginLanguage { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "userRegion")]
	public string UserRegion { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "ipRegion")]
	public string IpRegion { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "gameEdition")]
	public string GameEdition { get; set; }

	public bool IsGameBought => GameEdition != null;

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "purchaseDate")]
	public string PurchaseDate { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "profileLevel")]
	public string ProfileLevel { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "totalInGame")]
	public int TotalTimeInGameSec { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "userAvatar")]
	public string AvatarUrl { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "supportUnreadNotifications")]
	public int? SupportUnreadNotifications { get; set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "sitePushStreamChannels")]
	public JArray SitePushStreamChannels { get; set; }

	public IReadOnlyCollection<ChannelSettings> ChannelSettings { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "geoInfo")]
	public JToken GeoInfo { get; private set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "language")]
	public string Language { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "launchOnStartup")]
	public bool LaunchOnStartup { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "launchMinimized")]
	public bool LaunchMinimized { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "closeBehavior")]
	public CloseBehavior CloseBehavior { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "gameStartBehavior")]
	public GameStartBehavior GameStartBehavior { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "enableGameAutoUpdate")]
	public bool GameAutoUpdateEnabled { get; set; }

	public int UpdateCheckInterval { get; set; }

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "gameProcessId")]
	public int GameProcessId { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "maxDownloadSpeed")]
	public uint MaxDownloadSpeedKb { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "maxUploadSpeed")]
	public uint MaxUploadSpeedKb { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "queueNotifyWithSound")]
	public bool SoundQueueNotificationEnabled { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "queueAutoLogIn")]
	public bool QueueAutostartEnabled { get; set; } = true;


	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "volumeValue")]
	public int VolumeValue { get; set; } = 80;


	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "maxBugReportSize")]
	public int MaxBugReportSize { get; set; } = 15728640;


	public int MaxCrashReportSize { get; set; } = 15728640;


	public int PingTimeout { get; set; } = 10000;


	public int TracertTimeout { get; set; } = 3;


	public int TracertMaxHops { get; set; } = 30;


	public int MaxUncompressedLogPackedSizeMb { get; set; } = 50;


	[Tag(new string[] { "ForSave" })]
	[JsonConverter(typeof(IsoDateTimeConverter))]
	[JsonProperty(PropertyName = "lastSendingLogsTime")]
	public DateTime LastSendingLogsTime { get; set; }

	[Tag(new string[] { "ForSave" })]
	[JsonProperty(PropertyName = "isInTheGameInstallationProgress")]
	public bool IsInTheGameInstallationProgress { get; set; }

	public event EventHandler OnUserProfileLoaded;

	public event EventHandler OnSettingsUpdated;

	public event EventHandler<OnBranchChangedEventArgs> OnBranchChanged;

	public event EventHandler<IBranch> OnBranchObsolete;

	public SettingsService(AppConfig appConfig, ILoggerFactory loggerFactory, IAccessService accessService, DIContractResolver dIContractResolver, JsonSerializerSettings jsonSerializerSettings, MultichannelDownloaderOptions multichannelDownloaderOptions)
	{
		_logger = loggerFactory.CreateLogger<SettingsService>();
		_appConfig = appConfig;
		_loggerFactory = loggerFactory;
		_accessService = accessService;
		_multichannelDownloaderOptions = multichannelDownloaderOptions;
		JsonSerializerSettings jsonContractResolverSerializerSettings = new JsonSerializerSettings
		{
			Converters = jsonSerializerSettings.Converters,
			ContractResolver = dIContractResolver,
			Formatting = jsonSerializerSettings.Formatting
		};
		JsonConvert.DefaultSettings = () => jsonContractResolverSerializerSettings;
		AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appConfig.AppPublisher, _appConfig.SubfolderName);
		Directory.CreateDirectory(AppDataDir);
		LocalAppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appConfig.AppPublisher, _appConfig.SubfolderName);
		Directory.CreateDirectory(LocalAppDataDir);
		ProgramDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _appConfig.AppPublisher, _appConfig.SubfolderName);
		Directory.CreateDirectory(ProgramDataDir);
		GuiUri = new Uri(_E05B._E000(20510), UriKind.Absolute);
		Branches = new List<IBranch>();
		LauncherBackendUri = new Uri(_E05B._E000(20596));
		AuthCenterUri = new Uri(_E05B._E000(20574));
		MainPageUri = new Uri(GuiUri, _E05B._E000(20669));
		LoginPageUri = new Uri(GuiUri, _E05B._E000(20618));
		RequestRestartPageUri = new Uri(GuiUri, _E05B._E000(20631));
		DialogPageUri = new Uri(GuiUri, _E05B._E000(20714));
		ErrorPageUri = new Uri(GuiUri, _E05B._E000(20726));
		ProgressBarPageUri = new Uri(GuiUri, _E05B._E000(20675));
		SelectLanguagePageUri = new Uri(GuiUri, _E05B._E000(20689));
		BugReportPageUri = new Uri(GuiUri, _E05B._E000(24362));
		InstallationPageUri = new Uri(GuiUri, _E05B._E000(24379));
		MatchingConfigurationPageUri = new Uri(GuiUri, _E05B._E000(24329));
		LicenseAgreementPageUri = new Uri(GuiUri, _E05B._E000(24348));
		CodeActivationPageUri = new Uri(GuiUri, _E05B._E000(24446));
		FeedbackPageUri = new Uri(GuiUri, _E05B._E000(24403));
		CaptchaPageUri = new Uri(GuiUri, _E05B._E000(24413));
		AccountId = _E05B._E000(25616);
		LoginLanguage = _E05B._E000(24499);
		UserRegion = _E05B._E000(24450);
		Language = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
		CloseBehavior = CloseBehavior.CloseLauncher;
		GameStartBehavior = GameStartBehavior.CollapseLauncherToTray;
		SaveLogin = true;
		UpdateCheckInterval = 1800;
		GameProcessId = -1;
		LastSendingLogsTime = DateTime.Now;
	}

	public void OnAwake()
	{
	}

	public void OnStop()
	{
		if (!KeepLoggedIn)
		{
			ResetTokenData();
		}
		Save();
	}

	public void Save()
	{
		try
		{
			_logger.LogDebug(_E05B._E000(24467));
			JsonSerializer jsonSerializer = new JsonSerializer();
			jsonSerializer.ContractResolver = new TaggedJsonContractResolver(_E05B._E000(24544));
			JsonSerializer jsonSerializer2 = jsonSerializer;
			JObject jObject = JObject.FromObject(this, jsonSerializer2);
			jObject[_E05B._E000(20535)] = 1;
			string contents = jObject.ToString(Formatting.None);
			Directory.CreateDirectory(AppDataDir);
			File.WriteAllText(Path.Combine(AppDataDir, _appConfig.SettingsFileName), contents);
			_logger.LogInformation(_E05B._E000(24552));
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(24516));
		}
	}

	public void ResetAccountSettings()
	{
		ResetTokenData();
		LoginOrEmail = null;
		AccountId = _E05B._E000(25616);
		LoginLanguage = _E05B._E000(24499);
		UserRegion = _E05B._E000(24450);
	}

	public void ResetTokenData()
	{
		AccessToken = null;
		AccessTokenExpirationTimeUtc = default(DateTime);
		RefreshToken = null;
	}

	public void InitialLoad()
	{
		try
		{
			_logger.LogDebug(_E05B._E000(24103));
			string path = Path.Combine(AppDataDir, _appConfig.SettingsFileName);
			if (File.Exists(path))
			{
				JObject jObject = JObject.Parse(File.ReadAllText(path));
				MigrateIfRequired(jObject);
				JsonConvert.PopulateObject(jObject.ToString(), this);
				_logger.LogInformation(_E05B._E000(24123), GetSafeClone().GetSavableSettings());
			}
			else
			{
				_logger.LogInformation(_E05B._E000(24162));
				IsFirstStart = true;
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(24158));
			IsFirstStart = true;
		}
		finally
		{
			if (string.IsNullOrWhiteSpace(LauncherTempDir))
			{
				LauncherTempDir = Path.Combine(Directory.GetCurrentDirectory(), _E05B._E000(24219));
			}
			Directory.CreateDirectory(LauncherTempDir);
		}
	}

	private void MigrateIfRequired(JObject json)
	{
		int settingsVersion;
		for (settingsVersion = json[_E05B._E000(20535)]?.Value<int>() ?? 0; settingsVersion != 1; settingsVersion = json[_E05B._E000(20535)].Value<int>())
		{
			ISettingsMigration settingsMigration = _migrations.FirstOrDefault((ISettingsMigration m) => m.FromVersion == settingsVersion);
			if (settingsMigration != null)
			{
				settingsMigration.Migrate(json);
			}
			else
			{
				_logger.LogError(_E05B._E000(24222), settingsVersion);
			}
		}
	}

	public string GetGuiSettings()
	{
		return JsonConvert.SerializeObject(this, new JsonSerializerSettings
		{
			Converters = new List<JsonConverter>
			{
				new BsgVersionConverter()
			},
			ContractResolver = new TaggedJsonContractResolver(_E05B._E000(24277))
		});
	}

	public string GetSavableSettings()
	{
		return JsonConvert.SerializeObject(this, new JsonSerializerSettings
		{
			ContractResolver = new TaggedJsonContractResolver(_E05B._E000(24544))
		});
	}

	public void Update(string json)
	{
		_logger.LogDebug(_E05B._E000(24286));
		JsonConvert.PopulateObject(json, this);
		Save();
		_logger.LogDebug(_E05B._E000(23865), GetSafeClone().GetSavableSettings());
		RaiseOnSettingsUpdated();
	}

	private void RaiseOnSettingsUpdated()
	{
		if (this.OnSettingsUpdated == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnSettingsUpdated.GetInvocationList();
		foreach (Delegate @delegate in invocationList)
		{
			try
			{
				@delegate.DynamicInvoke(this, EventArgs.Empty);
			}
			catch (TargetInvocationException)
			{
				_logger.LogError(_E05B._E000(23929));
			}
			catch (FileNotFoundException)
			{
				_logger.LogError(_E05B._E000(23997));
			}
		}
	}

	private SettingsService GetSafeClone()
	{
		SettingsService obj = (SettingsService)MemberwiseClone();
		obj.AccessToken = _E05B._E000(24061);
		obj.RefreshToken = _E05B._E000(24010);
		obj.LoginOrEmail = obj.LoginOrEmail?.ToSecretData();
		return obj;
	}

	public void LoadUserProfile(UserProfile userProfile)
	{
		try
		{
			AccountId = userProfile.AccountId;
			Nickname = userProfile.Nickname;
			LoginLanguage = userProfile.UserLanguage;
			UserRegion = userProfile.UserRegion;
			IpRegion = userProfile.IpRegion;
			GameEdition = userProfile.GameEdition;
			PurchaseDate = userProfile.PurchaseDate;
			AvatarUrl = userProfile.AvatarUrl;
			SupportUnreadNotifications = userProfile.SupportUnreadNotifications;
			SitePushStreamChannels = userProfile.SitePushStreamChannels;
			ChannelSettings = (IReadOnlyCollection<ChannelSettings>)(object)userProfile.ChannelSettings;
			GeoInfo = userProfile.GeoInfo;
			if (userProfile.MultiChannelDownloadSettings != null)
			{
				try
				{
					string text = JsonConvert.SerializeObject(userProfile.MultiChannelDownloadSettings);
					_logger.LogInformation(_E05B._E000(24023), text);
					_multichannelDownloaderOptions.Update(userProfile.MultiChannelDownloadSettings);
				}
				catch (Exception exception)
				{
					_logger.LogError(exception, _E05B._E000(23579));
				}
			}
			Branches.ForEach(delegate(IBranch b)
			{
				b.OnSettingsUpdated -= BranchSettingsUpdated;
			});
			BranchData[] branches = userProfile.Branches;
			foreach (BranchData bInfo in branches)
			{
				IBranch branch = Branches.FirstOrDefault((IBranch b) => b.Name == bInfo.Name);
				if (branch != null)
				{
					branch.Update(bInfo);
					continue;
				}
				Branch item = new Branch(_accessService, _loggerFactory, _appConfig, bInfo);
				Branches.Add(item);
			}
			Branches.Where((IBranch b) => userProfile.Branches.All((BranchData sb) => sb.Name != b.Name)).ToArray().ForEach(delegate(IBranch ob)
			{
				Branches.Remove(ob);
				this.OnBranchObsolete?.Invoke(this, ob);
			});
			if (!Branches.Any((IBranch b) => b.IsSelected))
			{
				SelectDefaultBranch();
			}
			if (!Branches.First((IBranch b) => b.IsSelected).IsActive || Branches.First((IBranch b) => b.IsSelected).BranchParticipationStatus == BranchParticipationStatus.Suspended)
			{
				SelectDefaultBranch();
			}
			Branches.ForEach(delegate(IBranch b)
			{
				b.OnSettingsUpdated += BranchSettingsUpdated;
			});
			IsBackendSettingsLoaded = true;
			this.OnUserProfileLoaded?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception innerException)
		{
			throw new SettingsServiceException(BsgExceptionCode.ErrorLoadingBackendSettings, innerException);
		}
	}

	public void LoadPlayerProfile(PlayerProfile playerProfile)
	{
		ProfileLevel = playerProfile?.ProfileLevel ?? _E05B._E000(23631);
		TotalTimeInGameSec = playerProfile?.TotalTimeInGameSec ?? (-1);
	}

	private void BranchSettingsUpdated(object sender, EventArgs e)
	{
		RaiseOnSettingsUpdated();
	}

	private void SelectDefaultBranch()
	{
		SelectedBranch = Branches.FirstOrDefault((IBranch b) => b.IsDefault) ?? throw new Exception(_E05B._E000(23628));
	}
}
