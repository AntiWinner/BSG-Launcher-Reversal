using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Eft.Launcher;

public struct BsgVersion : IEquatable<BsgVersion>, IEquatable<Version>, IComparable<BsgVersion>, IComparable<Version>
{
	[CompilerGenerated]
	private readonly byte _E000;

	[CompilerGenerated]
	private readonly ushort _E001;

	[CompilerGenerated]
	private readonly ushort _E002;

	[CompilerGenerated]
	private readonly ushort _E003;

	[CompilerGenerated]
	private readonly uint _E004;

	public byte Release
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public ushort Major
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public ushort Minor
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public ushort Hotfix
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	public uint Build
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
	}

	public BsgVersion(byte release, ushort major, ushort minor, ushort hotfix, uint build)
	{
		_E000 = release;
		_E001 = major;
		_E002 = minor;
		_E003 = hotfix;
		_E004 = build;
	}

	public static BsgVersion Parse(string input)
	{
		if (input == null)
		{
			throw new ArgumentNullException(_E05B._E000(582));
		}
		string[] array = input.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
		if (array.Length == 4)
		{
			return new BsgVersion(byte.Parse(array[0]), ushort.Parse(array[1]), ushort.Parse(array[2]), 0, uint.Parse(array[3]));
		}
		if (array.Length == 5)
		{
			return new BsgVersion(byte.Parse(array[0]), ushort.Parse(array[1]), ushort.Parse(array[2]), ushort.Parse(array[3]), uint.Parse(array[4]));
		}
		throw new NotSupportedException(string.Format(_E05B._E000(57653), array.Length));
	}

	public static bool TryParse(string input, out BsgVersion result)
	{
		try
		{
			result = Parse(input);
			return true;
		}
		catch
		{
			result = default(BsgVersion);
			return false;
		}
	}

	public bool Equals(BsgVersion other)
	{
		if (Release.Equals(other.Release) && Major.Equals(other.Major) && Minor.Equals(other.Minor))
		{
			return Build.Equals(other.Build);
		}
		return false;
	}

	public bool Equals(Version other)
	{
		if (other != null && other.Major < 255 && Release.Equals((byte)other.Major) && other.Minor < 65535 && Major.Equals((ushort)other.Minor) && other.Build < 65535 && Minor.Equals((ushort)other.Build) && Build < int.MaxValue)
		{
			return Build.Equals((uint)other.Revision);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is BsgVersion other))
		{
			if (obj is Version other2)
			{
				return Equals(other2);
			}
			return false;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return Release.GetHashCode() + Major.GetHashCode() + Minor.GetHashCode() + Build.GetHashCode();
	}

	public int CompareTo(BsgVersion other)
	{
		int num = Release.CompareTo(other.Release);
		if (num == 0)
		{
			num = Major.CompareTo(other.Major);
		}
		if (num == 0)
		{
			num = Minor.CompareTo(other.Minor);
		}
		if (num == 0)
		{
			num = Build.CompareTo(other.Build);
		}
		return num;
	}

	public int CompareTo(Version other)
	{
		int num = ((other.Major > 255) ? (-1) : 0);
		if (num == 0)
		{
			num = Release.CompareTo((byte)other.Major);
		}
		if (num == 0)
		{
			num = ((other.Minor > 65535) ? (-1) : 0);
		}
		if (num == 0)
		{
			num = Major.CompareTo((ushort)other.Minor);
		}
		if (num == 0)
		{
			num = ((other.Build > 65535) ? (-1) : 0);
		}
		if (num == 0)
		{
			num = Minor.CompareTo((ushort)other.Build);
		}
		if (num == 0)
		{
			num = ((Build > int.MaxValue) ? 1 : 0);
		}
		if (num == 0)
		{
			num = ((other.Revision < 0) ? 1 : 0);
		}
		if (num == 0)
		{
			num = Build.CompareTo((uint)other.Revision);
		}
		return num;
	}

	public override string ToString()
	{
		return string.Format(_E05B._E000(57701), Release, Major, Minor, Hotfix, Build);
	}

	[Obsolete]
	public string ToBuildNumberString()
	{
		return string.Format(_E05B._E000(57721), Release, Major, Minor, Build);
	}

	public static bool operator ==(BsgVersion v1, BsgVersion v2)
	{
		return v1.Equals(v2);
	}

	public static bool operator !=(BsgVersion v1, BsgVersion v2)
	{
		return !v1.Equals(v2);
	}

	public static bool operator ==(BsgVersion v1, Version v2)
	{
		return v1.Equals(v2);
	}

	public static bool operator !=(BsgVersion v1, Version v2)
	{
		return !v1.Equals(v2);
	}

	public static bool operator >(BsgVersion v1, BsgVersion v2)
	{
		return v1.CompareTo(v2) > 0;
	}

	public static bool operator <(BsgVersion v1, BsgVersion v2)
	{
		return v1.CompareTo(v2) < 0;
	}

	public static bool operator >(BsgVersion v1, Version v2)
	{
		return v1.CompareTo(v2) > 0;
	}

	public static bool operator <(BsgVersion v1, Version v2)
	{
		return v1.CompareTo(v2) < 0;
	}

	public static explicit operator Version(BsgVersion bsgVersion)
	{
		byte release = bsgVersion.Release;
		ushort major = bsgVersion.Major;
		ushort minor = bsgVersion.Minor;
		if (bsgVersion.Build > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException(_E05B._E000(57673));
		}
		return new Version(release, major, minor, (int)bsgVersion.Build);
	}

	public static explicit operator BsgVersion(Version version)
	{
		if (version.Major <= 255)
		{
			byte release = (byte)version.Major;
			if (version.Minor <= 65535)
			{
				ushort major = (ushort)version.Minor;
				if (version.Build <= 65535)
				{
					ushort minor = (ushort)version.Build;
					if (version.Revision < 0)
					{
						throw new ArgumentOutOfRangeException(_E05B._E000(57440));
					}
					return new BsgVersion(release, major, minor, 0, (uint)version.Revision);
				}
				throw new ArgumentOutOfRangeException(_E05B._E000(57389));
			}
			throw new ArgumentOutOfRangeException(_E05B._E000(57850));
		}
		throw new ArgumentOutOfRangeException(_E05B._E000(57729));
	}
}
