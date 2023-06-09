using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Bsg.Launcher.Utils;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.DialogService;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.Services;

public class DialogService : IDialogService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public DialogWindow _E000;

		public DialogService _E001;

		public DialogWindowMessage message;

		public Exception exc;

		public string[] _E002;

		internal void _E000()
		{
			this._E000 = new DialogWindow(_E001._E002, message, exc, _E002)
			{
				Owner = Application.Current.MainWindow
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public DialogService _E000;

		public string _E001;

		public LicenseAgreementWindow _E002;

		internal void _E000()
		{
			_E002 = new LicenseAgreementWindow(this._E000._E002, _E001)
			{
				Owner = Application.Current.MainWindow
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public FeedbackWindow _E000;

		public DialogService _E001;

		public string _E002;

		internal void _E000()
		{
			this._E000 = new FeedbackWindow(_E001._E002, _E002)
			{
				Owner = Application.Current.MainWindow
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public CaptchaWindow _E000;

		public DialogService _E001;

		public HttpResponseMessage responseWithCaptcha;

		public HttpClientHandler _E002;

		internal void _E000()
		{
			this._E000 = new CaptchaWindow(_E001._E002, responseWithCaptcha, _E002)
			{
				Owner = Application.Current.MainWindow
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public DialogService _E000;

		public Exception exc;

		internal void _E000()
		{
			Window owner = (from Window w in (Application.Current.MainWindow as MainWindow)?.OwnedWindows
				where !(w is DialogWindow) && !(w is ErrorWindow)
				select w).LastOrDefault() ?? Application.Current.MainWindow;
			ErrorWindow errorWindow = new ErrorWindow(this._E000._E002, this._E000.m__E003.SerializeForUi(exc));
			errorWindow.ShowInTaskbar = false;
			errorWindow.ShowActivated = true;
			errorWindow.Owner = owner;
			errorWindow.ShowDialog();
		}
	}

	[CompilerGenerated]
	private Action<Task> m__E000;

	[CompilerGenerated]
	private bool _E001;

	private readonly IServiceProvider _E002;

	private readonly ExceptionAdapter m__E003;

	private Task<bool?> _E004;

	public bool IsLicenseAgreementWindowShowed
	{
		[CompilerGenerated]
		get
		{
			return this._E001;
		}
		[CompilerGenerated]
		private set
		{
			this._E001 = value;
		}
	}

	public event Action<Task> WhenAnExceptionIsDisplayed
	{
		[CompilerGenerated]
		add
		{
			Action<Task> action = this.m__E000;
			Action<Task> action2;
			do
			{
				action2 = action;
				Action<Task> value2 = (Action<Task>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Task> action = this.m__E000;
			Action<Task> action2;
			do
			{
				action2 = action;
				Action<Task> value2 = (Action<Task>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public DialogService(IServiceProvider serviceProvider)
	{
		this._E002 = serviceProvider;
		this.m__E003 = serviceProvider.GetRequiredService<ExceptionAdapter>();
	}

	public async Task<IDialogWindow> CreateDialog(DialogWindowMessage message, Exception exc, params string[] args)
	{
		DialogWindow result = null;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			result = new DialogWindow(this._E002, message, exc, args)
			{
				Owner = Application.Current.MainWindow
			};
		});
		return result;
	}

	public Task<IDialogWindow> CreateDialog(DialogWindowMessage message, params string[] args)
	{
		return CreateDialog(message, null, args);
	}

	public async Task<DialogResult> ShowDialog(DialogWindowMessage message, Exception exc, params string[] args)
	{
		return await (await CreateDialog(message, exc, args)).ShowDialog();
	}

	public Task<DialogResult> ShowDialog(DialogWindowMessage message, params string[] args)
	{
		return ShowDialog(message, null, args);
	}

	public async Task<DialogResult> ShowLicenseAgreementWindow(string document = null)
	{
		try
		{
			IsLicenseAgreementWindowShowed = true;
			LicenseAgreementWindow licenseAgreementWindow = null;
			await Application.Current.Dispatcher.BeginInvoke((Action)delegate
			{
				licenseAgreementWindow = new LicenseAgreementWindow(this._E002, document)
				{
					Owner = Application.Current.MainWindow
				};
			});
			return await ((IDialogWindow)licenseAgreementWindow).ShowDialog();
		}
		finally
		{
			IsLicenseAgreementWindowShowed = false;
		}
	}

	public Task<bool?> ShowCodeActivationWindow()
	{
		Task<bool?> task = Interlocked.CompareExchange(ref this._E004, null, null);
		if (task != null)
		{
			return task;
		}
		this._E004 = Application.Current.Dispatcher.InvokeAsync(delegate
		{
			bool? result = new CodeActivationWindow(this._E002)
			{
				Owner = Application.Current.MainWindow
			}.ShowDialog();
			this._E004 = null;
			return result;
		}).Task;
		return this._E004;
	}

	public async Task<DialogResult> ShowFeedbackWindow(string feedbackFormData)
	{
		FeedbackWindow feedbackWindow = null;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			feedbackWindow = new FeedbackWindow(this._E002, feedbackFormData)
			{
				Owner = Application.Current.MainWindow
			};
		});
		return await ((IDialogWindow)feedbackWindow).ShowDialog();
	}

	public async Task<DialogResult> ShowCaptchaWindow(HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler)
	{
		CaptchaWindow captchaWindow = null;
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			captchaWindow = new CaptchaWindow(this._E002, responseWithCaptcha, clientHandler)
			{
				Owner = Application.Current.MainWindow
			};
		});
		return await ((IDialogWindow)captchaWindow).ShowDialog();
	}

	public Task ShowException(Exception exc)
	{
		Task task = Application.Current.Dispatcher.InvokeAsync(delegate
		{
			Window owner = (from Window w in (Application.Current.MainWindow as MainWindow)?.OwnedWindows
				where !(w is DialogWindow) && !(w is ErrorWindow)
				select w).LastOrDefault() ?? Application.Current.MainWindow;
			ErrorWindow errorWindow = new ErrorWindow(this._E002, this.m__E003.SerializeForUi(exc));
			errorWindow.ShowInTaskbar = false;
			errorWindow.ShowActivated = true;
			errorWindow.Owner = owner;
			errorWindow.ShowDialog();
		}).Task;
		this.m__E000?.Invoke(task);
		return task;
	}

	[CompilerGenerated]
	private bool? _E000()
	{
		bool? result = new CodeActivationWindow(this._E002)
		{
			Owner = Application.Current.MainWindow
		}.ShowDialog();
		this._E004 = null;
		return result;
	}
}
