using Eft.Launcher.Services.AuthService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Base.Services.AuthService;

public static class AuthServiceExtensions
{
	public static IServiceCollection AddAuthService(this IServiceCollection services)
	{
		return services.AddSingleton<IAuthService, _E01B>();
	}
}
