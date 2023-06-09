using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.DialogService;

public interface IDialogService
{
	bool IsLicenseAgreementWindowShowed { get; }

	event Action<Task> WhenAnExceptionIsDisplayed;

	Task<IDialogWindow> CreateDialog(DialogWindowMessage message, Exception exc, params string[] args);

	Task<IDialogWindow> CreateDialog(DialogWindowMessage message, params string[] args);

	Task<DialogResult> ShowDialog(DialogWindowMessage message, Exception exc, params string[] args);

	Task<DialogResult> ShowDialog(DialogWindowMessage message, params string[] args);

	Task<DialogResult> ShowLicenseAgreementWindow(string document = null);

	Task<bool?> ShowCodeActivationWindow();

	Task<DialogResult> ShowFeedbackWindow(string feedbackFormData);

	Task<DialogResult> ShowCaptchaWindow(HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler);

	Task ShowException(Exception exc);
}
