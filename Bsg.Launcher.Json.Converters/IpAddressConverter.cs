using System;
using System.Net;
using Newtonsoft.Json;

namespace Bsg.Launcher.Json.Converters;

public class IpAddressConverter : JsonConverter<IPAddress>
{
	public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		if (reader.TokenType == JsonToken.String)
		{
			return IPAddress.Parse((string)serializer.Deserialize(reader, typeof(string)));
		}
		throw new NotImplementedException();
	}

	public override void WriteJson(JsonWriter writer, IPAddress value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}
}
