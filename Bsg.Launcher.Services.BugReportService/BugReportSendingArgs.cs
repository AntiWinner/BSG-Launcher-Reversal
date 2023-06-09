using System;
using System.Runtime.CompilerServices;

namespace Bsg.Launcher.Services.BugReportService;

public class BugReportSendingArgs : EventArgs
{
	[CompilerGenerated]
	private readonly BugReport _E000;

	[CompilerGenerated]
	private readonly bool _E001;

	public BugReport BugReport
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public bool IsSendingSuccessful
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public BugReportSendingArgs(BugReport bugReport, bool isSendingSuccessful)
	{
		_E000 = bugReport;
		_E001 = isSendingSuccessful;
	}
}
