using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.ConsistencyControl;

public static class DiExtensions
{
	public static IServiceCollection AddConsistencyControl(this IServiceCollection services)
	{
		return services.AddSingleton<IConsistencyControlService, _E035>();
	}
}
