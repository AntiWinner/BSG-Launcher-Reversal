namespace Bsg.Network.MultichannelDownloading;

public class ChunkChecksumException : ChunkException
{
	public ChunkChecksumException(string message)
		: base(message)
	{
	}

	public static ChunkChecksumException TheChecksumDoesNotMatch()
	{
		return new ChunkChecksumException("The checksum does not match");
	}
}
