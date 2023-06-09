using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using CefSharp;
using CefSharp.Wpf;
using Eft.Launcher.Gui.Wpf.CefEngine;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.Views;

public class CaptchaWindow : BrowserWindowBase, ICaptchaWindowDelegate, IWindowDelegate, IDialogWindow, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public CaptchaWindow _E000;

		public DialogResult _E001;

		internal void _E000()
		{
			bool? flag = this._E000.ShowDialog();
			_E001 = ((flag == false) ? Eft.Launcher.Services.DialogService.DialogResult.Negative : ((flag == true) ? Eft.Launcher.Services.DialogService.DialogResult.Positive : Eft.Launcher.Services.DialogService.DialogResult.Cancelled));
		}
	}

	private readonly ISettingsService _settingsService;

	private readonly HttpResponseMessage _responseWithCaptcha;

	private readonly HttpClientHandler _E007;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	internal CaptchaWindow(IServiceProvider serviceProvider, HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler)
		: base(serviceProvider)
	{
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_responseWithCaptcha = responseWithCaptcha;
		_E007 = clientHandler;
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	protected override async Task OnBrowserInitialized()
	{
		await base.OnBrowserInitialized();
		if (_E007.CookieContainer != null)
		{
			await Browser.LoadCookiesAsync(_settingsService.GuiUri, _E007.CookieContainer);
		}
		string html = await _responseWithCaptcha.Content.ReadAsStringAsync();
		_responseWithCaptcha.RequestMessage.RequestUri.ToString();
		Browser.FrameLoadEnd += _E000;
		IFrame mainFrame = Browser.GetMainFrame();
		IRequest request = await mainFrame.CreateRequestAsync(_responseWithCaptcha.RequestMessage);
		mainFrame.LoadHtml(html);
		mainFrame.LoadRequest(request);
		Browser.Visibility = Visibility.Visible;
	}

	private void _E000(object sender, FrameLoadEndEventArgs e)
	{
		if (e.HttpStatusCode == 200 && e.Url.StartsWith(_responseWithCaptcha.RequestMessage.RequestUri.ToString()))
		{
			base.Dispatcher.Invoke(() => base.DialogResult = true);
			Close();
		}
	}

	async Task<DialogResult> IDialogWindow.ShowDialog()
	{
		DialogResult result = Eft.Launcher.Services.DialogService.DialogResult.Cancelled;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			bool? flag = ShowDialog();
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
			Uri resourceLocator = new Uri(_E05B._E000(25055), UriKind.Relative);
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
	bool? ICaptchaWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void ICaptchaWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000()
	{
		return base.OnBrowserInitialized();
	}

	[CompilerGenerated]
	private bool? _E000()
	{
		return base.DialogResult = true;
	}
}
