using System;
using System.Net.Http;
using Eft.Launcher.Network.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Network.Http;

internal class ApiResponse<TData> : ApiResponse, IApiResponse<TData>, IApiResponse
{
	public TData Data { get; }

	private ApiResponse(int code, string message, JToken jsonData, JToken jsonArgs, HttpResponseMessage httpResponse, JsonSerializer jsonSerializer)
		: base(code, message, jsonData, jsonArgs, httpResponse)
	{
		Data = ApiResponse.ConvertData<TData>(jsonData, jsonSerializer);
	}

	internal static ApiResponse<TData> FromHttpResponse(HttpResponseMessage httpResponse, JsonSerializer jsonSerializer)
	{
		ApiContent apiContent = ApiResponse.GetApiContent(httpResponse);
		return new ApiResponse<TData>(apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args, httpResponse, jsonSerializer);
	}
}
internal class ApiResponse : IApiResponse
{
	public int Code { get; }

	public string Message { get; }

	public JToken JsonData { get; }

	public JToken JsonArgs { get; }

	public HttpResponseMessage HttpResponse { get; }

	protected ApiResponse(int code, string message, JToken data, JToken args, HttpResponseMessage httpResponse)
	{
		Code = code;
		Message = message;
		JsonData = data;
		JsonArgs = args;
		HttpResponse = httpResponse;
	}

	public bool IsSuccessStatusCode()
	{
		return Code == 0;
	}

	public void EnsureSuccessStatusCode()
	{
		if (IsSuccessStatusCode())
		{
			return;
		}
		throw new ApiNetworkException(HttpResponse, Code, Message, JsonData, JsonArgs);
	}

	internal static ApiResponse FromHttpResponse(HttpResponseMessage httpResponse)
	{
		ApiContent apiContent = GetApiContent(httpResponse);
		return new ApiResponse(apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args, httpResponse);
	}

	public static ApiContent GetApiContent(HttpResponseMessage httpResponse)
	{
		return (httpResponse?.Content as ApiContent) ?? throw new Exception(string.Format(_E05B._E000(10320), httpResponse.RequestMessage.RequestUri));
	}

	public static TData ConvertData<TData>(JToken jsonData, JsonSerializer jsonSerializer)
	{
		if (jsonData != null)
		{
			if (jsonData is TData)
			{
				return (TData)(object)((jsonData is TData) ? jsonData : null);
			}
			return jsonData.ToObject<TData>(jsonSerializer);
		}
		return default(TData);
	}
}
