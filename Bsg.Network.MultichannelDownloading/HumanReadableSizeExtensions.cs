namespace Bsg.Network.MultichannelDownloading;

public static class HumanReadableSizeExtensions
{
	public static string ToHumanReadableSize(this int size)
	{
		return ((double)size).ToHumanReadableSize();
	}

	public static string ToHumanReadableSize(this long size)
	{
		return ((double)size).ToHumanReadableSize();
	}

	public static string ToHumanReadableSize(this double size)
	{
		string[] array = new string[5]
		{
			_E05B._E000(2235),
			_E05B._E000(2233),
			_E05B._E000(2238),
			_E05B._E000(2179),
			_E05B._E000(2176)
		};
		int num = 0;
		while (size >= 1024.0 && num < array.Length - 1)
		{
			num++;
			size /= 1024.0;
		}
		return string.Format(string.Format(_E05B._E000(2181), size, array[num]));
	}
}
