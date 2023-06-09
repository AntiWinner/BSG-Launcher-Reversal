using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class RequestRestartDialogWindowViewModel : WindowViewModelBase
{
	private readonly ISettingsService _settingsService;

	private IRequestRestartDialogWindowDelegate _E004;

	public RequestRestartDialogWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IRequestRestartDialogWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.RequestRestartPageUri.ToString());
	}

	[DebuggerHidden]
	public void Ok()
	{
		LogJsDotNetCall();
		Application.Current.Dispatcher.Invoke(() => _E004.DialogResult = true);
		_E004.Close();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private bool? _E000()
	{
		return _E004.DialogResult = true;
	}
}
