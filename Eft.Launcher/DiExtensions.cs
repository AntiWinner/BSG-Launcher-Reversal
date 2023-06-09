using System;
using Eft.Launcher.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher;

public static class DiExtensions
{
	public static IServiceCollection AddService<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService, IService
	{
		services.AddSingleton<TImplementation>();
		services.AddSingleton((IServiceProvider s) => (TService)s.GetService<TImplementation>());
		services.AddSingleton((Func<IServiceProvider, IService>)((IServiceProvider s) => s.GetService<TImplementation>()));
		return services;
	}
}
