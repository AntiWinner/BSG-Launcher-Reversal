using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

[JsonObject(MemberSerialization.OptIn)]
public class CodeActivationWindow : BrowserWindowBase, ICodeActivationWindowDelegate, IWindowDelegate, IComponentConnector
{
	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	internal CodeActivationWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public void Success(JToken data)
	{
		ExecuteJavaScript(_E05B._E000(24941) + data.ToString(Formatting.None) + _E05B._E000(24948));
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
		Application.Current.MainWindow.Activate();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(24954), UriKind.Relative);
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
	bool? ICodeActivationWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void ICodeActivationWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}
}
