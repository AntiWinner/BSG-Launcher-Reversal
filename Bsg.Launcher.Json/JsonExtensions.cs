using System;
using System.Linq;
using Bsg.Launcher.Json.Converters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bsg.Launcher.Json;

public static class JsonExtensions
{
	public static IServiceCollection AddJsonSettings(this IServiceCollection services)
	{
		return services.AddSingleton<JsonConverter, IpAddressConverter>().AddSingleton<JsonConverter, BsgVersionConverter>().AddSingleton<JsonConverter, GuiExceptionConverter>()
			.AddSingleton((IServiceProvider sp) => new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = sp.GetServices<JsonConverter>().ToList(),
				Formatting = Formatting.None
			})
			.AddSingleton((IServiceProvider sp) => JsonSerializer.Create(sp.GetRequiredService<JsonSerializerSettings>()));
	}
}
