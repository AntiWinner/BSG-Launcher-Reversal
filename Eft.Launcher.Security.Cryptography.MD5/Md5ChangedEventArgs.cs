using System;
using System.Collections.Generic;

namespace Eft.Launcher.Security.Cryptography.MD5;

public class Md5ChangedEventArgs : EventArgs
{
	public readonly byte[] NewData;

	public readonly string FingerPrint;

	public Md5ChangedEventArgs(IList<byte> data, string hashedValue)
	{
		NewData = new byte[data.Count];
		for (int i = 0; i < data.Count; i++)
		{
			NewData[i] = data[i];
		}
		FingerPrint = hashedValue;
	}

	public Md5ChangedEventArgs(string data, string hashedValue)
	{
		NewData = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			NewData[i] = (byte)data[i];
		}
		FingerPrint = hashedValue;
	}
}
