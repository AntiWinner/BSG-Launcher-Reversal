using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Bsg.Network.MultichannelDownloading;

public class Metronome : IDisposable
{
	[CompilerGenerated]
	private readonly TimeSpan m__E000;

	private readonly string _E001;

	private Action _E002;

	private CancellationTokenSource _E003;

	public TimeSpan Interval
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	public bool IsStarted => _E003 != null;

	public Metronome(TimeSpan interval, Action onTick, string name = "Metronome")
	{
		this.m__E000 = interval;
		_E002 = onTick ?? throw new ArgumentNullException(_E05B._E000(2192));
		_E001 = name;
	}

	public void Start()
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		if (Interlocked.Exchange(ref _E003, cancellationTokenSource) == cancellationTokenSource)
		{
			throw new InvalidOperationException(_E05B._E000(2201) + _E001 + _E05B._E000(2204));
		}
		Thread thread = new Thread(_E000)
		{
			Priority = ThreadPriority.Lowest,
			IsBackground = true,
			Name = _E001
		};
		_E003.Token.Register(thread.Join);
		thread.Start();
	}

	public void Stop()
	{
		CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref _E003, null);
		if (cancellationTokenSource != null)
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
		}
	}

	public void Tick()
	{
		_E002();
	}

	public void Subscribe(Action callback)
	{
		_E002 = (Action)Delegate.Combine(_E002, callback);
	}

	private void _E000(object state)
	{
		while (true)
		{
			CancellationTokenSource cancellationTokenSource = _E003;
			if (cancellationTokenSource != null && !cancellationTokenSource.Token.WaitHandle.WaitOne(Interval))
			{
				_E002();
				continue;
			}
			break;
		}
	}

	public void Dispose()
	{
		Stop();
	}
}
