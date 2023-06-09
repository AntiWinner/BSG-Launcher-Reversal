using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Eft.Launcher;
using Eft.Launcher.Services.InformationCollectionService;

namespace Bsg.Launcher.Services.BugReportService;

public class BugReport
{
	[CompilerGenerated]
	private DateTime _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private SystemInfo _E003;

	[CompilerGenerated]
	private BsgVersion _E004;

	[CompilerGenerated]
	private IEnumerable<string> _E005;

	[CompilerGenerated]
	private IEnumerable<string> _E006;

	[CompilerGenerated]
	private IEnumerable<string> _E007;

	[CompilerGenerated]
	private IEnumerable<ServerAvailabilityInfo> _E008;

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

	public string CategoryId
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

	public string Message
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

	public SystemInfo SystemInfo
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

	public BsgVersion GameVersion
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

	public IEnumerable<string> AttachedFiles
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

	public IEnumerable<string> GameLogDirectories
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		set
		{
			_E006 = value;
		}
	}

	public IEnumerable<string> LauncherLogFiles
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		set
		{
			_E007 = value;
		}
	}

	public IEnumerable<ServerAvailabilityInfo> ServersAvailabilityInfo
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
		[CompilerGenerated]
		set
		{
			_E008 = value;
		}
	}
}
