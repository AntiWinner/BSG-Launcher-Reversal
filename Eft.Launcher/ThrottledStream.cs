using System;
using System.IO;
using System.Threading;

namespace Eft.Launcher;

public class ThrottledStream : Stream
{
	public const long Infinite = 0L;

	private readonly Stream _E000;

	private long _E001;

	private long _E002;

	private long _E003;

	protected long CurrentMilliseconds => Environment.TickCount;

	public long MaximumBytesPerSecond
	{
		get
		{
			return _E001;
		}
		set
		{
			if (MaximumBytesPerSecond != value)
			{
				_E001 = value;
				Reset();
			}
		}
	}

	public override bool CanRead => _E000.CanRead;

	public override bool CanSeek => _E000.CanSeek;

	public override bool CanWrite => _E000.CanWrite;

	public override long Length => _E000.Length;

	public override long Position
	{
		get
		{
			return _E000.Position;
		}
		set
		{
			_E000.Position = value;
		}
	}

	public ThrottledStream(Stream baseStream)
		: this(baseStream, 0L)
	{
	}

	public ThrottledStream(Stream baseStream, long maximumBytesPerSecond)
	{
		if (maximumBytesPerSecond < 0)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(61228), maximumBytesPerSecond, _E05B._E000(61190));
		}
		_E000 = baseStream ?? throw new ArgumentNullException(_E05B._E000(61309));
		_E001 = maximumBytesPerSecond;
		_E003 = CurrentMilliseconds;
		_E002 = 0L;
	}

	public override void Flush()
	{
		_E000.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		Throttle(count);
		return _E000.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _E000.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		_E000.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		Throttle(count);
		_E000.Write(buffer, offset, count);
	}

	public override string ToString()
	{
		return _E000.ToString();
	}

	protected void Throttle(int bufferSizeInBytes)
	{
		if (_E001 <= 0 || bufferSizeInBytes <= 0)
		{
			return;
		}
		_E002 += bufferSizeInBytes;
		long num = CurrentMilliseconds - _E003;
		if (num <= 0 || _E002 * 1000 / num <= _E001)
		{
			return;
		}
		int num2 = (int)(_E002 * 1000 / _E001 - num);
		if (num2 > 1)
		{
			try
			{
				Thread.Sleep(num2);
			}
			catch (ThreadAbortException)
			{
			}
			Reset();
		}
	}

	protected void Reset()
	{
		if (CurrentMilliseconds - _E003 > 1000)
		{
			_E002 = 0L;
			_E003 = CurrentMilliseconds;
		}
	}
}
