using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Bsg.Launcher.Json.Converters;
using CefSharp.Wpf;
using Eft.Launcher.Services.DialogService;
using Newtonsoft.Json;

namespace Eft.Launcher.Gui.Wpf.Views;

[JsonObject(MemberSerialization.OptIn)]
public class DialogWindow : BrowserWindowBase, IDialogWindowDelegate, IWindowDelegate, IDialogWindow, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public DialogWindow _E000;

		public DialogResult _E001;

		internal void _E000()
		{
			bool? flag = ((Window)this._E000).ShowDialog();
			_E001 = ((flag == false) ? Eft.Launcher.Services.DialogService.DialogResult.Negative : ((flag == true) ? Eft.Launcher.Services.DialogService.DialogResult.Positive : Eft.Launcher.Services.DialogService.DialogResult.Cancelled));
		}
	}

	[JsonProperty(PropertyName = "args")]
	public string[] Args;

	[JsonProperty(PropertyName = "exception")]
	[JsonConverter(typeof(GuiExceptionConverter))]
	public Exception Exception;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	[JsonProperty(PropertyName = "code")]
	public DialogWindowMessage Message { get; }

	internal DialogWindow(IServiceProvider serviceProvider, DialogWindowMessage message, Exception exc, params string[] args)
		: base(serviceProvider)
	{
		Message = message;
		Args = args;
		Exception = exc;
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	protected override async Task OnBrowserInitialized()
	{
		await base.OnBrowserInitialized();
		ExecuteJavaScript(_E05B._E000(24988) + JsonConvert.SerializeObject(this, Formatting.None) + _E05B._E000(24948));
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
			Uri resourceLocator = new Uri(_E05B._E000(25011), UriKind.Relative);
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
	bool? IDialogWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void IDialogWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000()
	{
		return base.OnBrowserInitialized();
	}
}
