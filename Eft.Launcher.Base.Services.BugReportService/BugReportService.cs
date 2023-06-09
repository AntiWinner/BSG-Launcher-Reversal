using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.Logging;
using Bsg.Launcher.Services.BugReportService;
using Eft.Launcher.Logging;
using Eft.Launcher.Services;
using Eft.Launcher.Services.BugReportService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.InformationCollectionService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Eft.Launcher.Base.Services.BugReportService;

public sealed class BugReportService : IBugReportService, IService
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public BugReportService _E000;

		public bool _E001;

		internal void _E000(int progress)
		{
			this._E000._E004?.Invoke(this._E000, new BugReportSendingProgressArgs(BugReportSendingState.CollectingServerAvailabilityInfo, (int)((float)progress * 0.7f)));
		}

		internal void _E001(int progress)
		{
			this._E000._E004?.Invoke(this._E000, new BugReportSendingProgressArgs(BugReportSendingState.SendingReport, this._E001 ? (70 + (int)((float)progress * 0.3f)) : progress));
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public TimeSpan _E000;

		public Func<string, bool> _E001;

		internal bool _E000(string d)
		{
			return DateTime.Now - new DirectoryInfo(d).CreationTime < this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public TimeSpan _E000;

		public Func<string, bool> _E001;

		internal bool _E000(string d)
		{
			return DateTime.Now - new DirectoryInfo(d).CreationTime < this._E000;
		}
	}

	private static readonly Regex m__E000 = new Regex(_E05B._E000(11123));

	[CompilerGenerated]
	private PreparingToSendAutoReportEventHandler m__E001;

	[CompilerGenerated]
	private EventHandler _E002;

	[CompilerGenerated]
	private EventHandler<BugReportSendingArgs> m__E003;

	[CompilerGenerated]
	private EventHandler<BugReportSendingProgressArgs> _E004;

	private readonly ILogger m__E005;

	private readonly LogOptions _E006;

	private readonly AppConfig _appConfig;

	private readonly ISettingsService _settingsService;

	private readonly ISiteCommunicationService _E007;

	private readonly IInformationCollectionService _E008;

	private readonly IGameService _E009;

	private readonly JsonSerializerSettings _E00A;

	private int _E00B;

	private CancellationTokenSource _E00C;

	public event PreparingToSendAutoReportEventHandler OnPreparingToSendAutoReport
	{
		[CompilerGenerated]
		add
		{
			PreparingToSendAutoReportEventHandler preparingToSendAutoReportEventHandler = this.m__E001;
			PreparingToSendAutoReportEventHandler preparingToSendAutoReportEventHandler2;
			do
			{
				preparingToSendAutoReportEventHandler2 = preparingToSendAutoReportEventHandler;
				PreparingToSendAutoReportEventHandler value2 = (PreparingToSendAutoReportEventHandler)Delegate.Combine(preparingToSendAutoReportEventHandler2, value);
				preparingToSendAutoReportEventHandler = Interlocked.CompareExchange(ref this.m__E001, value2, preparingToSendAutoReportEventHandler2);
			}
			while ((object)preparingToSendAutoReportEventHandler != preparingToSendAutoReportEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			PreparingToSendAutoReportEventHandler preparingToSendAutoReportEventHandler = this.m__E001;
			PreparingToSendAutoReportEventHandler preparingToSendAutoReportEventHandler2;
			do
			{
				preparingToSendAutoReportEventHandler2 = preparingToSendAutoReportEventHandler;
				PreparingToSendAutoReportEventHandler value2 = (PreparingToSendAutoReportEventHandler)Delegate.Remove(preparingToSendAutoReportEventHandler2, value);
				preparingToSendAutoReportEventHandler = Interlocked.CompareExchange(ref this.m__E001, value2, preparingToSendAutoReportEventHandler2);
			}
			while ((object)preparingToSendAutoReportEventHandler != preparingToSendAutoReportEventHandler2);
		}
	}

	public event EventHandler OnBugReportSendingStarted
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

	public event EventHandler<BugReportSendingArgs> OnBugReportSendingCompleted
	{
		[CompilerGenerated]
		add
		{
			EventHandler<BugReportSendingArgs> eventHandler = this.m__E003;
			EventHandler<BugReportSendingArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BugReportSendingArgs> value2 = (EventHandler<BugReportSendingArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<BugReportSendingArgs> eventHandler = this.m__E003;
			EventHandler<BugReportSendingArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BugReportSendingArgs> value2 = (EventHandler<BugReportSendingArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this.m__E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<BugReportSendingProgressArgs> OnBugReportSendingProgress
	{
		[CompilerGenerated]
		add
		{
			EventHandler<BugReportSendingProgressArgs> eventHandler = this._E004;
			EventHandler<BugReportSendingProgressArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BugReportSendingProgressArgs> value2 = (EventHandler<BugReportSendingProgressArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E004, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<BugReportSendingProgressArgs> eventHandler = this._E004;
			EventHandler<BugReportSendingProgressArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BugReportSendingProgressArgs> value2 = (EventHandler<BugReportSendingProgressArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref this._E004, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public BugReportService(ILogger<BugReportService> logger, LogOptions logOptions, AppConfig appConfig, ISettingsService settingsService, ISiteCommunicationService siteCommunicationService, IInformationCollectionService informationCollectionService, IGameService gameService, JsonSerializerSettings jsonSerializerSettings)
	{
		this.m__E005 = logger;
		this._E006 = logOptions;
		_appConfig = appConfig;
		_settingsService = settingsService;
		_E007 = siteCommunicationService;
		_E008 = informationCollectionService;
		_E009 = gameService;
		_E00A = jsonSerializerSettings;
	}

	public void OnAwake()
	{
		_E009.OnGameClosedAsync += _E000;
	}

	public void OnStop()
	{
		_E009.OnGameClosedAsync -= _E000;
	}

	private async Task _E000(ProcessLifecycleInformation processLifecycleInfo)
	{
		if (processLifecycleInfo.ExitCode != 0 && processLifecycleInfo.ExitCode != 1)
		{
			bool sendAutoReport = true;
			this.m__E001?.Invoke(ref sendAutoReport);
			if (sendAutoReport)
			{
				await SendGameCrashReport(processLifecycleInfo.ExitCode);
			}
		}
	}

	public async Task<BugReportSendingResult> SendBugReport(string categoryId, string message, IEnumerable<string> attachedFiles, TimeSpan gameLogsFreshness, int gameLogsSizeLimit, bool attachLauncherLogs, bool collectServersAvailabilityInfo)
	{
		if (Interlocked.CompareExchange(ref _E00B, 1, 0) != 0)
		{
			this.m__E005.LogError(_E05B._E000(11077));
			throw new BugReportServiceException(BsgExceptionCode.TheBugReportCanNotBeSentBecauseTheBugReportServiceIsBusy);
		}
		bool isSendingSuccessful = false;
		BugReport bugReport = null;
		try
		{
			this._E002?.Invoke(this, EventArgs.Empty);
			List<string> gameLogDirectories = _E001(gameLogsFreshness).ToList();
			BugReport bugReport2 = new BugReport
			{
				CategoryId = categoryId,
				Message = message,
				SystemInfo = _E008.GetSystemInfo(),
				GameVersion = _E009.GameVersion,
				AttachedFiles = attachedFiles,
				GameLogDirectories = gameLogDirectories,
				LauncherLogFiles = (attachLauncherLogs ? _E000() : null)
			};
			BugReport bugReport3 = bugReport2;
			IReadOnlyCollection<ServerAvailabilityInfo> readOnlyCollection = (IReadOnlyCollection<ServerAvailabilityInfo>)(bugReport3.ServersAvailabilityInfo = ((!collectServersAvailabilityInfo) ? null : (await _E008.GetServersAvailabilityInfo(delegate(int progress)
			{
				this._E004?.Invoke(this, new BugReportSendingProgressArgs(BugReportSendingState.CollectingServerAvailabilityInfo, (int)((float)progress * 0.7f)));
			}))));
			bugReport = bugReport2;
			if (bugReport.CalculateSize(gameLogsSizeLimit, _E00A, this.m__E005) > _settingsService.MaxBugReportSize)
			{
				throw new SiteCommunicationServiceException(BsgExceptionCode.BugReportCannotBeSentDueToTooLargeSize);
			}
			IEnumerable<string> sids;
			try
			{
				sids = _E000(gameLogDirectories);
			}
			catch (Exception exc)
			{
				this.m__E005.Exception(exc, _E05B._E000(11198));
				sids = Enumerable.Empty<string>();
			}
			BugReportSendingResult result = await _E007.SendBugReport(bugReport, gameLogsSizeLimit, sids, delegate(int progress)
			{
				this._E004?.Invoke(this, new BugReportSendingProgressArgs(BugReportSendingState.SendingReport, collectServersAvailabilityInfo ? (70 + (int)((float)progress * 0.3f)) : progress));
			});
			this.m__E005.LogInformation(_E05B._E000(11155));
			isSendingSuccessful = true;
			return result;
		}
		catch (Exception exc2)
		{
			this.m__E005.Exception(exc2, _E05B._E000(11246));
			throw;
		}
		finally
		{
			_E00B = 0;
			this.m__E003?.Invoke(this, new BugReportSendingArgs(bugReport, isSendingSuccessful));
		}
	}

	public Task SendGameCrashReport(int exitCode)
	{
		try
		{
			string[] array = _E000(TimeSpan.FromMinutes(1.0)).Take(1).ToArray();
			CrashReport crashReport = new CrashReport
			{
				CreationTime = DateTime.Now,
				GameVersion = _E009.GameVersion,
				ExitCode = exitCode,
				HaveDump = array.SelectMany((string cdd) => Directory.EnumerateFiles(cdd, _E05B._E000(11205), SearchOption.AllDirectories)).Any(),
				SystemInfo = _E008.GetSystemInfo(),
				AttachedDirectories = array.Concat(_E001(TimeSpan.FromDays(1.0)))
			};
			return _E007.SendGameCrashReport(crashReport);
		}
		catch (Exception exc)
		{
			this.m__E005.Exception(exc, _E05B._E000(11476));
			return Task.CompletedTask;
		}
	}

	public long CalculateBugReportSize(IEnumerable<string> attachedFiles, TimeSpan gameLogsFreshness, int gameLogsSizeLimit, bool attachLauncherLogs)
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		Interlocked.Exchange(ref _E00C, cancellationTokenSource)?.Cancel();
		return new BugReport
		{
			AttachedFiles = attachedFiles,
			GameLogDirectories = _E001(gameLogsFreshness),
			LauncherLogFiles = (attachLauncherLogs ? _E000() : null)
		}.CalculateSize(gameLogsSizeLimit, _E00A, this.m__E005, cancellationTokenSource.Token) + 5120;
	}

	private IEnumerable<string> _E000(TimeSpan freshness)
	{
		string path = Path.Combine(Path.GetTempPath(), _appConfig.AppPublisher, _appConfig.GameShortName, _E05B._E000(11230));
		if (!Directory.Exists(path))
		{
			yield break;
		}
		foreach (string item in from d in Directory.EnumerateDirectories(path)
			where DateTime.Now - new DirectoryInfo(d).CreationTime < freshness
			select d)
		{
			yield return item;
		}
	}

	private IEnumerable<string> _E001(TimeSpan freshness)
	{
		if (!_E009.CheckGameIsInstalled())
		{
			this.m__E005.LogDebug(_E05B._E000(10790));
			yield break;
		}
		if (!Directory.Exists(_settingsService.SelectedBranch.GameRootDir))
		{
			this.m__E005.LogDebug(_E05B._E000(10770), _settingsService.SelectedBranch.GameRootDir);
			yield break;
		}
		string text = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _E05B._E000(10817), _E05B._E000(10843));
		if (File.Exists(text))
		{
			yield return text;
		}
		string path = Path.Combine(_settingsService.SelectedBranch.GameRootDir, _E05B._E000(25496));
		if (!Directory.Exists(path))
		{
			yield break;
		}
		foreach (string item in from d in Directory.EnumerateDirectories(path)
			orderby new DirectoryInfo(d).CreationTime descending
			where DateTime.Now - new DirectoryInfo(d).CreationTime < freshness
			select d)
		{
			yield return item;
		}
	}

	private IEnumerable<string> _E000(IEnumerable<string> gameLogDirectories)
	{
		return from m in gameLogDirectories.SelectMany((string gld) => Directory.EnumerateFiles(gld, _E05B._E000(11215), SearchOption.TopDirectoryOnly)).SelectMany((string log) => BugReportService.m__E000.Matches(File.ReadAllText(log)).Cast<Match>())
			select m.Value;
	}

	private IEnumerable<string> _E000()
	{
		if (Directory.Exists(this._E006.FileLogOptions.LogsDirectory))
		{
			return Directory.EnumerateFiles(this._E006.FileLogOptions.LogsDirectory, _E05B._E000(12108), SearchOption.TopDirectoryOnly);
		}
		this.m__E005.LogDebug(_E05B._E000(11064), this._E006.FileLogOptions.LogsDirectory);
		return Enumerable.Empty<string>();
	}
}
