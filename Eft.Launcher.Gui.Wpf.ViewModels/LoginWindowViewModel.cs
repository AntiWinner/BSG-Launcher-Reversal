using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using Eft.Launcher.Gui.Wpf.Views;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Services.AuthService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class LoginWindowViewModel : WindowViewModelBase
{
	[CompilerGenerated]
	private sealed class _E002
	{
		public LoginWindowViewModel _E000;

		public string _E001;

		public string _E002;

		public string _E003;

		internal async void _E000(object w)
		{
			try
			{
				await this._E000._E000(_E001, _E002, _E003);
			}
			catch (Exception exc)
			{
				this._E000.m__E004.ShowError(exc);
			}
			finally
			{
				this._E000._E000();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public LoginWindowViewModel _E000;

		public string _E001;

		public string _E002;

		public string _E003;

		internal async void _E000(object w)
		{
			_ = 1;
			try
			{
				await this._E000._E014.ActivateHardware(_E001, _E002);
				await this._E000._E000(_E001, _E003, null);
			}
			catch (Exception exc)
			{
				this._E000.m__E004.ShowError(exc);
			}
			finally
			{
				this._E000._E000();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public LoginWindowViewModel _E000;

		public string _E001;

		public string _E002;

		public IJavascriptCallback _E003;

		internal async Task _E000()
		{
			JToken jToken = null;
			try
			{
				jToken = await this._E000._E014.BindPhone(_E001, _E002);
			}
			catch (Exception exc)
			{
				this._E000.m__E004.ShowError(exc);
			}
			finally
			{
				await _E003.ExecuteAsync(jToken?.ToString(Formatting.None));
				this._E000._E000();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public LoginWindowViewModel _E000;

		public string _E001;

		public string _E002;

		public string _E003;

		public IJavascriptCallback _E004;

		internal async Task _E000()
		{
			JToken jToken = null;
			try
			{
				_ = 1;
				try
				{
					jToken = await this._E000._E014.VerifyPhone(_E001, _E002);
					await this._E000._E000(_E001, _E003, null);
				}
				catch (Exception exc)
				{
					this._E000.m__E004.ShowError(exc);
				}
			}
			finally
			{
				await _E004.ExecuteAsync(jToken?.ToString(Formatting.None));
				this._E000._E000();
			}
		}
	}

	private readonly IServiceProvider _E00F;

	private readonly ISettingsService _settingsService;

	private readonly ILauncherUpdateService _launcherUpdateService;

	private readonly IAuthService _E014;

	private readonly int[] _E015 = new int[5] { 0, 5, 15, 30, 60 };

	private readonly System.Timers.Timer _E016;

	private ILoginWindowDelegate m__E004;

	private bool _E017;

	private int _E018;

	public LoginWindowViewModel(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_E00F = serviceProvider;
		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_launcherUpdateService = serviceProvider.GetRequiredService<ILauncherUpdateService>();
		_E014 = serviceProvider.GetRequiredService<IAuthService>();
		_E016 = new System.Timers.Timer(500.0);
		_E016.Elapsed += delegate
		{
			bool flag = Control.IsKeyLocked(Keys.Capital);
			if (flag != _E017)
			{
				_E017 = flag;
				this.m__E004?.SetCapsLockState(_E017);
			}
		};
		_E016.Start();
	}

	internal override async Task _E001(IWindowDelegate windowDelegate)
	{
		this.m__E004 = (ILoginWindowDelegate)windowDelegate;
		await base._E001(windowDelegate);
		try
		{
			_launcherUpdateService.CheckForStartAfterUpdate();
		}
		catch (Exception exception)
		{
			Logger.LogError(exception, _E05B._E000(29613));
		}
		try
		{
			LauncherUpdate launcherCheckForUpdateResult = await _launcherUpdateService.CheckForLauncherUpdate();
			if (launcherCheckForUpdateResult.UpdateIsRequired)
			{
				await _launcherUpdateService.BeginUpdateLauncher(launcherCheckForUpdateResult);
				if (await this.m__E004.RequestRestartForUpdate())
				{
					_launcherUpdateService.EndUpdateLauncher(launcherCheckForUpdateResult);
				}
				this.m__E004.Close();
			}
		}
		catch (NetworkException ex) when (ex.IsLocalProblems)
		{
			Logger.LogWarning(_E05B._E000(29590));
		}
		catch (Exception exception2)
		{
			Logger.LogError(exception2, _E05B._E000(29651));
		}
		await this.m__E004.LoadAsync(_settingsService.LoginPageUri.ToString());
		_E017 = Control.IsKeyLocked(Keys.Capital);
		this.m__E004.SetCapsLockState(_E017);
	}

	private bool _E000()
	{
		bool num = Interlocked.CompareExchange(ref _E018, 1, 0) == 0;
		if (!num)
		{
			Logger.LogWarning(_E05B._E000(29520));
		}
		return num;
	}

	private void _E000()
	{
		_E018 = 0;
	}

	private async Task _E000(string email, string password, string captcha)
	{
		email = email.Trim();
		password = password.GetMd5();
		await _E014.LogIn(email, password, captcha);
		await System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			MainWindow mainWindow = new MainWindow(_E00F, startedFromLoginPage: true);
			mainWindow.Show();
			System.Windows.Application.Current.MainWindow = mainWindow;
			this.m__E004.Close();
		});
	}

	protected override void OnClosing()
	{
		_E016.Stop();
		_E016.Dispose();
		base.OnClosing();
	}

	[DebuggerHidden]
	public void Login(string email, string password, string captcha)
	{
		LogJsDotNetCall(email.ToSecretData(), password?.GetHexHashCode(), captcha?.GetHexHashCode());
		if (!this._E000())
		{
			return;
		}
		ThreadPool.QueueUserWorkItem(async delegate
		{
			try
			{
				await _E000(email, password, captcha);
			}
			catch (Exception exc)
			{
				this.m__E004.ShowError(exc);
			}
			finally
			{
				this._E000();
			}
		});
	}

	[DebuggerHidden]
	public void ActivateDevice(string email, string password, string code)
	{
		LogJsDotNetCall(email.ToSecretData(), password?.GetHexHashCode(), code);
		if (!this._E000())
		{
			return;
		}
		ThreadPool.QueueUserWorkItem(async delegate
		{
			_ = 1;
			try
			{
				await _E014.ActivateHardware(email, code);
				await _E000(email, password, null);
			}
			catch (Exception exc)
			{
				this.m__E004.ShowError(exc);
			}
			finally
			{
				this._E000();
			}
		});
	}

	[DebuggerHidden]
	public void VerifyPhone(string email, string phone, IJavascriptCallback callback)
	{
		LogJsDotNetCall(email.ToSecretData(), phone.ToSecretData());
		if (!this._E000())
		{
			return;
		}
		Task.Run(async delegate
		{
			JToken jToken = null;
			try
			{
				jToken = await _E014.BindPhone(email, phone);
			}
			catch (Exception exc)
			{
				this.m__E004.ShowError(exc);
			}
			finally
			{
				await callback.ExecuteAsync(jToken?.ToString(Formatting.None));
				this._E000();
			}
		});
	}

	[DebuggerHidden]
	public void VerifyCode(string email, string password, string verificationCode, IJavascriptCallback callback)
	{
		LogJsDotNetCall(email.ToSecretData(), password?.GetHexHashCode(), verificationCode);
		if (!this._E000())
		{
			return;
		}
		Task.Run(async delegate
		{
			JToken jToken = null;
			try
			{
				_ = 1;
				try
				{
					jToken = await _E014.VerifyPhone(email, verificationCode);
					await _E000(email, password, null);
				}
				catch (Exception exc)
				{
					this.m__E004.ShowError(exc);
				}
			}
			finally
			{
				await callback.ExecuteAsync(jToken?.ToString(Formatting.None));
				this._E000();
			}
		});
	}

	[DebuggerHidden]
	public void SetRemember(bool remember)
	{
		LogJsDotNetCall(Logger, remember);
		_settingsService.KeepLoggedIn = remember;
	}

	[DebuggerHidden]
	public string GetLogin()
	{
		LogJsDotNetCall();
		if (!_settingsService.SaveLogin)
		{
			return null;
		}
		return _settingsService.LoginOrEmail;
	}

	[CompilerGenerated]
	private void _E000(object sender, ElapsedEventArgs e)
	{
		bool flag = Control.IsKeyLocked(Keys.Capital);
		if (flag != _E017)
		{
			_E017 = flag;
			this.m__E004?.SetCapsLockState(_E017);
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E000(IWindowDelegate windowDelegate)
	{
		return base._E001(windowDelegate);
	}

	[CompilerGenerated]
	private void _E001()
	{
		MainWindow mainWindow = new MainWindow(_E00F, startedFromLoginPage: true);
		mainWindow.Show();
		System.Windows.Application.Current.MainWindow = mainWindow;
		this.m__E004.Close();
	}
}
