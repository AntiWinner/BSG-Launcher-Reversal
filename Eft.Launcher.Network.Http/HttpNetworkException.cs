using System;
using System.Net.Http;
using System.Text;

namespace Eft.Launcher.Network.Http;

public class HttpNetworkException : NetworkException
{
	public HttpResponseMessage Response { get; }

	public HttpNetworkException(HttpResponseMessage response, string reason = "", Exception innerException = null)
		: base(response?.RequestMessage, BuildReasonMessage(response, reason), isLocalProblems: false, innerException)
	{
		Response = response;
	}

	private static string BuildReasonMessage(HttpResponseMessage response, string reason)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(reason))
		{
			stringBuilder.Append(reason);
		}
		if (response != null)
		{
			if (!string.IsNullOrEmpty(reason))
			{
				if (!reason.TrimEnd(' ').EndsWith("."))
				{
					stringBuilder.Append('.');
				}
				if (!reason.EndsWith(" "))
				{
					stringBuilder.Append(' ');
				}
			}
			stringBuilder.Append("Status code: ");
			stringBuilder.Append(response.StatusCode.ToString());
		}
		return stringBuilder.ToString();
	}
}
