using System;

namespace Eft.Launcher.Base.Network.Http;

internal class ZlibContentException : Exception
{
	public ZlibContentException(string message)
		: base(message)
	{
	}

	public ZlibContentException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
