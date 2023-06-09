using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services;
using Eft.Launcher.Services.AnalyticsService;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.ConsistencyService;
using Eft.Launcher.Services.DownloadService;
using Eft.Launcher.Services.GameService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.AnalyticsService;

public class AnalyticsService : IAnalyticsService, IService
{
	private class _E000
	{
		[CompilerGenerated]
		private string m__E000;

		[CompilerGenerated]
		private JObject _E001;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.m__E000;
			}
			[CompilerGenerated]
			set
			{
				this.m__E000 = value;
			}
		}

		public JObject _E000
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			set
			{
				_E001 = value;
			}
		}

		public _E000(string name, JObject data)
		{
			Name = name;
			_E000 = data;
		}
	}

	private readonly ILogger m__E000;

	private readonly ILauncherUpdateService _launcherUpdateService;

	private readonly IGameUpdateService _gameUpdateService;

	private readonly IGameService _E001;

	private readonly IDownloadService _downloadService;

	private readonly ILauncherBackendService _E002;

	private readonly IAuthService _E003;

	private readonly IConsistencyService _consistencyService;

	private readonly ConcurrentQueue<_E000> _E004 = new ConcurrentQueue<_E000>();

	public AnalyticsService(ILogger<AnalyticsService> logger, ILauncherUpdateService launcherUpdateService, IGameUpdateService gameUpdateService, IGameService gameService, IDownloadService downloadService, ILauncherBackendService backendService, IAuthService authService, IConsistencyService consistencyService)
	{
		this.m__E000 = logger;
		_launcherUpdateService = launcherUpdateService;
		_gameUpdateService = gameUpdateService;
		this._E001 = gameService;
		_downloadService = downloadService;
		this._E002 = backendService;
		_E003 = authService;
		_consistencyService = consistencyService;
	}

	public void OnAwake()
	{
		_launcherUpdateService.OnLauncherInstallationCompleted += OnLauncherInstallationCompleted;
		_gameUpdateService.OnInstallationCompleted += OnGameInstallationCompleted;
		_gameUpdateService.OnUpdateCompleted += OnGameUpdateCompleted;
		this._E001.OnGameClosedAsync += _E000;
		_downloadService.OnEndDownload += OnEndDownload;
		_E003.OnLoggedIn += _E000;
		_consistencyService.OnConsistencyCheckingCompleted += OnConsistencyCheckingCompleted;
	}

	public void OnStop()
	{
		_launcherUpdateService.OnLauncherInstallationCompleted -= OnLauncherInstallationCompleted;
		_gameUpdateService.OnInstallationCompleted -= OnGameInstallationCompleted;
		_gameUpdateService.OnUpdateCompleted -= OnGameUpdateCompleted;
		this._E001.OnGameClosedAsync -= _E000;
		_downloadService.OnEndDownload -= OnEndDownload;
		_E003.OnLoggedIn -= _E000;
		_consistencyService.OnConsistencyCheckingCompleted -= OnConsistencyCheckingCompleted;
	}

	private void SendLauncherInstallationResult(Version version, InstallationResult applyingResult)
	{
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(12835)
			},
			{
				_E05B._E000(10722),
				version.ToString()
			},
			{
				_E05B._E000(21279),
				applyingResult.ToString()
			}
		};
		this._E000(new _E000(_E05B._E000(12854), data));
	}

	private void SendGameInstallationResult(BsgVersion version, InstallationResult applyingResult)
	{
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(12808)
			},
			{
				_E05B._E000(10722),
				version.ToString()
			},
			{
				_E05B._E000(21279),
				applyingResult.ToString()
			}
		};
		this._E000(new _E000(_E05B._E000(12831), data));
	}

	private void SendGameUpdateResult(BsgVersion fromVersion, BsgVersion toVersion, InstallationResult applyingResult)
	{
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(12909)
			},
			{
				_E05B._E000(12922),
				fromVersion.ToString()
			},
			{
				_E05B._E000(12870),
				toVersion.ToString()
			},
			{
				_E05B._E000(21279),
				applyingResult.ToString()
			}
		};
		this._E000(new _E000(_E05B._E000(12876), data));
	}

	private void SendConsistencyCheckingResult(IConsistencyCheckingResult consistencyCheckingResult)
	{
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(12888)
			},
			{
				_E05B._E000(12972),
				consistencyCheckingResult.IsFullCheck ? _E05B._E000(12986) : _E05B._E000(12983)
			},
			{
				_E05B._E000(12989),
				consistencyCheckingResult.IsSuccess
			}
		};
		this._E000(new _E000(_E05B._E000(12939), data));
	}

	private void SendDownloadResult(string cdnDomainName, DownloadCategory downloadCategory, int averageDownloadSpeedMBit)
	{
		string text = ((averageDownloadSpeedMBit < 5) ? _E05B._E000(12993) : ((averageDownloadSpeedMBit < 10) ? _E05B._E000(13050) : ((averageDownloadSpeedMBit < 30) ? _E05B._E000(13036) : ((averageDownloadSpeedMBit < 60) ? _E05B._E000(13030) : _E05B._E000(12958)))));
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(13002)
			},
			{
				_E05B._E000(13018),
				cdnDomainName
			},
			{
				_E05B._E000(21744),
				downloadCategory.ToString()
			},
			{
				_E05B._E000(12579),
				text
			}
		};
		this._E000(new _E000(_E05B._E000(12584), data));
	}

	private void _E000(int exitCode, bool isCrash)
	{
		JObject data = new JObject
		{
			{
				_E05B._E000(13272),
				_E05B._E000(12599)
			},
			{
				_E05B._E000(12546),
				exitCode
			},
			{
				_E05B._E000(12553),
				isCrash
			}
		};
		this._E000(new _E000(_E05B._E000(12561), data));
	}

	private void _E000(_E000 report)
	{
		_E004.Enqueue(report);
		_E000();
	}

	private void _E000()
	{
		ThreadPool.QueueUserWorkItem(async delegate
		{
			while (_E003.IsLoggedIn && _E004.Count > 0)
			{
				if (_E004.TryDequeue(out var result))
				{
					await this._E000(result);
				}
			}
		});
	}

	private async Task _E000(_E000 report)
	{
		try
		{
			this.m__E000.LogDebug(_E05B._E000(12575), report.Name);
			await this._E002.SendAnalytics(report._E000);
			this.m__E000.LogDebug(_E05B._E000(12618), report.Name);
		}
		catch (BackendServiceException ex) when (ex.BsgExceptionCode == BsgExceptionCode.ErrorWhileSendingAnalytics && ex.Args.Contains(201.ToString()) && report.Name == _E05B._E000(12854))
		{
			this.m__E000.LogWarning(_E05B._E000(12734), report.Name);
		}
		catch (Exception exception)
		{
			this.m__E000.LogError(exception, _E05B._E000(12737), report.Name);
		}
	}

	private void OnLauncherInstallationCompleted(Version version, InstallationResult installationResult)
	{
		SendLauncherInstallationResult(version, installationResult);
	}

	private void OnGameInstallationCompleted(object sender, InstallationCompletedEventArgs e)
	{
		SendGameInstallationResult(e.Version, e.InstallationResult);
	}

	private void OnGameUpdateCompleted(object sender, UpdateCompletedEventArgs e)
	{
		SendGameUpdateResult(e.FromVersion, e.Version, e.InstallationResult);
	}

	private Task _E000(ProcessLifecycleInformation pli)
	{
		_E000(pli.ExitCode, Math.Abs(pli.ExitCode) > 1);
		return Task.CompletedTask;
	}

	private void OnEndDownload(object sender, EndDownloadEventArgs e)
	{
		if (e.DownloadCategory != 0 && e.DownloadState == DownloadState.DownloadSucceded && !e.IsDownloadWasResumed)
		{
			SendDownloadResult(e.DownloadUri.Host, e.DownloadCategory, e.AverageDownloadSpeedMbitSec);
		}
	}

	private void _E000(object sender, EventArgs e)
	{
		_E000();
	}

	private void OnConsistencyCheckingCompleted(object sender, IConsistencyCheckingResult e)
	{
		SendConsistencyCheckingResult(e);
	}

	[CompilerGenerated]
	private async void _E000(object w)
	{
		while (_E003.IsLoggedIn && _E004.Count > 0)
		{
			if (_E004.TryDequeue(out var result))
			{
				await this._E000(result);
			}
		}
	}
}
