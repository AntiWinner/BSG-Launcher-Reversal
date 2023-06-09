using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bsg.Network.MultichannelDownloading;

public static class DiExtensions
{
	public static IServiceCollection AddMultichannelDownloading(this IServiceCollection services, Action<MultichannelDownloaderOptions> configureOptions = null)
	{
		MultichannelDownloaderOptions multichannelDownloaderOptions = new MultichannelDownloaderOptions();
		configureOptions?.Invoke(multichannelDownloaderOptions);
		return services.AddSingleton(multichannelDownloaderOptions).AddSingleton<IChannelFactory, _E045>().AddSingleton<IChannelMonitorFactory, _E03B>()
			.AddSingleton<IChannelProviderFactory, _E03C>()
			.AddSingleton<IMultichannelDownloaderFactory, _E047>();
	}
}
