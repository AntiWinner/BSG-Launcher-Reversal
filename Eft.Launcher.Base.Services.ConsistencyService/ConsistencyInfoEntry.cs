using Newtonsoft.Json;

namespace Eft.Launcher.Base.Services.ConsistencyService;

[JsonObject]
internal class ConsistencyInfoEntry
{
	public string Path { get; private set; }

	public string Hash { get; private set; }

	public long Size { get; private set; }

	public bool IsCritical { get; private set; }

	private ConsistencyInfoEntry()
	{
	}

	public ConsistencyInfoEntry(string path, string hash, long size, bool isCritical)
	{
		Path = path;
		Hash = hash;
		Size = size;
		IsCritical = isCritical;
	}
}
