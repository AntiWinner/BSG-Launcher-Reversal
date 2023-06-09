using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Eft.Launcher.Services.DialogService;

namespace Eft.Launcher.Gui.Wpf.Views;

public class LicenseAgreementWindow : BrowserWindowBase, ILicenseAgreementWindowDelegate, IWindowDelegate, IDialogWindow, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public LicenseAgreementWindow _E000;

		public DialogResult _E001;

		internal void _E000()
		{
			bool? flag = ((Window)this._E000).ShowDialog();
			_E001 = ((flag == false) ? Eft.Launcher.Services.DialogService.DialogResult.Negative : ((flag == true) ? Eft.Launcher.Services.DialogService.DialogResult.Positive : Eft.Launcher.Services.DialogService.DialogResult.Cancelled));
		}
	}

	[CompilerGenerated]
	private readonly string _E00B;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public string Document
	{
		[CompilerGenerated]
		get
		{
			return _E00B;
		}
	}

	public LicenseAgreementWindow(IServiceProvider serviceProvider, string document)
		: base(serviceProvider)
	{
		_E00B = document;
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public new async Task<DialogResult> ShowDialog()
	{
		DialogResult result = Eft.Launcher.Services.DialogService.DialogResult.Cancelled;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			bool? flag = base.ShowDialog();
			result = ((flag == false) ? Eft.Launcher.Services.DialogService.DialogResult.Negative : ((flag == true) ? Eft.Launcher.Services.DialogService.DialogResult.Positive : Eft.Launcher.Services.DialogService.DialogResult.Cancelled));
		});
		return result;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(28475), UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			Browser = (ChromiumWebBrowser)target;
			break;
		case 2:
			LoadingAnimation = (LoadingAnimation)target;
			break;
		default:
			_E003 = true;
			break;
		}
	}

	[SpecialName]
	bool? ILicenseAgreementWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void ILicenseAgreementWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}
}
