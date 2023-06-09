using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.WebSockets;

public static class DiExtensions
{
	public static IServiceCollection AddWebSockets(this IServiceCollection services, Action<WebSocketSettingsBuilder> configureSettings = null)
	{
		WebSocketSettingsBuilder webSocketSettingsBuilder = new WebSocketSettingsBuilder();
		configureSettings?.Invoke(webSocketSettingsBuilder);
		WebSocketSettings implementationInstance = webSocketSettingsBuilder._E000();
		services.AddSingleton(implementationInstance);
		services.AddSingleton<IWebSocketClientFactory, _E02F>();
		return services;
	}
}
