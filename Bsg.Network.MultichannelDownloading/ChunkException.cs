using System;

namespace Bsg.Network.MultichannelDownloading;

public class ChunkException : MultichannelDownloadingException
{
	public ChunkException(string message)
		: base(message)
	{
	}

	public ChunkException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
