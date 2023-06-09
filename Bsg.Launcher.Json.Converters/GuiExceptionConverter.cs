using System;
using System.Linq;
using Eft.Launcher;
using Eft.Launcher.Network.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bsg.Launcher.Json.Converters;

public class GuiExceptionConverter : JsonConverter<Exception>
{
	public override Exception ReadJson(JsonReader reader, Type objectType, Exception existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override void WriteJson(JsonWriter writer, Exception value, JsonSerializer serializer)
	{
		JObject jObject = new JObject();
		jObject.Add(_E05B._E000(13272), value.GetType().Name);
		if (value is CodeException ex)
		{
			jObject.Add(_E05B._E000(21599), ex.Code);
			jObject.Add(_E05B._E000(21670), JToken.FromObject(ex.Args));
		}
		else if (value is ApiNetworkException ex2)
		{
			jObject.Add(_E05B._E000(21599), ex2.ApiCode);
			string propertyName = _E05B._E000(21670);
			JToken apiData = ex2.ApiData;
			jObject.Add(propertyName, (apiData == null || !apiData.HasValues) ? ex2.ApiArgs : ex2.ApiData);
		}
		if (!serializer.Converters.Any((JsonConverter c) => c.GetType() == typeof(GuiExceptionConverter)))
		{
			serializer.Converters.Add(this);
		}
		jObject.Add(_E05B._E000(21736), value.Message);
		if (value.InnerException != null)
		{
			jObject.Add(_E05B._E000(57636), JToken.FromObject(value.InnerException, serializer));
		}
		jObject.WriteTo(writer);
	}
}
