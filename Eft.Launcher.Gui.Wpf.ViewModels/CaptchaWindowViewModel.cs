using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Eft.Launcher.Gui.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class CaptchaWindowViewModel : WindowViewModelBase
{
	private ICaptchaWindowDelegate _E004;

	public CaptchaWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (ICaptchaWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
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
