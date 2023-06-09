namespace Eft.Launcher.Services.DialogService;

public enum DialogWindowMessage
{
	DirectoryNotEmpty = 12100001,
	NoAccessToTheFolder = 12000002,
	AbortTheInstallationProcess = 13100003,
	BugReportSuccessfullySent = 11000004,
	DoYouWantToSendBugReportAfterLauncherCrash = 13300005,
	DoYouWantToSendBugReportAfterGameCrash = 13300006,
	IsNotGameDirectory = 11000007,
	FileUsedByAnotherProcessAndCannotBeUpdated = 14400008,
	IfYouStopDownloadingTheCurrentlyDownloadedFilesWillBeDeleted = 13300009,
	CannotInstallTheGameToTheFolder = 11000010,
	UnableToDownoadFile = 14500011,
	YourParticipationInBranchHasBeenRevoked = 11000012,
	TheBranchNoLongerExists = 13300013,
	FailedToEstablishBackendConnection = 14200014,
	DoYouWantToClearDirectory = 13300015,
	TooSlowConnectionFileCantBeDownloaded = 12500016,
	TheGameCannotBeStartedPleaseTryAgainLater = 11000017,
	DownloadingProblem = 14500018,
	InstallationProblem = 14500019
}
