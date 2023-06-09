using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher;

public abstract class LifecycleControllerBase
{
	[CompilerGenerated]
	private Action _E002;

	public event Action OnExiting
	{
		[CompilerGenerated]
		add
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Exit(int exitCode)
	{
		_E002?.Invoke();
		OnExit(exitCode);
	}

	protected internal abstract void ConfigureServices(IServiceCollection services);

	protected internal abstract void Configure(IServiceProvider sp);

	protected internal virtual void OnStart(string[] args)
	{
	}

	protected virtual void OnExit(int exitCode)
	{
	}
}
