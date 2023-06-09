using System;
using Eft.Launcher.Network.Http;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Logging;

public static class LoggingExtensions
{
	public static void Exception(this ILogger logger, Exception exc, string message, params object[] args)
	{
		bool flag = exc is UnauthorizedApiNetworkException;
		logger.Log(flag ? LogLevel.Information : LogLevel.Error, flag ? null : exc, message, args);
	}
}
