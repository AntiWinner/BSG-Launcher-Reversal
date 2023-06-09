using System;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.ConsistencyService;

[Obsolete]
public interface IConsistencyService
{
	string ConsistencyInfoFileName { get; }

	IConsistencyCheckingResult LastConsistencyCheckingResult { get; }

	event EventHandler OnConsistencyCheckingStarted;

	event EventHandler<IConsistencyCheckingResult> OnConsistencyCheckingCompleted;

	event EventHandler OnConsistencyCheckingError;

	event EventHandler<IProgressReport> OnConsistencyCheckingProgress;

	event EventHandler OnRepairStarted;

	event EventHandler OnRepairCompleted;

	event EventHandler OnRepairError;

	event EventHandler<IProgressReport> OnRepairProgress;

	void CreateConsistencyInfo(string gameRootDir);

	IConsistencyCheckingResult CheckConsistency(bool fullCheck);

	Task Repair(IConsistencyCheckingResult consistencyCheckingResult);

	void CancelCurrentOperation();
}
