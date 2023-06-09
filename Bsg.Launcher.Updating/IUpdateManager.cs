using System;
using System.IO;
using System.Threading;
using Eft.Launcher;

namespace Bsg.Launcher.Updating;

public interface IUpdateManager
{
	event UpdateInstallationProblemEventHandler OnUpdateInstallationProblem;

	UpdateMetadata CreateUpdate(string oldApplicationRoot, BsgVersion oldApplicationVersion, string newApplicationRoot, BsgVersion newApplicationVersion, Stream updateOutputStream, CancellationToken cancellationToken);

	string CreateTxtUpdateReport(UpdateMetadata metadata);

	string CreateCsvUpdateReport(UpdateMetadata metadata);

	IUpdate OpenUpdate(Stream updateStream);

	void InstallUpdate(IUpdate update, string applicationRoot, Action<long, long> onProgress = null, CancellationToken cancellationToken = default(CancellationToken));
}
