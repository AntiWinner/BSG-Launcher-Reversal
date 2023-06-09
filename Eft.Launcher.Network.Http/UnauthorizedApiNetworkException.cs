using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Network.Http;

public class UnauthorizedApiNetworkException : ApiNetworkException
{
	public UnauthorizedApiNetworkException(HttpResponseMessage response, int code, string message, JToken data, JToken args, string reason = "", Exception innerException = null)
		: base(response, code, message, data, args, reason, innerException)
	{
	}
}
