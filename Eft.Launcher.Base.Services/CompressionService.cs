using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Services.CompressionService;
using Ionic.Zip;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Base.Services;

public class CompressionService : ICompressionService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public CompressionService _E000;

		public string _E001;

		public string _E002;

		public Action<long, long> _E003;

		public CancellationToken _E004;

		internal void _E000()
		{
			this._E000.m__E001.LogDebug(_E05B._E000(20135), _E001, _E002);
			if (!ZipFile.IsZipFile(_E001))
			{
				throw new CompressionServiceException(BsgExceptionCode.FileIsNotAZipArchive, _E001);
			}
			using (ZipFile zipFile = ZipFile.Read(_E001))
			{
				_E001 CS_0024_003C_003E8__locals0 = new _E001
				{
					_E005 = this
				};
				zipFile.BufferSize = 1048576;
				zipFile.CodecBufferSize = 1048576;
				long size = zipFile.Entries.Sum((ZipEntry entry) => entry.UncompressedSize);
				this._E000.m__E002.ThrowIfNotEnoughSpace(Path.Combine(_E002, _E05B._E000(20126)), size);
				CS_0024_003C_003E8__locals0._E004 = zipFile.Entries.Sum((ZipEntry e) => e.UncompressedSize);
				CS_0024_003C_003E8__locals0._E003 = 0L;
				CS_0024_003C_003E8__locals0._E000 = new Dictionary<ZipEntry, long>();
				CS_0024_003C_003E8__locals0._E001 = DateTime.Now;
				CS_0024_003C_003E8__locals0._E002 = TimeSpan.FromMilliseconds(200.0);
				if (_E003 != null)
				{
					_E003(CS_0024_003C_003E8__locals0._E003, CS_0024_003C_003E8__locals0._E004);
					zipFile.ExtractProgress += delegate(object sender, ExtractProgressEventArgs e)
					{
						lock (CS_0024_003C_003E8__locals0._E000)
						{
							DateTime now = DateTime.Now;
							bool flag = now - CS_0024_003C_003E8__locals0._E001 > CS_0024_003C_003E8__locals0._E002;
							if (flag)
							{
								CS_0024_003C_003E8__locals0._E001 = now;
							}
							switch (e.EventType)
							{
							case ZipProgressEventType.Extracting_BeforeExtractEntry:
								CS_0024_003C_003E8__locals0._E000.Add(e.CurrentEntry, 0L);
								break;
							case ZipProgressEventType.Extracting_AfterExtractEntry:
								CS_0024_003C_003E8__locals0._E000.Remove(e.CurrentEntry);
								Interlocked.Add(ref CS_0024_003C_003E8__locals0._E003, e.CurrentEntry.UncompressedSize);
								break;
							case ZipProgressEventType.Extracting_EntryBytesWritten:
								if (flag && CS_0024_003C_003E8__locals0._E000.ContainsKey(e.CurrentEntry))
								{
									CS_0024_003C_003E8__locals0._E000[e.CurrentEntry] = e.BytesTransferred;
								}
								break;
							}
							if (flag)
							{
								CS_0024_003C_003E8__locals0._E005._E003(CS_0024_003C_003E8__locals0._E003 + CS_0024_003C_003E8__locals0._E000.Sum((KeyValuePair<ZipEntry, long> kvp) => kvp.Value), CS_0024_003C_003E8__locals0._E004);
							}
						}
					};
				}
				try
				{
					foreach (ZipEntry item in zipFile)
					{
						_E004.ThrowIfCancellationRequested();
						item.Extract(_E002, ExtractExistingFileAction.OverwriteSilently);
					}
				}
				catch (Exception ex) when (ex.HResult == -2147024784)
				{
					ex.AddDiskLetter(_E002);
					throw ex;
				}
				_E003?.Invoke(CS_0024_003C_003E8__locals0._E004, CS_0024_003C_003E8__locals0._E004);
			}
			this._E000.m__E001.LogDebug(_E05B._E000(20196), _E001, _E002);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Dictionary<ZipEntry, long> _E000;

		public DateTime _E001;

		public TimeSpan _E002;

		public long _E003;

		public long _E004;

		public _E000 _E005;

		internal void _E000(object sender, ExtractProgressEventArgs e)
		{
			lock (this._E000)
			{
				DateTime now = DateTime.Now;
				bool flag = now - _E001 > _E002;
				if (flag)
				{
					_E001 = now;
				}
				switch (e.EventType)
				{
				case ZipProgressEventType.Extracting_BeforeExtractEntry:
					this._E000.Add(e.CurrentEntry, 0L);
					break;
				case ZipProgressEventType.Extracting_AfterExtractEntry:
					this._E000.Remove(e.CurrentEntry);
					Interlocked.Add(ref _E003, e.CurrentEntry.UncompressedSize);
					break;
				case ZipProgressEventType.Extracting_EntryBytesWritten:
					if (flag && this._E000.ContainsKey(e.CurrentEntry))
					{
						this._E000[e.CurrentEntry] = e.BytesTransferred;
					}
					break;
				}
				if (flag)
				{
					_E005._E003(_E003 + this._E000.Sum((KeyValuePair<ZipEntry, long> kvp) => kvp.Value), _E004);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public CompressionService _E000;

		public string _E001;

		public string _E002;

		public Action<long, long> _E003;

		public EventHandler<SaveProgressEventArgs> _E004;

		internal void _E000()
		{
			this._E000.m__E001.LogDebug(_E05B._E000(19756), _E001, _E002);
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.BufferSize = 1048576;
				zipFile.CodecBufferSize = 1048576;
				zipFile.UseZip64WhenSaving = Zip64Option.Always;
				zipFile.SaveProgress += delegate(object s, SaveProgressEventArgs e)
				{
					_E003?.Invoke(e.BytesTransferred, e.TotalBytesToTransfer);
				};
				zipFile.AddDirectory(_E002);
				zipFile.Save(_E001);
			}
			this._E000.m__E001.LogDebug(_E05B._E000(19823), _E001, (float)new FileInfo(_E001).Length / 1024f / 1024f);
		}

		internal void _E000(object s, SaveProgressEventArgs e)
		{
			_E003?.Invoke(e.BytesTransferred, e.TotalBytesToTransfer);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public CompressionService _E000;

		public string _E001;

		public string _E002;

		public Stream _E003;

		public Func<ZipEntry, bool> _E004;

		internal void _E000()
		{
			this._E000.m__E001.LogDebug(_E05B._E000(19873), _E001, _E002);
			using (ZipFile zipFile = new ZipFile(_E002))
			{
				zipFile.EntriesSorted.First((ZipEntry e) => e.FileName == _E001).Extract(_E003);
			}
			this._E000.m__E001.LogDebug(_E05B._E000(19852), _E001);
		}

		internal bool _E000(ZipEntry e)
		{
			return e.FileName == _E001;
		}
	}

	private const int m__E000 = 1048576;

	private readonly ILogger m__E001;

	private readonly Utils m__E002;

	public CompressionService(ILogger<CompressionService> logger, Utils utils)
	{
		this.m__E001 = logger;
		this.m__E002 = utils;
	}

	public Task ExtractZipToDir(string zipFile, string dir, CancellationToken cancellationToken, Action<long, long> onProgress)
	{
		long num;
		long location;
		Dictionary<ZipEntry, long> dictionary;
		DateTime dateTime;
		TimeSpan timeSpan;
		return Task.Run(delegate
		{
			this.m__E001.LogDebug(_E05B._E000(20135), zipFile, dir);
			if (!ZipFile.IsZipFile(zipFile))
			{
				throw new CompressionServiceException(BsgExceptionCode.FileIsNotAZipArchive, zipFile);
			}
			using (ZipFile zipFile2 = ZipFile.Read(zipFile))
			{
				zipFile2.BufferSize = 1048576;
				zipFile2.CodecBufferSize = 1048576;
				long size = zipFile2.Entries.Sum((ZipEntry entry) => entry.UncompressedSize);
				this.m__E002.ThrowIfNotEnoughSpace(Path.Combine(dir, _E05B._E000(20126)), size);
				num = zipFile2.Entries.Sum((ZipEntry e) => e.UncompressedSize);
				location = 0L;
				dictionary = new Dictionary<ZipEntry, long>();
				dateTime = DateTime.Now;
				timeSpan = TimeSpan.FromMilliseconds(200.0);
				if (onProgress != null)
				{
					onProgress(location, num);
					zipFile2.ExtractProgress += delegate(object sender, ExtractProgressEventArgs e)
					{
						lock (dictionary)
						{
							DateTime now = DateTime.Now;
							bool flag = now - dateTime > timeSpan;
							if (flag)
							{
								dateTime = now;
							}
							switch (e.EventType)
							{
							case ZipProgressEventType.Extracting_BeforeExtractEntry:
								dictionary.Add(e.CurrentEntry, 0L);
								break;
							case ZipProgressEventType.Extracting_AfterExtractEntry:
								dictionary.Remove(e.CurrentEntry);
								Interlocked.Add(ref location, e.CurrentEntry.UncompressedSize);
								break;
							case ZipProgressEventType.Extracting_EntryBytesWritten:
								if (flag && dictionary.ContainsKey(e.CurrentEntry))
								{
									dictionary[e.CurrentEntry] = e.BytesTransferred;
								}
								break;
							}
							if (flag)
							{
								onProgress(location + dictionary.Sum((KeyValuePair<ZipEntry, long> kvp) => kvp.Value), num);
							}
						}
					};
				}
				try
				{
					foreach (ZipEntry item in zipFile2)
					{
						cancellationToken.ThrowIfCancellationRequested();
						item.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
					}
				}
				catch (Exception ex) when (ex.HResult == -2147024784)
				{
					ex.AddDiskLetter(dir);
					throw ex;
				}
				onProgress?.Invoke(num, num);
			}
			this.m__E001.LogDebug(_E05B._E000(20196), zipFile, dir);
		});
	}

	public async Task CreateZipFromDir(string dir, string zipFile, CancellationToken cancellationToken, Action<long, long> onProgress)
	{
		await Task.Run(delegate
		{
			this.m__E001.LogDebug(_E05B._E000(19756), zipFile, dir);
			using (ZipFile zipFile2 = new ZipFile())
			{
				zipFile2.BufferSize = 1048576;
				zipFile2.CodecBufferSize = 1048576;
				zipFile2.UseZip64WhenSaving = Zip64Option.Always;
				zipFile2.SaveProgress += delegate(object s, SaveProgressEventArgs e)
				{
					onProgress?.Invoke(e.BytesTransferred, e.TotalBytesToTransfer);
				};
				zipFile2.AddDirectory(dir);
				zipFile2.Save(zipFile);
			}
			this.m__E001.LogDebug(_E05B._E000(19823), zipFile, (float)new FileInfo(zipFile).Length / 1024f / 1024f);
		});
	}

	public Stream CreateZip(IDictionary<string, string> files)
	{
		this.m__E001.LogDebug(_E05B._E000(19990), files.Count);
		using ZipFile zipFile = new ZipFile();
		zipFile.BufferSize = 1048576;
		zipFile.CodecBufferSize = 1048576;
		zipFile.UseZip64WhenSaving = Zip64Option.Always;
		foreach (KeyValuePair<string, string> file in files)
		{
			zipFile.AddFile(file.Key, file.Value);
		}
		MemoryStream memoryStream = new MemoryStream();
		zipFile.Save(memoryStream);
		this.m__E001.LogDebug(_E05B._E000(20040));
		return memoryStream;
	}

	public async Task ReadFileFromZip(string zipFilePath, string fileToReadRelativePath, Stream outputStream)
	{
		await Task.Run(delegate
		{
			this.m__E001.LogDebug(_E05B._E000(19873), fileToReadRelativePath, zipFilePath);
			using (ZipFile zipFile = new ZipFile(zipFilePath))
			{
				zipFile.EntriesSorted.First((ZipEntry e) => e.FileName == fileToReadRelativePath).Extract(outputStream);
			}
			this.m__E001.LogDebug(_E05B._E000(19852), fileToReadRelativePath);
		});
	}
}
