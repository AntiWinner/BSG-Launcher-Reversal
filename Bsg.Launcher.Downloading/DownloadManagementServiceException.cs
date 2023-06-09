using System;

namespace Bsg.Launcher.Downloading;

public class DownloadManagementServiceException : Exception
{
	private DownloadManagementServiceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public static DownloadManagementServiceException UnableToCreateDownloaderWithCurrentSettings()
	{
		return new DownloadManagementServiceException("With the current settings, it is not possible to create a downloader", null);
	}

	public static DownloadManagementServiceException ErrorDownloadingUpdates()
	{
		return new DownloadManagementServiceException("Error downloading updates", null);
	}

	public static DownloadManagementServiceException WrongUri(string uri)
	{
		return new DownloadManagementServiceException("Wrong URI: \"" + uri + "\"", null);
	}

	public static DownloadManagementServiceException WrongDestinationPath(string path)
	{
		return new DownloadManagementServiceException("Wrong destination path: \"" + path + "\"", null);
	}
}
