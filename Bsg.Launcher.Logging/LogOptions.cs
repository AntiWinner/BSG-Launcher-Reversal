using System;
using System.Runtime.CompilerServices;

namespace Bsg.Launcher.Logging;

public class LogOptions
{
	[CompilerGenerated]
	private string _E000 = Guid.NewGuid().ToString();

	[CompilerGenerated]
	private RemoteLogOptions _E001;

	[CompilerGenerated]
	private FileLogOptions _E002;

	[CompilerGenerated]
	private ConsoleLogOptions _E003;

	public string TraceId
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

	public RemoteLogOptions RemoteLogOptions
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

	public FileLogOptions FileLogOptions
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

	public ConsoleLogOptions ConsoleLogOptions
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
}
