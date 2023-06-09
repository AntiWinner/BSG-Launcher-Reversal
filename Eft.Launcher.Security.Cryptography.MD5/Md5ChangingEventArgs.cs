using System;
using System.Collections.Generic;

namespace Eft.Launcher.Security.Cryptography.MD5;

public class Md5ChangingEventArgs : EventArgs
{
	public readonly byte[] NewData;

	public Md5ChangingEventArgs(IList<byte> data)
	{
		NewData = new byte[data.Count];
		for (int i = 0; i < data.Count; i++)
		{
			NewData[i] = data[i];
		}
	}

	public Md5ChangingEventArgs(string data)
	{
		NewData = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			NewData[i] = (byte)data[i];
		}
	}
}
