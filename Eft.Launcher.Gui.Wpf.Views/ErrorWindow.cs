using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Eft.Launcher.Gui.Wpf.Views;

[JsonObject(MemberSerialization.OptIn)]
public class ErrorWindow : BrowserWindowBase, IErrorWindowDelegate, IWindowDelegate, IComponentConnector
{
	private readonly string _E004;

	private readonly string _E005;

	[CompilerGenerated]
	private bool? _E006;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public new bool? DialogResult
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		set
		{
			_E006 = value;
		}
	}

	public ErrorWindow(IServiceProvider serviceProvider, string serializedException)
		: base(serviceProvider)
	{
		_E004 = serviceProvider.GetRequiredService<ISettingsService>().ErrorPageUri.ToString();
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
		_E005 = serializedException;
	}

	protected override async Task OnBrowserInitialized()
	{
		await base.OnBrowserInitialized();
		await LoadAsync(_E004);
		ExecuteJavaScript(_E05B._E000(25046) + _E005 + _E05B._E000(24948));
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
			Uri resourceLocator = new Uri(_E05B._E000(25060), UriKind.Relative);
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

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000()
	{
		return base.OnBrowserInitialized();
	}
}
