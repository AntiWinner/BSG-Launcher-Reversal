using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class MatchingConfigurationWindowViewModel : WindowViewModelBase
{
	[CompilerGenerated]
	private sealed class _E002
	{
		public MatchingConfigurationWindowViewModel _E000;

		public DatacenterDto _E001;

		public List<IPAddress> _E002;

		public List<int> _E003;

		internal void _E000(IPAddress ip)
		{
			int num = -1;
			try
			{
				PingReply pingReply = new Ping().Send(ip, this._E000._settingsService.PingTimeout);
				num = (int)((pingReply.Status == IPStatus.Success) ? pingReply.RoundtripTime : (-1));
				if (pingReply.Status != 0)
				{
					this._E000.Logger.LogDebug(_E05B._E000(30569), _E001.Name, ip, pingReply.Status);
				}
			}
			catch (Exception exception)
			{
				this._E000.Logger.LogTrace(exception, _E05B._E000(30642), ip, _E001.Name);
				_E002.Add(ip);
				num = -2;
			}
			_E003.Add(num);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string _E000;

		internal bool _E000(DatacenterDto _)
		{
			return _.Name == this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public MatchingConfigurationWindowViewModel _E000;

		public string _E001;

		public Func<bool?> _E002;

		public Func<ControlledQueueToken, Task> _E003;

		internal async void _E000(object w)
		{
			try
			{
				await this._E000.ExecuteSingleQueueOperation(async delegate
				{
					this._E000._E000(this._E001);
					this._E000.m__E004.MatchingConfiguration = this._E001;
					await Application.Current.Dispatcher.InvokeAsync(() => this._E000.m__E004.DialogResult = true);
					this._E000.m__E004.Close();
				}, _E05B._E000(30612));
			}
			catch (Exception exc)
			{
				await this._E000.m__E002.ShowException(exc);
			}
		}

		internal async Task _E000(ControlledQueueToken token)
		{
			this._E000._E000(this._E001);
			this._E000.m__E004.MatchingConfiguration = this._E001;
			await Application.Current.Dispatcher.InvokeAsync(() => this._E000.m__E004.DialogResult = true);
			this._E000.m__E004.Close();
		}

		internal bool? _E000()
		{
			return this._E000.m__E004.DialogResult = true;
		}
	}

	private const int _E00A = 3000;

	private readonly ISettingsService _settingsService;

	private readonly IGameBackendService _E006;

	private readonly IDialogService m__E002;

	private readonly System.Timers.Timer _E00B;

	private readonly CancellationTokenSource _E00C;

	private readonly ParallelOptions _E00D;

	private IMatchingConfigurationWindowDelegate m__E004;

	private DatacenterDto[] _E00E;

	public MatchingConfigurationWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_E006 = serviceProvider.GetRequiredService<IGameBackendService>();
		this.m__E002 = serviceProvider.GetRequiredService<IDialogService>();
		_E00B = new System.Timers.Timer(3000.0);
		_E00B.Elapsed += _E000;
		_E00C = new CancellationTokenSource();
		_E00C.Token.Register(delegate
		{
			_E00B.Stop();
			_E00B.Elapsed -= _E000;
		});
		_E00D = new ParallelOptions
		{
			CancellationToken = _E00C.Token,
			MaxDegreeOfParallelism = Environment.ProcessorCount
		};
	}

	private void _E000(object sender, ElapsedEventArgs e)
	{
		_E00B.Stop();
		try
		{
			this._E000();
			_E00B.Start();
		}
		catch (OperationCanceledException)
		{
		}
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		this.m__E004 = (IMatchingConfigurationWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await this.m__E004.LoadAsync(_settingsService.MatchingConfigurationPageUri.ToString());
		await this._E000();
	}

	private async Task _E000()
	{
		try
		{
			_E00E = await _E006.GetDatacenters();
			this.m__E004.RenderForm(_E00E);
			try
			{
				this._E000();
				_E00B.Start();
			}
			catch (OperationCanceledException)
			{
			}
		}
		catch (Exception ex2)
		{
			Logger.LogError(ex2, _E05B._E000(30515));
			await this.m__E002.ShowException(ex2);
			Close();
		}
	}

	private void _E000()
	{
		Logger.LogTrace(_E05B._E000(26854));
		Parallel.ForEach(_E00E, _E00D, delegate(DatacenterDto dc)
		{
			int ping = this._E000(dc);
			this.m__E004.SetPing(dc.Name, ping);
		});
		Logger.LogTrace(_E05B._E000(26868));
	}

	private int _E000(DatacenterDto datacenter)
	{
		List<int> list = new List<int>();
		List<IPAddress> list2 = new List<IPAddress>();
		Parallel.ForEach(datacenter.IpAddresses, _E00D, delegate(IPAddress ip)
		{
			int num = -1;
			try
			{
				PingReply pingReply = new Ping().Send(ip, _settingsService.PingTimeout);
				num = (int)((pingReply.Status == IPStatus.Success) ? pingReply.RoundtripTime : (-1));
				if (pingReply.Status != 0)
				{
					Logger.LogDebug(_E05B._E000(30569), datacenter.Name, ip, pingReply.Status);
				}
			}
			catch (Exception exception)
			{
				Logger.LogTrace(exception, _E05B._E000(30642), ip, datacenter.Name);
				list2.Add(ip);
				num = -2;
			}
			list.Add(num);
		});
		if (list2.Any())
		{
			Logger.LogInformation(_E05B._E000(26833), datacenter.Name);
		}
		if (!list.All((int p) => p == -2))
		{
			if (!list.All((int p) => p < 0))
			{
				return list.Where((int p) => p >= 0).Max();
			}
			return -1;
		}
		return -2;
	}

	protected override void OnClosing()
	{
		_E00B.Stop();
		_E00C.Cancel();
		base.OnClosing();
	}

	private void _E000(string json)
	{
		foreach (string current in JObject.Parse(json).First.First.Values<string>())
		{
			DatacenterDto datacenterDto = _E00E.First((DatacenterDto _) => _.Name == current);
			if (datacenterDto.MaxPingTime <= 0 || this._E000(datacenterDto) <= datacenterDto.MaxPingTime)
			{
				continue;
			}
			throw new BsgException(BsgExceptionCode.YouCannotSelectThisDatacenterBecauseItHasTooLongResponseTimeForYourRegion, current);
		}
	}

	[DebuggerHidden]
	public void Apply(string json)
	{
		LogJsDotNetCall(json);
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await ExecuteSingleQueueOperation(async delegate
				{
					_E000(json);
					this.m__E004.MatchingConfiguration = json;
					await Application.Current.Dispatcher.InvokeAsync(() => this.m__E004.DialogResult = true);
					this.m__E004.Close();
				}, _E05B._E000(30612));
			}
			catch (Exception exc)
			{
				await this.m__E002.ShowException(exc);
			}
		});
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E00B.Stop();
		_E00B.Elapsed -= _E000;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private void _E000(DatacenterDto dc)
	{
		int ping = this._E000(dc);
		this.m__E004.SetPing(dc.Name, ping);
	}
}
