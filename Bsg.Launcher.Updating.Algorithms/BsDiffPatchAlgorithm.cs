using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Bsg.Launcher.Updating.Algorithms;

public class BsDiffPatchAlgorithm : IPatchAlgorithm
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MemoryStream _E000;

		public FileStream _E001;

		internal Stream _E000()
		{
			return new MemoryStream(this._E000.GetBuffer());
		}

		internal Stream _E001()
		{
			return new FileStream(this._E001.SafeFileHandle, FileAccess.Read);
		}
	}

	public static long MaxFileSize = 104857600L;

	public byte Id => 1;

	public string Name => _E05B._E000(704);

	public void ApplyPatch(Stream originalFileStream, Stream patchStream, Stream resultFileStream, Action<long, long> onProgress)
	{
		MemoryStream memoryStream = patchStream as MemoryStream;
		object obj;
		if (memoryStream == null)
		{
			FileStream fileStream = patchStream as FileStream;
			if (fileStream == null)
			{
				throw new NotSupportedException();
			}
			obj = (Func<Stream>)(() => new FileStream(fileStream.SafeFileHandle, FileAccess.Read));
		}
		else
		{
			obj = (Func<Stream>)(() => new MemoryStream(memoryStream.GetBuffer()));
		}
		Func<Stream> openPatchStream = (Func<Stream>)obj;
		_E02C._E000(originalFileStream, openPatchStream, resultFileStream);
	}

	public void CreatePatch(Stream oldFileStream, Stream newFileStream, Stream patchOutputStream)
	{
		byte[] array = new byte[oldFileStream.Length];
		oldFileStream.Read(array, 0, array.Length);
		byte[] array2 = new byte[newFileStream.Length];
		newFileStream.Read(array2, 0, array2.Length);
		TimeSpan timeSpan = TimeSpan.FromMinutes(10.0);
		try
		{
			using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeSpan);
			_E02C._E000(array, array2, patchOutputStream, cancellationTokenSource.Token);
		}
		catch (StackOverflowException innerException)
		{
			throw PatchAlgorithmException.BsDiffStackOverflow(oldFileStream.Length, newFileStream.Length, innerException);
		}
		catch (OutOfMemoryException innerException2)
		{
			throw PatchAlgorithmException.BsDiffOutOfMemory(oldFileStream.Length, newFileStream.Length, innerException2);
		}
		catch (OperationCanceledException innerException3)
		{
			throw PatchAlgorithmException.BsDiffOutOfTime(oldFileStream.Length, newFileStream.Length, timeSpan, innerException3);
		}
	}

	public bool IsApplicableFor(long oldFileSize, long newFileSize)
	{
		if (oldFileSize <= MaxFileSize)
		{
			return newFileSize <= MaxFileSize;
		}
		return false;
	}

	public override string ToString()
	{
		return Name + _E05B._E000(713);
	}
}
