using System.Collections.Generic;

namespace Eft.Launcher.Services.ConsistencyService;

public interface IConsistencyCheckingResult
{
	bool IsSuccess { get; }

	IReadOnlyList<string> BrokenFiles { get; }

	bool IsFullCheck { get; }
}
