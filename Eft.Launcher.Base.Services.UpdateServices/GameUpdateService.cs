using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.ConsistencyControl;
using Bsg.Launcher.Downloading;
using Bsg.Launcher.Updating;
using Bsg.Launcher.Utils;
using Bsg.Math.Graphs;
using Bsg.Math.Graphs.Dijkstra;
using Bsg.Network.MultichannelDownloading;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.CompressionService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.DownloadService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Base.Services.UpdateServices;

public class GameUpdateService : IGameUpdateService
{
	private enum GameUpdateServiceLastAction
	{
		Unknown,
		DownloadAndInstallGame,
		DownloadAndApplyGameUpdateSet
	}

	private GameUpdateServiceState _state;

	private readonly ILogger _logger;

	private readonly ISettingsService _settingsService;

	private readonly Lazy<IGameService> _gameServiceLazy;

	private readonly ILauncherBackendService _backendService;

	private readonly IDownloadService _downloadService;

	private readonly IDownloadManagementService _downloadManagementService;

	private readonly ICompressionService _compressionService;

	private readonly IConsistencyService _consistencyService;

	private readonly IConsistencyControlService _consistencyControlService;

	private readonly IAccessService _accessService;

	private readonly IAuthService _authService;

	private readonly IUpdateManager _updateManager;

	private readonly IDialogService _dialogService;

	private readonly IFileManager _fileManager;

	private readonly Utils _utils;

	private readonly AppConfig _appConfig;

	private CancellationTokenSource _cts;

	private GameUpdateServiceLastAction _lastAction;

	private GamePackageInfo _latestGamePackageInfo;

	private GameUpdateSet _lastGameUpdateSet;

	public GameUpdateServiceState State
	{
		get
		{
			return _state;
		}
		private set
		{
			if (_state != value)
			{
				_state = value;
				this.OnStateChanged?.Invoke(this, value);
			}
		}
	}

	public event Action<long, long> OnProgress;

	public event EventHandler<GameUpdateServiceState> OnStateChanged;

	public event EventHandler<GameUpdateSet> OnUpdateChecked;

	public event EventHandler<UpdateCompletedEventArgs> OnUpdateCompleted;

	public event EventHandler<InstallationCompletedEventArgs> OnInstallationCompleted;

	public event Action<BsgVersion> OnInstallationStarted;

	public GameUpdateService(ILogger<GameUpdateService> logger, ISettingsService settingsService, Lazy<IGameService> gameServiceLazy, ILauncherBackendService backendService, IDownloadService downloadService, IDownloadManagementService downloadManagementService, ICompressionService compressionService, IConsistencyService consistencyService, IConsistencyControlService consistencyControlService, IAccessService accessService, IAuthService authService, IUpdateManager updateManager, IDialogService dialogService, IFileManager fileManager, Utils utils, AppConfig appConfig)
	{
		_logger = logger;
		_settingsService = settingsService;
		_gameServiceLazy = gameServiceLazy;
		_backendService = backendService;
		_downloadService = downloadService;
		_downloadManagementService = downloadManagementService;
		_compressionService = compressionService;
		_consistencyService = consistencyService;
		_consistencyControlService = consistencyControlService;
		_accessService = accessService;
		_authService = authService;
		_updateManager = updateManager;
		_dialogService = dialogService;
		_fileManager = fileManager;
		_utils = utils;
		_appConfig = appConfig;
		_consistencyService.OnConsistencyCheckingStarted += delegate
		{
			State = GameUpdateServiceState.ConsistencyChecking;
		};
		_consistencyService.OnConsistencyCheckingCompleted += delegate
		{
			State = GameUpdateServiceState.Idle;
		};
		_consistencyService.OnConsistencyCheckingError += delegate
		{
			State = GameUpdateServiceState.UpdateError;
		};
		_consistencyService.OnRepairStarted += delegate
		{
			State = GameUpdateServiceState.RepairingGame;
		};
		_consistencyService.OnRepairCompleted += delegate
		{
			State = GameUpdateServiceState.Idle;
		};
		_consistencyService.OnRepairError += delegate
		{
			State = GameUpdateServiceState.UpdateError;
		};
		_authService.OnLoggedOut += delegate
		{
			PauseInstallation();
		};
		_cts = new CancellationTokenSource();
		_settingsService.OnBranchChanged += delegate
		{
			State = GameUpdateServiceState.Idle;
			_lastAction = GameUpdateServiceLastAction.Unknown;
		};
		_settingsService.OnBranchObsolete += SettingsService_OnBranchObsolete;
	}

	private async void SettingsService_OnBranchObsolete(object sender, IBranch branch)
	{
		try
		{
			if (branch.GameRootDir != null)
			{
				string uninstaller = Path.Combine(branch.GameRootDir, _E05B._E000(19925));
				bool flag = File.Exists(uninstaller);
				if (flag)
				{
					flag = await _dialogService.ShowDialog(DialogWindowMessage.TheBranchNoLongerExists, branch.Name) == DialogResult.Positive;
				}
				if (flag)
				{
					Process.Start(uninstaller);
				}
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(19495), branch?.Name);
		}
	}

	public async Task<GameUpdateSet> CheckForGameUpdate(bool fastChecking = false)
	{
		GameUpdateSet result = new GameUpdateSet
		{
			UpdateNecessity = UpdateNecessity.UpdateNotRequired
		};
		try
		{
			_logger.LogInformation(_E05B._E000(19483));
			State = GameUpdateServiceState.CheckingForUpdate;
			Task dummyProgress = Task.Delay(5000);
			_gameServiceLazy.Value.UpdateGameVersion();
			if (_gameServiceLazy.Value.CheckGameIsInstalled())
			{
				BsgVersion gameVersion = _gameServiceLazy.Value.GameVersion;
				if (gameVersion == default(BsgVersion))
				{
					_logger.LogWarning(_E05B._E000(19564));
				}
				else
				{
					result = await GetGameUpdateSet(gameVersion);
				}
			}
			_gameServiceLazy.Value.UpdateGameIsInstalled();
			if (!fastChecking)
			{
				await dummyProgress;
			}
		}
		catch (NetworkException ex) when (ex.IsLocalProblems)
		{
			_logger.LogWarning(ex, _E05B._E000(19613));
			throw;
		}
		catch (Exception exception)
		{
			_logger.LogWarning(exception, _E05B._E000(19658));
			throw;
		}
		finally
		{
			State = GameUpdateServiceState.Idle;
			this.OnUpdateChecked?.Invoke(this, result);
		}
		return result;
	}

	private async Task<GameUpdateSet> GetGameUpdateSet(BsgVersion fromVersion)
	{
		IReadOnlyCollection<GameUpdateInfo> readOnlyCollection = await _backendService.GetGameUpdates();
		if (!readOnlyCollection.Any())
		{
			_logger.LogInformation(_E05B._E000(19238));
			return new GameUpdateSet
			{
				UpdateNecessity = UpdateNecessity.UpdateNotRequired
			};
		}
		Graph<BsgVersion> graph = new Graph<BsgVersion>();
		foreach (GameUpdateInfo item in readOnlyCollection)
		{
			graph.OneWayConnect(item.FromVersion, item.Version, 1, item);
		}
		if (graph.Vertices.Any())
		{
			BsgVersion value = graph.FindLatestVertex().Value;
			if (fromVersion != value && value != default(BsgVersion))
			{
				if (new DijkstraAlgorithm<BsgVersion>(graph).TryFindShortestPathEdges(fromVersion, value, out var result))
				{
					GameUpdateInfo[] updates = result.Select((GraphEdge<BsgVersion> e) => (GameUpdateInfo)e.Tag).ToArray();
					_logger.LogInformation(_E05B._E000(19256), fromVersion.ToString(), value.ToString());
					return new GameUpdateSet
					{
						NewVersion = value,
						UpdateNecessity = UpdateNecessity.UpdateRequired,
						Updates = updates
					};
				}
				_logger.LogInformation(_E05B._E000(19322), fromVersion.ToString(), value.ToString());
				return new GameUpdateSet
				{
					NewVersion = value,
					UpdateNecessity = UpdateNecessity.ReinstallRequired
				};
			}
		}
		_logger.LogInformation(_E05B._E000(19390), fromVersion.ToString());
		return new GameUpdateSet
		{
			UpdateNecessity = UpdateNecessity.UpdateNotRequired
		};
	}

	public async Task<InstallationResult> DownloadAndInstallGame(string installationPath, GamePackageInfo gameInstallationInfo)
	{
		if (gameInstallationInfo == null)
		{
			throw new ArgumentNullException(_E05B._E000(19434));
		}
		InstallationResult result = InstallationResult.Unknown;
		GameUpdateServiceState finalState = GameUpdateServiceState.Idle;
		BsgVersion gameVersion = default(BsgVersion);
		try
		{
			_logger.LogDebug(_E05B._E000(19453));
			Directory.CreateDirectory(installationPath);
			if (!(await _accessService.AssignFullPermissions(installationPath)))
			{
				return InstallationResult.Stopped;
			}
			this.OnInstallationStarted?.Invoke(gameInstallationInfo.Version);
			CancellationToken cancellationToken = GetCancellationToken();
			_lastAction = GameUpdateServiceLastAction.DownloadAndInstallGame;
			_latestGamePackageInfo = gameInstallationInfo;
			_settingsService.SelectedBranch.GameRootDir = installationPath;
			cancellationToken.ThrowIfCancellationRequested();
			_logger.LogInformation(_E05B._E000(18982));
			IEnumerable<IBranch> enumerable = _settingsService.Branches.Where((IBranch b) => !b.IsSelected);
			foreach (IBranch b2 in enumerable)
			{
				string text = Path.Combine(b2.GameRootDir ?? "", _appConfig.GameShortName + _E05B._E000(18970));
				if (File.Exists(text))
				{
					BsgVersion branchVersion = _utils.GetGameVersion(text);
					_logger.LogDebug(_E05B._E000(18973), branchVersion, b2.Name);
					GameUpdateSet updateSet = await GetGameUpdateSet(branchVersion);
					if (updateSet.UpdateNecessity != UpdateNecessity.ReinstallRequired)
					{
						State = GameUpdateServiceState.InstallingUpdate;
						_settingsService.IsInTheGameInstallationProgress = true;
						_settingsService.Save();
						_utils.CopyDir(b2.GameRootDir, installationPath, this.OnProgress);
						if (updateSet.UpdateNecessity == UpdateNecessity.UpdateRequired)
						{
							await DownloadAndApplyGameUpdateSet(updateSet);
						}
						gameVersion = updateSet.NewVersion;
						_settingsService.IsInTheGameInstallationProgress = false;
						_settingsService.Save();
						return InstallationResult.Succeded;
					}
					_logger.LogInformation(_E05B._E000(19123), branchVersion, b2.Name);
				}
				else
				{
					_logger.LogDebug(_E05B._E000(18690), b2.Name);
				}
			}
			_logger.LogInformation(_E05B._E000(18753));
			gameVersion = gameInstallationInfo.Version;
			State = GameUpdateServiceState.DownloadingUpdate;
			string text2 = await _downloadManagementService.DownloadTheGamePackageAsync(gameInstallationInfo, this.OnProgress, cancellationToken);
			if (!string.IsNullOrWhiteSpace(gameInstallationInfo.Hash))
			{
				State = GameUpdateServiceState.ConsistencyChecking;
				_consistencyControlService.EnsureConsistencyByHash(text2, gameInstallationInfo.Hash, this.OnProgress, cancellationToken);
			}
			State = GameUpdateServiceState.InstallingUpdate;
			_settingsService.IsInTheGameInstallationProgress = true;
			_settingsService.Save();
			await _compressionService.ExtractZipToDir(text2, installationPath, cancellationToken, this.OnProgress);
			_logger.LogDebug(_E05B._E000(18833));
			_lastAction = GameUpdateServiceLastAction.Unknown;
			_gameServiceLazy.Value.InstallAnticheat();
			_settingsService.IsInTheGameInstallationProgress = false;
			_settingsService.Save();
			result = InstallationResult.Succeded;
			finalState = GameUpdateServiceState.Idle;
		}
		catch (Win32Exception exception)
		{
			_logger.LogWarning(exception, _E05B._E000(18933));
			finalState = GameUpdateServiceState.Idle;
			throw;
		}
		catch (OperationCanceledException)
		{
			if (State != GameUpdateServiceState.Pause)
			{
				State = GameUpdateServiceState.Idle;
				result = InstallationResult.Stopped;
			}
			else
			{
				State = GameUpdateServiceState.Pause;
				result = InstallationResult.Paused;
			}
			finalState = State;
			throw;
		}
		catch (ConsistencyControlServiceException)
		{
			result = InstallationResult.ConsistencyError;
			finalState = GameUpdateServiceState.Idle;
			throw;
		}
		catch (MultichannelDownloadingException)
		{
			result = InstallationResult.DownloadError;
			finalState = GameUpdateServiceState.Idle;
			throw;
		}
		catch (Exception ex4) when (ex4.HResult == -2147024784)
		{
			if (State == GameUpdateServiceState.DownloadingUpdate)
			{
				result = InstallationResult.DownloadError;
				finalState = GameUpdateServiceState.Idle;
			}
			else
			{
				finalState = GameUpdateServiceState.UpdateError;
				result = InstallationResult.InstallationError;
			}
			throw;
		}
		catch (Exception innerException)
		{
			result = InstallationResult.InstallationError;
			finalState = GameUpdateServiceState.UpdateError;
			throw new UpdateServiceException(BsgExceptionCode.ErrorWhileInstallingTheGame, innerException);
		}
		finally
		{
			this.OnInstallationCompleted?.Invoke(this, new InstallationCompletedEventArgs(gameVersion, result));
			State = finalState;
		}
		return result;
	}

	public async Task<InstallationResult> DownloadAndApplyGameUpdateSet(GameUpdateSet gameUpdateSet)
	{
		BsgVersion initialVersion = _gameServiceLazy.Value.GameVersion;
		InstallationResult result = InstallationResult.Unknown;
		GameUpdateServiceState finalState = GameUpdateServiceState.Idle;
		long allUpdatesInstallationTime;
		long fullyCompletedUpdatesProgress;
		try
		{
			if (!(await _accessService.AssignFullPermissions(_settingsService.SelectedBranch.GameRootDir)))
			{
				result = InstallationResult.Stopped;
			}
			else
			{
				CancellationToken cancellationToken = GetCancellationToken();
				cancellationToken.ThrowIfCancellationRequested();
				State = GameUpdateServiceState.DownloadingUpdate;
				_lastAction = GameUpdateServiceLastAction.DownloadAndApplyGameUpdateSet;
				_lastGameUpdateSet = gameUpdateSet;
				string[] array = await _downloadManagementService.DownloadTheGameUpdatesAsync(gameUpdateSet.Updates, this.OnProgress, cancellationToken);
				State = GameUpdateServiceState.ConsistencyChecking;
				List<(string, string)> list = new List<(string, string)>(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					list.Add((array[i], gameUpdateSet.Updates[i].Hash));
				}
				_consistencyControlService.EnsureConsistencyByHash(list, this.OnProgress, cancellationToken);
				State = GameUpdateServiceState.InstallingUpdate;
				_settingsService.IsInTheGameInstallationProgress = true;
				_settingsService.Save();
				_gameServiceLazy.Value.UninstallAnticheat();
				IList<IUpdate> list2 = new List<IUpdate>(array.Length);
				try
				{
					string[] array2 = array;
					foreach (string path in array2)
					{
						Stream stream = _fileManager.CaptureFile(path);
						try
						{
							IUpdate item = _updateManager.OpenUpdate(stream);
							list2.Add(item);
						}
						catch
						{
							stream.Dispose();
							throw;
						}
					}
					allUpdatesInstallationTime = list2.Sum((IUpdate u) => u.ApproximateInstallationTime);
					fullyCompletedUpdatesProgress = 0L;
					this.OnProgress?.Invoke(0L, allUpdatesInstallationTime);
					foreach (IUpdate item2 in list2)
					{
						_updateManager.InstallUpdate(item2, _settingsService.SelectedBranch.GameRootDir, UpdateInstallationProgress, cancellationToken);
						fullyCompletedUpdatesProgress += item2.ApproximateInstallationTime;
					}
					GC.Collect();
					_settingsService.IsInTheGameInstallationProgress = false;
					_settingsService.Save();
					this.OnProgress?.Invoke(allUpdatesInstallationTime, allUpdatesInstallationTime);
				}
				finally
				{
					foreach (IUpdate item3 in list2)
					{
						item3.Dispose();
					}
				}
				_gameServiceLazy.Value.InstallAnticheat();
				_downloadService.Cleanup();
				try
				{
					DeleteEmptyDirectories(_settingsService.SelectedBranch.GameRootDir);
				}
				catch (Exception exception)
				{
					_logger.LogError(exception, _E05B._E000(18465));
				}
				finalState = GameUpdateServiceState.Idle;
				if (result == InstallationResult.Unknown)
				{
					result = InstallationResult.Succeded;
				}
			}
		}
		catch (Win32Exception exception2)
		{
			_logger.LogWarning(exception2, _E05B._E000(18439));
			finalState = GameUpdateServiceState.Idle;
			result = InstallationResult.Stopped;
			throw;
		}
		catch (OperationCanceledException)
		{
			if (State != GameUpdateServiceState.Pause)
			{
				State = GameUpdateServiceState.Idle;
				result = InstallationResult.Stopped;
			}
			else
			{
				State = GameUpdateServiceState.Pause;
				result = InstallationResult.Paused;
			}
			finalState = State;
			throw;
		}
		catch (ConsistencyControlServiceException)
		{
			result = InstallationResult.ConsistencyError;
			finalState = GameUpdateServiceState.Idle;
			throw;
		}
		catch (MultichannelDownloadingException)
		{
			result = InstallationResult.DownloadError;
			finalState = GameUpdateServiceState.Idle;
			throw;
		}
		catch (Exception ex4) when (ex4.HResult == -2147024784)
		{
			if (State == GameUpdateServiceState.DownloadingUpdate)
			{
				result = InstallationResult.DownloadError;
				finalState = GameUpdateServiceState.Idle;
			}
			else
			{
				finalState = GameUpdateServiceState.UpdateError;
				result = InstallationResult.InstallationError;
			}
			throw;
		}
		catch (IOException exception3)
		{
			_logger.LogWarning(exception3, _E05B._E000(18439));
			finalState = GameUpdateServiceState.Idle;
			result = InstallationResult.Stopped;
			throw;
		}
		catch (Exception exception4)
		{
			_logger.LogError(exception4, _E05B._E000(18545));
			finalState = GameUpdateServiceState.UpdateError;
			result = InstallationResult.InstallationError;
			throw;
		}
		finally
		{
			if (result == InstallationResult.Succeded)
			{
				_lastAction = GameUpdateServiceLastAction.Unknown;
			}
			this.OnUpdateCompleted?.Invoke(this, new UpdateCompletedEventArgs(initialVersion, gameUpdateSet.NewVersion, result));
			State = finalState;
		}
		return result;
		void UpdateInstallationProgress(long current, long total)
		{
			this.OnProgress?.Invoke(fullyCompletedUpdatesProgress + current, allUpdatesInstallationTime);
		}
	}

	public void CreateUpdate(BsgVersion fromVersion, BsgVersion toVersion, string oldDir, string newDir, string updatePath)
	{
		try
		{
			State = GameUpdateServiceState.CreatingUpdate;
			using FileStream updateOutputStream = _fileManager.CaptureFile(updatePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
			UpdateMetadata metadata = _updateManager.CreateUpdate(oldDir, fromVersion, newDir, toVersion, updateOutputStream, GetCancellationToken());
			string directoryName = Path.GetDirectoryName(updatePath);
			string fileName = Path.GetFileName(updatePath);
			File.WriteAllText(contents: _updateManager.CreateTxtUpdateReport(metadata), path: Path.Combine(directoryName, fileName + _E05B._E000(19965)));
			File.WriteAllText(contents: _updateManager.CreateCsvUpdateReport(metadata), path: Path.Combine(directoryName, fileName + _E05B._E000(19913)));
		}
		finally
		{
			State = GameUpdateServiceState.Idle;
		}
	}

	public void PauseInstallation()
	{
		State = GameUpdateServiceState.Pause;
		_cts.Cancel();
	}

	public async Task ResumeInstallation()
	{
		if (_lastAction == GameUpdateServiceLastAction.DownloadAndInstallGame)
		{
			if (await DownloadAndInstallGame(_settingsService.SelectedBranch.GameRootDir, _latestGamePackageInfo) == InstallationResult.Succeded)
			{
				await CheckForGameUpdate(fastChecking: true);
			}
		}
		else if (_lastAction == GameUpdateServiceLastAction.DownloadAndApplyGameUpdateSet)
		{
			if (await DownloadAndApplyGameUpdateSet(_lastGameUpdateSet) == InstallationResult.Succeded)
			{
				await CheckForGameUpdate(fastChecking: true);
			}
		}
		else
		{
			_logger.LogWarning(_E05B._E000(18512), _lastAction);
		}
	}

	public async Task StopInstallation(bool showCancellationDialog)
	{
		bool flag = true;
		IDialogWindow stopInstallationDialog;
		if (showCancellationDialog)
		{
			stopInstallationDialog = await _dialogService.CreateDialog(DialogWindowMessage.IfYouStopDownloadingTheCurrentlyDownloadedFilesWillBeDeleted);
			OnStateChanged += onStateChangedHandler;
			flag = await stopInstallationDialog.ShowDialog() == DialogResult.Positive;
			OnStateChanged -= onStateChangedHandler;
		}
		if (flag)
		{
			State = GameUpdateServiceState.Idle;
			_cts.Cancel();
			await Task.Delay(1000);
			if (_latestGamePackageInfo != null)
			{
				_downloadService.Cleanup(_latestGamePackageInfo);
			}
			if (_lastGameUpdateSet != null)
			{
				_downloadService.Cleanup(_lastGameUpdateSet);
			}
			_downloadService.CleanFileFragments();
		}
		void onStateChangedHandler(object e, GameUpdateServiceState state)
		{
			if (state != GameUpdateServiceState.DownloadingUpdate || state != GameUpdateServiceState.Pause)
			{
				stopInstallationDialog.Close();
			}
		}
	}

	public async Task<bool> TryResumeLastGameUpdateSetInstallation()
	{
		_logger.LogDebug(_E05B._E000(18586));
		bool result = false;
		try
		{
			IEnumerable<string> gameUpdateFiles = _downloadService.GetFileFragments(DownloadCategory.EftClientUpdate).Concat(_downloadService.GetFiles(DownloadCategory.EftClientUpdate));
			if (gameUpdateFiles.Any())
			{
				GameUpdateSet gameUpdateSet = await CheckForGameUpdate(fastChecking: true);
				_lastGameUpdateSet = gameUpdateSet;
				List<string> source = gameUpdateFiles.Where(delegate(string guf)
				{
					IReadOnlyCollection<BsgVersion> versions = _utils.ExtractVersionsFromString(Path.GetFileNameWithoutExtension(guf));
					return versions.Count == 2 && gameUpdateSet.Updates.Any((GameUpdateInfo gu) => gu.FromVersion == versions.First() && gu.Version == versions.Skip(1).First());
				}).ToList();
				if (source.Any())
				{
					_lastAction = GameUpdateServiceLastAction.DownloadAndApplyGameUpdateSet;
					State = GameUpdateServiceState.Pause;
					result = true;
					long arg = source.Sum((string f) => new FileInfo(f).Length);
					long arg2 = (from u in gameUpdateSet.Updates
						select await _downloadManagementService.GetFileSizeAsync(u.DownloadUri, _cts.Token) into t
						select t.Result).Sum();
					this.OnProgress?.Invoke(arg, arg2);
				}
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(18645));
			if (State == GameUpdateServiceState.Pause)
			{
				State = GameUpdateServiceState.Idle;
			}
		}
		if (result)
		{
			_logger.LogDebug(_E05B._E000(22296));
		}
		else
		{
			_logger.LogDebug(_E05B._E000(22367));
		}
		return result;
	}

	public async Task<bool> TryResumeLastGameInstallation()
	{
		_logger.LogDebug(_E05B._E000(22427));
		bool result = false;
		try
		{
			if (!string.IsNullOrWhiteSpace(_settingsService.SelectedBranch.GameRootDir))
			{
				IEnumerable<string> gamePackageFragments = _downloadService.GetFileFragments(DownloadCategory.EftClientDistr);
				if (gamePackageFragments.Any())
				{
					GamePackageInfo gamePackageInfo = (_latestGamePackageInfo = await _backendService.GetGamePackage());
					if (gamePackageInfo != null)
					{
						BsgVersion actualVersion = gamePackageInfo.Version;
						string text = gamePackageFragments.FirstOrDefault((string f) => _utils.ExtractVersionsFromString(Path.GetFileNameWithoutExtension(f)).Contains(actualVersion));
						if (!string.IsNullOrEmpty(text))
						{
							_lastAction = GameUpdateServiceLastAction.DownloadAndInstallGame;
							State = GameUpdateServiceState.Pause;
							result = true;
							long sizeOfDownloadedFile = new FileInfo(text).Length;
							long arg = await _downloadManagementService.GetFileSizeAsync(gamePackageInfo.DownloadUri, _cts.Token);
							this.OnProgress?.Invoke(sizeOfDownloadedFile, arg);
						}
					}
				}
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(22465));
			if (State == GameUpdateServiceState.Pause)
			{
				State = GameUpdateServiceState.Idle;
			}
		}
		if (result)
		{
			_logger.LogDebug(_E05B._E000(22071));
		}
		else
		{
			_logger.LogDebug(_E05B._E000(22113));
		}
		return result;
	}

	private void DeleteEmptyDirectories(string rootDir)
	{
		string[] directories = Directory.GetDirectories(rootDir);
		foreach (string text in directories)
		{
			DeleteEmptyDirectories(text);
			if (!Directory.EnumerateFileSystemEntries(text).Any())
			{
				Directory.Delete(text, recursive: false);
			}
		}
	}

	private CancellationToken GetCancellationToken()
	{
		if (_cts.IsCancellationRequested)
		{
			_cts = new CancellationTokenSource();
		}
		return _cts.Token;
	}
}
