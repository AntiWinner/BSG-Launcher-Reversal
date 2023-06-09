using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services.DialogService;

namespace Eft.Launcher.Base.Services.DialogService;

public class DialogServiceMock : IDialogService
{
	[CompilerGenerated]
	private Action<Task> _E000;

	public bool IsLicenseAgreementWindowShowed
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public event Action<Task> WhenAnExceptionIsDisplayed
	{
		[CompilerGenerated]
		add
		{
			Action<Task> action = _E000;
			Action<Task> action2;
			do
			{
				action2 = action;
				Action<Task> value2 = (Action<Task>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Task> action = _E000;
			Action<Task> action2;
			do
			{
				action2 = action;
				Action<Task> value2 = (Action<Task>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public Task<IDialogWindow> CreateDialog(DialogWindowMessage message, Exception exc, params string[] args)
	{
		throw new NotImplementedException();
	}

	public Task<IDialogWindow> CreateDialog(DialogWindowMessage message, params string[] args)
	{
		throw new NotImplementedException();
	}

	public Task<DialogResult> ShowCaptchaWindow(HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler)
	{
		throw new NotImplementedException();
	}

	public Task<DialogResult> ShowDialog(DialogWindowMessage message, Exception exc, params string[] args)
	{
		throw new NotImplementedException();
	}

	public Task<DialogResult> ShowDialog(DialogWindowMessage message, params string[] args)
	{
		throw new NotImplementedException();
	}

	public Task ShowException(Exception exc)
	{
		throw new NotImplementedException();
	}

	public Task<DialogResult> ShowFeedbackWindow(string feedbackFormData)
	{
		throw new NotImplementedException();
	}

	public Task<DialogResult> ShowLicenseAgreementWindow(string document = null)
	{
		throw new NotImplementedException();
	}

	public Task<bool?> ShowCodeActivationWindow()
	{
		throw new NotImplementedException();
	}
}
