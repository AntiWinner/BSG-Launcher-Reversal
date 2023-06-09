using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class InstallationWindowViewModel : WindowViewModelBase
{
	private readonly ISettingsService _settingsService;

	private readonly ILauncherBackendService _E008;

	private readonly IDialogService _E002;

	private IInstallationWindowDelegate _E004;

	private long _E009;

	public InstallationWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_E008 = serviceProvider.GetRequiredService<ILauncherBackendService>();
		this._E002 = serviceProvider.GetRequiredService<IDialogService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IInstallationWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		GamePackageInfo gamePackageInfo = await _E008.GetGamePackage();
		if (gamePackageInfo == null)
		{
			throw new Exception(_E05B._E000(26653));
		}
		_E009 = gamePackageInfo.RequiredFreeSpace;
		await _E004.LoadAsync(_settingsService.InstallationPageUri.ToString());
		_E004.DistribResponseData = gamePackageInfo;
		_E004.InstallationPath = (string.IsNullOrWhiteSpace(_settingsService.SelectedBranch.GameRootDir) ? _settingsService.SelectedBranch.DefaultGameRootDir : _settingsService.SelectedBranch.GameRootDir);
		_E004.UpdateInstallationInfo(_E009, _E000(_E004.InstallationPath));
	}

	private long _E000(string path)
	{
		string text = _E05B._E000(27102);
		try
		{
			text = Path.GetPathRoot(path.WithoutLongPathPrefix());
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveInfo.IsReady && driveInfo.Name == text)
				{
					return driveInfo.TotalFreeSpace;
				}
			}
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(26662), text);
		}
		return -1L;
	}

	[DebuggerHidden]
	public void SelectGameFolder()
	{
		LogJsDotNetCall();
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				string text = await ShowSelectGameFolderDialog(_E004.InstallationPath);
				if (text != null)
				{
					_E004.InstallationPath = text;
					_E004.UpdateInstallationInfo(_E009, _E000(text));
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, _E05B._E000(26744));
				await this._E002.ShowException(ex);
			}
		});
	}

	[DebuggerHidden]
	public async void Ok()
	{
		LogJsDotNetCall();
		try
		{
			if (_E000(_E004.InstallationPath) < _E009)
			{
				throw new BsgException(BsgExceptionCode.NotEnoughDiskSpaceForInstallation);
			}
			if (!(await CanInstallToDirectory(_E004.InstallationPath, notifyUser: true)))
			{
				return;
			}
			bool flag = Directory.Exists(_E004.InstallationPath) && Directory.EnumerateFileSystemEntries(_E004.InstallationPath).Any();
			if (flag)
			{
				flag = await this._E002.ShowDialog(DialogWindowMessage.DirectoryNotEmpty, _E004.InstallationPath.WithoutLongPathPrefix()) != DialogResult.Positive;
			}
			if (!flag)
			{
				Application.Current.Dispatcher.Invoke(() => _E004.DialogResult = true);
				_E004.Close();
			}
		}
		catch (Exception exc)
		{
			await this._E002.ShowException(exc);
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private async void _E000(object w)
	{
		try
		{
			string text = await ShowSelectGameFolderDialog(_E004.InstallationPath);
			if (text != null)
			{
				_E004.InstallationPath = text;
				_E004.UpdateInstallationInfo(_E009, _E000(text));
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, _E05B._E000(26744));
			await this._E002.ShowException(ex);
		}
	}

	[CompilerGenerated]
	private bool? _E000()
	{
		return _E004.DialogResult = true;
	}
}
