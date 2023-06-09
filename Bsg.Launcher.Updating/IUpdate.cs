using System;
using Ionic.Zip;

namespace Bsg.Launcher.Updating;

public interface IUpdate : IDisposable
{
	UpdateMetadata Metadata { get; }

	long ApproximateInstallationTime { get; }

	ZipFile Content { get; }
}
