using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Eft.Launcher;

public class PausableTimer : IDisposable
{
	[CompilerGenerated]
	private Action<TimerState, TimerState> m__E000;

	[CompilerGenerated]
	private TimerState m__E001;

	[CompilerGenerated]
	private readonly long m__E002;

	private readonly Action<PausableTimer> m__E003;

	private readonly Timer _E004;

	private readonly Stopwatch _E005;

	public TimerState State
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public long Delay
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
	}

	public event Action<TimerState, TimerState> OnStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<TimerState, TimerState> action = this.m__E000;
			Action<TimerState, TimerState> action2;
			do
			{
				action2 = action;
				Action<TimerState, TimerState> value2 = (Action<TimerState, TimerState>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<TimerState, TimerState> action = this.m__E000;
			Action<TimerState, TimerState> action2;
			do
			{
				action2 = action;
				Action<TimerState, TimerState> value2 = (Action<TimerState, TimerState>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public PausableTimer(long delay, Action<PausableTimer> action)
	{
		this.m__E002 = delay;
		this.m__E003 = action ?? throw new ArgumentNullException(_E05B._E000(61227));
		_E004 = new Timer(_E000, null, -1, -1);
		_E005 = new Stopwatch();
	}

	public void Start()
	{
		_E000(TimerState.Running, new TimerState[2]
		{
			TimerState.Ready,
			TimerState.Paused
		}, delegate
		{
			_E004.Change(Delay, -1L);
			_E005.Restart();
		});
	}

	public void Stop()
	{
		_E000(TimerState.Ready, new TimerState[2]
		{
			TimerState.Running,
			TimerState.Paused
		}, delegate
		{
			_E004.Change(-1, -1);
			_E005.Reset();
		});
	}

	public void Pause()
	{
		_E000(TimerState.Paused, new TimerState[1] { TimerState.Running }, delegate
		{
			_E004.Change(-1, -1);
			_E005.Stop();
		});
	}

	public void Resume()
	{
		_E000(TimerState.Running, new TimerState[1] { TimerState.Paused }, delegate
		{
			_E005.Start();
			long dueTime = Math.Max(Delay - _E005.ElapsedMilliseconds, 1L);
			_E004.Change(dueTime, -1L);
		});
	}

	private void _E000(object state)
	{
		Stop();
		this.m__E003(this);
	}

	private bool _E000(TimerState newState, TimerState[] prevStates, Action switchAction)
	{
		lock (_E004)
		{
			if (!prevStates.Contains(State))
			{
				return false;
			}
			switchAction();
			TimerState state = State;
			State = newState;
			this.m__E000?.Invoke(state, newState);
			return true;
		}
	}

	public void Dispose()
	{
		Stop();
		_E004.Dispose();
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E004.Change(Delay, -1L);
		_E005.Restart();
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E004.Change(-1, -1);
		_E005.Reset();
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E004.Change(-1, -1);
		_E005.Stop();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E005.Start();
		long dueTime = Math.Max(Delay - _E005.ElapsedMilliseconds, 1L);
		_E004.Change(dueTime, -1L);
	}
}
