using System;
using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.WebSockets.Managed;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bsg.Launcher;
using Bsg.Launcher.ConsistencyControl;
using Bsg.Launcher.Downloading;
using Bsg.Launcher.Logging;
using Bsg.Launcher.Models;
using Bsg.Launcher.Models.WsMessages;
using Bsg.Launcher.Network.Http;
using Bsg.Launcher.Queues;
using Bsg.Launcher.Services.AuthService;
using Bsg.Launcher.Services.BackendService;
using Bsg.Launcher.Updating;
using Bsg.Launcher.Utils;
using Bsg.Launcher.WebSockets;
using Bsg.Network.MultichannelDownloading;
using CefSharp;
using CefSharp.Handler;
using CefSharp.Wpf;
using DryIoc;
using Eft.Launcher;
using Eft.Launcher.Base.Network.Http;
using Eft.Launcher.Base.Services.BackendService;
using Eft.Launcher.Gui.Wpf.CefEngine;
using Eft.Launcher.Gui.Wpf.Services;
using Eft.Launcher.Gui.Wpf.ViewModels;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Logging;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Security.Cryptography.MD5;
using Eft.Launcher.Services;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.ClientLogs;
using Eft.Launcher.Services.CompressionService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.InformationCollectionService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using FastRsync.Core;
using FastRsync.Delta;
using FastRsync.Diagnostics;
using FastRsync.Signature;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using Ionic.Zip;
using Ionic.Zlib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Shell;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Context;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Parsing;

[SuppressUnmanagedCodeSecurity]
internal static class _E007
{
	public delegate IntPtr _E000(Microsoft.Shell.WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

	[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "CommandLineToArgvW")]
	private static extern IntPtr _E000([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);

	[DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
	private static extern IntPtr _E000(IntPtr hMem);

	public static string[] _E000(string cmdLine)
	{
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = _E000(cmdLine, out var numArgs);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			string[] array = new string[numArgs];
			for (int i = 0; i < numArgs; i++)
			{
				IntPtr ptr = Marshal.ReadIntPtr(intPtr, i * Marshal.SizeOf(typeof(IntPtr)));
				array[i] = Marshal.PtrToStringUni(ptr);
			}
			return array;
		}
		finally
		{
			_E000(intPtr);
		}
	}
}
internal class _E008 : IDownloadManagementHandler
{
	private readonly IDialogService _E000;

	private Task<DialogResult> _E001;

	public _E008(IDialogService dialogService)
	{
		_E000 = dialogService;
	}

	public void OnDownloadError(Exception exception, ref bool retry)
	{
		bool flag = false;
		try
		{
			Task<DialogResult> task;
			lock (this)
			{
				if (_E001 == null)
				{
					_E001 = _E000.ShowDialog(DialogWindowMessage.DownloadingProblem, exception);
					flag = true;
				}
				task = _E001;
			}
			retry = task.Result == DialogResult.Positive;
		}
		finally
		{
			if (flag)
			{
				lock (this)
				{
					_E001 = null;
				}
			}
		}
	}
}
internal static class _E009
{
	public const int _E000 = 65535;

	public static readonly int _E001 = _E000(_E05B._E000(26569));

	[DllImport("user32", EntryPoint = "PostMessage")]
	public static extern bool _E000(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

	[DllImport("user32", EntryPoint = "RegisterWindowMessage")]
	public static extern int _E000(string message);
}
internal class _E00A : LifecycleControllerBase
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public AppConfig appConfig;

		internal void _E000(LogOptions cfg)
		{
			LogLevel level = LogLevel.Debug;
			string serverUrl = _E05B._E000(25438);
			string apiToken = _E05B._E000(25477);
			cfg.FileLogOptions = new FileLogOptions
			{
				LogsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appConfig.AppPublisher, appConfig.SubfolderName, _E05B._E000(25496)),
				LogFileName = _E05B._E000(25571),
				LogFilesLimit = 10,
				Level = level
			};
			cfg.RemoteLogOptions = new RemoteLogOptions
			{
				ServerUrl = serverUrl,
				ApiToken = apiToken
			};
			cfg.ConsoleLogOptions = new ConsoleLogOptions
			{
				Level = level
			};
		}
	}

	private IServiceProvider m__E000;

	private Microsoft.Extensions.Logging.ILogger _E001;

	private AppConfig _appConfig;

	private ILauncherMetadata _launcherMetadata;

	private ISettingsService _settingsService;

	public _E00A()
	{
		AppDomain.CurrentDomain.UnhandledException += _E000;
	}

	protected internal override void ConfigureServices(IServiceCollection services)
	{
		services.AddCustomCefHandlers().AddLogging()._E000()
			.AddExceptionAdapter()
			.AddGameBackend()
			.AddLauncherBackend()
			.AddWebSockets(delegate(WebSocketSettingsBuilder cfg)
			{
				cfg.WithReconnectionAttempts(new int[1] { 3000 }).WithNamespaces(typeof(QueueWaitingCompletedWsMessage).Namespace);
			})
			.AddDownloadManagement<_E008>()
			.AddQueues()
			.AddSingleton<IDialogService, DialogService>()
			.AddSingleton<ICaptchaHandler, _E00C>();
	}

	protected internal override void Configure(IServiceProvider sp)
	{
		AppConfig appConfig = sp.GetRequiredService<AppConfig>();
		sp.ConfigureLogging(delegate(LogOptions cfg)
		{
			LogLevel level = LogLevel.Debug;
			string serverUrl = _E05B._E000(25438);
			string apiToken = _E05B._E000(25477);
			cfg.FileLogOptions = new FileLogOptions
			{
				LogsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appConfig.AppPublisher, appConfig.SubfolderName, _E05B._E000(25496)),
				LogFileName = _E05B._E000(25571),
				LogFilesLimit = 10,
				Level = level
			};
			cfg.RemoteLogOptions = new RemoteLogOptions
			{
				ServerUrl = serverUrl,
				ApiToken = apiToken
			};
			cfg.ConsoleLogOptions = new ConsoleLogOptions
			{
				Level = level
			};
		});
		this.m__E000 = sp;
		_E001 = sp.GetRequiredService<ILogger<_E00A>>();
		_appConfig = this.m__E000.GetRequiredService<AppConfig>();
		_launcherMetadata = this.m__E000.GetRequiredService<ILauncherMetadata>();
		_settingsService = sp.GetRequiredService<ISettingsService>();
		ViewModelLocator._E000(sp);
	}

	protected internal override void OnStart(string[] args)
	{
		bool flag = args.Length != 0 && args.Contains(_E05B._E000(26588));
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		if (flag)
		{
			Directory.SetCurrentDirectory(directoryName);
		}
		_E001.LogDebug(_E05B._E000(26153), directoryName);
		if (Debugger.IsAttached)
		{
			_E001.LogDebug(_E05B._E000(26127));
		}
		else
		{
			try
			{
				string text = _E05B._E000(26210) + _appConfig.AppId;
				using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text, writable: false);
				if (registryKey == null)
				{
					_E001.LogWarning(_E05B._E000(26273), text);
				}
				else
				{
					string text2 = registryKey.GetValue(_E05B._E000(26357)) as string;
					_E001.LogDebug(_E05B._E000(26309), text2);
					if (text2 != null && text2.TrimEnd('\\', ' ').ToLowerInvariant() != directoryName.TrimEnd('\\', ' ').ToLowerInvariant())
					{
						MessageBox.Show(_E05B._E000(25860) + text2 + _E05B._E000(25957));
						Environment.Exit(0);
					}
				}
			}
			catch (Exception exception)
			{
				_E001.LogWarning(exception, _E05B._E000(26020));
			}
		}
		CefSettings cefSettings = new CefSettings
		{
			CachePath = Path.Combine(_settingsService.LocalAppDataDir, _E05B._E000(26013)),
			LogFile = Path.Combine(_settingsService.LocalAppDataDir, _E05B._E000(26084)),
			UserAgent = new ProductInfoHeaderValue(_E05B._E000(26103), _launcherMetadata.LauncherVersion.ToString()).ToString(),
			IgnoreCertificateErrors = true,
			MultiThreadedMessageLoop = true,
			BrowserSubprocessPath = Assembly.GetExecutingAssembly().Location
		};
		cefSettings.RegisterScheme(new CefCustomScheme
		{
			SchemeName = _E05B._E000(26051),
			SchemeHandlerFactory = new _E00E(),
			IsLocal = true,
			IsSecure = false
		});
		cefSettings.RegisterScheme(new CefCustomScheme
		{
			SchemeName = _settingsService.GuiUri.Scheme,
			SchemeHandlerFactory = new _E00F(),
			DomainName = _settingsService.GuiUri.Host
		});
		cefSettings.CefCommandLineArgs.Add(_E05B._E000(26053), _E05B._E000(26072));
		cefSettings.CefCommandLineArgs.Add(_E05B._E000(26078), _E05B._E000(26072));
		if (args.Contains(_E05B._E000(25657)))
		{
			cefSettings.CefCommandLineArgs.Add(_E05B._E000(25604), _E05B._E000(26072));
		}
		else
		{
			cefSettings.CefCommandLineArgs.Add(_E05B._E000(25604), _E05B._E000(25616));
		}
		CefSharpSettings.ConcurrentTaskExecution = true;
		CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
		Cef.Initialize((CefSettingsBase)cefSettings, performDependencyCheck: false, (IBrowserProcessHandler)null);
		ThreadPool.SetMinThreads(120, 120);
		if (_settingsService.IsFirstStart)
		{
			BrowserWindowBase browserWindowBase = new SelectLanguageWindow(this.m__E000);
			browserWindowBase.Show();
		}
		else if (_settingsService.KeepLoggedIn)
		{
			BrowserWindowBase browserWindowBase = new MainWindow(this.m__E000);
			if (_settingsService.LaunchMinimized && _settingsService.LaunchOnStartup && flag)
			{
				browserWindowBase.WindowState = WindowState.Minimized;
				browserWindowBase.Show();
				browserWindowBase.Hide();
			}
			else
			{
				browserWindowBase.Show();
			}
		}
		else
		{
			BrowserWindowBase browserWindowBase = new LoginWindow(this.m__E000);
			browserWindowBase.Show();
		}
	}

	protected override void OnExit(int exitCode)
	{
		if (_E001 == null)
		{
			return;
		}
		try
		{
			_E001.LogInformation(_E05B._E000(25622), exitCode);
			this.m__E000.GetRequiredService<IAccessService>().StopService();
			_E000(_settingsService.LaunchOnStartup);
			this.m__E000.GetRequiredService<ILauncherUpdateService>();
			_E001.LogDebug(_E05B._E000(25668));
			Cef.Shutdown();
			Cef.WaitForBrowsersToClose();
			_E001.LogInformation(_E05B._E000(25685));
		}
		catch (Exception exception)
		{
			_E001?.LogCritical(exception, _E05B._E000(25772));
		}
		finally
		{
			this.m__E000.GetRequiredService<ILoggerFactory>().Dispose();
		}
	}

	private void _E000(bool isChecked)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(_E05B._E000(25742), writable: true);
			if (isChecked)
			{
				registryKey.SetValue(_appConfig.AppShortName, Assembly.GetExecutingAssembly().Location + _E05B._E000(25848));
			}
			else if (registryKey.GetValue(_appConfig.AppShortName) != null)
			{
				registryKey.DeleteValue(_appConfig.AppShortName);
			}
		}
		catch (Exception exception)
		{
			_E001.LogError(exception, _E05B._E000(25796) + (isChecked ? _E05B._E000(25376) : _E05B._E000(25814)) + _E05B._E000(25388));
		}
	}

	internal void _E000(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		if (_E001 != null)
		{
			_E001.LogCritical(e.Exception, _E05B._E000(25356));
			e.Handled = true;
		}
		else
		{
			MessageBox.Show(e.Exception.Message, _E05B._E000(25447), MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void _E000(object sender, UnhandledExceptionEventArgs e)
	{
		Exception ex = (Exception)e.ExceptionObject;
		_E001?.LogCritical(ex, _E05B._E000(25449), e.IsTerminating);
		try
		{
			this.m__E000.GetRequiredService<DialogService>().ShowException(ex).Wait();
		}
		catch
		{
			MessageBox.Show(ex.Message, _E05B._E000(25447), MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}
}
internal static class _E00B
{
	public static IServiceCollection _E000(this IServiceCollection services)
	{
		return services.AddTransient<MainWindowViewModel>().AddTransient<LoginWindowViewModel>().AddTransient<RequestRestartDialogWindowViewModel>()
			.AddTransient<DialogWindowViewModel>()
			.AddTransient<ErrorWindowViewModel>()
			.AddTransient<ProgressWindowViewModel>()
			.AddTransient<SelectLanguageWindowViewModel>()
			.AddTransient<BugReportWindowViewModel>()
			.AddTransient<InstallationWindowViewModel>()
			.AddTransient<LicenseAgreementWindowViewModel>()
			.AddTransient<MatchingConfigurationWindowViewModel>()
			.AddTransient<LabelViewModel>()
			.AddTransient<FeedbackWindowViewModel>()
			.AddTransient<CaptchaWindowViewModel>()
			.AddTransient<CodeActivationWindowViewModel>();
	}
}
internal class _E00C : ICaptchaHandler
{
	private readonly IDialogService _E000;

	private readonly ILogger<_E00C> _E001;

	private TaskCompletionSource<bool> _E002;

	public _E00C(IDialogService dialogService, ILogger<_E00C> logger)
	{
		this._E000 = dialogService;
		_E001 = logger;
	}

	public async Task<bool> Handle(HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler)
	{
		TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
		TaskCompletionSource<bool> taskCompletionSource2 = Interlocked.CompareExchange(ref _E002, taskCompletionSource, null);
		if (taskCompletionSource2 == null)
		{
			_E001.LogWarning(_E05B._E000(31089));
			bool flag = await this._E000.ShowCaptchaWindow(responseWithCaptcha, clientHandler) == DialogResult.Positive;
			if (flag)
			{
				_E001.LogInformation(_E05B._E000(31040));
			}
			else
			{
				_E001.LogWarning(_E05B._E000(31063));
			}
			taskCompletionSource.SetResult(flag);
			_E002 = null;
			return flag;
		}
		return await taskCompletionSource2.Task;
	}
}
internal class _E00D : DisplayHandler
{
	private readonly Microsoft.Extensions.Logging.ILogger _E000;

	public _E00D(ILogger<_E00D> logger)
	{
		_E000 = logger;
	}

	protected override bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
	{
		if (consoleMessageArgs.Level == LogSeverity.Error || consoleMessageArgs.Level == LogSeverity.Fatal)
		{
			_E000.LogWarning(_E05B._E000(31125), consoleMessageArgs.Level, consoleMessageArgs.Message);
		}
		return base.OnConsoleMessage(chromiumWebBrowser, consoleMessageArgs);
	}
}
internal class _E00E : ISchemeHandlerFactory
{
	public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
	{
		Uri uri = new Uri(request.Url);
		string text = (uri.Host + _E05B._E000(26481) + uri.LocalPath).Replace('/', '\\');
		string mimeType = Cef.GetMimeType(Path.GetExtension(text));
		return ResourceHandler.FromFilePath(text, mimeType, autoDisposeStream: true);
	}
}
internal class _E00F : ISchemeHandlerFactory
{
	public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
	{
		Uri uri = new Uri(request.Url);
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _E05B._E000(30137), uri.LocalPath.TrimStart('/').Replace('/', '\\'));
		if (!File.Exists(text))
		{
			return ResourceHandler.ForErrorMessage(_E05B._E000(31229) + text + _E05B._E000(31174), HttpStatusCode.NotFound);
		}
		string mimeType = Cef.GetMimeType(Path.GetExtension(text));
		return ResourceHandler.FromFilePath(text, mimeType, autoDisposeStream: true);
	}
}
internal class _E010 : IRequestHandler
{
	[CompilerGenerated]
	private EventHandler _E001;

	private readonly Microsoft.Extensions.Logging.ILogger _E002;

	private readonly ISettingsService _settingsService;

	public event EventHandler _E000
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E001;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E001;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public _E010(ILogger<_E010> logger, ISettingsService settingsService)
	{
		_E002 = logger;
		_settingsService = settingsService;
	}

	public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
	{
		return true;
	}

	public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
	{
		_E002.LogDebug(_E05B._E000(31186), targetUrl);
		Process.Start(targetUrl);
		return true;
	}

	public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
	{
		_E002.LogWarning(_E05B._E000(30739));
	}

	public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
	{
		_E002.LogWarning(_E05B._E000(30784));
		return false;
	}

	public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
	{
		_E002.LogDebug(_E05B._E000(30902), status);
	}

	void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
	{
		_E002.LogDebug(_E05B._E000(30964));
		_E001?.Invoke(this, EventArgs.Empty);
	}

	public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
	{
		_E002.LogWarning(_E05B._E000(18208));
		return false;
	}

	public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
	{
		if (request.TransitionType == TransitionType.LinkClicked && request.ResourceType != ResourceType.SubFrame)
		{
			_E002.LogDebug(_E05B._E000(18189), request.Url);
			string text = null;
			try
			{
				text = _settingsService.SelectedBranch.SiteUri.ToString().TrimEnd('/');
			}
			catch
			{
			}
			string text2 = request.Url;
			if (text != null && request.Url.Contains(text))
			{
				text2 = text2.TrimEnd(' ', '/');
				text2 += (request.Url.Contains(_E05B._E000(26772)) ? _E05B._E000(26778) : _E05B._E000(26772));
				text2 = text2 + _E05B._E000(18243) + _settingsService.Language;
			}
			Process.Start(text2);
			return true;
		}
		return false;
	}

	public void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
	{
	}

	public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
	{
		return new Eft.Launcher.Gui.Wpf.CefEngine.ResourceRequestHandler();
	}

	public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
	{
		return true;
	}
}
internal class _E011 : ILifeSpanHandler
{
	private readonly Microsoft.Extensions.Logging.ILogger _E000;

	public _E011(ILogger<_E011> logger)
	{
		_E000 = logger;
	}

	public bool DoClose(IWebBrowser browserControl, IBrowser browser)
	{
		return false;
	}

	public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
	{
	}

	public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
	{
	}

	public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
	{
		_E000.LogDebug(_E05B._E000(18245), targetUrl);
		Process.Start(targetUrl);
		newBrowser = null;
		return true;
	}
}
internal class _E012 : IContextMenuHandler
{
	public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
	{
		model.Clear();
	}

	public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
	{
		return false;
	}

	public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
	{
	}

	public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
	{
		return false;
	}
}
[CompilerGenerated]
internal sealed class _E013
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 20)]
	private struct _E000
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 26)]
	private struct _E001
	{
	}

	internal static readonly _E001 _E000/* Not supported: data(2B 00 25 00 23 00 3B 00 3C 00 3E 00 3A 00 7C 00 3F 00 2A 00 2F 00 22 00 27 00) */;

	internal static readonly _E000 _E001/* Not supported: data(00 00 00 00 05 00 00 00 0F 00 00 00 1E 00 00 00 3C 00 00 00) */;
}
internal class _E014 : JsonConverter
{
	private static readonly byte[] _E000;

	static _E014()
	{
		string s = _E05B._E000(20458);
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher(_E05B._E000(20476)).Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				s = managementObjectEnumerator.Current[_E05B._E000(20004)].ToString();
			}
		}
		catch
		{
		}
		using SHA256 sHA = SHA256.Create();
		_E000 = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		string text = (string)value;
		if (string.IsNullOrEmpty(text))
		{
			writer.WriteNull();
			return;
		}
		if (text.StartsWith(_E05B._E000(20016)) && text.EndsWith(_E05B._E000(20020)))
		{
			writer.WriteValue(text.Trim('{', '}'));
			return;
		}
		using MemoryStream memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(text), writable: false);
		using MemoryStream memoryStream = new MemoryStream();
		using AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider
		{
			Key = _E000
		};
		byte[] iV = aesCryptoServiceProvider.IV;
		memoryStream.Write(iV, 0, iV.Length);
		memoryStream.Flush();
		ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor(_E000, iV);
		using (CryptoStream destination = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
		{
			memoryStream2.CopyTo(destination);
		}
		writer.WriteValue(Convert.ToBase64String(memoryStream.ToArray()));
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		string text = reader.Value as string;
		if (string.IsNullOrEmpty(text))
		{
			return reader.Value;
		}
		try
		{
			using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text), writable: false);
			using MemoryStream memoryStream2 = new MemoryStream();
			using AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider
			{
				Key = _E000
			};
			byte[] array = new byte[16];
			if (memoryStream.Read(array, 0, 16) < 16)
			{
				throw new CryptographicException(_E05B._E000(20024));
			}
			ICryptoTransform transform = aesCryptoServiceProvider.CreateDecryptor(_E000, array);
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(memoryStream2);
			}
			return Encoding.UTF8.GetString(memoryStream2.ToArray());
		}
		catch
		{
			return string.Empty;
		}
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(string);
	}
}
internal class _E015 : ISystemManagement
{
	private enum SystemFirmwareTableProviderSignature
	{
		ACPI = 1094930505,
		FIRM = 1179210317,
		RSMB = 1381190978
	}

	private enum SystemFirmwareTableType : byte
	{
		BiosInformation = 0,
		SystemInformation = 1,
		BaseboardInformation = 2,
		SystemEnclosureOrChassis = 3,
		ProcessorInformation = 4,
		[Obsolete]
		MemoryControllerInformation = 5,
		[Obsolete]
		MemoryModuleInformation = 6,
		CacheInformation = 7,
		PortConnectorInformation = 8,
		SystemSlots = 9,
		[Obsolete]
		OnBoardDevicesInformation = 10,
		OemStrings = 11,
		SystemConfigurationOptions = 12,
		BiosLanguageInformation = 13,
		GroupAssotiations = 14,
		SystemEventLog = 15,
		PhysicalMemoryArray = 16,
		MemoryDevice = 17,
		MemoryErrorInformation32Bit = 18,
		MemoryArrayMappedAddress = 19,
		MemoryDeviceMappedAddress = 20,
		BuiltinPointingDevice = 21,
		PortableBattery = 22,
		SystemReset = 23,
		HardwareSecurity = 24,
		SystemPowerControls = 25,
		VoltageProbe = 26,
		CoolingDevice = 27,
		TemperatureProbe = 28,
		ElectricalCurrentProbe = 29,
		OutOfBandRemoteAccess = 30,
		BootIntegrityServicesEntryPoint = 31,
		SystemBootInformation = 32,
		MemoryErrorInformation64Bit = 33,
		ManagementDevice = 34,
		ManagementDeviceComponent = 35,
		ManagementDeviceThresholdData = 36,
		MemoryChannel = 37,
		IpmiDeviceInformation = 38,
		SystemPowerSupply = 39,
		AdditionalInformation = 40,
		OnboardDevicesExtendedInformation = 41,
		ManagementControllerHostInterface = 42,
		TpmDevice = 43,
		ProcessorAdditionalInformation = 44,
		Inactive = 127,
		EndOfTable = 127
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct SystemFirmwareRawData
	{
		public byte Used20CallingMethod;

		public byte SMBIOSMajorVersion;

		public byte SMBIOSMinorVersion;

		public byte DmiRevision;

		public uint Length;

		public unsafe fixed byte SMBIOSTableData[1];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct _E000
	{
		public SystemFirmwareTableType _E000;

		public byte _E001;

		public ushort _E002;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct _E001
	{
		public _E000 _E000;

		public byte _E001;

		public byte _E002;

		public ushort _E003;

		public byte _E004;

		public byte _E005;

		public ulong _E006;

		public ushort _E007;

		public byte _E008;

		public byte _E009;

		public byte _E00A;

		public byte _E00B;

		public ushort _E00C;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct _E002
	{
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		[CompilerGenerated]
		[UnsafeValueType]
		public struct _E000
		{
			public byte _E000;
		}

		public _E015._E000 _E000;

		public byte _E001;

		public byte _E002;

		public byte _E003;

		public byte _E004;

		public unsafe fixed byte _E005[16];

		public byte _E006;

		public byte _E007;

		public byte _E008;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct _E003
	{
		[StructLayout(LayoutKind.Sequential, Size = 2)]
		[CompilerGenerated]
		[UnsafeValueType]
		public struct _E000
		{
			public ushort _E000;
		}

		public _E015._E000 _E000;

		public byte _E001;

		public byte _E002;

		public byte _E003;

		public byte _E004;

		public byte _E005;

		public byte _E006;

		public byte _E007;

		public ushort _E008;

		public byte _E009;

		public byte _E00A;

		public unsafe fixed ushort _E00B[1];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct _E004
	{
		public _E000 _E000;

		public byte _E001;

		public byte _E002;

		public byte _E003;

		public byte _E004;

		public ulong Id;

		public byte _E005;

		public byte _E006;

		public ushort _E007;

		public ushort _E008;

		public ushort _E009;

		public byte _E00A;

		public byte _E00B;

		public ushort _E00C;

		public ushort _E00D;

		public ushort _E00E;

		public byte _E00F;

		public byte _E010;

		public byte _E011;

		public byte _E012;

		public byte _E013;

		public byte _E014;

		public ushort _E015;

		public ushort _E016;

		public ushort _E017;

		public ushort _E018;

		public ushort _E019;
	}

	private unsafe delegate _E001 _E005<_E001>(_E000* header);

	private readonly ILogger<_E015> m__E000;

	[DllImport("kernel32.dll", EntryPoint = "GetSystemFirmwareTable", SetLastError = true)]
	private unsafe static extern int _E000(SystemFirmwareTableProviderSignature firmwareTableProviderSignature, uint firmwareTableID, byte* pFirmwareTableBuffer, int bufferSize);

	public _E015(ILogger<_E015> logger)
	{
		this.m__E000 = logger;
	}

	public unsafe SmBiosInfo GetBiosInfo()
	{
		return _E000<SmBiosInfo>(SystemFirmwareTableType.BiosInformation, _E000);
	}

	public unsafe SmSystemInfo GetSystemInfo()
	{
		return _E000<SmSystemInfo>(SystemFirmwareTableType.SystemInformation, this._E000);
	}

	public unsafe SmBaseboardInfo GetBaseboardInfo()
	{
		return _E000<SmBaseboardInfo>(SystemFirmwareTableType.BaseboardInformation, this._E000);
	}

	public unsafe SmProcessorInfo GetProcessorInfo()
	{
		return _E000<SmProcessorInfo>(SystemFirmwareTableType.ProcessorInformation, this._E000);
	}

	public unsafe SmSystemFirmwareInfo GetSystemFirmwareInfo()
	{
		try
		{
			int num = _E000(SystemFirmwareTableProviderSignature.RSMB, 0u, null, 0);
			byte* ptr = stackalloc byte[(int)(uint)num];
			int num2 = _E000(SystemFirmwareTableProviderSignature.RSMB, 0u, ptr, num);
			if (num2 >= sizeof(SystemFirmwareRawData) && num2 == num && ptr != null)
			{
				SystemFirmwareRawData* ptr2 = (SystemFirmwareRawData*)ptr;
				return new SmSystemFirmwareInfo
				{
					Version = string.Format(_E05B._E000(23708), ptr2->SMBIOSMajorVersion, ptr2->SMBIOSMinorVersion)
				};
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			this.m__E000.LogError(_E05B._E000(23716), lastWin32Error);
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(23780));
		}
		return null;
	}

	private unsafe _E001 _E000<_E001>(SystemFirmwareTableType type, _E005<_E001> parser)
	{
		try
		{
			int num = _E000(SystemFirmwareTableProviderSignature.RSMB, 0u, null, 0);
			byte* ptr = stackalloc byte[(int)(uint)num];
			int num2 = _E000(SystemFirmwareTableProviderSignature.RSMB, 0u, ptr, num);
			if (num2 < sizeof(_E000) || num2 != num || ptr == null)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				this.m__E000.LogError(_E05B._E000(23716), lastWin32Error);
			}
			else
			{
				SystemFirmwareRawData* ptr2 = (SystemFirmwareRawData*)ptr;
				byte* ptr3 = ptr2->SMBIOSTableData;
				while (ptr3 < ptr2->SMBIOSTableData + ptr2->Length)
				{
					_E000* ptr4 = (_E000*)ptr3;
					if (ptr4->_E001 < 4)
					{
						break;
					}
					if (ptr4->_E000 == type)
					{
						return parser(ptr4);
					}
					byte* ptr5;
					for (ptr5 = ptr3 + (int)ptr4->_E001; ptr5 < ptr2->SMBIOSTableData + ptr2->Length && (*ptr5 != 0 || ptr5[1] != 0); ptr5++)
					{
					}
					ptr5 += 2;
					ptr3 = ptr5;
				}
				this.m__E000.LogWarning(_E05B._E000(23752), type.ToString());
			}
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(23309), type.ToString());
		}
		return default(_E001);
	}

	private unsafe SmBiosInfo _E000(_E000* header)
	{
		_ = *header;
		return new SmBiosInfo
		{
			Vendor = _E000(header, ((_E001*)header)->_E001),
			Version = _E000(header, ((_E001*)header)->_E002),
			ReleaseDate = _E000(header, ((_E001*)header)->_E004)
		};
	}

	private unsafe SmSystemInfo _E000(_E000* header)
	{
		if (header->_E000 != SystemFirmwareTableType.SystemInformation)
		{
			throw new Exception(_E05B._E000(23378));
		}
		if (header->_E001 < 25)
		{
			throw new Exception(_E05B._E000(23439));
		}
		SmSystemInfo smSystemInfo = new SmSystemInfo
		{
			Manufacturer = _E000(header, ((_E002*)header)->_E001),
			ProductName = _E000(header, ((_E002*)header)->_E002),
			Version = _E000(header, ((_E002*)header)->_E003),
			SerialNumber = _E000(header, ((_E002*)header)->_E004)
		};
		string text;
		if (header->_E001 <= 8)
		{
			Guid empty = Guid.Empty;
			text = empty.ToString();
		}
		else
		{
			text = ((Guid*)(&((_E002*)header)->_E005._E000))->ToString();
		}
		smSystemInfo.Uuid = text.ToUpperInvariant();
		if (header->_E001 > 25)
		{
			smSystemInfo.SkuNumber = _E000(header, ((_E002*)header)->_E007);
			smSystemInfo.Family = _E000(header, ((_E002*)header)->_E008);
		}
		return smSystemInfo;
	}

	private unsafe SmBaseboardInfo _E000(_E000* header)
	{
		return new SmBaseboardInfo
		{
			Manufacturer = _E000(header, ((_E003*)header)->_E001),
			ProductName = _E000(header, ((_E003*)header)->_E002),
			Version = _E000(header, ((_E003*)header)->_E003),
			SerialNumber = _E000(header, ((_E003*)header)->_E004)
		};
	}

	private unsafe SmProcessorInfo _E000(_E000* header)
	{
		SmProcessorInfo smProcessorInfo = new SmProcessorInfo
		{
			Manufacturer = _E000(header, ((_E004*)header)->_E004),
			Id = ((_E004*)header)->Id,
			Version = _E000(header, ((_E004*)header)->_E005)
		};
		if (header->_E001 > 32)
		{
			smProcessorInfo.SerialNumber = _E000(header, ((_E004*)header)->_E00F);
			smProcessorInfo.PartNumber = _E000(header, ((_E004*)header)->_E011);
		}
		return smProcessorInfo;
	}

	private unsafe string _E000(_E000* header, byte substringNumber)
	{
		sbyte* ptr = (sbyte*)((byte*)header + (int)header->_E001);
		sbyte* ptr2 = ptr;
		while (substringNumber-- > 0)
		{
			if (ptr < ptr2)
			{
				ptr = ptr2 + 1;
			}
			ptr2 = _E000(ptr);
		}
		int length = (int)(ptr2 - ptr);
		return new string(ptr, 0, length, Encoding.ASCII);
	}

	private unsafe sbyte* _E000(sbyte* startPosition)
	{
		sbyte* ptr;
		for (ptr = startPosition; *ptr != 0; ptr++)
		{
		}
		return ptr;
	}
}
internal class _E016 : IInformationCollectionService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string _E000;

		internal bool _E000(ManagementObject mo)
		{
			return ((string)mo.GetPropertyValue(_E05B._E000(9863))).Contains(this._E000);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E016 _E000;

		public Action<int> _E001;

		public List<ServerAvailabilityInfo> _E002;

		public int _E003;

		public ICollection<IServerData> _E004;

		internal void _E000(IServerData server)
		{
			int num = -1;
			TracertEntry[] traceRoute = null;
			try
			{
				PingReply pingReply = new Ping().Send(server.HostNameOrIpAddress, this._E000._settingsService.PingTimeout);
				num = (int)((pingReply.Status == IPStatus.Success) ? pingReply.RoundtripTime : (-1));
				if (server.ServerType == ServerType.Backend)
				{
					traceRoute = this._E000._E000(server.HostNameOrIpAddress, this._E000._settingsService.TracertMaxHops, this._E000._settingsService.TracertTimeout).ToArray();
				}
			}
			catch
			{
				this._E000.m__E000.LogWarning(_E05B._E000(9869), server.HostNameOrIpAddress);
				num = -2;
			}
			_E002.Add(new ServerAvailabilityInfo(server.HostNameOrIpAddress, num, traceRoute));
			Interlocked.Increment(ref _E003);
			_E001?.Invoke(_E003 * 100 / _E004.Count);
		}
	}

	private readonly Microsoft.Extensions.Logging.ILogger m__E000;

	private readonly ISettingsService _settingsService;

	private readonly Lazy<IGameBackendService> m__E001;

	private readonly ISystemManagement _E002;

	private SystemInfo _E003;

	private readonly object _E004 = new object();

	[DllImport("iphlpapi", EntryPoint = "GetBestInterface")]
	private static extern int _E000(uint dwDestAddr, ref uint pdwBestIfIndex);

	[DllImport("kernel32.dll", EntryPoint = "GetPhysicallyInstalledSystemMemory")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool _E000(out long totalMemoryInKilobytes);

	public _E016(Lazy<IGameBackendService> gameBackendServiceLazy, ISystemManagement systemManagement, ISettingsService settingsService, ILogger<_E016> logger)
	{
		this.m__E000 = logger;
		_settingsService = settingsService;
		this.m__E001 = gameBackendServiceLazy;
		this._E002 = systemManagement;
	}

	public SystemInfo GetSystemInfo()
	{
		lock (_E004)
		{
			if (this._E003 != null)
			{
				return this._E003;
			}
			try
			{
				this.m__E000.LogDebug(_E05B._E000(23546));
				SystemInfo systemInfo = new SystemInfo();
				systemInfo.MachineName = Environment.MachineName;
				systemInfo.Baseboard = _E000<BaseboardInfo>(_E05B._E000(23505)).FirstOrDefault() ?? new BaseboardInfo();
				systemInfo.Bios = _E000<BiosInfo>(_E05B._E000(23073)).FirstOrDefault() ?? new BiosInfo();
				systemInfo.Bios.Uuid = this._E000(_E05B._E000(23086))?.FirstOrDefault()?.GetPropertyValue(_E05B._E000(23065)) as string;
				systemInfo.Cpu = _E000<CpuInfo>(_E05B._E000(23068));
				systemInfo.Gpu = _E000<GpuInfo>(_E05B._E000(23148));
				foreach (GpuInfo item in systemInfo.Gpu)
				{
					item.AdapterRamGb = (uint)Math.Round((double)item.AdapterRam / 1024.0 / 1024.0 / 1024.0);
				}
				systemInfo.Nic = from _ in _E000<NicInfo>(_E05B._E000(23110))
					where _.PhysicalAdapter && _.InterfaceIndex < 32
					select _;
				int num = this._E000();
				foreach (NicInfo item2 in systemInfo.Nic)
				{
					item2.IsCurrent = item2.InterfaceIndex == num;
				}
				systemInfo.Os = _E000<OsInfo>(_E05B._E000(23129)).FirstOrDefault() ?? new OsInfo();
				systemInfo.Os.Language = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
				systemInfo.Os.InstallationTimestamp = this._E000();
				systemInfo.Ram = new RamInfo
				{
					Modules = _E000<RamModuleInfo>(_E05B._E000(23219))
				};
				systemInfo.Ram.TotalSizeGb = (uint)Math.Round(systemInfo.Ram.Modules.Sum((Func<RamModuleInfo, double>)((RamModuleInfo m) => m.Capacity)) / 1024.0 / 1024.0 / 1024.0);
				if (systemInfo.Ram.TotalSizeGb == 0 && _E000(out var totalMemoryInKilobytes))
				{
					systemInfo.Ram.TotalSizeGb = (uint)(totalMemoryInKilobytes / 1024 / 1024);
				}
				systemInfo.Storage = _E000<StorageInfo>(_E05B._E000(23174));
				int num2 = this._E000(Directory.GetCurrentDirectory());
				int num3 = this._E000(Environment.SystemDirectory);
				foreach (StorageInfo item3 in systemInfo.Storage)
				{
					item3.SizeGb = (uint)Math.Round((double)item3.Size / 1024.0 / 1024.0 / 1024.0);
					item3.IsLauncherInstalled = item3.Index == num2;
					item3.IsSystemDrive = item3.Index == num3;
				}
				systemInfo.SmSystemFirmware = this._E002.GetSystemFirmwareInfo();
				systemInfo.SmBios = this._E002.GetBiosInfo();
				systemInfo.SmSystem = this._E002.GetSystemInfo();
				systemInfo.SmBaseboard = this._E002.GetBaseboardInfo();
				systemInfo.SmProcessor = this._E002.GetProcessorInfo();
				systemInfo.UpdateChecksum();
				this.m__E000.LogDebug(_E05B._E000(23190));
				this._E003 = systemInfo;
				return systemInfo;
			}
			catch (Exception ex)
			{
				this.m__E000.LogError(ex, _E05B._E000(23284));
				throw new InformationCollectionServiceException(BsgExceptionCode.ErrorRetrievingSystemInformation, ex);
			}
		}
	}

	private int _E000(string path)
	{
		try
		{
			string value = Path.GetPathRoot(path).TrimEnd('\\');
			string input = (string)this._E000(_E05B._E000(23254))?.FirstOrDefault((ManagementObject mo) => ((string)mo.GetPropertyValue(_E05B._E000(9863))).Contains(value))?.GetPropertyValue(_E05B._E000(22800));
			Match match = new Regex(_E05B._E000(22813)).Match(input);
			if (!match.Success)
			{
				throw new Exception("");
			}
			return int.Parse(match.Value);
		}
		catch (Exception exception)
		{
			this.m__E000.LogWarning(exception, _E05B._E000(22894));
			return -1;
		}
	}

	public string GetHwIdV1()
	{
		SystemInfo systemInfo = GetSystemInfo();
		using SHA1 sHA = SHA1.Create();
		long num = DateTime.UtcNow.ToUnixTimeStamp() / 1000000;
		CpuInfo cpuInfo = systemInfo.Cpu.FirstOrDefault();
		string[] array = new string[7]
		{
			null,
			null,
			(systemInfo.Bios.Manufacturer + systemInfo.Bios.Name + systemInfo.Bios.SerialNumber).GetHash(sHA).ToHex(),
			(systemInfo.Baseboard.Manufacturer + systemInfo.Baseboard.Name + systemInfo.Baseboard.Product + systemInfo.Baseboard.SerialNumber).GetHash(sHA).ToHex(),
			(cpuInfo?.Manufacturer + cpuInfo?.Name + cpuInfo?.SerialNumber + cpuInfo?.UniqueId).GetHash(sHA).ToHex(),
			(systemInfo.Os.Manufacturer + systemInfo.Os.SerialNumber).GetHash(sHA).ToHex(),
			this._E000()
		};
		array[0] = _E05B._E000(22864);
		string s = string.Concat(num.ToString(), string.Concat(array));
		array[1] = sHA.ComputeHash(Encoding.UTF8.GetBytes(s)).ToHex();
		return string.Join(_E05B._E000(22869), array);
	}

	private string _E000()
	{
		SystemInfo systemInfo = GetSystemInfo();
		try
		{
			using SHA1 sHA = SHA1.Create();
			return sHA.ComputeHash(Encoding.UTF8.GetBytes(systemInfo.Baseboard.SerialNumber + systemInfo.Bios.SerialNumber + systemInfo.Cpu.FirstOrDefault()?.UniqueId + systemInfo.Os.SerialNumber)).ToHex().GetMd5();
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(22875));
			return null;
		}
	}

	private ulong _E000()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(_E05B._E000(22919), writable: false);
			string value = registryKey.GetValue(_E05B._E000(23026))?.ToString();
			if (!string.IsNullOrWhiteSpace(value))
			{
				return Convert.ToUInt64(value);
			}
			this.m__E000.LogWarning(_E05B._E000(23038));
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(22568));
		}
		return 0uL;
	}

	private int _E000()
	{
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(_settingsService.LauncherBackendUri.Host);
			uint pdwBestIfIndex = uint.MaxValue;
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress in array)
			{
				int num = _E000(BitConverter.ToUInt32(iPAddress.GetAddressBytes(), 0), ref pdwBestIfIndex);
				if (num == 0)
				{
					break;
				}
				this.m__E000.LogWarning(_E05B._E000(22555), _settingsService.LauncherBackendUri.Host, iPAddress.ToString(), num);
			}
			if (pdwBestIfIndex != uint.MaxValue)
			{
				return (int)pdwBestIfIndex;
			}
			this.m__E000.LogWarning(_E05B._E000(22702), _settingsService.LauncherBackendUri.Host, hostAddresses);
		}
		catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
		{
			this.m__E000.LogWarning(_E05B._E000(22765));
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(10026));
		}
		return -1;
	}

	public async Task<IReadOnlyCollection<ServerAvailabilityInfo>> GetServersAvailabilityInfo(Action<int> onProgress = null)
	{
		try
		{
			this.m__E000.LogDebug(_E05B._E000(9929));
			onProgress?.Invoke(0);
			List<ServerAvailabilityInfo> list = new List<ServerAvailabilityInfo>();
			ICollection<IServerData> servers = (await this.m__E001.Value.GetListOfServers()).Servers;
			int location = 0;
			Parallel.ForEach(servers, delegate(IServerData server)
			{
				int num = -1;
				TracertEntry[] traceRoute = null;
				try
				{
					PingReply pingReply = new Ping().Send(server.HostNameOrIpAddress, _settingsService.PingTimeout);
					num = (int)((pingReply.Status == IPStatus.Success) ? pingReply.RoundtripTime : (-1));
					if (server.ServerType == ServerType.Backend)
					{
						traceRoute = _E000(server.HostNameOrIpAddress, _settingsService.TracertMaxHops, _settingsService.TracertTimeout).ToArray();
					}
				}
				catch
				{
					this.m__E000.LogWarning(_E05B._E000(9869), server.HostNameOrIpAddress);
					num = -2;
				}
				list.Add(new ServerAvailabilityInfo(server.HostNameOrIpAddress, num, traceRoute));
				Interlocked.Increment(ref location);
				onProgress?.Invoke(location * 100 / servers.Count);
			});
			this.m__E000.LogDebug(_E05B._E000(9477));
			return list;
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(9554));
			return null;
		}
	}

	private IEnumerable<TracertEntry> _E000(string ipAddressOrHostName, int maxHops, int timeout)
	{
		if (maxHops < 1)
		{
			throw new ArgumentException(_E05B._E000(9611));
		}
		if (timeout < 1)
		{
			throw new ArgumentException(_E05B._E000(9707));
		}
		Ping ping = new Ping();
		PingOptions pingOptions = new PingOptions(1, dontFragment: true);
		Stopwatch stopwatch = new Stopwatch();
		PingReply pingReply;
		do
		{
			stopwatch.Start();
			pingReply = ping.Send(ipAddressOrHostName, timeout, new byte[1], pingOptions);
			stopwatch.Stop();
			string hostname = string.Empty;
			if (pingReply.Address != null)
			{
				try
				{
					hostname = Dns.GetHostEntry(pingReply.Address).HostName;
				}
				catch (SocketException)
				{
				}
			}
			yield return new TracertEntry
			{
				HopID = pingOptions.Ttl,
				Address = ((pingReply.Address == null) ? _E05B._E000(9678) : pingReply.Address.ToString()),
				Hostname = hostname,
				ReplyTime = stopwatch.ElapsedMilliseconds,
				ReplyStatus = pingReply.Status
			};
			pingOptions.Ttl++;
			stopwatch.Reset();
		}
		while (pingReply.Status != 0 && pingOptions.Ttl <= maxHops);
	}

	private ManagementObject[] _E000(string wmiRequest)
	{
		ManagementObject[] result = null;
		try
		{
			result = new ManagementObjectSearcher(wmiRequest).Get().Cast<ManagementObject>().ToArray();
			return result;
		}
		catch (ManagementException ex)
		{
			LogLevel logLevel = ((ex.ErrorCode == ManagementStatus.NotFound || ex.ErrorCode == ManagementStatus.InvalidNamespace) ? LogLevel.Information : LogLevel.Error);
			this.m__E000.Log(logLevel, null, _E05B._E000(9999), wmiRequest, ex.ErrorCode);
			return result;
		}
		catch (COMException ex2)
		{
			this.m__E000.LogError(_E05B._E000(10063), wmiRequest, ex2.ErrorCode);
			return result;
		}
		catch (Exception ex3)
		{
			this.m__E000.LogError(_E05B._E000(10141), wmiRequest, ex3.GetType(), ex3.Message);
			return result;
		}
	}

	private IReadOnlyCollection<_E002> _E000<_E002>(string wmiClass) where _E002 : class, new()
	{
		string text = _E05B._E000(9778) + wmiClass;
		ManagementObject[] array = this._E000(text);
		List<_E002> list = new List<_E002>();
		Type typeFromHandle = typeof(_E002);
		try
		{
			if (array != null)
			{
				if (array.Length != 0)
				{
					PropertyInfo[] properties = typeFromHandle.GetProperties();
					ManagementObject[] array2 = array;
					foreach (ManagementObject managementObject in array2)
					{
						_E002 val = new _E002();
						PropertyInfo[] array3 = properties;
						foreach (PropertyInfo propertyInfo in array3)
						{
							foreach (PropertyData property in managementObject.Properties)
							{
								if (property.Name.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase))
								{
									propertyInfo.SetValue(val, property.Value);
									break;
								}
							}
						}
						list.Add(val);
					}
					return list;
				}
				this.m__E000.LogWarning(_E05B._E000(9731), text);
				return list;
			}
			return list;
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(9793), text, typeFromHandle.Name);
			return list;
		}
	}
}
internal class _E017 : IClientLogService, IService
{
	private readonly IGameService m__E000;

	private readonly ISettingsService _settingsService;

	private readonly ICompressionService _compressionService;

	private readonly IHttpClientBuilder _E001;

	private readonly ILogger<_E017> _E002;

	private readonly SemaphoreSlim _E003 = new SemaphoreSlim(1, 1);

	private HttpClient _E004;

	public _E017(IGameService gameService, ISettingsService settingsService, ICompressionService compressionService, IHttpClientBuilder httpClientBuilder, ILogger<_E017> logger)
	{
		this.m__E000 = gameService;
		_settingsService = settingsService;
		_compressionService = compressionService;
		this._E001 = httpClientBuilder;
		_E002 = logger;
		this._E001 = httpClientBuilder.AddExceptionHandling(ensureSuccessStatusCodes: true).AddRequestMetadata(addLanguage: true, addBranchName: true, addGameVersion: true).AddAuthentication()
			.AddLogging<_E017>()
			.AddApiResponses(_E05B._E000(21599), _E05B._E000(21666), _E05B._E000(21670), _E05B._E000(21673))
			.AddJsonResponses();
	}

	public void OnAwake()
	{
		this.m__E000.OnGameClosedAsync += _E000;
		_settingsService.OnUserProfileLoaded += _E000;
		_settingsService.OnBranchChanged += OnBranchChanged;
	}

	public void OnStop()
	{
		this.m__E000.OnGameClosedAsync -= _E000;
		_settingsService.OnUserProfileLoaded -= _E000;
		_settingsService.OnBranchChanged -= OnBranchChanged;
	}

	private async Task _E000()
	{
		if (!Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
		{
			return;
		}
		await _E003.WaitAsync();
		try
		{
			if (_settingsService.LastSendingLogsTime > DateTime.Now)
			{
				_settingsService.LastSendingLogsTime = DateTime.Now;
			}
			if (_settingsService.LastSendingLogsTime + TimeSpan.FromDays(1.0) < DateTime.Now)
			{
				_settingsService.LastSendingLogsTime = DateTime.Now;
			}
			string path = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _E05B._E000(25496));
			if (!Directory.Exists(path))
			{
				return;
			}
			KeyValuePair<DateTime, string>[] array = (from dir in Directory.EnumerateDirectories(path)
				select new KeyValuePair<DateTime, string>(new DirectoryInfo(dir).CreationTime, dir) into kvp
				where kvp.Key > _settingsService.LastSendingLogsTime
				orderby kvp.Key
				select kvp).ToArray();
			if (array.Length == 0)
			{
				return;
			}
			_E002.LogDebug(_E05B._E000(11713));
			int num = _settingsService.MaxUncompressedLogPackedSizeMb * 1024 * 1024;
			KeyValuePair<DateTime, string>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				KeyValuePair<DateTime, string> keyValuePair = array2[i];
				string[] source = Directory.EnumerateFiles(keyValuePair.Value, _settingsService.SelectedBranch.IsDefault ? _E05B._E000(11732) : _E05B._E000(27034)).ToArray();
				if (!source.Any())
				{
					continue;
				}
				long num2 = source.Sum((string df) => new FileInfo(df).Length);
				if (num2 < num)
				{
					using Stream content = _compressionService.CreateZip(source.ToDictionary((string fts) => fts, (string fts) => ""));
					using StreamContent content2 = new StreamContent(content);
					await _E000(_E05B._E000(11299), content2);
				}
				else
				{
					_E002.LogWarning(_E05B._E000(11324), keyValuePair.Value, num2 / 1024 / 1024);
				}
			}
			_E002.LogDebug(_E05B._E000(11426));
		}
		catch (HttpNetworkException ex) when (ex.Response.StatusCode == HttpStatusCode.Forbidden)
		{
			_E002.LogDebug(_E05B._E000(11454));
		}
		catch (Exception exc)
		{
			_E002.Exception(exc, _E05B._E000(11516));
		}
		finally
		{
			_settingsService.LastSendingLogsTime = DateTime.Now;
			_settingsService.Save();
			_E003.Release();
		}
	}

	private async Task<JToken> _E000(string requestUri, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
		{
			Content = content
		};
		HttpResponseMessage httpResponseMessage = await _E004.SendAsync(request, cancellationToken);
		if (!(httpResponseMessage.Content is ApiContent apiContent))
		{
			throw new WrongResponseHttpNetworkException(httpResponseMessage);
		}
		if (apiContent.Code != 0)
		{
			throw new ApiNetworkException(httpResponseMessage, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args);
		}
		return apiContent.Data;
	}

	private Task _E000(ProcessLifecycleInformation processLifecycleInfo)
	{
		return _E000();
	}

	private void _E000(object sender, EventArgs e)
	{
		_E000(_settingsService.SelectedBranch.LogsUri);
	}

	private void OnBranchChanged(object sender, OnBranchChangedEventArgs e)
	{
		_E000(e.NewBranch.LogsUri);
	}

	private void _E000(Uri baseAddress)
	{
		_E004 = this._E001.WithBaseAddress(baseAddress).Build();
	}

	[CompilerGenerated]
	private bool _E000(KeyValuePair<DateTime, string> kvp)
	{
		return kvp.Key > _settingsService.LastSendingLogsTime;
	}
}
internal class _E018 : IGameBackendService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E018 _E000;

		public ISettingsService settingsService;

		internal void _E000(object s, EventArgs eArgs)
		{
			this._E000._E000(settingsService.SelectedBranch.GameBackendUri);
		}

		internal void _003C_002Ector_003Eb__1(object s, OnBranchChangedEventArgs eArgs)
		{
			this._E000._E000(eArgs.NewBranch.GameBackendUri);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public List<string> _E000;

		public JToken _E001;

		internal DatacenterDto _E000(JToken dc)
		{
			IPAddress address;
			return new DatacenterDto
			{
				Name = dc[_E05B._E000(10609)].Value<string>(),
				Region = dc[_E05B._E000(10622)].Value<string>(),
				IpAddresses = (from ipStr in dc[_E05B._E000(10567)].Values<string>()
					select (!IPAddress.TryParse(ipStr, out address)) ? null : address into ip
					where ip != null
					select ip).Distinct().ToArray(),
				IsSelected = this._E000.Contains(dc[_E05B._E000(10609)].Value<string>()),
				AvgWaitTime = (dc[_E05B._E000(10564)]?.Value<int>() ?? (-1)),
				MaxPingTime = (dc.SelectToken(_E05B._E000(10583) + dc[_E05B._E000(10622)].Value<string>())?.Value<int?>() ?? _E001[_E05B._E000(10656)]?.Value<int?>() ?? 300)
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _E018 _E000;

		public string _E001;

		internal Task<JToken> _E000()
		{
			return this._E000._E000(HttpMethod.Post, _E05B._E000(10678), new StringContent(_E001));
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _E018 _E000;

		public JsonContent _E001;

		internal async Task<GameSessionInfo> _E000()
		{
			return await this._E000._E000<GameSessionInfo>(HttpMethod.Post, _E05B._E000(10638), _E001);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public string _E000;

		public _E018 _E001;

		public IBranch branch;

		internal bool _003CActivateCode_003Eb__1(IBranch b)
		{
			return this._E000.StartsWith(b.Name, StringComparison.InvariantCultureIgnoreCase);
		}

		internal Task<JToken> _E000()
		{
			return _E001._E000(HttpMethod.Post, new Uri(branch.GameBackendUri, _E05B._E000(10713)).ToString(), new JsonContent(new JObject { 
			{
				_E05B._E000(21599),
				this._E000
			} }));
		}
	}

	private readonly IInformationCollectionService m__E000;

	private readonly ILogger<_E01A> _E001;

	private readonly ISettingsService _settingsService;

	private readonly JsonSerializer m__E002;

	private readonly IHttpClientBuilder m__E003;

	private HttpClient m__E004;

	public _E018(IHttpClientBuilder httpClientBuilder, IInformationCollectionService informationCollectionService, ILogger<_E01A> logger, ISettingsService settingsService, JsonSerializer jsonSerializer)
	{
		_E018 obj = this;
		this.m__E000 = informationCollectionService;
		this._E001 = logger;
		_settingsService = settingsService;
		this.m__E002 = jsonSerializer;
		this.m__E003 = httpClientBuilder.AddExceptionHandling(ensureSuccessStatusCodes: true).AddRequestMetadata().AddAuthentication()
			.AddLogging<_E018>()
			.AddApiResponses(_E05B._E000(10916), _E05B._E000(10920), _E05B._E000(21670), _E05B._E000(21673))
			.AddJsonResponses()
			.AddCompression()
			.WithBaseAddress(_settingsService.LauncherBackendUri);
		settingsService.OnUserProfileLoaded += delegate
		{
			obj._E000(settingsService.SelectedBranch.GameBackendUri);
		};
		settingsService.OnBranchChanged += delegate(object s, OnBranchChangedEventArgs eArgs)
		{
			obj._E000(eArgs.NewBranch.GameBackendUri);
		};
	}

	private void _E000(Uri baseAddress)
	{
		this.m__E004 = this.m__E003.WithBaseAddress(baseAddress).Build();
	}

	public async Task<IListOfServersResponseData> GetListOfServers()
	{
		IListOfServersResponseData listOfServersResponseData;
		try
		{
			listOfServersResponseData = await this._E001.WithExceptionsHandling(_E05B._E000(10594), () => _E000<ListOfServersResponseData>(HttpMethod.Post, _E05B._E000(10530)));
		}
		catch
		{
			listOfServersResponseData = new ListOfServersResponseData();
		}
		listOfServersResponseData.Servers.Add(new ServerData(_settingsService.LauncherBackendUri.Host, _settingsService.LauncherBackendUri.Port, ServerType.Backend));
		if (_settingsService.SelectedBranch.GameBackendUri != null)
		{
			listOfServersResponseData.Servers.Add(new ServerData(_settingsService.SelectedBranch.GameBackendUri.Host, _settingsService.SelectedBranch.GameBackendUri.Port, ServerType.Backend));
		}
		listOfServersResponseData.Servers.Add(new ServerData(_settingsService.SelectedBranch.TradingBackendUri.Host, _settingsService.SelectedBranch.TradingBackendUri.Port, ServerType.Backend));
		return listOfServersResponseData;
	}

	public Task<DatacenterDto[]> GetDatacenters()
	{
		return this._E001.WithExceptionsHandling(_E05B._E000(10929), async delegate
		{
			JToken jToken = await _E000(HttpMethod.Post, _E05B._E000(10293));
			List<string> list = jToken[_E05B._E000(10252)].Values<string>().ToList();
			IPAddress address;
			return (jToken[_E05B._E000(10337)] as JArray).Select((JToken dc) => new DatacenterDto
			{
				Name = dc[_E05B._E000(10609)].Value<string>(),
				Region = dc[_E05B._E000(10622)].Value<string>(),
				IpAddresses = (from ipStr in dc[_E05B._E000(10567)].Values<string>()
					select (!IPAddress.TryParse(ipStr, out address)) ? null : address into ip
					where ip != null
					select ip).Distinct().ToArray(),
				IsSelected = list.Contains(dc[_E05B._E000(10609)].Value<string>()),
				AvgWaitTime = (dc[_E05B._E000(10564)]?.Value<int>() ?? (-1)),
				MaxPingTime = (dc.SelectToken(_E05B._E000(10583) + dc[_E05B._E000(10622)].Value<string>())?.Value<int?>() ?? jToken[_E05B._E000(10656)]?.Value<int?>() ?? 300)
			}).ToArray();
		});
	}

	public Task SetMatchingConfiguration(string matchingConfiguration)
	{
		return this._E001.WithExceptionsHandling(_E05B._E000(10881), () => _E000(HttpMethod.Post, _E05B._E000(10678), new StringContent(matchingConfiguration)));
	}

	public Task<PlayerProfile> GetPlayerProfile()
	{
		return this._E001.WithExceptionsHandling(_E05B._E000(10910), () => _E000<PlayerProfile>(HttpMethod.Post, _E05B._E000(10549)));
	}

	public async Task<GameSessionInfo> GetGameSession(BsgVersion gameVersion, string branchName)
	{
		string hwIdV = this.m__E000.GetHwIdV1();
		SystemInfo systemInfo = this.m__E000.GetSystemInfo();
		JsonContent content = new JsonContent(new JObject
		{
			{
				_E05B._E000(10722),
				new JObject
				{
					{
						_E05B._E000(10730),
						gameVersion.ToString()
					},
					{
						_E05B._E000(10732),
						branchName
					},
					{
						_E05B._E000(10743),
						_E05B._E000(10751)
					}
				}
			},
			{
				_E05B._E000(10749),
				hwIdV
			},
			{
				_E05B._E000(21407),
				JObject.FromObject(systemInfo, this.m__E002)
			}
		});
		return await this._E001.WithExceptionsHandling(_E05B._E000(10694), async () => await _E000<GameSessionInfo>(HttpMethod.Post, _E05B._E000(10638), content));
	}

	public Task CancelQueueWaiting()
	{
		return this._E001.WithExceptionsHandling(_E05B._E000(10993), () => _E000(HttpMethod.Post, _E05B._E000(10504))).ContinueWith((Task<JToken> t) => Task.CompletedTask);
	}

	public Task<JToken> ActivateCode(string activationCode)
	{
		IBranch branch = _settingsService.Branches.Where((IBranch b) => b.IsActive).FirstOrDefault((IBranch b) => activationCode.StartsWith(b.Name, StringComparison.InvariantCultureIgnoreCase)) ?? _settingsService.Branches.First((IBranch b) => b.IsDefault);
		return this._E001.WithExceptionsHandling(_E05B._E000(10948), () => _E000(HttpMethod.Post, new Uri(branch.GameBackendUri, _E05B._E000(10713)).ToString(), new JsonContent(new JObject { 
		{
			_E05B._E000(21599),
			activationCode
		} })));
	}

	public override string ToString()
	{
		return this.m__E004.BaseAddress.ToString();
	}

	private async Task<_E003> _E000<_E003>(HttpMethod method, string requestUri, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		JToken jToken = await _E000(method, requestUri, content, cancellationToken);
		return (jToken == null) ? default(_E003) : ApiResponse.ConvertData<_E003>(jToken, this.m__E002);
	}

	private async Task<JToken> _E000(HttpMethod method, string requestUri, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = new HttpRequestMessage(method, requestUri)
		{
			Content = content
		};
		using HttpResponseMessage httpResponseMessage = await this.m__E004.SendAsync(request, cancellationToken);
		using ApiContent apiContent = ApiResponse.GetApiContent(httpResponseMessage);
		if (apiContent.Code != 0)
		{
			throw new ApiNetworkException(httpResponseMessage, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args);
		}
		return apiContent.Data;
	}

	[CompilerGenerated]
	private Task<ListOfServersResponseData> _E000()
	{
		return _E000<ListOfServersResponseData>(HttpMethod.Post, _E05B._E000(10530));
	}

	[CompilerGenerated]
	private async Task<DatacenterDto[]> _E000()
	{
		JToken jToken = await _E000(HttpMethod.Post, _E05B._E000(10293));
		List<string> list = jToken[_E05B._E000(10252)].Values<string>().ToList();
		IPAddress address;
		return (jToken[_E05B._E000(10337)] as JArray).Select((JToken dc) => new DatacenterDto
		{
			Name = dc[_E05B._E000(10609)].Value<string>(),
			Region = dc[_E05B._E000(10622)].Value<string>(),
			IpAddresses = (from ipStr in dc[_E05B._E000(10567)].Values<string>()
				select (!IPAddress.TryParse(ipStr, out address)) ? null : address into ip
				where ip != null
				select ip).Distinct().ToArray(),
			IsSelected = list.Contains(dc[_E05B._E000(10609)].Value<string>()),
			AvgWaitTime = (dc[_E05B._E000(10564)]?.Value<int>() ?? (-1)),
			MaxPingTime = (dc.SelectToken(_E05B._E000(10583) + dc[_E05B._E000(10622)].Value<string>())?.Value<int?>() ?? jToken[_E05B._E000(10656)]?.Value<int?>() ?? 300)
		}).ToArray();
	}

	[CompilerGenerated]
	private Task<JToken> _E000()
	{
		return _E000(HttpMethod.Post, _E05B._E000(10504));
	}
}
internal class _E019 : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(ListOfServersResponseData);
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		ListOfServersResponseData listOfServersResponseData = new ListOfServersResponseData();
		if (reader != null && reader.TokenType == JsonToken.StartArray)
		{
			foreach (JToken item in JArray.Load(reader))
			{
				string hostNameOrIpAddress = item[_E05B._E000(10567)].Value<string>();
				int port = item[_E05B._E000(10349)].Value<int>();
				listOfServersResponseData.Servers.Add(new ServerData(hostNameOrIpAddress, port, ServerType.GameServer));
			}
			return listOfServersResponseData;
		}
		return listOfServersResponseData;
	}
}
internal class _E01A : ILauncherBackendService
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public _E01A _E000;

		public string _E001;

		internal Task<GamePackageInfo> _E000()
		{
			return this._E000._E000<GamePackageInfo>(HttpMethod.Get, _E05B._E000(14097) + _E001);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public _E01A _E000;

		public JContainer _E001;

		internal Task<JToken> _E000()
		{
			return this._E000._E000(HttpMethod.Post, _E05B._E000(13832), new JsonContent(_E001));
		}
	}

	private readonly HttpClient m__E000;

	private readonly Microsoft.Extensions.Logging.ILogger m__E001;

	private readonly JsonSerializer _E002;

	public _E01A(IHttpClientBuilder httpClientBuilder, ISettingsService settingsService, ILogger<_E01A> logger, JsonSerializer jsonSerializer)
	{
		this.m__E000 = httpClientBuilder.AddExceptionHandling(ensureSuccessStatusCodes: true).AddRequestMetadata().AddAuthentication()
			.AddLogging<_E01A>()
			.AddApiResponses(_E05B._E000(10916), _E05B._E000(10920), _E05B._E000(21670), _E05B._E000(21673))
			.AddJsonResponses()
			.AddCompression()
			.WithBaseAddress(settingsService.LauncherBackendUri)
			.Build();
		this.m__E001 = logger;
		this._E002 = jsonSerializer;
	}

	public async Task<LauncherPackageInfo> GetLauncherPackage()
	{
		try
		{
			return await this.m__E001.WithExceptionsHandling(_E05B._E000(10449), () => _E000<LauncherPackageInfo>(HttpMethod.Get, _E05B._E000(10425)));
		}
		catch (ApiNetworkException ex) when (ex.ApiCode == 300)
		{
			this.m__E001.LogWarning(_E05B._E000(14131));
			return null;
		}
	}

	public async Task<GamePackageInfo> GetGamePackage(BsgVersion version = default(BsgVersion))
	{
		try
		{
			string text = ((version == default(BsgVersion)) ? "" : string.Format(_E05B._E000(14190), version));
			return await this.m__E001.WithExceptionsHandling(_E05B._E000(14201), () => _E000<GamePackageInfo>(HttpMethod.Get, _E05B._E000(14097) + text));
		}
		catch (ApiNetworkException ex) when (ex.ApiCode == 300)
		{
			if (version == default(BsgVersion))
			{
				this.m__E001.LogWarning(_E05B._E000(14167));
			}
			else
			{
				this.m__E001.LogWarning(_E05B._E000(14252), version);
			}
			return null;
		}
	}

	public async Task<IReadOnlyCollection<GameUpdateInfo>> GetGameUpdates()
	{
		try
		{
			return (await this.m__E001.WithExceptionsHandling(_E05B._E000(14308), () => _E000<List<GameUpdateInfo>>(HttpMethod.Get, _E05B._E000(10387)))) ?? new List<GameUpdateInfo>();
		}
		catch (ApiNetworkException ex) when (ex.ApiCode == 300)
		{
			this.m__E001.LogWarning(_E05B._E000(14335));
			return (IReadOnlyCollection<GameUpdateInfo>)(object)new GameUpdateInfo[0];
		}
	}

	public async Task<LauncherDistribResponseData> GetLauncherDistrib()
	{
		try
		{
			return await this.m__E001.WithExceptionsHandling(_E05B._E000(10449), () => _E000<LauncherDistribResponseData>(HttpMethod.Post, _E05B._E000(10469), null, skipAuth: true));
		}
		catch (ApiNetworkException ex) when (ex.ApiCode == 300)
		{
			this.m__E001.LogWarning(_E05B._E000(14299));
			return null;
		}
	}

	public Task<UserProfile> GetUserProfile()
	{
		return this.m__E001.WithExceptionsHandling(_E05B._E000(10352), () => _E000<UserProfile>(HttpMethod.Post, _E05B._E000(10433)));
	}

	public Task SendAnalytics(JContainer json)
	{
		return this.m__E001.WithExceptionsHandling(_E05B._E000(10311), () => _E000(HttpMethod.Post, _E05B._E000(13832), new JsonContent(json)));
	}

	public override string ToString()
	{
		return this.m__E000.BaseAddress.ToString();
	}

	private async Task<_E003> _E000<_E003>(HttpMethod httpMethod, string requestUri, HttpContent content = null, bool skipAuth = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		JToken jToken = await _E000(httpMethod, requestUri, content, skipAuth, cancellationToken);
		return (jToken == null) ? default(_E003) : ConvertData<_E003>(jToken);
	}

	private async Task<JToken> _E000(HttpMethod httpMethod, string requestUri, HttpContent content = null, bool skipAuth = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod, requestUri)
		{
			Content = content
		};
		if (skipAuth)
		{
			requestMessage.SkipAuth();
		}
		using HttpResponseMessage httpResponseMessage = await this.m__E000.SendAsync(requestMessage, cancellationToken);
		using ApiContent apiContent = _E000(httpResponseMessage);
		if (apiContent.Code != 0)
		{
			throw new ApiNetworkException(httpResponseMessage, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args);
		}
		return apiContent.Data;
	}

	private ApiContent _E000(HttpResponseMessage httpResponse)
	{
		return (httpResponse?.Content as ApiContent) ?? throw new Exception(string.Format(_E05B._E000(10320), httpResponse.RequestMessage.RequestUri));
	}

	private TData ConvertData<TData>(JToken jsonData)
	{
		if (jsonData != null)
		{
			if (jsonData is TData)
			{
				return (TData)(object)((jsonData is TData) ? jsonData : null);
			}
			return jsonData.ToObject<TData>(this._E002);
		}
		return default(TData);
	}

	[CompilerGenerated]
	private Task<LauncherPackageInfo> _E000()
	{
		return _E000<LauncherPackageInfo>(HttpMethod.Get, _E05B._E000(10425));
	}

	[CompilerGenerated]
	private Task<List<GameUpdateInfo>> _E000()
	{
		return _E000<List<GameUpdateInfo>>(HttpMethod.Get, _E05B._E000(10387));
	}

	[CompilerGenerated]
	private Task<LauncherDistribResponseData> _E000()
	{
		return _E000<LauncherDistribResponseData>(HttpMethod.Post, _E05B._E000(10469), null, skipAuth: true);
	}
}
internal sealed class _E01B : IAuthService
{
	[CompilerGenerated]
	private sealed class _E004
	{
		public _E01B _E000;

		public string _E001;

		public string _E002;

		internal Task<JToken> _E000()
		{
			string hwIdV = this._E000.m__E006.GetHwIdV1();
			SystemInfo systemInfo = this._E000.m__E006.GetSystemInfo();
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					_E001
				},
				{
					_E05B._E000(10749),
					hwIdV
				},
				{
					_E05B._E000(21407),
					JObject.FromObject(systemInfo, this._E000._E008)
				},
				{
					_E05B._E000(13245),
					_E002
				}
			});
			return this._E000._E000(_E05B._E000(13192), content, null, skipAuth: true, preventTokenUpdate: false, hasSecretRequestData: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public string _E000;

		public string _E001;

		public _E01B _E002;

		internal Task<JToken> _E000()
		{
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					this._E000
				},
				{
					_E05B._E000(13289),
					_E001
				}
			});
			return _E002._E000(_E05B._E000(13299), content, null, skipAuth: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public string _E000;

		public string _E001;

		public _E01B _E002;

		internal Task<JToken> _E000()
		{
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					this._E000
				},
				{
					_E05B._E000(21599),
					_E001
				}
			});
			return _E002._E000(_E05B._E000(13253), content, null, skipAuth: true);
		}
	}

	private static readonly int[] m__E000 = new int[9] { 401001, 209, 229, 205, 240, 243, 248, 249, 206 };

	[CompilerGenerated]
	private EventHandler _E001;

	[CompilerGenerated]
	private EventHandler _E002;

	[CompilerGenerated]
	private AuthorizationErrorEventHandler _E003;

	[CompilerGenerated]
	private bool m__E004;

	[CompilerGenerated]
	private OAuthToken m__E005;

	private readonly ISettingsService _settingsService;

	private readonly IInformationCollectionService m__E006;

	private readonly Microsoft.Extensions.Logging.ILogger _E007;

	private readonly JsonSerializer _E008;

	private readonly HttpClient _E009;

	public bool IsLoggedIn
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E004 = value;
		}
	}

	public OAuthToken AccessToken
	{
		[CompilerGenerated]
		get
		{
			return this.m__E005;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E005 = value;
		}
	}

	public event EventHandler OnLoggedOut
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = this._E001;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = this._E001;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler OnLoggedIn
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = this._E002;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = this._E002;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event AuthorizationErrorEventHandler OnAuthorizationError
	{
		[CompilerGenerated]
		add
		{
			AuthorizationErrorEventHandler authorizationErrorEventHandler = this._E003;
			AuthorizationErrorEventHandler authorizationErrorEventHandler2;
			do
			{
				authorizationErrorEventHandler2 = authorizationErrorEventHandler;
				AuthorizationErrorEventHandler value2 = (AuthorizationErrorEventHandler)Delegate.Combine(authorizationErrorEventHandler2, value);
				authorizationErrorEventHandler = Interlocked.CompareExchange(ref this._E003, value2, authorizationErrorEventHandler2);
			}
			while ((object)authorizationErrorEventHandler != authorizationErrorEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			AuthorizationErrorEventHandler authorizationErrorEventHandler = this._E003;
			AuthorizationErrorEventHandler authorizationErrorEventHandler2;
			do
			{
				authorizationErrorEventHandler2 = authorizationErrorEventHandler;
				AuthorizationErrorEventHandler value2 = (AuthorizationErrorEventHandler)Delegate.Remove(authorizationErrorEventHandler2, value);
				authorizationErrorEventHandler = Interlocked.CompareExchange(ref this._E003, value2, authorizationErrorEventHandler2);
			}
			while ((object)authorizationErrorEventHandler != authorizationErrorEventHandler2);
		}
	}

	public _E01B(IHttpClientBuilder httpClientBuilder, ISettingsService settingsService, IInformationCollectionService informationCollectionService, ILogger<_E01B> logger, JsonSerializer jsonSerializer)
	{
		_settingsService = settingsService;
		this.m__E006 = informationCollectionService;
		this._E007 = logger;
		_E008 = jsonSerializer;
		AccessToken = new OAuthToken(_settingsService.AccessToken, _settingsService.AccessTokenExpirationTimeUtc);
		_E009 = httpClientBuilder.AddExceptionHandling(ensureSuccessStatusCodes: true).AddRequestMetadata().AddAuthentication()
			.AddLogging<_E01B>()
			.AddApiResponses(_E05B._E000(10916), _E05B._E000(10920), _E05B._E000(21670), _E05B._E000(21673))
			.AddJsonResponses()
			.AddCompression()
			.WithBaseAddress(_settingsService.LauncherBackendUri)
			.Build();
	}

	public void EnsureSuccessAuthorization(HttpResponseMessage response)
	{
		if (response?.Content is ApiContent apiContent)
		{
			if (_E01B.m__E000.Contains(apiContent.Code))
			{
				AuthServiceException ex = new AuthServiceException(string.Format(_E05B._E000(13853), apiContent.Code), new ApiNetworkException(response, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args));
				this._E003?.Invoke(ex);
				_E000();
				throw ex;
			}
			if (apiContent.Code == 301002)
			{
				throw new AuthServiceTokenExpiredException();
			}
			return;
		}
		this._E007.LogError(_E05B._E000(13951));
		AuthServiceException ex2 = new AuthServiceException(_E05B._E000(13951));
		this._E003?.Invoke(ex2);
		throw ex2;
	}

	public async Task LogIn(string email, string passwordHash, string captcha)
	{
		if (IsLoggedIn)
		{
			throw new AuthServiceException(_E05B._E000(13622));
		}
		_settingsService.ResetAccountSettings();
		_settingsService.LoginOrEmail = email;
		try
		{
			this._E007.LogDebug(_E05B._E000(13584), email.ToSecretData());
			string hwIdV = this.m__E006.GetHwIdV1();
			SystemInfo systemInfo = this.m__E006.GetSystemInfo();
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					email
				},
				{
					_E05B._E000(13681),
					passwordHash
				},
				{
					_E05B._E000(10749),
					hwIdV
				},
				{
					_E05B._E000(21407),
					JObject.FromObject(systemInfo, _E008)
				},
				{
					_E05B._E000(13684),
					captcha
				}
			});
			JToken oAuthResponseData = await _E000(_E05B._E000(13692), content, null, skipAuth: true, preventTokenUpdate: false, hasSecretRequestData: true);
			this._E007.LogInformation(_E05B._E000(13645), email.ToSecretData());
			_E000(oAuthResponseData);
			IsLoggedIn = true;
			this._E002?.Invoke(this, EventArgs.Empty);
			_settingsService.Save();
		}
		catch (ApiNetworkException)
		{
			throw;
		}
		catch (NetworkException ex2) when (ex2.IsLocalProblems)
		{
			this._E007.LogWarning(_E05B._E000(13741), email.ToSecretData());
			throw;
		}
		catch (Exception exception)
		{
			this._E007.LogError(exception, _E05B._E000(13715), email.ToSecretData());
			throw;
		}
	}

	public async Task<bool> LoginBySavedToken()
	{
		if (IsLoggedIn)
		{
			return true;
		}
		try
		{
			this._E007.LogInformation(_E05B._E000(13802));
			AccessToken.ThrowIfExpired();
			this._E007.LogInformation(_E05B._E000(13768));
			IsLoggedIn = true;
			this._E002?.Invoke(this, EventArgs.Empty);
		}
		catch (AuthServiceTokenExpiredException)
		{
			try
			{
				await UpdateAccessToken();
				this._E007.LogInformation(_E05B._E000(13313));
				IsLoggedIn = true;
				this._E002?.Invoke(this, EventArgs.Empty);
			}
			catch (NetworkException ex) when (ex.IsLocalProblems)
			{
				this._E007.LogWarning(_E05B._E000(13429));
				throw;
			}
			catch (Exception ex2) when (ex2 is AuthServiceException || ex2 is UnauthorizedApiNetworkException)
			{
				this._E007.LogInformation(_E05B._E000(13483));
				return false;
			}
			catch (Exception exception)
			{
				this._E007.LogError(exception, _E05B._E000(13442));
				return false;
			}
		}
		return IsLoggedIn;
	}

	public async Task<OAuthToken> UpdateAccessToken()
	{
		if (string.IsNullOrEmpty(_settingsService.RefreshToken))
		{
			_E000();
			throw new AuthServiceException(_E05B._E000(13464));
		}
		string hwIdV = this.m__E006.GetHwIdV1();
		SystemInfo systemInfo = this.m__E006.GetSystemInfo();
		JsonContent content = new JsonContent(new JObject
		{
			{
				_E05B._E000(13518),
				_E05B._E000(14024)
			},
			{
				_E05B._E000(14024),
				_settingsService.RefreshToken
			},
			{
				_E05B._E000(10749),
				hwIdV
			},
			{
				_E05B._E000(21407),
				JObject.FromObject(systemInfo, _E008)
			},
			{
				_E05B._E000(13531),
				0
			}
		});
		JToken oAuthResponseData;
		try
		{
			oAuthResponseData = await _E000(_E05B._E000(13089), content, null, skipAuth: true, preventTokenUpdate: false, hasSecretRequestData: true);
		}
		catch (UnauthorizedApiNetworkException)
		{
			_E000();
			throw new AuthServiceException(_E05B._E000(13464));
		}
		catch (ApiNetworkException innerException)
		{
			throw new AuthServiceException(_E05B._E000(13114), innerException);
		}
		_E000(oAuthResponseData);
		this._E007.LogDebug(_E05B._E000(13076));
		return AccessToken;
	}

	private void _E000()
	{
		if (!IsLoggedIn)
		{
			this._E007.LogInformation(_E05B._E000(13989));
			return;
		}
		try
		{
			this._E001?.Invoke(this, EventArgs.Empty);
		}
		finally
		{
			IsLoggedIn = false;
			string loginOrEmail = _settingsService.LoginOrEmail;
			_settingsService.ResetAccountSettings();
			_settingsService.LoginOrEmail = loginOrEmail;
			_settingsService.KeepLoggedIn = false;
			_settingsService.Save();
			this._E007.LogInformation(_E05B._E000(14013));
		}
	}

	public async Task LogOut()
	{
		try
		{
			await _E000(_E05B._E000(13158), null, null, skipAuth: false, preventTokenUpdate: true);
		}
		catch (UnauthorizedApiNetworkException ex)
		{
			this._E007.LogWarning(_E05B._E000(13174), ex.ApiCode);
		}
		catch (NetworkException ex2) when (ex2.IsLocalProblems)
		{
			this._E007.LogWarning(_E05B._E000(13219));
		}
		catch (Exception exception)
		{
			this._E007.LogError(exception, _E05B._E000(32143));
		}
		finally
		{
			_E000();
		}
	}

	public Task ActivateHardware(string email, string activationCode)
	{
		return this._E007.WithExceptionsHandling(_E05B._E000(13962), delegate
		{
			string hwIdV = this.m__E006.GetHwIdV1();
			SystemInfo systemInfo = this.m__E006.GetSystemInfo();
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					email
				},
				{
					_E05B._E000(10749),
					hwIdV
				},
				{
					_E05B._E000(21407),
					JObject.FromObject(systemInfo, _E008)
				},
				{
					_E05B._E000(13245),
					activationCode
				}
			});
			return _E000(_E05B._E000(13192), content, null, skipAuth: true, preventTokenUpdate: false, hasSecretRequestData: true);
		});
	}

	public Task<JToken> BindPhone(string email, string phone)
	{
		return this._E007.WithExceptionsHandling(_E05B._E000(13976), delegate
		{
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					email
				},
				{
					_E05B._E000(13289),
					phone
				}
			});
			return _E000(_E05B._E000(13299), content, null, skipAuth: true);
		});
	}

	public Task<JToken> VerifyPhone(string email, string verificationCode)
	{
		return this._E007.WithExceptionsHandling(_E05B._E000(14053), delegate
		{
			JsonContent content = new JsonContent(new JObject
			{
				{
					_E05B._E000(13679),
					email
				},
				{
					_E05B._E000(21599),
					verificationCode
				}
			});
			return _E000(_E05B._E000(13253), content, null, skipAuth: true);
		});
	}

	private void _E000(JToken oAuthResponseData)
	{
		try
		{
			string text = oAuthResponseData.Value<string>(_E05B._E000(14064));
			_settingsService.AccessToken = text;
			int num = oAuthResponseData.Value<int>(_E05B._E000(14019));
			DateTime dateTime = (DateTime.UtcNow.ToUnixTimeStamp() + num).FromUnixTimestampToDateTime();
			_settingsService.AccessTokenExpirationTimeUtc = dateTime;
			AccessToken = new OAuthToken(text, dateTime);
			string refreshToken = oAuthResponseData.Value<string>(_E05B._E000(14024));
			_settingsService.RefreshToken = refreshToken;
		}
		catch (Exception exception)
		{
			this._E007.LogCritical(exception, _E05B._E000(14042));
			_E000();
		}
	}

	private async Task<JToken> _E000(string requestUri, HttpContent content = null, HttpMethod method = null, bool skipAuth = false, bool preventTokenUpdate = false, bool hasSecretRequestData = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = new HttpRequestMessage(method ?? HttpMethod.Post, requestUri)
		{
			Content = content
		};
		if (skipAuth)
		{
			request.SkipAuth();
		}
		if (preventTokenUpdate)
		{
			request.PreventTokenUpdate();
		}
		if (hasSecretRequestData)
		{
			request.MarkAsSecretRequestData();
		}
		using HttpResponseMessage httpResponseMessage = await _E009.SendAsync(request, cancellationToken);
		ApiContent apiContent = ApiResponse.GetApiContent(httpResponseMessage);
		if (apiContent.Code != 0)
		{
			throw new ApiNetworkException(httpResponseMessage, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args);
		}
		return apiContent.Data;
	}
}
internal class _E01C : DelegatingHandler
{
	private readonly string m__E000;

	private readonly string _E001;

	private readonly string _E002;

	private readonly string _E003;

	private readonly bool _E004;

	public _E01C(string apiCodeKey, string apiMessageKey, string apiDataKey, string apiArgsKey, bool throwIfParsingFailed, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		this.m__E000 = apiCodeKey;
		_E001 = apiMessageKey;
		_E002 = apiDataKey;
		_E003 = apiArgsKey;
		_E004 = throwIfParsingFailed;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
		if (httpResponseMessage.Content is JsonContent jsonContent)
		{
			int? num = jsonContent.Json.Value<int?>(this.m__E000);
			if (num.HasValue)
			{
				string message = jsonContent.Json.Value<string>(_E001);
				JToken data = jsonContent.Json[_E002];
				JToken args = jsonContent.Json[_E003];
				httpResponseMessage.Content = new ApiContent(num.Value, message, data, args, jsonContent);
			}
			else if (_E004)
			{
				throw new HttpNetworkException(httpResponseMessage, _E05B._E000(16080));
			}
		}
		return httpResponseMessage;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E01D : DelegatingHandler
{
	private readonly Func<HttpRequestMessage, Task> m__E000;

	private readonly Func<HttpResponseMessage, Task> _E001;

	public _E01D(Func<HttpRequestMessage, Task> onRequest, Func<HttpResponseMessage, Task> onResponse, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		this.m__E000 = onRequest;
		_E001 = onResponse;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (this.m__E000 != null)
		{
			await this.m__E000(request);
		}
		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
		if (_E001 != null)
		{
			await _E001(response);
		}
		return response;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E01E : DelegatingHandler
{
	private readonly ICaptchaHandler m__E000;

	private readonly ILogger<_E01E> _E001;

	private readonly HttpClientHandler _E002;

	private readonly ManualResetEventSlim _E003 = new ManualResetEventSlim(initialState: true);

	private int _E004;

	public _E01E(ICaptchaHandler captchaHandler, IServiceProvider serviceProvider, HttpClientHandler httpClientHandler)
		: base(httpClientHandler)
	{
		this.m__E000 = captchaHandler;
		_E001 = serviceProvider.GetRequiredService<ILogger<_E01E>>();
		_E002 = httpClientHandler;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		_E003.Wait(cancellationToken);
		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
		if (_E01E._E000(response))
		{
			int num = Interlocked.Increment(ref _E004);
			if (num == 1)
			{
				_E003.Reset();
			}
			else if (num == ServicePointManager.DefaultConnectionLimit)
			{
				Interlocked.Decrement(ref _E004);
				_E001.LogWarning(_E05B._E000(15634));
				return response;
			}
			try
			{
				if (!(await this.m__E000.Handle(response, _E002)))
				{
					_E001.LogWarning(_E05B._E000(15776));
					throw new CloudflareCaptchaException(_E05B._E000(15776), _E01E._E000(response));
				}
				response = await base.SendAsync(request, cancellationToken);
			}
			finally
			{
				if (Interlocked.Decrement(ref _E004) == 0)
				{
					_E003.Set();
				}
			}
		}
		if (response.StatusCode == HttpStatusCode.Forbidden)
		{
			string text = _E01E._E000(response);
			if (text != null)
			{
				throw new CloudflareCaptchaException(_E05B._E000(15748), text);
			}
		}
		return response;
	}

	private static bool _E000(HttpResponseMessage response)
	{
		if (response.StatusCode == HttpStatusCode.Forbidden && response.Headers.TryGetValues(_E05B._E000(15679), out var values) && values != null)
		{
			return values.Contains(_E05B._E000(26072));
		}
		return false;
	}

	private static string _E000(HttpResponseMessage response)
	{
		if (!response.Headers.TryGetValues(_E05B._E000(15625), out var values))
		{
			return null;
		}
		return values?.FirstOrDefault();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E01F : IHttpClientBuilder
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public bool _E000;

		internal HttpMessageHandler _E000(HttpMessageHandler inner)
		{
			return new _E022(this._E000, inner);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E01F _E000;

		public bool _E001;

		public bool _E002;

		public bool _E003;

		internal HttpMessageHandler _E000(HttpMessageHandler inner)
		{
			return new _E021(this._E000._settingsService, this._E000._E004, this._E000._launcherMetadata, _E001, _E002, _E003, inner);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string _E000;

		public string _E001;

		public string _E002;

		public string _E003;

		public bool _E004;

		internal HttpMessageHandler _E000(HttpMessageHandler inner)
		{
			return new _E01C(this._E000, _E001, _E002, _E003, _E004, inner);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Func<HttpRequestMessage, Task> _E000;

		public Func<HttpResponseMessage, Task> _E001;

		internal HttpMessageHandler _E000(HttpMessageHandler inner)
		{
			return new _E01D(this._E000, _E001, inner);
		}
	}

	private readonly IServiceProvider m__E000;

	private readonly HttpClientBuilderOptions m__E001;

	private readonly ICaptchaHandler m__E002;

	private readonly Lazy<IAuthService> m__E003;

	private readonly ISettingsService _settingsService;

	private readonly Lazy<IGameService> _E004;

	private readonly ILauncherMetadata _launcherMetadata;

	private readonly List<Func<HttpMessageHandler, HttpMessageHandler>> _E005 = new List<Func<HttpMessageHandler, HttpMessageHandler>>();

	private Uri _E006;

	static _E01F()
	{
		ServicePointManager.DefaultConnectionLimit = 3;
	}

	public _E01F(IServiceProvider serviceProvider, HttpClientBuilderOptions httpClientBuilderOptions, ICaptchaHandler captchaHandler, Lazy<IAuthService> authServiceLazy, ISettingsService settingsService, Lazy<IGameService> gameServiceLazy, ILauncherMetadata launcherMetadata)
	{
		this.m__E000 = serviceProvider;
		this.m__E001 = httpClientBuilderOptions;
		this.m__E002 = captchaHandler;
		this.m__E003 = authServiceLazy;
		_settingsService = settingsService;
		_E004 = gameServiceLazy;
		_launcherMetadata = launcherMetadata;
	}

	public IHttpClientBuilder AddExceptionHandling(bool ensureSuccessStatusCodes)
	{
		_E005.Add((HttpMessageHandler inner) => new _E022(ensureSuccessStatusCodes, inner));
		return this;
	}

	public IHttpClientBuilder AddRequestMetadata(bool addLanguage, bool addBranchName, bool addGameVersion)
	{
		_E005.Add((HttpMessageHandler inner) => new _E021(_settingsService, _E004, _launcherMetadata, addLanguage, addBranchName, addGameVersion, inner));
		return this;
	}

	public IHttpClientBuilder AddAuthentication()
	{
		_E005.Add((HttpMessageHandler inner) => new _E020(this.m__E003, inner));
		return this;
	}

	public IHttpClientBuilder AddLogging<TLogger>()
	{
		_E005.Add((HttpMessageHandler inner) => new _E024(typeof(TLogger).Name + _E05B._E000(15768), this.m__E000, inner));
		return this;
	}

	public IHttpClientBuilder AddApiResponses(string apiCodeKey, string apiMessageKey, string apiDataKey, string apiArgsKey, bool throwIfParsingFailed)
	{
		_E005.Add((HttpMessageHandler inner) => new _E01C(apiCodeKey, apiMessageKey, apiDataKey, apiArgsKey, throwIfParsingFailed, inner));
		return this;
	}

	public IHttpClientBuilder AddJsonResponses()
	{
		_E005.Add((HttpMessageHandler inner) => new _E023(inner));
		return this;
	}

	public IHttpClientBuilder AddCompression()
	{
		_E005.Add((HttpMessageHandler inner) => new _E025(inner));
		return this;
	}

	public IHttpClientBuilder AddCallback(Func<HttpRequestMessage, Task> onRequest, Func<HttpResponseMessage, Task> onResponse)
	{
		_E005.Add((HttpMessageHandler inner) => new _E01D(onRequest, onResponse, inner));
		return this;
	}

	public IHttpClientBuilder WithBaseAddress(Uri baseAddress)
	{
		_E006 = baseAddress;
		return this;
	}

	public HttpClient Build()
	{
		HttpClientHandler httpClientHandler = this.m__E001.HttpClientHandlerFactory();
		HttpMessageHandler httpMessageHandler = new _E01E(this.m__E002, this.m__E000, httpClientHandler);
		for (int num = _E005.Count - 1; num >= 0; num--)
		{
			httpMessageHandler = _E005[num](httpMessageHandler);
		}
		return new HttpClient(httpMessageHandler)
		{
			BaseAddress = _E006
		};
	}

	[CompilerGenerated]
	private HttpMessageHandler _E000(HttpMessageHandler inner)
	{
		return new _E020(this.m__E003, inner);
	}

	[CompilerGenerated]
	private HttpMessageHandler _E000<_E004>(HttpMessageHandler inner)
	{
		return new _E024(typeof(_E004).Name + _E05B._E000(15768), this.m__E000, inner);
	}
}
internal class _E020 : DelegatingHandler
{
	public const string _E000 = "skip_auth";

	public const string _E001 = "prevent_token_update";

	private static readonly SemaphoreSlim _E002 = new SemaphoreSlim(1, 1);

	private readonly Lazy<IAuthService> _E003;

	public _E020(Lazy<IAuthService> authServiceLazy, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		_E003 = authServiceLazy;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		bool flag = request.Properties.ContainsKey(_E05B._E000(15869));
		bool flag2 = request.Properties.ContainsKey(_E05B._E000(15819));
		OAuthToken token = default(OAuthToken);
		HttpResponseMessage response = null;
		try
		{
			try
			{
				if (!flag)
				{
					await _E002.WaitAsync(cancellationToken);
					try
					{
						token = _E003.Value.AccessToken;
					}
					finally
					{
						_E002.Release();
					}
					token.ThrowIfExpired();
					request.Headers.Authorization = new AuthenticationHeaderValue(_E05B._E000(15838), token.Value);
				}
				response = await base.SendAsync(request, cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					_E003.Value.EnsureSuccessAuthorization(response);
				}
			}
			catch (AuthServiceTokenExpiredException) when (!flag2 && !flag)
			{
				await _E002.WaitAsync(cancellationToken);
				try
				{
					if (token.Value == _E003.Value.AccessToken.Value)
					{
						token = await _E003.Value.UpdateAccessToken();
					}
				}
				finally
				{
					_E002.Release();
				}
				token.ThrowIfExpired();
				request.Headers.Authorization = new AuthenticationHeaderValue(_E05B._E000(15838), token.Value);
				response = await base.SendAsync(request, cancellationToken);
				_E003.Value.EnsureSuccessAuthorization(response);
				return response;
			}
			return response;
		}
		catch (AuthServiceException)
		{
			if (!(response.Content is ApiContent apiContent))
			{
				throw new WrongResponseHttpNetworkException(response);
			}
			throw new UnauthorizedApiNetworkException(response, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args, _E05B._E000(15399));
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E021 : DelegatingHandler
{
	private readonly ISettingsService _settingsService;

	private readonly Lazy<IGameService> _E000;

	private readonly string _E001;

	private readonly bool _E002;

	private readonly bool _E003;

	private readonly bool _E004;

	private readonly ProductInfoHeaderValue _E005;

	public _E021(ISettingsService settingsService, Lazy<IGameService> gameServiceLazy, ILauncherMetadata launcherMetadata, bool addLanguage, bool addBranchName, bool addGameVersion, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		_settingsService = settingsService;
		_E000 = gameServiceLazy;
		_E001 = launcherMetadata.LauncherVersion.ToString();
		_E002 = addLanguage;
		_E003 = addBranchName;
		_E004 = addGameVersion;
		_E005 = new ProductInfoHeaderValue(_E05B._E000(26103), _E001);
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.Headers.UserAgent.Add(_E005);
		string leftPart = request.RequestUri.GetLeftPart(UriPartial.Path);
		StringBuilder stringBuilder = new StringBuilder(request.RequestUri.Query ?? "");
		stringBuilder.Append(request.RequestUri.Query.Contains(_E05B._E000(26772)) ? _E05B._E000(26778) : _E05B._E000(26772));
		stringBuilder.Append(_E05B._E000(15374) + _E001);
		if (_settingsService.IsBackendSettingsLoaded)
		{
			stringBuilder.Append(_E05B._E000(15389) + _settingsService.SelectedBranch.Name);
		}
		if (_E002)
		{
			stringBuilder.Append(_E05B._E000(15460) + _settingsService.Language);
		}
		if (_E003 && _settingsService.IsBackendSettingsLoaded)
		{
			stringBuilder.Append(_E05B._E000(15389) + _settingsService.SelectedBranch.Name);
		}
		if (_E004 && _E000.Value.GameVersion != default(BsgVersion))
		{
			stringBuilder.Append(string.Format(_E05B._E000(15473), _E000.Value.GameVersion));
		}
		request.RequestUri = new Uri(leftPart + stringBuilder.ToString());
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E022 : DelegatingHandler
{
	private readonly bool m__E000;

	public _E022(bool ensureSuccessStatusCodes, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		this.m__E000 = ensureSuccessStatusCodes;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpResponseMessage httpResponseMessage;
		try
		{
			httpResponseMessage = await base.SendAsync(request, cancellationToken);
		}
		catch (Exception ex)
		{
			if (ex is NetworkException)
			{
				throw;
			}
			throw new NetworkException(request, ex.InnerException is WebException, ex);
		}
		if (this.m__E000 && !httpResponseMessage.IsSuccessStatusCode)
		{
			throw new HttpNetworkException(httpResponseMessage);
		}
		return httpResponseMessage;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E023 : DelegatingHandler
{
	public _E023(HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
		if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
		{
			try
			{
				httpResponseMessage.Content = new JsonContent(httpResponseMessage.Content);
				return httpResponseMessage;
			}
			catch (Exception ex) when (!(ex is HttpMessageResponseHandlerException))
			{
				throw new HttpMessageResponseHandlerException(httpResponseMessage, _E05B._E000(15424), ex);
			}
		}
		return httpResponseMessage;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E024 : DelegatingHandler
{
	public const string _E000 = "secret_request_data";

	private readonly Microsoft.Extensions.Logging.ILogger _E001;

	public _E024(string loggerName, IServiceProvider sp, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		this._E001 = sp.GetRequiredService<ILoggerFactory>().CreateLogger(loggerName);
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		Exception exception = null;
		HttpResponseMessage response = null;
		HttpResponseMessage result;
		try
		{
			response = await base.SendAsync(request, cancellationToken);
			result = response;
		}
		catch (Exception ex)
		{
			exception = ex;
			throw;
		}
		finally
		{
			stopwatch.Stop();
			_E000(response, exception, out var logLevel, out var localProblems);
			if (response == null && exception is HttpMessageResponseHandlerException ex2)
			{
				response = ex2.Response;
			}
			bool hasSecretRequestData = request.HasSecretRequestData();
			await Task.WhenAll(_E000(hasSecretRequestData, logLevel, request, response, exception, localProblems, stopwatch.ElapsedMilliseconds), _E000(hasSecretRequestData, logLevel, request, response, exception, stopwatch.ElapsedMilliseconds));
		}
		return result;
	}

	private static void _E000(HttpResponseMessage response, Exception exception, out LogLevel logLevel, out IReadOnlyCollection<string> localProblems)
	{
		if (exception is HttpRequestException ex && ex.InnerException is WebException ex2)
		{
			List<string> list = new List<string>();
			for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
			{
				list.Add(ex3.Message);
			}
			localProblems = list;
			logLevel = LogLevel.Warning;
		}
		else
		{
			logLevel = ((exception != null) ? LogLevel.Error : ((response != null && response.IsSuccessStatusCode) ? LogLevel.Debug : LogLevel.Warning));
			localProblems = null;
		}
	}

	private async Task _E000(bool hasSecretRequestData, LogLevel logLevel, HttpRequestMessage request, HttpResponseMessage response, Exception exception, IReadOnlyCollection<string> localProblems, long requestDurationMs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<object> list = new List<object>();
		stringBuilder.Append(_E05B._E000(15548));
		if (response == null || !response.IsSuccessStatusCode)
		{
			stringBuilder.Append(_E05B._E000(15488));
			list.Add(_E000(request.RequestUri));
		}
		stringBuilder.Append(_E05B._E000(15518));
		list.Add(request.RequestUri.ToString());
		if (response != null)
		{
			stringBuilder.AppendLine().Append(_E05B._E000(15592));
			list.Add(response.StatusCode.ToString());
			stringBuilder.Append(_E05B._E000(15615));
			list.Add((response.Content as ApiContent)?.Code);
			stringBuilder.Append(_E05B._E000(15560));
			list.Add(requestDurationMs);
			if (!response.IsSuccessStatusCode && response.Headers != null && response.Headers.Any())
			{
				stringBuilder.Append(_E05B._E000(15138));
				list.Add(_E000(response.Headers));
			}
			if (response.Content != null && (!response.IsSuccessStatusCode || exception != null))
			{
				string text = await _E000(response.Content);
				if (text.Contains('\n'))
				{
					stringBuilder.AppendLine().Append(_E05B._E000(15158));
				}
				else
				{
					stringBuilder.AppendLine().Append(_E05B._E000(15122));
				}
				list.Add(text);
			}
		}
		if (localProblems != null)
		{
			foreach (string localProblem in localProblems)
			{
				stringBuilder.AppendLine().Append(_E05B._E000(15204) + localProblem);
			}
		}
		using (this._E001.BeginScope(new Dictionary<string, object> { 
		{
			_E05B._E000(15208),
			null
		} }))
		{
			this._E001.Log(logLevel, (localProblems == null) ? exception : null, stringBuilder.ToString(), list.ToArray());
		}
	}

	private async Task _E000(bool hasSecretRequestData, LogLevel logLevel, HttpRequestMessage request, HttpResponseMessage response, Exception exception, long requestDurationMs)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{
				_E05B._E000(15220),
				null
			},
			{
				_E05B._E000(15174),
				_E000(request.RequestUri)
			}
		};
		if (response != null && (!response.IsSuccessStatusCode || exception != null))
		{
			if (response.Headers != null && response.Headers.Any())
			{
				dictionary.Add(_E05B._E000(15193), _E000(response.Headers));
			}
			if (response.Content != null)
			{
				Dictionary<string, object> dictionary2 = dictionary;
				string value = await _E000(response.Content);
				dictionary2.Add(_E05B._E000(15272), value);
			}
			if (request.Content != null && !hasSecretRequestData)
			{
				Dictionary<string, object> dictionary2 = dictionary;
				string value = await _E000(request.Content);
				dictionary2.Add(_E05B._E000(15295), value);
			}
		}
		string message = _E05B._E000(15247);
		string text = response?.StatusCode.ToString() ?? _E05B._E000(15308);
		int? num = (response?.Content as ApiContent)?.Code;
		using (this._E001.BeginScope(dictionary))
		{
			this._E001.Log(logLevel, exception, message, text, num, requestDurationMs, request.RequestUri.ToString());
		}
	}

	private static string _E000(HttpHeaders headers)
	{
		return string.Join(_E05B._E000(27867), headers.Select((KeyValuePair<string, IEnumerable<string>> h) => _E05B._E000(15316) + h.Key + _E05B._E000(15322) + string.Join(_E05B._E000(27867), h.Value) + _E05B._E000(15327)));
	}

	private Task<string> _E000(HttpContent httpContent)
	{
		HttpContent originalContent = httpContent.GetOriginalContent();
		if (originalContent != null)
		{
			if (!(originalContent is JsonContent) && !(originalContent is StringContent))
			{
				return Task.FromResult(originalContent.GetType().Name.Replace(_E05B._E000(30137), "").Replace(_E05B._E000(15524), ""));
			}
			return originalContent.ReadAsStringAsync();
		}
		return Task.FromResult<string>(null);
	}

	private static string _E000(Uri uri)
	{
		string result = _E05B._E000(15535);
		try
		{
			result = string.Join(_E05B._E000(21716), Dns.GetHostEntry(uri.DnsSafeHost).AddressList.Select((IPAddress _) => _.ToString()));
			return result;
		}
		catch
		{
			return result;
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal class _E025 : DelegatingHandler
{
	public const string _E000 = "zlib";

	public _E025(HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (!request.Properties.ContainsKey(_E05B._E000(15325)))
		{
			request.Properties.Add(_E05B._E000(15325), null);
		}
		if (request.Content != null && !(request.Content is ZlibContent))
		{
			request.Content = new ZlibContent(request.Content, CompressionMode.Compress);
		}
		HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
		if (httpResponseMessage.Content != null && httpResponseMessage.IsSuccessStatusCode)
		{
			httpResponseMessage.Content = new ZlibContent(httpResponseMessage.Content, CompressionMode.Decompress);
		}
		return httpResponseMessage;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _E000(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return base.SendAsync(request, cancellationToken);
	}
}
internal static class _E026
{
	public static Exception AddDiskLetter(this Exception exc, string path)
	{
		string pathRoot = Path.GetPathRoot(Path.GetFullPath(path).WithoutLongPathPrefix());
		if (!exc.Data.Contains("driveLetter"))
		{
			exc.Data.Add("driveLetter", pathRoot);
		}
		else
		{
			exc.Data["driveLetter"] = pathRoot;
		}
		return exc;
	}
}
internal class _E027 : IFileManager
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FileStream _E000;

		public FileMode _E001;

		public FileAccess _E002;

		public FileShare _E003;

		internal void _E000(string p)
		{
			this._E000 = new FileStream(p, _E001, _E002, _E003);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E027 _E000;

		public string _E001;

		internal void _E000()
		{
			this._E000._E000(File.Delete, this._E001);
		}

		internal void _E001()
		{
			this._E000._E000(delegate(string p)
			{
				Directory.Delete(p, recursive: true);
			}, this._E001);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public bool _E000;

		public bool _E001;

		public string _E002;

		public _E027 _E003;

		internal void _E000(string s)
		{
			if (this._E000)
			{
				if (_E001)
				{
					if (Directory.Exists(_E002))
					{
						Directory.Delete(_E002, recursive: true);
					}
				}
				else if (File.Exists(_E002))
				{
					File.Delete(_E002);
				}
			}
			Directory.CreateDirectory(Path.GetDirectoryName(_E002));
			if (Path.GetPathRoot(s).Equals(Path.GetPathRoot(_E002), StringComparison.InvariantCultureIgnoreCase))
			{
				Directory.Move(s, _E002);
			}
			else if (_E001)
			{
				_E003._E000(s, _E002, this._E000);
			}
			else
			{
				File.Copy(s, _E002, this._E000);
			}
		}
	}

	private const int m__E000 = 3;

	private const int m__E001 = 1000;

	[CompilerGenerated]
	private Action<string, HandledEventArgs> m__E002;

	private readonly Microsoft.Extensions.Logging.ILogger _E003;

	public event Action<string, HandledEventArgs> OnFileIsUsedByAnotherProcess
	{
		[CompilerGenerated]
		add
		{
			Action<string, HandledEventArgs> action = this.m__E002;
			Action<string, HandledEventArgs> action2;
			do
			{
				action2 = action;
				Action<string, HandledEventArgs> value2 = (Action<string, HandledEventArgs>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<string, HandledEventArgs> action = this.m__E002;
			Action<string, HandledEventArgs> action2;
			do
			{
				action2 = action;
				Action<string, HandledEventArgs> value2 = (Action<string, HandledEventArgs>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E027(ILogger<_E027> logger)
	{
		_E003 = logger;
	}

	public FileStream CaptureFile(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, FileShare fileShare = FileShare.Read)
	{
		FileStream result = null;
		_E000(delegate(string p)
		{
			result = new FileStream(p, fileMode, fileAccess, fileShare);
		}, path);
		return result;
	}

	public void Delete(string path)
	{
		Action action = (File.Exists(path) ? ((Action)delegate
		{
			_E000(File.Delete, path);
		}) : (Directory.Exists(path) ? ((Action)delegate
		{
			_E000(delegate(string p)
			{
				Directory.Delete(p, recursive: true);
			}, path);
		}) : null));
		if (action == null)
		{
			return;
		}
		try
		{
			action();
			_E003.LogTrace(_E05B._E000(14880), path);
		}
		catch (Exception exception)
		{
			_E003.LogError(exception, _E05B._E000(14855), path);
			throw;
		}
	}

	public void Delete(IEnumerable<string> paths)
	{
		foreach (string path in paths)
		{
			Delete(path);
		}
	}

	public void Move(string sourcePath, string destinationPath, bool overwrite)
	{
		bool flag = false;
		if (Directory.Exists(sourcePath))
		{
			flag = true;
		}
		else if (!File.Exists(sourcePath))
		{
			throw new IOException(_E05B._E000(14879));
		}
		try
		{
			_E000(delegate(string s)
			{
				if (overwrite)
				{
					if (flag)
					{
						if (Directory.Exists(destinationPath))
						{
							Directory.Delete(destinationPath, recursive: true);
						}
					}
					else if (File.Exists(destinationPath))
					{
						File.Delete(destinationPath);
					}
				}
				Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
				if (Path.GetPathRoot(s).Equals(Path.GetPathRoot(destinationPath), StringComparison.InvariantCultureIgnoreCase))
				{
					Directory.Move(s, destinationPath);
				}
				else if (flag)
				{
					_E000(s, destinationPath, overwrite);
				}
				else
				{
					File.Copy(s, destinationPath, overwrite);
				}
			}, sourcePath);
			_E003.LogTrace(_E05B._E000(14961), sourcePath, destinationPath);
		}
		catch (Exception exception)
		{
			_E003.LogError(exception, _E05B._E000(15010), sourcePath, destinationPath);
			throw;
		}
	}

	private void _E000(Action<string> fileAction, string path)
	{
		int num = 0;
		while (true)
		{
			try
			{
				fileAction(path);
				break;
			}
			catch (Exception ex) when (ex.HResult == -2147024864)
			{
				if (num < 3)
				{
					num++;
					Thread.Sleep(1000);
					continue;
				}
				_E003.LogInformation(_E05B._E000(14999), path);
				HandledEventArgs handledEventArgs = new HandledEventArgs();
				this.m__E002?.Invoke(path, handledEventArgs);
				if (handledEventArgs.Handled)
				{
					_E003.LogDebug(_E05B._E000(15096), path);
					num = 0;
					continue;
				}
				_E003.LogWarning(_E05B._E000(14634), path);
				throw;
			}
		}
	}

	private void _E000(string sourcePath, string targetPath, bool overwrite)
	{
		string[] directories = Directory.GetDirectories(sourcePath, _E05B._E000(27034), SearchOption.AllDirectories);
		for (int i = 0; i < directories.Length; i++)
		{
			Directory.CreateDirectory(directories[i].Replace(sourcePath, targetPath));
		}
		directories = Directory.GetFiles(sourcePath, _E05B._E000(27034), SearchOption.AllDirectories);
		foreach (string obj in directories)
		{
			File.Copy(obj, obj.Replace(sourcePath, targetPath), overwrite);
		}
	}
}
internal class _E028 : IPatchAlgorithmProvider
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public long _E000;

		public long _E001;

		internal bool _E000(IPatchAlgorithm alg)
		{
			return alg.IsApplicableFor(this._E000, _E001);
		}
	}

	private readonly Dictionary<byte, IPatchAlgorithm> m__E000;

	public _E028(IEnumerable<IPatchAlgorithm> patchAlgorithms)
	{
		this.m__E000 = patchAlgorithms.OrderBy((IPatchAlgorithm alg) => alg.Id).ToDictionary((IPatchAlgorithm alg) => alg.Id);
	}

	public IPatchAlgorithm ChooseAnAlgorithm(long oldFileSize, long newFileSize)
	{
		return this.m__E000.Values.Last((IPatchAlgorithm alg) => alg.IsApplicableFor(oldFileSize, newFileSize));
	}

	public IPatchAlgorithm GetAlgorithm(byte algorithmId)
	{
		if (!this.m__E000.TryGetValue(algorithmId, out var value))
		{
			throw UpdateException.UnknownAlgorithmId(algorithmId);
		}
		return value;
	}
}
internal class _E029 : IUpdate, IDisposable
{
	internal static readonly string[] _E000 = new string[2]
	{
		_E05B._E000(14623),
		_E05B._E000(14692)
	};

	[CompilerGenerated]
	private readonly UpdateMetadata _E001;

	[CompilerGenerated]
	private readonly long _E002;

	[CompilerGenerated]
	private readonly Ionic.Zip.ZipFile _E003;

	private readonly Stream _E004;

	private readonly JsonSerializerSettings _E005;

	public UpdateMetadata Metadata
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public long ApproximateInstallationTime
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public Ionic.Zip.ZipFile Content
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	public _E029(Stream updateStream, JsonSerializerSettings jsonSerializerSettings)
	{
		_E005 = jsonSerializerSettings;
		_E004 = updateStream;
		_E003 = Ionic.Zip.ZipFile.Read(_E004);
		_E001 = _E000(Content);
		_E002 = Metadata.Files.Sum((UpdateFileEntry f) => f.ApplyTime);
	}

	private UpdateMetadata _E000(Ionic.Zip.ZipFile zip)
	{
		Ionic.Zip.ZipEntry zipEntry = zip.Entries.FirstOrDefault((Ionic.Zip.ZipEntry e) => _E029._E000.Contains(e.FileName)) ?? throw UpdateException.UpdateIsCorrupted(_E05B._E000(14602));
		using MemoryStream memoryStream = new MemoryStream();
		zipEntry.Extract(memoryStream);
		memoryStream.Position = 0L;
		using StreamReader streamReader = new StreamReader(memoryStream);
		return JsonConvert.DeserializeObject<UpdateMetadata>(streamReader.ReadToEnd(), _E005);
	}

	public void Dispose()
	{
		_E004?.Dispose();
		Content?.Dispose();
	}
}
internal class _E02A : IUpdateManager
{
	private class _E000 : IDisposable
	{
		private const double m__E000 = 0.1;

		private static readonly TimeSpan m__E001 = TimeSpan.FromMilliseconds(250.0);

		private readonly long _E002;

		private readonly Action<long, long> _E003;

		private readonly Timer _E004;

		private UpdateFileEntry _E005;

		private long _E006;

		private long _E007;

		internal _E000(UpdateMetadata metadata, Action<long, long> progressCallback)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException(_E05B._E000(1160));
			}
			_E002 = metadata.Files.Sum((UpdateFileEntry f) => f.ApplyTime);
			_E003 = progressCallback ?? throw new ArgumentNullException(_E05B._E000(1175));
			_E004 = new Timer(_E000, null, TimeSpan.Zero, _E02A._E000.m__E001);
		}

		public void Dispose()
		{
			_E004.Dispose();
			_E001();
		}

		internal void _E000(UpdateFileEntry updateEntry)
		{
			_E005 = updateEntry;
		}

		internal void _E000(long bytesExtracted, long totalBytesToExtract)
		{
			long num = _E005.ApplyTime * bytesExtracted / totalBytesToExtract;
			_E007 = ((_E005.State == UpdateFileEntryState.Modified) ? ((long)((double)num * 0.1)) : num);
		}

		internal void _E001(long bytesInstalled, long totalBytesToInstall)
		{
			_E007 = (long)((double)_E005.ApplyTime * 0.1 + 0.9 * (double)_E005.ApplyTime * (double)bytesInstalled / (double)totalBytesToInstall);
		}

		internal void _E000()
		{
			_E007 = 0L;
			_E006 += _E005.ApplyTime;
		}

		private void _E001()
		{
			_E003(_E006 + _E007, _E002);
		}

		private void _E000(object state)
		{
			_E001();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string _E000;

		public string _E001;

		public string[] _E002;

		public CancellationToken _E003;

		public _E02A _E004;

		public int _E005;

		public Dictionary<byte, int> _E006;

		public UpdateMetadata _E007;

		internal string _E000(string o)
		{
			return o.Substring(this._E000.Length).TrimStart('\\', '/', ' ');
		}

		internal string _E001(string o)
		{
			return o.Substring(this._E001.Length).TrimStart('\\', '/', ' ');
		}

		internal bool _E000(string f)
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E002
			{
				_E000 = f
			};
			return _E002.Any((string a) => CS_0024_003C_003E8__locals0._E000.EndsWith(a));
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string _E000;

		internal bool _E000(string a)
		{
			return this._E000.EndsWith(a);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public ICSharpCode.SharpZipLib.Zip.ZipOutputStream _E000;

		public ObjectPool<StreamComparator> _E001;

		public Stopwatch _E002;

		public _E02B _E003;

		public _E001 _E004;

		internal void _E000(string f)
		{
			_E004._E003.ThrowIfCancellationRequested();
			IPatchAlgorithm patchAlgorithm = null;
			Stream stream = null;
			string text = Path.Combine(_E004._E000, f);
			try
			{
				using (FileStream fileStream = _E004._E004.m__E002.CaptureFile(text))
				{
					using FileStream fileStream2 = _E004._E004.m__E002.CaptureFile(Path.Combine(_E004._E001, f));
					StreamComparator streamComparator = _E001.Get();
					try
					{
						if (streamComparator.Equals(fileStream, fileStream2))
						{
							int num = _E004._E005;
							_E004._E005 = num + 1;
							return;
						}
					}
					finally
					{
						_E001.Return(streamComparator);
					}
					fileStream.Position = 0L;
					fileStream2.Position = 0L;
					patchAlgorithm = _E004._E004.m__E001.ChooseAnAlgorithm(fileStream.Length, fileStream2.Length);
					stream = new MemoryStream();
					_E004._E004._E006.LogDebug(_E05B._E000(1254), patchAlgorithm.Name, f);
					try
					{
						try
						{
							patchAlgorithm.CreatePatch(fileStream, fileStream2, stream);
						}
						catch (PatchAlgorithmException exception) when (patchAlgorithm.Id != 0)
						{
							_E004._E004._E006.LogWarning(exception, _E05B._E000(1232), patchAlgorithm.Name, f, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream.Length), Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream2.Length));
							patchAlgorithm = _E004._E004.m__E001.GetAlgorithm(0);
							_E004._E004._E006.LogWarning(_E05B._E000(843), patchAlgorithm.Name, f);
							stream.Position = 0L;
							stream.SetLength(0L);
							fileStream.Position = 0L;
							fileStream2.Position = 0L;
							patchAlgorithm.CreatePatch(fileStream, fileStream2, stream);
						}
					}
					catch (Exception exception2)
					{
						_E004._E004._E006.LogError(exception2, _E05B._E000(1232), patchAlgorithm.Name, f, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream.Length), Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream2.Length));
						throw;
					}
					stream.Position = 0L;
				}
				_E002.Restart();
				try
				{
					_E004._E004._E000(text, stream, patchAlgorithm, null);
					stream.Position = 0L;
				}
				catch (Exception exception3)
				{
					_E004._E004._E006.LogError(exception3, _E05B._E000(909), text);
					throw;
				}
				_E002.Stop();
				long compressedSize;
				lock (this._E000)
				{
					long position = this._E000.Position;
					this._E000.PutNextEntry(_E003._E000(stream, f + _E05B._E000(1631)));
					stream.CopyTo(this._E000);
					this._E000.Flush();
					compressedSize = this._E000.Position - position;
					if (_E004._E006.ContainsKey(patchAlgorithm.Id))
					{
						_E004._E006[patchAlgorithm.Id]++;
					}
					else
					{
						_E004._E006.Add(patchAlgorithm.Id, 1);
					}
				}
				_E004._E007.Files.Add(new UpdateFileEntry
				{
					Path = f,
					State = UpdateFileEntryState.Modified,
					PatchAlgorithmId = patchAlgorithm.Id,
					ApplyTime = _E002.ElapsedMilliseconds,
					Size = stream.Length,
					CompressedSize = compressedSize
				});
				_E004._E004._E006.LogDebug(_E05B._E000(1023), patchAlgorithm.Name, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(stream.Length), f);
			}
			finally
			{
				stream?.Dispose();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _E000 _E000;

		internal void _E000(object sender, ExtractProgressEventArgs eArgs)
		{
			if (eArgs.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				this._E000._E000(eArgs.BytesTransferred, eArgs.TotalBytesToTransfer);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public string _E000;

		public string _E001;

		internal bool _E000(Ionic.Zip.ZipEntry e)
		{
			return e.FileName == this._E000;
		}

		internal bool _E001(Ionic.Zip.ZipEntry e)
		{
			return e.FileName == this._E001;
		}
	}

	[CompilerGenerated]
	private UpdateInstallationProblemEventHandler m__E000;

	private readonly IPatchAlgorithmProvider m__E001;

	private readonly IFileManager m__E002;

	private readonly IDownloadManagementService m__E003;

	private readonly ILauncherBackendService m__E004;

	private readonly JsonSerializerSettings m__E005;

	private readonly AppConfig _appConfig;

	private readonly ILogger<_E02A> _E006;

	public event UpdateInstallationProblemEventHandler OnUpdateInstallationProblem
	{
		[CompilerGenerated]
		add
		{
			UpdateInstallationProblemEventHandler updateInstallationProblemEventHandler = this.m__E000;
			UpdateInstallationProblemEventHandler updateInstallationProblemEventHandler2;
			do
			{
				updateInstallationProblemEventHandler2 = updateInstallationProblemEventHandler;
				UpdateInstallationProblemEventHandler value2 = (UpdateInstallationProblemEventHandler)Delegate.Combine(updateInstallationProblemEventHandler2, value);
				updateInstallationProblemEventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, updateInstallationProblemEventHandler2);
			}
			while ((object)updateInstallationProblemEventHandler != updateInstallationProblemEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			UpdateInstallationProblemEventHandler updateInstallationProblemEventHandler = this.m__E000;
			UpdateInstallationProblemEventHandler updateInstallationProblemEventHandler2;
			do
			{
				updateInstallationProblemEventHandler2 = updateInstallationProblemEventHandler;
				UpdateInstallationProblemEventHandler value2 = (UpdateInstallationProblemEventHandler)Delegate.Remove(updateInstallationProblemEventHandler2, value);
				updateInstallationProblemEventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, updateInstallationProblemEventHandler2);
			}
			while ((object)updateInstallationProblemEventHandler != updateInstallationProblemEventHandler2);
		}
	}

	public _E02A(IPatchAlgorithmProvider patchAlgorithmProvider, IFileManager fileManager, IDownloadManagementService downloadManagementService, ILauncherBackendService launcherBackendService, JsonSerializerSettings jsonSerializerSettings, AppConfig appConfig, ILogger<_E02A> logger)
	{
		this.m__E001 = patchAlgorithmProvider;
		this.m__E002 = fileManager;
		this.m__E003 = downloadManagementService;
		this.m__E004 = launcherBackendService;
		this.m__E005 = jsonSerializerSettings;
		_appConfig = appConfig;
		_E006 = logger;
	}

	public UpdateMetadata CreateUpdate(string oldApplicationRoot, BsgVersion oldApplicationVersion, string newApplicationRoot, BsgVersion newApplicationVersion, Stream updateOutputStream, CancellationToken cancellationToken)
	{
		_E006.LogInformation(_E05B._E000(14704), oldApplicationVersion.ToString(), newApplicationVersion.ToString(), oldApplicationRoot, newApplicationRoot);
		string[] source = new string[5]
		{
			_E05B._E000(14802),
			_E05B._E000(27068),
			_E05B._E000(12154),
			_E05B._E000(19925),
			_E05B._E000(14804)
		};
		UpdateMetadata updateMetadata = new UpdateMetadata
		{
			MetadataVersion = 2,
			FromVersion = oldApplicationVersion,
			ToVersion = newApplicationVersion
		};
		List<string> list = (from o in Directory.EnumerateFiles(oldApplicationRoot, _E05B._E000(27034), SearchOption.AllDirectories)
			select o.Substring(oldApplicationRoot.Length).TrimStart('\\', '/', ' ')).ToList();
		cancellationToken.ThrowIfCancellationRequested();
		List<string> list2 = (from o in Directory.EnumerateFiles(newApplicationRoot, _E05B._E000(27034), SearchOption.AllDirectories)
			select o.Substring(newApplicationRoot.Length).TrimStart('\\', '/', ' ')).ToList();
		cancellationToken.ThrowIfCancellationRequested();
		List<string> list3 = list.Except(list2).ToList();
		_E006.LogDebug(_E05B._E000(14812), list3.Count, string.Join(_E05B._E000(14362), list3));
		foreach (string item in list3)
		{
			updateMetadata.Files.Add(new UpdateFileEntry
			{
				Path = item,
				State = UpdateFileEntryState.Deleted
			});
		}
		List<string> list4 = list2.Where((string f) => source.Any((string a) => f.EndsWith(a))).ToList();
		foreach (string item2 in list4)
		{
			list2.Remove(item2);
		}
		List<string> list5 = list2.Except(list).ToList();
		list5.AddRange(list4);
		_E006.LogDebug(_E05B._E000(14367), list5.Count, string.Join(_E05B._E000(14362), list5));
		List<string> source2 = list2.Intersect(list).ToList();
		int num = 0;
		Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
		ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipOutputStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(updateOutputStream, 104857600);
		try
		{
			zipOutputStream.UseZip64 = UseZip64.Dynamic;
			zipOutputStream.SetComment(string.Format(_E05B._E000(14419), _appConfig.GameFullName, oldApplicationVersion, newApplicationVersion));
			_E02B obj = new _E02B(DateTime.Now);
			_E006.LogDebug(_E05B._E000(14504));
			foreach (string item3 in list5)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string text = Path.Combine(newApplicationRoot, item3);
				long length = new FileInfo(text).Length;
				long position = zipOutputStream.Position;
				zipOutputStream.PutNextEntry(obj._E000(text, item3));
				using (FileStream fileStream = this.m__E002.CaptureFile(text))
				{
					fileStream.CopyTo(zipOutputStream);
				}
				zipOutputStream.Flush();
				long compressedSize = zipOutputStream.Position - position;
				updateMetadata.Files.Add(new UpdateFileEntry
				{
					Path = item3,
					State = UpdateFileEntryState.New,
					Size = length,
					CompressedSize = compressedSize
				});
			}
			_E006.LogDebug(_E05B._E000(14524));
			Stopwatch stopwatch = new Stopwatch();
			ObjectPool<StreamComparator> objectPool = ObjectPool.Create<StreamComparator>();
			Parallel.ForEach(source2, new ParallelOptions
			{
				CancellationToken = cancellationToken,
				MaxDegreeOfParallelism = Math.Max(Environment.ProcessorCount / 2, 1)
			}, delegate(string f)
			{
				cancellationToken.ThrowIfCancellationRequested();
				IPatchAlgorithm patchAlgorithm = null;
				Stream stream = null;
				string text2 = Path.Combine(oldApplicationRoot, f);
				try
				{
					using (FileStream fileStream2 = this.m__E002.CaptureFile(text2))
					{
						using FileStream fileStream3 = this.m__E002.CaptureFile(Path.Combine(newApplicationRoot, f));
						StreamComparator streamComparator = objectPool.Get();
						try
						{
							if (streamComparator.Equals(fileStream2, fileStream3))
							{
								int num2 = num;
								num = num2 + 1;
								return;
							}
						}
						finally
						{
							objectPool.Return(streamComparator);
						}
						fileStream2.Position = 0L;
						fileStream3.Position = 0L;
						patchAlgorithm = this.m__E001.ChooseAnAlgorithm(fileStream2.Length, fileStream3.Length);
						stream = new MemoryStream();
						_E006.LogDebug(_E05B._E000(1254), patchAlgorithm.Name, f);
						try
						{
							try
							{
								patchAlgorithm.CreatePatch(fileStream2, fileStream3, stream);
							}
							catch (PatchAlgorithmException exception) when (patchAlgorithm.Id != 0)
							{
								_E006.LogWarning(exception, _E05B._E000(1232), patchAlgorithm.Name, f, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream2.Length), Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream3.Length));
								patchAlgorithm = this.m__E001.GetAlgorithm(0);
								_E006.LogWarning(_E05B._E000(843), patchAlgorithm.Name, f);
								stream.Position = 0L;
								stream.SetLength(0L);
								fileStream2.Position = 0L;
								fileStream3.Position = 0L;
								patchAlgorithm.CreatePatch(fileStream2, fileStream3, stream);
							}
						}
						catch (Exception exception2)
						{
							_E006.LogError(exception2, _E05B._E000(1232), patchAlgorithm.Name, f, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream2.Length), Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(fileStream3.Length));
							throw;
						}
						stream.Position = 0L;
					}
					stopwatch.Restart();
					try
					{
						_E000(text2, stream, patchAlgorithm, null);
						stream.Position = 0L;
					}
					catch (Exception exception3)
					{
						_E006.LogError(exception3, _E05B._E000(909), text2);
						throw;
					}
					stopwatch.Stop();
					long compressedSize2;
					lock (zipOutputStream)
					{
						long position2 = zipOutputStream.Position;
						zipOutputStream.PutNextEntry(obj._E000(stream, f + _E05B._E000(1631)));
						stream.CopyTo(zipOutputStream);
						zipOutputStream.Flush();
						compressedSize2 = zipOutputStream.Position - position2;
						if (dictionary.ContainsKey(patchAlgorithm.Id))
						{
							dictionary[patchAlgorithm.Id]++;
						}
						else
						{
							dictionary.Add(patchAlgorithm.Id, 1);
						}
					}
					updateMetadata.Files.Add(new UpdateFileEntry
					{
						Path = f,
						State = UpdateFileEntryState.Modified,
						PatchAlgorithmId = patchAlgorithm.Id,
						ApplyTime = stopwatch.ElapsedMilliseconds,
						Size = stream.Length,
						CompressedSize = compressedSize2
					});
					_E006.LogDebug(_E05B._E000(1023), patchAlgorithm.Name, Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(stream.Length), f);
				}
				finally
				{
					stream?.Dispose();
				}
			});
			foreach (KeyValuePair<byte, int> item4 in dictionary)
			{
				IPatchAlgorithm algorithm = this.m__E001.GetAlgorithm(item4.Key);
				_E006.LogDebug(_E05B._E000(14480), item4.Value, algorithm.Name);
			}
			_E006.LogDebug(_E05B._E000(14531), num);
			updateMetadata.Files.Sort((UpdateFileEntry x, UpdateFileEntry y) => y.ApplyTime.CompareTo(x.ApplyTime));
			string value = JsonConvert.SerializeObject(updateMetadata, this.m__E005);
			using MemoryStream memoryStream = new MemoryStream();
			using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 65536, leaveOpen: true))
			{
				streamWriter.Write(value);
			}
			memoryStream.Position = 0L;
			zipOutputStream.PutNextEntry(obj._E000(memoryStream, _E029._E000.First()));
			memoryStream.CopyTo(zipOutputStream);
		}
		finally
		{
			if (zipOutputStream != null)
			{
				((IDisposable)zipOutputStream).Dispose();
			}
		}
		_E006.LogInformation(_E05B._E000(1839), oldApplicationVersion.ToString(), newApplicationVersion.ToString());
		Directory.Delete(oldApplicationRoot, recursive: true);
		return updateMetadata;
	}

	public string CreateTxtUpdateReport(UpdateMetadata metadata)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format(_E05B._E000(1860), metadata.FromVersion));
		stringBuilder.AppendLine(string.Format(_E05B._E000(1882), metadata.ToVersion));
		UpdateFileEntry[] array = (from f in metadata.Files
			where f.State == UpdateFileEntryState.Deleted
			orderby f.Path
			select f).ToArray();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(string.Format(_E05B._E000(1962), array.Length));
		UpdateFileEntry[] array2 = array;
		foreach (UpdateFileEntry updateEntry in array2)
		{
			stringBuilder.AppendLine(_E000(updateEntry));
		}
		UpdateFileEntry[] array3 = (from f in metadata.Files
			where f.State == UpdateFileEntryState.New
			orderby f.Path
			select f).ToArray();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(string.Format(_E05B._E000(1983), array3.Length));
		foreach (UpdateFileEntry item in from f in metadata.Files
			where f.State == UpdateFileEntryState.New
			orderby f.Path
			select f)
		{
			stringBuilder.AppendLine(_E000(item));
		}
		stringBuilder.AppendLine();
		UpdateFileEntry[] array4 = (from f in metadata.Files
			where f.State == UpdateFileEntryState.Modified
			orderby f.Path
			select f).ToArray();
		stringBuilder.AppendLine(string.Format(_E05B._E000(1934), array4.Length));
		array2 = array4;
		foreach (UpdateFileEntry updateEntry2 in array2)
		{
			stringBuilder.AppendLine(_E000(updateEntry2));
		}
		return stringBuilder.ToString();
	}

	private string _E000(UpdateFileEntry updateEntry)
	{
		if (updateEntry.State != UpdateFileEntryState.Modified)
		{
			return ((updateEntry.State == UpdateFileEntryState.New) ? (Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(updateEntry.Size).PadRight(9) + _E05B._E000(2019)) : string.Empty) + updateEntry.Path;
		}
		return this.m__E001.GetAlgorithm(updateEntry.PatchAlgorithmId).Name.PadRight(9) + _E05B._E000(2019) + Bsg.Launcher.HumanReadableSizeExtensions.ToHumanReadableSize(updateEntry.Size).PadRight(9) + _E05B._E000(2019) + updateEntry.Path;
	}

	public string CreateCsvUpdateReport(UpdateMetadata metadata)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(_E05B._E000(2017));
		foreach (UpdateFileEntry file in metadata.Files)
		{
			string text = ((file.State == UpdateFileEntryState.Modified) ? this.m__E001.GetAlgorithm(file.PatchAlgorithmId).Name : _E05B._E000(22869));
			string text2 = ((file.State == UpdateFileEntryState.Modified || file.State == UpdateFileEntryState.New) ? ((double)file.Size / 1024.0 / 1024.0).ToString(_E05B._E000(1587)) : _E05B._E000(22869));
			string text3 = ((file.State == UpdateFileEntryState.Modified || file.State == UpdateFileEntryState.New) ? ((double)file.CompressedSize / 1024.0 / 1024.0).ToString(_E05B._E000(1587)) : _E05B._E000(22869));
			string text4 = ((file.State == UpdateFileEntryState.Modified) ? file.ApplyTime.ToString() : _E05B._E000(22869));
			stringBuilder.AppendLine(string.Format(_E05B._E000(1590), file.Path, text2, text3, file.State, text, text4));
		}
		return stringBuilder.ToString();
	}

	public IUpdate OpenUpdate(Stream updateStream)
	{
		return new _E029(updateStream, this.m__E005);
	}

	public void InstallUpdate(IUpdate update, string applicationRoot, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		_E006.LogInformation(_E05B._E000(1550), update.Metadata.FromVersion, update.Metadata.ToVersion);
		if (update.Metadata.MetadataVersion > 2)
		{
			throw UpdateException.TheLauncherDoesNotSupportTheInstallationOfThisUpdate();
		}
		_E000 obj;
		if (onProgress == null)
		{
			obj = null;
		}
		else
		{
			obj = new _E000(update.Metadata, onProgress);
			update.Content.ExtractProgress += delegate(object sender, ExtractProgressEventArgs eArgs)
			{
				if (eArgs.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
				{
					obj._E000(eArgs.BytesTransferred, eArgs.TotalBytesToTransfer);
				}
			};
		}
		try
		{
			Action<long, long> onProgress2 = ((obj == null) ? null : new Action<long, long>(obj._E001));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				foreach (UpdateFileEntry file in update.Metadata.Files)
				{
					cancellationToken.ThrowIfCancellationRequested();
					obj?._E000(file);
					string text = Path.Combine(applicationRoot, file.Path);
					switch (file.State)
					{
					case UpdateFileEntryState.Modified:
					{
						if (!File.Exists(text))
						{
							throw UpdateException.UnableToUpdateFile(text, _E05B._E000(1605));
						}
						string text2 = file.Path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + _E05B._E000(1631);
						Ionic.Zip.ZipEntry obj2 = update.Content.Entries.FirstOrDefault((Ionic.Zip.ZipEntry e) => e.FileName == text2) ?? throw UpdateException.UnableToUpdateFile(text, _E05B._E000(1696));
						IPatchAlgorithm algorithm = this.m__E001.GetAlgorithm(file.PatchAlgorithmId);
						memoryStream.SetLength(0L);
						obj2.Extract(memoryStream);
						memoryStream.Position = 0L;
						while (true)
						{
							try
							{
								try
								{
									_E000(text, memoryStream, algorithm, onProgress2);
								}
								catch (Exception ex) when (ex.HResult != -2147024784 && !(ex is UnauthorizedAccessException))
								{
									try
									{
										_E006.LogWarning(_E05B._E000(1723), text);
										string text3 = this.m__E004.GetGamePackage(update.Metadata.ToVersion).Result.UnpackedUri.Trim();
										if (!text3.EndsWith(_E05B._E000(30664)))
										{
											text3 += _E05B._E000(30664);
										}
										string text4 = Path.Combine(text3, file.Path).Replace('\\', '/');
										this.m__E003.DownloadFileAsync(text4.ToString(), text, onProgress2, tryUseMetadata: false, redownloadIfExist: true, cancellationToken).Wait();
										_E006.LogInformation(_E05B._E000(1764), text);
										goto end_IL_021d;
									}
									catch (Exception exception)
									{
										_E006.LogError(exception, _E05B._E000(1314), text);
									}
									throw;
									end_IL_021d:;
								}
							}
							catch (Exception error)
							{
								_E006.LogWarning(_E05B._E000(1288), text);
								bool retry = false;
								this.m__E000?.Invoke(error, ref retry);
								_E006.LogWarning(_E05B._E000(1356), retry ? _E05B._E000(1511) : _E05B._E000(1437), text);
								if (retry)
								{
									continue;
								}
								throw;
							}
							break;
						}
						break;
					}
					case UpdateFileEntryState.New:
					{
						string text5 = file.Path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
						Ionic.Zip.ZipEntry zipEntry = update.Content.Entries.FirstOrDefault((Ionic.Zip.ZipEntry e) => e.FileName == text5) ?? throw UpdateException.UnableToUpdateFile(text, _E05B._E000(1513));
						Directory.CreateDirectory(Path.GetDirectoryName(text));
						if (File.Exists(text))
						{
							this.m__E002.Delete(text);
						}
						using (FileStream stream = this.m__E002.CaptureFile(text, FileMode.CreateNew, FileAccess.Write, FileShare.None))
						{
							zipEntry.Extract(stream);
						}
						break;
					}
					case UpdateFileEntryState.Deleted:
						if (File.Exists(text))
						{
							this.m__E002.Delete(text);
						}
						break;
					}
					obj?._E000();
				}
			}
			_E006.LogInformation(_E05B._E000(1534), update.Metadata.FromVersion, update.Metadata.ToVersion);
		}
		finally
		{
			obj?.Dispose();
		}
	}

	private void _E000(string targetFilePath, Stream patchStream, IPatchAlgorithm patchAlgorithm, Action<long, long> onProgress)
	{
		string text = targetFilePath + _E05B._E000(1084);
		try
		{
			this.m__E002.Move(targetFilePath, text, overwrite: true);
			using (FileStream originalFileStream = this.m__E002.CaptureFile(text))
			{
				using FileStream resultFileStream = this.m__E002.CaptureFile(targetFilePath, FileMode.CreateNew, FileAccess.ReadWrite);
				patchAlgorithm.ApplyPatch(originalFileStream, patchStream, resultFileStream, onProgress);
			}
			this.m__E002.Delete(text);
		}
		catch (Exception ex)
		{
			_E006.LogError(ex, _E05B._E000(1031), targetFilePath);
			if (ex.HResult == -2147024784)
			{
				ex.AddDiskLetter(targetFilePath);
			}
			try
			{
				this.m__E002.Move(text, targetFilePath, overwrite: true);
				_E006.LogInformation(_E05B._E000(1088), targetFilePath);
			}
			catch (Exception exception)
			{
				_E006.LogError(exception, _E05B._E000(1195), targetFilePath);
			}
			throw;
		}
	}
}
internal class _E02B
{
	private readonly DateTime m__E000;

	public _E02B(DateTime dateTime)
	{
		this.m__E000 = dateTime;
	}

	public ICSharpCode.SharpZipLib.Zip.ZipEntry _E000(string filePath, string entryName)
	{
		return new ICSharpCode.SharpZipLib.Zip.ZipEntry(entryName)
		{
			DateTime = this.m__E000,
			Size = new FileInfo(filePath).Length
		};
	}

	public ICSharpCode.SharpZipLib.Zip.ZipEntry _E000(Stream stream, string entryName)
	{
		return new ICSharpCode.SharpZipLib.Zip.ZipEntry(entryName)
		{
			DateTime = this.m__E000,
			Size = stream.Length
		};
	}
}
internal static class _E02C
{
	private const long m__E000 = 3473478480300364610L;

	private const int m__E001 = 32;

	public static void _E000(byte[] oldData, byte[] newData, Stream output, CancellationToken cancellationToken)
	{
		if (oldData == null)
		{
			throw new ArgumentNullException(_E05B._E000(557));
		}
		if (newData == null)
		{
			throw new ArgumentNullException(_E05B._E000(565));
		}
		if (output == null)
		{
			throw new ArgumentNullException(_E05B._E000(573));
		}
		if (!output.CanSeek)
		{
			throw new ArgumentException(_E05B._E000(518), _E05B._E000(573));
		}
		if (!output.CanWrite)
		{
			throw new ArgumentException(_E05B._E000(614), _E05B._E000(573));
		}
		byte[] array = new byte[32];
		_E000(3473478480300364610L, array, 0);
		_E000(0L, array, 8);
		_E000(0L, array, 16);
		_E000(newData.Length, array, 24);
		long position = output.Position;
		output.Write(array, 0, array.Length);
		int[] i = _E000(oldData);
		byte[] array2 = new byte[newData.Length];
		byte[] array3 = new byte[newData.Length];
		int num = 0;
		int num2 = 0;
		using (BZip2OutputStream bZip2OutputStream = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			int j = 0;
			int pos = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			while (j < newData.Length)
			{
				cancellationToken.ThrowIfCancellationRequested();
				int num7 = 0;
				int k = (j += num3);
				for (; j < newData.Length; j++)
				{
					for (num3 = _E000(i, oldData, newData, j, 0, oldData.Length, out pos); k < j + num3; k++)
					{
						if (k + num6 < oldData.Length && oldData[k + num6] == newData[k])
						{
							num7++;
						}
					}
					if ((num3 == num7 && num3 != 0) || num3 > num7 + 8)
					{
						break;
					}
					if (j + num6 < oldData.Length && oldData[j + num6] == newData[j])
					{
						num7--;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				if (num3 == num7 && j != newData.Length)
				{
					continue;
				}
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				while (num4 + num11 < j && num5 + num11 < oldData.Length)
				{
					if (oldData[num5 + num11] == newData[num4 + num11])
					{
						num8++;
					}
					num11++;
					if (num8 * 2 - num11 > num9 * 2 - num10)
					{
						num9 = num8;
						num10 = num11;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				int num12 = 0;
				if (j < newData.Length)
				{
					num8 = 0;
					int num13 = 0;
					for (int l = 1; j >= num4 + l && pos >= l; l++)
					{
						if (oldData[pos - l] == newData[j - l])
						{
							num8++;
						}
						if (num8 * 2 - l > num13 * 2 - num12)
						{
							num13 = num8;
							num12 = l;
						}
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				if (num4 + num10 > j - num12)
				{
					int num14 = num4 + num10 - (j - num12);
					num8 = 0;
					int num15 = 0;
					int num16 = 0;
					for (int m = 0; m < num14; m++)
					{
						if (newData[num4 + num10 - num14 + m] == oldData[num5 + num10 - num14 + m])
						{
							num8++;
						}
						if (newData[j - num12 + m] == oldData[pos - num12 + m])
						{
							num8--;
						}
						if (num8 > num15)
						{
							num15 = num8;
							num16 = m + 1;
						}
						cancellationToken.ThrowIfCancellationRequested();
					}
					num10 += num16 - num14;
					num12 -= num16;
				}
				for (int n = 0; n < num10; n++)
				{
					array2[num + n] = (byte)(newData[num4 + n] - oldData[num5 + n]);
				}
				for (int num17 = 0; num17 < j - num12 - (num4 + num10); num17++)
				{
					array3[num2 + num17] = newData[num4 + num10 + num17];
				}
				num += num10;
				num2 += j - num12 - (num4 + num10);
				byte[] array4 = new byte[8];
				_E000(num10, array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				_E000(j - num12 - (num4 + num10), array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				_E000(pos - num12 - (num5 + num10), array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				num4 = j - num12;
				num5 = pos - num12;
				num6 = pos - j;
			}
		}
		long position2 = output.Position;
		_E000(position2 - position - 32, array, 8);
		using (BZip2OutputStream bZip2OutputStream2 = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			bZip2OutputStream2.Write(array2, 0, num);
		}
		_E000(output.Position - position2, array, 16);
		using (BZip2OutputStream bZip2OutputStream3 = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			bZip2OutputStream3.Write(array3, 0, num2);
		}
		long position3 = output.Position;
		output.Position = position;
		output.Write(array, 0, array.Length);
		output.Position = position3;
	}

	public static void _E000(Stream input, Func<Stream> openPatchStream, Stream output)
	{
		if (input == null)
		{
			throw new ArgumentNullException(_E05B._E000(582));
		}
		if (openPatchStream == null)
		{
			throw new ArgumentNullException(_E05B._E000(584));
		}
		if (output == null)
		{
			throw new ArgumentNullException(_E05B._E000(573));
		}
		long num;
		long num2;
		long num3;
		using (Stream stream = openPatchStream())
		{
			if (!stream.CanRead)
			{
				throw new ArgumentException(_E05B._E000(600), _E05B._E000(584));
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException(_E05B._E000(697), _E05B._E000(584));
			}
			byte[] buf = _E000(stream, 32);
			if (_E000(buf, 0) != 3473478480300364610L)
			{
				throw new InvalidOperationException(_E05B._E000(666));
			}
			num = _E000(buf, 8);
			num2 = _E000(buf, 16);
			num3 = _E000(buf, 24);
			if (num < 0 || num2 < 0 || num3 < 0)
			{
				throw new InvalidOperationException(_E05B._E000(666));
			}
		}
		byte[] array = new byte[1048576];
		byte[] array2 = new byte[1048576];
		using Stream stream2 = openPatchStream();
		using Stream stream3 = openPatchStream();
		using Stream stream4 = openPatchStream();
		stream2.Seek(32L, SeekOrigin.Current);
		stream3.Seek(32 + num, SeekOrigin.Current);
		stream4.Seek(32 + num + num2, SeekOrigin.Current);
		using BZip2InputStream stream5 = new BZip2InputStream(stream2);
		using BZip2InputStream stream6 = new BZip2InputStream(stream3);
		using BZip2InputStream stream7 = new BZip2InputStream(stream4);
		long[] array3 = new long[3];
		byte[] array4 = new byte[8];
		int num4 = 0;
		int num5 = 0;
		while (num5 < num3)
		{
			for (int i = 0; i < 3; i++)
			{
				_E000(stream5, array4, 0, 8);
				array3[i] = _E000(array4, 0);
			}
			if (num5 + array3[0] > num3)
			{
				throw new InvalidOperationException(_E05B._E000(666));
			}
			input.Position = num4;
			int num6 = (int)array3[0];
			while (num6 > 0)
			{
				int num7 = Math.Min(num6, 1048576);
				_E000(stream6, array, 0, num7);
				int num8 = Math.Min(num7, (int)(input.Length - input.Position));
				_E000(input, array2, 0, num8);
				for (int j = 0; j < num8; j++)
				{
					array[j] += array2[j];
				}
				output.Write(array, 0, num7);
				num5 += num7;
				num4 += num7;
				num6 -= num7;
			}
			if (num5 + array3[1] > num3)
			{
				throw new InvalidOperationException(_E05B._E000(666));
			}
			num6 = (int)array3[1];
			while (num6 > 0)
			{
				int num9 = Math.Min(num6, 1048576);
				_E000(stream7, array, 0, num9);
				output.Write(array, 0, num9);
				num5 += num9;
				num6 -= num9;
			}
			num4 = (int)(num4 + array3[2]);
		}
	}

	private static int _E000(byte[] left, int leftOffset, byte[] right, int rightOffset)
	{
		for (int i = 0; i < left.Length - leftOffset && i < right.Length - rightOffset; i++)
		{
			int num = left[i + leftOffset] - right[i + rightOffset];
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}

	private static int _E001(byte[] oldData, int oldOffset, byte[] newData, int newOffset)
	{
		int i;
		for (i = 0; i < oldData.Length - oldOffset && i < newData.Length - newOffset && oldData[i + oldOffset] == newData[i + newOffset]; i++)
		{
		}
		return i;
	}

	private static int _E000(int[] I, byte[] oldData, byte[] newData, int newOffset, int start, int end, out int pos)
	{
		if (end - start < 2)
		{
			int num = _E001(oldData, I[start], newData, newOffset);
			int num2 = _E001(oldData, I[end], newData, newOffset);
			if (num > num2)
			{
				pos = I[start];
				return num;
			}
			pos = I[end];
			return num2;
		}
		int num3 = start + (end - start) / 2;
		if (_E000(oldData, I[num3], newData, newOffset) >= 0)
		{
			return _E000(I, oldData, newData, newOffset, start, num3, out pos);
		}
		return _E000(I, oldData, newData, newOffset, num3, end, out pos);
	}

	private static void _E000(int[] I, int[] v, int start, int len, int h)
	{
		if (len < 16)
		{
			int num;
			for (int i = start; i < start + len; i += num)
			{
				num = 1;
				int num2 = v[I[i] + h];
				for (int j = 1; i + j < start + len; j++)
				{
					if (v[I[i + j] + h] < num2)
					{
						num2 = v[I[i + j] + h];
						num = 0;
					}
					if (v[I[i + j] + h] == num2)
					{
						_E000(ref I[i + num], ref I[i + j]);
						num++;
					}
				}
				for (int k = 0; k < num; k++)
				{
					v[I[i + k]] = i + num - 1;
				}
				if (num == 1)
				{
					I[i] = -1;
				}
			}
			return;
		}
		int num3 = v[I[start + len / 2] + h];
		int num4 = 0;
		int num5 = 0;
		for (int l = start; l < start + len; l++)
		{
			if (v[I[l] + h] < num3)
			{
				num4++;
			}
			if (v[I[l] + h] == num3)
			{
				num5++;
			}
		}
		num4 += start;
		num5 += num4;
		int num6 = start;
		int num7 = 0;
		int num8 = 0;
		while (num6 < num4)
		{
			if (v[I[num6] + h] < num3)
			{
				num6++;
			}
			else if (v[I[num6] + h] == num3)
			{
				_E000(ref I[num6], ref I[num4 + num7]);
				num7++;
			}
			else
			{
				_E000(ref I[num6], ref I[num5 + num8]);
				num8++;
			}
		}
		while (num4 + num7 < num5)
		{
			if (v[I[num4 + num7] + h] == num3)
			{
				num7++;
				continue;
			}
			_E000(ref I[num4 + num7], ref I[num5 + num8]);
			num8++;
		}
		if (num4 > start)
		{
			_E000(I, v, start, num4 - start, h);
		}
		for (num6 = 0; num6 < num5 - num4; num6++)
		{
			v[I[num4 + num6]] = num5 - 1;
		}
		if (num4 == num5 - 1)
		{
			I[num4] = -1;
		}
		if (start + len > num5)
		{
			_E000(I, v, num5, start + len - num5, h);
		}
	}

	private static int[] _E000(byte[] oldData)
	{
		int[] array = new int[256];
		foreach (byte b in oldData)
		{
			array[b]++;
		}
		for (int j = 1; j < 256; j++)
		{
			array[j] += array[j - 1];
		}
		for (int num = 255; num > 0; num--)
		{
			array[num] = array[num - 1];
		}
		array[0] = 0;
		int[] array2 = new int[oldData.Length + 1];
		for (int k = 0; k < oldData.Length; k++)
		{
			array2[++array[oldData[k]]] = k;
		}
		int[] array3 = new int[oldData.Length + 1];
		for (int l = 0; l < oldData.Length; l++)
		{
			array3[l] = array[oldData[l]];
		}
		for (int m = 1; m < 256; m++)
		{
			if (array[m] == array[m - 1] + 1)
			{
				array2[array[m]] = -1;
			}
		}
		array2[0] = -1;
		int num2 = 1;
		while (array2[0] != -(oldData.Length + 1))
		{
			int num3 = 0;
			int num4 = 0;
			while (num4 < oldData.Length + 1)
			{
				if (array2[num4] < 0)
				{
					num3 -= array2[num4];
					num4 -= array2[num4];
					continue;
				}
				if (num3 != 0)
				{
					array2[num4 - num3] = -num3;
				}
				num3 = array3[array2[num4]] + 1 - num4;
				_E000(array2, array3, num4, num3, num2);
				num4 += num3;
				num3 = 0;
			}
			if (num3 != 0)
			{
				array2[num4 - num3] = -num3;
			}
			num2 += num2;
		}
		for (int n = 0; n < oldData.Length + 1; n++)
		{
			array2[array3[n]] = n;
		}
		return array2;
	}

	private static void _E000(ref int first, ref int second)
	{
		int num = first;
		first = second;
		second = num;
	}

	private static long _E000(byte[] buf, int offset)
	{
		long num = buf[offset + 7] & 0x7F;
		for (int num2 = 6; num2 >= 0; num2--)
		{
			num *= 256;
			num += buf[offset + num2];
		}
		if ((buf[offset + 7] & 0x80u) != 0)
		{
			num = -num;
		}
		return num;
	}

	private static void _E000(long value, byte[] buf, int offset)
	{
		long num = ((value < 0) ? (-value) : value);
		for (int i = 0; i < 8; i++)
		{
			buf[offset + i] = (byte)num;
			num >>= 8;
		}
		if (value < 0)
		{
			buf[offset + 7] |= 128;
		}
	}

	private static byte[] _E000(Stream stream, int count)
	{
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(747));
		}
		byte[] array = new byte[count];
		_E000(stream, array, 0, count);
		return array;
	}

	private static void _E000(Stream stream, byte[] buffer, int offset, int count)
	{
		if (stream == null)
		{
			throw new ArgumentNullException(_E05B._E000(749));
		}
		if (buffer == null)
		{
			throw new ArgumentNullException(_E05B._E000(758));
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(767));
		}
		if (count < 0 || buffer.Length - offset < count)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(747));
		}
		while (count > 0)
		{
			int num = stream.Read(buffer, offset, count);
			if (num == 0)
			{
				throw new EndOfStreamException();
			}
			offset += num;
			count -= num;
		}
	}
}
internal class _E02D : IPatchAlgorithm
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Action<long, long> _E000;

		internal void _E000(object sender, ProgressReport report)
		{
			this._E000?.Invoke(report.CurrentPosition, report.Total);
		}
	}

	public byte Id => 0;

	public string Name => _E05B._E000(728);

	public void ApplyPatch(Stream originalFileStream, Stream patchStream, Stream resultFileStream, Action<long, long> onProgress)
	{
		Progress<ProgressReport> progress = new Progress<ProgressReport>();
		progress.ProgressChanged += delegate(object sender, ProgressReport report)
		{
			onProgress?.Invoke(report.CurrentPosition, report.Total);
		};
		BinaryDeltaReader delta = new BinaryDeltaReader(patchStream, progress);
		DeltaApplier deltaApplier = new DeltaApplier();
		deltaApplier.SkipHashCheck = false;
		deltaApplier.Apply(originalFileStream, delta, resultFileStream);
	}

	public void CreatePatch(Stream oldFileStream, Stream newFileStream, Stream patchOutputStream)
	{
		SignatureBuilder signatureBuilder = new SignatureBuilder();
		using MemoryStream memoryStream = new MemoryStream();
		signatureBuilder.Build(oldFileStream, new SignatureWriter(memoryStream));
		memoryStream.Seek(0L, SeekOrigin.Begin);
		new DeltaBuilder().BuildDelta(deltaWriter: new AggregateCopyOperationsDecorator(new BinaryDeltaWriter(patchOutputStream)), newFileStream: newFileStream, signatureReader: new SignatureReader(memoryStream, null));
	}

	public bool IsApplicableFor(long oldFileSize, long newFileSize)
	{
		return true;
	}

	public override string ToString()
	{
		return Name + _E05B._E000(713);
	}
}
internal class _E02E : IWebSocketChannel, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000<_E005>
	{
		public Action<_E005> _E000;

		internal void _E000(object wsm)
		{
			this._E000((_E005)wsm);
		}
	}

	[CompilerGenerated]
	private sealed class _E001<_E005>
	{
		public TaskCompletionSource<_E005> _E000;

		public _E02E _E001;

		internal void _E000()
		{
			this._E000.SetException(this._E001._connectionFailedException);
		}

		internal void _E001()
		{
			this._E000.TrySetCanceled();
		}

		internal void _E002()
		{
			this._E000.TrySetCanceled();
		}

		internal void _E000(_E005 m)
		{
			this._E000.SetResult(m);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public ArraySegment<byte> _E000;

		public _E02E _E001;
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public WebSocketReceiveResult _E000;

		public _E003 _E001;

		internal void _E000(object _)
		{
			_E001._E001._E000(Encoding.UTF8.GetString(_E001._E000.Array, 0, this._E000.Count));
		}
	}

	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private readonly string m__E001;

	[CompilerGenerated]
	private readonly Uri _E002;

	private readonly WebSocketSettings m__E003;

	private readonly Microsoft.Extensions.Logging.ILogger m__E004;

	private readonly Dictionary<Type, List<_E030>> _E005;

	private readonly CancellationTokenSource _E006;

	private readonly CancellationTokenSource _E007;

	private WebSocket _E008;

	private Exception _connectionFailedException;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return m__E001;
		}
	}

	public Uri ChannelUri
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public event Action OnChannelClosed
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E02E(Uri channelUri, WebSocketSettings settings, CancellationToken cancellationToken, Microsoft.Extensions.Logging.ILogger logger, string name = null)
	{
		_E002 = channelUri;
		this.m__E003 = settings;
		this.m__E004 = logger;
		_E005 = new Dictionary<Type, List<_E030>>();
		_E007 = new CancellationTokenSource();
		cancellationToken.Register(_E007.Cancel);
		_E006 = new CancellationTokenSource();
		m__E001 = channelUri.ToString();
		ThreadPool.QueueUserWorkItem(delegate
		{
			_E000();
		});
	}

	public void Dispose()
	{
		if (!_E006.IsCancellationRequested)
		{
			_E007.Cancel();
		}
	}

	public IDisposable Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, [CallerMemberName] string subscriberName = "anonymous")
	{
		Type typeFromHandle = typeof(TMessage);
		_E030 obj = new _E030(typeFromHandle, delegate(object wsm)
		{
			messageReceivedCallback((TMessage)wsm);
		}, _E000, subscriberName);
		if (!_E005.TryGetValue(typeFromHandle, out var value))
		{
			value = new List<_E030>();
			_E005.Add(typeFromHandle, value);
		}
		value.Add(obj);
		return obj;
	}

	public async Task<TMessage> WaitForMessageAsync<TMessage>(CancellationToken cancellationToken = default(CancellationToken))
	{
		_E007.Token.ThrowIfCancellationRequested();
		TaskCompletionSource<TMessage> taskCompletionSource = new TaskCompletionSource<TMessage>();
		using (_E006.Token.Register(delegate
		{
			taskCompletionSource.SetException(_connectionFailedException);
		}))
		{
			using (_E007.Token.Register(delegate
			{
				taskCompletionSource.TrySetCanceled();
			}))
			{
				using (cancellationToken.Register(delegate
				{
					taskCompletionSource.TrySetCanceled();
				}))
				{
					using (Subscribe(delegate(TMessage m)
					{
						taskCompletionSource.SetResult(m);
					}, _E05B._E000(3689)))
					{
						return await taskCompletionSource.Task;
					}
				}
			}
		}
	}

	private void _E000(_E030 subscription)
	{
		if (_E005.TryGetValue(subscription._E000, out var value))
		{
			value.Remove(subscription);
		}
	}

	private void _E000()
	{
		try
		{
			this.m__E004.LogDebug(_E05B._E000(294), Name);
			WebSocketException ex3 = default(WebSocketException);
			for (int i = 0; i <= this.m__E003.ReconnectionAttempts.Length; i++)
			{
				try
				{
					try
					{
						_E008 = new System.Net.WebSockets.ClientWebSocket();
					}
					catch (PlatformNotSupportedException)
					{
						this.m__E004.LogInformation(_E05B._E000(263), Environment.OSVersion.VersionString);
						_E008 = new System.Net.WebSockets.Managed.ClientWebSocket();
					}
					_E008.ConnectAsync(ChannelUri, _E007.Token).Wait();
					i = 0;
					this.m__E004.LogInformation(_E05B._E000(330), Name);
					ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1048576]);
					while (_E008.State == WebSocketState.Open)
					{
						WebSocketReceiveResult result = _E008.ReceiveAsync(buffer, _E007.Token).Result;
						if (result.MessageType == WebSocketMessageType.Close)
						{
							this.m__E004.LogWarning(_E05B._E000(420), Name, _E008.State, _E008.CloseStatus, _E008.CloseStatusDescription);
							return;
						}
						if (!result.EndOfMessage)
						{
							throw new Exception(_E05B._E000(7));
						}
						ThreadPool.QueueUserWorkItem(delegate
						{
							_E000(Encoding.UTF8.GetString(buffer.Array, 0, result.Count));
						});
					}
				}
				catch (AggregateException ex2) when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					ex3 = ex2.InnerException as WebSocketException;
					return ex3 != null && !ex2.Contains((WebException e) => e.Status == WebExceptionStatus.RequestCanceled);
				}).Invoke())
				{
					_E008.Dispose();
					if (i < this.m__E003.ReconnectionAttempts.Length)
					{
						int num = this.m__E003.ReconnectionAttempts[i];
						string message = ((ex3.InnerException == null) ? ex3 : ex3.InnerException).Message;
						this.m__E004.LogWarning(_E05B._E000(106), Name, message, num);
						Thread.Sleep(num);
						continue;
					}
					this.m__E004.LogWarning(ex3, _E05B._E000(229), Name);
					throw ex3;
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (AggregateException ex5) when (ex5.InnerException is TaskCanceledException || ex5.Contains((WebException e) => e.Status == WebExceptionStatus.RequestCanceled))
		{
		}
		catch (Exception ex6)
		{
			this.m__E004.LogError(ex6, _E05B._E000(3889), Name);
			_connectionFailedException = ex6;
			_E006.Cancel();
		}
		finally
		{
			try
			{
				this.m__E004.LogDebug(_E05B._E000(3857), Name);
				if (_E008 != null)
				{
					if (_E008.State == WebSocketState.Open || _E008.State == WebSocketState.Connecting || _E008.State == WebSocketState.CloseReceived)
					{
						_E008.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
					}
					_E008.Dispose();
				}
				_E007.Cancel();
				this.m__E004.LogInformation(_E05B._E000(3954), Name);
			}
			finally
			{
				this.m__E000?.Invoke();
			}
		}
	}

	private void _E000(string messageString)
	{
		object obj;
		try
		{
			JObject jObject = JObject.Parse(messageString);
			if (!jObject.ContainsKey(_E05B._E000(3916)))
			{
				JToken jToken = jObject.SelectToken(_E05B._E000(13272));
				if (jToken != null)
				{
					jObject.AddFirst(new JProperty(_E05B._E000(3916), jToken));
					jObject.Remove(_E05B._E000(13272));
				}
			}
			obj = jObject.ToObject<object>(this.m__E003.Serializer);
			this.m__E004.LogDebug(_E05B._E000(3926), Name, messageString.Trim(' ', '\r', '\n'));
		}
		catch (JsonReaderException) when (!messageString.TrimStart(' ').StartsWith(_E05B._E000(4026)))
		{
			this.m__E004.LogWarning(_E05B._E000(4024), messageString.Trim(' ', '\r', '\n'));
			return;
		}
		catch (Exception exception)
		{
			this.m__E004.LogError(exception, _E05B._E000(3989), messageString.Trim(' ', '\r', '\n'));
			return;
		}
		if (!_E005.TryGetValue(obj.GetType(), out var value))
		{
			return;
		}
		_E030[] array = value.ToArray();
		foreach (_E030 obj2 in array)
		{
			try
			{
				obj2._E000(obj);
			}
			catch (Exception exception2)
			{
				this.m__E004.LogError(exception2, _E05B._E000(4043), Name, obj2._E000, obj2._E000.Name);
			}
		}
	}

	[CompilerGenerated]
	private void _E000(object _)
	{
		_E000();
	}
}
internal class _E02F : IWebSocketClientFactory
{
	private readonly ILoggerFactory _E000;

	private readonly WebSocketSettings _E001;

	public _E02F(ILoggerFactory loggerFactory, WebSocketSettings settings)
	{
		_E000 = loggerFactory;
		_E001 = settings;
	}

	public IWebSocketChannel CreateChannel(Uri channelUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new _E02E(channelUri, _E001, cancellationToken, _E000.CreateLogger(_E05B._E000(3709) + channelUri.Host));
	}
}
internal class _E030 : IDisposable
{
	[CompilerGenerated]
	private readonly Type m__E000;

	[CompilerGenerated]
	private readonly Action<object> _E001;

	[CompilerGenerated]
	private readonly string _E002;

	private readonly Action<_E030> _E003;

	public Type _E000
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	public Action<object> _E000
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public string _E000
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public _E030(Type messageType, Action<object> callback, Action<_E030> unsubscribeAction, string subscriberName)
	{
		this.m__E000 = messageType;
		_E001 = callback;
		_E002 = subscriberName;
		_E003 = unsubscribeAction;
	}

	public void Dispose()
	{
		_E003?.Invoke(this);
	}
}
internal class _E031 : IQueueHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E031 _E000;

		public QueueInfo _E001;

		internal void _E000()
		{
			Task.Run((Func<Task>)this._E000._E004.CancelQueueWaiting).Wait();
		}

		internal void _E000(QueueWaitingStatusChangedWsMessage statusMessage)
		{
			_E001.Position = Math.Max(1, _E001.Position - statusMessage.PassedUsersCount);
			int estimatedWaitingTime = (int)Math.Max(-1.0, (double)_E001.Position * statusMessage.AverageWaitingTimeByUserSec);
			this._E000._E001?.Invoke(_E001.Position, estimatedWaitingTime);
		}
	}

	[CompilerGenerated]
	private QueueStateChangedEventHandler m__E000;

	[CompilerGenerated]
	private QueueProgressEventHandler _E001;

	private readonly ILogger<_E031> _E002;

	private readonly IWebSocketClientFactory _E003;

	private readonly IGameBackendService _E004;

	public event QueueStateChangedEventHandler OnQueueStateChanged
	{
		[CompilerGenerated]
		add
		{
			QueueStateChangedEventHandler queueStateChangedEventHandler = this.m__E000;
			QueueStateChangedEventHandler queueStateChangedEventHandler2;
			do
			{
				queueStateChangedEventHandler2 = queueStateChangedEventHandler;
				QueueStateChangedEventHandler value2 = (QueueStateChangedEventHandler)Delegate.Combine(queueStateChangedEventHandler2, value);
				queueStateChangedEventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, queueStateChangedEventHandler2);
			}
			while ((object)queueStateChangedEventHandler != queueStateChangedEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			QueueStateChangedEventHandler queueStateChangedEventHandler = this.m__E000;
			QueueStateChangedEventHandler queueStateChangedEventHandler2;
			do
			{
				queueStateChangedEventHandler2 = queueStateChangedEventHandler;
				QueueStateChangedEventHandler value2 = (QueueStateChangedEventHandler)Delegate.Remove(queueStateChangedEventHandler2, value);
				queueStateChangedEventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, queueStateChangedEventHandler2);
			}
			while ((object)queueStateChangedEventHandler != queueStateChangedEventHandler2);
		}
	}

	public event QueueProgressEventHandler OnQueueProgress
	{
		[CompilerGenerated]
		add
		{
			QueueProgressEventHandler queueProgressEventHandler = this._E001;
			QueueProgressEventHandler queueProgressEventHandler2;
			do
			{
				queueProgressEventHandler2 = queueProgressEventHandler;
				QueueProgressEventHandler value2 = (QueueProgressEventHandler)Delegate.Combine(queueProgressEventHandler2, value);
				queueProgressEventHandler = Interlocked.CompareExchange(ref this._E001, value2, queueProgressEventHandler2);
			}
			while ((object)queueProgressEventHandler != queueProgressEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			QueueProgressEventHandler queueProgressEventHandler = this._E001;
			QueueProgressEventHandler queueProgressEventHandler2;
			do
			{
				queueProgressEventHandler2 = queueProgressEventHandler;
				QueueProgressEventHandler value2 = (QueueProgressEventHandler)Delegate.Remove(queueProgressEventHandler2, value);
				queueProgressEventHandler = Interlocked.CompareExchange(ref this._E001, value2, queueProgressEventHandler2);
			}
			while ((object)queueProgressEventHandler != queueProgressEventHandler2);
		}
	}

	public _E031(ILogger<_E031> logger, IWebSocketClientFactory webSocketClientFactory, IGameBackendService gameBackendService)
	{
		_E002 = logger;
		_E003 = webSocketClientFactory;
		_E004 = gameBackendService;
	}

	public async Task<int> WaitForQueueAsync(QueueInfo queueInfo, CancellationToken cancellationToken)
	{
		_E002.LogDebug(_E05B._E000(3371));
		cancellationToken.Register(delegate
		{
			Task.Run((Func<Task>)_E004.CancelQueueWaiting).Wait();
		});
		try
		{
			this.m__E000?.Invoke(isInTheQueue: true);
			this._E001?.Invoke(queueInfo.Position, -1);
			using IWebSocketChannel webSocketChannel2 = _E003.CreateChannel(queueInfo.PrivateUrl, cancellationToken);
			using IWebSocketChannel webSocketChannel = _E003.CreateChannel(queueInfo.PublicUrl, cancellationToken);
			webSocketChannel.OnChannelClosed += webSocketChannel2.Dispose;
			using (webSocketChannel.Subscribe(delegate(QueueWaitingStatusChangedWsMessage statusMessage)
			{
				queueInfo.Position = Math.Max(1, queueInfo.Position - statusMessage.PassedUsersCount);
				int estimatedWaitingTime = (int)Math.Max(-1.0, (double)queueInfo.Position * statusMessage.AverageWaitingTimeByUserSec);
				this._E001?.Invoke(queueInfo.Position, estimatedWaitingTime);
			}, _E05B._E000(3386)))
			{
				QueueWaitingCompletedWsMessage obj = await webSocketChannel2.WaitForMessageAsync<QueueWaitingCompletedWsMessage>(cancellationToken);
				_E002.LogDebug(_E05B._E000(3336));
				return obj.TimeToStartGameSec;
			}
		}
		catch (OperationCanceledException)
		{
			_E002.LogInformation(_E05B._E000(3424));
			throw;
		}
		catch (Exception exception)
		{
			_E002.LogError(exception, _E05B._E000(3392));
			if (!cancellationToken.IsCancellationRequested)
			{
				await _E004.CancelQueueWaiting();
			}
			throw;
		}
		finally
		{
			this.m__E000?.Invoke(isInTheQueue: false);
		}
	}
}
internal class _E032 : IDownloadManagementHandler
{
	public void OnDownloadError(Exception exception, ref bool retry)
	{
	}
}
internal class _E033 : IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E033 _E000;

		public int _E001;

		internal void _E000(long bytesDownloaded, long totalSize)
		{
			this._E000._E001[_E001] = bytesDownloaded;
			this._E000._E002[_E001] = totalSize;
		}
	}

	private readonly Action<long, long> m__E000;

	private readonly long[] _E001;

	private readonly long[] _E002;

	private readonly Metronome _E003;

	public _E033(Action<long, long> callback, int entitiesCount, TimeSpan progressInterval)
	{
		_E001 = new long[entitiesCount];
		_E002 = new long[entitiesCount];
		this.m__E000 = callback;
		_E003 = new Metronome(progressInterval, _E000, typeof(_E033).Name);
		_E003.Start();
	}

	public Action<long, long> _E000(int entityIndex)
	{
		return delegate(long bytesDownloaded, long totalSize)
		{
			_E001[entityIndex] = bytesDownloaded;
			_E002[entityIndex] = totalSize;
		};
	}

	public void _E000(int entityIndex, long totalSize)
	{
		_E002[entityIndex] = totalSize;
		_E000();
	}

	public void _E000()
	{
		this.m__E000?.Invoke(_E001.Sum(), _E002.Sum());
	}

	public void Dispose()
	{
		_E003.Dispose();
	}
}
internal class _E034 : IDownloadManagementService, IService
{
	[CompilerGenerated]
	private sealed class _E002
	{
		public IReadOnlyList<(string relativeUri, string destinationPath)> _E000;

		public CancellationToken _E001;

		public _E034 _E002;

		public IMultichannelDownloader _E003;

		public _E033 _E004;

		internal async void _E000(int i)
		{
			(string relativeUri, string destinationPath) tuple = this._E000[i];
			try
			{
				long totalSize = await _E003.GetFileSizeAsync(tuple.relativeUri, _E001);
				_E004._E000(i, totalSize);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception exception)
			{
				_E002._E00B.LogError(exception, _E05B._E000(3560), tuple.relativeUri);
			}
		}
	}

	private const int m__E000 = 3;

	private const string _E001 = ".bsgp";

	private const string m__E002 = ".bsgu";

	private const string _E003 = ".bsgf";

	private const string _E004 = ".mcd";

	private readonly IDownloadManagementHandler _E005;

	private readonly AppConfig _appConfig;

	private readonly ISettingsService _settingsService;

	private readonly IFileManager _E006;

	private readonly IMultichannelDownloaderFactory _E007;

	private readonly Lazy<IGameService> _E008;

	private readonly Lazy<IGameUpdateService> _gameUpdateServiceLazy;

	private readonly Utils _E009;

	private readonly TimeSpan _E00A;

	private readonly Microsoft.Extensions.Logging.ILogger _E00B;

	private readonly ConcurrentDictionary<string, Task> _E00C = new ConcurrentDictionary<string, Task>();

	private readonly SemaphoreSlim _E00D = new SemaphoreSlim(3);

	public _E034(IDownloadManagementHandler downloadManagementHandler, AppConfig appConfig, ISettingsService settingsService, IFileManager fileManager, IMultichannelDownloaderFactory multichannelDownloaderFactory, Lazy<IGameService> gameServiceLazy, Lazy<IGameUpdateService> gameUpdateServiceLazy, MultichannelDownloaderOptions multichannelDownloaderOptions, Utils utils, ILogger<_E034> logger)
	{
		this._E005 = downloadManagementHandler;
		_appConfig = appConfig;
		_settingsService = settingsService;
		_E006 = fileManager;
		_E007 = multichannelDownloaderFactory;
		_E008 = gameServiceLazy;
		_gameUpdateServiceLazy = gameUpdateServiceLazy;
		_E009 = utils;
		_E00A = TimeSpan.FromMilliseconds(multichannelDownloaderOptions.ProgressIntervalMs);
		_E00B = logger;
	}

	public void OnAwake()
	{
		_gameUpdateServiceLazy.Value.OnInstallationCompleted += OnGameInstallationCompleted;
		_gameUpdateServiceLazy.Value.OnInstallationStarted += _E000;
		_gameUpdateServiceLazy.Value.OnUpdateCompleted += OnGameUpdateCompleted;
		_E008.Value.OnGameStateChanged += _E000;
	}

	public void OnStop()
	{
		_gameUpdateServiceLazy.Value.OnInstallationCompleted -= OnGameInstallationCompleted;
		_gameUpdateServiceLazy.Value.OnUpdateCompleted -= OnGameUpdateCompleted;
		_E008.Value.OnGameStateChanged -= _E000;
	}

	public async Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken)
	{
		using IMultichannelDownloader multichannelDownloader = _E007.Create(_settingsService.ChannelSettings);
		return await multichannelDownloader.GetFileSizeAsync(relativeUri, cancellationToken);
	}

	public async Task<string> DownloadTheGamePackageAsync(GamePackageInfo gamePackageInfo, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		string path = string.Format(_E05B._E000(3477), _appConfig.GameShortName, _settingsService.SelectedBranch.Name, gamePackageInfo.Version, _E05B._E000(3558));
		string text = Path.Combine(_settingsService.LauncherTempDir, path);
		using (IMultichannelDownloader downloader = _E007.Create(_settingsService.ChannelSettings))
		{
			await _E000(gamePackageInfo.DownloadUri, text, downloader, onProgress, tryUseMetadata: true, redownloadIfExist: false, cancellationToken);
		}
		return text;
	}

	public Task<string[]> DownloadTheGameUpdatesAsync(IReadOnlyCollection<GameUpdateInfo> updates, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		(string, string)[] downloadSpecification = updates.Select(delegate(GameUpdateInfo u)
		{
			string path = string.Format(_E05B._E000(3518), _appConfig.GameShortName, _settingsService.SelectedBranch.Name, u.FromVersion, u.Version, _E05B._E000(3475));
			string item = Path.Combine(_settingsService.LauncherTempDir, path);
			return (u.DownloadUri, item);
		}).ToArray();
		return DownloadFilesAsync(downloadSpecification, onProgress, tryUseMetadata: true, redownloadIfExist: false, cancellationToken);
	}

	public async Task<string[]> DownloadFilesAsync(IReadOnlyList<(string relativeUri, string destinationPath)> downloadSpecification, Action<long, long> onProgress, bool tryUseMetadata = false, bool redownloadIfExist = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (downloadSpecification == null)
		{
			throw new ArgumentNullException(_E05B._E000(3549));
		}
		IMultichannelDownloader multichannelDownloader = _E007.Create(_settingsService.ChannelSettings);
		try
		{
			_E033 obj = new _E033(onProgress, downloadSpecification.Count, _E00A);
			try
			{
				obj._E000();
				cancellationToken.ThrowIfCancellationRequested();
				Parallel.For(0, downloadSpecification.Count, new ParallelOptions
				{
					CancellationToken = cancellationToken
				}, async delegate(int i)
				{
					(string relativeUri, string destinationPath) tuple2 = downloadSpecification[i];
					try
					{
						long totalSize = await multichannelDownloader.GetFileSizeAsync(tuple2.relativeUri, cancellationToken);
						obj._E000(i, totalSize);
					}
					catch (ObjectDisposedException)
					{
					}
					catch (OperationCanceledException)
					{
					}
					catch (Exception exception)
					{
						_E00B.LogError(exception, _E05B._E000(3560), tuple2.relativeUri);
					}
				});
				for (int j = 0; j < downloadSpecification.Count; j++)
				{
					(string, string) tuple = downloadSpecification[j];
					Action<long, long> onProgress2 = obj._E000(j);
					await _E000(tuple.Item1, tuple.Item2, multichannelDownloader, onProgress2, tryUseMetadata, redownloadIfExist, cancellationToken);
				}
				obj._E000();
				return downloadSpecification.Select(((string relativeUri, string destinationPath) s) => s.destinationPath).ToArray();
			}
			finally
			{
				if (obj != null)
				{
					((IDisposable)obj).Dispose();
				}
			}
		}
		finally
		{
			if (multichannelDownloader != null)
			{
				multichannelDownloader.Dispose();
			}
		}
	}

	public async Task DownloadFileAsync(string relativeUri, string destinationPath, Action<long, long> onProgress, bool tryUseMetadata = false, bool redownloadIfExist = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(relativeUri))
		{
			throw DownloadManagementServiceException.WrongUri(relativeUri);
		}
		string text = destinationPath + _E05B._E000(3127);
		using (IMultichannelDownloader downloader = _E007.Create(_settingsService.ChannelSettings))
		{
			await _E000(relativeUri, text, downloader, onProgress, tryUseMetadata, redownloadIfExist, cancellationToken);
		}
		_E006.Move(text, destinationPath, overwrite: true);
	}

	private async Task _E000(string relativeUri, string destinationPath, IMultichannelDownloader downloader, Action<long, long> onProgress, bool tryUseMetadata = false, bool redownloadIfExist = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		string fileName = Path.GetFileName(destinationPath);
		if (string.IsNullOrWhiteSpace(fileName))
		{
			throw DownloadManagementServiceException.WrongDestinationPath(destinationPath);
		}
		Directory.CreateDirectory(_settingsService.LauncherTempDir);
		string text = Path.Combine(_settingsService.LauncherTempDir, fileName) + _E05B._E000(3129);
		destinationPath = destinationPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		_E00D.Wait(cancellationToken);
		try
		{
			_ = 1;
			try
			{
				while (!_E00C.TryAdd(destinationPath, null))
				{
					_E00B.LogWarning(_E05B._E000(3132), destinationPath);
					if (_E00C.TryGetValue(destinationPath, out var value))
					{
						await (value ?? Task.Delay(100, cancellationToken));
					}
				}
				if (!redownloadIfExist && File.Exists(destinationPath))
				{
					_E00B.LogInformation(_E05B._E000(3140), destinationPath);
					long length = new FileInfo(destinationPath).Length;
					onProgress?.Invoke(length, length);
					return;
				}
				using (FileStream destinationStream = _E006.CaptureFile(text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
				{
					bool retry = true;
					while (retry)
					{
						try
						{
							retry = false;
							Task task = downloader.DownloadAsync(relativeUri, destinationStream, onProgress, tryUseMetadata, cancellationToken);
							_E00C[destinationPath] = task;
							await task;
						}
						catch (MultichannelDownloaderException exception)
						{
							this._E005.OnDownloadError(exception, ref retry);
							_E00B.LogInformation(_E05B._E000(3254), retry ? _E05B._E000(3274) : _E05B._E000(3325), relativeUri);
							if (retry)
							{
								downloader.ResetErrors();
								continue;
							}
							throw;
						}
					}
				}
				_E006.Move(text, destinationPath, overwrite: true);
			}
			finally
			{
				_E00C.TryRemove(destinationPath, out var _);
			}
		}
		finally
		{
			_E00D.Release();
		}
	}

	private void _E000()
	{
		IEnumerable<string> paths = new IEnumerable<string>[4]
		{
			Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3412)),
			Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3421)),
			Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3494)),
			Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3503))
		}.SelectMany((IEnumerable<string> f) => f);
		_E006.Delete(paths);
	}

	private void OnGameInstallationCompleted(object sender, InstallationCompletedEventArgs e)
	{
		CleanupByInstallationResult(e.InstallationResult);
	}

	private void _E000(BsgVersion version)
	{
		Directory.CreateDirectory(_settingsService.LauncherTempDir);
		foreach (string item in Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3412)).Concat(Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(3505))))
		{
			BsgVersion bsgVersion = _E009.ExtractVersionsFromString(item).LastOrDefault();
			if (bsgVersion != default(BsgVersion) && bsgVersion < version)
			{
				_E006.Delete(item);
			}
		}
	}

	private void OnGameUpdateCompleted(object sender, UpdateCompletedEventArgs e)
	{
		CleanupByInstallationResult(e.InstallationResult);
	}

	private void CleanupByInstallationResult(InstallationResult installationResult)
	{
		if (installationResult == InstallationResult.Succeded || installationResult == InstallationResult.Stopped || installationResult == InstallationResult.ConsistencyError || installationResult == InstallationResult.HasSkippedFiles)
		{
			_E000();
		}
	}

	private void _E000(object sender, GameStateChangedEventArgs e)
	{
		if (e.NewState == GameState.ReadyToGame)
		{
			_E000();
		}
	}

	[CompilerGenerated]
	private (string DownloadUri, string destinationPath) _E000(GameUpdateInfo u)
	{
		string path = string.Format(_E05B._E000(3518), _appConfig.GameShortName, _settingsService.SelectedBranch.Name, u.FromVersion, u.Version, _E05B._E000(3475));
		string item = Path.Combine(_settingsService.LauncherTempDir, path);
		return (u.DownloadUri, item);
	}
}
internal class _E035 : IConsistencyControlService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public long _E000;

		public Action<long, long> _E001;

		public long _E002;

		public long _E003;

		internal void _E000(long bytesProcessed, long streamSize)
		{
			this._E000 = bytesProcessed;
			_E001(_E002 + this._E000, _E003);
		}
	}

	private const int m__E000 = 300;

	private readonly IFileManager _E001;

	private readonly Microsoft.Extensions.Logging.ILogger _E002;

	public _E035(IFileManager fileManager, ILogger<_E035> logger)
	{
		_E001 = fileManager;
		_E002 = logger;
	}

	public void EnsureConsistencyByHash(IReadOnlyCollection<(string filePath, string hash)> data, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		List<(Stream, string)> list = new List<(Stream, string)>(data.Count);
		try
		{
			foreach (var datum in data)
			{
				list.Add((_E001.CaptureFile(datum.filePath), datum.hash));
			}
			EnsureConsistencyByHash(list, onProgress, cancellationToken);
		}
		finally
		{
			foreach (var item in list)
			{
				item.Item1.Dispose();
			}
		}
	}

	public void EnsureConsistencyByHash(IReadOnlyCollection<(Stream stream, string hash)> data, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		long arg = data.Sum(((Stream stream, string hash) d) => d.stream.Length);
		long num = 0L;
		long num2 = 0L;
		foreach (var datum in data)
		{
			EnsureConsistencyByHash(datum.stream, datum.hash, (onProgress == null) ? onProgress : ((Action<long, long>)delegate(long bytesProcessed, long streamSize)
			{
				num2 = bytesProcessed;
				onProgress(num + num2, arg);
			}), cancellationToken);
			num += datum.stream.Length;
			num2 = 0L;
		}
	}

	public void EnsureConsistencyByHash(string filePath, string hash, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		using FileStream stream = _E001.CaptureFile(filePath);
		EnsureConsistencyByHash(stream, hash, onProgress, cancellationToken);
	}

	public void EnsureConsistencyByHash(Stream stream, string hash, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		if (!CheckHash(stream, hash, onProgress, cancellationToken))
		{
			string text = (stream as FileStream)?.Name ?? stream.GetType().Name;
			_E002.LogError(_E05B._E000(3283), text, stream.Length);
			throw ConsistencyControlServiceException.ChecksumDoesNotMatch(text);
		}
	}

	public bool CheckHash(Stream stream, string hash, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		return GetHash(stream, onProgress, cancellationToken).ToHex().ToLowerInvariant() == hash.ToLowerInvariant();
	}

	public byte[] GetHash(Stream stream, Action<long, long> onProgress, CancellationToken cancellationToken)
	{
		byte[] result = null;
		try
		{
			using System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
			DateTime dateTime = DateTime.MinValue;
			byte[] array = new byte[4096];
			long num = stream.Length - stream.Position;
			long num2 = 0L;
			int num3 = 0;
			do
			{
				cancellationToken.ThrowIfCancellationRequested();
				num3 = stream.Read(array, 0, array.Length);
				mD.TransformBlock(array, 0, num3, array, 0);
				num2 += num3;
				if (onProgress != null)
				{
					DateTime now = DateTime.Now;
					if ((now - dateTime).TotalMilliseconds > 300.0)
					{
						dateTime = now;
						onProgress(num2, stream.Length);
					}
				}
			}
			while (num2 < num);
			mD.TransformFinalBlock(array, 0, 0);
			result = mD.Hash;
		}
		catch (TargetInvocationException)
		{
			_E002.LogWarning(_E05B._E000(2824));
			Eft.Launcher.Security.Cryptography.MD5.MD5 mD2 = new Eft.Launcher.Security.Cryptography.MD5.MD5();
			mD2.SetValueFromStream(stream);
			result = mD2.HashAsByteArray;
		}
		onProgress?.Invoke(stream.Length, stream.Length);
		return result;
	}
}
[CompilerGenerated]
[_E036]
internal sealed class _E036 : Attribute
{
}
[CompilerGenerated]
[_E036]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
internal sealed class _E037 : Attribute
{
	public readonly byte[] _E000;

	public _E037(byte P_0)
	{
		_E000 = new byte[1] { P_0 };
	}

	public _E037(byte[] P_0)
	{
		_E000 = P_0;
	}
}
internal class _E038 : IServiceCollection, IList<ServiceDescriptor>, ICollection<ServiceDescriptor>, IEnumerable<ServiceDescriptor>, IEnumerable
{
	private readonly DryIoc.IContainer m__E000;

	private bool _E001;

	public ServiceDescriptor this[int index]
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public int Count
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public _E038(DryIoc.IContainer container)
	{
		this.m__E000 = container;
	}

	public IServiceProvider _E000()
	{
		if (_E001)
		{
			throw new Exception(_E05B._E000(2727));
		}
		_E001 = true;
		return this.m__E000;
	}

	public void Add(ServiceDescriptor descriptor)
	{
		this.m__E000._E000(descriptor);
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public bool Contains(ServiceDescriptor item)
	{
		throw new NotImplementedException();
	}

	public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public IEnumerator<ServiceDescriptor> GetEnumerator()
	{
		throw new NotImplementedException();
	}

	public int IndexOf(ServiceDescriptor item)
	{
		throw new NotImplementedException();
	}

	public void Insert(int index, ServiceDescriptor item)
	{
		throw new NotImplementedException();
	}

	public bool Remove(ServiceDescriptor item)
	{
		throw new NotImplementedException();
	}

	public void RemoveAt(int index)
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new NotImplementedException();
	}
}
[CompilerGenerated]
internal sealed class _E039
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 6)]
	private struct _E000
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
	private struct _E001
	{
	}

	internal static readonly _E001 _E000/* Not supported: data(69 1E 06 00 D1 00 00 00 E5 00 00 00 CD 00 00 00 F0 00 00 00 F3 00 00 00 F8 00 00 00 F9 00 00 00 CE 00 00 00) */;

	internal static readonly _E000 _E001/* Not supported: data(20 00 0D 00 0A 00) */;

	internal static readonly _E000 _E002/* Not supported: data(5C 00 2F 00 20 00) */;
}
internal class _E03A : IChannelMonitor, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E03A _E000;

		public int _E001;

		internal _E03D _E000(IChannel c)
		{
			return new _E03D(c, _E001, this._E000._E008);
		}
	}

	private static readonly TimeSpan m__E000 = TimeSpan.FromSeconds(1.0);

	[CompilerGenerated]
	private Action _E001;

	[CompilerGenerated]
	private Action _E002;

	[CompilerGenerated]
	private Action<IChannel> _E003;

	[CompilerGenerated]
	private readonly IReadOnlyCollection<IChannelSensor> _E004;

	[CompilerGenerated]
	private int _E005;

	private readonly MultichannelDownloaderOptions _E006;

	private readonly Microsoft.Extensions.Logging.ILogger _E007;

	private readonly Metronome _E008;

	private int _E009;

	public IReadOnlyCollection<IChannelSensor> Sensors
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
	}

	public int Speed
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public event Action OnSpareNodeActivationThresholdReached
	{
		[CompilerGenerated]
		add
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnSpareNodeActivationThresholdRestored
	{
		[CompilerGenerated]
		add
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<IChannel> OnConnectionBreakingThresholdReached
	{
		[CompilerGenerated]
		add
		{
			Action<IChannel> action = _E003;
			Action<IChannel> action2;
			do
			{
				action2 = action;
				Action<IChannel> value2 = (Action<IChannel>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IChannel> action = _E003;
			Action<IChannel> action2;
			do
			{
				action2 = action;
				Action<IChannel> value2 = (Action<IChannel>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E03A(MultichannelDownloaderOptions options, IReadOnlyCollection<IChannel> channels, MultichannelDownloaderContext context)
	{
		_E03A obj = this;
		_E006 = options;
		_E007 = context.ContextLogger;
		int timeCoverageSec = Math.Max(_E006.SpareNodeThresholdTimeoutSec, 5);
		_E008 = new Metronome(_E03A.m__E000, _E000, _E05B._E000(2297));
		_E004 = (IReadOnlyCollection<IChannelSensor>)(object)channels.Select((IChannel c) => new _E03D(c, timeCoverageSec, obj._E008)).ToArray();
		_E009 = options.SpareNodeThresholdTimeoutSec;
		_E008.Start();
	}

	private void _E000()
	{
		try
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			int num = 0;
			int num2 = 0;
			foreach (IChannelSensor sensor in Sensors)
			{
				if (!sensor.Channel.IsSpare && sensor.Channel.CanDownloadFile(null))
				{
					flag3 = false;
				}
				if (sensor.IsActive)
				{
					num += sensor.Speed;
					flag2 = true;
					if (!sensor.Channel.IsSpare)
					{
						num2 += sensor.Speed;
					}
				}
				else if (!sensor.Channel.IsSpare && sensor.Channel.CanDownloadFile(null))
				{
					flag = true;
				}
			}
			Speed = num;
			if (flag3)
			{
				if (_E009 >= 0)
				{
					_E009 = -1;
					_E007.LogInformation(_E05B._E000(2257));
					_E001?.Invoke();
				}
			}
			else if (!flag2)
			{
				_E007.LogTrace(_E05B._E000(5906), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num));
				_E009 = _E006.SpareNodeThresholdTimeoutSec;
			}
			else if (_E009 > 0)
			{
				if (num < _E006.SpareNodeActivationThreshold && !flag)
				{
					_E009--;
					_E007.LogTrace(_E05B._E000(6005), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num), _E009);
					if (_E009 == 0)
					{
						_E009 = -_E006.SpareNodeThresholdTimeoutSec;
						_E007.LogInformation(_E05B._E000(6076));
						_E001?.Invoke();
					}
				}
				else if (_E009 != _E006.SpareNodeThresholdTimeoutSec)
				{
					_E009 = _E006.SpareNodeThresholdTimeoutSec;
					_E007.LogTrace(_E05B._E000(6116), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num), _E009);
				}
			}
			else if (flag)
			{
				_E009 = _E006.SpareNodeThresholdTimeoutSec;
				_E007.LogTrace(_E05B._E000(5683), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num), _E009);
			}
			else if (num2 > _E006.SpareNodeActivationThreshold)
			{
				_E009++;
				_E007.LogTrace(_E05B._E000(5722), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num), _E009);
				if (_E009 == 0)
				{
					_E009 = _E006.SpareNodeThresholdTimeoutSec;
					_E007.LogInformation(_E05B._E000(5857));
					_E002?.Invoke();
				}
			}
			else if (_E009 != -_E006.SpareNodeThresholdTimeoutSec)
			{
				_E009 = -_E006.SpareNodeThresholdTimeoutSec;
				_E007.LogTrace(_E05B._E000(6116), Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num), _E009);
			}
		}
		catch (Exception exception)
		{
			_E007.LogError(exception, _E05B._E000(5832));
		}
	}

	public void Dispose()
	{
		_E008.Dispose();
	}
}
internal class _E03B : IChannelMonitorFactory
{
	private readonly MultichannelDownloaderOptions _E000;

	public _E03B(MultichannelDownloaderOptions options)
	{
		_E000 = options;
	}

	public IChannelMonitor Create(IReadOnlyCollection<IChannel> channels, MultichannelDownloaderContext context)
	{
		return new _E03A(_E000, channels, context);
	}
}
internal class _E03C : IChannelProviderFactory
{
	private readonly MultichannelDownloaderOptions _E000;

	public _E03C(MultichannelDownloaderOptions downloaderOptions)
	{
		_E000 = downloaderOptions;
	}

	public IChannelProvider Create(IChannelMonitor channelQualityMonitor, IReadOnlyCollection<IChannel> channels, MultichannelDownloaderContext context)
	{
		return new _E040(channelQualityMonitor, channels, _E000, context);
	}
}
internal class _E03D : IChannelSensor
{
	[CompilerGenerated]
	private readonly IChannel m__E000;

	[CompilerGenerated]
	private bool m__E001;

	[CompilerGenerated]
	private int m__E002;

	private readonly Metronome _E003;

	private readonly Queue<int> _E004;

	private readonly int _E005;

	private int _E006;

	private int _E007;

	public IChannel Channel
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	public bool IsActive
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public int Speed
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public _E03D(IChannel channel, int timeCoverageSec, Metronome metronome)
	{
		this.m__E000 = channel;
		_E003 = metronome;
		_E005 = (int)Math.Ceiling((double)timeCoverageSec / _E003.Interval.TotalSeconds);
		_E004 = new Queue<int>(_E005);
		channel.OnDataReadingStarted += _E000;
		channel.OnDataRead += _E000;
		_E003.Subscribe(_E001);
	}

	private void _E000()
	{
		IsActive = true;
	}

	private void _E000(int bytesFilled)
	{
		Interlocked.Add(ref _E007, bytesFilled);
	}

	private void _E001()
	{
		if (Channel.IsBusy || _E007 > 0)
		{
			IsActive = true;
			_E002();
		}
		else
		{
			IsActive = false;
			Speed = 0;
		}
	}

	private void _E002()
	{
		lock (_E004)
		{
			int num = Interlocked.Exchange(ref _E007, 0);
			_E006 += num;
			if (_E004.Count == _E005)
			{
				int num2 = _E004.Dequeue();
				_E006 -= num2;
			}
			_E004.Enqueue(num);
			Speed = (int)((double)(_E006 / _E004.Count) / _E003.Interval.TotalSeconds);
		}
	}
}
internal class _E03E
{
	[CompilerGenerated]
	private readonly int m__E000;

	[CompilerGenerated]
	private readonly long m__E001;

	[CompilerGenerated]
	private readonly int _E002;

	[CompilerGenerated]
	private bool _E003;

	public int _E000
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	public long _E000
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
	}

	public int _E001
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	public _E03E(int number, long offset, int size, bool isCompleted)
	{
		this.m__E000 = number;
		this.m__E001 = offset;
		_E002 = size;
		this._E000 = isCompleted;
	}

	public void _E000()
	{
		this._E000 = true;
	}
}
internal class _E03F : IChannel, IDisposable
{
	[CompilerGenerated]
	private Action<_E03F> _E002;

	[CompilerGenerated]
	private Action<_E03F, ChannelRevokationReason> _E003;

	[CompilerGenerated]
	private object _E004;

	[CompilerGenerated]
	private bool _E005;

	private readonly IChannel _E006;

	private int _E007;

	public bool IsSpare => _E006.IsSpare;

	public bool IsBusy => _E006.IsBusy;

	public Uri Endpoint => _E006.Endpoint;

	public object _E000
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		private set
		{
			_E004 = value;
		}
	}

	public bool _E000
	{
		get
		{
			if (_E007 != 0)
			{
				return this._E000 != null;
			}
			return false;
		}
	}

	public bool _E001
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public event Action OnDataReadingStarted
	{
		add
		{
			_E006.OnDataReadingStarted += value;
		}
		remove
		{
			_E006.OnDataReadingStarted -= value;
		}
	}

	public event Action<int> OnDataRead
	{
		add
		{
			_E006.OnDataRead += value;
		}
		remove
		{
			_E006.OnDataRead -= value;
		}
	}

	public event Action OnDataReadingCompleted
	{
		add
		{
			_E006.OnDataReadingCompleted += value;
		}
		remove
		{
			_E006.OnDataReadingCompleted -= value;
		}
	}

	public event Action<_E03F> _E000
	{
		[CompilerGenerated]
		add
		{
			Action<_E03F> action = _E002;
			Action<_E03F> action2;
			do
			{
				action2 = action;
				Action<_E03F> value2 = (Action<_E03F>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E03F> action = _E002;
			Action<_E03F> action2;
			do
			{
				action2 = action;
				Action<_E03F> value2 = (Action<_E03F>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E03F, ChannelRevokationReason> _E001
	{
		[CompilerGenerated]
		add
		{
			Action<_E03F, ChannelRevokationReason> action = _E003;
			Action<_E03F, ChannelRevokationReason> action2;
			do
			{
				action2 = action;
				Action<_E03F, ChannelRevokationReason> value2 = (Action<_E03F, ChannelRevokationReason>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E03F, ChannelRevokationReason> action = _E003;
			Action<_E03F, ChannelRevokationReason> action2;
			do
			{
				action2 = action;
				Action<_E03F, ChannelRevokationReason> value2 = (Action<_E03F, ChannelRevokationReason>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E03F(IChannel channel)
	{
		_E006 = channel;
	}

	public bool _E000(object tenant, bool isImportant = false)
	{
		if (Interlocked.Exchange(ref _E007, 1) == 0)
		{
			this._E001 = isImportant;
			this._E000 = tenant;
			_E002?.Invoke(this);
			return true;
		}
		return false;
	}

	public void _E000(ChannelRevokationReason reason)
	{
		if (Interlocked.Exchange(ref _E007, 0) != 0)
		{
			_E003?.Invoke(this, reason);
			this._E001 = false;
			this._E000 = null;
		}
	}

	public bool CanDownloadFile(string relativeUri)
	{
		return _E006.CanDownloadFile(relativeUri);
	}

	public Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken)
	{
		return _E006.GetFileSizeAsync(relativeUri, cancellationToken);
	}

	public void FillChunk(string relativeUri, IChunk chunk, CancellationToken cancellationToken)
	{
		_E006.FillChunk(relativeUri, chunk, cancellationToken);
	}

	public void DownloadFile(string relativeUri, Stream destinationStream, CancellationToken cancellationToken)
	{
		_E006.DownloadFile(relativeUri, destinationStream, cancellationToken);
	}

	public void ResetErrors()
	{
		_E006.ResetErrors();
	}

	public void AddError(Exception exception)
	{
		_E006.AddError(exception);
	}

	public void Dispose()
	{
		_E000(ChannelRevokationReason.ChannelDestruction);
		_E006.Dispose();
	}

	public override string ToString()
	{
		return _E006.ToString();
	}
}
internal class _E040 : IChannelProvider, IDisposable
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public string _E000;

		internal bool _E000(_E03F c)
		{
			if (!c._E000)
			{
				return c.CanDownloadFile(this._E000);
			}
			return false;
		}

		internal bool _E001(_E03F c)
		{
			return !c.CanDownloadFile(this._E000);
		}

		internal bool _E002(_E03F c)
		{
			if (!c._E000)
			{
				return c.CanDownloadFile(this._E000);
			}
			return false;
		}

		internal bool _E003(_E03F c)
		{
			return !c.CanDownloadFile(this._E000);
		}
	}

	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private readonly int m__E001;

	[CompilerGenerated]
	private int m__E002;

	private readonly IChannelMonitor m__E003;

	private readonly MultichannelDownloaderOptions _E004;

	private readonly Microsoft.Extensions.Logging.ILogger _E005;

	private readonly IReadOnlyList<_E03F> _E006;

	private readonly IReadOnlyList<_E03F> _E007;

	private readonly SemaphoreSlim _E008 = new SemaphoreSlim(0);

	private bool _E009;

	private bool _E00A;

	public int ChannelsCount
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
	}

	public int LeasedChannelsCount
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public event Action OnChannelForLeaseAppeared
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E040(IChannelMonitor channelQualityMonitor, IReadOnlyCollection<IChannel> channels, MultichannelDownloaderOptions downloaderOptions, MultichannelDownloaderContext context)
	{
		if (channels.Count == 0)
		{
			throw new ChannelProviderException(_E05B._E000(5378));
		}
		this.m__E001 = channels.Count;
		this.m__E003 = channelQualityMonitor;
		_E004 = downloaderOptions;
		_E005 = context.ContextLogger;
		_E006 = (from c in channels
			where c.IsSpare
			select new _E03F(c)).ToList();
		List<_E03F> list = (from c in channels
			where !c.IsSpare
			select new _E03F(c)).ToList();
		list._E000();
		_E007 = list;
		foreach (_E03F item in _E007)
		{
			item._E001 += _E000;
			item._E000 += _E000;
		}
		foreach (_E03F item2 in _E006)
		{
			item2._E001 += _E000;
			item2._E000 += _E000;
		}
		this.m__E003.OnSpareNodeActivationThresholdReached += _E000;
		this.m__E003.OnSpareNodeActivationThresholdRestored += _E001;
		this.m__E003.OnConnectionBreakingThresholdReached += _E000;
	}

	public ILeasedChannel WaitForLeaseChannel(CancellationToken cancellationToken, string relativeUri, object tenant, bool isImportant)
	{
		if (!TryLeaseChannel(out var leasedChannel, relativeUri, tenant, isImportant))
		{
			do
			{
				_E008.Wait(cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
				_E003();
			}
			while (!TryLeaseChannel(out leasedChannel, relativeUri, tenant, isImportant));
		}
		return leasedChannel;
	}

	public async Task<ILeasedChannel> WaitForLeaseChannelAsync(CancellationToken cancellationToken, string relativeUri, object tenant, bool isImportant)
	{
		if (!TryLeaseChannel(out var leasedChannel, relativeUri, tenant, isImportant))
		{
			do
			{
				await _E008.WaitAsync(cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
				_E003();
			}
			while (!TryLeaseChannel(out leasedChannel, relativeUri, tenant, isImportant));
		}
		return leasedChannel;
	}

	public bool TryLeaseChannel(out ILeasedChannel leasedChannel, string relativeUri, object tenant, bool isImportant)
	{
		lock (this)
		{
			if (!_E00A && LeasedChannelsCount < _E004.SimultaneouslyUsedChannelsLimit)
			{
				_E03F obj = _E007.FirstOrDefault((_E03F c) => !c._E000 && c.CanDownloadFile(relativeUri));
				if (obj == null && (isImportant || _E009 || _E007.All((_E03F c) => !c.CanDownloadFile(relativeUri))))
				{
					obj = _E006.FirstOrDefault((_E03F c) => !c._E000 && c.CanDownloadFile(relativeUri));
					if (obj == null && LeasedChannelsCount == 0 && _E007.Concat(_E006).All((_E03F c) => !c.CanDownloadFile(relativeUri)))
					{
						_E005.LogWarning(_E05B._E000(5492), relativeUri);
						_E008.Release();
						throw new ChannelProviderNoAvailableChannelsException(_E05B._E000(5551));
					}
				}
				if (obj != null)
				{
					using (_E005.BeginScope(new Dictionary<string, object> { 
					{
						_E05B._E000(5600),
						obj
					} }))
					{
						if (obj._E000(tenant, isImportant))
						{
							leasedChannel = new _E041(obj);
							_E005.LogTrace(_E05B._E000(5608), relativeUri);
							return true;
						}
						_E005.LogWarning(_E05B._E000(5589));
					}
				}
			}
			leasedChannel = null;
			return false;
		}
	}

	public void ResetErrors()
	{
		lock (this)
		{
			foreach (_E03F item in _E007.Concat(_E006))
			{
				item.ResetErrors();
			}
			_E005.LogInformation(_E05B._E000(5143));
		}
		this.m__E000?.Invoke();
	}

	private void _E000(_E03F channel)
	{
		LeasedChannelsCount++;
	}

	private void _E000(_E03F channel, ChannelRevokationReason reason)
	{
		reason = reason ?? ChannelRevokationReason.Unknown;
		using (_E005.BeginScope(new Dictionary<string, object> { 
		{
			_E05B._E000(5600),
			channel
		} }))
		{
			_E005.Log(reason.Severity, reason.Exception, _E05B._E000(5230) + reason.MessageTemplate, reason.MessageArgs);
		}
		int num;
		lock (this)
		{
			num = --LeasedChannelsCount;
			if (_E00A)
			{
				return;
			}
		}
		_E008.Release();
		if (num < _E004.SimultaneouslyUsedChannelsLimit)
		{
			this.m__E000?.Invoke();
		}
	}

	private void _E000()
	{
		lock (this)
		{
			_E009 = true;
			if (_E006.Any((_E03F c) => !c._E000 && c.CanDownloadFile(null)))
			{
				this.m__E000?.Invoke();
			}
		}
	}

	private void _E001()
	{
		lock (this)
		{
			_E009 = false;
			foreach (_E03F item in _E006.Where((_E03F c) => c._E000 && !c._E001))
			{
				item._E000(ChannelRevokationReason.SpareNodeActivationThresholdRestored);
			}
		}
	}

	private void _E000(IChannel channel)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		lock (this)
		{
			if (_E00A)
			{
				return;
			}
			_E00A = true;
			this.m__E003.OnSpareNodeActivationThresholdReached -= _E000;
			this.m__E003.OnSpareNodeActivationThresholdRestored -= _E001;
			this.m__E003.OnConnectionBreakingThresholdReached -= _E000;
			foreach (_E03F item in _E006)
			{
				item.Dispose();
			}
			foreach (_E03F item2 in _E007)
			{
				item2.Dispose();
			}
			_E002();
			foreach (_E03F item3 in _E007)
			{
				item3._E001 -= _E000;
				item3._E000 -= _E000;
			}
			foreach (_E03F item4 in _E006)
			{
				item4._E001 -= _E000;
				item4._E000 -= _E000;
			}
			_E008.Dispose();
		}
	}

	private void _E002()
	{
		while (_E008.CurrentCount == 0)
		{
			_E008.Release();
		}
		_E008.Release(10);
	}

	private void _E003()
	{
		if (_E00A)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}
}
internal class _E041 : ILeasedChannel, IChannel, IDisposable
{
	[CompilerGenerated]
	private readonly bool m__E000;

	[CompilerGenerated]
	private readonly Uri _E001;

	private _E03F _E002;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public _E03F _E000 => _E002 ?? throw new ChannelLeaseRevokedException(this, string.Format(_E05B._E000(5196), this));

	public bool IsSpare
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public bool IsBusy => this._E000.IsBusy;

	public Uri Endpoint
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public event Action OnDataReadingStarted
	{
		add
		{
			this._E000.OnDataReadingStarted += value;
		}
		remove
		{
			this._E000.OnDataReadingStarted -= value;
		}
	}

	public event Action<int> OnDataRead
	{
		add
		{
			this._E000.OnDataRead += value;
		}
		remove
		{
			this._E000.OnDataRead -= value;
		}
	}

	public event Action OnDataReadingCompleted
	{
		add
		{
			this._E000.OnDataReadingCompleted += value;
		}
		remove
		{
			this._E000.OnDataReadingCompleted -= value;
		}
	}

	public _E041(_E03F leasableChannel)
	{
		if (leasableChannel == null)
		{
			throw new ArgumentNullException(_E05B._E000(5273));
		}
		_E002 = leasableChannel;
		_E001 = leasableChannel.Endpoint;
		this.m__E000 = leasableChannel.IsSpare;
		leasableChannel._E001 += _E000;
	}

	public bool CanDownloadFile(string relativeUri)
	{
		return this._E000.CanDownloadFile(relativeUri);
	}

	public Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken)
	{
		return this._E000.GetFileSizeAsync(relativeUri, cancellationToken);
	}

	public void FillChunk(string relativeUri, IChunk chunk, CancellationToken cancellationToken)
	{
		this._E000.FillChunk(relativeUri, chunk, cancellationToken);
	}

	public void DownloadFile(string relativeUri, Stream destinationStream, CancellationToken cancellationToken)
	{
		this._E000.DownloadFile(relativeUri, destinationStream, cancellationToken);
	}

	public void ResetErrors()
	{
		this._E000.ResetErrors();
	}

	public void AddError(Exception exception)
	{
		this._E000.AddError(exception);
	}

	public void Dispose(ChannelRevokationReason reason)
	{
		_E03F obj = Interlocked.Exchange(ref _E002, null);
		if (obj != null)
		{
			obj._E001 -= _E000;
			obj._E000(reason);
		}
	}

	public void Dispose()
	{
		Dispose(ChannelRevokationReason.Unknown);
	}

	public override string ToString()
	{
		return (IsSpare ? _E05B._E000(5353) : "") + Endpoint.ToString();
	}

	private void _E000(_E03F leasableChannel, ChannelRevokationReason reason)
	{
		Dispose(reason);
	}
}
internal static class _E042
{
	private static Random m__E000 = new Random();

	public static void _E000<_E001>(this IList<_E001> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = _E042.m__E000.Next(num + 1);
			_E001 value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static int _E000(this byte[] array, int offset, int length)
	{
		int num = offset + length;
		if (num > array.Length)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(5361), string.Format(_E05B._E000(5370), _E05B._E000(767), _E05B._E000(5361), num, _E05B._E000(4910), array.Length));
		}
		int num2 = 0;
		for (int i = offset; i < num; i++)
		{
			num2 += array[i];
		}
		return num2;
	}
}
internal class _E043 : IChunk, IDisposable
{
	private static readonly ArrayPool<byte> m__E000 = ArrayPool<byte>.Shared;

	private static readonly int[] m__E001 = new int[5] { 50, 100, 200, 500, 700 };

	[CompilerGenerated]
	private readonly int _E002;

	[CompilerGenerated]
	private readonly long _E003;

	[CompilerGenerated]
	private readonly int _E004;

	[CompilerGenerated]
	private int _E005;

	private readonly int? _E006;

	private byte[] _E007;

	private bool _E008;

	public int Number
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public long Offset
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	public int Size
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
	}

	public int BytesFilled
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public _E043(int number, long offset, int size, int? checksum)
	{
		_E002 = number;
		_E003 = offset;
		_E004 = size;
		_E006 = checksum;
	}

	public void FillFrom(Stream stream)
	{
		if (_E008)
		{
			throw new ObjectDisposedException(ToString());
		}
		_E007 = _E043.m__E000.Rent(Size);
		try
		{
			int num = 0;
			int num2 = 0;
			while (true)
			{
				num = stream.Read(_E007, BytesFilled, Size - BytesFilled);
				if (num == 0)
				{
					if (num2 == _E043.m__E001.Length)
					{
						throw new ChunkException(string.Format(_E05B._E000(4912), Number));
					}
					int millisecondsTimeout = _E043.m__E001[num2];
					num2++;
					Thread.Sleep(millisecondsTimeout);
				}
				else
				{
					num2 = 0;
				}
				if (num != 0)
				{
					BytesFilled += num;
					if (BytesFilled == Size)
					{
						break;
					}
				}
			}
		}
		catch
		{
			_E001();
			throw;
		}
	}

	public void PourOut(Stream stream)
	{
		try
		{
			if (_E007 == null)
			{
				throw new ChunkException(string.Format(_E05B._E000(4886), Number));
			}
			stream.Position = Offset;
			stream.Write(_E007, 0, Size);
		}
		catch
		{
			_E001();
			throw;
		}
	}

	public void EnsureChecksum()
	{
		if (_E006.HasValue)
		{
			int num = 0;
			for (int i = 0; i < Size; i++)
			{
				num += _E007[i];
			}
			if (num != _E006)
			{
				_E001();
				throw ChunkChecksumException.TheChecksumDoesNotMatch();
			}
		}
	}

	public void Dispose()
	{
		_E008 = true;
		_E000();
	}

	public override string ToString()
	{
		return string.Format(_E05B._E000(4987), Number);
	}

	private void _E000()
	{
		if (_E007 != null)
		{
			try
			{
				_E043.m__E000.Return(_E007);
			}
			catch (ThreadAbortException)
			{
			}
			_E007 = null;
		}
	}

	private void _E001()
	{
		BytesFilled = 0;
		_E000();
	}
}
internal class _E044 : IChannel, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HttpWebRequest request;

		internal void _E000()
		{
			request.Abort();
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public HttpWebRequest request;

		internal void _E000()
		{
			request.Abort();
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public HttpWebRequest request;

		internal void _E000()
		{
			request.Abort();
		}
	}

	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private Action<int> _E001;

	[CompilerGenerated]
	private Action m__E002;

	[CompilerGenerated]
	private readonly bool m__E003;

	[CompilerGenerated]
	private readonly Uri _E004;

	private readonly Microsoft.Extensions.Logging.ILogger _E005;

	private readonly HashSet<string> _E006 = new HashSet<string>();

	private bool _E007;

	private int _E008;

	public bool IsSpare
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
	}

	public bool IsBusy => _E008 != 0;

	public Uri Endpoint
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
	}

	public event Action OnDataReadingStarted
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<int> OnDataRead
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = this._E001;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> action = this._E001;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDataReadingCompleted
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E044(ChannelSettings channelSettings, MultichannelDownloaderContext context)
	{
		this.m__E003 = channelSettings.IsSpare;
		_E004 = channelSettings.Endpoint;
		_E005 = context.ContextLogger;
	}

	public bool CanDownloadFile(string relativeUri)
	{
		if (!_E007)
		{
			return !_E006.Contains(relativeUri);
		}
		return false;
	}

	public async Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		HttpWebRequest request = WebRequest.CreateHttp(_E000(relativeUri));
		request.Method = _E05B._E000(17457);
		using (cancellationToken.Register(delegate
		{
			request.Abort();
		}))
		{
			try
			{
				using WebResponse webResponse = await request.GetResponseAsync();
				if (!long.TryParse(webResponse.Headers.Get(_E05B._E000(17460)) ?? throw new ChannelException(this, _E05B._E000(4436)), out var result))
				{
					throw new ChannelException(this, _E05B._E000(4511));
				}
				return result;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (WebException ex2) when (ex2.Status == WebExceptionStatus.RequestCanceled)
			{
				_E005.LogDebug(_E05B._E000(4137));
				cancellationToken.ThrowIfCancellationRequested();
				throw;
			}
			catch (WebException ex3) when (((Func<bool>)delegate
			{
				// Could not convert BlockContainer to single expression
				HttpWebResponse obj = ex3.Response as HttpWebResponse;
				return obj != null && obj.StatusCode == HttpStatusCode.NotFound;
			}).Invoke())
			{
				_E006.Add(relativeUri);
				throw new ChannelFileNotFoundException(this, relativeUri);
			}
			catch (Exception ex4)
			{
				AddError(ex4);
				_E005.LogWarning(ex4, _E05B._E000(4113), this, relativeUri);
				if (ex4 is ChannelException)
				{
					throw;
				}
				throw new ChannelException(this, _E05B._E000(4161), ex4);
			}
		}
	}

	public void FillChunk(string relativeUri, IChunk chunk, CancellationToken cancellationToken)
	{
		if (Interlocked.CompareExchange(ref _E008, 1, 0) != 0)
		{
			throw new ChannelException(this, _E05B._E000(4989));
		}
		try
		{
			cancellationToken.ThrowIfCancellationRequested();
			this.m__E000?.Invoke();
			HttpWebRequest request = WebRequest.CreateHttp(_E000(relativeUri));
			request.Method = _E05B._E000(5010);
			using (cancellationToken.Register(delegate
			{
				request.Abort();
			}))
			{
				try
				{
					request.AddRange(chunk.Offset, chunk.Offset + chunk.Size);
					using WebResponse webResponse = request.GetResponse();
					using Stream originalStream = webResponse.GetResponseStream();
					using ProgressStream stream = new ProgressStream(originalStream, this._E001);
					chunk.FillFrom(stream);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (SocketException ex2) when (ex2.SocketErrorCode == SocketError.Interrupted)
				{
					_E005.LogDebug(_E05B._E000(5014));
					cancellationToken.ThrowIfCancellationRequested();
					throw new ChannelException(this, _E05B._E000(5119), ex2);
				}
				catch (WebException ex3) when (ex3.Status == WebExceptionStatus.RequestCanceled)
				{
					_E005.LogTrace(_E05B._E000(4669));
					cancellationToken.ThrowIfCancellationRequested();
					throw new ChannelException(this, _E05B._E000(4705), ex3);
				}
				catch (WebException ex4) when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					HttpWebResponse obj = ex4.Response as HttpWebResponse;
					return obj != null && obj.StatusCode == HttpStatusCode.NotFound;
				}).Invoke())
				{
					_E006.Add(relativeUri);
					throw new ChannelFileNotFoundException(this, relativeUri);
				}
				catch (Exception ex5)
				{
					AddError(ex5);
					throw new ChannelException(this, _E05B._E000(4701), ex5);
				}
			}
		}
		finally
		{
			_E008 = 0;
			this.m__E002?.Invoke();
		}
	}

	public void DownloadFile(string relativeUri, Stream destinationStream, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(relativeUri))
		{
			throw new ArgumentException(_E05B._E000(4739), _E05B._E000(4847));
		}
		if (destinationStream == null)
		{
			throw new ArgumentNullException(_E05B._E000(4859));
		}
		if (Interlocked.CompareExchange(ref _E008, 1, 0) != 0)
		{
			throw new ChannelException(this, _E05B._E000(4989));
		}
		try
		{
			cancellationToken.ThrowIfCancellationRequested();
			this.m__E000?.Invoke();
			HttpWebRequest request = WebRequest.CreateHttp(_E000(relativeUri));
			request.Method = _E05B._E000(5010);
			using (cancellationToken.Register(delegate
			{
				request.Abort();
			}))
			{
				try
				{
					using WebResponse webResponse = request.GetResponse();
					using Stream originalStream = webResponse.GetResponseStream();
					using ProgressStream progressStream = new ProgressStream(originalStream, this._E001);
					progressStream.CopyTo(destinationStream);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (SocketException ex2) when (ex2.SocketErrorCode == SocketError.Interrupted)
				{
					_E005.LogDebug(_E05B._E000(4809));
					cancellationToken.ThrowIfCancellationRequested();
					throw new ChannelException(this, _E05B._E000(5119), ex2);
				}
				catch (WebException ex3) when (ex3.Status == WebExceptionStatus.RequestCanceled)
				{
					_E005.LogDebug(_E05B._E000(4402));
					cancellationToken.ThrowIfCancellationRequested();
					throw new ChannelException(this, _E05B._E000(4705), ex3);
				}
				catch (WebException ex4) when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					HttpWebResponse obj = ex4.Response as HttpWebResponse;
					return obj != null && obj.StatusCode == HttpStatusCode.NotFound;
				}).Invoke())
				{
					_E006.Add(relativeUri);
					throw new ChannelFileNotFoundException(this, relativeUri);
				}
				catch (Exception ex5)
				{
					AddError(ex5);
					_E005.LogInformation(_E05B._E000(4374), this, relativeUri);
					throw new ChannelException(this, _E05B._E000(4478), ex5);
				}
			}
		}
		finally
		{
			_E008 = 0;
			this.m__E002?.Invoke();
		}
	}

	public void ResetErrors()
	{
		_E007 = false;
		_E006.Clear();
	}

	public void AddError(Exception _)
	{
		_E007 = true;
	}

	public void Dispose()
	{
	}

	public override string ToString()
	{
		return (IsSpare ? _E05B._E000(5353) : "") + Endpoint.Host;
	}

	private Uri _E000(string relativeUri)
	{
		return new Uri(Endpoint, relativeUri.TrimStart(' ', '/'));
	}
}
internal class _E045 : IChannelFactory
{
	private static readonly string[] _E000 = new string[2]
	{
		_E05B._E000(4185),
		_E05B._E000(4188)
	};

	public bool CanCreateChannelFor(ChannelSettings channelSettings)
	{
		return Array.IndexOf(_E000, channelSettings.Endpoint.Scheme) != -1;
	}

	public IChannel CreateChannel(ChannelSettings channelSettings, MultichannelDownloaderContext context)
	{
		return new _E044(channelSettings, context);
	}
}
internal class _E046 : IMultichannelDownloader, IDisposable
{
	[CompilerGenerated]
	private Action<IMultichannelDownloader> _E000;

	[CompilerGenerated]
	private readonly IChannelMonitor _E001;

	private readonly MultichannelDownloaderContext _E002;

	private readonly IChannelProvider _E003;

	private readonly _E049 _E004;

	private readonly MultichannelDownloaderOptions _E005;

	private readonly Microsoft.Extensions.Logging.ILogger _E006;

	private readonly SemaphoreSlim _E007 = new SemaphoreSlim(1);

	private readonly CancellationTokenSource _E008 = new CancellationTokenSource();

	private bool _E009;

	public int Id => _E002.DownloaderId;

	public IChannelMonitor Monitor
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public event Action<IMultichannelDownloader> OnDisposing
	{
		[CompilerGenerated]
		add
		{
			Action<IMultichannelDownloader> action = this._E000;
			Action<IMultichannelDownloader> action2;
			do
			{
				action2 = action;
				Action<IMultichannelDownloader> value2 = (Action<IMultichannelDownloader>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this._E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IMultichannelDownloader> action = this._E000;
			Action<IMultichannelDownloader> action2;
			do
			{
				action2 = action;
				Action<IMultichannelDownloader> value2 = (Action<IMultichannelDownloader>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this._E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E046(MultichannelDownloaderContext context, IChannelProvider channelProvider, IChannelMonitor channelQualityMonitor, _E049 helper, MultichannelDownloaderOptions downloaderOptions)
	{
		_E002 = context;
		_E003 = channelProvider;
		_E001 = channelQualityMonitor;
		_E004 = helper;
		_E005 = downloaderOptions;
		_E006 = context.ContextLogger;
	}

	public Task<long> GetFileSizeAsync(string relativeUri, CancellationToken cancellationToken)
	{
		if (_E009)
		{
			throw new ObjectDisposedException(_E05B._E000(4262));
		}
		using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_E008.Token, cancellationToken);
		return _E004._E000(relativeUri, cancellationTokenSource.Token);
	}

	public async Task DownloadAsync(string relativeUri, Stream destinationStream, Action<long, long> onProgress, bool tryUseMetadata, CancellationToken cancellationToken)
	{
		if (_E009)
		{
			throw new ObjectDisposedException(_E05B._E000(4262));
		}
		string text = ((destinationStream.Length > 0) ? _E05B._E000(4235) : _E05B._E000(4227));
		_E006.LogInformation(_E05B._E000(4243), relativeUri, text, Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(_E005.ChunkSize));
		if (destinationStream == null)
		{
			throw new ArgumentNullException(_E05B._E000(4859));
		}
		if (!destinationStream.CanWrite)
		{
			throw new MultichannelDownloaderException(_E05B._E000(7969));
		}
		if (_E007.CurrentCount == 0)
		{
			_E006.LogDebug(_E05B._E000(7945), relativeUri);
		}
		using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_E008.Token, cancellationToken))
		{
			_E007.Wait(cancellationTokenSource.Token);
			cancellationTokenSource.Token.ThrowIfCancellationRequested();
			_E04B obj = null;
			try
			{
				try
				{
					obj = new _E04B(relativeUri, destinationStream, onProgress, _E004, _E005.ChunkSize, tryUseMetadata, _E003, _E006, TimeSpan.FromMilliseconds(_E005.ProgressIntervalMs));
					await obj._E000();
					cancellationTokenSource.Token.Register(obj.Dispose);
					cancellationTokenSource.Token.ThrowIfCancellationRequested();
					obj._E000(cancellationTokenSource.Token);
				}
				catch (ThreadAbortException)
				{
					_E006.LogWarning(_E05B._E000(8028), relativeUri);
				}
				catch (OperationCanceledException)
				{
					_E006.LogDebug(_E05B._E000(8087), relativeUri);
					throw;
				}
				catch (IOException ex3) when (ex3.HResult == -2147024784)
				{
					string text2 = ((destinationStream is FileStream fileStream) ? Path.GetPathRoot(fileStream.Name) : _E05B._E000(15308));
					_E006.LogWarning(_E05B._E000(8133), text2);
					throw MultichannelDownloaderException.ThereIsNotEnoughSpaceOnDisk(text2);
				}
				catch (Exception ex4)
				{
					_E006.LogError(ex4, _E05B._E000(7732), relativeUri);
					if (ex4 is MultichannelDownloadingException)
					{
						throw;
					}
					throw MultichannelDownloaderException.FailedToDownloadTheFile(relativeUri);
				}
				finally
				{
					_E007.Release();
				}
				try
				{
					obj._E001(cancellationTokenSource.Token);
				}
				catch (OperationCanceledException)
				{
					_E006.LogDebug(_E05B._E000(8087), relativeUri);
					throw;
				}
				catch (ChannelProviderNoAvailableChannelsException)
				{
					_E006.LogWarning(_E05B._E000(7788), relativeUri);
					throw MultichannelDownloaderException.FailedToDownloadTheFile(relativeUri);
				}
				catch (Exception ex7)
				{
					_E006.LogError(ex7, _E05B._E000(7809), relativeUri);
					if (ex7 is MultichannelDownloadingException)
					{
						throw;
					}
					throw MultichannelDownloaderException.FailedToDownloadTheFile(relativeUri);
				}
			}
			finally
			{
				obj?.Dispose();
			}
		}
		_E006.LogInformation(_E05B._E000(7930), relativeUri, Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(destinationStream.Position));
	}

	public void ResetErrors()
	{
		_E003.ResetErrors();
	}

	public override string ToString()
	{
		return _E05B._E000(4287) + Id;
	}

	public void Dispose()
	{
		this._E000?.Invoke(this);
		_E009 = true;
		_E008.Cancel();
		_E003.Dispose();
		Monitor.Dispose();
	}
}
internal class _E047 : IMultichannelDownloaderFactory
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ChannelSettings _E000;

		internal bool _E000(IChannelFactory f)
		{
			return f.CanCreateChannelFor(this._E000);
		}
	}

	[CompilerGenerated]
	private Action<IMultichannelDownloader> m__E000;

	private readonly IEnumerable<IChannelFactory> _E001;

	private readonly IChannelMonitorFactory _E002;

	private readonly IChannelProviderFactory _E003;

	private readonly MultichannelDownloaderOptions _E004;

	private readonly ILoggerFactory _E005;

	private readonly Microsoft.Extensions.Logging.ILogger _E006;

	private int _E007;

	public event Action<IMultichannelDownloader> OnMultichannelDownloaderCreated
	{
		[CompilerGenerated]
		add
		{
			Action<IMultichannelDownloader> action = this.m__E000;
			Action<IMultichannelDownloader> action2;
			do
			{
				action2 = action;
				Action<IMultichannelDownloader> value2 = (Action<IMultichannelDownloader>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IMultichannelDownloader> action = this.m__E000;
			Action<IMultichannelDownloader> action2;
			do
			{
				action2 = action;
				Action<IMultichannelDownloader> value2 = (Action<IMultichannelDownloader>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public _E047(IEnumerable<IChannelFactory> factories, IChannelMonitorFactory channelQualityMonitorFactory, IChannelProviderFactory channelProviderFactory, MultichannelDownloaderOptions downloaderOptions, ILoggerFactory loggerFactory)
	{
		_E001 = factories;
		_E002 = channelQualityMonitorFactory;
		_E003 = channelProviderFactory;
		_E004 = downloaderOptions;
		_E005 = loggerFactory;
		_E006 = loggerFactory.CreateLogger<_E047>();
	}

	public IMultichannelDownloader Create(IReadOnlyCollection<ChannelSettings> сhannelSettings)
	{
		if (сhannelSettings == null)
		{
			throw new ArgumentNullException(_E05B._E000(7484));
		}
		if (сhannelSettings.Count == 0)
		{
			throw new MultichannelDownloaderFactoryException(_E05B._E000(7443));
		}
		MultichannelDownloaderContext multichannelDownloaderContext = new MultichannelDownloaderContext(Interlocked.Increment(ref _E007));
		_E04A logger2 = (_E04A)(multichannelDownloaderContext.ContextLogger = new _E04A(multichannelDownloaderContext, _E005.CreateLogger<_E046>()));
		List<IChannel> list = new List<IChannel>(сhannelSettings.Count);
		foreach (ChannelSettings current in сhannelSettings)
		{
			IChannel item = (_E001.FirstOrDefault((IChannelFactory f) => f.CanCreateChannelFor(current)) ?? throw new MultichannelDownloaderFactoryException(string.Format(_E05B._E000(7496), current.Endpoint))).CreateChannel(current, multichannelDownloaderContext);
			list.Add(item);
		}
		IChannelMonitor channelQualityMonitor = _E002.Create(list, multichannelDownloaderContext);
		IChannelProvider channelProvider = _E003.Create(channelQualityMonitor, list, multichannelDownloaderContext);
		_E049 helper = new _E049(multichannelDownloaderContext, channelProvider);
		_E046 obj = new _E046(multichannelDownloaderContext, channelProvider, channelQualityMonitor, helper, _E004);
		logger2.LogDebug(_E05B._E000(7570), list.Count, list.Count((IChannel c) => c.IsSpare));
		this.m__E000?.Invoke(obj);
		return obj;
	}
}
internal class _E048
{
	public const byte _E000 = 32;

	private const byte m__E001 = 1;

	private static readonly ArrayPool<byte> _E002 = ArrayPool<byte>.Shared;

	private static readonly Encoding _E003 = Encoding.UTF8;

	private static readonly char[] _E004 = _E05B._E000(7013).ToCharArray();

	private static readonly ushort _E005 = 41;

	[CompilerGenerated]
	private int _E006;

	[CompilerGenerated]
	private readonly int _E007;

	private readonly long _E008;

	private readonly byte _E009;

	private readonly int _E00A;

	private readonly _E03E[] _E00B;

	private int _E00C;

	public IReadOnlyCollection<_E03E> _E000 => (IReadOnlyCollection<_E03E>)(object)_E00B;

	public int _E000
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		private set
		{
			_E006 = value;
		}
	}

	public int _E001
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
	}

	private _E048(long fileSize, int chunkSize)
	{
		_E009 = 1;
		_E008 = fileSize;
		_E00A = chunkSize;
		long result;
		int num = (int)Math.DivRem(_E008, chunkSize, out result);
		int num2 = (int)result;
		int num3 = num + ((num2 > 0) ? 1 : 0);
		_E007 = _E005 + _E000(num3);
		_E00B = new _E03E[num3];
		for (int i = 0; i < num; i++)
		{
			_E00B[i] = new _E03E(i, (long)i * (long)chunkSize, chunkSize, isCompleted: false);
		}
		if (num2 > 0)
		{
			_E00B[num] = new _E03E(num, (long)num * (long)chunkSize, num2, isCompleted: false);
		}
		this._E000 = num3;
	}

	private _E048(Stream destinationStream, long fileSize)
	{
		if (destinationStream.Length < fileSize + 32)
		{
			throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7229));
		}
		_E008 = fileSize;
		destinationStream.Position = fileSize;
		long length = destinationStream.Length;
		byte[] array = _E002.Rent((int)(length - fileSize));
		try
		{
			if (destinationStream.Read(array, 0, 32) != 32)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7273));
			}
			char[] array2 = _E003.GetChars(array, 0, 32);
			Array.Resize(ref array2, _E004.Length);
			if (!array2.SequenceEqual(_E004))
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7236));
			}
			_E009 = (byte)destinationStream.ReadByte();
			if (_E009 != 1)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7252));
			}
			int num = 33;
			if (destinationStream.Read(array, num, 4) != 4)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7344));
			}
			num += 4;
			if (destinationStream.Read(array, num, 4) != 4)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7308));
			}
			_E00A = BitConverter.ToInt32(array, num);
			if (_E00A <= 0)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7406));
			}
			long result;
			int num2 = (int)Math.DivRem(_E008, _E00A, out result);
			int num3 = (int)result;
			int num4 = num2 + ((num3 > 0) ? 1 : 0);
			int num5 = _E000(num4);
			_E007 = _E005 + num5;
			if (destinationStream.Length < fileSize + this._E001)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7369));
			}
			if (destinationStream.Read(array, _E005, num5) != num5)
			{
				throw new MultichannelDownloaderFilePostfixException(_E05B._E000(7388));
			}
			_E00B = new _E03E[num4];
			for (int i = 0; i < num4; i++)
			{
				int size = ((i < num2) ? _E00A : num3);
				bool flag = _E000(array, _E005, i);
				_E00B[i] = new _E03E(i, (long)i * (long)_E00A, size, flag);
				if (!flag)
				{
					this._E000++;
				}
			}
		}
		finally
		{
			_E002.Return(array);
		}
	}

	public static _E048 _E000(Stream destinationStream, long fileSize, int chunkSize)
	{
		_E048 obj = new _E048(fileSize, chunkSize);
		obj._E001(destinationStream);
		return obj;
	}

	public static _E048 _E000(Stream destinationStream, long fileSize)
	{
		return new _E048(destinationStream, fileSize);
	}

	public void _E000(Stream destinationStream)
	{
		destinationStream.Position = _E008;
		destinationStream.SetLength(_E008);
		destinationStream.Flush();
	}

	public void _E000(IChunk chunk, Stream destinationStream)
	{
		_E00B[chunk.Number]._E000();
		this._E000--;
		int num = _E005 + chunk.Number / 8;
		int num2 = chunk.Number % 8;
		byte b = 0;
		for (int i = 0; i < 8; i++)
		{
			int num3 = chunk.Number + i - num2;
			if (num3 < _E00B.Length && _E00B[num3]._E000)
			{
				b = (byte)(b | (byte)(1 << i));
			}
		}
		destinationStream.Position = _E008 + num;
		destinationStream.WriteByte(b);
		destinationStream.Flush();
	}

	private void _E001(Stream destinationStream)
	{
		byte[] array = _E002.Rent(this._E001);
		try
		{
			Array.Clear(array, 0, this._E001);
			if (_E003.GetBytes(_E004, 0, _E004.Length, array, 0) > 32)
			{
				throw new MultichannelDownloaderException(string.Format(_E05B._E000(6970), (byte)32));
			}
			byte b = 32;
			array[b++] = 1;
			b = (byte)(b + 4);
			array[b++] = (byte)_E00A;
			array[b++] = (byte)(_E00A >> 8);
			array[b++] = (byte)(_E00A >> 16);
			array[b++] = (byte)(_E00A >> 24);
			destinationStream.SetLength(_E008 + this._E001);
			destinationStream.Position = _E008;
			destinationStream.Write(array, 0, this._E001);
			destinationStream.Flush();
		}
		finally
		{
			_E002.Return(array);
		}
	}

	private static int _E000(int chunksCount)
	{
		return chunksCount / 8 + ((chunksCount % 8 > 0) ? 1 : 0);
	}

	private static bool _E000(byte[] array, int offset, int flagNumber)
	{
		int num = offset + flagNumber / 8;
		int num2 = flagNumber % 8;
		return (array[num] & (1 << num2)) != 0;
	}
}
internal class _E049
{
	private readonly IChannelProvider m__E000;

	private readonly Microsoft.Extensions.Logging.ILogger _E001;

	public _E049(MultichannelDownloaderContext context, IChannelProvider channelProvider)
	{
		this.m__E000 = channelProvider;
		_E001 = context.ContextLogger;
	}

	public async Task<long> _E000(string relativeUri, CancellationToken cancellationToken)
	{
		_E001.LogTrace(_E05B._E000(6428), relativeUri);
		long num;
		while (true)
		{
			ILeasedChannel leasedChannel = await this.m__E000.WaitForLeaseChannelAsync(cancellationToken, relativeUri, this, isImportant: true);
			try
			{
				num = await leasedChannel.GetFileSizeAsync(relativeUri, cancellationToken);
				leasedChannel.Dispose(ChannelRevokationReason.FileSizeSuccessfullyReceived);
			}
			catch (ChannelException exception)
			{
				leasedChannel.Dispose(_E000(exception));
				continue;
			}
			catch (Exception exception2)
			{
				leasedChannel.Dispose(_E000(exception2));
				throw;
			}
			break;
		}
		_E001.LogDebug(_E05B._E000(6464), relativeUri, Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num));
		return num;
	}

	public bool _E000(string relativeUri, int chunkSize, CancellationToken cancellationToken, out MultichannelFileMetadata metadata)
	{
		_E001.LogTrace(_E05B._E000(6976), relativeUri);
		using MemoryStream memoryStream = new MemoryStream();
		while (true)
		{
			string relativeUri2 = string.Format(_E05B._E000(7083), relativeUri, chunkSize);
			ILeasedChannel leasedChannel;
			try
			{
				leasedChannel = this.m__E000.WaitForLeaseChannel(cancellationToken, relativeUri2, this, isImportant: true);
			}
			catch (ChannelProviderNoAvailableChannelsException)
			{
				_E001.LogWarning(_E05B._E000(7094), relativeUri);
				metadata = null;
				return false;
			}
			try
			{
				leasedChannel.DownloadFile(relativeUri2, memoryStream, cancellationToken);
				leasedChannel.Dispose(ChannelRevokationReason.MetadataSuccessfullyReceived);
			}
			catch (ChannelException exception)
			{
				leasedChannel.Dispose(_E000(exception));
				memoryStream.Position = 0L;
				memoryStream.SetLength(0L);
				continue;
			}
			catch (Exception exception2)
			{
				leasedChannel.Dispose(_E000(exception2));
				throw;
			}
			break;
		}
		memoryStream.Position = 0L;
		using StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true);
		using JsonTextReader reader2 = new JsonTextReader(reader);
		metadata = new JsonSerializer().Deserialize<MultichannelFileMetadata>(reader2);
		_E001.LogDebug(_E05B._E000(7069), relativeUri);
		return true;
	}

	public ChannelRevokationReason _E000(Exception exception)
	{
		if (exception is ChannelLeaseRevokedException)
		{
			return new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(7123));
		}
		if (!(exception is ChannelFileNotFoundException ex))
		{
			if (!(exception is ChannelException ex2))
			{
				if (!(exception is OperationCanceledException))
				{
					if (!(exception is ThreadAbortException))
					{
						if (exception is ChunkChecksumException)
						{
							return new ChannelRevokationReason(LogLevel.Warning, _E05B._E000(6441));
						}
						return new ChannelRevokationReason(LogLevel.Error, exception, _E05B._E000(6405));
					}
					return new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(6863));
				}
				return new ChannelRevokationReason(LogLevel.Trace, _E05B._E000(6891));
			}
			Exception ex3 = ex2;
			while (ex3.InnerException != null)
			{
				ex3 = ex3.InnerException;
			}
			if (!(ex3 is WebException ex4) || !(ex4.Response is HttpWebResponse httpWebResponse) || httpWebResponse.StatusCode != HttpStatusCode.ServiceUnavailable)
			{
				return new ChannelRevokationReason(LogLevel.Warning, _E05B._E000(6756), ex3.GetType().Name, ex3.HResult, ex3.Message);
			}
			return new ChannelRevokationReason(LogLevel.Trace, _E05B._E000(6797));
		}
		return new ChannelRevokationReason(LogLevel.Trace, _E05B._E000(6704), ex.File);
	}
}
internal class _E04A : Microsoft.Extensions.Logging.ILogger
{
	private readonly MultichannelDownloaderContext _E000;

	private readonly Microsoft.Extensions.Logging.ILogger _E001;

	public _E04A(MultichannelDownloaderContext context, Microsoft.Extensions.Logging.ILogger logger)
	{
		_E000 = context;
		_E001 = logger ?? throw new ArgumentNullException(_E05B._E000(6576));
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return _E001.BeginScope(state);
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return _E001.IsEnabled(logLevel);
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		using (BeginScope(new Dictionary<string, object> { 
		{
			_E05B._E000(6585),
			_E000
		} }))
		{
			_E001.Log(logLevel, eventId, state, exception, formatter);
		}
	}
}
internal class _E04B : IDisposable
{
	private static int m__E000;

	[CompilerGenerated]
	private readonly int m__E001 = Interlocked.Increment(ref _E04B.m__E000);

	private readonly string m__E002;

	private readonly Stream m__E003;

	private readonly _E049 m__E004;

	private readonly int _E005;

	private readonly bool _E006;

	private readonly IChannelProvider _E007;

	private readonly Microsoft.Extensions.Logging.ILogger _E008;

	private readonly _E04C _E009;

	private readonly CancellationTokenSource _E00A = new CancellationTokenSource();

	private readonly ManualResetEventSlim _E00B = new ManualResetEventSlim(initialState: false);

	private readonly ManualResetEventSlim _E00C = new ManualResetEventSlim(initialState: false);

	private _E048 _E00D;

	private ConcurrentQueue<IChunk> _E00E;

	private Exception _exception;

	private int _E00F;

	public int Id
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
	}

	public _E04B(string relativeUri, Stream destinationStream, Action<long, long> onProgress, _E049 helper, int defaultChunkSize, bool tryUseMetadata, IChannelProvider channelProvider, Microsoft.Extensions.Logging.ILogger logger, TimeSpan progressInterval)
	{
		this.m__E002 = relativeUri ?? throw new ArgumentNullException(_E05B._E000(4847));
		this.m__E003 = destinationStream ?? throw new ArgumentNullException(_E05B._E000(4859));
		this.m__E004 = helper;
		_E005 = defaultChunkSize;
		_E006 = tryUseMetadata;
		_E007 = channelProvider ?? throw new ArgumentNullException(_E05B._E000(6534));
		_E008 = logger;
		_E009 = new _E04C(onProgress, progressInterval);
	}

	public void Dispose()
	{
		if (_E00A.IsCancellationRequested)
		{
			return;
		}
		_E007.OnChannelForLeaseAppeared -= _E003;
		_E00A.Cancel();
		_E009.Dispose();
		if (_E00E != null)
		{
			IChunk result;
			while (_E00E.TryDequeue(out result))
			{
				result.Dispose();
			}
		}
		_E004();
		_E00C.Set();
		_E00B.Set();
		_E008.LogDebug(_E05B._E000(6550), Id);
	}

	internal async Task _E000()
	{
		_E008.LogDebug(_E05B._E000(59099), Id, this.m__E002);
		long num = await this.m__E004._E000(this.m__E002, _E00A.Token);
		_E009._E000(num);
		if (this.m__E003.Length == 0L)
		{
			_E00D = _E048._E000(this.m__E003, num, _E005);
		}
		else
		{
			try
			{
				_E00D = _E048._E000(this.m__E003, num);
			}
			catch (MultichannelDownloaderFilePostfixException exception)
			{
				_E008.LogWarning(exception, _E05B._E000(58649), Id);
				_E00D = _E048._E000(this.m__E003, num, _E005);
			}
		}
		MultichannelFileMetadata metadata = null;
		if (_E006 && this.m__E004._E000(this.m__E002, _E005, _E00A.Token, out metadata))
		{
			if (metadata.FleSize != num || metadata.ChunkSize != _E005 || metadata.Checksums?.Length != _E00D._E000.Count)
			{
				_E008.LogError(_E05B._E000(58791), metadata.FleSize, num, metadata.ChunkSize, _E005, metadata.Checksums.Length, _E00D._E000.Count, Id);
				metadata = null;
			}
			else
			{
				_E008.LogInformation(_E05B._E000(58436), this.m__E002, Id);
			}
		}
		_E00E = new ConcurrentQueue<IChunk>();
		int num2 = 0;
		long num3 = 0L;
		int num4 = 0;
		long num5 = 0L;
		foreach (_E03E item in _E00D._E000)
		{
			if (item._E000)
			{
				_E009._E001(item._E001);
				num2++;
				num3 += item._E001;
				continue;
			}
			int? checksum = null;
			if (metadata != null)
			{
				if (metadata.Checksums.Length > item._E000)
				{
					checksum = metadata.Checksums[item._E000];
				}
				else
				{
					_E008.LogError(_E05B._E000(58620), item._E000, Id);
				}
			}
			_E043 obj = new _E043(item._E000, item._E000, item._E001, checksum);
			_E00E.Enqueue(obj);
			_E009._E000(obj);
			num4++;
			num5 += item._E001;
		}
		_E009._E000();
		_E008.LogDebug(_E05B._E000(58141), Id, num2, Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num3), num4, Bsg.Network.MultichannelDownloading.HumanReadableSizeExtensions.ToHumanReadableSize(num5));
		_E007.OnChannelForLeaseAppeared += _E003;
		this._E000();
	}

	internal void _E000(CancellationToken cancellationToken)
	{
		_E008.LogDebug(_E05B._E000(6607), Id);
		_E00B.Wait(cancellationToken);
	}

	internal void _E001(CancellationToken cancellationToken)
	{
		_E008.LogTrace(_E05B._E000(6243), Id);
		_E00C.Wait(cancellationToken);
		if (!_E00B.IsSet)
		{
			_E00B.Set();
		}
		cancellationToken.ThrowIfCancellationRequested();
		if (_exception != null)
		{
			throw _exception;
		}
		_E00A.Token.ThrowIfCancellationRequested();
	}

	private bool _E000()
	{
		if (!_E00A.Token.IsCancellationRequested)
		{
			return _E00E.Count > 0;
		}
		return false;
	}

	private void _E000()
	{
		if (!this._E000())
		{
			return;
		}
		lock (this)
		{
			try
			{
				ILeasedChannel leasedChannel;
				while (this._E000() && _E007.TryLeaseChannel(out leasedChannel, this.m__E002, this))
				{
					_E00F++;
					if (!ThreadPool.QueueUserWorkItem(_E000, leasedChannel))
					{
						_E00F--;
						leasedChannel.Dispose();
						_E008.LogError(_E05B._E000(6320), Id);
					}
				}
			}
			catch (ChannelProviderNoAvailableChannelsException exception)
			{
				ChannelProviderNoAvailableChannelsException ex = (ChannelProviderNoAvailableChannelsException)(_exception = exception);
				_E00B.Set();
				_E00C.Set();
			}
		}
	}

	private void _E001()
	{
		lock (this)
		{
			if (!_E00B.IsSet)
			{
				_E008.LogDebug(_E05B._E000(6393), Id);
				_E00B.Set();
			}
		}
	}

	private void _E000(object state)
	{
		ILeasedChannel leasedChannel = state as ILeasedChannel;
		try
		{
			Thread.CurrentThread.Name = _E05B._E000(59190) + leasedChannel.Endpoint.Host;
			_E00A.Token.ThrowIfCancellationRequested();
			IChunk result;
			while (_E00E.TryDequeue(out result))
			{
				if (!this._E000())
				{
					this._E001();
				}
				bool flag;
				try
				{
					leasedChannel.FillChunk(this.m__E002, result, _E00A.Token);
					_E008.LogTrace(_E05B._E000(59197), result);
					result.EnsureChecksum();
					lock (this.m__E003)
					{
						result.PourOut(this.m__E003);
						_E008.LogTrace(_E05B._E000(59155), result);
						_E00D._E000(result, this.m__E003);
						_E008.LogTrace(_E05B._E000(59237), result);
						flag = _E00D._E000 == 0;
					}
				}
				catch (ChunkChecksumException exception)
				{
					_E00E.Enqueue(result);
					leasedChannel.AddError(exception);
					_E008.LogError(_E05B._E000(59256), leasedChannel, this.m__E002, result, Id);
					throw;
				}
				catch
				{
					_E00E.Enqueue(result);
					_E008.LogTrace(_E05B._E000(59386), result);
					throw;
				}
				result.Dispose();
				if (!flag)
				{
					continue;
				}
				try
				{
					lock (this.m__E003)
					{
						_E00D._E000(this.m__E003);
					}
				}
				catch (Exception exception2)
				{
					_E008.LogError(exception2, _E05B._E000(59350), Id);
					throw;
				}
				_E009._E001();
				_E009._E002();
				_E008.LogDebug(_E05B._E000(58898), Id);
				_E00C.Set();
			}
			_E000(leasedChannel, ChannelRevokationReason.TheNeedIsExhausted);
		}
		catch (Exception ex)
		{
			ChannelRevokationReason channelRevokationReason = this.m__E004._E000(ex);
			_E000(leasedChannel, channelRevokationReason);
			if (channelRevokationReason.Severity >= LogLevel.Error)
			{
				_exception = new MultichannelDownloadingException(_E05B._E000(59003), ex);
				Dispose();
			}
		}
	}

	private void _E000(ILeasedChannel channel, ChannelRevokationReason revokationReason)
	{
		try
		{
			bool flag;
			lock (this)
			{
				flag = --_E00F == 0;
			}
			channel.Dispose(revokationReason);
			if (flag)
			{
				_E002();
			}
		}
		catch (Exception exception)
		{
			if (_exception == null)
			{
				_exception = exception;
				Dispose();
			}
		}
	}

	private void _E002()
	{
		_E008.LogDebug(_E05B._E000(58967), Id);
		this._E000();
	}

	private void _E003()
	{
		this._E000();
	}

	private void _E004()
	{
		if (!SpinWait.SpinUntil(() => _E00F == 0, 15000))
		{
			_E008.LogError(_E05B._E000(59015), Id);
		}
	}

	[CompilerGenerated]
	private bool _E001()
	{
		return _E00F == 0;
	}
}
internal class _E04C : IDisposable
{
	private readonly Action<long, long> m__E000;

	private readonly List<IChunk> m__E001;

	private readonly Metronome m__E002;

	private long _E003;

	private long _E004;

	private long _E005;

	public _E04C(Action<long, long> progressCallback, TimeSpan progressInterval)
	{
		this.m__E000 = progressCallback;
		this.m__E001 = new List<IChunk>();
		this.m__E002 = new Metronome(progressInterval, _E002, _E05B._E000(57875));
	}

	public void _E000(long fileSize)
	{
		_E003 = fileSize;
	}

	public void _E001(long bytes)
	{
		Interlocked.Add(ref _E004, bytes);
	}

	public void _E000(IChunk chunk)
	{
		if (this.m__E002.IsStarted)
		{
			throw new InvalidOperationException(_E05B._E000(57958));
		}
		this.m__E001.Add(chunk);
	}

	public void _E000()
	{
		this.m__E002.Start();
	}

	public void _E001()
	{
		this.m__E002.Start();
	}

	public void _E002()
	{
		long num = _E004 + ((IEnumerable<IChunk>)this.m__E001).Sum((Func<IChunk, long>)((IChunk c) => c.BytesFilled));
		if (Interlocked.Exchange(ref _E005, num) != num)
		{
			this.m__E000(num, _E003);
		}
	}

	public void Dispose()
	{
		this.m__E002.Dispose();
	}
}
[CompilerGenerated]
internal sealed class _E04D
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 20)]
	private struct _E000
	{
	}

	internal static readonly _E000 _E000/* Not supported: data(32 00 00 00 64 00 00 00 C8 00 00 00 F4 01 00 00 BC 02 00 00) */;
}
internal sealed class _E04E
{
	public uint _E000;

	public uint _E001;

	public uint _E002;

	public uint _E003;

	public _E04E()
	{
		_E000 = 1732584193u;
		_E001 = 4023233417u;
		_E002 = 2562383102u;
		_E003 = 271733878u;
	}

	public override string ToString()
	{
		return _E04F._E000(_E000).ToString(_E05B._E000(60769)) + _E04F._E000(_E001).ToString(_E05B._E000(60769)) + _E04F._E000(_E002).ToString(_E05B._E000(60769)) + _E04F._E000(_E003).ToString(_E05B._E000(60769));
	}
}
internal sealed class _E04F
{
	public static uint _E000(uint uiNumber, ushort shift)
	{
		return (uiNumber >> 32 - shift) | (uiNumber << (int)shift);
	}

	public static uint _E000(uint uiNumber)
	{
		return ((uiNumber & 0xFF) << 24) | (uiNumber >> 24) | ((uiNumber & 0xFF0000) >> 8) | ((uiNumber & 0xFF00) << 8);
	}
}
[CompilerGenerated]
internal sealed class _E050
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 6)]
	private struct _E000
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 256)]
	private struct _E001
	{
	}

	internal static readonly _E001 _E000/* Not supported: data(78 A4 6A D7 56 B7 C7 E8 DB 70 20 24 EE CE BD C1 AF 0F 7C F5 2A C6 87 47 13 46 30 A8 01 95 46 FD D8 98 80 69 AF F7 44 8B B1 5B FF FF BE D7 5C 89 22 11 90 6B 93 71 98 FD 8E 43 79 A6 21 08 B4 49 62 25 1E F6 40 B3 40 C0 51 5A 5E 26 AA C7 B6 E9 5D 10 2F D6 53 14 44 02 81 E6 A1 D8 C8 FB D3 E7 E6 CD E1 21 D6 07 37 C3 87 0D D5 F4 ED 14 5A 45 05 E9 E3 A9 F8 A3 EF FC D9 02 6F 67 8A 4C 2A 8D 42 39 FA FF 81 F6 71 87 22 61 9D 6D 0C 38 E5 FD 44 EA BE A4 A9 CF DE 4B 60 4B BB F6 70 BC BF BE C6 7E 9B 28 FA 27 A1 EA 85 30 EF D4 05 1D 88 04 39 D0 D4 D9 E5 99 DB E6 F8 7C A2 1F 65 56 AC C4 44 22 29 F4 97 FF 2A 43 A7 23 94 AB 39 A0 93 FC C3 59 5B 65 92 CC 0C 8F 7D F4 EF FF D1 5D 84 85 4F 7E A8 6F E0 E6 2C FE 14 43 01 A3 A1 11 08 4E 82 7E 53 F7 35 F2 3A BD BB D2 D7 2A 91 D3 86 EB) */;

	internal static readonly _E000 _E001/* Not supported: data(5C 00 2F 00 20 00) */;
}
internal class _E051
{
	private const int m__E000 = 1000;

	private const int _E001 = 1024;

	private readonly MessageTemplateParser _E002 = new MessageTemplateParser();

	private readonly object _E003 = new object();

	private readonly Hashtable _E004 = new Hashtable();

	public MessageTemplate _E000(string messageTemplate)
	{
		if (messageTemplate == null)
		{
			throw new ArgumentNullException(_E05B._E000(60777));
		}
		if (messageTemplate.Length > 1024)
		{
			return _E002.Parse(messageTemplate);
		}
		MessageTemplate messageTemplate2 = (MessageTemplate)_E004[messageTemplate];
		if (messageTemplate2 != null)
		{
			return messageTemplate2;
		}
		messageTemplate2 = _E002.Parse(messageTemplate);
		lock (_E003)
		{
			if (_E004.Count == 1000)
			{
				_E004.Clear();
			}
			_E004[messageTemplate] = messageTemplate2;
			return messageTemplate2;
		}
	}
}
internal class _E052 : Microsoft.Extensions.Logging.ILogger
{
	private const string m__E000 = "SOURCE_CONTEXT";

	private readonly _E054 _E001;

	private readonly Serilog.ILogger _E002;

	private static readonly _E051 _E003 = new _E051();

	private static readonly LogEventProperty[] _E004 = (from n in Enumerable.Range(0, 48)
		select new LogEventProperty(_E05B._E000(60865), new ScalarValue(n))).ToArray();

	public _E052(_E054 provider, Serilog.ILogger logger, string name)
	{
		if (logger == null)
		{
			throw new ArgumentNullException(_E05B._E000(6576));
		}
		_E001 = provider ?? throw new ArgumentNullException(_E05B._E000(60793));
		_E002 = logger.ForContext(_E05B._E000(60736), name);
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return _E002.IsEnabled((LogEventLevel)logLevel);
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return _E001._E000(state);
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (!_E002.IsEnabled((LogEventLevel)logLevel))
		{
			return;
		}
		try
		{
			_E000((LogEventLevel)logLevel, eventId, state, exception, formatter);
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine(string.Format(_E05B._E000(60753), _E05B._E000(60858), arg));
		}
	}

	private void _E000<_E008>(LogEventLevel level, EventId eventId, _E008 state, Exception exception, Func<_E008, Exception, string> formatter)
	{
		Serilog.ILogger logger = _E002;
		string text = null;
		List<LogEventProperty> list = new List<LogEventProperty>();
		if ((object)state is IEnumerable<KeyValuePair<string, object>> enumerable)
		{
			foreach (KeyValuePair<string, object> item in enumerable)
			{
				LogEventProperty property3;
				if (item.Key == _E05B._E000(60804) && item.Value is string text2)
				{
					text = text2;
				}
				else if (item.Key.StartsWith(_E05B._E000(60827)))
				{
					if (logger.BindProperty(item.Key.Substring(1), item.Value, destructureObjects: true, out var property))
					{
						list.Add(property);
					}
				}
				else if (item.Key.StartsWith(_E05B._E000(11798)))
				{
					if (logger.BindProperty(item.Key.Substring(1), item.Value?.ToString(), destructureObjects: true, out var property2))
					{
						list.Add(property2);
					}
				}
				else if (logger.BindProperty(item.Key, item.Value, destructureObjects: false, out property3))
				{
					list.Add(property3);
				}
			}
			Type type = state.GetType();
			TypeInfo typeInfo = type.GetTypeInfo();
			if (text == null && !typeInfo.IsGenericType)
			{
				text = _E05B._E000(4026) + type.Name + _E05B._E000(60825);
				if (logger.BindProperty(type.Name, _E000(state, formatter), destructureObjects: false, out var property4))
				{
					list.Add(property4);
				}
			}
		}
		if (text == null)
		{
			string text3 = null;
			if (state != null)
			{
				text3 = _E05B._E000(60829);
				text = _E05B._E000(60903);
			}
			else if (formatter != null)
			{
				text3 = _E05B._E000(60909);
				text = _E05B._E000(60917);
			}
			if (text3 != null && logger.BindProperty(text3, _E000(state, formatter), destructureObjects: false, out var property5))
			{
				list.Add(property5);
			}
		}
		if (eventId.Id != 0 || eventId.Name != null)
		{
			list.Add(_E000(eventId));
		}
		MessageTemplate messageTemplate = _E003._E000(text ?? "");
		LogEvent logEvent = new LogEvent(DateTimeOffset.Now, level, exception, messageTemplate, list);
		logger.Write(logEvent);
	}

	private static object _E000<_E008>(_E008 state, Func<_E008, Exception, string> formatter)
	{
		object result = state;
		if (formatter != null)
		{
			result = formatter(state, null);
		}
		return result;
	}

	internal static LogEventProperty _E000(EventId eventId)
	{
		List<LogEventProperty> list = new List<LogEventProperty>(2);
		if (eventId.Id != 0)
		{
			if (eventId.Id >= 0 && eventId.Id < _E004.Length)
			{
				list.Add(_E004[eventId.Id]);
			}
			else
			{
				list.Add(new LogEventProperty(_E05B._E000(60865), new ScalarValue(eventId.Id)));
			}
		}
		if (eventId.Name != null)
		{
			list.Add(new LogEventProperty(_E05B._E000(60870), new ScalarValue(eventId.Name)));
		}
		return new LogEventProperty(_E05B._E000(60873), new StructureValue(list));
	}
}
internal class _E053 : ILoggerFactory, IDisposable
{
	private readonly _E054 _E000;

	public _E053(_E054 provider)
	{
		_E000 = provider;
	}

	public void Dispose()
	{
		_E000.Dispose();
	}

	public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
	{
		return _E000.CreateLogger(categoryName);
	}

	public void AddProvider(ILoggerProvider provider)
	{
		throw new NotSupportedException();
	}
}
internal class _E054 : ILoggerProvider, IDisposable, ILogEventEnricher, ILogEventSink
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public LogOptions _E000;

		internal void _E000(LoggerSinkConfiguration c)
		{
			c.File(new _E056(), Path.Combine(this._E000.FileLogOptions.LogsDirectory, this._E000.FileLogOptions.LogFileName), retainedFileCountLimit: this._E000.FileLogOptions.LogFilesLimit, restrictedToMinimumLevel: (LogEventLevel)this._E000.FileLogOptions.Level, fileSizeLimitBytes: 1073741824L, levelSwitch: null, buffered: false, shared: false, flushToDiskInterval: null, rollingInterval: RollingInterval.Day);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public LoggingLevelSwitch _E000;

		public _E000 _E001;

		internal void _E000(LoggerSinkConfiguration c)
		{
			c.Seq(_E001._E000.RemoteLogOptions.ServerUrl, LogEventLevel.Verbose, 1000, apiKey: _E001._E000.RemoteLogOptions.ApiToken, controlLevelSwitch: this._E000, period: null, bufferBaseFilename: null, bufferSizeLimitBytes: null, eventBodyLimitBytes: 262144L);
		}
	}

	internal const string _E000 = "{OriginalFormat}";

	private const string m__E001 = "Scope";

	private const string _E002 = "<REMOTE_ONLY>";

	private const string _E003 = "<FILE_ONLY>";

	private readonly AsyncLocal<_E055> _E004 = new AsyncLocal<_E055>();

	private readonly Serilog.ILogger _E005;

	internal _E055 _E000
	{
		get
		{
			return _E004.Value;
		}
		set
		{
			_E004.Value = value;
		}
	}

	public _E054(LogOptions options, IEnumerable<ILogEventEnricher> enrichers)
	{
		LoggerConfiguration loggerConfiguration = new LoggerConfiguration().Enrich.With(enrichers.ToArray()).Enrich.With(this).MinimumLevel.Verbose();
		if (options.FileLogOptions != null)
		{
			loggerConfiguration = loggerConfiguration.WriteTo.Conditional(delegate(LogEvent e)
			{
				if (e.Properties.ContainsKey(_E05B._E000(15220)))
				{
					e.RemovePropertyIfPresent(_E05B._E000(15220));
					return false;
				}
				return true;
			}, delegate(LoggerSinkConfiguration c)
			{
				c.File(new _E056(), Path.Combine(options.FileLogOptions.LogsDirectory, options.FileLogOptions.LogFileName), retainedFileCountLimit: options.FileLogOptions.LogFilesLimit, restrictedToMinimumLevel: (LogEventLevel)options.FileLogOptions.Level, fileSizeLimitBytes: 1073741824L, levelSwitch: null, buffered: false, shared: false, flushToDiskInterval: null, rollingInterval: RollingInterval.Day);
			});
		}
		if (options.ConsoleLogOptions != null)
		{
			loggerConfiguration = loggerConfiguration.WriteTo.Console((LogEventLevel)options.ConsoleLogOptions.Level, _E05B._E000(60881));
		}
		LoggingLevelSwitch loggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);
		if (options.RemoteLogOptions != null)
		{
			loggerConfiguration = loggerConfiguration.WriteTo.Conditional(delegate(LogEvent e)
			{
				if (e.Properties.ContainsKey(_E05B._E000(15208)))
				{
					e.RemovePropertyIfPresent(_E05B._E000(15208));
					return false;
				}
				return true;
			}, delegate(LoggerSinkConfiguration c)
			{
				c.Seq(options.RemoteLogOptions.ServerUrl, LogEventLevel.Verbose, 1000, apiKey: options.RemoteLogOptions.ApiToken, controlLevelSwitch: loggingLevelSwitch, period: null, bufferBaseFilename: null, bufferSizeLimitBytes: null, eventBodyLimitBytes: 262144L);
			});
		}
		_E005 = loggerConfiguration.CreateLogger();
	}

	public Microsoft.Extensions.Logging.ILogger CreateLogger(string name)
	{
		return new _E052(this, _E005, name);
	}

	public IDisposable _E000<_E001>(_E001 state)
	{
		if (this._E000 != null)
		{
			return new _E055(this, state);
		}
		IDisposable chainedDisposable = LogContext.Push(this);
		return new _E055(this, state, chainedDisposable);
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		List<LogEventPropertyValue> list = null;
		for (_E055 obj = this._E000; obj != null; obj = obj._E000)
		{
			obj._E000(logEvent, propertyFactory, out var scopeItem);
			if (scopeItem != null)
			{
				list = list ?? new List<LogEventPropertyValue>();
				list.Add(scopeItem);
			}
		}
		if (list != null)
		{
			list.Reverse();
			logEvent.AddPropertyIfAbsent(new LogEventProperty(_E05B._E000(60438), new SequenceValue(list)));
		}
	}

	public void Dispose()
	{
		(_E005 as IDisposable)?.Dispose();
	}

	public void Emit(LogEvent logEvent)
	{
		_E005.Write(logEvent);
	}
}
internal class _E055 : IDisposable
{
	private const string m__E000 = "None";

	private readonly _E054 _E001;

	private readonly object _E002;

	private readonly IDisposable _E003;

	private bool _E004;

	[CompilerGenerated]
	private readonly _E055 _E005;

	public _E055 _E000
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
	}

	public _E055(_E054 provider, object state, IDisposable chainedDisposable = null)
	{
		_E001 = provider;
		_E002 = state;
		_E005 = _E001._E000;
		_E001._E000 = this;
		_E003 = chainedDisposable;
	}

	public void Dispose()
	{
		if (_E004)
		{
			return;
		}
		_E004 = true;
		for (_E055 obj = _E001._E000; obj != null; obj = obj._E000)
		{
			if (obj == this)
			{
				_E001._E000 = this._E000;
			}
		}
		_E003?.Dispose();
	}

	public void _E000(LogEvent logEvent, ILogEventPropertyFactory propertyFactory, out LogEventPropertyValue scopeItem)
	{
		if (_E002 == null)
		{
			scopeItem = null;
			return;
		}
		if (_E002 is IEnumerable<KeyValuePair<string, object>> enumerable)
		{
			scopeItem = null;
			{
				foreach (KeyValuePair<string, object> item in enumerable)
				{
					if (item.Key == _E05B._E000(60804) && item.Value is string)
					{
						scopeItem = new ScalarValue(_E002.ToString());
						continue;
					}
					string text = item.Key;
					bool destructureObjects = false;
					object obj = item.Value;
					if (text.StartsWith(_E05B._E000(60827)))
					{
						text = text.Substring(1);
						destructureObjects = true;
					}
					if (text.StartsWith(_E05B._E000(11798)))
					{
						text = text.Substring(1);
						obj = obj?.ToString();
					}
					LogEventProperty property = propertyFactory.CreateProperty(text, obj, destructureObjects);
					logEvent.AddPropertyIfAbsent(property);
				}
				return;
			}
		}
		scopeItem = propertyFactory.CreateProperty(_E05B._E000(60440), _E002).Value;
	}
}
internal class _E056 : ITextFormatter
{
	private static readonly Regex m__E000 = new Regex(_E05B._E000(60578), RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

	public void Format(LogEvent logEvent, TextWriter output)
	{
		output.Write(logEvent.Timestamp.ToLocalTime().ToString(_E05B._E000(60515)));
		output.Write(' ');
		string text = ((LogLevel)logEvent.Level).ToString().ToUpperInvariant();
		if (text.Length > 5)
		{
			text = text.Substring(0, 4);
		}
		output.Write((_E05B._E000(15316) + text + _E05B._E000(15327)).PadLeft(7));
		output.Write(' ');
		int newLineOffset = 39;
		string value = _E000(logEvent, output, newLineOffset);
		output.WriteLine(value);
		_E000(logEvent.Exception, output);
	}

	private static void _E000(Exception exc, TextWriter output)
	{
		if (exc == null)
		{
			return;
		}
		output.WriteLine();
		while (exc != null)
		{
			output.Write(_E05B._E000(60543));
			output.Write(exc.GetType().Name);
			output.Write(_E05B._E000(15322));
			output.WriteLine(exc.Message);
			if (exc.StackTrace != null)
			{
				output.WriteLine(_E05B._E000(60482) + exc.StackTrace.Replace(_E05B._E000(14362), _E05B._E000(60484)));
			}
			exc = exc.InnerException;
		}
		output.WriteLine();
	}

	private static string _E000(LogEvent logEvent, TextWriter output, int newLineOffset)
	{
		string input = logEvent.RenderMessage();
		input = _E056.m__E000.Replace(input, "").Replace(_E05B._E000(60492), _E05B._E000(27975)).Replace(_E05B._E000(14362), _E05B._E000(14362) + new string(' ', newLineOffset));
		if (_E000(_E05B._E000(6585), logEvent, output.FormatProvider, out var prop))
		{
			input = _E05B._E000(17420) + prop + _E05B._E000(60497) + input;
		}
		if (_E000(_E05B._E000(5600), logEvent, output.FormatProvider, out var prop2))
		{
			input = input.ReplaceFirst(_E05B._E000(60502), _E05B._E000(59190) + prop2);
		}
		return input;
	}

	private static bool _E000(string propName, LogEvent logEvent, IFormatProvider formatProvider, out string prop)
	{
		if (logEvent.Properties.TryGetValue(propName, out var value))
		{
			using (StringWriter stringWriter = new StringWriter(formatProvider))
			{
				value.Render(stringWriter);
				prop = stringWriter.ToString().Trim('"');
				return true;
			}
		}
		prop = null;
		return false;
	}
}
internal class _E057 : ILogEventEnricher
{
	private readonly ILauncherMetadata _launcherMetadata;

	public _E057(ILauncherMetadata launcherMetadata)
	{
		_launcherMetadata = launcherMetadata;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(_E05B._E000(60585), _launcherMetadata.LauncherVersion.ToString()));
	}
}
internal class _E058 : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(_E05B._E000(60600), Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString()));
	}
}
internal class _E059 : ILogEventEnricher
{
	private readonly string _E000;

	public _E059(LogOptions logOptions)
	{
		_E000 = logOptions.TraceId;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(_E05B._E000(60545), _E000));
	}
}
internal class _E05A : ILogEventEnricher
{
	private readonly Lazy<ISettingsService> _settingsServiceLazy;

	public _E05A(Lazy<ISettingsService> settingsServiceLazy)
	{
		_settingsServiceLazy = settingsServiceLazy;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		if (_settingsServiceLazy.Value.AccountId != _E05B._E000(25616))
		{
			logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_E05B._E000(60552), _settingsServiceLazy.Value.AccountId));
		}
		if (!string.IsNullOrEmpty(_settingsServiceLazy.Value.LoginOrEmail))
		{
			logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_E05B._E000(60565), _settingsServiceLazy.Value.LoginOrEmail.ToSecretData()));
		}
	}
}
internal sealed class _E05B
{
	private delegate string _E000(int _E20B);

	private sealed class _E001
	{
		public MethodBuilder _E000(TypeBuilder _E20C)
		{
			MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig;
			byte[] array3 = default(byte[]);
			string[] array2 = default(string[]);
			MethodBuilder methodBuilder = default(MethodBuilder);
			Type type = default(Type);
			ConstructorInfo constructor = default(ConstructorInfo);
			MethodInfo method11 = default(MethodInfo);
			MethodInfo method12 = default(MethodInfo);
			MethodInfo method13 = default(MethodInfo);
			MethodInfo method = default(MethodInfo);
			MethodInfo method2 = default(MethodInfo);
			MethodInfo method3 = default(MethodInfo);
			MethodInfo method4 = default(MethodInfo);
			ConstructorInfo constructor2 = default(ConstructorInfo);
			MethodInfo method9 = default(MethodInfo);
			MethodInfo method10 = default(MethodInfo);
			ConstructorInfo constructor3 = default(ConstructorInfo);
			MethodInfo method5 = default(MethodInfo);
			MethodInfo method8 = default(MethodInfo);
			MethodInfo method6 = default(MethodInfo);
			MethodInfo method7 = default(MethodInfo);
			while (true)
			{
				int num = -(--104 << 4) >> 6;
				while (true)
				{
					num ^= -(~(-377570926 + 377570980));
					switch (num + (3328 >> 6))
					{
					case 0:
						array3[~(-195112123 - -195112087)] = (byte)(array3[-((~(-(0x6023DC65 ^ -627177025)) + 689607959 >> 1 << 2) - -677430187 + 267282002)] ^ -(~(-698290983 + 698291038)));
						array3[(-970331620 ^ -510367207) - 666730465] = (byte)(array3[-(~(~-375164602 + -426007109) - 715395051) - 664552508] ^ 0x3D);
						array3[-686557730 + 686557804 >> 1] = (byte)(array3[(-(-(-(-736989030 + 147555980)) ^ -223508116) - -222094302 - -524499842) ^ -32530125] ^ -(~(~(-(227875391 + -461238704))) + -233363336 << 1));
						array3[~((-304562959 ^ 0x1759D932) + -409172176) + -654129465 - -152774227] = (byte)(array3[(0x18646417 ^ -427344969) + 18653318] ^ (~(((--1837558703 ^ -662546415) - -123786232 + 426837189 - -300441565) ^ 0x18411427) >> 7));
						num = ~(-498121781 - -498121996) >> 3;
						continue;
					case 1:
					{
						string @string = Encoding.UTF8.GetString(array3);
						char[] array8 = new char[-(~(~(-1227233359 - 116264279) - 651763199) - 207011791) - 387562103 + -511184126];
						array8[-((-866411902 ^ -640592263) + -470601820 - 503285676) - 612469517 >> 5] = (char)(-(~(-(-(-91341591 - 454307331 + 58518641))) + -487130398 >> 1));
						array2 = @string.Split(array8);
						methodBuilder = _E20C.DefineMethod("?", attributes);
						type = Type.GetType(array2[-445657838 ^ -445657853]);
						constructor = type.GetConstructor((BindingFlags)(-(~(102 >> 1))), null, new Type[-(~(-(0x3523A387 ^ -202068553) + 292777647) >> 7) ^ 0x9538C1], null);
						num = -(~(0xCB93C9B ^ -213466246));
						continue;
					}
					case 2:
						array3[-(~(144 >> 3))] = (byte)(array3[(-(-(~(-219766934 ^ 0x6277499)) - 483795926) - 656510654) ^ 0xF2A937] ^ 0x68);
						array3[~(668762383 + -668762394) << 1] = (byte)(array3[-459151728 ^ -459151740] ^ 0x16u);
						array3[-(-2131149164 >> 2) ^ 0x1FC1B04E] = (byte)(array3[-((-(-(~(-(1146381412 >> 1))) - -420537426) << 2) - 610613137)] ^ 0x3D);
						array3[-(--760180320 + -340406231 - 419774265) >> 4 << 6 >> 5] = (byte)(array3[-((0x1FE2CFD4 ^ 0x1C1309) - 536796403)] ^ 0x2Bu);
						num = (0x3E2D197C ^ -390369876) + 158172723 - -536610027;
						continue;
					case 3:
						array3[-(-(((-81496683 << 2) ^ 0xFC4E663) - -407963369) - 72994400) >> 7] = (byte)(array3[~(-(-269172018 + -49540870) - -244280873 + -64730292) + 498263481] ^ ~(-(~((0x26EB92B8 ^ 0x20A62B1E) - 105759399)) >> 5));
						array3[~(652289832 - 652289929) >> 3] = (byte)(array3[(0x21429079 ^ 0x2142907F) << 1] ^ ~(-(-406526398 ^ -111915903) + 364877493 - -147919795));
						array3[((--776750576 + -583317671) ^ 0xB878E09) << 1 >> 7] = (byte)(array3[~(~(-61349385 ^ -476026758) + 438649757) - 97666531] ^ 0x47);
						array3[~(-(~(-(-505817852 - -681582151)))) ^ 0xA79F347] = (byte)(array3[62475593 + -431576891 - -369101312] ^ 0x76);
						num = -(--28 << 1) >> 3 << 6 >> 4;
						continue;
					case 4:
					{
						method11 = typeof(Stream).GetMethod(array2[-((-(-(-1249917780 ^ -406067943) - 391536075) >> 2) ^ 0xC2C1666) ^ -380537225], (BindingFlags)(-(~(((-(0x27549D53 ^ -674263621) ^ 0x2111185E) >> 1) ^ 0x173AFBC4) >> 1)), null, new Type[0 << 7 >> 2], null);
						method12 = typeof(AppDomain).GetMethod(array2[~(-(528444504 + 283040116 - -308474642 >> 1) + 532837630) - 480067881 + 452925895], (BindingFlags)199419975 ^ (BindingFlags)199420031, null, new Type[~((-1001302319 ^ 0xA1161C1) - 293843402 >> 1) - 564247900 << 2], null);
						Type typeFromHandle5 = typeof(AppDomain);
						string name4 = array2[-596413513 + 596413528];
						int bindingAttr6 = -(~(-126068190 ^ -126068207));
						Type[] array10 = new Type[((~(-(-894279828 - -477427643)) + -226641103) ^ 0x282A75F) + 618154746];
						array10[~(-(-142440590 - -55973256)) - -309328205 - 222860870] = typeof(string);
						array10[~(-(~(((0x234B15E7 ^ 0x10165CA3) >> 2) - 215438034)) >> 6)] = typeof(object);
						method13 = typeFromHandle5.GetMethod(name4, (BindingFlags)bindingAttr6, null, array10, null);
						methodBuilder.SetReturnType(typeof(string));
						num = ~(-413425567 + -26217926) + -439643515;
						continue;
					}
					case 5:
						array3 = Convert.FromBase64String("WT9fY3IKPCFJDEszFyICAUkSPg9zSXR5JVQ0JT1rbhx/fG9dBhVoNTNcMzcLTRRaQE47R2V0VHlwZWtyb21IYW5kbAM7Z2V0X05hbWU7SW5kZXhPZjtFeGl0O2dldF9GcmFtZUNvdW50O2dldF9MZW5ndGg7UmVhZFN0cmluZztBZGQ7Z2V0X1Bvc2l0aW9uO2dldF9DdXJyZW50RG9tYWluO1NldERhdGE7UnVudGltZU1ldGhvZDtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tUcmFjZTtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tGcmFtZTsyNjQwMztTeXN0ZW0uRW52aXJvbm1lbnQ7ZGU0ZG90O1NpbXBsZUFzc2VtYmx5RXhwbG9yZXI7YmFiZWx2bTtzbW9rZXRlc3Q=");
						array3[~(-(-523816467 ^ -337241683) ^ 0xB21283F)] = (byte)(array3[~652892827 + 652892828] ^ 0x1Eu);
						array3[--223503011 + -223503010] = (byte)(array3[0x1DFC37BE ^ 0x1DFC37BF] ^ -(~(422414197 + -422408438) >> 6));
						array3[~(-((~(~(-(-495554284 + -680447192) - 538751739)) ^ -90096849) + 547712029))] = (byte)(array3[512099843 + -512099839 >> 1] ^ (-(0x16FFF13C ^ 0x1337A816) - -97016149));
						num = -595550768 - -595550739;
						continue;
					case 6:
						array3[~(~(-(-596930250 - 57688335)) - -654618546)] = (byte)(array3[-(((~(-344513524) ^ 0xE0432BC) - 420899187 << 4) + -392691175)] ^ (-(-802018598 - -456281646) + -345736872));
						array3[-(-718656247 + 414488085 - -435762937 - -585246276 + -304586033) + 412255058] = (byte)(array3[~(307466692 - -43897313 + -351366566) >> 6] ^ -(~((~(-23175506 + 75407971) - 274086624) ^ -326319032)));
						array3[-(-(-204431605 ^ -288143157) + 486791063)] = (byte)(array3[-265788644 ^ -265788619] ^ 3u);
						array3[~((-4863101 + -221532829) ^ 0xD7E8733)] = (byte)(array3[0xEF5B609 ^ 0xEF5B623] ^ -(-(~777989227 - -530903011) ^ -247086334));
						num = -684900109 ^ 0x28D2BF18;
						continue;
					case 7:
						array3[326959063 - 326959048] = (byte)(array3[-671432383 ^ -671432370] ^ -((-(-524667200 >> 4) >> 1) - 16395955));
						array3[-(-(-((-(-616541028 + 306716718) ^ -366846961) + -88913197) + 229252261)) ^ 0x1AA0FA89] = (byte)(array3[((-744863167 - -211473641) ^ -533389462) >> 2] ^ 0x26);
						array3[-(-(~(-62444841 - 305814894 - 390829537) - 679291716) + 79797538)] = (byte)(array3[(--215790147 ^ 0xCDCB063) >> 5] ^ ~(-(~(0x13E4B47 ^ -20859697)) << 3 >> 3));
						array3[-((-(~(~-882015591)) ^ 0x24B0084B) - 300631155) + -29936903 >> 5] = (byte)(array3[-(-(-893905536 >> 5) - -157515959) - -185450525] ^ (-(~(-(~(-(-680634392 ^ 0xAEA739)) + 139421365) ^ -317170590)) + 218631147));
						num = ((-(220624974 - 51977539) + -515598677) ^ 0x261720B0) + 249554153;
						continue;
					case 8:
						array3[(0x33CB95E1 ^ -322763135) + 553086115] = (byte)(array3[-((~(-564908314) ^ -32230635) - -541072881)] ^ ((~(((-(~-1133410457) >> 2) - 436556062) ^ -150827934) ^ -567832743) + -63826916 >> 2));
						array3[~(--250217590 - -688368174 + -225431414 - 517178089) + 195976278 >> 2] = (byte)(array3[((~(-((0x3679EB2D ^ 0x20106E5D) + 612980143)) ^ -404496228) - -98296537) ^ -487776673] ^ 0);
						array3[(~(-(0x2404C0EA ^ -31180749)) + 253813721 - -292178991 >> 5) + 2793150] = (byte)(array3[-(550899606 + -122656343) - -428243268] ^ -(-1448619 ^ 0x161AC0));
						array3[-(~53711406) ^ 0x3339229] = (byte)(array3[~(-(-414512850 + -279031169 - -161047373) - 532496650) << 1] ^ ((-((-1075965636 ^ -510063608) + -45012380) >> 3) - -192088388));
						num = -(-(~(--79311202) + 619275253 - -69921395) + 504371149 - -105514316);
						continue;
					case 9:
					{
						Type type2 = type;
						string name3 = array2[-(~(~(~604898141 - 69740996))) - 674639138];
						int bindingAttr5 = -(-496346165 ^ 0x1D95A407);
						Type[] array9 = new Type[-(-(~((-(-989050718 + 333598468) + -418043024) ^ -237408971)) >> 5) >> 3];
						array9[-(~(~(1603768812 >> 2))) - -400942203] = typeof(int);
						method = type2.GetMethod(name3, (BindingFlags)bindingAttr5, null, array9, null);
						while (true)
						{
							IL_0f1c:
							int num5 = ~(20 >> 2 << 2);
							while (true)
							{
								num5 ^= -(~(~((--1667804253 - 597287964) ^ 0x17231EFE) ^ -686675620) << 7) >> 6;
								switch (num5 + -(-(-211292672 - -426255803) - -214960123 >> 6))
								{
								case 4:
									break;
								default:
									goto IL_0f1c;
								case 0:
									method2 = Type.GetType(array2[(~(-131163194 + -601794731 - 300032656) >> 2) - -56996541 + -315244168]).GetMethod(array2[-83193680 ^ -83193679], (BindingFlags)(~(-(~((0x1BA9D46F ^ -494585190) + -634898942) + 140915149)) + -571397550 + -318912242), null, new Type[(-(~((-326156429 ^ 0xC2FE6DB) + -480199274) + 154544441) + 572031887) ^ -589041259], null);
									num5 = (0x60D9F9E ^ -65344344) - -99126450;
									continue;
								case 1:
									method3 = typeof(MemberInfo).GetMethod(array2[(0x161D2FE ^ 0xAFFC009) + -194908917], (BindingFlags)(-(-383323665 ^ 0x16D90E23)), null, new Type[~((~(-(-(1239868278 + -295618156))) ^ -593194076) - 454259026)], null);
									num5 = ~(-(-9912461 << 6) - 278250775) + 356146712;
									continue;
								case 2:
									num = ~((~783144806 - -473290679 >> 2) + 77464028 >> 4);
									num5 = ~(-337170320 + 337170336);
									continue;
								case 3:
									method4 = typeof(Type).GetMethod(array2[-405028475 - -405028477 << 1], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[((((-1280786642 - -692639290 >> 3) ^ -205262167) - -463678066) ^ 0x2400EA76) << 1], null);
									num5 = -396946798 ^ 0x17A8ED7B;
									continue;
								}
								break;
							}
							break;
						}
						continue;
					}
					case 10:
					{
						Type typeFromHandle3 = typeof(BinaryReader);
						int bindingAttr3 = ~((0x22B274F1 ^ 0x223472B6) + -8783484);
						Type[] array6 = new Type[-(-275840532 - -76244953 + -367737534) - 567333112];
						array6[~603569257 - -603569258] = typeof(Stream);
						constructor2 = typeFromHandle3.GetConstructor((BindingFlags)bindingAttr3, null, array6, null);
						while (true)
						{
							IL_1251:
							int num4 = -(~333939408) ^ -333939400;
							while (true)
							{
								num4 ^= ~(-(~(-128591260 << 1)) ^ 0xF544B0E);
								switch (num4 + -(((-967225123 ^ -296288243) >> 3) - 84004154 >> 4))
								{
								case 4:
									break;
								default:
									goto IL_1251;
								case 0:
									num = (-201991073 ^ 0xC8F5E03) - -8748427;
									num4 = -673144008 ^ 0x281F5CD2;
									continue;
								case 1:
									method9 = typeof(BinaryReader).GetMethod(array2[~(-(-872540578 ^ -622080429)) - 286644225], (BindingFlags)(-(~(--531650095 ^ -74055467)) ^ -467249457), null, new Type[-(-212642328 ^ 0x3A5DC73) ^ 0xF097665], null);
									num4 = ((~(-(985482117 + -437507195) + -664050295) >> 3) + 41714600) ^ -193217744;
									continue;
								case 2:
								{
									Type typeFromHandle4 = typeof(Hashtable);
									string name2 = array2[-(0x1F55DC03 ^ -525720957) >> 5];
									int bindingAttr4 = -((-(731997606 - 219352103) ^ 0x1E7046B7) >> 1 << 4) + -133234204;
									Type[] array7 = new Type[~(-(~24452812 - -24452816))];
									array7[~(~((-((0x38FE3EF9 ^ 0x263F9187) + -543084542) >> 3) - -320165019) ^ 0x1348F8AB)] = typeof(object);
									array7[-(-(-89179262 ^ -89179264) >> 1)] = typeof(object);
									method10 = typeFromHandle4.GetMethod(name2, (BindingFlags)bindingAttr4, null, array7, null);
									num4 = ~((0x1353260 ^ 0x20A09E05) - 563457116);
									continue;
								}
								case 3:
									constructor3 = typeof(Hashtable).GetConstructor((BindingFlags)(~((--91946158 ^ 0x27DAD32A) - 580923321)), null, new Type[~((0x2604A672 ^ -81589845) + 584601638)], null);
									num4 = -((-451682663 ^ 0x19B36F40) - 24683782) - 81261366;
									continue;
								}
								break;
							}
							break;
						}
						continue;
					}
					case 11:
					{
						Type typeFromHandle = typeof(string);
						int bindingAttr = ((~(-(~-108532213 << 2) ^ 0xABF79D2) - -662979749) ^ 0x223F4980) + -491291263 + -193800341 - -267968518;
						Type[] array4 = new Type[-((-182873777 ^ -188803183) << 4 >> 4) ^ -27690720];
						array4[-(-((0x2372A40E ^ 0x27A8C0BA) - -52735496) - -134156988) << 1] = typeof(string);
						array4[~76152342 + 76152344] = typeof(StringComparison);
						method5 = typeFromHandle.GetMethod("IndexOf", (BindingFlags)bindingAttr, null, array4, null);
						while (true)
						{
							IL_159e:
							int num3 = -611065900 + 611065897 << 1;
							while (true)
							{
								num3 ^= 0x33;
								switch (num3 + (~(~(-(0x168EA932 ^ -398050117)) ^ -544006036) + 559658526))
								{
								case 4:
									break;
								default:
									goto IL_159e;
								case 0:
									method8 = typeof(Stream).GetMethod(array2[(--177002319 << 1) - 354004628], (BindingFlags)(-549993684) ^ (BindingFlags)(-549993704), null, new Type[-(((((--1280543463 - 549214736) ^ -626925679) - -410359291) ^ -395301120) + 505632191)], null);
									num3 = ~(~(385178258 + -385178260) << 6 >> 4);
									continue;
								case 1:
									num = -(-(~(~(--930840481) + 510410142 >> 2)) >> 6) - 1642337;
									num3 = ~((-2022208888 >> 3) + 252776118);
									continue;
								case 2:
								{
									Type typeFromHandle2 = typeof(Environment);
									string name = array2[-(~15 << 2 >> 6) << 3];
									int bindingAttr2 = ((~(--205793327) >> 1) ^ -102897560) << 3 >> 7;
									Type[] array5 = new Type[(((((--960227488 + -265884970) ^ 0x1881B62E) >> 1) ^ 0x8491) - 572672122) ^ -154210878];
									array5[-113642205 - -113642205 >> 1] = typeof(int);
									method6 = typeFromHandle2.GetMethod(name, (BindingFlags)bindingAttr2, null, array5, null);
									num3 = -(-505331014 + 505331021);
									continue;
								}
								case 3:
									method7 = typeof(StackTrace).GetMethod(array2[~(((-(-(-159821407 - -32890025) + -227290491) << 1) ^ -200718102) >> 4)], (BindingFlags)(((~(-(-699742408 + 601495187)) ^ -230087556) + 558027618) ^ 0x29B0CD2C), null, new Type[~(--401682857 ^ -401682858)], null);
									num3 = (-196778909 ^ 0xAFF1259) - -21334458;
									continue;
								}
								break;
							}
							break;
						}
						continue;
					}
					case 12:
						array3[-(((--866671772 >> 2) ^ 0x112B59C1) + 140223733) - -639431690] = (byte)(array3[(-(~(1474630918 + -683139259 - 312047370) + -287055604) >> 1) - 383249900] ^ -((~(-((-57268381 ^ -611621419) - 423543640)) ^ 0x286A8B0A) + -632616082));
						array3[-(-(~-446851823) + 446851819) << 4] = (byte)(array3[61001546 - 61001498] ^ --45);
						array3[-(~(~((-39537830 ^ -507293015) + -402666218 - -128701434) + 202605511)) >> 2] = (byte)(array3[~(-(0x3B08B60B ^ -694528225) + -309141278)] ^ ~((-(~(~((0x4D456644 ^ -239708316) >> 1))) ^ -30994463) + 543070019));
						array3[~(-(0x26394F15 ^ 0x26394F51))] = (byte)(array3[~(-(~(-(-762851037 ^ 0x12EF6403)) ^ -590544638) - -100339445) - 380282084] ^ 0x66);
						num = (--374884060 >> 1) ^ -187442028;
						continue;
					case 13:
						array3[(~(-(-(-(1052056559 - 637921093) ^ -632316009) + 385192601)) + 146190376) ^ -494080536] = (byte)(array3[-(-(~1145870237) - 713536013) + 432334248] ^ -(~60));
						array3[(-(-1017018259 - -89733517 + 526265613 - 125204473) ^ -586902104) - -466913381 + 567433817] = (byte)(array3[-((-(~223761684 - 143786316) - 114475477) ^ -253072540)] ^ 0x40u);
						array3[(-195122605 ^ -195122829) >> 5] = (byte)(array3[(~(0x3B45072B ^ 0x1855B56D) - -707658503 >> 6) + -1865002] ^ 0x37);
						array3[-646564091 - -646564117] = (byte)(array3[~(-(~-806110462 - 335904208)) ^ 0x1C06C736] ^ (-(~(-(-162364145 + 551284552 - 326692489))) - -62228005));
						num = ~(-97147661 - 23615293 + 673170231) - -552407256;
						continue;
					case 14:
						array3[~(-30072140 - -30072014 >> 1) >> 1] = (byte)(array3[900854990 - -166965779 + -523995402 - 516593101 + -206772928 + 680749025 - 501208332] ^ ((uint)(-(-(~((-(--343772845) + -38806462) ^ 0xD405D3D)) - -688357635)) ^ 0xF2866E2Fu));
						array3[(-5818915 ^ -145330122) - 702997487 + -125526564 + 678494250 << 4] = (byte)(array3[~(-(-(-(-834376186 ^ -340245887)) + -239712437) + 397570993)] ^ (--364347253 - 364346565 >> 4));
						array3[(-649515545 ^ -676543426) + -249948600] = (byte)(array3[~(((~(-((1294503552 >> 4) - -363570872) << 1) ^ -617591656) + 32810998) ^ 0x91B4A67) + -119803364] ^ ((uint)(~((-491400934 ^ 0x28A56351 ^ 0x27470176) - 340264243) + -519623251) ^ 0x7F77FA7u));
						array3[(-((-201423183 ^ 0x2086011) - 558837272 + -152093960 + 527754543) ^ 0x18F42715) >> 1] = (byte)(array3[-(~(0x1DBA7D5B ^ -459287295)) ^ -114972551] ^ 0x1F);
						num = -(((((-(~(-(~645270043) + -490718755)) ^ -375913196) - -347367682) ^ 0x51019D7) + 123556872) ^ -137360854);
						continue;
					case 15:
						array3[~(-(((-788482669 ^ 0x5CB3B08) + 197735905 - -669177757) ^ 0x8779F98)) >> 7] = (byte)(array3[((-((-1727702765 ^ -252479069) >> 4) << 2) ^ -267655574) + -361337271] ^ ((-(-815236876 ^ -439301882) ^ -157583722) + -601634832 >> 1));
						array3[(0x8589831 ^ 0x279FE534) + -255148323 - 344958145 + -201496345] = (byte)(array3[0x1B2A8484 ^ 0x1B2A848C] ^ 0x72);
						array3[((0x45E8F306 ^ 0x25B6FE4C) - 643918538 >> 5) + -30401803] = (byte)(array3[(-735913517 ^ -48498643) + -691609822 >> 5] ^ 0x4Bu);
						array3[-((1692961460 >> 2) + -423240375)] = (byte)(array3[-(-(-(0x6C67564D ^ -183851326) - 261486208) - 143475013) + -715394607 - 680699357 - 206743072] ^ (-(-(0x550BECAB ^ -94489255) + -714741199) - -638586989));
						num = -((899264800 >> 1) - 449631376 >> 4 << 3) >> 6;
						continue;
					case 16:
						array3[~(-((~((-1750544775 ^ 0x2340988B) - -704264115 + 511184486) << 4) + -60371199)) + -649920021] = (byte)(array3[((0x6B0B2EAB ^ 0x26464F0E) + 443995067 >> 4) + -108806955] ^ -(~(0xC962D5B ^ 0xC962D1A)));
						array3[-(~(0x16F39CA6 ^ -438413430) + -215102719)] = (byte)(array3[~-330570874 - -39519059 + 269703369 + -554960053 - 84833237 << 2] ^ ((uint)(-(~(342092700 - 375999734) - -643855695)) ^ 0xD79A293Fu));
						array3[-(~269030854 - -269030135 << 2 >> 6)] = (byte)(array3[~(-(-(0x2A5496BE ^ -710186644)))] ^ (~(-1018450294 + 420512004) + -597938256));
						array3[-((-591121169 ^ 0x78DD3D0) + -2284162 - -205189200) - 413008581] = (byte)(array3[-((0x1E9ED532 ^ -641297954) >> 2) + -660034594 - 587515369 + 544276382 - -465644640 >> 2] ^ (-94488382 + 203913877 - 109425405));
						num = 0x2A1AE4B9 ^ -706405546;
						continue;
					case 17:
						array3[-168860867 + 168861299 >> 4] = (byte)(array3[~((~(-(-946644981 ^ -246119584)) + -644471199) ^ 0x146C8261) ^ -70419378] ^ (-(-757579734 ^ 0x24512E60) + -158765426));
						array3[~(-(0x12EFFFD3 ^ 0x2221AC58) - -471763800) ^ 0x14AFC82E] = (byte)(array3[~697800818 - -136630341 + 561170506] ^ (-(261996321 + -261996953) >> 3));
						array3[~(-(~(-(~242848806) ^ 0xB9AD693) - -486390862) - -387611003)] = (byte)(array3[-472321356 ^ -472321367] ^ ((uint)((~(-1763213873) - -237092276 >> 1) + -522374263 - 527641138) ^ 0xEFC765B6u ^ 0x12C04CFDu));
						array3[~(~(-(~(-1013719256 + -594908468)) - 393407554) >> 2) - -500508878] = (byte)(array3[~(-(0xCDDD377 ^ -432785702)) - -353777492 >> 7] ^ (--45 << 1 >> 1));
						num = ~(-186764525 + 186764813 >> 4);
						continue;
					case 18:
					{
						MethodBuilder methodBuilder2 = methodBuilder;
						Type[] array = new Type[~(-(--49042403 + -49042339) >> 5)];
						array[0xED14246 ^ 0xED14246] = typeof(Stream);
						methodBuilder2.SetParameters(array);
						while (true)
						{
							int num2 = (-465081583 ^ 0xF0DAB0C) - -347422701;
							while (true)
							{
								num2 ^= 0x30;
								switch (num2 - (0xACF9C3E ^ 0xACF9C07))
								{
								case 0:
								{
									ILGenerator iLGenerator = methodBuilder.GetILGenerator();
									iLGenerator.DeclareLocal(type);
									iLGenerator.DeclareLocal(typeof(long));
									iLGenerator.DeclareLocal(typeof(BinaryReader));
									iLGenerator.DeclareLocal(typeof(Hashtable));
									iLGenerator.DeclareLocal(typeof(string));
									iLGenerator.DeclareLocal(typeof(int));
									iLGenerator.DeclareLocal(typeof(Type));
									iLGenerator.DeclareLocal(typeof(string));
									Label label = iLGenerator.DefineLabel();
									Label label2 = iLGenerator.DefineLabel();
									Label label3 = iLGenerator.DefineLabel();
									Label label4 = iLGenerator.DefineLabel();
									Label label5 = iLGenerator.DefineLabel();
									Label label6 = iLGenerator.DefineLabel();
									Label label7 = iLGenerator.DefineLabel();
									iLGenerator.Emit(OpCodes.Newobj, constructor);
									iLGenerator.Emit(OpCodes.Stloc_0);
									iLGenerator.Emit(OpCodes.Ldc_I4_0);
									iLGenerator.Emit(OpCodes.Stloc_S, -(~(-(~387216464) - -20590518 + -328165272) ^ 0x4BF3C6B));
									iLGenerator.Emit(OpCodes.Br, label);
									iLGenerator.MarkLabel(label5);
									iLGenerator.Emit(OpCodes.Ldloc_0);
									iLGenerator.Emit(OpCodes.Ldloc_S, -373537251 ^ -373537256);
									iLGenerator.Emit(OpCodes.Callvirt, method);
									iLGenerator.Emit(OpCodes.Callvirt, method2);
									iLGenerator.Emit(OpCodes.Callvirt, method3);
									iLGenerator.Emit(OpCodes.Stloc_S, ~(0x14847DC5 ^ -344227268));
									iLGenerator.Emit(OpCodes.Ldloc_S, -(19702268 + -19702274));
									iLGenerator.Emit(OpCodes.Brfalse, label2);
									iLGenerator.Emit(OpCodes.Ldloc_S, (1362291052 - 642924729 + -546187067 << 1 >> 1) - -280894331 + -454072819 >> 7);
									iLGenerator.Emit(OpCodes.Callvirt, method4);
									iLGenerator.Emit(OpCodes.Stloc_S, --259061466 ^ 0xF70F6DD);
									iLGenerator.Emit(OpCodes.Ldloc_S, -(~(0x288DFB58 ^ 0x288DFB5E)));
									iLGenerator.Emit(OpCodes.Ldstr, array2[-(~(-(-(1170452964 + -589226322) - -122604568 + 458622054)))]);
									iLGenerator.Emit(OpCodes.Ldc_I4_5);
									iLGenerator.Emit(OpCodes.Callvirt, method5);
									iLGenerator.Emit(OpCodes.Ldc_I4_M1);
									iLGenerator.Emit(OpCodes.Bne_Un, label3);
									iLGenerator.Emit(OpCodes.Ldloc_S, ~(-(-56826277 + 56826285)));
									iLGenerator.Emit(OpCodes.Ldstr, array2[-(-272682036 + 572483337 - 299801345) >> 1]);
									iLGenerator.Emit(OpCodes.Ldc_I4_5);
									iLGenerator.Emit(OpCodes.Callvirt, method5);
									iLGenerator.Emit(OpCodes.Ldc_I4_M1);
									iLGenerator.Emit(OpCodes.Bne_Un, label3);
									iLGenerator.Emit(OpCodes.Ldloc_S, (((--336358536 ^ 0x1BE6B583) << 1) ^ 0x1FD5B22E) >> 3);
									iLGenerator.Emit(OpCodes.Ldstr, array2[(0x142E73F6 ^ -259008165) + 459167082]);
									iLGenerator.Emit(OpCodes.Ldc_I4_5);
									iLGenerator.Emit(OpCodes.Callvirt, method5);
									iLGenerator.Emit(OpCodes.Ldc_I4_M1);
									iLGenerator.Emit(OpCodes.Bne_Un, label3);
									iLGenerator.Emit(OpCodes.Ldloc_S, -(~(-((-486627863 ^ 0x566D48F) - 394413025) + -504028342) - -299824470) >> 4);
									iLGenerator.Emit(OpCodes.Ldstr, array2[~(-(~(-514241844) + -173170557)) ^ 0x145455AD]);
									iLGenerator.Emit(OpCodes.Ldc_I4_5);
									iLGenerator.Emit(OpCodes.Callvirt, method5);
									iLGenerator.Emit(OpCodes.Ldc_I4_M1);
									iLGenerator.Emit(OpCodes.Beq, label4);
									iLGenerator.MarkLabel(label3);
									iLGenerator.Emit(OpCodes.Ldc_I4_0);
									iLGenerator.Emit(OpCodes.Call, method6);
									iLGenerator.MarkLabel(label4);
									iLGenerator.Emit(OpCodes.Ldloc_S, ~-40894138 - 40894132);
									iLGenerator.Emit(OpCodes.Ldc_I4_1);
									iLGenerator.Emit(OpCodes.Add);
									iLGenerator.Emit(OpCodes.Stloc_S, (0x7234920 ^ 0x723492A) >> 1);
									iLGenerator.MarkLabel(label);
									iLGenerator.Emit(OpCodes.Ldloc_S, -((0x1E0E8F45 ^ -504270691) >> 1 << 3) >> 5);
									iLGenerator.Emit(OpCodes.Ldloc_0);
									iLGenerator.Emit(OpCodes.Callvirt, method7);
									iLGenerator.Emit(OpCodes.Blt, label5);
									iLGenerator.MarkLabel(label2);
									iLGenerator.Emit(OpCodes.Ldarg_0);
									iLGenerator.Emit(OpCodes.Callvirt, method8);
									iLGenerator.Emit(OpCodes.Stloc_1);
									iLGenerator.Emit(OpCodes.Ldarg_0);
									iLGenerator.Emit(OpCodes.Newobj, constructor2);
									iLGenerator.Emit(OpCodes.Stloc_2);
									iLGenerator.Emit(OpCodes.Newobj, constructor3);
									iLGenerator.Emit(OpCodes.Stloc_3);
									iLGenerator.Emit(OpCodes.Ldloc_2);
									iLGenerator.Emit(OpCodes.Callvirt, method9);
									iLGenerator.Emit(OpCodes.Stloc_S, (~-1876851169 >> 4) - 117303194);
									iLGenerator.Emit(OpCodes.Ldloc_3);
									iLGenerator.Emit(OpCodes.Ldc_I4_M1);
									iLGenerator.Emit(OpCodes.Box, typeof(int));
									iLGenerator.Emit(OpCodes.Ldloc_S, ~(-(~(4169182 - 243485325) - 142790129) - -96526008));
									iLGenerator.Emit(OpCodes.Callvirt, method10);
									iLGenerator.Emit(OpCodes.Br, label6);
									iLGenerator.MarkLabel(label7);
									iLGenerator.Emit(OpCodes.Ldloc_3);
									iLGenerator.Emit(OpCodes.Ldarg_0);
									iLGenerator.Emit(OpCodes.Callvirt, method11);
									iLGenerator.Emit(OpCodes.Conv_I4);
									iLGenerator.Emit(OpCodes.Ldc_I4, ~(~(~(0x26B5C083 ^ -643942260) >> 4) >> 7) >> 7);
									iLGenerator.Emit(OpCodes.Add);
									iLGenerator.Emit(arg: int.Parse(array2[((-(~(355611501 + -471502553)) + -638894399) ^ 0x2104F9F9 ^ 0x29B235) + 231757689]), opcode: OpCodes.Ldc_I4);
									iLGenerator.Emit(OpCodes.Xor);
									iLGenerator.Emit(OpCodes.Box, typeof(int));
									iLGenerator.Emit(OpCodes.Ldloc_2);
									iLGenerator.Emit(OpCodes.Callvirt, method9);
									iLGenerator.Emit(OpCodes.Callvirt, method10);
									iLGenerator.MarkLabel(label6);
									iLGenerator.Emit(OpCodes.Ldarg_0);
									iLGenerator.Emit(OpCodes.Callvirt, method11);
									iLGenerator.Emit(OpCodes.Ldloc_1);
									iLGenerator.Emit(OpCodes.Blt, label7);
									iLGenerator.Emit(OpCodes.Call, method12);
									iLGenerator.Emit(OpCodes.Ldloc_S, ~((-((-180689574 >> 1) - 475088388) + -488883848) ^ -76549360) >> 3);
									iLGenerator.Emit(OpCodes.Ldloc_3);
									iLGenerator.Emit(OpCodes.Callvirt, method13);
									iLGenerator.Emit(OpCodes.Ldloc_S, -663217396 ^ -663217400);
									iLGenerator.Emit(OpCodes.Ret);
									num2 = ((((~(-(~(0x260DCD4F ^ 0x72217B5))) << 1 >> 2) ^ 0x10FC9287) << 3) ^ -56360898) >> 1;
									continue;
								}
								case 1:
									methodBuilder.DefineParameter(-(~162633565) - 162633565, (ParameterAttributes)(((-(-685566956 - -538776794 << 1) + -567155892) ^ 0xC51F9BA) - -471832374), "a");
									num2 = 576 >> 6;
									continue;
								case 2:
									return methodBuilder;
								}
								break;
							}
						}
					}
					}
					break;
				}
			}
		}

		public string _E000(Stream _E20D)
		{
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName
			{
				Name = "?"
			}, (AssemblyBuilderAccess)(-(0x20B37357 ^ -548631384))).DefineDynamicModule("?").DefineType("?", (TypeAttributes)(-(~(~-210041676 - 77022288 + 90727556) - -27951969) + -195794975));
			_E000(typeBuilder);
			Type type = typeBuilder.CreateType();
			int invokeAttr = 652915769 - 311644987 + 225772969 - 50351975 + -516691496;
			object[] array = new object[(--18599623 << 1) + 156821164 - 194020409];
			array[~(~(-(-231047344 ^ -231047344) << 6)) >> 4] = _E20D;
			return (string)type.InvokeMember("?", (BindingFlags)invokeAttr, null, null, array);
		}

		static _E001()
		{
			while (true)
			{
				int num = 704611595 - 704603787 >> 6;
				while (true)
				{
					num ^= 0x39;
					switch (num - ((0x212CD5C3 ^ 0x212CD537) >> 2))
					{
					case 0:
						num = ((~(-(~508279657) + 318037125) - -195249815) ^ -631066805) >> 2;
						continue;
					case 1:
						num = (-219612795 ^ -488067084) + -686979764 - -418522185;
						continue;
					case 2:
						num = ~(-(~(--52804202) - -267002785) ^ 0xCC4694C);
						continue;
					case 3:
						num = ~(-(~(-(0xD44A13B ^ -245757019) + 267685847))) ^ -332784959;
						continue;
					case 4:
						num = ~(~(-(-((-1518530956 >> 2) + 664071434) + 39781159)) + 244656513 << 1 >> 4);
						continue;
					case 5:
						num = ~(~((-(-1277668598 + 654830210) >> 1) - 237972784) + -1904920 - -75351205);
						continue;
					case 6:
						num = ~(282956210 + -282956459) >> 1;
						continue;
					case 7:
						num = ((~(-(~(-797443744)) ^ 0x16612903) - 285450188) ^ -706160111) + 49457848;
						continue;
					case 8:
						num = 0x46DE26F ^ 0x46DE268;
						continue;
					case 9:
						return;
					}
					break;
				}
			}
		}
	}

	private static string _E002;

	private static _E000 _E003;

	static _E05B()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		_E003 = _E001;
		Stream stream = _E05C._E000(executingAssembly.GetManifestResourceStream(_E003(-82332320 ^ -82332320)));
		_E002 = new _E001()._E000(stream);
	}

	public static string _E000(int _E208)
	{
		return (string)((Hashtable)AppDomain.CurrentDomain.GetData(_E002))[_E208];
	}

	public static string _E001(int _E209)
	{
		char[] array = "ðÝüØé".ToCharArray();
		int num = array.Length;
		while ((num -= ~(~(-(((--404169469 ^ 0xAC4767D) - 183985635) ^ -243967128) + -156322188) >> 6)) >= ~(0xF41FB3F ^ 0x79FEA4) + 255329692)
		{
			array[num] = (char)(array[num] ^ ((uint)(~(((-2019369204 >> 1) ^ -570233924) - 479344631) - -461000327) ^ 0x1A3902DCu) ^ (uint)_E209);
		}
		return new string(array);
	}
}
[Obfuscation(Feature = "dead code", Exclude = true, StripAfterObfuscation = false)]
internal class _E05C
{
	private static byte[] _E005;

	private static byte[] _E006;

	static _E05C()
	{
		_E005 = new byte[4];
		while (true)
		{
			int num = _E062._E000(78);
			while (true)
			{
				switch (num)
				{
				case 0:
					_E006[3] = 50;
					num = 6;
					continue;
				case 1:
					_E005[3] = 49;
					num = 0;
					continue;
				case 2:
					_E005[2] = (_E006[2] = 65);
					num = 1;
					continue;
				case 3:
					_E005[1] = (_E006[1] = 83);
					num = _E062._E000(83);
					continue;
				case 4:
					_E006 = new byte[4];
					num = 5;
					continue;
				case 5:
					_E005[0] = (_E006[0] = 82);
					num = 3;
					continue;
				case 6:
					return;
				}
				break;
			}
		}
	}

	public static Stream _E000(Stream _E20E)
	{
		BinaryReader binaryReader = new BinaryReader(_E20E);
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		int num = binaryReader.ReadUInt16();
		byte[] array = new byte[num];
		int num3 = default(int);
		byte[] array2 = default(byte[]);
		bool flag3 = default(bool);
		int num5 = default(int);
		bool flag2 = default(bool);
		byte[] array4 = default(byte[]);
		int num6 = default(int);
		RSACryptoServiceProvider rSACryptoServiceProvider = default(RSACryptoServiceProvider);
		byte[] array3 = default(byte[]);
		bool flag = default(bool);
		while (true)
		{
			int num2 = _E062._E000(80);
			while (true)
			{
				switch (num2)
				{
				case 0:
					num3 = 0;
					num2 = 3;
					continue;
				case 1:
					binaryReader.Read(array, 0, num);
					num2 = 4;
					continue;
				case 2:
					array[num3] = (byte)(array[num3] ^ array2[num3 % 4]);
					num2 = 6;
					continue;
				case 3:
					if (num3 != 0)
					{
						num2 = 2;
						continue;
					}
					goto IL_0095;
				case 4:
					array2 = new byte[4];
					num2 = _E062._E000(82);
					continue;
				case 5:
					binaryReader.Read(array2, 0, 4);
					num2 = 0;
					continue;
				case 6:
					{
						num3++;
						goto IL_0095;
					}
					IL_0095:
					if (num3 >= num)
					{
						BinaryReader binaryReader2 = new BinaryReader(new MemoryStream(array, writable: false));
						while (true)
						{
							int num4 = _E062._E000(76);
							while (true)
							{
								switch (num4)
								{
								case 0:
									flag3 = binaryReader2.ReadBoolean();
									num4 = 1;
									continue;
								case 1:
									num5 = binaryReader2.ReadInt32();
									num4 = 10;
									continue;
								case 2:
									if (flag2)
									{
										num4 = _E062._E000(85);
										continue;
									}
									if (0 == 0)
									{
										byte[] publicKey = Assembly.GetExecutingAssembly().GetName().GetPublicKey();
										if (publicKey == null || publicKey.Length != 160)
										{
											throw new InvalidOperationException();
										}
										Buffer.BlockCopy(publicKey, 12, array4, 0, num6);
										while (true)
										{
											int num7 = _E062._E000(80);
											while (true)
											{
												switch (num7)
												{
												case 0:
													rSACryptoServiceProvider = new RSACryptoServiceProvider();
													num7 = 2;
													continue;
												case 1:
													array4[5] |= 128;
													num7 = 0;
													continue;
												case 2:
													goto end_IL_01fc;
												}
												break;
											}
											continue;
											end_IL_01fc:
											break;
										}
										rSACryptoServiceProvider.ImportParameters(_E000(publicKey));
									}
									goto case 11;
								case 3:
									array4 = new byte[num6];
									num4 = 7;
									continue;
								case 4:
									flag2 = binaryReader2.ReadBoolean();
									num4 = 8;
									continue;
								case 5:
									binaryReader2.Read(array3, 0, num5);
									num4 = 4;
									continue;
								case 6:
									flag = binaryReader2.ReadBoolean();
									num4 = 0;
									continue;
								case 7:
									rSACryptoServiceProvider = null;
									num4 = 2;
									continue;
								case 8:
									num6 = binaryReader2.ReadByte();
									num4 = 3;
									continue;
								case 9:
									binaryReader2.ReadString();
									num4 = 6;
									continue;
								case 10:
									array3 = new byte[num5];
									num4 = 5;
									continue;
								case 11:
								{
									if (flag || 1 == 0)
									{
										if (flag2 || 1 == 0)
										{
											binaryReader2.Read(array4, 0, num6);
										}
										int num8 = binaryReader2.ReadByte();
										byte[] array5 = new byte[num8];
										binaryReader2.Read(array5, 0, num8);
										dESCryptoServiceProvider.IV = array5;
										dESCryptoServiceProvider.Key = array4;
									}
									MemoryStream memoryStream = new MemoryStream();
									if (flag || 1 == 0)
									{
										CryptoStream cryptoStream = new CryptoStream(binaryReader.BaseStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read);
										try
										{
											if (flag3 || 1 == 0)
											{
												_E062._E000(cryptoStream, memoryStream);
											}
											else
											{
												_E000(cryptoStream, memoryStream);
											}
										}
										finally
										{
											if (cryptoStream != null || 1 == 0)
											{
												((IDisposable)cryptoStream).Dispose();
											}
										}
									}
									else if (flag3 || 1 == 0)
									{
										_E062._E000(binaryReader.BaseStream, memoryStream);
									}
									else
									{
										_E000(binaryReader.BaseStream, memoryStream);
									}
									if (rSACryptoServiceProvider != null || 1 == 0)
									{
										memoryStream.Position = 0L;
										if (!_E000(rSACryptoServiceProvider, memoryStream, array3) && 0 == 0)
										{
											throw new InvalidOperationException();
										}
									}
									memoryStream.Position = 0L;
									return memoryStream;
								}
								}
								break;
							}
						}
					}
					goto case 2;
				}
				break;
			}
		}
	}

	private static byte[] _E000(byte[] _E20F, int _E20F, int _E20F)
	{
		if (_E20F == null || _E20F.Length < _E20F + _E20F)
		{
			return null;
		}
		byte[] array = new byte[_E20F];
		Array.Copy(_E20F, _E20F, array, 0, _E20F);
		return array;
	}

	private static void _E000(Stream _E210, Stream _E210)
	{
		byte[] array = new byte[4096];
		while (true)
		{
			int num = _E210.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			_E210.Write(array, 0, num);
		}
	}

	private static RSAParameters _E000(byte[] _E211)
	{
		bool flag = _E211.Length == 160;
		if ((flag || 1 == 0) && !_E000(_E211, _E005, 20) && 0 == 0)
		{
			return default(RSAParameters);
		}
		if (!flag && 0 == 0 && !_E000(_E211, _E006, 8) && 0 == 0)
		{
			return default(RSAParameters);
		}
		RSAParameters result = default(RSAParameters);
		int num = ((flag ? true : false) ? 20 : 8);
		int num3 = default(int);
		while (true)
		{
			int num2 = _E062._E000(82);
			while (true)
			{
				switch (num2)
				{
				case 0:
					if (flag)
					{
						num2 = 9;
						continue;
					}
					if (true)
					{
						num += num3;
						while (true)
						{
							int num4 = _E062._E000(88);
							while (true)
							{
								switch (num4)
								{
								case 0:
									num3 = 128;
									num4 = 18;
									continue;
								case 1:
									num += num3;
									num4 = 7;
									continue;
								case 2:
									Array.Reverse(result.InverseQ);
									num4 = 6;
									continue;
								case 3:
									num3 = 64;
									num4 = 19;
									continue;
								case 4:
									Array.Reverse(result.DP);
									num4 = 12;
									continue;
								case 5:
									num += num3;
									num4 = 3;
									continue;
								case 6:
									num += num3;
									num4 = 0;
									continue;
								case 7:
									num3 = 64;
									num4 = 14;
									continue;
								case 8:
									result.InverseQ = _E000(_E211, num, num3);
									num4 = 2;
									continue;
								case 9:
									num3 = 64;
									num4 = 16;
									continue;
								case 10:
									num3 = 64;
									num4 = 8;
									continue;
								case 11:
									Array.Reverse(result.DQ);
									num4 = 21;
									continue;
								case 12:
									num += num3;
									num4 = 9;
									continue;
								case 13:
									Array.Reverse(result.P);
									num4 = 5;
									continue;
								case 14:
									result.DP = _E000(_E211, num, num3);
									num4 = 4;
									continue;
								case 15:
									result.P = _E000(_E211, num, num3);
									num4 = 13;
									continue;
								case 16:
									result.DQ = _E000(_E211, num, num3);
									num4 = 11;
									continue;
								case 17:
									Array.Reverse(result.Q);
									num4 = 1;
									continue;
								case 18:
									result.D = _E000(_E211, num, num3);
									num4 = 20;
									continue;
								case 19:
									result.Q = _E000(_E211, num, num3);
									num4 = _E062._E000(91);
									continue;
								case 20:
									Array.Reverse(result.D);
									num4 = 23;
									continue;
								case 21:
									num += num3;
									num4 = 10;
									continue;
								case 22:
									num3 = 64;
									num4 = 15;
									continue;
								case 23:
									return result;
								}
								break;
							}
						}
					}
					goto case 9;
				case 1:
					Array.Reverse(result.Modulus);
					num2 = 0;
					continue;
				case 2:
					result.Modulus = _E000(_E211, num, num3);
					num2 = 1;
					continue;
				case 3:
					num += num3;
					num2 = 6;
					continue;
				case 4:
					Array.Reverse(result.Exponent);
					num2 = 3;
					continue;
				case 5:
					num += 8;
					num2 = 7;
					continue;
				case 6:
					num3 = 128;
					num2 = _E062._E000(83);
					continue;
				case 7:
					num3 = 4;
					num2 = 8;
					continue;
				case 8:
					result.Exponent = _E000(_E211, num, num3);
					num2 = 4;
					continue;
				case 9:
					return result;
				}
				break;
			}
		}
	}

	private static bool _E000(byte[] _E212, byte[] _E212, int _E212)
	{
		int num = 0;
		while (true)
		{
			int num2 = _E062._E000(84);
			while (true)
			{
				switch (num2)
				{
				case 0:
					if (num != 0)
					{
						num2 = 1;
						continue;
					}
					goto IL_0033;
				case 1:
					if (_E212[num + _E212] != _E212[num])
					{
						num2 = 2;
						continue;
					}
					num++;
					goto IL_0033;
				case 2:
					{
						return false;
					}
					IL_0033:
					if (num < _E212.Length)
					{
						goto case 1;
					}
					return true;
				}
				break;
			}
		}
	}

	private static bool _E000(RSACryptoServiceProvider _E213, Stream _E213, byte[] _E213)
	{
		byte[] array = (array = new SHA1CryptoServiceProvider().ComputeHash(_E213));
		return _E213.VerifyHash(array, null, _E213);
	}
}
internal sealed class _E05D
{
	private sealed class _E000
	{
		internal static int[] _E000(int[] _E222, int _E222)
		{
			int[] array = new int[_E222.Length];
			int num2 = default(int);
			int[] array2 = default(int[]);
			int num4 = default(int);
			int num6 = default(int);
			while (true)
			{
				int num = _E062._E000(80);
				while (true)
				{
					switch (num)
					{
					case 0:
						num2 = 0;
						num = 2;
						continue;
					case 1:
						array2 = new int[_E222.Length];
						num = 0;
						continue;
					case 2:
						if (num2 != 0)
						{
							num = _E062._E000(77);
							continue;
						}
						goto IL_005b;
					case 3:
						array[num2] = num2;
						num = 4;
						continue;
					case 4:
						{
							num2++;
							goto IL_005b;
						}
						IL_005b:
						if (num2 >= array.Length)
						{
							Array.Copy(_E222, array2, _E222.Length);
							while (true)
							{
								int num3 = _E062._E000(84);
								while (true)
								{
									switch (num3)
									{
									case 0:
										Array.Sort(array2, array);
										num3 = 1;
										continue;
									case 1:
										num4 = 0;
										num3 = 2;
										continue;
									case 2:
										if (num4 != 0)
										{
											num3 = 3;
											continue;
										}
										goto IL_00af;
									case 3:
										{
											num4++;
											goto IL_00af;
										}
										IL_00af:
										if (num4 >= array2.Length || array2[num4] != 0 || 1 == 0)
										{
											int[] array3 = new int[array2.Length - num4];
											Array.Copy(array2, num4, array3, 0, array3.Length);
											int[] array4 = ((array3.Length == 0 && 0 == 0) ? new int[0] : ((array3.Length != 1) ? _E001(array3, _E222) : new int[1] { 1 }));
											int[] array5 = new int[_E222.Length];
											while (true)
											{
												int num5 = _E062._E000(80);
												while (true)
												{
													switch (num5)
													{
													case 0:
														num6++;
														num5 = 4;
														continue;
													case 1:
														num6 = 0;
														num5 = _E062._E000(83);
														continue;
													case 2:
														if (num6 != 0)
														{
															num5 = 3;
															continue;
														}
														goto case 4;
													case 3:
														array5[array[num6 + num4]] = array4[num6];
														num5 = 0;
														continue;
													case 4:
														if (num6 < array4.Length)
														{
															goto case 3;
														}
														return array5;
													}
													break;
												}
											}
										}
										goto case 3;
									}
									break;
								}
							}
						}
						goto case 3;
					}
					break;
				}
			}
		}

		private static int[] _E001(int[] _E223, int _E223)
		{
			int num = _E223.Length;
			int[] array = default(int[]);
			int num3 = default(int);
			int[][] array2 = default(int[][]);
			int num10 = default(int);
			int num6 = default(int);
			int num12 = default(int);
			while (true)
			{
				int num2 = _E062._E000(83);
				while (true)
				{
					int num4;
					int[] array5;
					switch (num2)
					{
					case 0:
						array[num3] = _E223[num3 * 2] + _E223[num3 * 2 + 1];
						num2 = _E062._E000(86);
						continue;
					case 1:
						num3 = 0;
						num2 = 5;
						continue;
					case 2:
						array2 = new int[_E223][];
						num2 = 4;
						continue;
					case 3:
						array = new int[_E223.Length / 2];
						num2 = 1;
						continue;
					case 4:
						array2[0] = _E223;
						num2 = 3;
						continue;
					case 5:
						if (num3 != 0)
						{
							num2 = 0;
							continue;
						}
						goto IL_007a;
					case 6:
						{
							num3++;
							goto IL_007a;
						}
						IL_007a:
						if (num3 < array.Length)
						{
							goto case 0;
						}
						num4 = 1;
						if (num4 == 0)
						{
							goto IL_008b;
						}
						goto IL_0120;
						IL_0120:
						if (num4 >= _E223)
						{
							int[] array3 = new int[num];
							while (true)
							{
								int num5 = _E062._E000(84);
								while (true)
								{
									switch (num5)
									{
									case 0:
										num10 = num - 1;
										num5 = 1;
										continue;
									case 1:
										num6 = _E223 - 1;
										num5 = 2;
										continue;
									case 2:
										while (num6 >= 0)
										{
											int[] array4 = array2[num6];
											int num7 = 0;
											int num8 = 0;
											int num9 = 0;
											if (num9 != 0)
											{
												goto IL_0178;
											}
											goto IL_01ab;
											IL_0178:
											if (num8 < _E223.Length && _E223[num8] == array4[num9])
											{
												array3[num8]++;
												num8++;
											}
											else
											{
												num7++;
											}
											num9++;
											goto IL_01ab;
											IL_01ab:
											if (num9 >= num10 * 2)
											{
												num10 = num7;
												num6--;
												continue;
											}
											goto IL_0178;
										}
										return array3;
									}
									break;
								}
							}
						}
						goto IL_008b;
						IL_008b:
						array5 = _E000(array, _E223);
						while (true)
						{
							int num11 = _E062._E000(83);
							while (true)
							{
								switch (num11)
								{
								case 0:
									num12 = 0;
									num11 = 3;
									continue;
								case 1:
									array = new int[array5.Length / 2];
									num11 = _E062._E000(84);
									continue;
								case 2:
									array2[num4] = array5;
									num11 = 1;
									continue;
								case 3:
									if (num12 != 0)
									{
										num11 = 4;
										continue;
									}
									goto IL_0113;
								case 4:
									array[num12] = array5[num12 * 2] + array5[num12 * 2 + 1];
									num11 = 5;
									continue;
								case 5:
									{
										num12++;
										goto IL_0113;
									}
									IL_0113:
									if (num12 >= array.Length)
									{
										goto end_IL_00b5;
									}
									goto case 4;
								}
								break;
							}
							continue;
							end_IL_00b5:
							break;
						}
						num4++;
						goto IL_0120;
					}
					break;
				}
			}
		}

		private static int[] _E000(int[] _E224, int[] _E224)
		{
			int[] array = new int[_E224.Length + _E224.Length];
			int num3 = default(int);
			int num4 = default(int);
			int num2 = default(int);
			while (true)
			{
				int num = _E062._E000(83);
				while (true)
				{
					switch (num)
					{
					case 0:
						array[num3++] = _E224[num4++];
						num = 6;
						continue;
					case 1:
						if (num3 != 0)
						{
							num = 3;
							continue;
						}
						goto case 6;
					case 2:
						num4 = 0;
						num = 5;
						continue;
					case 3:
						if (_E224[num4] < _E224[num2])
						{
							num = 0;
							continue;
						}
						array[num3++] = _E224[num2++];
						goto case 6;
					case 4:
						num3 = 0;
						num = 1;
						continue;
					case 5:
						num2 = 0;
						num = _E062._E000(78);
						continue;
					case 6:
						if (num4 >= _E224.Length || num2 >= _E224.Length)
						{
							while (num4 < _E224.Length)
							{
								array[num3++] = _E224[num4++];
							}
							while (num2 < _E224.Length)
							{
								array[num3++] = _E224[num2++];
							}
							return array;
						}
						goto case 3;
					}
					break;
				}
			}
		}
	}

	private struct _E001
	{
		internal _E05F _E002;

		internal ushort _E003;
	}

	internal static _E05F[] _E000;

	internal static _E05F[] _E001;

	internal static _E061 _E002;

	internal static readonly int[] _E003;

	internal static readonly int[] _E004;

	internal static readonly int[] _E005;

	internal static readonly int[] _E006;

	internal static readonly int[] _E007;

	static _E05D()
	{
		int num4 = default(int);
		int num3 = default(int);
		int num7 = default(int);
		int num6 = default(int);
		while (true)
		{
			int num = _E062._E000(81);
			while (true)
			{
				switch (num)
				{
				case 0:
					_E004[1] = 4;
					_E004[2] = 5;
					_E004[3] = 6;
					num = 1;
					continue;
				case 1:
					_E004[4] = 7;
					_E004[5] = 8;
					_E004[6] = 9;
					num = 16;
					continue;
				case 2:
					_E004[10] = 15;
					_E004[11] = 17;
					_E004[12] = 19;
					num = 14;
					continue;
				case 3:
					_E003 = new int[19];
					_E003[0] = 16;
					_E003[1] = 17;
					num = 12;
					continue;
				case 4:
					_E004[19] = 59;
					_E004[20] = 67;
					_E004[21] = 83;
					num = 11;
					continue;
				case 5:
					num4 = 0;
					if (num4 != 0)
					{
						goto IL_00d5;
					}
					goto IL_0331;
				case 6:
					_E004[25] = 163;
					_E004[26] = 195;
					_E004[27] = 227;
					num = 18;
					continue;
				case 7:
					_E003[9] = 5;
					_E003[10] = 11;
					_E003[11] = 4;
					num = 13;
					continue;
				case 8:
					_E003[18] = 15;
					_E004 = new int[29];
					_E004[0] = 3;
					num = 0;
					continue;
				case 9:
					_E003[15] = 2;
					_E003[16] = 14;
					_E003[17] = 1;
					num = 8;
					continue;
				case 10:
					_E05D._E000 = new _E05F[288];
					_E05D._E001 = new _E05F[32];
					num = 3;
					continue;
				case 11:
					_E004[22] = 99;
					_E004[23] = 115;
					_E004[24] = 131;
					num = 6;
					continue;
				case 12:
					_E003[2] = 18;
					_E003[4] = 8;
					_E003[5] = 7;
					num = 17;
					continue;
				case 13:
					_E003[12] = 12;
					_E003[13] = 3;
					_E003[14] = 13;
					num = 9;
					continue;
				case 14:
					_E004[13] = 23;
					_E004[14] = 27;
					_E004[15] = 31;
					num = 19;
					continue;
				case 15:
					num = 10;
					continue;
				case 16:
					_E004[7] = 10;
					_E004[8] = 11;
					_E004[9] = 13;
					num = 2;
					continue;
				case 17:
					_E003[6] = 9;
					_E003[7] = 6;
					_E003[8] = 10;
					num = 7;
					continue;
				case 18:
					_E004[28] = 258;
					_E005 = new int[29];
					num3 = 8;
					num = 5;
					continue;
				case 19:
					_E004[16] = 35;
					while (true)
					{
						IL_02dd:
						int num2 = _E062._E000(80);
						while (true)
						{
							switch (num2)
							{
							case 3:
								break;
							default:
								goto IL_02dd;
							case 0:
								_E004[18] = 51;
								num2 = 2;
								continue;
							case 1:
								_E004[17] = 43;
								num2 = 0;
								continue;
							case 2:
								num = _E062._E000(78);
								num2 = 3;
								continue;
							}
							break;
						}
						break;
					}
					continue;
				case 20:
					{
						_E005[num3] = num4;
						num3++;
						goto IL_0331;
					}
					IL_0331:
					if (num3 >= 28)
					{
						_E006 = new int[30];
						while (true)
						{
							int num5 = _E062._E000(77);
							while (true)
							{
								int num8;
								int num9;
								int num10;
								int num11;
								int num12;
								switch (num5)
								{
								case 0:
									_E006[14] = 129;
									_E006[15] = 193;
									num5 = 16;
									continue;
								case 1:
									_E006[20] = 1025;
									_E006[21] = 1537;
									num5 = 5;
									continue;
								case 2:
									_E006[28] = 16385;
									_E006[29] = 24577;
									num5 = 8;
									continue;
								case 3:
									_E006[0] = 1;
									_E006[1] = 2;
									num5 = 15;
									continue;
								case 4:
									_E006[26] = 8193;
									_E006[27] = 12289;
									num5 = 2;
									continue;
								case 5:
									_E006[22] = 2049;
									_E006[23] = 3073;
									num5 = 14;
									continue;
								case 6:
									_E006[10] = 33;
									_E006[11] = 49;
									num5 = 11;
									continue;
								case 7:
									_E006[6] = 9;
									_E006[7] = 13;
									num5 = 10;
									continue;
								case 8:
									_E007 = new int[30];
									num7 = 4;
									num5 = 13;
									continue;
								case 9:
									_E006[4] = 5;
									_E006[5] = 7;
									num5 = 7;
									continue;
								case 10:
									_E006[8] = 17;
									_E006[9] = 25;
									num5 = 6;
									continue;
								case 11:
									_E006[12] = 65;
									_E006[13] = 97;
									num5 = 0;
									continue;
								case 12:
									_E006[18] = 513;
									_E006[19] = 769;
									num5 = _E062._E000(80);
									continue;
								case 13:
									num6 = 0;
									if (num6 != 0)
									{
										num5 = 17;
										continue;
									}
									goto IL_05b5;
								case 14:
									_E006[24] = 4097;
									_E006[25] = 6145;
									num5 = 4;
									continue;
								case 15:
									_E006[2] = 3;
									_E006[3] = 4;
									num5 = 9;
									continue;
								case 16:
									_E006[16] = 257;
									_E006[17] = 385;
									num5 = 12;
									continue;
								case 17:
									{
										if (num7 % 2 == 0 && 0 == 0)
										{
											num6++;
										}
										_E007[num7] = num6;
										num7++;
										goto IL_05b5;
									}
									IL_05b5:
									if (num7 < 30)
									{
										goto case 17;
									}
									num8 = 0;
									if (num8 != 0)
									{
										goto IL_05c1;
									}
									goto IL_05ef;
									IL_05ef:
									if (num8 <= 143)
									{
										goto IL_05c1;
									}
									num9 = 144;
									if (num9 == 0)
									{
										goto IL_0603;
									}
									goto IL_063b;
									IL_063b:
									if (num9 <= 255)
									{
										goto IL_0603;
									}
									num10 = 256;
									if (num10 == 0)
									{
										goto IL_064f;
									}
									goto IL_0680;
									IL_0680:
									if (num10 <= 279)
									{
										goto IL_064f;
									}
									num11 = 280;
									if (num11 == 0)
									{
										goto IL_0694;
									}
									goto IL_06cb;
									IL_06cb:
									if (num11 <= 287)
									{
										goto IL_0694;
									}
									num12 = 0;
									if (num12 != 0)
									{
										goto IL_06db;
									}
									goto IL_0716;
									IL_0716:
									if (num12 <= 31)
									{
										goto IL_06db;
									}
									_E002 = _E000(_E05D._E000, _E05D._E001);
									return;
									IL_06db:
									_E05D._E001[num12]._E002 = num12;
									_E05D._E001[num12]._E003 = 5;
									num12++;
									goto IL_0716;
									IL_0694:
									_E05D._E000[num11]._E002 = 192 + num11 - 280;
									_E05D._E000[num11]._E003 = 8;
									num11++;
									goto IL_06cb;
									IL_064f:
									_E05D._E000[num10]._E002 = num10 - 256;
									_E05D._E000[num10]._E003 = 7;
									num10++;
									goto IL_0680;
									IL_0603:
									_E05D._E000[num9]._E002 = 400 + num9 - 144;
									_E05D._E000[num9]._E003 = 9;
									num9++;
									goto IL_063b;
									IL_05c1:
									_E05D._E000[num8]._E002 = 48 + num8;
									_E05D._E000[num8]._E003 = 8;
									num8++;
									goto IL_05ef;
								}
								break;
							}
						}
					}
					goto IL_00d5;
					IL_00d5:
					if (num3 % 4 != 0)
					{
						num = 20;
						continue;
					}
					if (0 == 0)
					{
						num4++;
					}
					goto case 20;
				}
				break;
			}
		}
	}

	internal static int _E000(int[] _E214, int[] _E214)
	{
		int num = 0;
		int num3 = default(int);
		while (true)
		{
			int num2 = _E062._E000(80);
			while (true)
			{
				switch (num2)
				{
				case 0:
					num += _E214[num3] * _E214[num3];
					num2 = _E062._E000(77);
					continue;
				case 1:
					num3 = 0;
					num2 = 2;
					continue;
				case 2:
					if (num3 == 0)
					{
						goto IL_0045;
					}
					num2 = 0;
					continue;
				case 3:
					{
						num3++;
						goto IL_0045;
					}
					IL_0045:
					if (num3 < _E214.Length)
					{
						goto case 0;
					}
					return num;
				}
				break;
			}
		}
	}

	internal static int _E001(int[] _E215, int[] _E215)
	{
		int num = 0;
		int num3 = default(int);
		int num5 = default(int);
		while (true)
		{
			int num2 = _E062._E000(83);
			while (true)
			{
				switch (num2)
				{
				case 0:
					if (num3 != 0)
					{
						num2 = 1;
						continue;
					}
					goto IL_004e;
				case 1:
					num += _E215[num3] * _E05D._E000[num3]._E003;
					num2 = 3;
					continue;
				case 2:
					num3 = 0;
					num2 = _E062._E000(84);
					continue;
				case 3:
					{
						num3++;
						goto IL_004e;
					}
					IL_004e:
					if (num3 >= _E215.Length)
					{
						while (true)
						{
							int num4 = _E062._E000(83);
							while (true)
							{
								switch (num4)
								{
								case 0:
									num += _E215[num5] * _E05D._E001[num5]._E003;
									num4 = 3;
									continue;
								case 1:
									if (num5 != 0)
									{
										num4 = 0;
										continue;
									}
									goto IL_00b5;
								case 2:
									num5 = 0;
									num4 = 1;
									continue;
								case 3:
									{
										num5++;
										goto IL_00b5;
									}
									IL_00b5:
									if (num5 >= _E215.Length)
									{
										return num;
									}
									goto case 0;
								}
								break;
							}
						}
					}
					goto case 1;
				}
				break;
			}
		}
	}

	internal static _E05F[] _E000(int[] _E216)
	{
		_E05F[] array = new _E05F[_E216.Length];
		int num2 = default(int);
		while (true)
		{
			int num = _E062._E000(84);
			while (true)
			{
				switch (num)
				{
				case 0:
					num2 = 0;
					num = _E062._E000(80);
					continue;
				case 1:
					if (num2 != 0)
					{
						num = 2;
						continue;
					}
					goto IL_004a;
				case 2:
					array[num2]._E003 = _E216[num2];
					num = 3;
					continue;
				case 3:
					{
						num2++;
						goto IL_004a;
					}
					IL_004a:
					if (num2 < _E216.Length)
					{
						goto case 2;
					}
					_E05D._E000(array);
					return array;
				}
				break;
			}
		}
	}

	internal static void _E000(_E05F[] _E217)
	{
		int num = _E217[0]._E003;
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_001e;
		}
		goto IL_0042;
		IL_0042:
		if (num2 >= _E217.Length)
		{
			int[] array = new int[num + 1];
			int num4 = default(int);
			int num6 = default(int);
			int num7 = default(int);
			int num10 = default(int);
			while (true)
			{
				int num3 = _E062._E000(83);
				while (true)
				{
					switch (num3)
					{
					case 0:
						if (num4 != 0)
						{
							num3 = 1;
							continue;
						}
						goto IL_00b0;
					case 1:
						array[_E217[num4]._E003]++;
						num3 = 3;
						continue;
					case 2:
						num4 = 0;
						num3 = _E062._E000(84);
						continue;
					case 3:
						{
							num4++;
							goto IL_00b0;
						}
						IL_00b0:
						if (num4 >= _E217.Length)
						{
							int[] array2 = new int[num + 1];
							while (true)
							{
								int num5 = _E062._E000(83);
								while (true)
								{
									switch (num5)
									{
									case 0:
										num6 = 1;
										num5 = 3;
										continue;
									case 1:
										num7 = num7 + array[num6 - 1] << 1;
										num5 = 5;
										continue;
									case 2:
										num7 = 0;
										num5 = 4;
										continue;
									case 3:
										if (num6 == 0)
										{
											num5 = _E062._E000(80);
											continue;
										}
										goto IL_0137;
									case 4:
										array[0] = 0;
										num5 = 0;
										continue;
									case 5:
										array2[num6] = num7;
										num5 = 6;
										continue;
									case 6:
										{
											num6++;
											goto IL_0137;
										}
										IL_0137:
										if (num6 > num)
										{
											int num8 = 0;
											while (true)
											{
												int num9 = _E062._E000(80);
												while (true)
												{
													switch (num9)
													{
													case 0:
														if (num10 != 0)
														{
															num9 = 3;
															continue;
														}
														if (1 == 0)
														{
															goto case 3;
														}
														goto IL_01cd;
													case 1:
														if (num8 != 0)
														{
															num9 = 2;
															continue;
														}
														goto IL_01db;
													case 2:
														num10 = _E217[num8]._E003;
														num9 = _E062._E000(84);
														continue;
													case 3:
														{
															_E217[num8]._E002 = array2[num10];
															array2[num10]++;
															goto IL_01cd;
														}
														IL_01cd:
														num8++;
														goto IL_01db;
														IL_01db:
														if (num8 >= _E217.Length)
														{
															return;
														}
														goto case 2;
													}
													break;
												}
											}
										}
										goto case 1;
									}
									break;
								}
							}
						}
						goto case 1;
					}
					break;
				}
			}
		}
		goto IL_001e;
		IL_001e:
		if (num < _E217[num2]._E003)
		{
			num = _E217[num2]._E003;
		}
		num2++;
		goto IL_0042;
	}

	internal static _E061 _E000(_E05F[] _E218, _E05F[] _E218)
	{
		return new _E061
		{
			_E002 = _E05D._E000(_E218),
			_E003 = _E05D._E000(_E218)
		};
	}

	internal static _E060 _E000(_E05F[] _E219)
	{
		_E001[] array = new _E001[_E219.Length];
		int num = 0;
		int num4 = default(int);
		_E001 obj = default(_E001);
		while (true)
		{
			int num2 = _E062._E000(84);
			while (true)
			{
				switch (num2)
				{
				case 0:
					num4 = 0;
					num2 = 2;
					continue;
				case 1:
					obj = default(_E001);
					num2 = _E062._E000(78);
					continue;
				case 2:
					if (num4 != 0)
					{
						num2 = 3;
						continue;
					}
					goto IL_00d9;
				case 3:
					if (_E219[num4]._E003 > 0)
					{
						num2 = 1;
						continue;
					}
					goto IL_00b5;
				case 4:
					{
						while (true)
						{
							int num3 = _E062._E000(84);
							while (true)
							{
								switch (num3)
								{
								case 0:
									obj._E002 = _E219[num4];
									num3 = 1;
									continue;
								case 1:
									obj._E003 = (ushort)num4;
									num3 = 2;
									continue;
								case 2:
									goto end_IL_0075;
								}
								break;
							}
							continue;
							end_IL_0075:
							break;
						}
						array[num++] = obj;
						goto IL_00b5;
					}
					IL_00b5:
					num4++;
					goto IL_00d9;
					IL_00d9:
					if (num4 < _E219.Length)
					{
						goto case 3;
					}
					return _E000(array, num, 0, 0);
				}
				break;
			}
		}
	}

	private static _E060 _E000(_E001[] _E21A, int _E21A, int _E21A, int _E21A)
	{
		_E001[] array = new _E001[_E21A];
		_E001 obj2 = default(_E001);
		_E060 obj = default(_E060);
		_E001[] array2 = default(_E001[]);
		int num2 = default(int);
		int num4 = default(int);
		int num3 = default(int);
		while (true)
		{
			int num = _E062._E000(83);
			while (true)
			{
				switch (num)
				{
				case 0:
					if (obj2._E002._E002 == _E21A)
					{
						num = _E062._E000(78);
						continue;
					}
					goto IL_00cc;
				case 1:
					obj._E004 = false;
					num = 9;
					continue;
				case 2:
					array2 = new _E001[_E21A];
					num = 7;
					continue;
				case 3:
					obj2 = _E21A[num2];
					num = 5;
					continue;
				case 4:
					obj._E004 = true;
					num = 8;
					continue;
				case 5:
					if (obj2._E002._E003 == _E21A)
					{
						num = 0;
						continue;
					}
					goto IL_00cc;
				case 6:
					num2 = 0;
					num = 10;
					continue;
				case 7:
					obj = new _E060();
					num = 1;
					continue;
				case 8:
					obj._E005 = obj2._E003;
					num = 11;
					continue;
				case 9:
					num4 = (num3 = 0);
					num = 6;
					continue;
				case 10:
					if (num2 != 0)
					{
						num = 3;
						continue;
					}
					goto IL_011f;
				case 11:
					{
						num2++;
						goto IL_011f;
					}
					IL_011f:
					if (num2 >= _E21A)
					{
						if (!obj._E004 && 0 == 0)
						{
							if (num3 > 0)
							{
								obj._E006 = _E000(array, num3, _E21A << 1, _E21A + 1);
							}
							if (num4 > 0)
							{
								obj._E007 = _E000(array2, num4, (_E21A << 1) | 1, _E21A + 1);
							}
						}
						return obj;
					}
					goto case 3;
					IL_00cc:
					if (((uint)(obj2._E002._E002 >> obj2._E002._E003 - _E21A - 1) & (true ? 1u : 0u)) != 0 || 1 == 0)
					{
						array2[num4++] = obj2;
					}
					else
					{
						array[num3++] = obj2;
					}
					goto case 11;
				}
				break;
			}
		}
	}

	internal static void _E000(int _E21B, out int _E21B, out int _E21B)
	{
		_E21B = _E004[_E21B - 257];
		_E21B = _E005[_E21B - 257];
	}

	internal static void _E000(int _E21C, out int _E21C, out int _E21C, out int _E21C)
	{
		int num = Array.BinarySearch(_E004, _E21C);
		if (num < 0)
		{
			num = ~num - 1;
		}
		_E21C = num + 257;
		while (true)
		{
			int num2 = _E062._E000(84);
			while (true)
			{
				switch (num2)
				{
				case 0:
					_E21C -= _E004[num];
					num2 = 1;
					continue;
				case 1:
					_E21C = _E005[num];
					num2 = 2;
					continue;
				case 2:
					return;
				}
				break;
			}
		}
	}

	internal static void _E001(int _E21D, out int _E21D, out int _E21D, out int _E21D)
	{
		int num = Array.BinarySearch(_E006, _E21D);
		if (num < 0)
		{
			num = ~num - 1;
		}
		_E21D = num;
		while (true)
		{
			int num2 = _E062._E000(84);
			while (true)
			{
				switch (num2)
				{
				case 0:
					_E21D -= _E006[num];
					num2 = 1;
					continue;
				case 1:
					_E21D = _E007[num];
					num2 = 2;
					continue;
				case 2:
					return;
				}
				break;
			}
		}
	}

	internal static int[] _E000(int[] _E21E, int _E21E)
	{
		return _E05D._E000._E000(_E21E, _E21E);
	}

	internal static int[] _E000(int[] _E21F)
	{
		return _E05D._E000._E000(_E21F, 15);
	}

	internal static int _E000(int _E220)
	{
		return _E220 switch
		{
			16 => 2, 
			17 => 3, 
			18 => 7, 
			_ => 0, 
		};
	}

	internal static int[] _E000(int[] _E221, int _E221, int _E221)
	{
		_E05E obj = new _E05E();
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_01a6;
		IL_01a6:
		if (num < _E221)
		{
			goto IL_0012;
		}
		return obj._E000();
		IL_0012:
		if (_E221[_E221 + num] == 0 && 0 == 0)
		{
			int num2 = 0;
			do
			{
				num2++;
				while (true)
				{
					int num3 = _E062._E000(83);
					while (true)
					{
						switch (num3)
						{
						case 0:
							if (num2 < 138)
							{
								num3 = 1;
								continue;
							}
							goto end_IL_0029;
						case 1:
							goto IL_005e;
						case 2:
							if (num + num2 < _E221)
							{
								num3 = _E062._E000(84);
								continue;
							}
							goto end_IL_0029;
						case 3:
							goto end_IL_0029;
						}
						break;
						IL_005e:
						if (_E221[_E221 + num + num2] == 0)
						{
							goto end_IL_0046;
						}
						num3 = 3;
					}
					continue;
					end_IL_0046:
					break;
				}
				continue;
				end_IL_0029:
				break;
			}
			while (true);
			if (num2 < 3)
			{
				if (num2 >= 1)
				{
					obj._E000(0);
				}
				if (num2 >= 2)
				{
					obj._E000(0);
				}
			}
			else if (num2 < 11)
			{
				obj._E000(17);
				obj._E000(num2 - 3);
			}
			else
			{
				obj._E000(18);
				obj._E000(num2 - 11);
			}
			num += num2;
		}
		else
		{
			int num4 = _E221[_E221 + num++];
			int num6 = default(int);
			while (true)
			{
				int num5 = _E062._E000(83);
				while (true)
				{
					switch (num5)
					{
					case 0:
						goto IL_0108;
					case 1:
						num6 = 0;
						num5 = 0;
						continue;
					case 2:
						obj._E000(num4);
						num5 = 1;
						continue;
					case 3:
						goto IL_012c;
					}
					break;
					IL_0108:
					if (num6 != 0)
					{
						num5 = _E062._E000(77);
						continue;
					}
					goto IL_0132;
					IL_012c:
					num6++;
					goto IL_0132;
					IL_0132:
					if (num + num6 >= _E221 || num6 >= 6 || _E221[_E221 + num + num6] != num4)
					{
						goto end_IL_00fd;
					}
					goto IL_012c;
				}
				continue;
				end_IL_00fd:
				break;
			}
			if (num6 >= 3)
			{
				obj._E000(16);
				while (true)
				{
					int num7 = _E062._E000(84);
					while (true)
					{
						switch (num7)
						{
						case 0:
							obj._E000(num6 - 3);
							num7 = 1;
							continue;
						case 1:
							num += num6;
							num7 = 2;
							continue;
						case 2:
							goto end_IL_016c;
						}
						break;
					}
					continue;
					end_IL_016c:
					break;
				}
			}
		}
		goto IL_01a6;
	}
}
[DefaultMember("Item")]
internal class _E05E
{
	private int[] _E004;

	private int _E005;

	public int _E000 => _E005;

	public int _E001 => _E004[index];

	public _E05E()
	{
		_E004 = new int[16];
	}

	public int _E000(int _E225)
	{
		if (_E005 == _E004.Length)
		{
			int[] array = new int[_E005 * 2];
			Array.Copy(_E004, 0, array, 0, _E005);
			_E004 = array;
		}
		_E004[_E005] = _E225;
		return _E005++;
	}

	public int[] _E000()
	{
		int[] array = new int[_E005];
		Array.Copy(_E004, 0, array, 0, _E005);
		return array;
	}
}
internal struct _E05F
{
	public int _E002;

	public int _E003;
}
internal sealed class _E060
{
	internal bool _E004;

	internal ushort _E005;

	internal _E060 _E006;

	internal _E060 _E007;
}
internal sealed class _E061
{
	internal _E060 _E002;

	internal _E060 _E003;
}
internal sealed class _E062
{
	public sealed class _E000
	{
		private _E002 _E00C = new _E002(32769);

		private _E001 _E00D;

		private _E061 _E00E;

		private int _E00F = -1;

		private int _E010 = -1;

		private bool _E011;

		private int _E012;

		private long _E013;

		private long _E014;

		private bool _E015;

		private int _E016;

		private bool _E017;

		public _E000(Stream _E228)
		{
			_E00D = new _E001(_E228);
		}

		public int _E000(byte[] _E229, int _E229, int _E229)
		{
			if (((_E229 == 0) ? true : false) || _E015 || 1 == 0)
			{
				return 0;
			}
			int num = 0;
			if (num != 0)
			{
				goto IL_002f;
			}
			goto IL_00c3;
			IL_00c3:
			if (num >= _E229)
			{
				goto IL_00d8;
			}
			goto IL_003e;
			IL_003e:
			if (_E00F < 0 && ((!_E015) ? true : false))
			{
				goto IL_002f;
			}
			if (!_E015 && 0 == 0)
			{
				int num2 = _E001(_E229, _E229 + num, _E229 - num);
				while (true)
				{
					int num3 = _E062._E000(84);
					while (true)
					{
						switch (num3)
						{
						case 0:
							goto IL_0096;
						case 1:
							num += num2;
							num3 = 2;
							continue;
						case 2:
							goto end_IL_008c;
						}
						break;
						IL_0096:
						if (num2 > 0)
						{
							num3 = 1;
							continue;
						}
						_E00F = -1;
						goto end_IL_008c;
					}
					continue;
					end_IL_008c:
					break;
				}
				goto IL_00c3;
			}
			goto IL_00d8;
			IL_00d8:
			return num;
			IL_002f:
			_E015 = !this._E000();
			goto IL_003e;
		}

		private bool _E000()
		{
			if (((!_E011) ? (8 >> 3) : (~234040503 - -234040504)) == 0)
			{
				return false;
			}
			_E013 = _E00D._E009;
			int num2 = default(int);
			while (true)
			{
				int num = _E062._E000(173248802 + -173248724);
				while (true)
				{
					switch (num)
					{
					case 0:
						if (_E00F == --353912547 + -353912546)
						{
							num = -655514075 - -655514082;
							continue;
						}
						if (_E00F == (0x27C032F ^ 0x27C0323) >> 1)
						{
							_E05F[] array = _E05D._E000;
							while (true)
							{
								int num3 = _E062._E000(~(-112547866 - -112547782));
								while (true)
								{
									switch (num3)
									{
									case 0:
										_E017 = (byte)(--212393235 - 212393235) != 0;
										num3 = -670857423 - -670857427;
										continue;
									case 1:
										_E00E = _E05D._E002;
										num3 = -(~-1);
										continue;
									case 2:
									{
										_E05F[] array2 = _E05D._E001;
										num3 = _E062._E000(-(597422985 - 597423062));
										continue;
									}
									case 3:
										_E012 = ~126371551 - -126371552;
										num3 = 573141246 + -573141245;
										continue;
									case 4:
										goto end_IL_0255;
									}
									break;
								}
								continue;
								end_IL_0255:
								break;
							}
						}
						else if (_E00F == (0x296A9867 ^ 0x296A9865))
						{
							_E000(_E00D, out var array, out var array2);
							while (true)
							{
								int num4 = _E062._E000(-95722585 + 95722669);
								while (true)
								{
									switch (num4)
									{
									case 0:
										_E012 = ~40901221 - -40901222;
										num4 = ~327449302 + 327449304;
										continue;
									case 1:
										_E00E = _E05D._E000(array, array2);
										num4 = -631953741 - -631953743;
										continue;
									case 2:
										goto end_IL_0354;
									}
									break;
								}
								continue;
								end_IL_0354:
								break;
							}
							_E017 = false;
						}
						goto case 8;
					case 1:
						_E012 = num2;
						num = ~(383799786 - 383799793);
						continue;
					case 2:
						num2 = _E00D._E000((-912724839 ^ 0x1FF33362) - -697573397) ^ 0x6540;
						num = ~(-2);
						continue;
					case 3:
						_E017 = true;
						num = ~-9;
						continue;
					case 4:
						_E011 = _E00D._E000(~(0x656D2A5 ^ -106353317)) > 0 >> 1 << 4;
						num = ~(0xA82C083 ^ -176341127);
						continue;
					case 5:
						_E00F = _E00D._E000(-589337685 ^ -589337681);
						num = _E062._E000(--603964005 ^ 0x23FFC231);
						continue;
					case 6:
						_E00E = null;
						num = ~-97 >> 5;
						continue;
					case 7:
						_E00D._E000();
						num = 360621763 + -360621761;
						continue;
					case 8:
						_E014 = _E00D._E009;
						return true;
					}
					break;
				}
			}
		}

		private int _E001(byte[] _E22A, int _E22A, int _E22A)
		{
			int num = _E22A;
			if (_E00F == -566939194 - -234897526 + 332041669)
			{
				if (_E012 > ~-1 >> 3)
				{
					int num2 = Math.Min(_E22A, _E012);
					while (true)
					{
						int num3 = _E062._E000(~-380076494 - 380076409);
						while (true)
						{
							switch (num3)
							{
							case 0:
								_E00D._E000(_E22A, _E22A, num2);
								num3 = ~(-633690470 ^ 0x25C55964);
								continue;
							case 1:
								_E00C._E000(_E22A, _E22A, num2);
								num3 = -115900774 ^ -115900775;
								continue;
							case 2:
								_E22A += num2;
								num3 = _E062._E000(164 << 6 >> 7);
								continue;
							case 3:
								_E012 -= num2;
								num3 = -532816548 + 532816552;
								continue;
							case 4:
								_E22A -= num2;
								num3 = ~-3;
								continue;
							case 5:
								goto end_IL_007e;
							}
							break;
						}
						continue;
						end_IL_007e:
						break;
					}
				}
			}
			else if (((!_E017) ? (~(-707734141 + 707734140)) : (-(-128 >> 7))) == 0)
			{
				if (_E016 > (0x985FDAD ^ -322792724) + 448303295)
				{
					_E000(_E22A, ref _E22A, ref _E22A);
				}
				if (_E22A > (189373180 << 1) - 378746360)
				{
					int num9 = default(int);
					do
					{
						int num4 = _E000(_E00D, _E00E._E002);
						_E017 = num4 == -112199175 - -112203271 >> 4;
						if (((!_E017) ? (~-1 << 3) : (-(692596696 + -692596697))) != 0)
						{
							break;
						}
						if (num4 < (0x94FD7D1 ^ 0x94FDFD1) >> 3)
						{
							int num5 = _E22A;
							_E22A = num5 + (~635998851 + 635998853);
							_E22A[num5] = (byte)num4;
							while (true)
							{
								int num6 = _E062._E000(523998691 + -523998611);
								while (true)
								{
									switch (num6)
									{
									case 0:
										_E22A--;
										num6 = ~304806442 + 304806445;
										continue;
									case 1:
										_E00C._E000((byte)num4);
										num6 = -0 << 1;
										continue;
									case 2:
										goto end_IL_02bf;
									}
									break;
								}
								continue;
								end_IL_02bf:
								break;
							}
						}
						else
						{
							if (num4 > 4560 >> 4)
							{
								continue;
							}
							int num7 = _E000(_E00D, num4);
							while (true)
							{
								int num8 = _E062._E000(~(-522552416 ^ 0x1F25840F));
								while (true)
								{
									switch (num8)
									{
									case 0:
										_E016 = num7;
										num8 = 92186405 + 273506954 - 365693356;
										continue;
									case 1:
										num9 = _E001(_E00D, _E00E._E003);
										num8 = 0x2883604 ^ 0x2883606;
										continue;
									case 2:
										_E010 = num9;
										num8 = _E062._E000(~-337 >> 2);
										continue;
									case 3:
										goto end_IL_0376;
									}
									break;
								}
								continue;
								end_IL_0376:
								break;
							}
							_E000(_E22A, ref _E22A, ref _E22A);
						}
					}
					while (_E22A > (-613398660 >> 2) - -153349665);
				}
			}
			_E014 = _E00D._E009;
			return _E22A - num;
		}

		private void _E000(byte[] _E22B, ref int _E22B, ref int _E22B)
		{
			int num = Math.Min(_E016, _E22B);
			byte[] array = _E00C._E000(_E010, Math.Min(num, _E010));
			while (true)
			{
				int num2 = _E062._E000(84);
				while (true)
				{
					switch (num2)
					{
					case 0:
						_E22B -= num;
						num2 = 1;
						continue;
					case 1:
						_E016 -= num;
						num2 = 2;
						continue;
					case 2:
						while (num > array.Length)
						{
							Array.Copy(array, 0, _E22B, _E22B, array.Length);
							while (true)
							{
								int num3 = _E062._E000(84);
								while (true)
								{
									switch (num3)
									{
									case 0:
										_E22B += array.Length;
										num3 = 1;
										continue;
									case 1:
										num -= array.Length;
										num3 = 2;
										continue;
									case 2:
										goto end_IL_0085;
									}
									break;
								}
								continue;
								end_IL_0085:
								break;
							}
							_E00C._E000(array, 0, array.Length);
						}
						Array.Copy(array, 0, _E22B, _E22B, num);
						_E22B += num;
						_E00C._E000(array, 0, num);
						return;
					}
					break;
				}
			}
		}

		public bool _E000(int _E22C)
		{
			byte[] array = new byte[1024];
			int num;
			while (_E22C > 0 && (num = _E000(array, 0, Math.Min(1024, _E22C))) > 0)
			{
				_E22C -= num;
			}
			return _E22C <= 0;
		}

		public void _E000()
		{
			byte[] array = new byte[1024];
			while (_E000(array, 0, 1024) > 0)
			{
			}
		}

		private static int _E000(_E001 _E22D, _E060 _E22D)
		{
			while (_E22D != null && ((!_E22D._E004) ? true : false))
			{
				_E22D = ((_E22D._E000(1) > 0) ? _E22D._E007 : _E22D._E006);
			}
			return _E22D._E005;
		}

		private static int _E000(_E001 _E22E, int _E22E)
		{
			_E05D._E000(_E22E, out var _E21B, out var _E21B2);
			if (_E21B2 > 0)
			{
				return _E21B + _E22E._E000(_E21B2);
			}
			return _E21B;
		}

		private static int _E001(_E001 _E22F, _E060 _E22F)
		{
			int num = _E000(_E22F, _E22F);
			int num3 = default(int);
			int num5 = default(int);
			int num4 = default(int);
			while (true)
			{
				int num2 = _E062._E000(84);
				while (true)
				{
					switch (num2)
					{
					case 0:
						num3 = _E05D._E006[num];
						num2 = 1;
						continue;
					case 1:
						num5 = _E05D._E007[num];
						num2 = 3;
						continue;
					case 2:
						num4 = _E22F._E000(num5);
						num2 = 4;
						continue;
					case 3:
						if (num5 > 0)
						{
							num2 = _E062._E000(83);
							continue;
						}
						return num3;
					case 4:
						return num3 + num4;
					}
					break;
				}
			}
		}

		private void _E000(_E001 _E230, out _E05F[] _E230, out _E05F[] _E230)
		{
			int num = _E230._E000(5) + 257;
			int[] array2 = default(int[]);
			int num3 = default(int);
			int num5 = default(int);
			int[] array = default(int[]);
			int num4 = default(int);
			int num7 = default(int);
			int[] array3 = default(int[]);
			int num9 = default(int);
			while (true)
			{
				int num2 = _E062._E000(83);
				while (true)
				{
					switch (num2)
					{
					case 0:
						array2 = new int[19];
						num2 = 1;
						continue;
					case 1:
						num3 = 0;
						num2 = 3;
						continue;
					case 2:
						num5 = _E230._E000(5) + 1;
						num2 = 6;
						continue;
					case 3:
						if (num3 != 0)
						{
							num2 = 5;
							continue;
						}
						goto IL_008a;
					case 4:
						array = _E05D._E003;
						num2 = 0;
						continue;
					case 5:
						array2[array[num3]] = _E230._E000(3);
						num2 = 7;
						continue;
					case 6:
						num4 = _E230._E000(4) + 4;
						num2 = 4;
						continue;
					case 7:
						{
							num3++;
							goto IL_008a;
						}
						IL_008a:
						if (num3 >= num4)
						{
							_E060 obj = _E05D._E000(_E05D._E000(array2));
							while (true)
							{
								int num6 = _E062._E000(77);
								while (true)
								{
									switch (num6)
									{
									case 0:
										if (num7 != 0)
										{
											num6 = 2;
											continue;
										}
										goto IL_011d;
									case 1:
										_E230 = new _E05F[num];
										num6 = 4;
										continue;
									case 2:
										_E230[num7]._E003 = array3[num7];
										num6 = _E062._E000(82);
										continue;
									case 3:
										array3 = _E000(_E230, obj, num + num5);
										num6 = 1;
										continue;
									case 4:
										num7 = 0;
										num6 = 0;
										continue;
									case 5:
										{
											num7++;
											goto IL_011d;
										}
										IL_011d:
										if (num7 >= num)
										{
											_E05D._E000(_E230);
											while (true)
											{
												int num8 = _E062._E000(84);
												while (true)
												{
													switch (num8)
													{
													case 0:
														_E230 = new _E05F[num5];
														num8 = 4;
														continue;
													case 1:
														if (num9 != 0)
														{
															num8 = _E062._E000(77);
															continue;
														}
														goto case 5;
													case 2:
														num9++;
														num8 = 5;
														continue;
													case 3:
														_E230[num9]._E003 = array3[num9 + num];
														num8 = 2;
														continue;
													case 4:
														num9 = 0;
														num8 = 1;
														continue;
													case 5:
														if (num9 < num5)
														{
															goto case 3;
														}
														_E05D._E000(_E230);
														return;
													}
													break;
												}
											}
										}
										goto case 2;
									}
									break;
								}
							}
						}
						goto case 5;
					}
					break;
				}
			}
		}

		private static int[] _E000(_E001 _E231, _E060 _E231, int _E231)
		{
			int[] array = new int[_E231];
			int num = 0;
			if (num != 0)
			{
				goto IL_0017;
			}
			goto IL_0137;
			IL_0137:
			if (num < _E231)
			{
				goto IL_0017;
			}
			return array;
			IL_0017:
			int num2 = _E000(_E231, _E231);
			int num6 = default(int);
			int num8 = default(int);
			while (true)
			{
				int num3 = _E062._E000(84);
				while (true)
				{
					switch (num3)
					{
					case 0:
						goto IL_003f;
					case 1:
						array[num] = num2;
						num3 = 2;
						continue;
					case 2:
						goto IL_012b;
					}
					break;
					IL_003f:
					if (num2 < 16)
					{
						num3 = 1;
						continue;
					}
					if (num2 == 16)
					{
						int num4 = _E231._E000(2) + 3;
						while (true)
						{
							int num5 = _E062._E000(84);
							while (true)
							{
								switch (num5)
								{
								case 0:
									num6 = 0;
									num5 = 1;
									continue;
								case 1:
									if (num6 != 0)
									{
										num5 = _E062._E000(83);
										continue;
									}
									goto IL_00ba;
								case 2:
									array[num + num6] = array[num - 1];
									num5 = 3;
									continue;
								case 3:
									{
										num6++;
										goto IL_00ba;
									}
									IL_00ba:
									if (num6 >= num4)
									{
										goto end_IL_0082;
									}
									goto case 2;
								}
								break;
							}
							continue;
							end_IL_0082:
							break;
						}
						num += num4 - 1;
					}
					else if (num2 == 17)
					{
						while (true)
						{
							int num7 = _E062._E000(80);
							while (true)
							{
								switch (num7)
								{
								case 0:
									num += num8 - 1;
									num7 = 2;
									continue;
								case 1:
									num8 = _E231._E000(3) + 3;
									num7 = 0;
									continue;
								case 2:
									goto end_IL_00e1;
								}
								break;
							}
							continue;
							end_IL_00e1:
							break;
						}
					}
					else if (num2 == 18)
					{
						int num9 = _E231._E000(7) + 11;
						num += num9 - 1;
					}
					goto IL_012b;
					IL_012b:
					num++;
					goto end_IL_0034;
				}
				continue;
				end_IL_0034:
				break;
			}
			goto IL_0137;
		}
	}

	private sealed class _E001
	{
		private uint _E005;

		private int _E006;

		private int _E007;

		private Stream _E008;

		internal long _E009;

		internal _E001(Stream _E232)
		{
			_E008 = _E232;
		}

		internal int _E000(int _E233)
		{
			_E009 += _E233;
			for (int num = _E233 - (_E007 - _E006); num > 0; num -= 8)
			{
				_E005 |= checked((uint)_E008.ReadByte()) << _E007;
				_E007 += 8;
			}
			int result = (int)(_E005 >> _E006) & ((1 << _E233) - 1);
			_E006 += _E233;
			if (_E007 == _E006)
			{
				_E007 = (_E006 = 0);
				_E005 = 0u;
				return result;
			}
			if (_E006 >= 8)
			{
				_E005 >>= _E006;
				_E007 -= _E006;
				_E006 = 0;
			}
			return result;
		}

		internal void _E000()
		{
			if (_E007 != _E006)
			{
				_E009 += _E007 - _E006;
			}
			_E007 = (_E006 = 0);
			_E005 = 0u;
		}

		internal void _E000(byte[] _E234, int _E234, int _E234)
		{
			int num = _E008.Read(_E234, _E234, _E234);
			_E009 += num << 3;
		}
	}

	private sealed class _E002
	{
		private byte[] _E004;

		private int _E005;

		internal int _E006;

		internal long _E007;

		internal _E002(int _E235)
		{
			_E006 = _E235;
			_E004 = new byte[_E235];
		}

		internal void _E000(byte _E236)
		{
			_E004[_E005++] = _E236;
			if (_E005 >= _E006)
			{
				_E005 = 0;
			}
			_E007++;
		}

		internal void _E000(byte[] _E237, int _E237, int _E237)
		{
			_E007 += _E237;
			if (_E237 >= _E006)
			{
				Array.Copy(_E237, _E237, _E004, 0, _E006);
				_E005 = 0;
				return;
			}
			if (_E005 + _E237 > _E006)
			{
				int num = _E006 - _E005;
				int num3 = default(int);
				while (true)
				{
					int num2 = _E062._E000(83);
					while (true)
					{
						switch (num2)
						{
						case 0:
							_E005 = num3;
							num2 = 4;
							continue;
						case 1:
							Array.Copy(_E237, _E237, _E004, _E005, num);
							num2 = _E062._E000(77);
							continue;
						case 2:
							num3 = _E005 + _E237 - _E006;
							num2 = 1;
							continue;
						case 3:
							Array.Copy(_E237, _E237 + num, _E004, 0, num3);
							num2 = 0;
							continue;
						case 4:
							return;
						}
						break;
					}
				}
			}
			Array.Copy(_E237, _E237, _E004, _E005, _E237);
			while (true)
			{
				int num4 = _E062._E000(84);
				while (true)
				{
					switch (num4)
					{
					case 0:
						_E005 += _E237;
						num4 = 1;
						continue;
					case 1:
						if (_E005 != _E006)
						{
							return;
						}
						num4 = 2;
						continue;
					case 2:
						_E005 = 0;
						return;
					}
					break;
				}
			}
		}

		internal byte[] _E000(int _E238, int _E238)
		{
			byte[] array = new byte[_E238];
			if (_E005 >= _E238)
			{
				Array.Copy(_E004, _E005 - _E238, array, 0, _E238);
			}
			else
			{
				int num = _E238 - _E005;
				while (true)
				{
					int num2 = _E062._E000(83);
					while (true)
					{
						switch (num2)
						{
						case 0:
							Array.Copy(_E004, _E006 - num, array, 0, num);
							num2 = 1;
							continue;
						case 1:
							Array.Copy(_E004, 0, array, num, _E238 - num);
							num2 = _E062._E000(77);
							continue;
						case 2:
							goto IL_0098;
						case 3:
							goto end_IL_0040;
						}
						break;
						IL_0098:
						if (num < _E238)
						{
							num2 = 0;
							continue;
						}
						Array.Copy(_E004, _E006 - num, array, 0, _E238);
						goto end_IL_0040;
					}
					continue;
					end_IL_0040:
					break;
				}
			}
			return array;
		}
	}

	public static void _E000(Stream _E226, Stream _E226)
	{
		byte[] array = new byte[4096];
		_E000 obj = new _E000(_E226);
		while (true)
		{
			int num = obj._E000(array, 0, array.Length);
			while (true)
			{
				IL_0024:
				int num2 = _E000(84);
				while (true)
				{
					switch (num2)
					{
					case 2:
						break;
					default:
						goto IL_0024;
					case 0:
						if (num <= 0)
						{
							return;
						}
						num2 = 1;
						continue;
					case 1:
						_E226.Write(array, 0, num);
						num2 = 2;
						continue;
					}
					break;
				}
				break;
			}
		}
	}

	internal static int _E000(int _E227)
	{
		return (_E227 - (0x2588FDCF ^ 0x2588FD83)) switch
		{
			8 => ~(297834924 + -297834956 >> 5), 
			7 => ~(-(~-4)), 
			2 => ~191236749 + 191236754, 
			1 => -(~45847611) + -45847609, 
			5 => (0x3201D7D ^ 0x4F467CD) + -131365537, 
			4 => -(-144348137 ^ 0x89A93E8), 
			0 => -458050036 - -299606329 + 158443716, 
			9 => -68433201 + 68433212, 
			6 => ~-161 >> 5, 
			10 => 629709629 + -629709623, 
			12 => -658143807 ^ -658143785, 
			15 => ~(~(272 >> 4)), 
			_ => -245443039 - -489086289 + -243643251, 
		};
	}
}
