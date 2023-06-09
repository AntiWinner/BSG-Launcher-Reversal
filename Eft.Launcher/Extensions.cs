using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Security.Cryptography.MD5;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.Logging;

namespace Eft.Launcher;

public static class Extensions
{
	private const string _E000 = "\\\\?\\";

	public static string ToHex(this byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException(_E05B._E000(57428));
		}
		StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
		foreach (byte b in bytes)
		{
			stringBuilder.Append(b.ToString(_E05B._E000(57438)));
		}
		return stringBuilder.ToString();
	}

	public static async Task<string> ToHex(this Task<byte[]> bytes)
	{
		return (await bytes).ToHex();
	}

	public static string GetHexHashCode(this string str)
	{
		return string.Format(_E05B._E000(57507), str.GetHashCode());
	}

	public static string GetMd5(this string str)
	{
		try
		{
			using System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
			return mD.ComputeHash(Encoding.UTF8.GetBytes(str)).ToHex();
		}
		catch (TargetInvocationException)
		{
			return new Eft.Launcher.Security.Cryptography.MD5.MD5
			{
				Value = str
			}.Hash;
		}
	}

	public static byte[] GetHash(this string str, HashAlgorithm hashAlgorithm)
	{
		if (str == null)
		{
			throw new ArgumentNullException(_E05B._E000(57509));
		}
		return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
	}

	public static string ToSecretData(this string data)
	{
		return data.GetHexHashCode();
	}

	public static async Task<bool> WaitOneAsync(this WaitHandle handle, int millisecondsTimeout = -1, CancellationToken cancellationToken = default(CancellationToken))
	{
		RegisteredWaitHandle registeredWaitHandle = null;
		CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
		try
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(handle, delegate(object state, bool timedOut)
			{
				((TaskCompletionSource<bool>)state).TrySetResult(!timedOut);
			}, taskCompletionSource, millisecondsTimeout, executeOnlyOnce: true);
			cancellationTokenRegistration = cancellationToken.Register(delegate(object state)
			{
				((TaskCompletionSource<bool>)state).TrySetCanceled();
			}, taskCompletionSource);
			return await taskCompletionSource.Task;
		}
		finally
		{
			registeredWaitHandle?.Unregister(null);
			cancellationTokenRegistration.Dispose();
		}
	}

	public static void ForEach<T>(this ICollection<T> source, Action<T> action)
	{
		foreach (T item in source)
		{
			action(item);
		}
	}

	public static bool Contains<T>(this Exception exc, Func<T, bool> filter = null) where T : Exception
	{
		if (!(exc is T) || (filter != null && !filter(exc as T)))
		{
			return exc.InnerException?.Contains(filter) ?? false;
		}
		return true;
	}

	public static DateTime FromUnixTimestampToDateTime(this long unixTimeStamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
	}

	public static long ToUnixTimeStamp(this DateTime dateTime)
	{
		return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static Task Catch(this Task task, Action<Exception> exceptionHandler)
	{
		return task.Catch<Exception>(exceptionHandler);
	}

	public static async Task Catch<TException>(this Task task, Action<TException> exceptionHandler) where TException : Exception
	{
		try
		{
			await task;
		}
		catch (TException obj)
		{
			exceptionHandler(obj);
		}
	}

	public static string ToLongString(this TimeSpan timeSpan)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (timeSpan.Days > 0)
		{
			stringBuilder.Append(string.Format(_E05B._E000(57513), timeSpan));
		}
		if (timeSpan.Hours > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(string.Format(_E05B._E000(57525), timeSpan));
		}
		if (timeSpan.Minutes > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(string.Format(_E05B._E000(57472), timeSpan));
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(string.Format(_E05B._E000(57485), timeSpan));
		return stringBuilder.ToString();
	}

	public static string NormalizePath(this string path)
	{
		string text = Path.GetFullPath(path).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Trim(' ', Path.DirectorySeparatorChar, '?')
			.ToLowerInvariant();
		string text2 = Path.GetPathRoot(text).ToUpperInvariant();
		return text2 + text.Substring(text2.Length);
	}

	public static string WithoutLongPathPrefix(this string path)
	{
		return path?.TrimStart(Path.DirectorySeparatorChar, '?', ' ');
	}

	public static string WithLongPathPrefix(this string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !AppConfig.Instance.LongPathHandlingEnabled)
		{
			return path;
		}
		return _E05B._E000(20503) + path.WithoutLongPathPrefix();
	}

	public static async Task<T> WithExceptionsHandling<T>(this ILogger logger, string description, Func<Task<T>> func)
	{
		try
		{
			return await func();
		}
		catch (UnauthorizedApiNetworkException)
		{
			throw;
		}
		catch (ApiNetworkException ex2) when (ex2.ApiCode > 1000 && ex2.ApiCode % 1000 != 0)
		{
			logger.LogWarning(_E05B._E000(57498) + description + _E05B._E000(57568), ex2.ApiCode);
			throw;
		}
		catch (ApiNetworkException ex3)
		{
			logger.LogWarning(_E05B._E000(57543) + description + _E05B._E000(57568), ex3.ApiCode);
			throw;
		}
		catch (NetworkException ex4) when (ex4.IsLocalProblems)
		{
			logger.LogWarning(_E05B._E000(57498) + description + _E05B._E000(57548));
			throw;
		}
		catch (Exception exception)
		{
			logger.LogError(exception, _E05B._E000(57543) + description);
			throw;
		}
	}

	public static string ReplaceFirst(this string text, string search, string replace)
	{
		int num = text.IndexOf(search);
		if (num < 0)
		{
			return text;
		}
		return text.Substring(0, num) + replace + text.Substring(num + search.Length);
	}
}
