using Eft.Launcher.Services.BackendService;
using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.Services.BackendService;

public static class DiExtensions
{
	public static IServiceCollection AddGameBackend(this IServiceCollection services)
	{
		return services.AddSingleton<IGameBackendService, _E018>();
	}

	public static IServiceCollection AddLauncherBackend(this IServiceCollection services)
	{
		return services.AddSingleton<ILauncherBackendService, _E01A>();
	}
}
