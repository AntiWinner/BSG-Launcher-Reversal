using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;

namespace Eft.Launcher.Gui.Wpf.Views;

public class ProgressWindow : BrowserWindowBase, IProgressWindowDelegate, IWindowDelegate, IComponentConnector
{
	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public ProgressWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public void SetProgress(int progress)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(28630), progress), string.Empty);
	}

	public void SetMessage(ProgressWindowMessage message)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(28215), (int)message));
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(28165), UriKind.Relative);
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
}
