using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class FeedbackWindowViewModel : WindowViewModelBase
{
	private readonly ISettingsService _settingsService;

	private IFeedbackWindowDelegate _E004;

	public FeedbackWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IFeedbackWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.FeedbackPageUri.ToString());
	}

	[DebuggerHidden]
	public void Ok()
	{
		LogJsDotNetCall();
		try
		{
			Application.Current.Dispatcher.Invoke(() => _E004.DialogResult = true);
			_E004.Close();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(27114));
		}
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
