using System;
using Eft.Launcher;
using Newtonsoft.Json;

namespace Bsg.Launcher.Json.Converters;

public class BsgVersionConverter : JsonConverter<BsgVersion>
{
	public override BsgVersion ReadJson(JsonReader reader, Type objectType, BsgVersion existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.String)
		{
			if (!BsgVersion.TryParse((string)reader.Value, out var result))
			{
				return default(BsgVersion);
			}
			return result;
		}
		throw new NotSupportedException(string.Format(_E05B._E000(57988), _E05B._E000(58079), reader.TokenType, JsonToken.String));
	}

	public override void WriteJson(JsonWriter writer, BsgVersion value, JsonSerializer serializer)
	{
		writer.WriteValue((value == default(BsgVersion)) ? null : value.ToString());
	}
}
