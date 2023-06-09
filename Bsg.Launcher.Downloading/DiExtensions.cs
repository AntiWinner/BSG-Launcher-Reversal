using Eft.Launcher;
using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.Downloading;

public static class DiExtensions
{
	public static IServiceCollection AddDownloadManagement(this IServiceCollection services)
	{
		return services.AddDownloadManagement<_E032>();
	}

	public static IServiceCollection AddDownloadManagement<THandler>(this IServiceCollection services) where THandler : class, IDownloadManagementHandler
	{
		return services.AddSingleton<IDownloadManagementHandler, THandler>().AddService<IDownloadManagementService, _E034>();
	}
}
