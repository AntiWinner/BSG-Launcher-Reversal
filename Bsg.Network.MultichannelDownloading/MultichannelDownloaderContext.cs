using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelDownloaderContext
{
	[CompilerGenerated]
	private readonly int _E000;

	[CompilerGenerated]
	private ILogger _E001;

	public int DownloaderId
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public ILogger ContextLogger
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public MultichannelDownloaderContext(int downloaderId)
	{
		_E000 = downloaderId;
	}

	public override string ToString()
	{
		return string.Format(_E05B._E000(2288), DownloaderId);
	}
}
