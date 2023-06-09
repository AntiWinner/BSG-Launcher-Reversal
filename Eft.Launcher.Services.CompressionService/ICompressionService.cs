using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.CompressionService;

public interface ICompressionService
{
	Task ExtractZipToDir(string zipFile, string dir, CancellationToken cancellationToken, Action<long, long> onProgress = null);

	Task CreateZipFromDir(string dir, string zipFile, CancellationToken cancellationToken, Action<long, long> onProgress = null);

	Stream CreateZip(IDictionary<string, string> files);

	Task ReadFileFromZip(string zipFilePath, string fileToReadRelativePath, Stream outputStream);
}
