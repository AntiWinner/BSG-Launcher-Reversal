using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public abstract class WindowViewModelBase
{
	protected class ControlledQueueToken
	{
		[CompilerGenerated]
		private readonly string _E000;

		private Action _E001;

		private int _E002;

		public string InitiatorMethodName
		{
			[CompilerGenerated]
			get
			{
				return _E000;
			}
		}

		internal ControlledQueueToken(string initiatorMethodName, Action releaseAction)
		{
			_E000 = initiatorMethodName;
			_E001 = releaseAction;
		}

		public void Release()
		{
			if (Interlocked.Exchange(ref _E002, 1) == 0)
			{
				_E001?.Invoke();
				_E001 = null;
			}
		}

		public override string ToString()
		{
			return InitiatorMethodName;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string _E000;

		public string _E001;

		public string _E002;

		public bool? _E003;

		internal void _E000()
		{
			CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
			{
				Title = this._E000,
				IsFolderPicker = true,
				RestoreDirectory = false,
				InitialDirectory = _E001,
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				DefaultDirectory = _E001,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = false,
				ShowPlacesList = true
			};
			switch (commonOpenFileDialog.ShowDialog())
			{
			case CommonFileDialogResult.Ok:
				_E002 = commonOpenFileDialog.FileName;
				_E003 = true;
				break;
			case CommonFileDialogResult.Cancel:
				_E003 = false;
				break;
			default:
				_E003 = null;
				break;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string _E000;

		internal bool _E000(string bDir)
		{
			return this._E000.NormalizePath() == bDir.NormalizePath();
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public WindowViewModelBase _E000;

		public string _E001;

		internal async void _E000(object w)
		{
			try
			{
				this._E000._settingsService.Update(_E001);
			}
			catch (Exception ex)
			{
				this._E000.Logger.LogError(ex, _E05B._E000(29460));
				await this._E000._E002.ShowException(ex);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public WindowViewModelBase _E000;

		public string _E001;

		internal async void _E000(object w)
		{
			await this._E000._E000(new Exception(_E001));
		}
	}

	[CompilerGenerated]
	private sealed class _E00A
	{
		public WindowViewModelBase _E000;

		public int _E001;

		public string _E002;

		public IList<object> _E003;

		internal async void _E000(object w)
		{
			await this._E000._E000((Exception)new CodeException(_E001, _E002, _E003?.Cast<string>().ToArray() ?? new string[0]));
		}
	}

	[CompilerGenerated]
	private sealed class _E00B
	{
		public WindowViewModelBase _E000;

		public int _E001;

		public IList<object> _E002;

		public IJavascriptCallback _E003;

		internal async void _E000(object w)
		{
			int num = (int)(await this._E000._E000((DialogWindowMessage)_E001, _E002?.Cast<string>().ToArray() ?? new string[0]));
			if (_E003 != null && _E003.CanExecute)
			{
				await _E003.ExecuteAsync(num);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00C
	{
		public WindowViewModelBase _E000;

		public string _E001;

		public string _E002;

		public IJavascriptCallback _E003;

		public IJavascriptCallback _E004;

		internal async void _E000(object w)
		{
			try
			{
				JToken jToken = await this._E000._E007.RequestAsync(_E001, new StringContent(_E002));
				if (_E003.CanExecute)
				{
					await _E003.ExecuteAsync(jToken.ToString());
				}
			}
			catch (ApiNetworkException ex)
			{
				if (_E004?.CanExecute ?? false)
				{
					await _E004.ExecuteAsync(ex.Response.StatusCode, ex.ApiCode);
				}
			}
			catch (HttpNetworkException ex2)
			{
				if (_E004?.CanExecute ?? false)
				{
					await _E004.ExecuteAsync(ex2.Response.StatusCode);
				}
			}
			catch (NetworkException)
			{
				if (_E004?.CanExecute ?? false)
				{
					await _E004.ExecuteAsync(0);
				}
			}
			catch (Exception exc)
			{
				if (_E004?.CanExecute ?? false)
				{
					await _E004.ExecuteAsync(0);
				}
				await this._E000._E002.ShowException(exc);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00D
	{
		public WindowViewModelBase _E000;

		public bool _E001;

		public string _E002;

		internal string _E000(string f)
		{
			return this._E000.EscapeFileSystemEntryForGui(_E001 ? f : f.Replace(Path.GetDirectoryName(_E002), "").TrimStart(Path.DirectorySeparatorChar));
		}
	}

	private static readonly char[] _E010 = new char[13]
	{
		'+', '%', '#', ';', '<', '>', ':', '|', '?', '*',
		'/', '"', '\''
	};

	protected readonly ILogger Logger;

	private readonly IServiceProvider _E00F;

	private readonly ISettingsService _settingsService;

	private readonly IDialogService _E002;

	private readonly ISiteCommunicationService _E007;

	private readonly IAccessService _E011;

	private readonly ILauncherBackendService _E012;

	private readonly IGameBackendService _E006;

	private IWindowDelegate _E004;

	private ControlledQueueToken _E013;

	internal WindowViewModelBase(IServiceProvider serviceProvider)
	{
		_E00F = serviceProvider;
		Logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().FullName);
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		this._E002 = serviceProvider.GetRequiredService<IDialogService>();
		this._E007 = serviceProvider.GetRequiredService<ISiteCommunicationService>();
		_E011 = serviceProvider.GetRequiredService<IAccessService>();
		_E012 = serviceProvider.GetRequiredService<ILauncherBackendService>();
		this._E006 = serviceProvider.GetRequiredService<IGameBackendService>();
	}

	protected virtual void SubscribeServices()
	{
		_settingsService.OnSettingsUpdated += _E000;
		_settingsService.OnUserProfileLoaded += _E000;
		this._E007.OnNetworkStateChanged += _E000;
		NetworkChange.NetworkAvailabilityChanged += _E000;
		this._E004.OnClosingWindow += _E001;
	}

	protected virtual void UnsubscribeServices()
	{
		_settingsService.OnSettingsUpdated -= _E000;
		_settingsService.OnUserProfileLoaded -= _E000;
		this._E007.OnNetworkStateChanged -= _E000;
		NetworkChange.NetworkAvailabilityChanged -= _E000;
		this._E004.OnClosingWindow -= _E001;
	}

	internal virtual Task _E001(IWindowDelegate windowDelegate)
	{
		this._E004 = windowDelegate;
		SubscribeServices();
		return Task.CompletedTask;
	}

	protected async Task ExecuteSingleQueueOperation(Func<ControlledQueueToken, Task> operation, [CallerMemberName] string callerMemberName = null)
	{
		if (operation == null)
		{
			throw new ArgumentNullException(_E05B._E000(29810));
		}
		if (Interlocked.CompareExchange(ref _E013, new ControlledQueueToken(callerMemberName, delegate
		{
			_E013 = null;
		}), null) != null)
		{
			Logger.LogWarning(_E05B._E000(29816), _E013);
			return;
		}
		ControlledQueueToken controlledQueueToken = _E013;
		try
		{
			await operation(controlledQueueToken);
		}
		finally
		{
			controlledQueueToken.Release();
		}
	}

	protected bool? ShowSelectFolderDialog(ref string selectedFolder, string title)
	{
		string path = selectedFolder;
		string text = selectedFolder;
		while (!Directory.Exists(text))
		{
			DirectoryInfo parent = Directory.GetParent(text);
			text = ((parent == null) ? Path.GetPathRoot(Environment.SystemDirectory) : parent.FullName);
		}
		bool? flag = null;
		Application.Current.Dispatcher.Invoke(delegate
		{
			CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
			{
				Title = title,
				IsFolderPicker = true,
				RestoreDirectory = false,
				InitialDirectory = text,
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				DefaultDirectory = text,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = false,
				ShowPlacesList = true
			};
			switch (commonOpenFileDialog.ShowDialog())
			{
			case CommonFileDialogResult.Ok:
				path = commonOpenFileDialog.FileName;
				flag = true;
				break;
			case CommonFileDialogResult.Cancel:
				flag = false;
				break;
			default:
				flag = null;
				break;
			}
		});
		if (flag == true)
		{
			selectedFolder = path.WithLongPathPrefix();
		}
		return flag;
	}

	protected async Task<string> ShowSelectGameFolderDialog(string initialFolder)
	{
		string selectedFolder = initialFolder;
		while (ShowSelectFolderDialog(ref selectedFolder, _E05B._E000(29856)) == true)
		{
			if (await CanInstallToDirectory(selectedFolder, notifyUser: true))
			{
				return selectedFolder;
			}
		}
		return null;
	}

	protected async Task<bool> CanInstallToDirectory(string directory, bool notifyUser = false)
	{
		try
		{
			if (directory == null || directory.Length <= 3 || directory.Length > 160 || directory.Substring(Path.GetPathRoot(directory).Length).IndexOfAny(_E010) != -1 || directory.StartsWith(GetLauncherDirectory()) || (from b in _settingsService.Branches
				where !b.IsSelected && !string.IsNullOrWhiteSpace(b.GameRootDir)
				select b.GameRootDir).Any((string bDir) => directory.NormalizePath() == bDir.NormalizePath()))
			{
				if (notifyUser)
				{
					await this._E002.ShowDialog(DialogWindowMessage.CannotInstallTheGameToTheFolder, directory.WithoutLongPathPrefix());
				}
			}
			else
			{
				if (_E011.CheckPermissions(directory))
				{
					return true;
				}
				if (notifyUser)
				{
					await this._E002.ShowDialog(DialogWindowMessage.NoAccessToTheFolder, directory);
				}
			}
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29887), directory);
			throw;
		}
		return false;
	}

	protected string GetLauncherDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	protected virtual void OnClosing()
	{
		UnsubscribeServices();
	}

	protected JContainer GetFilesystemEntryAsJson(string filesystemEntry)
	{
		return new JObject
		{
			{
				_E05B._E000(30622),
				EscapeFileSystemEntryForGui(filesystemEntry)
			},
			{
				_E05B._E000(30689),
				new FileInfo(filesystemEntry).CreationTimeUtc.ToUnixTimeStamp()
			},
			{
				_E05B._E000(30711),
				new FileInfo(filesystemEntry).LastWriteTimeUtc.ToUnixTimeStamp()
			}
		};
	}

	protected async Task<UserProfile> LoadProfiles()
	{
		UserProfile userProfile = await _E012.GetUserProfile();
		_settingsService.LoadUserProfile(userProfile);
		try
		{
			PlayerProfile playerProfile = await this._E006.GetPlayerProfile();
			_settingsService.LoadPlayerProfile(playerProfile);
			return userProfile;
		}
		catch
		{
			return userProfile;
		}
	}

	protected string EscapeFileSystemEntryForGui(string filesystemEntry)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Path.IsPathRooted(filesystemEntry))
		{
			stringBuilder.Append(_settingsService.GuiUri.Scheme + _E05B._E000(30660));
		}
		stringBuilder.Append(filesystemEntry.Replace(_E05B._E000(24721), _E05B._E000(30664)).TrimStart('/'));
		if (Directory.Exists(filesystemEntry) && !filesystemEntry.EndsWith(_E05B._E000(30664)))
		{
			stringBuilder.Append(_E05B._E000(30664));
		}
		return stringBuilder.ToString();
	}

	protected string UnescapeFilesystemEntryForGui(string escapedFilesystemEntry)
	{
		return escapedFilesystemEntry.Replace(_settingsService.GuiUri.Scheme + _E05B._E000(30660), "").Replace('/', '\\');
	}

	private void _E000(Exception exception)
	{
		this._E000(exception).Wait();
	}

	private async Task _E000(Exception exception)
	{
		try
		{
			await _E00F.GetRequiredService<IDialogService>().ShowException(exception);
		}
		catch (Exception exception2)
		{
			Logger.LogError(exception2, _E05B._E000(29892), exception?.Message);
		}
	}

	private DialogResult _E000(DialogWindowMessage dialogWindowMessage, params string[] args)
	{
		return this._E000(dialogWindowMessage, args).Result;
	}

	private async Task<DialogResult> _E000(DialogWindowMessage dialogWindowMessage, params string[] args)
	{
		try
		{
			return await _E00F.GetRequiredService<IDialogService>().ShowDialog(dialogWindowMessage, args);
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29485), dialogWindowMessage);
			return DialogResult.Cancelled;
		}
	}

	private void _E000(SiteNetworkState networkState)
	{
		if (this._E004.IsBrowserReady)
		{
			if (networkState == SiteNetworkState.Busy)
			{
				this._E004.ShowLoader();
			}
			else
			{
				this._E004.HideLoader();
			}
		}
	}

	private void _E000(object sender, NetworkAvailabilityEventArgs eArgs)
	{
		this._E004.SetNetworkAvailability(eArgs.IsAvailable);
	}

	private void _E000(object sender, EventArgs e)
	{
		if (this._E004.IsBrowserReady)
		{
			this._E004.NotifyAboutSettingsUpdate(_settingsService.GetGuiSettings());
		}
	}

	private void _E001(object sender, EventArgs e)
	{
		OnClosing();
	}

	[DebuggerHidden]
	public void DisplayBrowser()
	{
		this._E004.DisplayBrowser();
	}

	[DebuggerHidden]
	public virtual void Close()
	{
		try
		{
			this._E004.Close();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30670));
		}
	}

	[DebuggerHidden]
	public void Goto(string url)
	{
		LogJsDotNetCall(url);
		try
		{
			this._E004.LoadAsync(url);
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30245), url);
		}
	}

	[DebuggerHidden]
	public void SetWindowState(string newWindowStateString)
	{
		LogJsDotNetCall(newWindowStateString);
		try
		{
			WindowState windowState = (WindowState)Enum.Parse(typeof(WindowState), newWindowStateString, ignoreCase: true);
			this._E004.SetWindowState(windowState);
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30264));
		}
	}

	[DebuggerHidden]
	public void SetSettings(string json)
	{
		LogJsDotNetCall(json);
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				_settingsService.Update(json);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(29460));
				await this._E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public string GetSettings()
	{
		LogJsDotNetCall();
		try
		{
			return _settingsService.GetGuiSettings();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30239));
			return null;
		}
	}

	[DebuggerHidden]
	public void ShowError(string message)
	{
		this._E000(new Exception(message));
	}

	[DebuggerHidden]
	public void ShowErrorAsync(string message)
	{
		ThreadPool.QueueUserWorkItem(async delegate
		{
			await this._E000(new Exception(message));
		});
	}

	[DebuggerHidden]
	public void ShowCodeError(int code, string message, IList<object> args)
	{
		this._E000((Exception)new CodeException(code, message, args?.Cast<string>().ToArray() ?? new string[0]));
	}

	[DebuggerHidden]
	public void ShowCodeErrorAsync(int code, string message, IList<object> args)
	{
		ThreadPool.QueueUserWorkItem(async delegate
		{
			await this._E000((Exception)new CodeException(code, message, args?.Cast<string>().ToArray() ?? new string[0]));
		});
	}

	[DebuggerHidden]
	public int ShowDialog(int dialogMessageCode, IList<object> args)
	{
		return (int)this._E000((DialogWindowMessage)dialogMessageCode, args?.Cast<string>().ToArray() ?? new string[0]);
	}

	[DebuggerHidden]
	public void ShowDialogAsync(int dialogMessageCode, IList<object> args, IJavascriptCallback callback)
	{
		ThreadPool.QueueUserWorkItem(async delegate
		{
			int num = (int)(await this._E000((DialogWindowMessage)dialogMessageCode, args?.Cast<string>().ToArray() ?? new string[0]));
			if (callback != null && callback.CanExecute)
			{
				await callback.ExecuteAsync(num);
			}
		});
	}

	[DebuggerHidden]
	public void ShowBugreport()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				BugReportWindow.ShowWindow(_E00F);
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, _E05B._E000(29696));
			}
		});
	}

	[DebuggerHidden]
	public string[] ShowSelectionDialog(string title, bool multiselect, bool isFolderPicker)
	{
		LogJsDotNetCall(title, multiselect, isFolderPicker);
		try
		{
			string[] array = this._E004.OpenFileDialog(title, multiselect, isFolderPicker);
			Logger.LogDebug(_E05B._E000(30327), array.Length);
			return array;
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30297));
			return new string[0];
		}
	}

	[DebuggerHidden]
	public void SiteRequest(string relativeUri, string content, IJavascriptCallback callback, IJavascriptCallback errorCallback = null)
	{
		int byteCount = Encoding.UTF8.GetByteCount(content);
		LogJsDotNetCall(relativeUri, string.Format(_E05B._E000(30340), byteCount));
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				JToken jToken = await this._E007.RequestAsync(relativeUri, new StringContent(content));
				if (callback.CanExecute)
				{
					await callback.ExecuteAsync(jToken.ToString());
				}
			}
			catch (ApiNetworkException ex)
			{
				if (errorCallback?.CanExecute ?? false)
				{
					await errorCallback.ExecuteAsync(ex.Response.StatusCode, ex.ApiCode);
				}
			}
			catch (HttpNetworkException ex2)
			{
				if (errorCallback?.CanExecute ?? false)
				{
					await errorCallback.ExecuteAsync(ex2.Response.StatusCode);
				}
			}
			catch (NetworkException)
			{
				if (errorCallback?.CanExecute ?? false)
				{
					await errorCallback.ExecuteAsync(0);
				}
			}
			catch (Exception exc)
			{
				if (errorCallback?.CanExecute ?? false)
				{
					await errorCallback.ExecuteAsync(0);
				}
				await this._E002.ShowException(exc);
			}
		});
	}

	[DebuggerHidden]
	public void RefreshNetworkAvailabilityState()
	{
		LogJsDotNetCall();
		try
		{
			this._E004?.SetNetworkAvailability(NetworkInterface.GetIsNetworkAvailable());
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30352));
		}
	}

	[DebuggerHidden]
	public void ShowDir(string dirType)
	{
		LogJsDotNetCall(dirType);
		try
		{
			if (!(dirType == _E05B._E000(30461)))
			{
				if (dirType == _E05B._E000(30400))
				{
					Directory.CreateDirectory(_settingsService.SelectedBranch.GameScreenshotsDir);
					Process.Start(_settingsService.SelectedBranch.GameScreenshotsDir);
				}
				else
				{
					Logger.LogWarning(_E05B._E000(30013), dirType);
				}
			}
			else if (Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
			{
				string text = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _E05B._E000(25496));
				Directory.CreateDirectory(text);
				Process.Start(text);
			}
			else
			{
				Logger.LogWarning(_E05B._E000(30412));
			}
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30063));
		}
	}

	[DebuggerHidden]
	public string GetGameVersion()
	{
		LogJsDotNetCall();
		try
		{
			return _E00F.GetRequiredService<IGameService>().GameVersion.ToString();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30034));
			return new Version().ToString();
		}
	}

	[DebuggerHidden]
	public string[] GetFiles(string dir, bool returnAbsolutePaths = false)
	{
		LogJsDotNetCall(_E05B._E000(30131), returnAbsolutePaths);
		try
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), _E05B._E000(30137), dir);
			if (!Directory.Exists(path))
			{
				Logger.LogWarning(_E05B._E000(30081), _settingsService.SelectedBranch.GameScreenshotsDir);
				return new string[0];
			}
			return (from f in Directory.EnumerateFiles(path)
				select EscapeFileSystemEntryForGui(returnAbsolutePaths ? f : f.Replace(Path.GetDirectoryName(path), "").TrimStart(Path.DirectorySeparatorChar))).ToArray();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(30198));
			return new string[0];
		}
	}

	[DebuggerHidden]
	public void SetSize(int? width, int? height)
	{
		LogJsDotNetCall(width, height);
		this._E004.SetSize(width, height);
	}

	[DebuggerHidden]
	public async void ReloadSettings()
	{
		LogJsDotNetCall();
		try
		{
			await LoadProfiles();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(29561));
			await this._E002.ShowException(ex);
		}
	}

	protected void LogJsDotNetCall(params object[] args)
	{
		MethodBase method = new StackFrame(1).GetMethod();
		if (method.Name == _E05B._E000(30154))
		{
			method = new StackFrame(3).GetMethod();
		}
		string text = GetType().Name.Replace(_E05B._E000(30161), "");
		Logger.LogDebug(_E05B._E000(30175), text, method.Name, string.Join(_E05B._E000(27867), args.Select((object a) => (a == null || !a.GetType().IsClass) ? a : string.Format(_E05B._E000(29877), a))));
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E013 = null;
	}

	[CompilerGenerated]
	private void _E000(object w)
	{
		try
		{
			BugReportWindow.ShowWindow(_E00F);
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29696));
		}
	}
}
