using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Base.Services.BackendService;

internal class ServerData : IServerData
{
	public string HostNameOrIpAddress { get; }

	public int Port { get; }

	public ServerType ServerType { get; }

	public ServerData(string hostNameOrIpAddress, int port, ServerType serverType)
	{
		HostNameOrIpAddress = hostNameOrIpAddress;
		Port = port;
		ServerType = serverType;
	}
}
