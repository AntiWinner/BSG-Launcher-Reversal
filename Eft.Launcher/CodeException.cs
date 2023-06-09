using System;

namespace Eft.Launcher;

public class CodeException : Exception
{
	public int Code { get; set; }

	public string[] Args { get; set; }

	public CodeException(int code, string message, params string[] args)
		: this(code, message, null, args)
	{
	}

	public CodeException(int code, string message, Exception innerException, params string[] args)
		: base(message, innerException)
	{
		Code = code;
		Args = args ?? new string[0];
	}
}
