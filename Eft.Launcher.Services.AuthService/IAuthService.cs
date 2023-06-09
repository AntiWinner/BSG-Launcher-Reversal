using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.AuthService;

public interface IAuthService
{
	bool IsLoggedIn { get; }

	OAuthToken AccessToken { get; }

	event EventHandler OnLoggedOut;

	event EventHandler OnLoggedIn;

	event AuthorizationErrorEventHandler OnAuthorizationError;

	void EnsureSuccessAuthorization(HttpResponseMessage response);

	Task LogIn(string email, string passwordHash, string captcha);

	Task<bool> LoginBySavedToken();

	Task<OAuthToken> UpdateAccessToken();

	Task LogOut();

	Task ActivateHardware(string email, string activationCode);

	Task<JToken> BindPhone(string email, string phone);

	Task<JToken> VerifyPhone(string email, string verificationCode);
}
