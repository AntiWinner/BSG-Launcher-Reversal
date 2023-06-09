using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Network.Http;

public class ApiNetworkException : HttpNetworkException
{
	public int ApiCode { get; }

	public string ApiMessage { get; }

	public JToken ApiData { get; }

	public JToken ApiArgs { get; }

	public ApiNetworkException(HttpResponseMessage response, int apiCode, string apiMessage, JToken apiData, JToken apiArgs, string reason = "", Exception innerException = null)
		: base(response, BuildReasonMessage(apiCode, reason), innerException)
	{
		ApiCode = apiCode;
		ApiMessage = apiMessage;
		ApiData = apiData;
		ApiArgs = apiArgs;
	}

	private static string BuildReasonMessage(int apiCode, string reason)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(reason))
		{
			stringBuilder.Append(reason);
			stringBuilder.Append(". ");
		}
		stringBuilder.Append("API Code: ");
		stringBuilder.Append(apiCode.ToString());
		return stringBuilder.ToString();
	}
}
