using Eft.Launcher.Services.InformationCollectionService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Base.Services.InformationCollectionService;

public static class InformationCollectionExtensions
{
	public static IServiceCollection AddInformationCollection(this IServiceCollection services)
	{
		return services.AddSingleton<IInformationCollectionService, _E016>().AddSingleton<ISystemManagement, _E015>();
	}
}
