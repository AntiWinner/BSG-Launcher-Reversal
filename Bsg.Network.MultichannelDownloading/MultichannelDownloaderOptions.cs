using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bsg.Network.MultichannelDownloading;

public class MultichannelDownloaderOptions
{
	[CompilerGenerated]
	private sealed class _E000<_E001, _E006>
	{
		public PropertyInfo _E000;

		internal bool _E000(PropertyInfo x)
		{
			return x.Name == this._E000.Name;
		}

		internal bool _E001(PropertyInfo x)
		{
			return x.Name == this._E000.Name;
		}
	}

	[CompilerGenerated]
	private int m__E000 = 524288;

	[CompilerGenerated]
	private int _E001 = 10;

	[CompilerGenerated]
	private int _E002;

	[CompilerGenerated]
	private int _E003;

	[CompilerGenerated]
	private int _E004 = 10;

	[CompilerGenerated]
	private int _E005 = 5242880;

	[CompilerGenerated]
	private int _E006 = 500;

	public int SpareNodeActivationThreshold
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public int SpareNodeThresholdTimeoutSec
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

	public int ConnectionBreakingThreshold
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

	public int ConnectionBreakingThresholdTimeoutSec
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

	public int SimultaneouslyUsedChannelsLimit
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

	public int ChunkSize
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

	public int ProgressIntervalMs
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		set
		{
			_E006 = value;
		}
	}

	public void Update(MultichannelDownloaderOptions multiChannelDownloadSettings)
	{
		_E000(multiChannelDownloadSettings, this);
	}

	private static void _E000<_E001, _E006>(_E001 source, _E006 dest)
	{
		List<PropertyInfo> list = (from x in typeof(_E001).GetProperties()
			where x.CanRead
			select x).ToList();
		List<PropertyInfo> source2 = (from x in typeof(_E006).GetProperties()
			where x.CanWrite
			select x).ToList();
		foreach (PropertyInfo current in list)
		{
			if (source2.Any((PropertyInfo x) => x.Name == current.Name))
			{
				PropertyInfo propertyInfo = source2.First((PropertyInfo x) => x.Name == current.Name);
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(dest, current.GetValue(source, null), null);
				}
			}
		}
	}
}
