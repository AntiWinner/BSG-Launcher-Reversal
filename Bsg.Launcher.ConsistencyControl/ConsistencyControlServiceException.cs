using System;
using Eft.Launcher;

namespace Bsg.Launcher.ConsistencyControl;

public class ConsistencyControlServiceException : BsgException
{
	private ConsistencyControlServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}

	public static ConsistencyControlServiceException ChecksumDoesNotMatch(string filePath)
	{
		return new ConsistencyControlServiceException(BsgExceptionCode.TheChecksumOfTheDownloadedFileDoesNotMatch, null, filePath);
	}

	public static ConsistencyControlServiceException UnableToCalculateTheChecksum(string filePath)
	{
		return new ConsistencyControlServiceException(BsgExceptionCode.UnableToCalculateTheChecksum, null, filePath);
	}
}
