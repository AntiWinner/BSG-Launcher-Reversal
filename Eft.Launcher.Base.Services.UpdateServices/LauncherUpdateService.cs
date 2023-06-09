using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DownloadService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Base.Services.UpdateServices;

public class LauncherUpdateService : ILauncherUpdateService
{
	private readonly ILogger _logger;

	private readonly ISettingsService _settingsService;

	private readonly ILauncherBackendService _backendService;

	private readonly IDownloadService _downloadService;

	private LauncherUpdateServiceState _state;

	private readonly string _launcherUpdateSignalFilePath;

	public ILauncherMetadata Metadata { get; }

	public LauncherUpdateServiceState State
	{
		get
		{
			return _state;
		}
		private set
		{
			_state = value;
			this.OnStateChanged?.Invoke(this, value);
		}
	}

	public event EventHandler<LauncherUpdateServiceState> OnStateChanged;

	public event EventHandler<IProgressReport> OnProgress;

	public event Action<Version, InstallationResult> OnLauncherInstallationCompleted;

	public LauncherUpdateService(ILogger<LauncherUpdateService> logger, ILauncherMetadata launcherMetadata, ISettingsService settingsService, ILauncherBackendService backendService, IDownloadService downloadService)
	{
		_logger = logger;
		Metadata = launcherMetadata;
		_settingsService = settingsService;
		_backendService = backendService;
		_downloadService = downloadService;
		_launcherUpdateSignalFilePath = Path.Combine(_settingsService.LocalAppDataDir, _E05B._E000(22088));
	}

	public void CheckForStartAfterUpdate()
	{
		if (!(Metadata.LauncherVersion != null))
		{
			return;
		}
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				if (File.Exists(_launcherUpdateSignalFilePath))
				{
					Version version = new Version(File.ReadAllText(_launcherUpdateSignalFilePath));
					InstallationResult arg = ((Metadata.LauncherVersion > version) ? InstallationResult.Succeded : InstallationResult.InstallationError);
					this.OnLauncherInstallationCompleted?.Invoke(Metadata.LauncherVersion, arg);
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, _E05B._E000(21764));
			}
			finally
			{
				if (File.Exists(_launcherUpdateSignalFilePath))
				{
					File.Delete(_launcherUpdateSignalFilePath);
				}
			}
		});
	}

	public async Task<LauncherUpdate> CheckForLauncherUpdate()
	{
		LauncherUpdate result = new LauncherUpdate();
		if (Metadata.LauncherVersion == null)
		{
			return result;
		}
		try
		{
			_logger.LogDebug(_E05B._E000(21860));
			State = LauncherUpdateServiceState.CheckingForLauncherUpdate;
			LauncherDistribResponseData launcherDistribResponseData = await _backendService.GetLauncherDistrib();
			if (launcherDistribResponseData == null)
			{
				return result;
			}
			result.UpdateIsRequired = launcherDistribResponseData.Version > Metadata.LauncherVersion;
			result.Version = launcherDistribResponseData.Version;
			result.DownloadUri = launcherDistribResponseData.DownloadUri;
			result.Hash = launcherDistribResponseData.Hash;
			State = LauncherUpdateServiceState.Idle;
			if (result.UpdateIsRequired)
			{
				_logger.LogInformation(_E05B._E000(21828), result.Version.ToString(), result.DownloadUri);
			}
			else
			{
				_logger.LogInformation(_E05B._E000(21919), Metadata.LauncherVersion.ToString());
			}
		}
		catch
		{
			State = LauncherUpdateServiceState.LauncherUpdateError;
			throw;
		}
		return result;
	}

	public async Task BeginUpdateLauncher(LauncherUpdate launcherUpdate)
	{
		try
		{
			try
			{
				_downloadService.Cleanup();
			}
			catch (FileNotFoundException)
			{
				_logger.LogError(_E05B._E000(21979));
			}
			_logger.LogDebug(_E05B._E000(21622));
			State = LauncherUpdateServiceState.DownloadingLauncherUpdate;
			string fileName = string.Format(_E05B._E000(21591), launcherUpdate.Version);
			await _downloadService.DownloadFile(launcherUpdate.DownloadUri, fileName, launcherUpdate.Hash, DownloadCategory.LauncherDistrib, CancellationToken.None, delegate(IProgressReport p)
			{
				this.OnProgress?.Invoke(this, p);
			});
			State = LauncherUpdateServiceState.Idle;
		}
		catch (Exception innerException)
		{
			State = LauncherUpdateServiceState.LauncherUpdateError;
			throw new UpdateServiceException(BsgExceptionCode.ErrorOnTheFirstPhaseOfUpdatingTheLauncher, innerException);
		}
	}

	public void EndUpdateLauncher(LauncherUpdate launcherUpdate)
	{
		try
		{
			_logger.LogDebug(_E05B._E000(22179));
			State = LauncherUpdateServiceState.ApplyingLauncherUpdate;
			string text = Path.Combine(_settingsService.LauncherTempDir, string.Format(_E05B._E000(22207), DownloadCategory.LauncherDistrib, launcherUpdate.Version));
			if (!File.Exists(text))
			{
				throw new UpdateServiceException(BsgExceptionCode.UpdateDistribDoesNotExist, text);
			}
			string arguments = _E05B._E000(22155);
			_logger.LogDebug(_E05B._E000(22257));
			Process.Start(text, arguments);
			_logger.LogDebug(_E05B._E000(22227));
			File.WriteAllText(_launcherUpdateSignalFilePath, Metadata.LauncherVersion.ToString());
		}
		catch (UpdateServiceException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			State = LauncherUpdateServiceState.LauncherUpdateError;
			throw new UpdateServiceException(BsgExceptionCode.ErrorOnTheLastPhaseOfUpdatingTheLauncher, innerException);
		}
	}
}
