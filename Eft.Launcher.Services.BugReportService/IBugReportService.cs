using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bsg.Launcher.Services.BugReportService;
using Eft.Launcher.Services.SiteCommunicationService;

namespace Eft.Launcher.Services.BugReportService;

public interface IBugReportService
{
	event PreparingToSendAutoReportEventHandler OnPreparingToSendAutoReport;

	event EventHandler OnBugReportSendingStarted;

	event EventHandler<BugReportSendingArgs> OnBugReportSendingCompleted;

	event EventHandler<BugReportSendingProgressArgs> OnBugReportSendingProgress;

	Task<BugReportSendingResult> SendBugReport(string categoryId, string message, IEnumerable<string> attachedFiles, TimeSpan gameLogsFreshness, int gameLogsSizeLimit, bool attachLauncherLogs, bool collectServersAvailabilityInfo);

	long CalculateBugReportSize(IEnumerable<string> attachedFiles, TimeSpan gameLogsFreshness, int gameLogsSizeLimit, bool attachLauncherLogs);
}
