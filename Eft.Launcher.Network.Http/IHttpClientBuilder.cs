using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eft.Launcher.Network.Http;

public interface IHttpClientBuilder
{
	IHttpClientBuilder AddExceptionHandling(bool ensureSuccessStatusCodes);

	IHttpClientBuilder AddRequestMetadata(bool addLanguage = false, bool addBranchName = false, bool addGameVersion = false);

	IHttpClientBuilder AddAuthentication();

	IHttpClientBuilder AddLogging<TLogger>();

	IHttpClientBuilder AddApiResponses(string apiCodeKey = "err", string apiMessageKey = "errmsg", string apiDataKey = "data", string apiArgsKey = "args", bool throwIfParsingFailed = true);

	IHttpClientBuilder AddJsonResponses();

	IHttpClientBuilder AddCompression();

	IHttpClientBuilder AddCallback(Func<HttpRequestMessage, Task> onRequest, Func<HttpResponseMessage, Task> onResponse);

	IHttpClientBuilder WithBaseAddress(Uri baseAddress);

	HttpClient Build();
}
