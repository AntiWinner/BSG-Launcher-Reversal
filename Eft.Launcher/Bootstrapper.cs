using System;
using Bsg.Launcher.ConsistencyControl;
using Bsg.Launcher.Json;
using Bsg.Launcher.Logging;
using Bsg.Launcher.Updating;
using Bsg.Launcher.Utils;
using Bsg.Network.MultichannelDownloading;
using DryIoc;
using Eft.Launcher.Base.Network.Http;
using Eft.Launcher.Base.Services;
using Eft.Launcher.Base.Services.AccessService;
using Eft.Launcher.Base.Services.AnalyticsService;
using Eft.Launcher.Base.Services.AuthService;
using Eft.Launcher.Base.Services.BugReportService;
using Eft.Launcher.Base.Services.ClientLogs;
using Eft.Launcher.Base.Services.ConsistencyService;
using Eft.Launcher.Base.Services.GameService;
using Eft.Launcher.Base.Services.InformationCollectionService;
using Eft.Launcher.Base.Services.SettingsService;
using Eft.Launcher.Base.Services.SiteCommunicationService;
using Eft.Launcher.Base.Services.UpdateServices;
using Eft.Launcher.Services;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.AnalyticsService;
using Eft.Launcher.Services.BugReportService;
using Eft.Launcher.Services.CompressionService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.DownloadService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher;

public class Bootstrapper
{
	private static IServiceProvider m__E000;

	private static IServiceCollection _E000(IServiceCollection services)
	{
		return services.AddSingleton(AppConfig.Instance).AddService<ISettingsService, SettingsService>().AddService<IAnalyticsService, AnalyticsService>()
			.AddService<IBugReportService, BugReportService>()
			.AddTransient<IBranch, Branch>()
			.AddSingleton<DIContractResolver>()
			.AddSingleton<IDownloadService, DownloadService>()
			.AddSingleton<ICompressionService, CompressionService>()
			.AddSingleton<IGameService, GameService>()
			.AddSingleton<ILauncherUpdateService, LauncherUpdateService>()
			.AddSingleton<ILauncherMetadata, LauncherMetadata>()
			.AddSingleton<IGameUpdateService, GameUpdateService>()
			.AddSingleton<IConsistencyService, ConsistencyService>()
			.AddSingleton<ISiteCommunicationService, SiteCommunicationService>()
			.AddSingleton<IAccessService, AccessService>()
			.AddSingleton<Utils>()
			.AddInformationCollection()
			.AddHttpNetwork()
			.AddAuthService()
			.AddClientLogService()
			.AddJsonSettings()
			.AddLogging()
			.AddMultichannelDownloading()
			.AddConsistencyControl()
			.AddFileManager()
			.AddUpdateManagement();
	}

	public static void Run(LifecycleControllerBase lifecycleController, string[] args)
	{
		_E038 obj = new _E038(new Container(Rules.MicrosoftDependencyInjectionRules));
		_E000(obj);
		lifecycleController.ConfigureServices(obj);
		Bootstrapper.m__E000 = obj._E000();
		lifecycleController.Configure(Bootstrapper.m__E000);
		LogOptions requiredService = Bootstrapper.m__E000.GetRequiredService<LogOptions>();
		ILogger<Bootstrapper> service = Bootstrapper.m__E000.GetService<ILogger<Bootstrapper>>();
		Version launcherVersion = Bootstrapper.m__E000.GetRequiredService<ILauncherMetadata>().LauncherVersion;
		service.LogInformation(_E05B._E000(2898), launcherVersion.ToString(), requiredService.TraceId.ToString());
		service.LogInformation(_E05B._E000(2958), Environment.OSVersion.Platform, Environment.OSVersion.Version);
		Bootstrapper.m__E000.GetService<ISettingsService>().InitialLoad();
		service.LogDebug(_E05B._E000(3046));
		foreach (IService service2 in Bootstrapper.m__E000.GetServices<IService>())
		{
			service.LogTrace(_E05B._E000(3069), service2.GetType().Name);
			service2.OnAwake();
			service.LogDebug(_E05B._E000(3030), service2.GetType().Name);
		}
		service.LogDebug(_E05B._E000(2600));
		lifecycleController.OnExiting += _E000;
		lifecycleController.OnStart(args ?? new string[0]);
	}

	private static void _E000()
	{
		ILogger<Bootstrapper> service = Bootstrapper.m__E000.GetService<ILogger<Bootstrapper>>();
		service.LogDebug(_E05B._E000(2566));
		foreach (IService service2 in Bootstrapper.m__E000.GetServices<IService>())
		{
			service.LogTrace(_E05B._E000(2590), service2.GetType().Name);
			service2.OnStop();
			service.LogDebug(_E05B._E000(2677), service2.GetType().Name);
		}
		service.LogDebug(_E05B._E000(2632));
	}
}
