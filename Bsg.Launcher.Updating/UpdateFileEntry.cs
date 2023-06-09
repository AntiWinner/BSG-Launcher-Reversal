using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Bsg.Launcher.Updating;

public class UpdateFileEntry
{
	[CompilerGenerated]
	private string _E000;

	[CompilerGenerated]
	private UpdateFileEntryState _E001;

	[CompilerGenerated]
	private byte _E002;

	[CompilerGenerated]
	private long _E003 = 10L;

	[CompilerGenerated]
	private long _E004;

	[CompilerGenerated]
	private long _E005;

	public string Path
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

	public UpdateFileEntryState State
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

	public byte PatchAlgorithmId
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

	public long ApplyTime
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	[JsonIgnore]
	public long Size
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		set
		{
			_E004 = value;
		}
	}

	[JsonIgnore]
	public long CompressedSize
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		set
		{
			_E005 = value;
		}
	}
}
