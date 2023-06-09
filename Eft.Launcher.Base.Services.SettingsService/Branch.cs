using System;
using System.IO;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Eft.Launcher.Base.Services.SettingsService;

[JsonObject(MemberSerialization.OptIn)]
public class Branch : IBranch
{
	private const string DefaultBranchName = "live";

	private readonly ILogger _logger;

	private readonly IAccessService _accessService;

	private readonly AppConfig _appConfig;

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "isDefault")]
	public bool IsDefault { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "isActive")]
	public bool IsActive { get; private set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "isSelected")]
	public bool IsSelected { get; set; }

	[Tag(new string[] { "ForGUI", "ForSave" })]
	[JsonProperty(PropertyName = "name")]
	public string Name { get; private set; }

	public string MatchingTag { get; private set; }

	private string UninstallSubKey => _E05B._E000(26210) + GameAppId;

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "gameDirectory")]
	public string GameRootDir
	{
		get
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(UninstallSubKey, writable: false);
			return (registryKey?.GetValue(_E05B._E000(26357)) as string).WithLongPathPrefix();
		}
		set
		{
			value = value.WithoutLongPathPrefix();
			if (GameRootDir.WithoutLongPathPrefix() != value)
			{
				_accessService.AddEntryToRegistry(UninstallSubKey, _E05B._E000(26357), value, RegistryValueKind.String);
				_logger.LogInformation(_E05B._E000(20836), value);
				RaiseOnSettingsUpdated();
			}
		}
	}

	public string DefaultGameRootDir => (_E05B._E000(20817) + _appConfig.AppPublisher + _E05B._E000(24721) + (Name.Equals(_E05B._E000(20821), StringComparison.InvariantCultureIgnoreCase) ? _E05B._E000(20898) : (_E05B._E000(20824) + Name + _E05B._E000(24948)))).WithLongPathPrefix();

	public string GameScreenshotsDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _appConfig.GameFullName, _E05B._E000(20902));

	public Uri GameBackendUri { get; private set; }

	public Uri TradingBackendUri { get; } = new Uri(_E05B._E000(20917));


	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "siteUri")]
	public Uri SiteUri { get; set; }

	public Uri LogsUri { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "feedbackBehavior")]
	public FeedbackBehavior FeedbackBehavior { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "participationStatus")]
	public BranchParticipationStatus BranchParticipationStatus { get; private set; }

	[Tag(new string[] { "ForGUI" })]
	[JsonProperty(PropertyName = "status")]
	public BranchStatus BranchStatus { get; private set; }

	public string GameAppId
	{
		get
		{
			if (!Name.ToLowerInvariant().StartsWith(_E05B._E000(20821)))
			{
				return _appConfig.GameShortName + _E05B._E000(20914) + Name;
			}
			return _appConfig.GameShortName;
		}
	}

	public string GameDisplayName
	{
		get
		{
			if (!Name.ToLowerInvariant().StartsWith(_E05B._E000(20821)))
			{
				return _appConfig.GameFullName + _E05B._E000(20912) + Name + _E05B._E000(24948);
			}
			return _appConfig.GameFullName;
		}
	}

	public event EventHandler OnSettingsUpdated;

	internal Branch(IAccessService accessService, ILoggerFactory loggerFactory, AppConfig appConfig, BranchData branchInfo)
		: this(accessService, loggerFactory, appConfig)
	{
		Update(branchInfo);
	}

	public Branch(IAccessService accessService, ILoggerFactory loggerFactory, AppConfig appConfig)
	{
		_logger = loggerFactory.CreateLogger<Branch>();
		_accessService = accessService;
		_appConfig = appConfig;
	}

	public void Update(BranchData branchData)
	{
		FeedbackBehavior = branchData.FeedbackBehavior;
		BranchParticipationStatus = branchData.BranchParticipationStatus;
		BranchStatus = branchData.BranchStatus;
		GameBackendUri = new Uri(branchData.GameBackendUri);
		IsActive = branchData.IsActive;
		IsDefault = branchData.IsDefault;
		LogsUri = new Uri(branchData.LogsUri);
		Name = branchData.Name;
		MatchingTag = (string.IsNullOrWhiteSpace(branchData.MatchingTag) ? branchData.Name : branchData.MatchingTag);
		SiteUri = new Uri(branchData.SiteUri);
	}

	private void RaiseOnSettingsUpdated()
	{
		this.OnSettingsUpdated?.Invoke(this, EventArgs.Empty);
	}
}
