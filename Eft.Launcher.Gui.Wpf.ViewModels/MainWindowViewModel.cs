using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Shell;
using Bsg.Launcher.Queues;
using Bsg.Launcher.Services.AuthService;
using Bsg.Launcher.Updating;
using Bsg.Launcher.Utils;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.BugReportService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.DownloadService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class MainWindowViewModel : WindowViewModelBase
{
	private enum ConsistencyCheckingMode
	{
		SkipChecking,
		FullChecking,
		FastChecking
	}

	[CompilerGenerated]
	private new sealed class _E001
	{
		public MainWindowViewModel _E000;

		public GameUpdateSet e;

		internal void _E000(object _)
		{
			Thread.Sleep(50);
			this._E000.m__E004.SetGameUpdateVersion(e.NewVersion);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public MainWindowViewModel _E000;

		public GameUpdateSet checkForUpdateResult;

		internal async Task _E000(ControlledQueueToken token)
		{
			if (this._E000._settingsService.GameAutoUpdateEnabled && this._E000.m__E019.GameState == GameState.UpdateRequired)
			{
				await this._E000.UpdateGameIfRequired(checkForUpdateResult, token);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00A
	{
		public ControlledQueueToken _E000;
	}

	[CompilerGenerated]
	private sealed class _E00E
	{
		public ControlledQueueToken _E000;

		public MainWindowViewModel _E001;

		internal void _E000()
		{
			_E00F CS_0024_003C_003E8__locals0 = new _E00F
			{
				_E001 = this,
				_E000 = new InstallationWindow(_E001.m__E00F)
				{
					Owner = Application.Current.MainWindow
				}
			};
			if (CS_0024_003C_003E8__locals0._E000.ShowDialog() != true)
			{
				return;
			}
			ThreadPool.QueueUserWorkItem(async delegate
			{
				_ = 2;
				try
				{
					CS_0024_003C_003E8__locals0._E001._E001._gameUpdateService.OnStateChanged += OnStateChanged;
					await CS_0024_003C_003E8__locals0._E001._E001._gameUpdateService.DownloadAndInstallGame(CS_0024_003C_003E8__locals0._E000.InstallationPath, CS_0024_003C_003E8__locals0._E000.DistribResponseData);
					await CS_0024_003C_003E8__locals0._E001._E001._E001(ConsistencyCheckingMode.FullChecking);
				}
				catch (Win32Exception)
				{
					CS_0024_003C_003E8__locals0._E001._E001.Logger.LogWarning(_E05B._E000(32051));
				}
				catch (OperationCanceledException)
				{
					CS_0024_003C_003E8__locals0._E001._E001.Logger.LogWarning(_E05B._E000(32072));
				}
				catch (Exception ex3)
				{
					CS_0024_003C_003E8__locals0._E001._E001.Logger.LogError(ex3, _E05B._E000(32174));
					await CS_0024_003C_003E8__locals0._E001._E001.m__E002.ShowException(ex3);
					CS_0024_003C_003E8__locals0._E001._E001.m__E019.UpdateGameIsInstalled();
				}
				finally
				{
					CS_0024_003C_003E8__locals0._E001._E001._gameUpdateService.OnStateChanged -= OnStateChanged;
				}
			});
			void OnStateChanged(object sender, GameUpdateServiceState e)
			{
				this._E000.Release();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00F
	{
		public InstallationWindow _E000;

		public _E00E _E001;

		internal async void _E000(object s)
		{
			_ = 2;
			try
			{
				_E001._E001._gameUpdateService.OnStateChanged += OnStateChanged;
				await _E001._E001._gameUpdateService.DownloadAndInstallGame(this._E000.InstallationPath, this._E000.DistribResponseData);
				await _E001._E001._E001(ConsistencyCheckingMode.FullChecking);
			}
			catch (Win32Exception)
			{
				_E001._E001.Logger.LogWarning(_E05B._E000(32051));
			}
			catch (OperationCanceledException)
			{
				_E001._E001.Logger.LogWarning(_E05B._E000(32072));
			}
			catch (Exception ex3)
			{
				_E001._E001.Logger.LogError(ex3, _E05B._E000(32174));
				await _E001._E001.m__E002.ShowException(ex3);
				_E001._E001.m__E019.UpdateGameIsInstalled();
			}
			finally
			{
				_E001._E001._gameUpdateService.OnStateChanged -= OnStateChanged;
			}
			void OnStateChanged(object sender, GameUpdateServiceState e)
			{
				((_E00E)this)._E000.Release();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E010
	{
		public LauncherUpdate launcherUpdate;

		public MainWindowViewModel _E000;

		internal async Task _E000(ControlledQueueToken token)
		{
			launcherUpdate = await this._E000._launcherUpdateService.CheckForLauncherUpdate();
			if (launcherUpdate.UpdateIsRequired)
			{
				await this._E000._launcherUpdateService.BeginUpdateLauncher(launcherUpdate);
				if (await this._E000.m__E004.RequestRestartForUpdate())
				{
					this._E000._launcherUpdateService.EndUpdateLauncher(launcherUpdate);
				}
				this._E000.m__E004.Close();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E011
	{
		public ControlledQueueToken _E000;
	}

	[CompilerGenerated]
	private sealed class _E012
	{
		public bool _E000;

		internal void _E000(bool isInTheQueue)
		{
			this._E000 = true;
		}
	}

	[CompilerGenerated]
	private sealed class _E017
	{
		public MainWindowViewModel _E000;

		public bool _E001;

		internal async void _E000(object w)
		{
			try
			{
				this._E000._consistencyService.CheckConsistency(_E001);
			}
			catch (Exception ex)
			{
				this._E000.Logger.LogError(ex, _E05B._E000(31764));
				await this._E000.m__E002.ShowException(ex);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E018
	{
		public string _E000;

		internal bool _003CSelectBranch_003Eb__0(IBranch b)
		{
			return b.Name == _E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E019
	{
		public MainWindowViewModel _E000;

		public string _E001;

		internal async void _E000(object w)
		{
			try
			{
				await this._E000.m__E002.ShowLicenseAgreementWindow(_E001);
			}
			catch (Exception ex)
			{
				this._E000.Logger.LogError(ex, _E05B._E000(31864));
				await this._E000.m__E002.ShowException(ex);
			}
		}
	}

	private readonly IServiceProvider m__E00F;

	private readonly ISettingsService _settingsService;

	private readonly ILauncherUpdateService _launcherUpdateService;

	private readonly IGameUpdateService _gameUpdateService;

	private readonly IGameService m__E019;

	private readonly ILauncherBackendService m__E012;

	private readonly IGameBackendService m__E006;

	private readonly IAuthService _E014;

	private readonly IConsistencyService _consistencyService;

	private readonly IFileManager _E01A;

	private readonly IDownloadService _downloadService;

	private readonly ISiteCommunicationService m__E007;

	private readonly IAccessService m__E011;

	private readonly IDialogService m__E002;

	private new readonly IBugReportService m__E001;

	private readonly IUpdateManager _E01B;

	private readonly IQueueHandler _E01C;

	private readonly int[] _E015 = new int[5] { 0, 5, 15, 30, 60 };

	private readonly System.Timers.Timer _E01D;

	private int _E01E;

	private bool _E01F;

	private IMainWindowDelegate m__E004;

	private bool _E020;

	private CancellationTokenSource _E021;

	public MainWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		this.m__E00F = serviceProvider;
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_launcherUpdateService = serviceProvider.GetRequiredService<ILauncherUpdateService>();
		_gameUpdateService = serviceProvider.GetRequiredService<IGameUpdateService>();
		this.m__E019 = serviceProvider.GetRequiredService<IGameService>();
		this.m__E012 = serviceProvider.GetRequiredService<ILauncherBackendService>();
		this.m__E006 = serviceProvider.GetRequiredService<IGameBackendService>();
		this._E014 = serviceProvider.GetRequiredService<IAuthService>();
		_consistencyService = serviceProvider.GetRequiredService<IConsistencyService>();
		this._E01A = serviceProvider.GetRequiredService<IFileManager>();
		_downloadService = serviceProvider.GetRequiredService<IDownloadService>();
		this.m__E007 = serviceProvider.GetRequiredService<ISiteCommunicationService>();
		this.m__E011 = serviceProvider.GetRequiredService<IAccessService>();
		this.m__E002 = serviceProvider.GetRequiredService<IDialogService>();
		this.m__E001 = serviceProvider.GetRequiredService<IBugReportService>();
		this._E01B = serviceProvider.GetRequiredService<IUpdateManager>();
		this._E01C = serviceProvider.GetRequiredService<IQueueHandler>();
		this._E014.OnLoggedOut += _E000;
		this._E014.OnAuthorizationError += _E000;
		this._E01A.OnFileIsUsedByAnotherProcess += _E000;
		double num = (double)new Random().Next(90, 110) * 0.01;
		this._E01D = new System.Timers.Timer((double)(_settingsService.UpdateCheckInterval * 1000) * num);
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		this.m__E004 = (IMainWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await Task.Run((Func<Task>)this._E000);
		this._E01D.Start();
	}

	protected override void SubscribeServices()
	{
		base.SubscribeServices();
		_gameUpdateService.OnStateChanged += OnGameUpdateStateChanged;
		_gameUpdateService.OnProgress += this.m__E004.SetGameUpdateProgress;
		_gameUpdateService.OnUpdateChecked += OnGameUpdateChecked;
		this.m__E019.OnGameStateChanged += _E000;
		this.m__E019.OnGameStarted += _E000;
		this.m__E019.OnGameVersionChanged += _E000;
		this.m__E019.OnGameClosedAsync += _E000;
		this._E01C.OnQueueProgress += _E000;
		_settingsService.OnBranchChanged += OnBranchChanged;
		_settingsService.OnUserProfileLoaded += _E001;
		_settingsService.OnSettingsUpdated += _E001;
		_consistencyService.OnConsistencyCheckingProgress += _E000;
		_consistencyService.OnRepairProgress += _E001;
		_downloadService.OnSlowConnectionDetected += OnSlowConnectionDetected;
		this.m__E001.OnPreparingToSendAutoReport += _E000;
		this._E01B.OnUpdateInstallationProblem += _E000;
		this._E01D.Elapsed += _E000;
	}

	protected override void UnsubscribeServices()
	{
		base.UnsubscribeServices();
		_gameUpdateService.OnStateChanged -= OnGameUpdateStateChanged;
		_gameUpdateService.OnProgress -= this.m__E004.SetGameUpdateProgress;
		_gameUpdateService.OnUpdateChecked -= OnGameUpdateChecked;
		this.m__E019.OnGameStateChanged -= _E000;
		this.m__E019.OnGameStarted -= _E000;
		this.m__E019.OnGameVersionChanged -= _E000;
		this.m__E019.OnGameClosedAsync -= _E000;
		this._E014.OnLoggedOut -= _E000;
		this._E014.OnAuthorizationError -= _E000;
		_settingsService.OnBranchChanged -= OnBranchChanged;
		_settingsService.OnUserProfileLoaded -= _E001;
		_settingsService.OnSettingsUpdated -= _E001;
		_consistencyService.OnConsistencyCheckingProgress -= _E000;
		_consistencyService.OnRepairProgress -= _E001;
		this._E01A.OnFileIsUsedByAnotherProcess -= _E000;
		_downloadService.OnSlowConnectionDetected -= OnSlowConnectionDetected;
		this.m__E001.OnPreparingToSendAutoReport -= _E000;
		this._E01B.OnUpdateInstallationProblem -= _E000;
		this._E01D.Elapsed -= _E000;
	}

	private void _E000()
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			LoginWindow loginWindow = new LoginWindow(this.m__E00F);
			loginWindow.Show();
			Application.Current.MainWindow = loginWindow;
		});
		base.Close();
	}

	private void OnGameUpdateStateChanged(object sender, GameUpdateServiceState newState)
	{
		switch (newState)
		{
		case GameUpdateServiceState.CheckingForUpdate:
		case GameUpdateServiceState.CheckingUpdateHash:
			this.m__E004.SetTaskbarState(TaskbarItemProgressState.Indeterminate);
			break;
		case GameUpdateServiceState.Idle:
			this.m__E004.SetTaskbarState(TaskbarItemProgressState.None);
			break;
		case GameUpdateServiceState.DownloadingUpdate:
		case GameUpdateServiceState.InstallingUpdate:
		case GameUpdateServiceState.RepairingGame:
		case GameUpdateServiceState.ConsistencyChecking:
		case GameUpdateServiceState.CreatingUpdate:
			this.m__E004.SetTaskbarState(TaskbarItemProgressState.Normal);
			break;
		case GameUpdateServiceState.Pause:
			this.m__E004.SetTaskbarState(TaskbarItemProgressState.Paused);
			break;
		case GameUpdateServiceState.UpdateError:
			this.m__E004.SetTaskbarState(TaskbarItemProgressState.Error);
			break;
		}
		if (this.m__E019.GameState != GameState.PreparingForGame)
		{
			this.m__E004.SetGameUpdateState(newState);
		}
	}

	private void OnGameUpdateChecked(object sender, GameUpdateSet e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			Thread.Sleep(50);
			this.m__E004.SetGameUpdateVersion(e.NewVersion);
		});
	}

	private void _E000(object sender, GameStateChangedEventArgs eventArgs)
	{
		this.m__E004.SetGameState(eventArgs.NewState);
		if (eventArgs.NewState == GameState.InGame)
		{
			switch (_settingsService.GameStartBehavior)
			{
			case GameStartBehavior.CollapseLauncherToTray:
				this.m__E004.CollapseToTray();
				break;
			case GameStartBehavior.MinimizeLauncher:
				this.m__E004.SetWindowState(WindowState.Minimized);
				break;
			case GameStartBehavior.DoNotCloseLauncher:
			case GameStartBehavior.CloseLauncher:
				break;
			}
		}
	}

	private void _E000(Process gameProcess)
	{
		if (_settingsService.GameStartBehavior == GameStartBehavior.CloseLauncher)
		{
			this.m__E004.Close();
		}
	}

	private void _E000(object sender, BsgVersion version)
	{
		this.m__E004.SetGameVersion(version);
	}

	private async void OnBranchChanged(object sender, OnBranchChangedEventArgs eArgs)
	{
		IBranch oldBranch = eArgs.OldBranch;
		if (oldBranch != null && oldBranch.BranchParticipationStatus == BranchParticipationStatus.Suspended)
		{
			await this.m__E002.ShowDialog(DialogWindowMessage.YourParticipationInBranchHasBeenRevoked, eArgs.OldBranch.Name);
		}
		if (this._E01F)
		{
			try
			{
				await _E000(ConsistencyCheckingMode.FastChecking);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(28963));
				await this.m__E002.ShowException(ex);
			}
		}
	}

	private void _E000(object sender, IProgressReport newProgress)
	{
		this.m__E004.SetGameUpdateProgress((long)newProgress.BytesTransferred, (long)newProgress.FileSize);
	}

	private void _E001(object sender, IProgressReport newProgress)
	{
		this.m__E004.SetGameUpdateProgress((long)newProgress.BytesTransferred, (long)newProgress.FileSize);
	}

	private async void _E000(string file, HandledEventArgs eventArgs)
	{
		bool flag = !eventArgs.Handled;
		if (flag)
		{
			flag = await this.m__E002.ShowDialog(DialogWindowMessage.FileUsedByAnotherProcessAndCannotBeUpdated, file) == DialogResult.Positive;
		}
		if (flag)
		{
			eventArgs.Handled = true;
		}
	}

	private void OnSlowConnectionDetected(SlowConnectionEventArgs obj)
	{
		obj.ResetConnection = this.m__E002.ShowDialog(DialogWindowMessage.TooSlowConnectionFileCantBeDownloaded, obj.Uri.AbsoluteUri).Result == DialogResult.Negative;
	}

	private void _E000(ref bool sendBugReport)
	{
		sendBugReport = this.m__E002.ShowDialog(DialogWindowMessage.DoYouWantToSendBugReportAfterGameCrash).Result == DialogResult.Positive;
	}

	private void _E000(Exception exc, ref bool retry)
	{
		retry = this.m__E002.ShowDialog(DialogWindowMessage.InstallationProblem, exc).Result == DialogResult.Positive;
	}

	private async Task _E000(ProcessLifecycleInformation pli)
	{
		this.m__E004.RestoreWindow();
		if (_settingsService.SelectedBranch.FeedbackBehavior == FeedbackBehavior.Required)
		{
			try
			{
				string feedbackFormData = await this.m__E007.GetFeedbackFormData();
				await this.m__E002.ShowFeedbackWindow(feedbackFormData);
			}
			catch
			{
			}
		}
	}

	private void _E000(int position, int estimatedWaitingTime)
	{
		this.m__E004.SetQueueValues(position, estimatedWaitingTime);
	}

	private void _E000(object sender, EventArgs e)
	{
		this._E000();
	}

	private void _E000(AuthServiceException exception)
	{
		this.m__E002.ShowException(exception).Wait();
	}

	private async void _E000(object sender, ElapsedEventArgs eArgs)
	{
		try
		{
			if (_launcherUpdateService.State != 0 || this.m__E019.GameState != GameState.ReadyToGame || _gameUpdateService.State != 0 || !this.m__E019.CheckGameIsInstalled())
			{
				return;
			}
			GameUpdateSet checkForUpdateResult = await _gameUpdateService.CheckForGameUpdate();
			await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
			{
				if (_settingsService.GameAutoUpdateEnabled && this.m__E019.GameState == GameState.UpdateRequired)
				{
					await UpdateGameIfRequired(checkForUpdateResult, token);
				}
			}, _E05B._E000(28947));
		}
		catch (NetworkException ex)
		{
			string text = (ex as ApiNetworkException)?.ApiCode.ToString() ?? (ex as HttpNetworkException)?.Response?.StatusCode.ToString() ?? _E05B._E000(27102);
			Logger.LogWarning(ex, _E05B._E000(29038), ex.GetType().Name, text, ex.Message);
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(29117));
			await this.m__E002.ShowException(ex2);
		}
	}

	protected override void OnClosing()
	{
		this._E01D.Stop();
		this._E021?.Cancel();
		base.OnClosing();
	}

	private async Task _E000()
	{
		try
		{
			_launcherUpdateService.CheckForStartAfterUpdate();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29152));
		}
		if (!this.m__E004.StartedFromLoginPage)
		{
			try
			{
				LauncherUpdate launcherCheckForUpdateResult = await _launcherUpdateService.CheckForLauncherUpdate();
				if (launcherCheckForUpdateResult.UpdateIsRequired)
				{
					await _launcherUpdateService.BeginUpdateLauncher(launcherCheckForUpdateResult);
					if (await this.m__E004.RequestRestartForUpdate())
					{
						_launcherUpdateService.EndUpdateLauncher(launcherCheckForUpdateResult);
					}
					this.m__E004.Close();
					return;
				}
			}
			catch (NetworkException ex) when (ex.IsLocalProblems)
			{
				Logger.LogWarning(_E05B._E000(29590));
			}
			catch (Exception exception2)
			{
				Logger.LogError(exception2, _E05B._E000(29651));
			}
		}
		try
		{
			using (MD5.Create())
			{
			}
		}
		catch (TargetInvocationException ex2)
		{
			Logger.LogWarning(_E05B._E000(29125), ex2.Message);
		}
		catch (Exception exception3)
		{
			Logger.LogError(exception3, _E05B._E000(28706));
		}
		try
		{
			int[] array = new int[5] { 0, 5, 15, 30, 60 };
			int num = 0;
			UserProfile userProfile;
			while (true)
			{
				try
				{
					if (await this._E014.LoginBySavedToken())
					{
						userProfile = await LoadProfiles();
						break;
					}
					this._E000();
					return;
				}
				catch (NetworkException ex3) when (ex3.IsLocalProblems)
				{
					Logger.LogWarning(_E05B._E000(28726));
					int num2 = array[Math.Min(num, array.Length - 1)];
					if (await this.m__E002.ShowDialog(DialogWindowMessage.FailedToEstablishBackendConnection, num2.ToString()) != DialogResult.Positive)
					{
						base.Close();
						return;
					}
					goto IL_0501;
				}
				IL_0501:
				num++;
			}
			if (!(userProfile.IsLicenseAgreementAlreadyAccepted ?? (!(await this.m__E007.CheckIfLicenseAgreementIsRequired()))))
			{
				do
				{
					if (await this.m__E002.ShowLicenseAgreementWindow() != DialogResult.Positive)
					{
						await this._E014.LogOut();
						return;
					}
				}
				while (await this.m__E007.CheckIfLicenseAgreementIsRequired());
			}
			await this.m__E004.LoadAsync(_settingsService.MainPageUri.ToString());
			if (_settingsService.IsInTheGameInstallationProgress)
			{
				Logger.LogWarning(_E05B._E000(28774));
			}
			try
			{
				await _E000(_settingsService.IsInTheGameInstallationProgress ? ConsistencyCheckingMode.FullChecking : ConsistencyCheckingMode.FastChecking);
			}
			catch (ConsistencyServiceException ex4)
			{
				Logger.LogWarning(ex4, _E05B._E000(28846));
				await this.m__E002.ShowException(ex4);
			}
			catch (NetworkException exc2)
			{
				Logger.LogWarning(_E05B._E000(28900));
				await this.m__E002.ShowException(exc2);
			}
			this._E01F = true;
			try
			{
				await this.m__E007.SendSystemInfo();
			}
			catch (Exception exception4)
			{
				Logger.LogError(exception4, _E05B._E000(28894));
			}
		}
		catch (UnauthorizedApiNetworkException)
		{
			Logger.LogInformation(_E05B._E000(32564));
		}
		catch (NetworkException ex6) when (ex6.IsLocalProblems)
		{
			Logger.LogWarning(_E05B._E000(32609));
			await this.m__E002.ShowException(ex6);
			base.Close();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, _E05B._E000(32579));
			await this.m__E002.ShowException(exc);
			if (exc is SettingsServiceException)
			{
				Logout();
			}
			base.Close();
		}
	}

	private Task _E000(ConsistencyCheckingMode consistencyCheckingMode)
	{
		return Task.WhenAll(this._E001(), _E001(consistencyCheckingMode));
	}

	private async Task _E001()
	{
		try
		{
			DatacenterDto[] datacenters = await this.m__E006.GetDatacenters();
			this.m__E004.SetCurrentGameServers(_settingsService.IpRegion, datacenters);
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(32598));
		}
	}

	private async Task _E001(ConsistencyCheckingMode consistencyCheckingMode)
	{
		this.m__E019.Init();
		bool flag = this.m__E019.CheckGameIsInstalled();
		if (flag)
		{
			Logger.LogDebug(_E05B._E000(32640), _settingsService.SelectedBranch.GameRootDir);
		}
		else
		{
			Logger.LogDebug(_E05B._E000(32738));
		}
		this.m__E019.UpdateGameVersion();
		if (this.m__E019.GameVersion != default(BsgVersion))
		{
			Logger.LogDebug(_E05B._E000(32764), this.m__E019.GameVersion);
		}
		this.m__E019.UpdateGameIsInstalled();
		if (flag)
		{
			if (this.m__E019.GameState == GameState.InGame)
			{
				return;
			}
			if (consistencyCheckingMode != 0)
			{
				if (!_consistencyService.CheckConsistency(consistencyCheckingMode == ConsistencyCheckingMode.FullChecking).IsSuccess)
				{
					return;
				}
				if (_settingsService.IsInTheGameInstallationProgress)
				{
					_settingsService.IsInTheGameInstallationProgress = false;
					_settingsService.Save();
				}
			}
			GameUpdateSet gameUpdateSet = await _gameUpdateService.CheckForGameUpdate(fastChecking: true);
			if (gameUpdateSet.UpdateNecessity == UpdateNecessity.UpdateNotRequired)
			{
				return;
			}
			if (_settingsService.GameAutoUpdateEnabled)
			{
				if (this.m__E019.GameState == GameState.UpdateRequired)
				{
					await UpdateGameIfRequired(gameUpdateSet, null);
					return;
				}
				Logger.LogWarning(_E05B._E000(32735), gameUpdateSet.UpdateNecessity, this.m__E019.GameState);
			}
			else if (gameUpdateSet.UpdateNecessity == UpdateNecessity.UpdateRequired)
			{
				await _gameUpdateService.TryResumeLastGameUpdateSetInstallation();
			}
			else if (gameUpdateSet.UpdateNecessity == UpdateNecessity.ReinstallRequired)
			{
				await _gameUpdateService.TryResumeLastGameInstallation();
			}
		}
		else
		{
			await _gameUpdateService.TryResumeLastGameInstallation();
		}
	}

	private async Task UpdateGameIfRequired(GameUpdateSet gameUpdateSet, ControlledQueueToken token)
	{
		if (gameUpdateSet.UpdateNecessity == UpdateNecessity.UpdateNotRequired)
		{
			return;
		}
		if (this._E020)
		{
			Logger.LogWarning(_E05B._E000(32339));
			return;
		}
		try
		{
			this._E020 = true;
			this.m__E019.CloseGame();
			bool fullCheck = false;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			_gameUpdateService.OnStateChanged += OnStateChanged;
			try
			{
				if (gameUpdateSet.UpdateNecessity != 0)
				{
					if (gameUpdateSet.UpdateNecessity == UpdateNecessity.ReinstallRequired)
					{
						Logger.LogWarning(_E05B._E000(32429), this.m__E019.GameVersion);
						GamePackageInfo gamePackageInfo = await this.m__E012.GetGamePackage();
						if (gamePackageInfo == null)
						{
							throw new Exception(_E05B._E000(26653));
						}
						await _gameUpdateService.DownloadAndInstallGame(_settingsService.SelectedBranch.GameRootDir, gamePackageInfo);
					}
					else
					{
						fullCheck = gameUpdateSet.Updates.Any((GameUpdateInfo u) => u.FullConsistencyCheck);
						flag = gameUpdateSet.Updates.Any((GameUpdateInfo u) => u.ClearCache);
						flag2 = gameUpdateSet.Updates.Any((GameUpdateInfo u) => u.DeleteLocalIni);
						flag3 = gameUpdateSet.Updates.Any((GameUpdateInfo u) => u.DeleteSharedIni);
						if (await _gameUpdateService.DownloadAndApplyGameUpdateSet(gameUpdateSet) == InstallationResult.HasSkippedFiles)
						{
							fullCheck = true;
						}
					}
					await _gameUpdateService.CheckForGameUpdate(fastChecking: true);
					_consistencyService.CheckConsistency(fullCheck);
				}
			}
			catch (OperationCanceledException)
			{
				Logger.LogInformation(_E05B._E000(32496));
			}
			catch (Win32Exception)
			{
				Logger.LogWarning(_E05B._E000(32463));
			}
			finally
			{
				_gameUpdateService.OnStateChanged -= OnStateChanged;
			}
			if (flag)
			{
				this.m__E019.ClearCache();
			}
			if (flag2)
			{
				this.m__E019.DeleteLocalIni();
			}
			if (flag3)
			{
				this.m__E019.DeleteSharedIni();
			}
		}
		finally
		{
			this._E020 = false;
		}
		void OnStateChanged(object sender, GameUpdateServiceState e)
		{
			token?.Release();
		}
	}

	private async Task<bool> _E000()
	{
		string text = (string.IsNullOrWhiteSpace(_settingsService.SelectedBranch.GameRootDir) ? _settingsService.SelectedBranch.DefaultGameRootDir : _settingsService.SelectedBranch.GameRootDir);
		while (true)
		{
			string text2;
			text = (text2 = await ShowSelectGameFolderDialog(text));
			if (text2 == null)
			{
				break;
			}
			if (this.m__E019.CheckGameIsInstalled(text))
			{
				_consistencyService.CancelCurrentOperation();
				try
				{
					await this.m__E011.AssignFullPermissions(text);
					_settingsService.SelectedBranch.GameRootDir = text;
					this.m__E019.UpdateInstallationInfo();
				}
				catch (Win32Exception)
				{
					break;
				}
				return true;
			}
			await this.m__E002.ShowDialog(DialogWindowMessage.IsNotGameDirectory, text);
		}
		return false;
	}

	private async Task _E000(bool deleteRootFolder = false)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(_settingsService.LauncherTempDir);
		bool flag = directoryInfo.Exists;
		if (flag)
		{
			flag = await this.m__E002.ShowDialog(DialogWindowMessage.DoYouWantToClearDirectory, directoryInfo.FullName) == DialogResult.Positive;
		}
		if (flag)
		{
			Directory.Delete(_settingsService.LauncherTempDir, recursive: true);
			if (!deleteRootFolder)
			{
				Directory.CreateDirectory(_settingsService.LauncherTempDir);
			}
		}
	}

	private void _E001(object sender, EventArgs e)
	{
		this.m__E004.SetLauncherVersion(_launcherUpdateService.Metadata.LauncherVersion);
		this.m__E004.SetGameEdition(_settingsService.GameEdition);
		this.m__E004.SetGameRegion(_settingsService.UserRegion);
	}

	[DebuggerHidden]
	public override void Close()
	{
		LogJsDotNetCall();
		try
		{
			if (_settingsService.CloseBehavior == CloseBehavior.MinimizeLauncher)
			{
				this.m__E004.CollapseToTray();
			}
			else if (_gameUpdateService.State == GameUpdateServiceState.InstallingUpdate)
			{
				Task<DialogResult> task = this.m__E002.ShowDialog(DialogWindowMessage.AbortTheInstallationProcess);
				task.Wait();
				if (task.Result == DialogResult.Positive)
				{
					base.Close();
				}
			}
			else
			{
				base.Close();
			}
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29242));
		}
	}

	[DebuggerHidden]
	public void Exit()
	{
		base.Close();
	}

	[DebuggerHidden]
	public void InstallGame()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
				{
					this.m__E019.CloseGame();
					await Application.Current.Dispatcher.BeginInvoke((Action)delegate
					{
						InstallationWindow installationWindow = new InstallationWindow(this.m__E00F)
						{
							Owner = Application.Current.MainWindow
						};
						if (installationWindow.ShowDialog() == true)
						{
							ThreadPool.QueueUserWorkItem(async delegate
							{
								_ = 2;
								try
								{
									_gameUpdateService.OnStateChanged += OnStateChanged;
									await _gameUpdateService.DownloadAndInstallGame(installationWindow.InstallationPath, installationWindow.DistribResponseData);
									await _E001(ConsistencyCheckingMode.FullChecking);
								}
								catch (Win32Exception)
								{
									Logger.LogWarning(_E05B._E000(32051));
								}
								catch (OperationCanceledException)
								{
									Logger.LogWarning(_E05B._E000(32072));
								}
								catch (Exception ex4)
								{
									Logger.LogError(ex4, _E05B._E000(32174));
									await this.m__E002.ShowException(ex4);
									this.m__E019.UpdateGameIsInstalled();
								}
								finally
								{
									_gameUpdateService.OnStateChanged -= OnStateChanged;
								}
							});
						}
					});
					void OnStateChanged(object sender, GameUpdateServiceState e)
					{
						token.Release();
					}
				}, _E05B._E000(31905));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(31917));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void RepairGame()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
				{
					this.m__E019.CloseGame();
					IConsistencyCheckingResult consistencyCheckingResult = _consistencyService.LastConsistencyCheckingResult;
					if (consistencyCheckingResult == null || !consistencyCheckingResult.IsFullCheck)
					{
						consistencyCheckingResult = _consistencyService.CheckConsistency(fullCheck: true);
					}
					await _consistencyService.Repair(consistencyCheckingResult);
					token.Release();
					await _E001(ConsistencyCheckingMode.SkipChecking);
				}, _E05B._E000(31899));
			}
			catch (OperationCanceledException)
			{
				Logger.LogWarning(_E05B._E000(31968));
			}
			catch (Exception ex2)
			{
				Logger.LogError(ex2, _E05B._E000(31943));
				await this.m__E002.ShowException(ex2);
			}
		});
	}

	[DebuggerHidden]
	public void CheckForUpdate()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			LauncherUpdate launcherUpdate = null;
			try
			{
				await ExecuteSingleQueueOperation(async delegate
				{
					launcherUpdate = await _launcherUpdateService.CheckForLauncherUpdate();
					if (launcherUpdate.UpdateIsRequired)
					{
						await _launcherUpdateService.BeginUpdateLauncher(launcherUpdate);
						if (await this.m__E004.RequestRestartForUpdate())
						{
							_launcherUpdateService.EndUpdateLauncher(launcherUpdate);
						}
						this.m__E004.Close();
					}
				}, _E05B._E000(31522));
			}
			catch (NetworkException ex) when (ex.IsLocalProblems)
			{
				Logger.LogWarning(_E05B._E000(29590));
				await this.m__E002.ShowException(ex);
			}
			catch (Exception ex2)
			{
				Logger.LogError(ex2, _E05B._E000(29651));
				await this.m__E002.ShowException(ex2);
			}
			if (launcherUpdate == null || !launcherUpdate.UpdateIsRequired)
			{
				try
				{
					await _gameUpdateService.CheckForGameUpdate();
				}
				catch (NetworkException ex3) when (ex3.IsLocalProblems)
				{
					Logger.LogWarning(_E05B._E000(31539));
					await this.m__E002.ShowException(ex3);
				}
				catch (Exception ex4)
				{
					Logger.LogError(ex4, _E05B._E000(31510));
					await this.m__E002.ShowException(ex4);
				}
			}
		});
	}

	[DebuggerHidden]
	public void UpdateGame()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
				{
					await UpdateGameIfRequired(await _gameUpdateService.CheckForGameUpdate(fastChecking: true), token);
				}, _E05B._E000(31602));
			}
			catch (Exception exc)
			{
				await this.m__E002.ShowException(exc);
			}
		});
	}

	[DebuggerHidden]
	public void PauseInstallation()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation((ControlledQueueToken token) => Task.Run((Action)_gameUpdateService.PauseInstallation), _E05B._E000(31615));
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Logger.LogError(ex2, _E05B._E000(31565));
				await this.m__E002.ShowException(ex2);
			}
		});
	}

	[DebuggerHidden]
	public void StopInstallation()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation((ControlledQueueToken token) => _gameUpdateService.StopInstallation(showCancellationDialog: true), _E05B._E000(31665));
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Logger.LogError(ex2, _E05B._E000(31616));
				await this.m__E002.ShowException(ex2);
			}
		});
	}

	[DebuggerHidden]
	public void ResumeInstallation()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
				{
					_gameUpdateService.OnStateChanged += OnStateChanged;
					try
					{
						await _gameUpdateService.ResumeInstallation();
					}
					finally
					{
						_gameUpdateService.OnStateChanged -= OnStateChanged;
					}
					void OnStateChanged(object sender, GameUpdateServiceState e)
					{
						token.Release();
					}
				}, _E05B._E000(31717));
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Logger.LogError(ex2, _E05B._E000(31738));
				await this.m__E002.ShowException(ex2);
			}
		});
	}

	[DebuggerHidden]
	public void StartGame()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate
				{
					bool flag = false;
					this._E021 = new CancellationTokenSource();
					this._E01C.OnQueueStateChanged += delegate
					{
						flag = true;
					};
					try
					{
						(string, int) tuple = await this.m__E019.PrepareGame(this._E021.Token);
						if (flag)
						{
							if (_settingsService.SoundQueueNotificationEnabled)
							{
								this.m__E004.PlaySound(_E05B._E000(31709));
							}
							if (_settingsService.QueueAutostartEnabled)
							{
								await this.m__E019.RunGame(tuple.Item1);
							}
							else
							{
								this.m__E004.RestoreWindow();
								this.m__E004.DrawMainButtonCountdown(tuple.Item2, disableMainButton: false);
							}
						}
						else
						{
							await this.m__E019.RunGame(tuple.Item1);
							this._E01E = 0;
						}
					}
					catch (OperationCanceledException)
					{
					}
					finally
					{
						this._E01C.OnQueueStateChanged -= delegate
						{
							flag = true;
						};
						this._E021?.Dispose();
						this._E021 = null;
					}
				}, _E05B._E000(31277));
			}
			catch (OperationCanceledException)
			{
				Logger.LogWarning(_E05B._E000(31291));
			}
			catch (UnauthorizedApiNetworkException)
			{
			}
			catch (NetworkException ex3)
			{
				this.m__E004.DrawMainButtonCountdown(this._E015[Math.Min(this._E01E, this._E015.Length - 1)], disableMainButton: true);
				this._E01E++;
				if (!(ex3 is ApiNetworkException ex4) || ex4.ApiCode != 232)
				{
					await this.m__E002.ShowException(ex3);
				}
				else
				{
					Logger.LogInformation(_E05B._E000(31254));
					if ((await _gameUpdateService.CheckForGameUpdate(fastChecking: true)).UpdateNecessity == UpdateNecessity.UpdateNotRequired)
					{
						await this.m__E002.ShowDialog(DialogWindowMessage.TheGameCannotBeStartedPleaseTryAgainLater);
					}
				}
			}
			catch (Exception ex5)
			{
				Logger.LogError(ex5, _E05B._E000(31318));
				await this.m__E002.ShowException(ex5);
			}
		});
	}

	[DebuggerHidden]
	public void CancelGameQueue()
	{
		LogJsDotNetCall();
		this._E021?.Cancel();
	}

	[DebuggerHidden]
	public async void Logout()
	{
		LogJsDotNetCall();
		try
		{
			if (_gameUpdateService.State != GameUpdateServiceState.InstallingUpdate)
			{
				await this._E014.LogOut();
			}
			else if (await this.m__E002.ShowDialog(DialogWindowMessage.AbortTheInstallationProcess) == DialogResult.Positive)
			{
				await this._E014.LogOut();
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(32143));
			await this.m__E002.ShowException(ex);
		}
	}

	[DebuggerHidden]
	public void SelectGameFolder()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate
				{
					this.m__E019.CloseGame();
					if (await this._E000())
					{
						await _E001(ConsistencyCheckingMode.FullChecking);
					}
				}, _E05B._E000(31408));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(26744));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void ShowMatchingConfig()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate
				{
					MatchingConfigurationWindow matchingConfigurationWindow = await MatchingConfigurationWindow.Create(this.m__E00F);
					if (await matchingConfigurationWindow.ShowDialog() == DialogResult.Positive)
					{
						await this.m__E006.SetMatchingConfiguration(matchingConfigurationWindow.MatchingConfiguration);
						await this._E001();
					}
				}, _E05B._E000(31367));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(31380));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public async void ShowLicenseAgreement()
	{
		LogJsDotNetCall();
		try
		{
			bool flag = !this.m__E002.IsLicenseAgreementWindowShowed;
			if (flag)
			{
				flag = await this.m__E002.ShowLicenseAgreementWindow() != DialogResult.Positive;
			}
			if (flag)
			{
				await this._E014.LogOut();
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(32230));
			await this.m__E002.ShowException(ex);
		}
	}

	[DebuggerHidden]
	public async void ShowCodeActivationWindow()
	{
		LogJsDotNetCall();
		try
		{
			await this.m__E002.ShowCodeActivationWindow();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(32209));
			await this.m__E002.ShowException(ex);
		}
	}

	[DebuggerHidden]
	public async void ShowFeedbackWindow()
	{
		LogJsDotNetCall();
		try
		{
			string feedbackFormData = await this.m__E007.GetFeedbackFormData();
			await this.m__E002.ShowFeedbackWindow(feedbackFormData);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(31800));
			await this.m__E002.ShowException(ex);
		}
	}

	[DebuggerHidden]
	public void CheckConsistency(bool fullCheck)
	{
		LogJsDotNetCall(fullCheck);
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				_consistencyService.CheckConsistency(fullCheck);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(31764));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void ClearCache()
	{
		LogJsDotNetCall();
		try
		{
			this.m__E019.ClearCache();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(29291));
			this.m__E002.ShowException(ex).Wait();
		}
	}

	[DebuggerHidden]
	public void SelectBranch(string branchName)
	{
		LogJsDotNetCall(branchName);
		try
		{
			IBranch branch = _settingsService.Branches.FirstOrDefault((IBranch b) => b.Name == branchName);
			if (branch != null)
			{
				_settingsService.SelectedBranch = branch;
				return;
			}
			Logger.LogWarning(_E05B._E000(29310), branchName);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(29415), branchName);
			this.m__E002.ShowException(ex).Wait();
		}
	}

	[DebuggerHidden]
	public void ShowLicenseAgreementWindow(string document)
	{
		LogJsDotNetCall(_E05B._E000(29385));
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await this.m__E002.ShowLicenseAgreementWindow(document);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(31864));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void SelectTempFolder()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				string selectedFolder = _settingsService.LauncherTempDir;
				while (ShowSelectFolderDialog(ref selectedFolder, _E05B._E000(31432)) == true && selectedFolder.NormalizePath() != _settingsService.LauncherTempDir.NormalizePath())
				{
					await _E000(deleteRootFolder: true);
					await this.m__E011.AssignFullPermissions(selectedFolder);
					if (this.m__E011.CheckPermissions(selectedFolder))
					{
						_settingsService.LauncherTempDir = selectedFolder;
						break;
					}
					await this.m__E002.ShowDialog(DialogWindowMessage.NoAccessToTheFolder, selectedFolder);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(31453));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void ClearTempFolder()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await _E000(deleteRootFolder: false);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(30991));
				await this.m__E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public void NavigateToDirectory(string dir)
	{
		LogJsDotNetCall();
		try
		{
			Process.Start(_E05B._E000(29392), dir);
		}
		catch (Exception exc)
		{
			this.m__E002.ShowException(exc).Wait();
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private void _E001()
	{
		LoginWindow loginWindow = new LoginWindow(this.m__E00F);
		loginWindow.Show();
		Application.Current.MainWindow = loginWindow;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _E002()
	{
		base.Close();
	}

	[CompilerGenerated]
	private async void _E000(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
			{
				this.m__E019.CloseGame();
				await Application.Current.Dispatcher.BeginInvoke((Action)delegate
				{
					InstallationWindow installationWindow = new InstallationWindow(this.m__E00F)
					{
						Owner = Application.Current.MainWindow
					};
					if (installationWindow.ShowDialog() == true)
					{
						ThreadPool.QueueUserWorkItem(async delegate
						{
							_ = 2;
							try
							{
								_gameUpdateService.OnStateChanged += OnStateChanged;
								await _gameUpdateService.DownloadAndInstallGame(installationWindow.InstallationPath, installationWindow.DistribResponseData);
								await _E001(ConsistencyCheckingMode.FullChecking);
							}
							catch (Win32Exception)
							{
								Logger.LogWarning(_E05B._E000(32051));
							}
							catch (OperationCanceledException)
							{
								Logger.LogWarning(_E05B._E000(32072));
							}
							catch (Exception ex4)
							{
								Logger.LogError(ex4, _E05B._E000(32174));
								await this.m__E002.ShowException(ex4);
								this.m__E019.UpdateGameIsInstalled();
							}
							finally
							{
								_gameUpdateService.OnStateChanged -= OnStateChanged;
							}
						});
					}
				});
				void OnStateChanged(object sender, GameUpdateServiceState e)
				{
					token.Release();
				}
			}, _E05B._E000(31905));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(31917));
			await this.m__E002.ShowException(ex);
		}
	}

	[CompilerGenerated]
	private async Task _E000(ControlledQueueToken token)
	{
		this.m__E019.CloseGame();
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			InstallationWindow installationWindow = new InstallationWindow(this.m__E00F)
			{
				Owner = Application.Current.MainWindow
			};
			if (installationWindow.ShowDialog() == true)
			{
				ThreadPool.QueueUserWorkItem(async delegate
				{
					_ = 2;
					try
					{
						_gameUpdateService.OnStateChanged += OnStateChanged;
						await _gameUpdateService.DownloadAndInstallGame(installationWindow.InstallationPath, installationWindow.DistribResponseData);
						await _E001(ConsistencyCheckingMode.FullChecking);
					}
					catch (Win32Exception)
					{
						Logger.LogWarning(_E05B._E000(32051));
					}
					catch (OperationCanceledException)
					{
						Logger.LogWarning(_E05B._E000(32072));
					}
					catch (Exception ex3)
					{
						Logger.LogError(ex3, _E05B._E000(32174));
						await this.m__E002.ShowException(ex3);
						this.m__E019.UpdateGameIsInstalled();
					}
					finally
					{
						_gameUpdateService.OnStateChanged -= OnStateChanged;
					}
				});
			}
		});
		void OnStateChanged(object sender, GameUpdateServiceState e)
		{
			token.Release();
		}
	}

	[CompilerGenerated]
	private async void _E001(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
			{
				this.m__E019.CloseGame();
				IConsistencyCheckingResult consistencyCheckingResult = _consistencyService.LastConsistencyCheckingResult;
				if (consistencyCheckingResult == null || !consistencyCheckingResult.IsFullCheck)
				{
					consistencyCheckingResult = _consistencyService.CheckConsistency(fullCheck: true);
				}
				await _consistencyService.Repair(consistencyCheckingResult);
				token.Release();
				await _E001(ConsistencyCheckingMode.SkipChecking);
			}, _E05B._E000(31899));
		}
		catch (OperationCanceledException)
		{
			Logger.LogWarning(_E05B._E000(31968));
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(31943));
			await this.m__E002.ShowException(ex2);
		}
	}

	[CompilerGenerated]
	private async Task _E001(ControlledQueueToken token)
	{
		this.m__E019.CloseGame();
		IConsistencyCheckingResult consistencyCheckingResult = _consistencyService.LastConsistencyCheckingResult;
		if (consistencyCheckingResult == null || !consistencyCheckingResult.IsFullCheck)
		{
			consistencyCheckingResult = _consistencyService.CheckConsistency(fullCheck: true);
		}
		await _consistencyService.Repair(consistencyCheckingResult);
		token.Release();
		await _E001(ConsistencyCheckingMode.SkipChecking);
	}

	[CompilerGenerated]
	private async void _E002(object w)
	{
		LauncherUpdate launcherUpdate = null;
		try
		{
			await ExecuteSingleQueueOperation(async delegate
			{
				launcherUpdate = await _launcherUpdateService.CheckForLauncherUpdate();
				if (launcherUpdate.UpdateIsRequired)
				{
					await _launcherUpdateService.BeginUpdateLauncher(launcherUpdate);
					if (await this.m__E004.RequestRestartForUpdate())
					{
						_launcherUpdateService.EndUpdateLauncher(launcherUpdate);
					}
					this.m__E004.Close();
				}
			}, _E05B._E000(31522));
		}
		catch (NetworkException ex) when (ex.IsLocalProblems)
		{
			Logger.LogWarning(_E05B._E000(29590));
			await this.m__E002.ShowException(ex);
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(29651));
			await this.m__E002.ShowException(ex2);
		}
		if (launcherUpdate == null || !launcherUpdate.UpdateIsRequired)
		{
			try
			{
				await _gameUpdateService.CheckForGameUpdate();
			}
			catch (NetworkException ex3) when (ex3.IsLocalProblems)
			{
				Logger.LogWarning(_E05B._E000(31539));
				await this.m__E002.ShowException(ex3);
			}
			catch (Exception ex4)
			{
				Logger.LogError(ex4, _E05B._E000(31510));
				await this.m__E002.ShowException(ex4);
			}
		}
	}

	[CompilerGenerated]
	private async void _E003(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
			{
				await UpdateGameIfRequired(await _gameUpdateService.CheckForGameUpdate(fastChecking: true), token);
			}, _E05B._E000(31602));
		}
		catch (Exception exc)
		{
			await this.m__E002.ShowException(exc);
		}
	}

	[CompilerGenerated]
	private async Task _E002(ControlledQueueToken token)
	{
		await UpdateGameIfRequired(await _gameUpdateService.CheckForGameUpdate(fastChecking: true), token);
	}

	[CompilerGenerated]
	private async void _E004(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation((ControlledQueueToken token) => Task.Run((Action)_gameUpdateService.PauseInstallation), _E05B._E000(31615));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(31565));
			await this.m__E002.ShowException(ex2);
		}
	}

	[CompilerGenerated]
	private Task _E003(ControlledQueueToken token)
	{
		return Task.Run((Action)_gameUpdateService.PauseInstallation);
	}

	[CompilerGenerated]
	private async void _E005(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation((ControlledQueueToken token) => _gameUpdateService.StopInstallation(showCancellationDialog: true), _E05B._E000(31665));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(31616));
			await this.m__E002.ShowException(ex2);
		}
	}

	[CompilerGenerated]
	private Task _E004(ControlledQueueToken token)
	{
		return _gameUpdateService.StopInstallation(showCancellationDialog: true);
	}

	[CompilerGenerated]
	private async void _E006(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate(ControlledQueueToken token)
			{
				_gameUpdateService.OnStateChanged += OnStateChanged;
				try
				{
					await _gameUpdateService.ResumeInstallation();
				}
				finally
				{
					_gameUpdateService.OnStateChanged -= OnStateChanged;
				}
				void OnStateChanged(object sender, GameUpdateServiceState e)
				{
					token.Release();
				}
			}, _E05B._E000(31717));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(31738));
			await this.m__E002.ShowException(ex2);
		}
	}

	[CompilerGenerated]
	private async Task _E005(ControlledQueueToken token)
	{
		_gameUpdateService.OnStateChanged += OnStateChanged;
		try
		{
			await _gameUpdateService.ResumeInstallation();
		}
		finally
		{
			_gameUpdateService.OnStateChanged -= OnStateChanged;
		}
		void OnStateChanged(object sender, GameUpdateServiceState e)
		{
			token.Release();
		}
	}

	[CompilerGenerated]
	private async void _E007(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate
			{
				bool flag = false;
				this._E021 = new CancellationTokenSource();
				this._E01C.OnQueueStateChanged += delegate
				{
					flag = true;
				};
				try
				{
					(string, int) tuple = await this.m__E019.PrepareGame(this._E021.Token);
					if (flag)
					{
						if (_settingsService.SoundQueueNotificationEnabled)
						{
							this.m__E004.PlaySound(_E05B._E000(31709));
						}
						if (_settingsService.QueueAutostartEnabled)
						{
							await this.m__E019.RunGame(tuple.Item1);
						}
						else
						{
							this.m__E004.RestoreWindow();
							this.m__E004.DrawMainButtonCountdown(tuple.Item2, disableMainButton: false);
						}
					}
					else
					{
						await this.m__E019.RunGame(tuple.Item1);
						this._E01E = 0;
					}
				}
				catch (OperationCanceledException)
				{
				}
				finally
				{
					this._E01C.OnQueueStateChanged -= delegate
					{
						flag = true;
					};
					this._E021?.Dispose();
					this._E021 = null;
				}
			}, _E05B._E000(31277));
		}
		catch (OperationCanceledException)
		{
			Logger.LogWarning(_E05B._E000(31291));
		}
		catch (UnauthorizedApiNetworkException)
		{
		}
		catch (NetworkException ex3)
		{
			this.m__E004.DrawMainButtonCountdown(this._E015[Math.Min(this._E01E, this._E015.Length - 1)], disableMainButton: true);
			this._E01E++;
			if (!(ex3 is ApiNetworkException ex4) || ex4.ApiCode != 232)
			{
				await this.m__E002.ShowException(ex3);
				return;
			}
			Logger.LogInformation(_E05B._E000(31254));
			if ((await _gameUpdateService.CheckForGameUpdate(fastChecking: true)).UpdateNecessity == UpdateNecessity.UpdateNotRequired)
			{
				await this.m__E002.ShowDialog(DialogWindowMessage.TheGameCannotBeStartedPleaseTryAgainLater);
			}
		}
		catch (Exception ex5)
		{
			Logger.LogError(ex5, _E05B._E000(31318));
			await this.m__E002.ShowException(ex5);
		}
	}

	[CompilerGenerated]
	private async Task _E006(ControlledQueueToken token)
	{
		bool flag = false;
		this._E021 = new CancellationTokenSource();
		this._E01C.OnQueueStateChanged += delegate
		{
			flag = true;
		};
		try
		{
			(string, int) tuple = await this.m__E019.PrepareGame(this._E021.Token);
			if (flag)
			{
				if (_settingsService.SoundQueueNotificationEnabled)
				{
					this.m__E004.PlaySound(_E05B._E000(31709));
				}
				if (_settingsService.QueueAutostartEnabled)
				{
					await this.m__E019.RunGame(tuple.Item1);
					return;
				}
				this.m__E004.RestoreWindow();
				this.m__E004.DrawMainButtonCountdown(tuple.Item2, disableMainButton: false);
			}
			else
			{
				await this.m__E019.RunGame(tuple.Item1);
				this._E01E = 0;
			}
		}
		catch (OperationCanceledException)
		{
		}
		finally
		{
			this._E01C.OnQueueStateChanged -= delegate
			{
				flag = true;
			};
			this._E021?.Dispose();
			this._E021 = null;
		}
	}

	[CompilerGenerated]
	private async void _E008(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate
			{
				this.m__E019.CloseGame();
				if (await this._E000())
				{
					await _E001(ConsistencyCheckingMode.FullChecking);
				}
			}, _E05B._E000(31408));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(26744));
			await this.m__E002.ShowException(ex);
		}
	}

	[CompilerGenerated]
	private async Task _E007(ControlledQueueToken token)
	{
		this.m__E019.CloseGame();
		if (await this._E000())
		{
			await _E001(ConsistencyCheckingMode.FullChecking);
		}
	}

	[CompilerGenerated]
	private async void _E009(object w)
	{
		try
		{
			await ExecuteSingleQueueOperation(async delegate
			{
				MatchingConfigurationWindow matchingConfigurationWindow = await MatchingConfigurationWindow.Create(this.m__E00F);
				if (await matchingConfigurationWindow.ShowDialog() == DialogResult.Positive)
				{
					await this.m__E006.SetMatchingConfiguration(matchingConfigurationWindow.MatchingConfiguration);
					await this._E001();
				}
			}, _E05B._E000(31367));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(31380));
			await this.m__E002.ShowException(ex);
		}
	}

	[CompilerGenerated]
	private async Task _E008(ControlledQueueToken token)
	{
		MatchingConfigurationWindow matchingConfigurationWindow = await MatchingConfigurationWindow.Create(this.m__E00F);
		if (await matchingConfigurationWindow.ShowDialog() == DialogResult.Positive)
		{
			await this.m__E006.SetMatchingConfiguration(matchingConfigurationWindow.MatchingConfiguration);
			await this._E001();
		}
	}

	[CompilerGenerated]
	private async void _E00A(object w)
	{
		try
		{
			string selectedFolder = _settingsService.LauncherTempDir;
			while (ShowSelectFolderDialog(ref selectedFolder, _E05B._E000(31432)) == true && selectedFolder.NormalizePath() != _settingsService.LauncherTempDir.NormalizePath())
			{
				await _E000(deleteRootFolder: true);
				await this.m__E011.AssignFullPermissions(selectedFolder);
				if (this.m__E011.CheckPermissions(selectedFolder))
				{
					_settingsService.LauncherTempDir = selectedFolder;
					break;
				}
				await this.m__E002.ShowDialog(DialogWindowMessage.NoAccessToTheFolder, selectedFolder);
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(31453));
			await this.m__E002.ShowException(ex);
		}
	}

	[CompilerGenerated]
	private async void _E00B(object w)
	{
		try
		{
			await _E000(deleteRootFolder: false);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(30991));
			await this.m__E002.ShowException(ex);
		}
	}
}
