using System;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.BugReportService;

public class BugReportSendingProgressArgs : EventArgs
{
	[CompilerGenerated]
	private readonly BugReportSendingState _E000;

	[CompilerGenerated]
	private readonly int _E001;

	public BugReportSendingState State
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public int Progress
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public BugReportSendingProgressArgs(BugReportSendingState state, int progress)
	{
		_E000 = state;
		_E001 = progress;
	}
}
