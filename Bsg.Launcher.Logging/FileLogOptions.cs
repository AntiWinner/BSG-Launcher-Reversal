using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Bsg.Launcher.Logging;

public class FileLogOptions
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private int _E002;

	[CompilerGenerated]
	private LogLevel _E003;

	public string LogsDirectory
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

	public string LogFileName
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

	public int LogFilesLimit
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

	public LogLevel Level
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
