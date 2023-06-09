using System;

namespace Bsg.Launcher.Updating;

public class PatchAlgorithmException : Exception
{
	private PatchAlgorithmException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public static PatchAlgorithmException BsDiffStackOverflow(long oldFileSize, long newFileSize, Exception innerException = null)
	{
		return new PatchAlgorithmException("StackOverflowException. Please decrease the BsDiff max file size. Old file size: " + oldFileSize.ToHumanReadableSize() + ", new file size: " + newFileSize.ToHumanReadableSize(), innerException);
	}

	public static PatchAlgorithmException BsDiffOutOfMemory(long oldFileSize, long newFileSize, Exception innerException = null)
	{
		return new PatchAlgorithmException("OutOfMemoryException. Please decrease the BsDiff max file size. Old file size: " + oldFileSize.ToHumanReadableSize() + ", new file size: " + newFileSize.ToHumanReadableSize(), innerException);
	}

	public static PatchAlgorithmException BsDiffOutOfTime(long oldFileSize, long newFileSize, TimeSpan timeout, Exception innerException = null)
	{
		return new PatchAlgorithmException($"TimeoutException. Please decrease the BsDiff max file size. Old file size: {oldFileSize.ToHumanReadableSize()}, new file size: {newFileSize.ToHumanReadableSize()}, timeout: {timeout.TotalSeconds}sec", innerException);
	}
}
