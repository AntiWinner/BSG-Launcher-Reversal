using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

public class LoginWindow : BrowserWindowBase, ILoginWindowDelegate, IWindowDelegate, IComponentConnector
{
	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public LoginWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);
		(PresentationSource.FromVisual(this) as HwndSource).AddHook(_E000);
	}

	private IntPtr _E000(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == _E009._E001)
		{
			RestoreWindow();
		}
		return IntPtr.Zero;
	}

	public void LockLoginButton(int delay)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(27419), delay));
	}

	public void SetCapsLockState(bool isKeyDown)
	{
		ExecuteJavaScript(_E05B._E000(27506) + isKeyDown.ToString().ToLower() + _E05B._E000(24948));
	}

	public void DisplayActivationCodeForm()
	{
		ExecuteJavaScript(_E05B._E000(27456));
	}

	public void DisplayCaptcha()
	{
		ExecuteJavaScript(_E05B._E000(27484));
	}

	public void DisplayPhoneBindingForm(JToken geoInfo)
	{
		ExecuteJavaScript(_E05B._E000(27566) + geoInfo?.ToString(Formatting.None) + _E05B._E000(24948));
	}

	public void DisplayPhoneVerificationForm(string codeExpire)
	{
		ExecuteJavaScript(_E05B._E000(27526) + codeExpire + _E05B._E000(24948));
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_E003)
		{
			_E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(27551), UriKind.Relative);
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
