using System;

namespace Eft.Launcher.Services.UpdateServices;

public interface ILauncherMetadata
{
	Version LauncherVersion { get; }
}
