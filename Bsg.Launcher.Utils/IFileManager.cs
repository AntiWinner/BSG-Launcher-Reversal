using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Bsg.Launcher.Utils;

public interface IFileManager
{
	event Action<string, HandledEventArgs> OnFileIsUsedByAnotherProcess;

	FileStream CaptureFile(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, FileShare fileShare = FileShare.Read);

	void Delete(string path);

	void Delete(IEnumerable<string> paths);

	void Move(string sourcePath, string destinationPath, bool overwrite);
}
