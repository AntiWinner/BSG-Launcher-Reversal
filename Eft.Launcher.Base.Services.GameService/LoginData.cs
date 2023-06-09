using Newtonsoft.Json;

namespace Eft.Launcher.Base.Services.GameService;

[JsonObject(MemberSerialization.OptIn)]
internal class LoginData
{
	[JsonProperty(PropertyName = "email")]
	public string Login { get; }

	[JsonProperty(PropertyName = "password")]
	public string Password { get; }

	[JsonProperty(PropertyName = "toggle")]
	public bool Remember { get; }

	[JsonProperty(PropertyName = "timestamp")]
	public long Timestamp { get; }

	public LoginData(string login, string password, bool remember, long timestamp = 0L)
	{
		Login = login;
		Password = password;
		Remember = remember;
		Timestamp = timestamp;
	}

	public override string ToString()
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			DefaultValueHandling = DefaultValueHandling.Ignore
		};
		return JsonConvert.SerializeObject(this, Formatting.None, settings);
	}
}
