using System;

namespace Eft.Launcher.Services.SiteCommunicationService;

public class SiteException : Exception
{
	public int Code { get; }

	public string[] Args { get; }

	public SiteException(int code, string[] args, Exception innerException = null)
		: base("", innerException)
	{
		Code = code;
		Args = args;
	}
}
