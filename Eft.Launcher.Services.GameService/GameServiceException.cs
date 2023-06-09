using System;

namespace Eft.Launcher.Services.GameService;

public class GameServiceException : BsgException
{
	public GameServiceException(BsgExceptionCode code, params string[] args)
		: base(code, args)
	{
	}

	public GameServiceException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base(code, innerException, args)
	{
	}
}
