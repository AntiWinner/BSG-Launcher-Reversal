using System;

namespace Bsg.Launcher.Services.AuthService;

public class AuthServiceException : Exception
{
	protected AuthServiceException()
	{
	}

	public AuthServiceException(string message)
		: base(message)
	{
	}

	public AuthServiceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
