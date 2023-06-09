using System.Runtime.CompilerServices;

namespace Bsg.Launcher.Logging;

public class RemoteLogOptions
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private string _E001;

	public string ServerUrl
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

	public string ApiToken
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
}
