using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class ProgressWindowViewModel : WindowViewModelBase
{
	private readonly ISettingsService _settingsService;

	private IProgressWindowDelegate _E004;

	public ProgressWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IProgressWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.ProgressBarPageUri.ToString());
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}
}
