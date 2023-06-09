using System;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.AuthService;

public struct OAuthToken
{
	[CompilerGenerated]
	private readonly string _E000;

	[CompilerGenerated]
	private readonly DateTime _E001;

	public string Value
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public DateTime ExpirationTimeUtc
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public OAuthToken(string value, DateTime expirationTimeUtc)
	{
		_E000 = value;
		_E001 = expirationTimeUtc;
	}
}
