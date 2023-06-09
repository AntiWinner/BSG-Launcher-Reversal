using Bsg.Launcher.Updating.Algorithms;
using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Launcher.Updating;

public static class DiExtensions
{
	public static IServiceCollection AddUpdateManagement(this IServiceCollection services)
	{
		return services.AddSingleton<IUpdateManager, _E02A>().AddSingleton<IPatchAlgorithmProvider, _E028>().AddSingleton<IPatchAlgorithm, _E02D>()
			.AddSingleton<IPatchAlgorithm, BsDiffPatchAlgorithm>();
	}
}
