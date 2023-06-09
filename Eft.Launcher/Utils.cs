using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using Bsg.Launcher.Utils;
using Eft.Launcher.Core;
using Eft.Launcher.Security.Cryptography.MD5;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher;

public class Utils
{
	public struct DirCalcInfo
	{
		[CompilerGenerated]
		private int _E000;

		[CompilerGenerated]
		private long _E001;

		public int FilesCount
		{
			[CompilerGenerated]
			get
			{
				return _E000;
			}
			[CompilerGenerated]
			internal set
			{
				_E000 = value;
			}
		}

		public long Size
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			internal set
			{
				_E001 = value;
			}
		}
	}

	private readonly Regex _E000 = new Regex(_E05B._E000(61258));

	private readonly ILogger _E001;

	private readonly IFileManager _E002;

	public Utils(ILogger<Utils> logger, IFileManager fileManager)
	{
		_E001 = logger;
		_E002 = fileManager;
	}

	public void ThrowIfNotEnoughSpace(string file, long size)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(file));
		using FileStream fileStream = new FileStream(file + _E05B._E000(61351), FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
		try
		{
			fileStream.SetLength(size);
		}
		catch
		{
			string pathRoot = Path.GetPathRoot(file);
			new IOException(_E05B._E000(61352) + pathRoot, -2147024784).Data?.Add(_E05B._E000(61329), pathRoot);
		}
	}

	public IReadOnlyCollection<BsgVersion> ExtractVersionsFromString(string str)
	{
		if (!string.IsNullOrWhiteSpace(str))
		{
			MatchCollection matchCollection = _E000.Matches(str);
			if (matchCollection.Count > 0)
			{
				List<BsgVersion> list = new List<BsgVersion>(matchCollection.Count);
				{
					foreach (Match item in matchCollection)
					{
						list.Add(BsgVersion.Parse(item.Value));
					}
					return list;
				}
			}
		}
		return new List<BsgVersion>();
	}

	[Obsolete("Please use IConsistencyControlService.CheckHash()")]
	public bool CheckHashForFile(string file, string hash, CancellationToken cancellationToken, Action<IProgressReport> onProgress = null)
	{
		return GetHashForFile(file, cancellationToken, onProgress).ToHex().ToLowerInvariant() == hash.ToLowerInvariant();
	}

	[Obsolete("Please use IConsistencyControlService.GetHash()")]
	public byte[] GetHashForFile(string file, CancellationToken cancellationToken, Action<IProgressReport> onProgress = null)
	{
		DateTime dateTime = DateTime.MinValue;
		long num = 0L;
		byte[] array = new byte[4096];
		byte[] array2 = null;
		try
		{
			using FileStream fileStream = _E002.CaptureFile(file);
			using System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
			int num2;
			while ((num2 = fileStream.Read(array, 0, array.Length)) > 0)
			{
				mD.TransformBlock(array, 0, num2, array, 0);
				num += num2;
				cancellationToken.ThrowIfCancellationRequested();
				if (onProgress != null)
				{
					DateTime now = DateTime.Now;
					if ((now - dateTime).TotalMilliseconds > 300.0)
					{
						dateTime = now;
						onProgress(new ProgressReport(num, fileStream.Length));
					}
				}
			}
			mD.TransformFinalBlock(array, 0, 0);
			array2 = mD.Hash;
			if (onProgress != null)
			{
				onProgress(new ProgressReport(fileStream.Length, fileStream.Length));
				return array2;
			}
			return array2;
		}
		catch (TargetInvocationException)
		{
			return new Eft.Launcher.Security.Cryptography.MD5.MD5
			{
				ValueAsByte = File.ReadAllBytes(file)
			}.HashAsByteArray;
		}
	}

	public void CopyDir(string sourceDir, string targetDir, Action<long, long> onProgress = null)
	{
		List<string> list = Directory.EnumerateFiles(sourceDir, _E05B._E000(27034), SearchOption.AllDirectories).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			string path = text.Substring(sourceDir.Length).TrimStart('\\', '/', ' ');
			string text2 = Path.Combine(targetDir, path);
			string directoryName = Path.GetDirectoryName(text2);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.Copy(text, text2, overwrite: true);
			onProgress?.Invoke(i, list.Count);
		}
	}

	public DirCalcInfo CalculateDirectory(string dir)
	{
		DirCalcInfo result = default(DirCalcInfo);
		foreach (FileInfo item in new DirectoryInfo(dir).EnumerateFiles(_E05B._E000(27034), SearchOption.AllDirectories))
		{
			result.Size += item.Length;
			result.FilesCount++;
		}
		return result;
	}

	public BsgVersion GetGameVersion(string gameExeFile)
	{
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(gameExeFile);
		BsgVersion bsgVersion;
		try
		{
			string[] array = versionInfo.ProductVersion.Split('-');
			if (array.Length == 2)
			{
				bsgVersion = BsgVersion.Parse(array[0]);
			}
			else
			{
				if (array.Length <= 2)
				{
					throw new Exception(_E05B._E000(61341));
				}
				bsgVersion = BsgVersion.Parse(array[0] + _E05B._E000(27969) + array[1]);
			}
		}
		catch (Exception exception)
		{
			_E001.LogWarning(exception, _E05B._E000(61382));
			bsgVersion = new BsgVersion((byte)versionInfo.FileMajorPart, (ushort)versionInfo.FileMinorPart, (ushort)versionInfo.FileBuildPart, 0, (uint)versionInfo.FilePrivatePart);
		}
		if (bsgVersion == default(BsgVersion))
		{
			throw new Exception(_E05B._E000(60973) + gameExeFile + _E05B._E000(27975));
		}
		return bsgVersion;
	}
}
