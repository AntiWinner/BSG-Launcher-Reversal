using System;
using System.Net.Http;

namespace Eft.Launcher.Network.Http;

public class NetworkException : Exception
{
	public HttpRequestMessage Request { get; }

	public bool IsLocalProblems { get; }

	public NetworkException(HttpRequestMessage request, string reason, bool isLocalProblems = false, Exception innerException = null)
		: base((isLocalProblems ? "Local problems" : "Error") + " on " + (request?.Method?.ToString() ?? "UNKNOWN_METHOD") + " " + (request?.RequestUri?.ToString() ?? "UNKNOWN_URI") + ((reason == null) ? "" : (". " + reason)), innerException)
	{
		Request = request;
		IsLocalProblems = isLocalProblems;
	}

	public NetworkException(HttpRequestMessage request, bool isLocalProblems = false, Exception innerException = null)
		: this(request, null, isLocalProblems, innerException)
	{
	}
}
