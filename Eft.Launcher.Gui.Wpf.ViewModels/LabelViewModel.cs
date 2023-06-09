using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;

namespace Eft.Launcher.Gui.Wpf.ViewModels;

public class LabelViewModel : INotifyPropertyChanged
{
	private readonly ISettingsService _settingsService;

	[CompilerGenerated]
	private readonly Visibility m__E000 = Visibility.Hidden;

	private string _E001;

	[CompilerGenerated]
	private PropertyChangedEventHandler _E002;

	public Visibility Visibility
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	public string Text
	{
		get
		{
			return _E001;
		}
		private set
		{
			_E001 = value;
			_E000(_E05B._E000(26787));
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		[CompilerGenerated]
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = _E002;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref _E002, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = _E002;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref _E002, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	public LabelViewModel(ISettingsService settingsService, IGameUpdateService gameUpdateService)
	{
		_settingsService = settingsService;
	}

	private void _E000()
	{
		Text = _settingsService.SelectedBranch.GameBackendUri?.ToString().Replace(_E05B._E000(26790), "").Replace(_E05B._E000(26798), "")
			.Trim('/');
	}

	private void _E000([CallerMemberName] string propertyName = null)
	{
		_E002?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
