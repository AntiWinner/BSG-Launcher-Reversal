using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.Services.BugReportService;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.SiteCommunicationService;

public interface ISiteCommunicationService
{
	event Action<SiteNetworkState> OnNetworkStateChanged;

	Task<BugReportSendingResult> SendBugReport(BugReport bugReport, int gameLogsSizeLimit, IEnumerable<string> sids, Action<int> onProgress);

	Task<BugReportSendingResult> SendGameCrashReport(CrashReport crashReport);

	Task SendSystemInfo();

	Task<bool> CheckIfLicenseAgreementIsRequired();

	Task<string> GetFeedbackFormData();

	Task<JToken> ActivateCode(string activationCode);

	Task<JToken> RequestAsync(string relativeUri, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken));
}
