using System;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.GameService;

public class QueueProgressEventArgs : EventArgs
{
	[CompilerGenerated]
	private readonly int _E000;

	[CompilerGenerated]
	private readonly int _E001;

	public int QueuePosition
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public int EstimatedTimeSec
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public QueueProgressEventArgs(int queuePosition, int estimatedTimeSec)
	{
		_E000 = queuePosition;
		_E001 = estimatedTimeSec;
	}
}
