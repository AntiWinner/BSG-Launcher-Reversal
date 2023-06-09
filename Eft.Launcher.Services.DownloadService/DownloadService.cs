using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Core;
using Eft.Launcher.Services.BackendService;
using Eft.Launcher.Services.DialogService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.UpdateServices;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher.Services.DownloadService;

[Obsolete]
public class DownloadService : IDownloadService
{
	private const int CdnRequestTimeoutMs = 5000;

	private const int BreakingSpeedBytesSec = 8192;

	private const int BreakingSpeedCheckRate = 15000;

	private const int BufferSize = 262144;

	private const int ProgressRate = 350;

	private readonly ISettingsService _settingsService;

	private readonly ILogger _logger;

	private readonly IDialogService _dialogService;

	private readonly Utils _utils;

	private DateTime _lastProgressTime = DateTime.MinValue;

	public event EventHandler<BeginDownloadEventArgs> OnBeginDownload;

	public event EventHandler<EndDownloadEventArgs> OnEndDownload;

	public event Action<SlowConnectionEventArgs> OnSlowConnectionDetected;

	public DownloadService(ILogger<DownloadService> logger, ISettingsService settingsService, IDialogService dialogService, Utils utils)
	{
		_logger = logger;
		_settingsService = settingsService;
		_dialogService = dialogService;
		_utils = utils;
	}

	public async Task<long> GetFileSize(Uri fileUri)
	{
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(fileUri);
		httpWebRequest.Method = _E05B._E000(17457);
		httpWebRequest.Timeout = 5000;
		using WebResponse webResponse = await httpWebRequest.GetResponseAsync();
		return long.Parse(webResponse.Headers.Get(_E05B._E000(17460)));
	}

	public IEnumerable<string> GetFileFragments(DownloadCategory downloadCategory)
	{
		string branchNameForCategory = GetBranchNameForCategory(downloadCategory);
		return from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir, string.Format(_E05B._E000(18362), branchNameForCategory, downloadCategory))
			where GetBranchname(f) == _settingsService.SelectedBranch.Name
			select f;
	}

	public IEnumerable<string> GetFiles(DownloadCategory downloadCategory)
	{
		string branchNameForCategory = GetBranchNameForCategory(downloadCategory);
		return from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir, string.Format(_E05B._E000(18308), branchNameForCategory, downloadCategory))
			where !f.EndsWith(_E05B._E000(17413))
			where GetBranchname(f) == _settingsService.SelectedBranch.Name
			select f;
	}

	public void Cleanup()
	{
		try
		{
			_logger.LogDebug(_E05B._E000(18322));
			IEnumerable<string> second = (from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir, string.Format(_E05B._E000(18415), DownloadCategory.LauncherDistrib))
				where !f.EndsWith(_E05B._E000(17416))
				select f into x
				orderby new FileInfo(x).CreationTime descending
				select x).ToArray().Take(1);
			string[] source = _settingsService.Branches.Select((IBranch b) => b.Name).ToArray();
			string[] gameDistribs = (from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir, string.Format(_E05B._E000(18415), DownloadCategory.EftClientDistr))
				where !f.EndsWith(_E05B._E000(17416))
				select f into x
				orderby new FileInfo(x).CreationTime descending
				select x).ToArray();
			IEnumerable<string> second2 = from b in source
				select gameDistribs.FirstOrDefault((string d) => Path.GetFileName(d).StartsWith(_E05B._E000(17420) + b + _E05B._E000(24948))) into d
				where d != null
				select d;
			string[] gameUpdates = (from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir, string.Format(_E05B._E000(18415), DownloadCategory.EftClientUpdate))
				where !f.EndsWith(_E05B._E000(17416))
				select f into x
				orderby new FileInfo(x).CreationTime descending
				select x).ToArray();
			IEnumerable<string> second3 = from b in source
				select gameUpdates.FirstOrDefault((string d) => Path.GetFileName(d).StartsWith(_E05B._E000(17420) + b + _E05B._E000(24948))) into d
				where d != null
				select d;
			foreach (string item in (from f in Directory.EnumerateFiles(_settingsService.LauncherTempDir)
				where !f.EndsWith(_E05B._E000(17416))
				select f).Except(second).Except(second2).Except(second3))
			{
				File.Delete(item);
				_logger.LogInformation(_E05B._E000(18417), item);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(18396));
		}
	}

	public void Cleanup(GamePackageInfo distribResponseData)
	{
		try
		{
			_logger.LogDebug(_E05B._E000(17969), distribResponseData.Version, _settingsService.SelectedBranch.Name);
			string text = string.Format(_E05B._E000(18043), _settingsService.SelectedBranch.Name, DownloadCategory.EftClientDistr, distribResponseData.Version);
			foreach (string item in Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(27034) + text + _E05B._E000(27034)))
			{
				File.Delete(item);
				_logger.LogDebug(_E05B._E000(17990), item);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(18083), distribResponseData.Version, _settingsService.SelectedBranch.Name);
		}
	}

	public void Cleanup(GameUpdateSet gameUpdateSet)
	{
		try
		{
			_logger.LogDebug(_E05B._E000(18163), gameUpdateSet.NewVersion, _settingsService.SelectedBranch.Name);
			foreach (GameUpdateInfo update in gameUpdateSet.Updates)
			{
				string text = string.Format(_E05B._E000(17727), _settingsService.SelectedBranch.Name, DownloadCategory.EftClientUpdate, update.FromVersion, update.Version);
				foreach (string item in Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(27034) + text + _E05B._E000(27034)))
				{
					File.Delete(item);
					_logger.LogDebug(_E05B._E000(17990), item);
				}
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, _E05B._E000(17676), gameUpdateSet.NewVersion, _settingsService.SelectedBranch.Name);
		}
	}

	public void CleanFileFragments()
	{
		if (!Directory.Exists(_settingsService.LauncherTempDir))
		{
			return;
		}
		foreach (string item in Directory.EnumerateFiles(_settingsService.LauncherTempDir, _E05B._E000(17833)).Where(delegate(string f)
		{
			string branchname = GetBranchname(f);
			return branchname == null || branchname == _settingsService.SelectedBranch.Name;
		}))
		{
			for (int i = 1; i <= 5; i++)
			{
				try
				{
					File.Delete(item);
				}
				catch (IOException)
				{
					if (i < 5)
					{
						_logger.LogWarning(_E05B._E000(17843), item, 500, 5 - i);
						Thread.Sleep(500);
						continue;
					}
					throw;
				}
			}
		}
	}

	private static string GetBranchname(string file)
	{
		Regex regex = new Regex(_E05B._E000(17449));
		string fileName = Path.GetFileName(file);
		return regex.Match(fileName).Value?.Trim('(', ')');
	}

	private string GetBranchNameForCategory(DownloadCategory downloadCategory)
	{
		if (downloadCategory != DownloadCategory.EftClientDistr && downloadCategory != DownloadCategory.EftClientUpdate)
		{
			return "";
		}
		return _settingsService.SelectedBranch.Name;
	}

	public async Task<string> DownloadFile(Uri uri, string fileName, string hash, DownloadCategory downloadCategory, CancellationToken cancellationToken, Action<IProgressReport> onProgress, Action<IProgressReport> onCheckingHashProgress = null)
	{
		string branchNameForCategory = GetBranchNameForCategory(downloadCategory);
		string path = ((downloadCategory != 0) ? string.Format(_E05B._E000(17539), string.IsNullOrWhiteSpace(branchNameForCategory) ? "" : (_E05B._E000(17420) + branchNameForCategory + _E05B._E000(24948)), downloadCategory, fileName) : fileName);
		string outFile = Path.Combine(_settingsService.LauncherTempDir, path);
		if (File.Exists(outFile))
		{
			long outFileSize = new FileInfo(outFile).Length;
			onProgress?.Invoke(new ProgressReport(outFileSize, outFileSize));
			if (await CheckFile(uri, outFile, hash, cancellationToken, onCheckingHashProgress))
			{
				_logger.LogInformation(_E05B._E000(17204), outFile);
				return outFile;
			}
			_logger.LogWarning(_E05B._E000(17544), outFile);
			File.Delete(outFile);
			onProgress?.Invoke(new ProgressReport(0.0, outFileSize));
		}
		bool isDownloadComplete = false;
		Stopwatch sw = new Stopwatch();
		long bytesLoaded = 0L;
		DownloadState downloadState = DownloadState.Unknown;
		bool continueDownload = false;
		string tempFile = outFile + _E05B._E000(17413);
		try
		{
			_logger.LogDebug(_E05B._E000(17260), uri, outFile);
			continueDownload = File.Exists(tempFile);
			if (continueDownload)
			{
				_logger.LogDebug(_E05B._E000(17318), uri);
			}
			else
			{
				Directory.CreateDirectory(Path.GetDirectoryName(outFile));
			}
			long outFileSize = 0L;
			CancellationTokenSource loadingCts;
			long lastBreakingTimerPeriodBytesLoaded;
			while (!isDownloadComplete)
			{
				try
				{
					using FileStream outFileStream = new FileStream(tempFile, File.Exists(tempFile) ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
					HttpWebRequest httpWebRequest = WebRequest.CreateHttp(uri);
					httpWebRequest.AddRange(outFileStream.Position);
					httpWebRequest.Timeout = 5000;
					cancellationToken.Register(httpWebRequest.Abort);
					using (HttpWebResponse httpWebResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync()))
					{
						_logger.LogDebug(_E05B._E000(17311), httpWebResponse.StatusCode, httpWebResponse.StatusDescription);
						this.OnBeginDownload?.Invoke(this, new BeginDownloadEventArgs(uri, outFile, downloadCategory, continueDownload));
						sw.Start();
						using (Stream responseStream = httpWebResponse.GetResponseStream())
						{
							using ThrottledStream throttledResponseStream = new ThrottledStream(responseStream, _settingsService.MaxDownloadSpeedKb * 1024);
							long contentLenght = httpWebResponse.ContentLength + outFileStream.Position;
							_utils.ThrowIfNotEnoughSpace(tempFile, contentLenght);
							loadingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
							lastBreakingTimerPeriodBytesLoaded = 0L;
							using (PausableTimer breakingTimer = new PausableTimer(15000L, delegate(PausableTimer bt)
							{
								long num3 = bytesLoaded - lastBreakingTimerPeriodBytesLoaded;
								lastBreakingTimerPeriodBytesLoaded = bytesLoaded;
								if (num3 / (bt.Delay / 1000) < 8192)
								{
									_logger.LogInformation(_E05B._E000(17426), uri);
									SlowConnectionEventArgs slowConnectionEventArgs = new SlowConnectionEventArgs(uri);
									this.OnSlowConnectionDetected?.Invoke(slowConnectionEventArgs);
									if (slowConnectionEventArgs.ResetConnection)
									{
										_logger.LogInformation(_E05B._E000(17533), uri);
										loadingCts.Cancel();
									}
								}
								try
								{
									bt.Start();
								}
								catch (ObjectDisposedException)
								{
								}
							}))
							{
								breakingTimer.Start();
								int delaysCount = 0;
								byte[] buffer = new byte[262144];
								int bytesReaded = 0;
								while (outFileStream.Position < contentLenght)
								{
									while (bytesReaded == 0)
									{
										breakingTimer.Resume();
										bytesReaded = await throttledResponseStream.ReadAsync(buffer, 0, buffer.Length, loadingCts.Token);
										breakingTimer.Pause();
										if (bytesReaded != 0)
										{
											continue;
										}
										if (delaysCount >= 6)
										{
											if (await _dialogService.ShowDialog(DialogWindowMessage.UnableToDownoadFile, uri.Segments.Last()) != DialogResult.Positive)
											{
												_logger.LogWarning(_E05B._E000(16898));
												throw new Exception(string.Format(_E05B._E000(17014), uri));
											}
											_logger.LogWarning(_E05B._E000(17368));
											delaysCount = 0;
										}
										else
										{
											await Task.Delay(500);
											delaysCount++;
										}
									}
									delaysCount = 0;
									loadingCts.Token.ThrowIfCancellationRequested();
									await outFileStream.WriteAsync(buffer, 0, bytesReaded, loadingCts.Token);
									bytesLoaded += bytesReaded;
									bytesReaded = 0;
									if (onProgress != null && (DateTime.Now - _lastProgressTime).TotalMilliseconds > 350.0)
									{
										_lastProgressTime = DateTime.Now;
										await outFileStream.FlushAsync();
										ProgressReport obj = new ProgressReport(outFileStream.Position, contentLenght);
										onProgress(obj);
									}
									throttledResponseStream.MaximumBytesPerSecond = _settingsService.MaxDownloadSpeedKb * 1024;
								}
							}
							isDownloadComplete = true;
						}
						sw.Stop();
						downloadState = DownloadState.DownloadSucceded;
					}
					await outFileStream.FlushAsync();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (WebException ex2) when (ex2.Status == WebExceptionStatus.RequestCanceled)
				{
					cancellationToken.ThrowIfCancellationRequested();
					throw;
				}
				catch (WebException ex3) when (ex3.Status == WebExceptionStatus.ProtocolError)
				{
					using HttpWebResponse httpWebResponse = (HttpWebResponse)ex3.Response;
					if (httpWebResponse.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
					{
						if (!(await CheckFile(uri, tempFile, hash, cancellationToken, onCheckingHashProgress)))
						{
							_logger.LogWarning(_E05B._E000(17544), tempFile);
							throw new DownloadServiceException(BsgExceptionCode.TheChecksumOfTheDownloadedFileDoesNotMatch, ex3);
						}
						_logger.LogWarning(ex3, _E05B._E000(16977), uri, tempFile);
						hash = null;
						downloadState = DownloadState.DownloadSucceded;
						isDownloadComplete = true;
					}
					else
					{
						ExceptionDispatchInfo.Capture((ex3 as Exception) ?? throw ex3).Throw();
					}
				}
				catch (Exception exc)
				{
					string code = (exc as WebException)?.Status.ToString() ?? exc.HResult.ToString();
					long num = (File.Exists(tempFile) ? new FileInfo(tempFile).Length : 0);
					if (num != outFileSize)
					{
						outFileSize = num;
						TimeSpan timeSpan = TimeSpan.FromMilliseconds(1000.0);
						_logger.LogInformation(_E05B._E000(17088), uri, exc.GetType(), code, exc.Message, timeSpan.ToLongString());
						await Task.Delay(timeSpan);
						continue;
					}
					if (await _dialogService.ShowDialog(DialogWindowMessage.UnableToDownoadFile, exc, uri.Segments.Last()) == DialogResult.Positive)
					{
						_logger.LogInformation(_E05B._E000(16753), uri, exc.GetType(), code, exc.Message);
						continue;
					}
					_logger.LogWarning(exc, _E05B._E000(16884), uri, exc.GetType(), code, exc.Message);
					throw new Exception(string.Format(_E05B._E000(17014), uri), exc);
				}
			}
			bool flag = !string.IsNullOrEmpty(hash);
			if (flag)
			{
				flag = !(await CheckFile(uri, tempFile, hash, cancellationToken, onCheckingHashProgress));
			}
			if (flag)
			{
				throw new DownloadServiceException(BsgExceptionCode.TheChecksumOfTheDownloadedFileDoesNotMatch);
			}
			if (File.Exists(outFile))
			{
				File.Delete(outFile);
			}
			File.Move(tempFile, outFile);
			_logger.LogDebug(_E05B._E000(16462), new FileInfo(outFile).Length, uri);
			return outFile;
		}
		catch (OperationCanceledException)
		{
			downloadState = DownloadState.DownloadCancelled;
			throw;
		}
		catch (WebException ex5) when (ex5.Status == WebExceptionStatus.ProtocolError)
		{
			File.Delete(tempFile);
			downloadState = DownloadState.DownloadFailed;
			throw;
		}
		catch (IOException)
		{
			File.Delete(tempFile);
			downloadState = DownloadState.DownloadCancelled;
			throw;
		}
		catch (DownloadServiceException ex7) when (ex7.BsgExceptionCode == BsgExceptionCode.TheChecksumOfTheDownloadedFileDoesNotMatch)
		{
			File.Delete(tempFile);
			downloadState = DownloadState.DownloadCancelled;
			throw;
		}
		catch (Exception ex8)
		{
			downloadState = DownloadState.DownloadFailed;
			if (ex8 is BsgException)
			{
				throw;
			}
			throw new DownloadServiceException(BsgExceptionCode.ErrorWhileDownloadingTheFile, ex8, uri.ToString());
		}
		finally
		{
			int averageDownloadSpeedMbitSec = -1;
			double num2 = (isDownloadComplete ? ((double)sw.ElapsedMilliseconds) : (-1.0));
			if (num2 > 0.0)
			{
				averageDownloadSpeedMbitSec = (int)((double)bytesLoaded * 8.0 / (num2 / 1000.0) / 1024.0 / 1024.0);
			}
			this.OnEndDownload?.Invoke(this, new EndDownloadEventArgs(uri, outFile, downloadCategory, continueDownload, averageDownloadSpeedMbitSec, downloadState));
		}
	}

	private async Task<bool> CheckFile(Uri fileUri, string filePath, string hash, CancellationToken cancellationToken, Action<IProgressReport> onProgress)
	{
		_logger.LogDebug(_E05B._E000(16519), filePath, fileUri);
		if (!string.IsNullOrEmpty(hash))
		{
			if (!_utils.CheckHashForFile(filePath, hash, cancellationToken, onProgress))
			{
				_logger.LogWarning(_E05B._E000(16624), filePath, new FileInfo(filePath).Length, fileUri);
				return false;
			}
			return true;
		}
		if (fileUri != null)
		{
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)(await ((HttpWebRequest)WebRequest.Create(fileUri)).GetResponseAsync()))
			{
				if (httpWebResponse.ContentLength != new FileInfo(filePath).Length)
				{
					_logger.LogWarning(_E05B._E000(20281), filePath, fileUri);
					return false;
				}
			}
			_logger.LogDebug(_E05B._E000(20402), filePath, fileUri);
			return true;
		}
		_logger.LogWarning(_E05B._E000(20338));
		return false;
	}
}
