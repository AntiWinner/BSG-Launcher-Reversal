using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Bsg.Launcher.Downloading;
using Bsg.Launcher.Utils;
using Eft.Launcher.Core;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Base.Services.ConsistencyService;

public class ConsistencyService : IConsistencyService
{
	private const int ProgressRate = 250;

	private readonly ILogger _logger;

	private readonly AppConfig _appConfig;

	private readonly ISettingsService _settingsService;

	private readonly ILauncherBackendService _backendService;

	private readonly Lazy<IGameService> _gameServiceLazy;

	private readonly IDownloadManagementService _downloadManagementService;

	private readonly IFileManager _fileManager;

	private readonly Utils _utils;

	private readonly List<string> _filters = new List<string>
	{
		_E05B._E000(12106),
		_E05B._E000(12108),
		_E05B._E000(12118),
		_E05B._E000(12127)
	};

	private readonly List<string> _criticalFiles = new List<string> { _E05B._E000(12201) };

	private readonly System.Timers.Timer _gcTimer;

	private DateTime _lastProgressTime = DateTime.MinValue;

	private CancellationTokenSource _cts;

	public string ConsistencyInfoFileName { get; } = _E05B._E000(12154);


	public IConsistencyCheckingResult LastConsistencyCheckingResult { get; private set; }

	public event EventHandler OnConsistencyCheckingStarted;

	public event EventHandler<IConsistencyCheckingResult> OnConsistencyCheckingCompleted;

	public event EventHandler OnConsistencyCheckingError;

	public event EventHandler<IProgressReport> OnConsistencyCheckingProgress;

	public event EventHandler OnRepairStarted;

	public event EventHandler OnRepairCompleted;

	public event EventHandler OnRepairError;

	public event EventHandler<IProgressReport> OnRepairProgress;

	public ConsistencyService(ILogger<ConsistencyService> logger, AppConfig appConfig, ISettingsService settingsService, ILauncherBackendService backendServiece, Lazy<IGameService> gameServiceLazy, IDownloadManagementService downloadManagementService, IAuthService authService, IFileManager fileManager, Utils utils)
	{
		_logger = logger;
		_appConfig = appConfig;
		_settingsService = settingsService;
		_backendService = backendServiece;
		_gameServiceLazy = gameServiceLazy;
		_downloadManagementService = downloadManagementService;
		_fileManager = fileManager;
		_utils = utils;
		authService.OnLoggedOut += delegate
		{
			CancelCurrentOperation();
		};
		_cts = new CancellationTokenSource();
		_gcTimer = new System.Timers.Timer(1000.0);
		_gcTimer.Elapsed += delegate
		{
			GC.Collect();
		};
	}

	public void CreateConsistencyInfo(string gameRootDir)
	{
		try
		{
			_logger.LogInformation(_E05B._E000(12170));
			string gameExeFile = Path.Combine(gameRootDir, _appConfig.GameShortName + _E05B._E000(18970));
			ConsistencyInfo consistencyInfo = new ConsistencyInfo(_utils.GetGameVersion(gameExeFile));
			_gcTimer.Start();
			foreach (string item in Directory.EnumerateFiles(gameRootDir, _E05B._E000(27034), SearchOption.AllDirectories))
			{
				string relativeFilePath = item.Substring(gameRootDir.Length).TrimStart('\\', '/', ' ');
				if (relativeFilePath.Split(Path.PathSeparator).Any((string p) => _filters.Any((string f) => Regex.IsMatch(p, WildCardToRegular(f)))))
				{
					_logger.LogInformation(_E05B._E000(12262), item);
					continue;
				}
				bool flag = _criticalFiles.Any((string cf) => relativeFilePath.StartsWith(cf, StringComparison.InvariantCultureIgnoreCase));
				if (flag)
				{
					_logger.LogInformation(_E05B._E000(12230), item);
				}
				consistencyInfo.Entries.Add(new ConsistencyInfoEntry(relativeFilePath, _utils.GetHashForFile(item, CancellationToken.None).ToHex(), new FileInfo(item).Length, flag));
			}
			string contents = consistencyInfo.ToJson();
			File.WriteAllText(Path.Combine(gameRootDir, ConsistencyInfoFileName), contents);
			_logger.LogInformation(_E05B._E000(11814));
		}
		catch (Exception innerException)
		{
			throw new ConsistencyServiceException(BsgExceptionCode.ErrorWhileCreatingConsistencyInfo, innerException);
		}
		finally
		{
			_gcTimer.Stop();
		}
	}

	private static string WildCardToRegular(string value)
	{
		return _E05B._E000(11785) + Regex.Escape(value).Replace(_E05B._E000(11791), _E05B._E000(27969)).Replace(_E05B._E000(11788), _E05B._E000(11793)) + _E05B._E000(11798);
	}

	public IConsistencyCheckingResult CheckConsistency(bool fullCheck)
	{
		try
		{
			_logger.LogDebug(_E05B._E000(11796), fullCheck ? _E05B._E000(11857) : _E05B._E000(11848));
			_gcTimer.Start();
			this.OnConsistencyCheckingStarted?.Invoke(this, EventArgs.Empty);
			ConsistencyInfo consistencyInfo = GetConsistencyInfo();
			_logger.LogDebug(_E05B._E000(11866) + ConsistencyInfoFileName + _E05B._E000(11938), consistencyInfo.Version.ToString(), consistencyInfo.Entries.Count);
			if (consistencyInfo.Entries.Count == 0)
			{
				_logger.LogWarning(ConsistencyInfoFileName + _E05B._E000(12007));
			}
			ConsistencyCheckingResult consistencyCheckingResult = new ConsistencyCheckingResult
			{
				IsFullCheck = fullCheck
			};
			CancellationToken cancellationToken = GetCancellationToken();
			int num = 0;
			foreach (ConsistencyInfoEntry entry in consistencyInfo.Entries)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string text = Path.Combine(_settingsService.SelectedBranch.GameRootDir, entry.Path);
				if (!File.Exists(text))
				{
					_logger.LogDebug(_E05B._E000(12012), text);
					consistencyCheckingResult.AddBrockenFile(entry.Path);
				}
				else if (new FileInfo(text).Length != entry.Size)
				{
					_logger.LogDebug(_E05B._E000(11563), text);
					consistencyCheckingResult.AddBrockenFile(entry.Path);
				}
				else if (fullCheck && !_utils.CheckHashForFile(text, entry.Hash, cancellationToken))
				{
					_logger.LogDebug(_E05B._E000(11616), text);
					consistencyCheckingResult.AddBrockenFile(entry.Path);
				}
				num++;
				if (this.OnConsistencyCheckingProgress != null && (DateTime.Now - _lastProgressTime).TotalMilliseconds > 250.0)
				{
					_lastProgressTime = DateTime.Now;
					ProgressReport e = new ProgressReport(num, consistencyInfo.Entries.Count);
					this.OnConsistencyCheckingProgress(this, e);
				}
			}
			this.OnConsistencyCheckingProgress?.Invoke(this, new ProgressReport(num, consistencyInfo.Entries.Count));
			LastConsistencyCheckingResult = consistencyCheckingResult;
			consistencyCheckingResult.IsSuccess = consistencyCheckingResult.BrokenFiles.Count == 0;
			_logger.Log(consistencyCheckingResult.IsSuccess ? LogLevel.Debug : LogLevel.Warning, _E05B._E000(11681), consistencyCheckingResult.IsSuccess ? _E05B._E000(11667) : _E05B._E000(11659));
			this.OnConsistencyCheckingCompleted?.Invoke(this, consistencyCheckingResult);
			return consistencyCheckingResult;
		}
		catch (OperationCanceledException)
		{
			this.OnConsistencyCheckingError?.Invoke(this, EventArgs.Empty);
			throw;
		}
		catch (BsgException)
		{
			this.OnConsistencyCheckingError?.Invoke(this, EventArgs.Empty);
			throw;
		}
		catch (Exception innerException)
		{
			this.OnConsistencyCheckingError?.Invoke(this, EventArgs.Empty);
			throw new ConsistencyServiceException(BsgExceptionCode.ErrorDuringCheckingConsistency, innerException);
		}
		finally
		{
			_gcTimer.Stop();
		}
	}

	public async Task Repair(IConsistencyCheckingResult consistencyCheckingResult)
	{
		_ = 1;
		try
		{
			_gcTimer.Start();
			CancellationToken cancellationToken = GetCancellationToken();
			this.OnRepairStarted?.Invoke(this, EventArgs.Empty);
			GamePackageInfo gamePackageInfo = await _backendService.GetGamePackage(_gameServiceLazy.Value.GameVersion);
			if (gamePackageInfo == null)
			{
				throw new Exception(_E05B._E000(11675));
			}
			_settingsService.IsInTheGameInstallationProgress = true;
			_settingsService.Save();
			(string, string)[] downloadSpecification = consistencyCheckingResult.BrokenFiles.Select((string brokenFileRelativePath) => (Path.Combine(gamePackageInfo.UnpackedUri, brokenFileRelativePath), Path.Combine(_settingsService.SelectedBranch.GameRootDir, brokenFileRelativePath))).ToArray();
			await _downloadManagementService.DownloadFilesAsync(downloadSpecification, ObsoleteProgressHandler, tryUseMetadata: false, redownloadIfExist: true, cancellationToken);
			_gameServiceLazy.Value.ClearCache();
			_settingsService.IsInTheGameInstallationProgress = false;
			_settingsService.Save();
			this.OnRepairCompleted?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception innerException)
		{
			this.OnRepairError?.Invoke(this, EventArgs.Empty);
			throw new ConsistencyServiceException(BsgExceptionCode.ErrorDuringRepair, innerException);
		}
		finally
		{
			_gcTimer.Stop();
		}
		void ObsoleteProgressHandler(long bytesDownloaded, long fileSize)
		{
			this.OnRepairProgress?.Invoke(this, new ProgressReport(bytesDownloaded, fileSize));
		}
	}

	private ConsistencyInfo GetConsistencyInfo()
	{
		string text = Path.Combine(_settingsService.SelectedBranch.GameRootDir, ConsistencyInfoFileName);
		if (!File.Exists(text))
		{
			throw new ConsistencyServiceException(BsgExceptionCode.FileNotFound, text);
		}
		using FileStream stream = _fileManager.CaptureFile(text);
		using StreamReader streamReader = new StreamReader(stream);
		return ConsistencyInfo.FromJson(streamReader.ReadToEnd());
	}

	private CancellationToken GetCancellationToken()
	{
		if (_cts.IsCancellationRequested)
		{
			_cts = new CancellationTokenSource();
		}
		return _cts.Token;
	}

	public void CancelCurrentOperation()
	{
		_cts.Cancel();
	}
}
