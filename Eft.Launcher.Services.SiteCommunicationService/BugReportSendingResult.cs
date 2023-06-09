using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.SiteCommunicationService;

public class BugReportSendingResult
{
	[CompilerGenerated]
	private readonly int _E000;

	[CompilerGenerated]
	private readonly string _E001;

	public int BugReportsLeft
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public string BugReportId
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public BugReportSendingResult(int bugReportsLeft, string bugReportId)
	{
		_E000 = bugReportsLeft;
		_E001 = bugReportId;
	}
}
