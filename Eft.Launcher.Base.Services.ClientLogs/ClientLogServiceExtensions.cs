using Eft.Launcher.Services.ClientLogs;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Base.Services.ClientLogs;

public static class ClientLogServiceExtensions
{
	public static IServiceCollection AddClientLogService(this IServiceCollection services)
	{
		return services.AddService<IClientLogService, _E017>();
	}
}
