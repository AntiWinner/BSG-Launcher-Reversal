using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eft.Launcher.Gui.Wpf;

public class AsyncCommand : ICommand
{
	private readonly Func<Task> _E000;

	private readonly Func<bool> _E001;

	private bool _E002;

	[CompilerGenerated]
	private EventHandler _E003;

	public event EventHandler CanExecuteChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E003;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E003;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E003, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public AsyncCommand(Func<Task> execute)
		: this(execute, () => true)
	{
	}

	public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
	{
		this._E000 = execute;
		_E001 = canExecute;
	}

	public bool CanExecute(object parameter)
	{
		if (_E002)
		{
			return !_E001();
		}
		return true;
	}

	public async void Execute(object parameter)
	{
		_E002 = true;
		OnCanExecuteChanged();
		try
		{
			await this._E000();
		}
		finally
		{
			_E002 = false;
			OnCanExecuteChanged();
		}
	}

	protected virtual void OnCanExecuteChanged()
	{
		_E003?.Invoke(this, new EventArgs());
	}
}
