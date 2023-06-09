using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using CefSharp;
using CefSharp.BrowserSubprocess;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Shell;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf;

public class App : Application, ISingleInstanceApp, IComponentConnector
{
	private readonly _E00A m__E000;

	private bool m__E001;

	[STAThread]
	public static int Main(string[] args)
	{
		if (args.Contains(_E05B._E000(25585)))
		{
			return AccessProvider.Main(args);
		}
		if (args.Contains(_E05B._E000(25589)))
		{
			_E000();
			return 0;
		}
		if (args.Contains(_E05B._E000(25537)))
		{
			return SystemInfoProvider.Main(args);
		}
		try
		{
			Cef.EnableHighDPISupport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(_E05B._E000(25559) + ex.Message);
			return 404;
		}
		try
		{
			Cef.EnableWaitForBrowsersToClose();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(_E05B._E000(25147) + ex2.Message);
			return 404;
		}
		if (args.Any((string a) => a.Contains(_E05B._E000(25246))))
		{
			return SelfHost.Main(args);
		}
		try
		{
			Thread.CurrentThread.Name = _E05B._E000(25195);
		}
		catch
		{
		}
		bool num = SingleInstance<App>.InitializeAsFirstInstance(AppConfig.Instance.SubfolderName);
		int result = 0;
		if (num)
		{
			App app = new App();
			app.InitializeComponent();
			result = app.Run();
			SingleInstance<App>.Cleanup();
		}
		else
		{
			_E009._E000((IntPtr)65535, _E009._E001, IntPtr.Zero, IntPtr.Zero);
		}
		return result;
	}

	private static void _E000()
	{
		_E001();
	}

	private static void _E001()
	{
		try
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppConfig.Instance.AppPublisher, AppConfig.Instance.SubfolderName, AppConfig.Instance.SettingsFileName);
			if (File.Exists(path))
			{
				string path2 = JObject.Parse(File.ReadAllText(path)).Value<string>(_E05B._E000(25198));
				if (Directory.Exists(path2))
				{
					Directory.Delete(path2, recursive: true);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(_E05B._E000(25211) + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	public bool SignalExternalCommandLineArgs(IList<string> args)
	{
		return true;
	}

	public App()
	{
		this.m__E000 = new _E00A();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		this.m__E000.Exit(e.ApplicationExitCode);
		base.OnExit(e);
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		try
		{
			Bootstrapper.Run(this.m__E000, e.Args);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, _E05B._E000(25258), MessageBoxButton.OK, MessageBoxImage.Hand);
			Shutdown((ex as CodeException)?.Code ?? 1000);
		}
	}

	private void _E000(object sender, DispatcherUnhandledExceptionEventArgs eArgs)
	{
		this.m__E000._E000(sender, eArgs);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		base.DispatcherUnhandledException += _E000;
		if (!this.m__E001)
		{
			this.m__E001 = true;
			Uri resourceLocator = new Uri(_E05B._E000(25278), UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		this.m__E001 = true;
	}
}
