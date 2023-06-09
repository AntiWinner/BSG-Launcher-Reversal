using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Network.Http;

public interface IApiResponse<out TData> : IApiResponse
{
	TData Data { get; }
}
public interface IApiResponse
{
	int Code { get; }

	string Message { get; }

	JToken JsonData { get; }

	HttpResponseMessage HttpResponse { get; }

	bool IsSuccessStatusCode();

	void EnsureSuccessStatusCode();
}
