using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bsg.Launcher.Gears;

public class WrappedStream : Stream
{
	protected readonly Stream SourceStream;

	private readonly bool _E000;

	public override bool CanRead => SourceStream.CanRead;

	public override bool CanSeek => SourceStream.CanSeek;

	public override bool CanWrite => SourceStream.CanWrite;

	public override long Length => SourceStream.Length;

	public override long Position
	{
		get
		{
			return SourceStream.Position;
		}
		set
		{
			SourceStream.Position = value;
		}
	}

	public WrappedStream(Stream source, bool closeSource = true)
	{
		SourceStream = source;
		_E000 = closeSource;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return SourceStream.Seek(offset, origin);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return SourceStream.Read(buffer, offset, count);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		return SourceStream.ReadAsync(buffer, offset, count, cancellationToken);
	}

	public override int ReadByte()
	{
		return SourceStream.ReadByte();
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return SourceStream.BeginRead(buffer, offset, count, callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return SourceStream.EndRead(asyncResult);
	}

	public override void SetLength(long value)
	{
		SourceStream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		SourceStream.Write(buffer, offset, count);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		return SourceStream.WriteAsync(buffer, offset, count, cancellationToken);
	}

	public override void WriteByte(byte value)
	{
		SourceStream.WriteByte(value);
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return SourceStream.BeginWrite(buffer, offset, count, callback, state);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		SourceStream.EndWrite(asyncResult);
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		return SourceStream.CopyToAsync(destination, bufferSize, cancellationToken);
	}

	public override void Flush()
	{
		SourceStream.Flush();
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		return SourceStream.FlushAsync(cancellationToken);
	}

	public override void Close()
	{
		if (_E000)
		{
			SourceStream.Close();
		}
		base.Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _E000)
		{
			SourceStream.Dispose();
		}
		base.Dispose(disposing);
	}
}
