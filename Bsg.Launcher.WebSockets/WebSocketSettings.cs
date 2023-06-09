using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Bsg.Launcher.WebSockets;

public class WebSocketSettings
{
	[CompilerGenerated]
	private JsonSerializer _E000;

	[CompilerGenerated]
	private int[] _E001;

	public JsonSerializer Serializer
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

	public int[] ReconnectionAttempts
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

	public WebSocketSettings(JsonSerializerSettings serializerSettings, int[] reconnectionAttempts = null)
	{
		Serializer = JsonSerializer.Create(serializerSettings);
		ReconnectionAttempts = reconnectionAttempts ?? new int[0];
	}
}
