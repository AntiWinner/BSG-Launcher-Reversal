using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Bsg.Launcher.Logging;

public class ConsoleLogOptions
{
	[CompilerGenerated]
	private LogLevel _E000;

	public LogLevel Level
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
}
