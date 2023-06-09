using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.Services.BugReportService;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.BugReportService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class BugReportWindowViewModel : WindowViewModelBase
{
	[CompilerGenerated]
	private new sealed class _E001
	{
		public BugReportWindowViewModel _E000;

		public string _E001;

		public string _E002;

		public string[] _E003;

		public int _E004;

		public int _E005;

		public bool _E006;

		public bool _E007;

		public Func<ControlledQueueToken, Task> _E008;

		internal string _E000(string f)
		{
			return this._E000.UnescapeFilesystemEntryForGui(f);
		}

		internal async void _E000(object w)
		{
			await this._E000.ExecuteSingleQueueOperation(async delegate
			{
				try
				{
					BugReportSendingResult bugReportSendingResult = await this._E000.m__E001.SendBugReport(this._E001, _E002, _E003, TimeSpan.FromSeconds(_E004), _E005, _E006, _E007);
					await this._E000._E002.ShowDialog(DialogWindowMessage.BugReportSuccessfullySent, bugReportSendingResult.BugReportsLeft.ToString(), bugReportSendingResult.BugReportId);
					this._E000._E004.Close();
				}
				catch (Exception exc)
				{
					await this._E000._E002.ShowException(exc);
				}
			}, _E05B._E000(27032));
		}

		internal async Task _E000(ControlledQueueToken token)
		{
			try
			{
				BugReportSendingResult bugReportSendingResult = await this._E000.m__E001.SendBugReport(this._E001, _E002, _E003, TimeSpan.FromSeconds(_E004), _E005, _E006, _E007);
				await this._E000._E002.ShowDialog(DialogWindowMessage.BugReportSuccessfullySent, bugReportSendingResult.BugReportsLeft.ToString(), bugReportSendingResult.BugReportId);
				this._E000._E004.Close();
			}
			catch (Exception exc)
			{
				await this._E000._E002.ShowException(exc);
			}
		}
	}

	private static readonly string[] m__E000 = new string[6]
	{
		_E05B._E000(27065),
		_E05B._E000(27068),
		_E05B._E000(27015),
		_E05B._E000(27018),
		_E05B._E000(27021),
		_E05B._E000(27024)
	};

	private readonly ISettingsService _settingsService;

	private new readonly IBugReportService m__E001;

	private readonly IDialogService _E002;

	private int _E003;

	private IBugReportWindowDelegate _E004;

	public BugReportWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		this.m__E001 = serviceProvider.GetRequiredService<IBugReportService>();
		_E002 = serviceProvider.GetRequiredService<IDialogService>();
		this.m__E001.OnBugReportSendingStarted += _E000;
		this.m__E001.OnBugReportSendingCompleted += _E000;
		this.m__E001.OnBugReportSendingProgress += _E000;
	}

	private void _E000(object sender, EventArgs e)
	{
		_E004.ShowBugReportSendingProgress();
	}

	private void _E000(object sender, BugReportSendingArgs e)
	{
		_E004.HideBugReportSendingProgress();
	}

	private void _E000(object sender, BugReportSendingProgressArgs progress)
	{
		_E004.UpdateBugReportSendingProgress(progress.State, progress.Progress);
	}

	protected override void OnClosing()
	{
		this.m__E001.OnBugReportSendingStarted -= _E000;
		this.m__E001.OnBugReportSendingCompleted -= _E000;
		this.m__E001.OnBugReportSendingProgress -= _E000;
		base.OnClosing();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IBugReportWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.BugReportPageUri.ToString());
	}

	[DebuggerHidden]
	public long CalculateSize(List<object> selectedFiles, int gameLogsFreshnessSec, int gameLogsSizeLimit, bool attachLauncherLogs)
	{
		LogJsDotNetCall(string.Format(_E05B._E000(26990), selectedFiles.Count), gameLogsFreshnessSec, gameLogsSizeLimit, attachLauncherLogs);
		try
		{
			if (Interlocked.Increment(ref _E003) == 1)
			{
				_E004.ShowLoader();
			}
			string[] attachedFiles = (from string f in selectedFiles?
				select UnescapeFilesystemEntryForGui(f)).ToArray() ?? new string[0];
			return this.m__E001.CalculateBugReportSize(attachedFiles, TimeSpan.FromSeconds(gameLogsFreshnessSec), gameLogsSizeLimit, attachLauncherLogs);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception exc)
		{
			_E002.ShowException(exc).Wait();
		}
		finally
		{
			if (Interlocked.Decrement(ref _E003) == 0)
			{
				_E004.HideLoader();
			}
		}
		return -1L;
	}

	[DebuggerHidden]
	public string ShowBugReportSelectionDialog(string title)
	{
		object[] content = ((IEnumerable<object>)(from f in _E004.OpenFileDialog(title, multiselect: true, isFolderPicker: false, _settingsService.SelectedBranch.GameScreenshotsDir, new KeyValuePair<string, string>[1]
			{
				new KeyValuePair<string, string>(_E05B._E000(26945), string.Join(_E05B._E000(26956), BugReportWindowViewModel.m__E000.Select((string e) => _E05B._E000(27034) + e)))
			})
			where BugReportWindowViewModel.m__E000.Contains(Path.GetExtension(f))
			select f).Select(base.GetFilesystemEntryAsJson)).ToArray();
		return new JObject { 
		{
			_E05B._E000(26962),
			new JArray(content)
		} }.ToString(Formatting.None);
	}

	[DebuggerHidden]
	public void SendBugReport(string categoryId, string message, List<object> selectedFiles, int gameLogsFreshnessSec, int gameLogsSizeLimit, bool attachLauncherLogs, bool collectServersAvailabilityInfo)
	{
		string[] array = (from string f in selectedFiles?
			select UnescapeFilesystemEntryForGui(f)).ToArray() ?? new string[0];
		LogJsDotNetCall(categoryId, string.Format(_E05B._E000(27043), message.Length), string.Format(_E05B._E000(26990), array.Length), gameLogsFreshnessSec, gameLogsSizeLimit, attachLauncherLogs, collectServersAvailabilityInfo);
		ThreadPool.QueueUserWorkItem(async delegate
		{
			await ExecuteSingleQueueOperation(async delegate
			{
				try
				{
					BugReportSendingResult bugReportSendingResult = await this.m__E001.SendBugReport(categoryId, message, array, TimeSpan.FromSeconds(gameLogsFreshnessSec), gameLogsSizeLimit, attachLauncherLogs, collectServersAvailabilityInfo);
					await _E002.ShowDialog(DialogWindowMessage.BugReportSuccessfullySent, bugReportSendingResult.BugReportsLeft.ToString(), bugReportSendingResult.BugReportId);
					_E004.Close();
				}
				catch (Exception exc)
				{
					await _E002.ShowException(exc);
				}
			}, _E05B._E000(27032));
		});
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private string _E000(string f)
	{
		return UnescapeFilesystemEntryForGui(f);
	}
}
