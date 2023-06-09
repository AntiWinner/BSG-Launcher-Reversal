using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

public class MatchingConfigurationWindow : BrowserWindowBase, IMatchingConfigurationWindowDelegate, IWindowDelegate, IDialogWindow, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MatchingConfigurationWindow _E000;

		public IServiceProvider _E001;

		internal void _E000()
		{
			this._E000 = new MatchingConfigurationWindow(_E001)
			{
				Owner = Application.Current.MainWindow
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public MatchingConfigurationWindow _E000;

		public DialogResult _E001;

		internal void _E000()
		{
			bool? flag = ((Window)this._E000).ShowDialog();
			_E001 = ((flag == false) ? Eft.Launcher.Services.DialogService.DialogResult.Negative : ((flag == true) ? Eft.Launcher.Services.DialogService.DialogResult.Positive : Eft.Launcher.Services.DialogService.DialogResult.Cancelled));
		}
	}

	[CompilerGenerated]
	private string _E00C;

	private readonly JsonSerializer _E00D;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	public string MatchingConfiguration
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		set
		{
			_E00C = value;
		}
	}

	public MatchingConfigurationWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_E00D = serviceProvider.GetRequiredService<JsonSerializer>();
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public void RenderForm(DatacenterDto[] datacenters)
	{
		string text = JArray.FromObject(datacenters, _E00D).ToString(Formatting.None);
		ExecuteJavaScript(_E05B._E000(28583) + text + _E05B._E000(24948), _E05B._E000(28595));
	}

	public void SetPing(string datacenter, int ping)
	{
		ExecuteJavaScript(string.Format(_E05B._E000(28544), datacenter, ping), string.Empty);
	}

	public static async Task<MatchingConfigurationWindow> Create(IServiceProvider serviceProvider)
	{
		MatchingConfigurationWindow result = null;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			result = new MatchingConfigurationWindow(serviceProvider)
			{
				Owner = Application.Current.MainWindow
			};
		});
		return result;
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
		if (!this._E003)
		{
			this._E003 = true;
			Uri resourceLocator = new Uri(_E05B._E000(28564), UriKind.Relative);
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
			this._E003 = true;
			break;
		}
	}

	[SpecialName]
	bool? IMatchingConfigurationWindowDelegate.get_DialogResult()
	{
		return base.DialogResult;
	}

	[SpecialName]
	void IMatchingConfigurationWindowDelegate.set_DialogResult(bool? value)
	{
		base.DialogResult = value;
	}
}
