using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Bsg.Launcher.Models;

public class GameSessionInfo
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private QueueInfo _E001;

	[JsonProperty("session")]
	public string Session
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

	[JsonProperty("queue_info")]
	public QueueInfo QueueInfo
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
