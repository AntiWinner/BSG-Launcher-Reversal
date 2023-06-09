using System;
using System.IO;

namespace Bsg.Network.MultichannelDownloading;

public interface IChunk : IDisposable
{
	int Number { get; }

	long Offset { get; }

	int Size { get; }

	int BytesFilled { get; }

	void FillFrom(Stream stream);

	void PourOut(Stream stream);

	void EnsureChecksum();
}
