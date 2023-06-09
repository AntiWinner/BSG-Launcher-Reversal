using System;
using System.Reflection;
using Eft.Launcher.Services.UpdateServices;

namespace Eft.Launcher.Base.Services.UpdateServices;

public class LauncherMetadata : ILauncherMetadata
{
	public Version LauncherVersion { get; }

	public LauncherMetadata()
	{
		LauncherVersion = Assembly.GetEntryAssembly().GetName().Version;
	}
}
