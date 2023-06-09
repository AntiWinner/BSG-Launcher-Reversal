using System;
using System.Net.Http;
using System.Threading.Tasks;
using Eft.Launcher.Network.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Network.Http;

public static class HttpExtensions
{
	public static IServiceCollection AddHttpNetwork(this IServiceCollection services)
	{
		return services.AddHttpNetwork(null);
	}

	public static IServiceCollection AddHttpNetwork(this IServiceCollection services, Action<HttpClientBuilderOptions> configureOptions)
	{
		HttpClientBuilderOptions httpClientBuilderOptions = new HttpClientBuilderOptions();
		configureOptions?.Invoke(httpClientBuilderOptions);
		return services.AddTransient<IHttpClientBuilder, _E01F>().AddSingleton(httpClientBuilderOptions);
	}

	public static async Task<JContainer> ReadAsJsonAsync(this HttpContent content)
	{
		if (content == null)
		{
			return null;
		}
		if (content is JsonContent jsonContent)
		{
			return jsonContent.Json;
		}
		return JObject.Parse(await content.ReadAsStringAsync());
	}
}
