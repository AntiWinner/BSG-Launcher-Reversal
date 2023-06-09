using System.Runtime.CompilerServices;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelFileMetadata
{
	[CompilerGenerated]
	private long _E000;

	[CompilerGenerated]
	private int _E001;

	[CompilerGenerated]
	private int[] _E002;

	public long FleSize
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

	public int ChunkSize
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

	public int[] Checksums
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}
}
