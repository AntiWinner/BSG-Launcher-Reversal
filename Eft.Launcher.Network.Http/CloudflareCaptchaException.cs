using System;

namespace Eft.Launcher.Network.Http;

public class CloudflareCaptchaException : Exception
{
	public string RayId { get; }

	public CloudflareCaptchaException(string message, string rayId)
		: base(message + ". CF-RAY: " + (string.IsNullOrWhiteSpace(rayId) ? "unknown" : rayId))
	{
		RayId = rayId;
	}
}
