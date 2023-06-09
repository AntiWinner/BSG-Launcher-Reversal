namespace Eft.Launcher.Services.BackendService;

public interface IServerData
{
	string HostNameOrIpAddress { get; }

	int Port { get; }

	ServerType ServerType { get; }
}
