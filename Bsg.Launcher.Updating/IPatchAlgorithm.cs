using System;
using System.IO;

namespace Bsg.Launcher.Updating;

public interface IPatchAlgorithm
{
	byte Id { get; }

	string Name { get; }

	void ApplyPatch(Stream originalFileStream, Stream patchStream, Stream resultFileStream, Action<long, long> onProgress = null);

	bool IsApplicableFor(long oldFileSize, long newFileSize);

	void CreatePatch(Stream oldFileStream, Stream newFileStream, Stream patchOutputStream);
}
