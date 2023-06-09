using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Eft.Launcher;

namespace Bsg.Launcher.Updating;

public class UpdateMetadata
{
	[CompilerGenerated]
	private byte _E000;

	[CompilerGenerated]
	private BsgVersion _E001;

	[CompilerGenerated]
	private BsgVersion _E002;

	public List<UpdateFileEntry> Files = new List<UpdateFileEntry>();

	public byte MetadataVersion
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

	public BsgVersion FromVersion
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

	public BsgVersion ToVersion
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
