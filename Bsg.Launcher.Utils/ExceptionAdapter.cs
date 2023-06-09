using System;
using Bsg.Launcher.Services.AuthService;
using Eft.Launcher;
using Eft.Launcher.Network.Http;
using Newtonsoft.Json;

namespace Bsg.Launcher.Utils;

public class ExceptionAdapter
{
	private class _E000
	{
		public Exception Exception { get; set; }
	}

	private readonly JsonSerializerSettings m__E000;

	public ExceptionAdapter(JsonSerializerSettings jsonSerializerSettings)
	{
		this.m__E000 = jsonSerializerSettings;
	}

	public string SerializeForUi(Exception exc)
	{
		return JsonConvert.SerializeObject(new _E000
		{
			Exception = AdaptForUi(exc)
		}, this.m__E000);
	}

	private Exception AdaptForUi(Exception exc)
	{
		if (exc.HResult == -2147024784)
		{
			string text = (exc.Data?["driveLetter"] as string) ?? "Unknown";
			return new BsgException(BsgExceptionCode.NotEnoughSpaceOnDisk, text);
		}
		if (exc is AuthServiceException && exc.InnerException is ApiNetworkException result)
		{
			return result;
		}
		return exc;
	}
}
