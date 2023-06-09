using System;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

internal class ViewModelLocator
{
	private static IServiceProvider m__E000;

	public MainWindowViewModel MainWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<MainWindowViewModel>();

	public LoginWindowViewModel LoginWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<LoginWindowViewModel>();

	public RequestRestartDialogWindowViewModel RequestRestartDialogWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<RequestRestartDialogWindowViewModel>();

	public DialogWindowViewModel DialogWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<DialogWindowViewModel>();

	public ErrorWindowViewModel ErrorWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<ErrorWindowViewModel>();

	public ProgressWindowViewModel ProgressWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<ProgressWindowViewModel>();

	public SelectLanguageWindowViewModel SelectLanguageWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<SelectLanguageWindowViewModel>();

	public BugReportWindowViewModel BugReportWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<BugReportWindowViewModel>();

	public InstallationWindowViewModel InstallationWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<InstallationWindowViewModel>();

	public LicenseAgreementWindowViewModel LicenseAgreementWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<LicenseAgreementWindowViewModel>();

	public MatchingConfigurationWindowViewModel MatchingConfigurationWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<MatchingConfigurationWindowViewModel>();

	public LabelViewModel LabelViewModel => ViewModelLocator.m__E000.GetRequiredService<LabelViewModel>();

	public FeedbackWindowViewModel FeedbackWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<FeedbackWindowViewModel>();

	public CaptchaWindowViewModel CaptchaWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<CaptchaWindowViewModel>();

	public CodeActivationWindowViewModel CodeActivationWindowViewModel => ViewModelLocator.m__E000.GetRequiredService<CodeActivationWindowViewModel>();

	public static void _E000(IServiceProvider serviceProvider)
	{
		ViewModelLocator.m__E000 = serviceProvider;
	}
}
