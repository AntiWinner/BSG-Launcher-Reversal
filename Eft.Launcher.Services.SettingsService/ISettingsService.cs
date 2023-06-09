using System;
using System.Collections.Generic;
using Bsg.Network.MultichannelDownloading;

namespace Eft.Launcher.Services.SettingsService;

public interface ISettingsService : IService
{
	bool IsBackendSettingsLoaded { get; }

	bool IsFirstStart { get; }

	string Configuration { get; }

	string SystemInfoChecksum { get; set; }

	ICollection<IBranch> Branches { get; set; }

	IBranch SelectedBranch { get; set; }

	string AppDataDir { get; }

	string LocalAppDataDir { get; }

	string ProgramDataDir { get; }

	string LauncherTempDir { get; set; }

	string P2pDir { get; }

	Uri LauncherBackendUri { get; }

	Uri AuthCenterUri { get; }

	Uri GuiUri { get; }

	Uri MainPageUri { get; }

	Uri LoginPageUri { get; }

	Uri RequestRestartPageUri { get; }

	Uri DialogPageUri { get; }

	Uri ErrorPageUri { get; }

	Uri ProgressBarPageUri { get; }

	Uri SelectLanguagePageUri { get; }

	Uri BugReportPageUri { get; }

	Uri InstallationPageUri { get; }

	Uri MatchingConfigurationPageUri { get; set; }

	Uri LicenseAgreementPageUri { get; }

	Uri CodeActivationPageUri { get; }

	Uri FeedbackPageUri { get; }

	Uri CaptchaPageUri { get; }

	string AccountId { get; set; }

	string Nickname { get; set; }

	string LoginOrEmail { get; set; }

	string AccessToken { get; set; }

	DateTime AccessTokenExpirationTimeUtc { get; set; }

	string RefreshToken { get; set; }

	bool KeepLoggedIn { get; set; }

	bool SaveLogin { get; set; }

	string LoginLanguage { get; set; }

	string UserRegion { get; set; }

	string IpRegion { get; set; }

	string GameEdition { get; set; }

	bool IsGameBought { get; }

	string PurchaseDate { get; set; }

	string ProfileLevel { get; set; }

	IReadOnlyCollection<ChannelSettings> ChannelSettings { get; }

	string Language { get; set; }

	bool LaunchOnStartup { get; set; }

	bool LaunchMinimized { get; set; }

	CloseBehavior CloseBehavior { get; set; }

	GameStartBehavior GameStartBehavior { get; set; }

	bool GameAutoUpdateEnabled { get; set; }

	int UpdateCheckInterval { get; set; }

	int GameProcessId { get; set; }

	uint MaxDownloadSpeedKb { get; set; }

	uint MaxUploadSpeedKb { get; set; }

	bool SoundQueueNotificationEnabled { get; set; }

	bool QueueAutostartEnabled { get; set; }

	int VolumeValue { get; set; }

	int MaxBugReportSize { get; }

	int MaxCrashReportSize { get; }

	int PingTimeout { get; }

	int TracertTimeout { get; set; }

	int TracertMaxHops { get; set; }

	int MaxUncompressedLogPackedSizeMb { get; set; }

	DateTime LastSendingLogsTime { get; set; }

	bool IsInTheGameInstallationProgress { get; set; }

	event EventHandler OnUserProfileLoaded;

	event EventHandler OnSettingsUpdated;

	event EventHandler<OnBranchChangedEventArgs> OnBranchChanged;

	event EventHandler<IBranch> OnBranchObsolete;

	void InitialLoad();

	string GetGuiSettings();

	void Update(string json);

	void Save();

	void ResetAccountSettings();

	void ResetTokenData();

	void LoadUserProfile(UserProfile userProfile);

	void LoadPlayerProfile(PlayerProfile playerProfile);
}
