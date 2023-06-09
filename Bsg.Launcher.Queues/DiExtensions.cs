using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.Queues;

public static class DiExtensions
{
	public static IServiceCollection AddQueues(this IServiceCollection services)
	{
		return services.AddSingleton<IQueueHandler, _E031>();
	}
}
