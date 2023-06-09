using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bsg.Launcher.WebSockets;

public class WebSocketSettingsBuilder
{
	private string[] m__E000 = new string[0];

	private string _E001 = _E05B._E000(3715);

	private int[] _E002 = new int[0];

	public WebSocketSettingsBuilder WithNamespaces(params string[] namespaces)
	{
		this.m__E000 = namespaces ?? new string[0];
		return this;
	}

	public WebSocketSettingsBuilder WithPostfix(string postfix)
	{
		_E001 = postfix;
		return this;
	}

	public WebSocketSettingsBuilder WithReconnectionAttempts(int[] reconnectionAttempts)
	{
		_E002 = reconnectionAttempts ?? new int[0];
		return this;
	}

	internal WebSocketSettings _E000()
	{
		return new WebSocketSettings(this._E000(), _E002);
	}

	private JsonSerializerSettings _E000()
	{
		return new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = new CamelCaseNamingStrategy()
			},
			SerializationBinder = new WebSocketDefaultSerializationBinder(this.m__E000, _E001),
			Formatting = Formatting.Indented
		};
	}
}
