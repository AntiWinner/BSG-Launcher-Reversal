using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Eft.Launcher.Security.Cryptography.MD5;

public class MD5
{
	public delegate void ValueChanging(object sender, Md5ChangingEventArgs changing);

	public delegate void ValueChanged(object sender, Md5ChangedEventArgs changed);

	protected static readonly uint[] T = new uint[64]
	{
		3614090360u, 3905402710u, 606105819u, 3250441966u, 4118548399u, 1200080426u, 2821735955u, 4249261313u, 1770035416u, 2336552879u,
		4294925233u, 2304563134u, 1804603682u, 4254626195u, 2792965006u, 1236535329u, 4129170786u, 3225465664u, 643717713u, 3921069994u,
		3593408605u, 38016083u, 3634488961u, 3889429448u, 568446438u, 3275163606u, 4107603335u, 1163531501u, 2850285829u, 4243563512u,
		1735328473u, 2368359562u, 4294588738u, 2272392833u, 1839030562u, 4259657740u, 2763975236u, 1272893353u, 4139469664u, 3200236656u,
		681279174u, 3936430074u, 3572445317u, 76029189u, 3654602809u, 3873151461u, 530742520u, 3299628645u, 4096336452u, 1126891415u,
		2878612391u, 4237533241u, 1700485571u, 2399980690u, 4293915773u, 2240044497u, 1873313359u, 4264355552u, 2734768916u, 1309151649u,
		4149444226u, 3174756917u, 718787259u, 3951481745u
	};

	protected uint[] X = new uint[16];

	internal _E04E _E000;

	protected byte[] ByteInput;

	[CompilerGenerated]
	private ValueChanging _E001;

	[CompilerGenerated]
	private ValueChanged _E002;

	public string Value
	{
		get
		{
			char[] array = new char[ByteInput.Length];
			for (int i = 0; i < ByteInput.Length; i++)
			{
				array[i] = (char)ByteInput[i];
			}
			return new string(array);
		}
		set
		{
			_E001?.Invoke(this, new Md5ChangingEventArgs(value));
			ByteInput = new byte[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				ByteInput[i] = (byte)value[i];
			}
			this._E000 = _E000();
			_E002?.Invoke(this, new Md5ChangedEventArgs(value, this._E000.ToString()));
		}
	}

	public byte[] ValueAsByte
	{
		get
		{
			byte[] array = new byte[ByteInput.Length];
			for (int i = 0; i < ByteInput.Length; i++)
			{
				array[i] = ByteInput[i];
			}
			return array;
		}
		set
		{
			_E001?.Invoke(this, new Md5ChangingEventArgs(value));
			ByteInput = new byte[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				ByteInput[i] = value[i];
			}
			this._E000 = _E000();
			_E002?.Invoke(this, new Md5ChangedEventArgs(value, this._E000.ToString()));
		}
	}

	public string Hash => this._E000.ToString();

	public byte[] HashAsByteArray => Encoding.UTF8.GetBytes(this._E000.ToString());

	public event ValueChanging OnValueChanging
	{
		[CompilerGenerated]
		add
		{
			ValueChanging valueChanging = _E001;
			ValueChanging valueChanging2;
			do
			{
				valueChanging2 = valueChanging;
				ValueChanging value2 = (ValueChanging)Delegate.Combine(valueChanging2, value);
				valueChanging = Interlocked.CompareExchange(ref _E001, value2, valueChanging2);
			}
			while ((object)valueChanging != valueChanging2);
		}
		[CompilerGenerated]
		remove
		{
			ValueChanging valueChanging = _E001;
			ValueChanging valueChanging2;
			do
			{
				valueChanging2 = valueChanging;
				ValueChanging value2 = (ValueChanging)Delegate.Remove(valueChanging2, value);
				valueChanging = Interlocked.CompareExchange(ref _E001, value2, valueChanging2);
			}
			while ((object)valueChanging != valueChanging2);
		}
	}

	public event ValueChanged OnValueChanged
	{
		[CompilerGenerated]
		add
		{
			ValueChanged valueChanged = _E002;
			ValueChanged valueChanged2;
			do
			{
				valueChanged2 = valueChanged;
				ValueChanged value2 = (ValueChanged)Delegate.Combine(valueChanged2, value);
				valueChanged = Interlocked.CompareExchange(ref _E002, value2, valueChanged2);
			}
			while ((object)valueChanged != valueChanged2);
		}
		[CompilerGenerated]
		remove
		{
			ValueChanged valueChanged = _E002;
			ValueChanged valueChanged2;
			do
			{
				valueChanged2 = valueChanged;
				ValueChanged value2 = (ValueChanged)Delegate.Remove(valueChanged2, value);
				valueChanged = Interlocked.CompareExchange(ref _E002, value2, valueChanged2);
			}
			while ((object)valueChanged != valueChanged2);
		}
	}

	public void SetValueFromStream(Stream stream)
	{
		byte[] array2 = (ValueAsByte = _E000(stream));
	}

	public MD5()
	{
		Value = string.Empty;
	}

	private static byte[] _E000(Stream stream)
	{
		return _E000(new BinaryReader(stream));
	}

	private static byte[] _E000(BinaryReader reader)
	{
		using MemoryStream memoryStream = new MemoryStream();
		byte[] array = new byte[4096];
		int count;
		while ((count = reader.Read(array, 0, array.Length)) != 0)
		{
			memoryStream.Write(array, 0, count);
		}
		return memoryStream.ToArray();
	}

	internal _E04E _E000()
	{
		_E04E obj = new _E04E();
		byte[] array = CreatePaddedBuffer();
		uint num = (uint)(array.Length * 8) / 32u;
		for (uint num2 = 0u; num2 < num / 16u; num2++)
		{
			CopyBlock(array, num2);
			PerformTransformation(ref obj._E000, ref obj._E001, ref obj._E002, ref obj._E003);
		}
		return obj;
	}

	protected void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
	{
		a = b + _E04F._E000(a + ((b & c) | (~b & d)) + X[k] + T[i - 1], s);
	}

	protected void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
	{
		a = b + _E04F._E000(a + ((b & d) | (c & ~d)) + X[k] + T[i - 1], s);
	}

	protected void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
	{
		a = b + _E04F._E000(a + (b ^ c ^ d) + X[k] + T[i - 1], s);
	}

	protected void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
	{
		a = b + _E04F._E000(a + (c ^ (b | ~d)) + X[k] + T[i - 1], s);
	}

	protected void PerformTransformation(ref uint a, ref uint b, ref uint c, ref uint d)
	{
		uint num = a;
		uint num2 = b;
		uint num3 = c;
		uint num4 = d;
		TransF(ref a, b, c, d, 0u, 7, 1u);
		TransF(ref d, a, b, c, 1u, 12, 2u);
		TransF(ref c, d, a, b, 2u, 17, 3u);
		TransF(ref b, c, d, a, 3u, 22, 4u);
		TransF(ref a, b, c, d, 4u, 7, 5u);
		TransF(ref d, a, b, c, 5u, 12, 6u);
		TransF(ref c, d, a, b, 6u, 17, 7u);
		TransF(ref b, c, d, a, 7u, 22, 8u);
		TransF(ref a, b, c, d, 8u, 7, 9u);
		TransF(ref d, a, b, c, 9u, 12, 10u);
		TransF(ref c, d, a, b, 10u, 17, 11u);
		TransF(ref b, c, d, a, 11u, 22, 12u);
		TransF(ref a, b, c, d, 12u, 7, 13u);
		TransF(ref d, a, b, c, 13u, 12, 14u);
		TransF(ref c, d, a, b, 14u, 17, 15u);
		TransF(ref b, c, d, a, 15u, 22, 16u);
		TransG(ref a, b, c, d, 1u, 5, 17u);
		TransG(ref d, a, b, c, 6u, 9, 18u);
		TransG(ref c, d, a, b, 11u, 14, 19u);
		TransG(ref b, c, d, a, 0u, 20, 20u);
		TransG(ref a, b, c, d, 5u, 5, 21u);
		TransG(ref d, a, b, c, 10u, 9, 22u);
		TransG(ref c, d, a, b, 15u, 14, 23u);
		TransG(ref b, c, d, a, 4u, 20, 24u);
		TransG(ref a, b, c, d, 9u, 5, 25u);
		TransG(ref d, a, b, c, 14u, 9, 26u);
		TransG(ref c, d, a, b, 3u, 14, 27u);
		TransG(ref b, c, d, a, 8u, 20, 28u);
		TransG(ref a, b, c, d, 13u, 5, 29u);
		TransG(ref d, a, b, c, 2u, 9, 30u);
		TransG(ref c, d, a, b, 7u, 14, 31u);
		TransG(ref b, c, d, a, 12u, 20, 32u);
		TransH(ref a, b, c, d, 5u, 4, 33u);
		TransH(ref d, a, b, c, 8u, 11, 34u);
		TransH(ref c, d, a, b, 11u, 16, 35u);
		TransH(ref b, c, d, a, 14u, 23, 36u);
		TransH(ref a, b, c, d, 1u, 4, 37u);
		TransH(ref d, a, b, c, 4u, 11, 38u);
		TransH(ref c, d, a, b, 7u, 16, 39u);
		TransH(ref b, c, d, a, 10u, 23, 40u);
		TransH(ref a, b, c, d, 13u, 4, 41u);
		TransH(ref d, a, b, c, 0u, 11, 42u);
		TransH(ref c, d, a, b, 3u, 16, 43u);
		TransH(ref b, c, d, a, 6u, 23, 44u);
		TransH(ref a, b, c, d, 9u, 4, 45u);
		TransH(ref d, a, b, c, 12u, 11, 46u);
		TransH(ref c, d, a, b, 15u, 16, 47u);
		TransH(ref b, c, d, a, 2u, 23, 48u);
		TransI(ref a, b, c, d, 0u, 6, 49u);
		TransI(ref d, a, b, c, 7u, 10, 50u);
		TransI(ref c, d, a, b, 14u, 15, 51u);
		TransI(ref b, c, d, a, 5u, 21, 52u);
		TransI(ref a, b, c, d, 12u, 6, 53u);
		TransI(ref d, a, b, c, 3u, 10, 54u);
		TransI(ref c, d, a, b, 10u, 15, 55u);
		TransI(ref b, c, d, a, 1u, 21, 56u);
		TransI(ref a, b, c, d, 8u, 6, 57u);
		TransI(ref d, a, b, c, 15u, 10, 58u);
		TransI(ref c, d, a, b, 6u, 15, 59u);
		TransI(ref b, c, d, a, 13u, 21, 60u);
		TransI(ref a, b, c, d, 4u, 6, 61u);
		TransI(ref d, a, b, c, 11u, 10, 62u);
		TransI(ref c, d, a, b, 2u, 15, 63u);
		TransI(ref b, c, d, a, 9u, 21, 64u);
		a += num;
		b += num2;
		c += num3;
		d += num4;
	}

	protected byte[] CreatePaddedBuffer()
	{
		uint num = (uint)((448 - ByteInput.Length * 8 % 512 + 512) % 512);
		if (num == 0)
		{
			num = 512u;
		}
		uint num2 = (uint)(ByteInput.Length + num / 8u + 8);
		ulong num3 = (ulong)ByteInput.Length * 8uL;
		byte[] array = new byte[num2];
		for (int i = 0; i < ByteInput.Length; i++)
		{
			array[i] = ByteInput[i];
		}
		array[ByteInput.Length] |= 128;
		for (int num4 = 8; num4 > 0; num4--)
		{
			array[num2 - num4] = (byte)((num3 >> (8 - num4) * 8) & 0xFF);
		}
		return array;
	}

	protected void CopyBlock(byte[] msg, uint block)
	{
		block <<= 6;
		for (uint num = 0u; num < 61; num += 4)
		{
			X[num >> 2] = (uint)((msg[block + (num + 3)] << 24) | (msg[block + (num + 2)] << 16) | (msg[block + (num + 1)] << 8) | msg[block + num]);
		}
	}
}
