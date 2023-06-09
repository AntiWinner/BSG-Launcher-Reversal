using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.Utils;

public static class DiExtensions
{
	public static IServiceCollection AddFileManager(this IServiceCollection services)
	{
		return services.AddSingleton<IFileManager, _E027>();
	}

	public static IServiceCollection AddExceptionAdapter(this IServiceCollection services)
	{
		return services.AddSingleton<ExceptionAdapter>();
	}
}
