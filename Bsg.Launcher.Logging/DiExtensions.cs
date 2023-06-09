using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Bsg.Launcher.Logging;

public static class DiExtensions
{
	public static IServiceCollection AddLogging(this IServiceCollection services, Action<LogOptions> configure = null)
	{
		LogOptions logOptions = new LogOptions();
		configure?.Invoke(logOptions);
		return services.AddSingleton(logOptions).AddSingleton(typeof(ILogger<>), typeof(Logger<>)).AddSingleton<ILogEventEnricher, _E05A>()
			.AddSingleton<ILogEventEnricher, _E059>()
			.AddSingleton<ILogEventEnricher, _E057>()
			.AddSingleton<ILogEventEnricher, _E058>()
			.AddSingleton<_E054>()
			.AddSingleton<ILoggerFactory, _E053>();
	}

	public static IServiceProvider ConfigureLogging(this IServiceProvider sp, Action<LogOptions> configure)
	{
		LogOptions requiredService = sp.GetRequiredService<LogOptions>();
		configure?.Invoke(requiredService);
		return sp;
	}
}
