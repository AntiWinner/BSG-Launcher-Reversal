using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Base.Network.Http;

public class HttpClientBuilderOptions
{
	[CompilerGenerated]
	private Func<HttpClientHandler> _E000 = () => new HttpClientHandler();

	public Func<HttpClientHandler> HttpClientHandlerFactory
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}
}
