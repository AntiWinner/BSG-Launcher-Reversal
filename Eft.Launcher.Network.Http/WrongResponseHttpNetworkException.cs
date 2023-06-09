using System;
using System.Net.Http;

namespace Eft.Launcher.Network.Http;

public class WrongResponseHttpNetworkException : HttpNetworkException
{
	public WrongResponseHttpNetworkException(HttpResponseMessage response, Exception innerException = null)
		: base(response, "Wrong response", innerException)
	{
	}
}
