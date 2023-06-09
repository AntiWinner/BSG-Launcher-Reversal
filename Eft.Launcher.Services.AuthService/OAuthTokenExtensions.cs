using System;
using Bsg.Launcher.Services.AuthService;

namespace Eft.Launcher.Services.AuthService;

public static class OAuthTokenExtensions
{
	public static bool IsExpired(this OAuthToken token)
	{
		if (!string.IsNullOrEmpty(token.Value))
		{
			return token.ExpirationTimeUtc < DateTime.UtcNow;
		}
		return true;
	}

	public static void ThrowIfExpired(this OAuthToken token)
	{
		if (token.IsExpired())
		{
			throw new AuthServiceTokenExpiredException();
		}
	}
}
