using System;

namespace Eft.Launcher;

public class BsgException : CodeException
{
	public BsgExceptionCode BsgExceptionCode => (BsgExceptionCode)base.Code;

	public BsgException(BsgExceptionCode code, params string[] args)
		: this(code, null, args)
	{
	}

	public BsgException(BsgExceptionCode code, Exception innerException, params string[] args)
		: base((int)code, code.ToString(), innerException, args)
	{
	}
}
