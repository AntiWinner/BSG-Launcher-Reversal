using System.Collections.Generic;
using Bsg.Launcher.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eft.Launcher.Base.Services.ConsistencyService;

[JsonObject]
internal class ConsistencyInfo
{
	private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
	{
		NullValueHandling = NullValueHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore,
		MissingMemberHandling = MissingMemberHandling.Ignore,
		Converters = new List<JsonConverter>
		{
			new BsgVersionConverter(),
			new StringEnumConverter()
		}
	};

	[JsonConverter(typeof(BsgVersionConverter))]
	public BsgVersion Version { get; private set; }

	public List<ConsistencyInfoEntry> Entries { get; private set; }

	private ConsistencyInfo()
	{
	}

	public ConsistencyInfo(BsgVersion version)
	{
		Entries = new List<ConsistencyInfoEntry>();
		Version = version;
	}

	public string ToJson()
	{
		return JsonConvert.SerializeObject(this, Formatting.None, _serializerSettings);
	}

	public static ConsistencyInfo FromJson(string json)
	{
		return JsonConvert.DeserializeObject<ConsistencyInfo>(json, _serializerSettings);
	}
}
