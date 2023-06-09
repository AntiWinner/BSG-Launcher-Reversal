using System;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelDownloaderException : MultichannelDownloadingException
{
	public MultichannelDownloaderException(string message)
		: base(message)
	{
	}

	public MultichannelDownloaderException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public static MultichannelDownloaderException ThereIsNotEnoughSpaceOnDisk(string driveLetter)
	{
		return new MultichannelDownloaderException("There is not enough space on disk " + driveLetter)
		{
			HResult = -2147024784,
			Data = { 
			{
				(object)"driveLetter",
				(object)driveLetter
			} }
		};
	}

	public static MultichannelDownloaderException FailedToDownloadTheFile(string relativeUri)
	{
		return new MultichannelDownloaderException("Failed to download \"" + relativeUri + "\"");
	}
}
