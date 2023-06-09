using System;

namespace Eft.Launcher.Services.SettingsService;

public interface IBranch
{
	bool IsDefault { get; }

	bool IsActive { get; }

	bool IsSelected { get; set; }

	string Name { get; }

	string MatchingTag { get; }

	string GameRootDir { get; set; }

	string DefaultGameRootDir { get; }

	string GameScreenshotsDir { get; }

	Uri GameBackendUri { get; }

	Uri TradingBackendUri { get; }

	Uri SiteUri { get; set; }

	Uri LogsUri { get; }

	FeedbackBehavior FeedbackBehavior { get; }

	BranchParticipationStatus BranchParticipationStatus { get; }

	BranchStatus BranchStatus { get; }

	string GameAppId { get; }

	string GameDisplayName { get; }

	event EventHandler OnSettingsUpdated;

	void Update(BranchData branchInfo);
}
