using System;

namespace Eft.Launcher.Services.UpdateServices;

public class UpdateFileFormatException : Exception
{
	public UpdateFileFormatException(string message)
		: base(message)
	{
	}

	public UpdateFileFormatException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
