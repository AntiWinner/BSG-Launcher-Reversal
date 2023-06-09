using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class CodeActivationWindowViewModel : WindowViewModelBase
{
	private static readonly Regex _E005 = new Regex(_E05B._E000(27081));

	private readonly IGameBackendService _E006;

	private readonly ISiteCommunicationService _E007;

	private readonly ISettingsService _settingsService;

	private ICodeActivationWindowDelegate _E004;

	public CodeActivationWindowViewModel(IServiceProvider serviceProvider, IGameBackendService gameBackendService, ISiteCommunicationService siteCommunicationService, ISettingsService settingsService)
		: base(serviceProvider)
	{
		_E006 = gameBackendService;
		_E007 = siteCommunicationService;
		_settingsService = settingsService;
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		_E004 = (ICodeActivationWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		await _E004.LoadAsync(_settingsService.CodeActivationPageUri.ToString());
	}

	[DebuggerHidden]
	public async void Ok(string activationCode)
	{
		LogJsDotNetCall();
		if (activationCode == null)
		{
			return;
		}
		try
		{
			activationCode = activationCode.Trim();
			JToken jToken = ((!_E005.IsMatch(activationCode)) ? (await _E006.ActivateCode(activationCode)) : (await _E007.ActivateCode(activationCode)));
			JToken data = jToken;
			Task<UserProfile> task = LoadProfiles();
			_E004.Success(data);
			await task;
		}
		catch (Exception exc)
		{
			_E004.ShowError(exc);
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}
}
