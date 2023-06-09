using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.InformationCollectionService;

public class SystemInfo
{
	[CompilerGenerated]
	private readonly int _E000 = 2;

	[CompilerGenerated]
	private string _E001;

	[CompilerGenerated]
	private string _E002;

	[CompilerGenerated]
	private SmSystemFirmwareInfo _E003;

	[CompilerGenerated]
	private SmBiosInfo _E004;

	[CompilerGenerated]
	private SmSystemInfo _E005;

	[CompilerGenerated]
	private SmBaseboardInfo _E006;

	[CompilerGenerated]
	private SmProcessorInfo _E007;

	[CompilerGenerated]
	private BaseboardInfo _E008;

	[CompilerGenerated]
	private BiosInfo _E009;

	[CompilerGenerated]
	private IEnumerable<CpuInfo> _E00A;

	[CompilerGenerated]
	private IEnumerable<GpuInfo> _E00B;

	[CompilerGenerated]
	private IEnumerable<NicInfo> _E00C;

	[CompilerGenerated]
	private OsInfo _E00D;

	[CompilerGenerated]
	private RamInfo _E00E;

	[CompilerGenerated]
	private IEnumerable<StorageInfo> _E00F;

	public int Version
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public string MachineName
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

	public string Checksum
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public SmSystemFirmwareInfo SmSystemFirmware
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

	public SmBiosInfo SmBios
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

	public SmSystemInfo SmSystem
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

	public SmBaseboardInfo SmBaseboard
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

	public SmProcessorInfo SmProcessor
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		set
		{
			_E007 = value;
		}
	}

	public BaseboardInfo Baseboard
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
		[CompilerGenerated]
		set
		{
			_E008 = value;
		}
	}

	public BiosInfo Bios
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		set
		{
			_E009 = value;
		}
	}

	public IEnumerable<CpuInfo> Cpu
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		set
		{
			_E00A = value;
		}
	}

	public IEnumerable<GpuInfo> Gpu
	{
		[CompilerGenerated]
		get
		{
			return _E00B;
		}
		[CompilerGenerated]
		set
		{
			_E00B = value;
		}
	}

	public IEnumerable<NicInfo> Nic
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		set
		{
			_E00C = value;
		}
	}

	public OsInfo Os
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		set
		{
			_E00D = value;
		}
	}

	public RamInfo Ram
	{
		[CompilerGenerated]
		get
		{
			return _E00E;
		}
		[CompilerGenerated]
		set
		{
			_E00E = value;
		}
	}

	public IEnumerable<StorageInfo> Storage
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		set
		{
			_E00F = value;
		}
	}

	public void UpdateChecksum()
	{
		long num = DateTime.UtcNow.ToUnixTimeStamp() / 1000000;
		JObject jObject = JObject.FromObject(this);
		jObject[_E05B._E000(61159)] = null;
		Checksum = num + jObject.ToString(Formatting.None).GetMd5();
	}
}
