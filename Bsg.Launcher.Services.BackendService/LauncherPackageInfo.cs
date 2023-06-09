using System;
using System.Runtime.CompilerServices;
using Bsg.Network.MultichannelDownloading;

namespace Bsg.Launcher.Services.BackendService;

public class LauncherPackageInfo
{
	[CompilerGenerated]
	private Version _E000;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private ChannelSettings[] _E003;

	public Version Version
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

	public string Hash
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

	public string DownloadUri
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

	public ChannelSettings[] ChannelSettings
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
}
