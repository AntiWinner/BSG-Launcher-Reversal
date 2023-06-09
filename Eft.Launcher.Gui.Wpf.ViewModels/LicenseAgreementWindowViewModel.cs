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

public class LicenseAgreementWindowViewModel : WindowViewModelBase
{
	private readonly ISettingsService _settingsService;

	private ILicenseAgreementWindowDelegate _E004;

	public LicenseAgreementWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (ILicenseAgreementWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		string text = _settingsService.LicenseAgreementPageUri.ToString().TrimEnd('/');
		await _E004.LoadAsync(text + (text.Contains(_E05B._E000(26772)) ? _E05B._E000(26778) : _E05B._E000(26772)) + _E05B._E000(26776) + _E004.Document);
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
			Logger.LogError(exception, _E05B._E000(26805));
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
