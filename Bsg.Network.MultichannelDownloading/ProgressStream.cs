using System;
using System.IO;

namespace Bsg.Network.MultichannelDownloading;

public class ProgressStream : Stream
{
	private readonly Stream _E000;

	private readonly Action<int> _E001;

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

	public ProgressStream(Stream originalStream, Action<int> readCallback)
	{
		_E000 = originalStream;
		_E001 = readCallback;
	}

	public override void Flush()
	{
		_E000.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = _E000.Read(buffer, offset, count);
		_E001?.Invoke(num);
		return num;
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
		_E000.Write(buffer, offset, count);
	}
}
