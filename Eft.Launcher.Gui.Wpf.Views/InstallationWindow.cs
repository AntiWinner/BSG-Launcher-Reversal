using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Gui.Wpf.Views;

public class InstallationWindow : BrowserWindowBase, IInstallationWindowDelegate, IWindowDelegate, IComponentConnector
{
	[CompilerGenerated]
	private GamePackageInfo _E009;

	[CompilerGenerated]
	private string _E00A;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public GamePackageInfo DistribResponseData
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		set
		{
			_E009 = value;
		}
	}

	public string InstallationPath
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		set
		{
			_E00A = value;
		}
	}

	public InstallationWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public void UpdateInstallationInfo(long requiredFreeSpace, long availableSpace)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(24751), InstallationPath.Replace(_E05B._E000(24721), _E05B._E000(24727)), requiredFreeSpace, availableSpace));
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(24724), UriKind.Relative);
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
	bool? IInstallationWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void IInstallationWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}
}
