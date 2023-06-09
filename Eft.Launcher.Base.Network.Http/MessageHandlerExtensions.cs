using System.Net.Http;

namespace Eft.Launcher.Base.Network.Http;

public static class MessageHandlerExtensions
{
	public static HttpRequestMessage SkipAuth(this HttpRequestMessage requestMessage)
	{
		requestMessage.Properties.Add(_E05B._E000(15869), null);
		return requestMessage;
	}

	public static HttpRequestMessage PreventTokenUpdate(this HttpRequestMessage requestMessage)
	{
		requestMessage.Properties.Add(_E05B._E000(15819), null);
		return requestMessage;
	}

	public static HttpRequestMessage MarkAsSecretRequestData(this HttpRequestMessage requestMessage)
	{
		requestMessage.Properties.Add(_E05B._E000(15418), null);
		return requestMessage;
	}

	public static bool HasSecretRequestData(this HttpRequestMessage requestMessage)
	{
		return requestMessage.Properties.ContainsKey(_E05B._E000(15418));
	}

	public static HttpContent GetOriginalContent(this HttpContent httpContent)
	{
		if (!(httpContent is ZlibContent zlibContent))
		{
			return httpContent;
		}
		return zlibContent.OriginalContent;
	}
}
