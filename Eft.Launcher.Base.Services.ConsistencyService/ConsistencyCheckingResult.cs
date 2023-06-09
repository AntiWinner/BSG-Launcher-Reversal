using System.Collections.Generic;
using Eft.Launcher.Services.ConsistencyService;

namespace Eft.Launcher.Base.Services.ConsistencyService;

public class ConsistencyCheckingResult : IConsistencyCheckingResult
{
	private readonly List<string> _brokenFiles = new List<string>();

	public bool IsSuccess { get; set; }

	public IReadOnlyList<string> BrokenFiles => _brokenFiles;

	public bool IsFullCheck { get; set; }

	public void AddBrockenFile(string file)
	{
		_brokenFiles.Add(file);
	}
}
