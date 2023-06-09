using System;
using System.Net.Http;

namespace Bsg.Launcher.Network.Http;

internal class HttpMessageResponseHandlerException : Exception
{
	public HttpResponseMessage Response { get; }

	public HttpMessageResponseHandlerException(HttpResponseMessage response, string message, Exception innerException)
		: base(message, innerException)
	{
		Response = response;
	}
}
