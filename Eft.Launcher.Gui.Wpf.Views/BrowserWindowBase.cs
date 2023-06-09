using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Bsg.Launcher.Utils;
using CefSharp;
using CefSharp.Wpf;
using Eft.Launcher.Gui.Wpf.CefEngine;
using Eft.Launcher.Gui.Wpf.ViewModels;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Eft.Launcher.Gui.Wpf.Views;

public class BrowserWindowBase : Window, IWindowDelegate
{
	private class _E000
	{
		[CompilerGenerated]
		private readonly string m__E000;

		[CompilerGenerated]
		private readonly string m__E001;

		public string _E000
		{
			[CompilerGenerated]
			get
			{
				return this.m__E000;
			}
		}

		public string _E001
		{
			[CompilerGenerated]
			get
			{
				return this.m__E001;
			}
		}

		public _E000(string code, string logAs)
		{
			this.m__E000 = code;
			this.m__E001 = logAs;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public string _E000;

		public bool _E001;

		public string _E002;

		public bool _E003;

		public KeyValuePair<string, string>[] _E004;

		public BrowserWindowBase _E005;

		public string[] _E006;

		internal void _E000()
		{
			using CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
			{
				Title = this._E000,
				IsFolderPicker = _E001,
				InitialDirectory = _E002,
				RestoreDirectory = false,
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = _E003,
				ShowPlacesList = true
			};
			if (_E004 != null)
			{
				KeyValuePair<string, string>[] array = _E004;
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<string, string> keyValuePair = array[i];
					commonOpenFileDialog.Filters.Add(new CommonFileDialogFilter(keyValuePair.Key, keyValuePair.Value));
				}
			}
			if (commonOpenFileDialog.ShowDialog(_E005) == CommonFileDialogResult.Ok)
			{
				_E006 = commonOpenFileDialog.FileNames.ToArray();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public BrowserWindowBase _E000;

		public int? _E001;

		public int? _E002;

		public Action _E003;

		internal void _E000()
		{
			this._E000.Dispatcher.BeginInvoke((Action)delegate
			{
				if (this._E001.HasValue)
				{
					this._E000.Width = this._E001.Value;
				}
				if (_E002.HasValue)
				{
					this._E000.Height = _E002.Value;
				}
			}, DispatcherPriority.Background);
		}

		internal void _E001()
		{
			if (this._E001.HasValue)
			{
				this._E000.Width = this._E001.Value;
			}
			if (_E002.HasValue)
			{
				this._E000.Height = _E002.Value;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public ManualResetEventSlim _E000;

		internal void _E000(object sender, EventArgs eArgs)
		{
			this._E000.Set();
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public BrowserWindowBase _E000;

		public WindowState _E001;

		internal void _E000()
		{
			this._E000.WindowState = _E001;
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public bool _E000;

		public BrowserWindowBase _E001;

		internal void _E000()
		{
			RequestRestartDialogWindow requestRestartDialogWindow = new RequestRestartDialogWindow(_E001.m__E000)
			{
				Owner = _E001
			};
			this._E000 = requestRestartDialogWindow.ShowDialog() == true;
		}
	}

	private const double _E00E = 7.0;

	[CompilerGenerated]
	private EventHandler _E00F;

	[CompilerGenerated]
	private EventHandler _E010;

	[CompilerGenerated]
	private bool _E011;

	[CompilerGenerated]
	private bool _E012;

	protected bool DoubleClickSwitchingEnabled;

	private readonly IServiceProvider m__E000;

	private readonly ILogger _E013;

	private readonly ISettingsService _settingsService;

	private readonly ILauncherUpdateService _launcherUpdateService;

	private readonly ExceptionAdapter _E014;

	private readonly ManualResetEventSlim _E015 = new ManualResetEventSlim(initialState: false);

	private readonly ManualResetEventSlim _E016 = new ManualResetEventSlim(initialState: false);

	private readonly CancellationTokenSource _E017 = new CancellationTokenSource();

	private bool _E018;

	private ChromiumWebBrowser _E019;

	private UIElement _E01A;

	private Region _E01B;

	private ProgressWindow m__E002;

	public bool IsClosed
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		private set
		{
			_E011 = value;
		}
	}

	public bool IsBrowserReady => _E015.IsSet;

	public bool IsInDragMoving
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		private set
		{
			_E012 = value;
		}
	}

	public event EventHandler OnClosingWindow
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E00F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E00F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E00F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E00F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler OnDragMoveStop
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E010;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E010, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E010;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E010, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public BrowserWindowBase(IServiceProvider serviceProvider)
	{
		try
		{
			Thread.CurrentThread.Name = GetType().Name;
		}
		catch
		{
		}
		base.KeyDown += OnKeyDown;
		this.m__E000 = serviceProvider;
		_E013 = serviceProvider.GetRequiredService<ILogger<BrowserWindowBase>>();
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_launcherUpdateService = serviceProvider.GetRequiredService<ILauncherUpdateService>();
		_E014 = serviceProvider.GetRequiredService<ExceptionAdapter>();
		base.MouseLeftButtonDown += _E001;
		this._E000();
	}

	private void _E000()
	{
		_launcherUpdateService.OnStateChanged += OnLauncherUpdateStateChanged;
		_launcherUpdateService.OnProgress += _E000;
	}

	private void _E001()
	{
		_launcherUpdateService.OnStateChanged -= OnLauncherUpdateStateChanged;
		_launcherUpdateService.OnProgress -= _E000;
	}

	protected void Initialize(ChromiumWebBrowser browser, UIElement loadingAnimation)
	{
		if (!_E018)
		{
			_E019 = browser;
			_E018 = true;
			_E01A = loadingAnimation;
			_E019.AllowDrop = false;
			_E019.DragHandler = new DragHandler(delegate(Region r)
			{
				_E01B = r;
			});
			_E019.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
			_E019.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
			_E019.JavascriptObjectRepository.Register(_E05B._E000(28333), base.DataContext, isAsync: true);
			_E019.FrameLoadEnd += _E000;
			_E019.FrameLoadStart += _E000;
			_E019.LifeSpanHandler = this.m__E000.GetRequiredService<ILifeSpanHandler>();
			_E019.MenuHandler = this.m__E000.GetRequiredService<IContextMenuHandler>();
			_E019.RequestHandler = this.m__E000.GetRequiredService<IRequestHandler>();
			_E019.DisplayHandler = this.m__E000.GetRequiredService<IDisplayHandler>();
			_E019.IsBrowserInitializedChanged += _E000;
			_E019.PreviewMouseLeftButtonDown += _E000;
			_E019.LoadError += _E000;
		}
	}

	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);
		_E013.LogInformation(_E05B._E000(28340), GetType().Name);
	}

	private async void _E000(object sender, DependencyPropertyChangedEventArgs e)
	{
		_E019.IsBrowserInitializedChanged -= _E000;
		try
		{
			_E016.Set();
			await OnBrowserInitialized();
		}
		catch (OperationCanceledException)
		{
			_E013.LogWarning(_E05B._E000(27731), GetType().Name);
			Close();
		}
		catch (Exception ex2)
		{
			_E013.LogError(ex2, _E05B._E000(27778));
			if (!(this is ErrorWindow) && !(this is DialogWindow))
			{
				await this.m__E000.GetRequiredService<IDialogService>().ShowException(ex2);
			}
			Close();
		}
	}

	private void _E000(object sender, LoadErrorEventArgs e)
	{
		_E013.LogInformation(_E05B._E000(28303), e.ErrorCode, e.FailedUrl);
	}

	protected virtual async Task OnBrowserInitialized()
	{
		if (base.DataContext is WindowViewModelBase windowViewModelBase)
		{
			await windowViewModelBase._E001(this);
		}
	}

	private void _E000([CallerMemberName] string invoker = null)
	{
		if (!_E018)
		{
			throw new InvalidOperationException(_E05B._E000(28353) + invoker + _E05B._E000(27948));
		}
	}

	public async Task LoadAsync(string url)
	{
		_E016.Wait(_E017.Token);
		if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var result))
		{
			_E019.Load(result.IsAbsoluteUri ? result.ToString() : new Uri(_settingsService.GuiUri, url).ToString());
		}
		else
		{
			_E019.Load(url);
		}
		await _E015.WaitHandle.WaitOneAsync(-1, _E017.Token);
	}

	protected void ExecuteJavaScript(string jsCode, string logAs = null)
	{
		_E000(_E05B._E000(27954));
		try
		{
			if (logAs != string.Empty)
			{
				_E013.LogDebug(_E05B._E000(27904), GetType().Name, logAs ?? jsCode);
			}
			if (_E015.IsSet)
			{
				_E019.ExecuteScriptAsync(jsCode);
			}
			else
			{
				_E019.ExecuteScriptAsyncWhenPageLoaded(jsCode);
			}
		}
		catch (ObjectDisposedException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception innerException)
		{
			throw new Exception(_E05B._E000(28003) + GetType().Name + _E05B._E000(27969) + jsCode + _E05B._E000(27975), innerException);
		}
	}

	protected async Task<JavascriptResponse> EvaluateJavaScript(string jsFunc, params string[] args)
	{
		_E000(_E05B._E000(27804));
		await _E015.WaitHandle.WaitOneAsync(-1, _E017.Token);
		_E013.LogDebug(_E05B._E000(27889), GetType().Name, jsFunc, string.Join(_E05B._E000(27867), args));
		ChromiumWebBrowser browser = _E019;
		object[] args2 = args ?? new string[0];
		return await browser.EvaluateScriptAsync(jsFunc, args2);
	}

	private void _E000(object sender, FrameLoadStartEventArgs e)
	{
		if (e.Frame.IsMain)
		{
			_E013.LogDebug(_E05B._E000(27973), GetType().Name);
			_E015.Reset();
		}
		else
		{
			_E013.LogTrace(_E05B._E000(27993), GetType().Name, e.Frame.Name);
		}
	}

	private void _E000(object sender, FrameLoadEndEventArgs e)
	{
		if (e.Frame.IsMain)
		{
			_E013.LogDebug(_E05B._E000(28039), GetType().Name);
			_E015.Set();
		}
		else
		{
			_E013.LogTrace(_E05B._E000(28055), GetType().Name, e.Frame.Name);
		}
	}

	private void _E000(object sender, MouseButtonEventArgs e)
	{
		if (_E01B == null)
		{
			return;
		}
		System.Windows.Point position = e.GetPosition(_E019);
		if (!_E01B.IsVisible((float)position.X, (float)position.Y))
		{
			return;
		}
		if (DoubleClickSwitchingEnabled && base.Tag != null && position == (System.Windows.Point)base.Tag)
		{
			base.Tag = null;
			if (base.WindowState == WindowState.Maximized)
			{
				base.WindowState = WindowState.Normal;
			}
			else
			{
				base.WindowState = WindowState.Maximized;
			}
		}
		else
		{
			base.Tag = position;
			if (base.WindowState != WindowState.Maximized)
			{
				IsInDragMoving = true;
				DragMove();
				IsInDragMoving = false;
				_E010?.Invoke(this, EventArgs.Empty);
			}
		}
		e.Handled = true;
	}

	public string[] OpenFileDialog(string title, bool multiselect, bool isFolderPicker, string initialDir = null, KeyValuePair<string, string>[] filters = null)
	{
		string[] result = new string[0];
		base.Dispatcher.Invoke(delegate
		{
			using CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
			{
				Title = title,
				IsFolderPicker = isFolderPicker,
				InitialDirectory = initialDir,
				RestoreDirectory = false,
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = multiselect,
				ShowPlacesList = true
			};
			if (filters != null)
			{
				KeyValuePair<string, string>[] array = filters;
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<string, string> keyValuePair = array[i];
					commonOpenFileDialog.Filters.Add(new CommonFileDialogFilter(keyValuePair.Key, keyValuePair.Value));
				}
			}
			if (commonOpenFileDialog.ShowDialog(this) == CommonFileDialogResult.Ok)
			{
				result = commonOpenFileDialog.FileNames.ToArray();
			}
		});
		return result;
	}

	public void NotifyAboutSettingsUpdate(string settingsJson)
	{
		ExecuteJavaScript(_E05B._E000(28153) + settingsJson + _E05B._E000(24948), _E05B._E000(28110));
	}

	public void SetNetworkAvailability(bool isAvailable)
	{
		ExecuteJavaScript(_E05B._E000(27684) + isAvailable.ToString().ToLowerInvariant() + _E05B._E000(24948));
	}

	public void ShowLoader()
	{
		ExecuteJavaScript(_E05B._E000(27708), string.Empty);
	}

	public void ShowError(Exception exc)
	{
		ExecuteJavaScript(_E05B._E000(25046) + _E014.SerializeForUi(exc) + _E05B._E000(24948));
	}

	public void HideLoader()
	{
		ExecuteJavaScript(_E05B._E000(27663), string.Empty);
	}

	public void Alert(string message)
	{
		ExecuteJavaScript(_E05B._E000(27674) + message + _E05B._E000(27746));
	}

	public void SetSize(int? width, int? height)
	{
		Action action = delegate
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				if (width.HasValue)
				{
					base.Width = width.Value;
				}
				if (height.HasValue)
				{
					base.Height = height.Value;
				}
			}, DispatcherPriority.Background);
		};
		if (!IsInDragMoving)
		{
			action();
			return;
		}
		ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(initialState: false);
		EventHandler value = delegate
		{
			manualResetEventSlim.Set();
		};
		OnDragMoveStop += value;
		manualResetEventSlim.Wait();
		action();
	}

	private void _E002()
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27750), base.WindowState));
	}

	public void DisplayBrowser()
	{
		base.Dispatcher.Invoke(delegate
		{
			_E019.Visibility = Visibility.Visible;
			_E01A.Visibility = Visibility.Hidden;
			base.MouseLeftButtonDown -= _E001;
		});
	}

	public void SetWindowState(WindowState newWindowState)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			base.WindowState = newWindowState;
		});
	}

	public void RestoreWindow()
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			if (base.Visibility == Visibility.Hidden)
			{
				Show();
			}
			base.WindowState = WindowState.Normal;
			Activate();
		});
	}

	public new void Close()
	{
		try
		{
			_E017.Cancel();
			_E017.Dispose();
		}
		catch (ObjectDisposedException)
		{
		}
		base.Dispatcher.Invoke(base.Close);
	}

	private void _E001(object sender, MouseButtonEventArgs e)
	{
		DragMove();
	}

	protected override void OnStateChanged(EventArgs e)
	{
		base.BorderThickness = new Thickness((base.WindowState == WindowState.Maximized) ? 7.0 : 0.0);
		_E002();
		base.OnStateChanged(e);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		_E001();
		_E00F?.Invoke(this, EventArgs.Empty);
		base.OnClosing(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		IsClosed = true;
		_E013.LogInformation(_E05B._E000(27715), GetType().Name);
		base.OnClosed(e);
	}

	protected virtual void OnKeyDown(object sender, KeyEventArgs e)
	{
	}

	public async Task<bool> RequestRestartForUpdate()
	{
		return await Task.Run(async delegate
		{
			bool result = false;
			await base.Dispatcher.BeginInvoke((Action)delegate
			{
				RequestRestartDialogWindow requestRestartDialogWindow = new RequestRestartDialogWindow(this.m__E000)
				{
					Owner = this
				};
				result = requestRestartDialogWindow.ShowDialog() == true;
			});
			return result;
		});
	}

	private void OnLauncherUpdateStateChanged(object sender, LauncherUpdateServiceState newState)
	{
		if (base.Dispatcher.Invoke(() => this != Application.Current.MainWindow))
		{
			return;
		}
		if (newState == LauncherUpdateServiceState.DownloadingLauncherUpdate)
		{
			this.m__E002 = base.Dispatcher.Invoke(delegate
			{
				ProgressWindow progressWindow = new ProgressWindow(this.m__E000);
				progressWindow.Owner = this;
				progressWindow.Closing += _E000;
				progressWindow.Show();
				return progressWindow;
			});
			this.m__E002.SetMessage(ProgressWindowMessage.DownloadingLauncher);
		}
		else
		{
			if (this.m__E002 == null)
			{
				return;
			}
			base.Dispatcher.Invoke(delegate
			{
				this.m__E002.Closing -= _E000;
				if (!this.m__E002.IsClosed)
				{
					this.m__E002.Close();
				}
				this.m__E002 = null;
			});
		}
	}

	private void _E000(object sender, CancelEventArgs e)
	{
		Close();
	}

	private void _E000(object sender, IProgressReport newProgress)
	{
		if (this.m__E002 != null && !this.m__E002.IsClosed)
		{
			int progress = (int)(newProgress.BytesTransferred * 100.0 / newProgress.FileSize);
			this.m__E002.SetProgress(progress);
		}
	}

	[CompilerGenerated]
	private void _E000(Region r)
	{
		_E01B = r;
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E019.Visibility = Visibility.Visible;
		_E01A.Visibility = Visibility.Hidden;
		base.MouseLeftButtonDown -= _E001;
	}

	[CompilerGenerated]
	private void _E004()
	{
		if (base.Visibility == Visibility.Hidden)
		{
			Show();
		}
		base.WindowState = WindowState.Normal;
		Activate();
	}

	[CompilerGenerated]
	private async Task<bool> _E000()
	{
		bool result = false;
		await base.Dispatcher.BeginInvoke((Action)delegate
		{
			RequestRestartDialogWindow requestRestartDialogWindow = new RequestRestartDialogWindow(this.m__E000)
			{
				Owner = this
			};
			result = requestRestartDialogWindow.ShowDialog() == true;
		});
		return result;
	}

	[CompilerGenerated]
	private bool _E000()
	{
		return this != Application.Current.MainWindow;
	}

	[CompilerGenerated]
	private ProgressWindow _E000()
	{
		ProgressWindow progressWindow = new ProgressWindow(this.m__E000);
		progressWindow.Owner = this;
		progressWindow.Closing += _E000;
		progressWindow.Show();
		return progressWindow;
	}

	[CompilerGenerated]
	private void _E005()
	{
		this.m__E002.Closing -= _E000;
		if (!this.m__E002.IsClosed)
		{
			this.m__E002.Close();
		}
		this.m__E002 = null;
	}
}
