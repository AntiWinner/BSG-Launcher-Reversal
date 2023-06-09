using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Eft.Launcher;
using Eft.Launcher.Services.InformationCollectionService;

namespace Bsg.Launcher.Services.BugReportService;

public class CrashReport
{
	[CompilerGenerated]
	private DateTime _E000;

	[CompilerGenerated]
	private BsgVersion _E001;

	[CompilerGenerated]
	private int _E002;

	[CompilerGenerated]
	private bool _E003;

	[CompilerGenerated]
	private IEnumerable<string> _E004;

	[CompilerGenerated]
	private SystemInfo _E005;

	public DateTime CreationTime
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	public BsgVersion GameVersion
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public int ExitCode
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public bool HaveDump
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public IEnumerable<string> AttachedDirectories
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		set
		{
			_E004 = value;
		}
	}

	public SystemInfo SystemInfo
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		set
		{
			_E005 = value;
		}
	}
}
