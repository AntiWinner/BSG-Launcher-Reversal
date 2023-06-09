using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using CefSharp.Wpf;
using Eft.Launcher.Services.BugReportService;

namespace Eft.Launcher.Gui.Wpf.Views;

public class BugReportWindow : BrowserWindowBase, IBugReportWindowDelegate, IWindowDelegate, IComponentConnector
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public IServiceProvider _E000;

		internal void _E000()
		{
			if (BugReportWindow.m__E001 != null)
			{
				BugReportWindow.m__E001.Focus();
				return;
			}
			BugReportWindow.m__E001 = new BugReportWindow(this._E000)
			{
				Owner = Application.Current.MainWindow
			};
			BugReportWindow.m__E001.Show();
			if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
			{
				BugReportWindow.m__E001.WindowState = WindowState.Maximized;
			}
		}
	}

	private readonly IServiceProvider m__E000;

	private static BugReportWindow m__E001;

	private ProgressWindow _E002;

	internal ChromiumWebBrowser Browser;

	internal LoadingAnimation LoadingAnimation;

	private bool _E003;

	private BugReportWindow(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		this.m__E000 = serviceProvider;
		DoubleClickSwitchingEnabled = true;
		InitializeComponent();
		Initialize(Browser, LoadingAnimation);
	}

	public static void ShowWindow(IServiceProvider serviceProvider)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			if (BugReportWindow.m__E001 != null)
			{
				BugReportWindow.m__E001.Focus();
			}
			else
			{
				BugReportWindow.m__E001 = new BugReportWindow(serviceProvider)
				{
					Owner = Application.Current.MainWindow
				};
				BugReportWindow.m__E001.Show();
				if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
				{
					BugReportWindow.m__E001.WindowState = WindowState.Maximized;
				}
			}
		});
	}

	public void HideBugReportSendingProgress()
	{
		base.Dispatcher.BeginInvoke((Action)delegate
		{
			if (!_E002.IsClosed)
			{
				_E002.Close();
			}
			_E002 = null;
		});
	}

	public void ShowBugReportSendingProgress()
	{
		base.Dispatcher.BeginInvoke((Action)delegate
		{
			_E002 = new ProgressWindow(this.m__E000)
			{
				Owner = (base.IsClosed ? Application.Current.MainWindow : this)
			};
			_E002.ShowDialog();
		});
	}

	public void UpdateBugReportSendingProgress(BugReportSendingState state, int progress)
	{
		if (_E002 != null && !_E002.IsClosed)
		{
			ProgressWindowMessage message = ((state == BugReportSendingState.CollectingServerAvailabilityInfo) ? ProgressWindowMessage.CollectingServerAvailabilityInfo : ProgressWindowMessage.UploadingBugReport);
			_E002.SetMessage(message);
			_E002.SetProgress(progress);
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		BugReportWindow.m__E001 = null;
		base.OnClosing(e);
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
			Uri resourceLocator = new Uri(_E05B._E000(25293), UriKind.Relative);
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
	private void _E000()
	{
		if (!_E002.IsClosed)
		{
			_E002.Close();
		}
		_E002 = null;
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E002 = new ProgressWindow(this.m__E000)
		{
			Owner = (base.IsClosed ? Application.Current.MainWindow : this)
		};
		_E002.ShowDialog();
	}
}
