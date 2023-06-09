using System;
using System.Runtime.InteropServices;
using Bsg.Launcher.Json;
using Bsg.Launcher.Services.BackendService;
using Bsg.Network.MultichannelDownloading;
using Eft.Launcher.Base.Network.Http;
using Eft.Launcher.Base.Services.AccessService;
using Eft.Launcher.Base.Services.InformationCollectionService;
using Eft.Launcher.Base.Services.SettingsService;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.InformationCollectionService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace Eft.Launcher.Gui.Wpf;

public class SystemInfoProvider
{
	[DllImport("kernel32.dll")]
	public static extern bool AttachConsole(int processId);

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();

	public static int Main(string[] args)
	{
		try
		{
			AttachConsole(-1);
			ServiceProvider provider = new ServiceCollection().AddTransient(typeof(Lazy<>), typeof(Lazy<>)).AddSingleton(AppConfig.Instance).AddSingleton<ISettingsService, SettingsService>()
				.AddGameBackend()
				.AddSingleton<IAccessService, AccessService>()
				.AddSingleton<DIContractResolver>()
				.AddHttpNetwork()
				.AddSingleton<ILoggerFactory, NullLoggerFactory>()
				.AddSingleton<ILogger, NullLogger>()
				.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>))
				.AddSingleton<MultichannelDownloaderOptions>()
				.AddJsonSettings()
				.AddInformationCollection()
				.BuildServiceProvider();
			SystemInfo systemInfo = provider.GetRequiredService<IInformationCollectionService>().GetSystemInfo();
			JsonSerializerSettings requiredService = provider.GetRequiredService<JsonSerializerSettings>();
			Console.WriteLine(JsonConvert.SerializeObject(systemInfo, requiredService));
			return 0;
		}
		finally
		{
			FreeConsole();
		}
	}
}
