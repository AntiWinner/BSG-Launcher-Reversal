using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.Models;
using Bsg.Launcher.Queues;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.GameService;

public class GameService : IGameService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FileSystemWatcher _E000;

		public GameService _E001;

		internal void _003C_002Ector_003Eb__8(object sender, GameUpdateServiceState e)
		{
			_E001._E000();
			if (e == GameUpdateServiceState.Idle)
			{
				if (_E001.GameState == GameState.InstallRequired || _E001.GameState == GameState.ReadyToGame || _E001.GameState == GameState.ReinstallRequired || _E001.GameState == GameState.RepairRequired)
				{
					try
					{
						if (Directory.Exists(_E001._settingsService.SelectedBranch.GameRootDir))
						{
							_E000.Path = _E001._settingsService.SelectedBranch.GameRootDir;
							_E000.EnableRaisingEvents = true;
						}
						return;
					}
					catch
					{
						return;
					}
				}
				_E000.EnableRaisingEvents = false;
			}
			else
			{
				_E000.EnableRaisingEvents = false;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GameService _E000;

		public Process _E001;

		public Task _E002;

		internal void _E000()
		{
			do
			{
				Thread.Sleep(250);
				_E001 = Process.GetProcessesByName(this._E000._appConfig.GameShortName).FirstOrDefault();
			}
			while (this._E000._E00F && _E001 == null && !_E002.IsCompleted);
		}
	}

	[CompilerGenerated]
	private EventHandler<GameStateChangedEventArgs> m__E000;

	[CompilerGenerated]
	private EventHandler<bool> m__E001;

	[CompilerGenerated]
	private EventHandler<BsgVersion> m__E002;

	[CompilerGenerated]
	private Func<ProcessLifecycleInformation, Task> m__E003;

	[CompilerGenerated]
	private Action<Process> m__E004;

	private readonly ILogger m__E005;

	private readonly AppConfig _appConfig;

	private readonly ISettingsService _settingsService;

	private readonly IConsistencyService _consistencyService;

	private readonly IGameUpdateService _gameUpdateService;

	private readonly IAuthService _E006;

	private readonly IAccessService _E007;

	private readonly IGameBackendService _E008;

	private readonly IQueueHandler _E009;

	private readonly Utils _E00A;

	private bool? _E00B;

	private bool? _E00C;

	private bool? _E00D;

	private bool? _E00E;

	private bool _E00F;

	private bool _E010;

	private bool _E011;

	private bool _E012;

	private bool _E013;

	private GameState _E014;

	private BsgVersion _E015;

	public GameState GameState
	{
		get
		{
			return _E014;
		}
		private set
		{
			if (_E014 != value)
			{
				GameState oldState = _E014;
				_E014 = value;
				this.m__E000?.Invoke(this, new GameStateChangedEventArgs(oldState, value));
			}
		}
	}

	public BsgVersion GameVersion
	{
		get
		{
			return _E015;
		}
		private set
		{
			if (_E015 != value)
			{
				_E015 = value;
				this.m__E002?.Invoke(this, value);
			}
		}
	}

	public event EventHandler<GameStateChangedEventArgs> OnGameStateChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler<GameStateChangedEventArgs> eventHandler = this.m__E000;
			EventHandler<GameStateChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GameStateChangedEventArgs> value2 = (EventHandler<GameStateChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<GameStateChangedEventArgs> eventHandler = this.m__E000;
			EventHandler<GameStateChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GameStateChangedEventArgs> value2 = (EventHandler<GameStateChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E000, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<bool> OnGameInstalledStateChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler<bool> eventHandler = this.m__E001;
			EventHandler<bool> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<bool> value2 = (EventHandler<bool>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<bool> eventHandler = this.m__E001;
			EventHandler<bool> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<bool> value2 = (EventHandler<bool>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E001, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<BsgVersion> OnGameVersionChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler<BsgVersion> eventHandler = this.m__E002;
			EventHandler<BsgVersion> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BsgVersion> value2 = (EventHandler<BsgVersion>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<BsgVersion> eventHandler = this.m__E002;
			EventHandler<BsgVersion> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BsgVersion> value2 = (EventHandler<BsgVersion>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E002, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event Func<ProcessLifecycleInformation, Task> OnGameClosedAsync
	{
		[CompilerGenerated]
		add
		{
			Func<ProcessLifecycleInformation, Task> func = this.m__E003;
			Func<ProcessLifecycleInformation, Task> func2;
			do
			{
				func2 = func;
				Func<ProcessLifecycleInformation, Task> value2 = (Func<ProcessLifecycleInformation, Task>)Delegate.Combine(func2, value);
				func = Interlocked.CompareExchange(ref this.m__E003, value2, func2);
			}
			while ((object)func != func2);
		}
		[CompilerGenerated]
		remove
		{
			Func<ProcessLifecycleInformation, Task> func = this.m__E003;
			Func<ProcessLifecycleInformation, Task> func2;
			do
			{
				func2 = func;
				Func<ProcessLifecycleInformation, Task> value2 = (Func<ProcessLifecycleInformation, Task>)Delegate.Remove(func2, value);
				func = Interlocked.CompareExchange(ref this.m__E003, value2, func2);
			}
			while ((object)func != func2);
		}
	}

	public event Action<Process> OnGameStarted
	{
		[CompilerGenerated]
		add
		{
			Action<Process> action = this.m__E004;
			Action<Process> action2;
			do
			{
				action2 = action;
				Action<Process> value2 = (Action<Process>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Process> action = this.m__E004;
			Action<Process> action2;
			do
			{
				action2 = action;
				Action<Process> value2 = (Action<Process>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public GameService(ILogger<GameService> logger, AppConfig appConfig, ISettingsService settingsService, IConsistencyService consistencyService, IGameUpdateService gameUpdateService, IAuthService authService, IAccessService accessService, IGameBackendService gameBackendService, IQueueHandler queueHandler, Utils utils)
	{
		this.m__E005 = logger;
		_appConfig = appConfig;
		_settingsService = settingsService;
		_consistencyService = consistencyService;
		_gameUpdateService = gameUpdateService;
		_E006 = authService;
		_E007 = accessService;
		_E008 = gameBackendService;
		_E009 = queueHandler;
		_E00A = utils;
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher
		{
			Filter = _E05B._E000(9682),
			NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName)
		};
		fileSystemWatcher.Deleted += delegate
		{
			_gameUpdateService.CheckForGameUpdate(fastChecking: true);
		};
		fileSystemWatcher.Renamed += delegate
		{
			_gameUpdateService.CheckForGameUpdate(fastChecking: true);
		};
		fileSystemWatcher.Created += delegate
		{
			_gameUpdateService.CheckForGameUpdate(fastChecking: true);
		};
		_consistencyService.OnConsistencyCheckingError += delegate
		{
			_E00D = true;
			this._E000();
		};
		_consistencyService.OnConsistencyCheckingCompleted += delegate(object sender, IConsistencyCheckingResult e)
		{
			_E00D = false;
			_E010 = false;
			_E00C = e.IsSuccess;
			this._E000();
		};
		_consistencyService.OnRepairError += delegate
		{
			_E010 = true;
			this._E000();
		};
		_consistencyService.OnRepairCompleted += delegate
		{
			_E010 = false;
			_E00C = true;
			this._E000();
		};
		_gameUpdateService.OnUpdateChecked += delegate(object sender, GameUpdateSet e)
		{
			_E00E = e.UpdateNecessity != UpdateNecessity.UpdateNotRequired;
			this._E000();
		};
		_gameUpdateService.OnStateChanged += delegate(object sender, GameUpdateServiceState e)
		{
			this._E000();
			if (e == GameUpdateServiceState.Idle)
			{
				if (GameState == GameState.InstallRequired || GameState == GameState.ReadyToGame || GameState == GameState.ReinstallRequired || GameState == GameState.RepairRequired)
				{
					try
					{
						if (Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
						{
							fileSystemWatcher.Path = _settingsService.SelectedBranch.GameRootDir;
							fileSystemWatcher.EnableRaisingEvents = true;
						}
						return;
					}
					catch
					{
						return;
					}
				}
				fileSystemWatcher.EnableRaisingEvents = false;
			}
			else
			{
				fileSystemWatcher.EnableRaisingEvents = false;
			}
		};
		_gameUpdateService.OnUpdateCompleted += delegate(object sender, UpdateCompletedEventArgs e)
		{
			_E012 = e.InstallationResult == InstallationResult.InstallationError || e.InstallationResult == InstallationResult.HasSkippedFiles;
			this._E000();
			if (e.InstallationResult == InstallationResult.Succeded || e.InstallationResult == InstallationResult.HasSkippedFiles)
			{
				_E00E = false;
				UpdateGameVersion();
				UpdateInstallationInfo();
			}
			this._E000();
		};
		_gameUpdateService.OnInstallationCompleted += delegate(object sender, InstallationCompletedEventArgs e)
		{
			if (e.InstallationResult == InstallationResult.Succeded)
			{
				_E00E = false;
				_E00B = true;
				ClearCache();
				UpdateInstallationInfo();
			}
		};
		_E006.OnLoggedOut += delegate
		{
			_E00B = null;
			_E015 = default(BsgVersion);
			CloseGame();
			this._E000();
		};
		_settingsService.OnUserProfileLoaded += delegate
		{
			this._E000();
		};
		_E009.OnQueueStateChanged += _E000;
	}

	private void _E000(bool isInTheQueue)
	{
		_E013 = isInTheQueue;
		this._E000();
	}

	public void Init()
	{
		_E00B = null;
		_E00C = null;
		_E00D = null;
		_E00E = null;
		_E00F = false;
		_E010 = false;
		_E011 = false;
		_E012 = false;
		_E014 = GameState.Unknown;
		if (_E000(out Process gameProcess))
		{
			gameProcess.Exited += _E000;
			try
			{
				gameProcess.EnableRaisingEvents = true;
				_E00F = true;
				_E00E = false;
			}
			catch (Win32Exception exception)
			{
				this.m__E005.LogWarning(exception, _E05B._E000(9684));
				_settingsService.GameProcessId = -1;
			}
			this._E000();
		}
	}

	private void _E000()
	{
		GameState = this._E000();
	}

	private GameState _E000()
	{
		if (_E00F)
		{
			return GameState.InGame;
		}
		if (_settingsService.IsGameBought)
		{
			if (_E00B == true)
			{
				if (!_E010 && (!_E00D.HasValue || _E00D == false))
				{
					if (!_E012 && (!_E00C.HasValue || _E00C == true))
					{
						bool? flag = _E00E;
						if (!flag.HasValue)
						{
							return GameState.Unknown;
						}
						if (_E00E == true)
						{
							return GameState.UpdateRequired;
						}
						if (_E00F)
						{
							return GameState.InGame;
						}
						if (_E013)
						{
							return GameState.InQueue;
						}
						if (_E011)
						{
							return GameState.PreparingForGame;
						}
						return GameState.ReadyToGame;
					}
					return GameState.RepairRequired;
				}
				return GameState.ReinstallRequired;
			}
			if (_E00B == false)
			{
				return GameState.InstallRequired;
			}
			return GameState.Unknown;
		}
		if (!_settingsService.IsGameBought)
		{
			return GameState.BuyRequired;
		}
		this.m__E005.LogDebug(_E05B._E000(9274));
		return GameState.Unknown;
	}

	public void UpdateGameVersion()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(_settingsService.SelectedBranch.GameRootDir) || !Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
			{
				GameVersion = default(BsgVersion);
			}
			else if (!CheckGameIsInstalled())
			{
				GameVersion = default(BsgVersion);
			}
			else
			{
				GameVersion = _E00A.GetGameVersion(this._E000());
			}
		}
		catch (Exception innerException)
		{
			throw new GameServiceException(BsgExceptionCode.ErrorWhileCheckingInstalledVersion, innerException);
		}
	}

	public void UpdateInstallationInfo()
	{
		try
		{
			UpdateGameVersion();
			this.m__E005.LogDebug(_E05B._E000(9241));
			string subkey = _E05B._E000(26210) + _settingsService.SelectedBranch.GameAppId;
			if (!CheckGameIsInstalled())
			{
				_E007.DeleteSectionFromRegistry(subkey);
			}
			else
			{
				long num = new DirectoryInfo(_settingsService.SelectedBranch.GameRootDir).GetFiles(_E05B._E000(9339), SearchOption.AllDirectories).Sum((FileInfo file) => file.Length) / 1024;
				string path = this._E000();
				string path2 = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _E05B._E000(19925));
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9343), path.WithoutLongPathPrefix(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9291), _settingsService.SelectedBranch.GameDisplayName, RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9303), GameVersion.ToString(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9376), num, RegistryValueKind.DWord);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9394), _settingsService.SelectedBranch.SiteUri.ToString(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(26357), _settingsService.SelectedBranch.GameRootDir.WithoutLongPathPrefix(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9401), 1.ToString(), RegistryValueKind.DWord);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9344), _appConfig.AppPublisher, RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9358), path2.WithoutLongPathPrefix(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9374), _settingsService.SelectedBranch.SiteUri.ToString(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9449), _settingsService.SelectedBranch.SiteUri.ToString(), RegistryValueKind.String);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9467), GameVersion.Major.ToString(), RegistryValueKind.DWord);
				_E007.AddEntryToRegistry(subkey, _E05B._E000(9414), GameVersion.Minor.ToString(), RegistryValueKind.DWord);
			}
			this.m__E005.LogDebug(_E05B._E000(9425));
		}
		catch (Win32Exception exception)
		{
			this.m__E005.LogWarning(exception, _E05B._E000(9018));
			throw;
		}
		catch (Exception exception2)
		{
			this.m__E005.LogError(exception2, _E05B._E000(8968));
		}
	}

	public bool CheckGameIsInstalled(string dirToCheck = null)
	{
		if (dirToCheck == null)
		{
			dirToCheck = _settingsService.SelectedBranch.GameRootDir;
		}
		if (Directory.Exists(dirToCheck))
		{
			return File.Exists(Path.Combine(dirToCheck, _appConfig.GameShortName + _E05B._E000(18970)));
		}
		return false;
	}

	public void UpdateGameIsInstalled()
	{
		bool flag = CheckGameIsInstalled();
		if (flag != _E00B)
		{
			_E00B = flag;
			this.m__E001?.Invoke(this, flag);
		}
		this._E000();
	}

	public async Task<(string gameSession, int timeToStartGameSec)> PrepareGame(CancellationToken cancellationToken)
	{
		if (_E011 || _E00F)
		{
			throw new Exception(_E05B._E000(8606));
		}
		_E011 = true;
		try
		{
			UpdateGameIsInstalled();
			if (!Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
			{
				throw new GameServiceException(BsgExceptionCode.GameRootDirectoryWasNotFound, _settingsService.SelectedBranch.GameRootDir);
			}
			if (!CheckGameIsInstalled())
			{
				throw new GameServiceException(BsgExceptionCode.ErrorWhileCheckingInstalledVersion);
			}
			if (!_consistencyService.CheckConsistency(fullCheck: false).IsSuccess)
			{
				throw new GameServiceException(BsgExceptionCode.ErrorWhileCheckingInstalledVersion);
			}
			try
			{
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _appConfig.GameFullName, _E05B._E000(8915));
				if (File.Exists(text))
				{
					new FileInfo(text).IsReadOnly = false;
				}
			}
			catch
			{
			}
			string item = null;
			int item2 = -1;
			do
			{
				GameSessionInfo gameSessionInfo = await _E008.GetGameSession(_E015, _settingsService.SelectedBranch.MatchingTag);
				if (gameSessionInfo.QueueInfo == null)
				{
					item = gameSessionInfo.Session;
					break;
				}
				item2 = await _E009.WaitForQueueAsync(gameSessionInfo.QueueInfo, cancellationToken);
			}
			while (_settingsService.QueueAutostartEnabled);
			return (item, item2);
		}
		finally
		{
			if (_E011)
			{
				_E011 = false;
				UpdateGameIsInstalled();
			}
		}
	}

	public async Task RunGame(string gameSession)
	{
		try
		{
			this.m__E005.LogDebug(_E05B._E000(8237));
			JObject jObject = new JObject();
			jObject.Add(_E05B._E000(8192), _settingsService.SelectedBranch.GameBackendUri.ToString().TrimEnd('/'));
			jObject.Add(_E05B._E000(8205), _settingsService.SelectedBranch.MatchingTag);
			JObject jObject2 = jObject;
			string arguments = _E05B._E000(8213) + gameSession + _E05B._E000(8310) + jObject2.ToString(Formatting.None);
			string text = this._E000();
			string anticheatExeFile;
			string text2 = (_E000(out anticheatExeFile) ? anticheatExeFile : text);
			Process process = new Process();
			process.StartInfo.FileName = text2;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.WorkingDirectory = _settingsService.SelectedBranch.GameRootDir;
			process.EnableRaisingEvents = true;
			process.Exited += _E000;
			if (!process.Start())
			{
				return;
			}
			_E00F = true;
			_settingsService.GameProcessId = process.Id;
			this._E000();
			Process process2 = ((text == text2) ? process : null);
			if (process2 == null)
			{
				this.m__E005.LogDebug(_E05B._E000(8316));
				try
				{
					Task task = Task.Delay(TimeSpan.FromSeconds(120.0));
					Task task2 = Task.Run(delegate
					{
						do
						{
							Thread.Sleep(250);
							process2 = Process.GetProcessesByName(_appConfig.GameShortName).FirstOrDefault();
						}
						while (_E00F && process2 == null && !task.IsCompleted);
					});
					await Task.WhenAny(task2, task);
				}
				catch (Exception exception)
				{
					this.m__E005.LogWarning(exception, _E05B._E000(8284));
				}
				if (process2 != null)
				{
					try
					{
						process2.Exited += _E000;
						process2.EnableRaisingEvents = true;
						_settingsService.GameProcessId = process2.Id;
					}
					catch (InvalidOperationException)
					{
						this.m__E005.LogWarning(_E05B._E000(8320));
					}
					try
					{
						process.EnableRaisingEvents = false;
						process.Exited -= _E000;
					}
					catch (InvalidOperationException)
					{
						this.m__E005.LogWarning(_E05B._E000(8419));
					}
				}
			}
			this.m__E005.LogDebug(_E05B._E000(8385), _E015.ToString(), process2?.Id, process2?.SessionId);
			this.m__E004?.Invoke(process2);
		}
		catch (Win32Exception ex3) when (ex3.NativeErrorCode == 1223)
		{
			this.m__E005.LogWarning(_E05B._E000(12051));
			throw new GameServiceException(BsgExceptionCode.CannotLaunchFromLongPath);
		}
		catch (GameServiceException ex4) when (ex4.BsgExceptionCode == BsgExceptionCode.GameRootDirectoryWasNotFound || ex4.BsgExceptionCode == BsgExceptionCode.ErrorWhileCheckingInstalledVersion)
		{
			UpdateGameIsInstalled();
			try
			{
				UpdateInstallationInfo();
			}
			catch
			{
			}
			throw;
		}
		catch (Exception innerException)
		{
			throw new GameServiceException(BsgExceptionCode.ErrorWhileStartingTheGame, innerException);
		}
		finally
		{
			this._E000();
		}
	}

	private void _E000(object sender, EventArgs e)
	{
		try
		{
			int num = (sender as Process)?.ExitCode ?? 0;
			this.m__E005.LogDebug(_E05B._E000(9072), _E015.ToString(), num);
			_settingsService.GameProcessId = -1;
			_E00F = false;
			this._E000();
			this.m__E003?.Invoke(new ProcessLifecycleInformation
			{
				ExitCode = num,
				AppVersion = _E015
			}).Wait();
		}
		catch (Exception exception)
		{
			this.m__E005.LogError(exception, _E05B._E000(9125));
		}
	}

	public void CloseGame()
	{
		try
		{
			if (_E000(out Process gameProcess) && !gameProcess.HasExited)
			{
				this.m__E005.LogInformation(_E05B._E000(9100), gameProcess.ProcessName);
				gameProcess.Close();
			}
		}
		catch (Exception innerException)
		{
			throw new GameServiceException(BsgExceptionCode.ErrorDuringClosingTheGame, innerException);
		}
	}

	private string _E000()
	{
		string text = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _appConfig.GameShortName + _E05B._E000(18970));
		if (!File.Exists(text))
		{
			throw new GameServiceException(BsgExceptionCode.CouldNotFindTheMainExeFileIn, _settingsService.SelectedBranch.GameRootDir);
		}
		return text;
	}

	private bool _E000(out Process gameProcess)
	{
		if (_settingsService.GameProcessId > -1)
		{
			try
			{
				gameProcess = Process.GetProcessById(_settingsService.GameProcessId);
				if (gameProcess.ProcessName == _appConfig.GameShortName)
				{
					return true;
				}
			}
			catch (ArgumentException ex) when (ex.HResult == -2147024809)
			{
				this.m__E005.LogDebug(_E05B._E000(9157), _settingsService.GameProcessId);
			}
			catch (Win32Exception exception)
			{
				this.m__E005.LogWarning(exception, _E05B._E000(8748), _settingsService.GameProcessId);
			}
			catch (Exception exception2)
			{
				this.m__E005.LogError(exception2, _E05B._E000(8729), _settingsService.GameProcessId);
			}
			_settingsService.GameProcessId = -1;
		}
		gameProcess = null;
		return false;
	}

	private bool _E000(out string anticheatExeFile)
	{
		string path = this._E000();
		anticheatExeFile = Path.Combine(Path.GetDirectoryName(path), _appConfig.GameShortName + _E05B._E000(8779));
		return File.Exists(anticheatExeFile);
	}

	public void ClearCache()
	{
		try
		{
			string text = Path.Combine(Path.GetTempPath(), _appConfig.AppPublisher, _appConfig.GameShortName);
			this.m__E005.LogInformation(_E05B._E000(8787), text);
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
				this.m__E005.LogInformation(_E05B._E000(8888));
			}
			else
			{
				this.m__E005.LogInformation(_E05B._E000(8851));
			}
		}
		catch (Exception exception)
		{
			this.m__E005.LogError(exception, _E05B._E000(8947));
		}
	}

	public void DeleteLocalIni()
	{
		string fullIniFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _appConfig.GameFullName, _E05B._E000(8915));
		_E000(fullIniFilePath);
	}

	public void DeleteSharedIni()
	{
		string fullIniFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _appConfig.GameFullName, _E05B._E000(8921));
		_E000(fullIniFilePath);
	}

	private void _E000(string fullIniFilePath)
	{
		try
		{
			this.m__E005.LogTrace(_E05B._E000(8486));
			if (File.Exists(fullIniFilePath))
			{
				File.Delete(fullIniFilePath);
				return;
			}
			this.m__E005.LogDebug(_E05B._E000(8448), fullIniFilePath);
		}
		catch (Exception exception)
		{
			this.m__E005.LogWarning(exception, _E05B._E000(8551), fullIniFilePath);
		}
	}

	public void InstallAnticheat()
	{
		if (_E000(out string anticheatExeFile))
		{
			int result = _E007.RunProcess(anticheatExeFile, _E05B._E000(8516), Path.GetDirectoryName(anticheatExeFile)).Result;
			if (result != 0)
			{
				throw new Exception(string.Format(_E05B._E000(8520), result));
			}
		}
	}

	public void UninstallAnticheat()
	{
		if (_E000(out string anticheatExeFile))
		{
			int result = _E007.RunProcess(anticheatExeFile, _E05B._E000(8626), Path.GetDirectoryName(anticheatExeFile)).Result;
			if (result != 0)
			{
				throw new Exception(string.Format(_E05B._E000(8630), result));
			}
		}
	}

	[CompilerGenerated]
	private void _E000(object s, FileSystemEventArgs ea)
	{
		_gameUpdateService.CheckForGameUpdate(fastChecking: true);
	}

	[CompilerGenerated]
	private void _E000(object s, RenamedEventArgs ea)
	{
		_gameUpdateService.CheckForGameUpdate(fastChecking: true);
	}

	[CompilerGenerated]
	private void _E001(object s, FileSystemEventArgs ea)
	{
		_gameUpdateService.CheckForGameUpdate(fastChecking: true);
	}

	[CompilerGenerated]
	private void _E001(object sender, EventArgs e)
	{
		_E00D = true;
		this._E000();
	}

	[CompilerGenerated]
	private void _E002(object sender, EventArgs e)
	{
		_E010 = true;
		this._E000();
	}

	[CompilerGenerated]
	private void _E003(object sender, EventArgs e)
	{
		_E010 = false;
		_E00C = true;
		this._E000();
	}

	[CompilerGenerated]
	private void _E004(object sender, EventArgs e)
	{
		_E00B = null;
		_E015 = default(BsgVersion);
		CloseGame();
		this._E000();
	}

	[CompilerGenerated]
	private void _E005(object s, EventArgs args)
	{
		this._E000();
	}
}
