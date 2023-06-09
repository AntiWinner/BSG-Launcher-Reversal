using System;
using System.Runtime.CompilerServices;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelSettings
{
	[CompilerGenerated]
	private readonly Uri _E000;

	[CompilerGenerated]
	private readonly bool _E001;

	public Uri Endpoint
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public bool IsSpare
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public ChannelSettings(Uri endpoint, bool isSpare)
	{
		_E000 = endpoint;
		_E001 = isSpare;
	}
}
