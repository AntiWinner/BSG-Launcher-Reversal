using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Eft.Launcher;

public sealed class AverageValueCalculator : IDisposable
{
	private const int m__E000 = 20;

	[CompilerGenerated]
	private int _E001;

	[CompilerGenerated]
	private int _E002;

	private readonly Queue<double> _E003 = new Queue<double>(20);

	private readonly Timer _E004 = new Timer(1000.0);

	public readonly double FileSize;

	private double _E005;

	private double _E006;

	public int SecondsLeft
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		private set
		{
			_E001 = value;
		}
	}

	public int CurrentSpeed
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public AverageValueCalculator(double fileSize, double bytesTransferred)
	{
		FileSize = fileSize;
		_E005 = bytesTransferred;
		SecondsLeft = -1;
		CurrentSpeed = -1;
		_E004.Elapsed += _E000;
		_E004.Start();
	}

	private void _E000(object sender, ElapsedEventArgs e)
	{
		lock (_E003)
		{
			while (_E003.Count > 20)
			{
				_E003.Dequeue();
			}
			double num = _E005;
			double item = num - _E006;
			_E006 = num;
			_E003.Enqueue(item);
			double num2 = _E003.Average();
			CurrentSpeed = (int)num2;
			double num3 = FileSize - num;
			SecondsLeft = ((num2 > 0.0) ? ((int)TimeSpan.FromSeconds(num3 / num2).TotalSeconds) : (-1));
		}
	}

	public void SetProgress(double bytesTransferred)
	{
		_E005 = bytesTransferred;
	}

	public void Dispose()
	{
		try
		{
			_E004.Dispose();
		}
		catch
		{
		}
		_E005 = 0.0;
		SecondsLeft = -1;
		CurrentSpeed = -1;
	}
}
