using System;

namespace Bsg.Launcher.Updating;

public class UpdateException : Exception
{
	private UpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public static UpdateException TheLauncherDoesNotSupportTheInstallationOfThisUpdate()
	{
		return new UpdateException("The launcher does not support the installation of this update. Please update the launcher", null);
	}

	public static UpdateException UnknownAlgorithmId(byte algorithmId)
	{
		return new UpdateException($"Algorithm with id {algorithmId} is unknown", null);
	}

	public static UpdateException UpdateIsCorrupted(string description, Exception innerException = null)
	{
		return new UpdateException("Update is corrupted. " + description, innerException);
	}

	public static UpdateException UnableToUpdateFile(string fileName, string description = null, Exception innerException = null)
	{
		return new UpdateException("Unable to update file \"" + fileName + "\". " + description, innerException);
	}
}
