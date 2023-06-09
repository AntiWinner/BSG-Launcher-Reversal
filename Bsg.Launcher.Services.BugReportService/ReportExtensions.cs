using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bsg.Launcher.Services.BugReportService;

public static class ReportExtensions
{
	public static long CalculateSize(this BugReport bugReport, int gameLogsSizeLimit, JsonSerializerSettings jsonSerializerSettings, ILogger logger = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		IList<AttachmentStream> list = null;
		try
		{
			list = bugReport.GetAllAttachments(gameLogsSizeLimit, jsonSerializerSettings, logger, cancellationToken);
			return list.Sum((AttachmentStream s) => s.Length);
		}
		finally
		{
			if (list != null)
			{
				foreach (AttachmentStream item in list)
				{
					item.Dispose();
				}
			}
		}
	}

	public static IList<AttachmentStream> GetAllAttachments(this BugReport bugReport, int gameLogsSizeLimit, JsonSerializerSettings jsonSerializerSettings, ILogger logger = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (from a in bugReport.GetAttachedFilesStreams(logger, cancellationToken)._E000(bugReport.GetGameLogsStream(gameLogsSizeLimit, jsonSerializerSettings, cancellationToken))._E000(bugReport.GetLauncherLogsStream(cancellationToken))
			where a != null
			select a).ToList();
	}

	public static IEnumerable<AttachmentStream> GetAttachedFilesStreams(this BugReport bugReport, ILogger logger = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		foreach (string attachedFile in bugReport.AttachedFiles)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return new AttachmentStream(File.OpenRead(attachedFile), Path.GetFileName(attachedFile));
		}
	}

	public static AttachmentStream GetGameLogsStream(this BugReport bugReport, int gameLogsSizeLimit, JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (bugReport.GameLogDirectories == null || !bugReport.GameLogDirectories.Any())
		{
			return null;
		}
		return _E000(_E05B._E000(3721), bugReport.CreationTime, bugReport.GameLogDirectories, _E05B._E000(12108), new AttachmentStream[2]
		{
			bugReport.GetSystemInfoStream(jsonSerializerSettings),
			bugReport.GetServersAvailabilityInfoStream(jsonSerializerSettings)
		}, cancellationToken);
	}

	public static AttachmentStream GetLauncherLogsStream(this BugReport bugReport, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (bugReport.LauncherLogFiles == null || !bugReport.LauncherLogFiles.Any())
		{
			return null;
		}
		return _E000(_E05B._E000(3739), bugReport.CreationTime, bugReport.LauncherLogFiles, _E05B._E000(27034), null, cancellationToken);
	}

	public static AttachmentStream GetServersAvailabilityInfoStream(this BugReport bugReport, JsonSerializerSettings jsonSerializerSettings)
	{
		if (bugReport.ServersAvailabilityInfo != null)
		{
			return new AttachmentStream(_E000(bugReport.ServersAvailabilityInfo, jsonSerializerSettings), _E05B._E000(3817));
		}
		return null;
	}

	public static AttachmentStream GetSystemInfoStream(this BugReport bugReport, JsonSerializerSettings jsonSerializerSettings)
	{
		if (bugReport.SystemInfo != null)
		{
			return new AttachmentStream(_E000(bugReport.SystemInfo, jsonSerializerSettings), _E05B._E000(3787));
		}
		return null;
	}

	public static AttachmentStream GetAttachmentsStream(this CrashReport crashReport, JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (crashReport.AttachedDirectories == null || !crashReport.AttachedDirectories.Any())
		{
			return null;
		}
		return _E000(_E05B._E000(3803), crashReport.CreationTime, crashReport.AttachedDirectories, _E05B._E000(27034), new AttachmentStream[1] { crashReport.GetSystemInfoStream(jsonSerializerSettings) }, cancellationToken);
	}

	public static AttachmentStream GetSystemInfoStream(this CrashReport crashReport, JsonSerializerSettings jsonSerializerSettings)
	{
		if (crashReport.SystemInfo != null)
		{
			return new AttachmentStream(_E000(crashReport.SystemInfo, jsonSerializerSettings), _E05B._E000(3787));
		}
		return null;
	}

	private static Stream _E000(object obj, JsonSerializerSettings jsonSerializerSettings)
	{
		return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, jsonSerializerSettings)));
	}

	private static IEnumerable<_E001> _E000<_E001>(this IEnumerable<_E001> enumerable, _E001 item)
	{
		return enumerable.Concat(new _E001[1] { item });
	}

	private static IEnumerable<string> _E000(this IEnumerable<string> paths, int zipLimit, CancellationToken cancellationToken)
	{
		DateTime now = DateTime.Now;
		using MemoryStream baseOutputStream = new MemoryStream();
		using ZipOutputStream zipOutputStream = new ZipOutputStream(baseOutputStream);
		foreach (string path in paths)
		{
			if (Directory.Exists(path))
			{
				foreach (string item in Directory.EnumerateFiles(path, _E05B._E000(27034), SearchOption.AllDirectories))
				{
					cancellationToken.ThrowIfCancellationRequested();
					string entryName = Path.GetFileName(path) + item.Remove(0, path.Length);
					_E000(zipOutputStream, item, entryName, now);
				}
			}
			else
			{
				if (!File.Exists(path))
				{
					continue;
				}
				cancellationToken.ThrowIfCancellationRequested();
				_E000(zipOutputStream, path, Path.GetFileName(path), now);
			}
			zipOutputStream.Flush();
			if (zipOutputStream.Length < zipLimit)
			{
				yield return path;
				continue;
			}
			yield break;
		}
	}

	private static AttachmentStream _E000(string attachmentName, DateTime creationTime, IEnumerable<string> paths, string filter = "*", IEnumerable<AttachmentStream> innerAttachments = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		AttachmentStream attachmentStream = new AttachmentStream(attachmentName);
		if (filter == null)
		{
			filter = _E05B._E000(27034);
		}
		try
		{
			using (ZipOutputStream zipOutputStream = new ZipOutputStream(attachmentStream)
			{
				IsStreamOwner = false
			})
			{
				foreach (string path in paths)
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (Directory.Exists(path))
					{
						foreach (string item in Directory.EnumerateFiles(path, filter, SearchOption.AllDirectories))
						{
							string entryName = Path.GetFileName(path) + item.Remove(0, path.Length);
							_E000(zipOutputStream, item, entryName, creationTime);
						}
					}
					else if (File.Exists(path) && Regex.IsMatch(path, _E05B._E000(11785) + Regex.Escape(filter).Replace(_E05B._E000(11791), _E05B._E000(27969)).Replace(_E05B._E000(11788), _E05B._E000(11793)) + _E05B._E000(11798)))
					{
						_E000(zipOutputStream, path, Path.GetFileName(path), creationTime);
					}
				}
				if (innerAttachments != null)
				{
					foreach (AttachmentStream innerAttachment in innerAttachments)
					{
						cancellationToken.ThrowIfCancellationRequested();
						if (innerAttachment != null)
						{
							_E000(zipOutputStream, innerAttachment, innerAttachment.FileName, creationTime);
						}
					}
				}
				zipOutputStream.Flush();
			}
			attachmentStream.Position = 0L;
			return attachmentStream;
		}
		catch
		{
			attachmentStream.Dispose();
			throw;
		}
	}

	private static void _E000(ZipOutputStream zip, string filePath, string entryName, DateTime creationTime)
	{
		using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using MemoryStream memoryStream = new MemoryStream();
		fileStream.CopyTo(memoryStream);
		memoryStream.Position = 0L;
		_E000(zip, memoryStream, entryName, creationTime);
	}

	private static void _E000(ZipOutputStream zip, Stream content, string entryName, DateTime creationTime)
	{
		ZipEntry entry = new ZipEntry(entryName)
		{
			DateTime = creationTime,
			Size = content.Length
		};
		zip.PutNextEntry(entry);
		content.CopyTo(zip);
		zip.CloseEntry();
	}
}
