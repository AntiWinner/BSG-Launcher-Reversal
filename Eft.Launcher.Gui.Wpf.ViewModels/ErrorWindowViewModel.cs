using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Eft.Launcher.Gui.Wpf.Views;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class ErrorWindowViewModel : WindowViewModelBase
{
	private IErrorWindowDelegate _E004;

	public ErrorWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	internal override Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (IErrorWindowDelegate)windowDelegate;
		return base._E001(windowDelegate);
	}

	[DebuggerHidden]
	public void Ok()
	{
		LogJsDotNetCall();
		_E004.Close();
	}
}
