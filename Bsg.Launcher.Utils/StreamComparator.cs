using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bsg.Launcher.Utils;

public class StreamComparator
{
	private const int m__E000 = 10485760;

	private readonly byte[] _E001 = new byte[10485760];

	private readonly byte[] _E002 = new byte[10485760];

	[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcmp")]
	private static extern int _E000(byte[] b1, byte[] b2, long count);

	public bool Equals(Stream aStream, Stream bStream)
	{
		if (aStream == null)
		{
			throw new ArgumentNullException(_E05B._E000(58018));
		}
		if (bStream == null)
		{
			throw new ArgumentNullException(_E05B._E000(58026));
		}
		if (aStream == bStream)
		{
			return true;
		}
		if (aStream.Length != bStream.Length)
		{
			return false;
		}
		int num;
		do
		{
			num = aStream.Read(_E001, 0, _E001.Length);
			if (bStream.Read(_E002, 0, _E001.Length) != num)
			{
				throw new Exception(_E05B._E000(58034));
			}
			if (_E000(_E001, _E002, num) != 0)
			{
				return false;
			}
		}
		while (num != 0);
		return true;
	}
}
