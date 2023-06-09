using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class SelectLanguageWindowViewModel : WindowViewModelBase
{
	private readonly IServiceProvider _E00F;

	private readonly ISettingsService _settingsService;

	private ISelectLanguageWindowDelegate _E004;

	public SelectLanguageWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_E00F = serviceProvider;
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (ISelectLanguageWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.SelectLanguagePageUri.ToString());
	}

	[DebuggerHidden]
	public void Ok()
	{
		LogJsDotNetCall();
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			LoginWindow loginWindow = new LoginWindow(_E00F);
			loginWindow.Show();
			Application.Current.MainWindow = loginWindow;
			_E004.Close();
		});
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private void _E000()
	{
		LoginWindow loginWindow = new LoginWindow(_E00F);
		loginWindow.Show();
		Application.Current.MainWindow = loginWindow;
		_E004.Close();
	}
}
