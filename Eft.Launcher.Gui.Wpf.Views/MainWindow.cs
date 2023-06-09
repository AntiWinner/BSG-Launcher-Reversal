using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Shell;
using CefSharp.Wpf;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

public class MainWindow : BrowserWindowBase, IMainWindowDelegate, IWindowDelegate, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MainWindow _E000;

		public TaskbarItemProgressState _E001;

		internal TaskbarItemProgressState _E000()
		{
			return this._E000.TaskbarItemInfo.ProgressState = _E001;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public long _E000;

		public long _E001;

		public MainWindow _E002;

		internal void _E000()
		{
			double num = ((this._E000 == 0L) ? 0.0 : ((double)_E001 / (double)this._E000));
			if (_E002.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.None)
			{
				return;
			}
			if (num < 0.001)
			{
				if (_E002.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Normal)
				{
					_E002.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
				}
				return;
			}
			if (_E002.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Indeterminate)
			{
				_E002.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
			}
			_E002.TaskbarItemInfo.ProgressValue = num;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public int _E000;

		public MainWindow _E001;

		internal void _E000()
		{
			if (this._E000 > 0)
			{
				if (_E001.TaskbarItemInfo.ProgressState != TaskbarItemProgressState.Error)
				{
					_E001.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
				}
			}
			else if (_E001.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Error)
			{
				_E001.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
			}
		}
	}

	[CompilerGenerated]
	private readonly bool _E01C;

	private readonly IDialogService _E01D;

	private readonly JsonSerializer _E00D;

	private readonly NotifyIcon _E01E;

	private readonly object _E01F = new object();

	private AverageValueCalculator _E020;

	private int _E021;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public bool StartedFromLoginPage
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
	}

	public MainWindow(IServiceProvider serviceProvider, bool startedFromLoginPage = false)
		: base(serviceProvider)
	{
		_E01C = startedFromLoginPage;
		DoubleClickSwitchingEnabled = true;
		_E01D = serviceProvider.GetRequiredService<IDialogService>();
		_E00D = serviceProvider.GetRequiredService<JsonSerializer>();
		AppConfig requiredService = serviceProvider.GetRequiredService<AppConfig>();
		_E01D.WhenAnExceptionIsDisplayed += _E000;
		base.TaskbarItemInfo = new TaskbarItemInfo();
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
		_E01E = new NotifyIcon
		{
			Visible = true,
			Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
			Text = requiredService.AppShortName
		};
		_E01E.Click += _E000;
		base.Title = this._E000();
	}

	private string _E000()
	{
		return ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), inherit: false))?.Title;
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);
		(PresentationSource.FromVisual(this) as HwndSource).AddHook(_E000);
	}

	[DebuggerStepThrough]
	private IntPtr _E000(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == _E009._E001)
		{
			RestoreWindow();
		}
		return IntPtr.Zero;
	}

	private void _E000(object sender, EventArgs e)
	{
		RestoreWindow();
	}

	public void SetGameUpdateState(GameUpdateServiceState newState)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27593), newState));
	}

	public void SetTaskbarState(TaskbarItemProgressState taskbarState)
	{
		base.Dispatcher.Invoke(() => base.TaskbarItemInfo.ProgressState = taskbarState);
	}

	public void SetGameUpdateProgress(long bytesDownloaded, long totalBytesToDownload)
	{
		int secondsLeft;
		int currentSpeed;
		lock (_E01F)
		{
			if (_E020 != null && _E020.FileSize != (double)totalBytesToDownload)
			{
				_E020.Dispose();
				_E020 = null;
			}
			if (_E020 == null)
			{
				_E020 = new AverageValueCalculator(totalBytesToDownload, bytesDownloaded);
			}
			else
			{
				_E020.SetProgress(bytesDownloaded);
			}
			secondsLeft = _E020.SecondsLeft;
			currentSpeed = _E020.CurrentSpeed;
		}
		ExecuteJavaScript(string.Format(_E05B._E000(27175), bytesDownloaded, totalBytesToDownload, secondsLeft, currentSpeed), string.Empty);
		base.Dispatcher.Invoke(delegate
		{
			double num = ((totalBytesToDownload == 0L) ? 0.0 : ((double)bytesDownloaded / (double)totalBytesToDownload));
			if (base.TaskbarItemInfo.ProgressState != 0)
			{
				if (num < 0.001)
				{
					if (base.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Normal)
					{
						base.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
					}
				}
				else
				{
					if (base.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Indeterminate)
					{
						base.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
					}
					base.TaskbarItemInfo.ProgressValue = num;
				}
			}
		});
	}

	public void SetGameUpdateVersion(BsgVersion version)
	{
		ExecuteJavaScript(_E05B._E000(27144) + ((version == default(BsgVersion)) ? "" : version.ToString()) + _E05B._E000(27233));
	}

	public void SetGameEdition(string edition)
	{
		ExecuteJavaScript(_E05B._E000(27238) + edition + _E05B._E000(27233));
	}

	public void SetGameVersion(BsgVersion version)
	{
		ExecuteJavaScript(_E05B._E000(27253) + ((version == default(BsgVersion)) ? "" : version.ToString()) + _E05B._E000(27233));
	}

	public void SetGameRegion(string region)
	{
		ExecuteJavaScript(_E05B._E000(27204) + region + _E05B._E000(27233));
	}

	public void SetGameState(GameState newState)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27220), newState));
	}

	public void SetLauncherVersion(Version version)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27304), version));
	}

	public void CollapseToTray()
	{
		System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			base.WindowState = WindowState.Minimized;
			Hide();
		});
	}

	public void SetCurrentGameServers(string ipRegion, DatacenterDto[] datacenters)
	{
		string text = JArray.FromObject(datacenters, _E00D).ToString(Formatting.None);
		ExecuteJavaScript(_E05B._E000(27270) + text + _E05B._E000(24948), _E05B._E000(27295));
	}

	public void SetQueueValues(int queuePosition, int estimatedTimeSec)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27385), queuePosition, estimatedTimeSec), string.Empty);
	}

	public void PlaySound(string soundFileName)
	{
		ExecuteJavaScript(_E05B._E000(27344) + soundFileName + _E05B._E000(27233));
	}

	public void DrawMainButtonCountdown(double seconds, bool disableMainButton)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27356), seconds, disableMainButton.ToString().ToLowerInvariant()));
	}

	private void _E000(bool newError)
	{
		int num = (newError ? Interlocked.Increment(ref _E021) : Interlocked.Decrement(ref _E021));
		base.Dispatcher.Invoke(delegate
		{
			if (num > 0)
			{
				if (base.TaskbarItemInfo.ProgressState != TaskbarItemProgressState.Error)
				{
					base.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
				}
			}
			else if (base.TaskbarItemInfo.ProgressState == TaskbarItemProgressState.Error)
			{
				base.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
			}
		});
	}

	private void _E000(Task exceptionTask)
	{
		_E000(newError: true);
		exceptionTask.ContinueWith(delegate
		{
			_E000(newError: false);
		});
	}

	protected override void OnClosed(EventArgs e)
	{
		_E01E.Visible = false;
		_E01E.Dispose();
		base.OnClosed(e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(26883), UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			Browser = (ChromiumWebBrowser)target;
			break;
		case 2:
			LoadingAnimation = (LoadingAnimation)target;
			break;
		default:
			_E003 = true;
			break;
		}
	}

	[CompilerGenerated]
	private void _E000()
	{
		base.WindowState = WindowState.Minimized;
		Hide();
	}

	[CompilerGenerated]
	private void _E001(Task t)
	{
		_E000(newError: false);
	}
}
