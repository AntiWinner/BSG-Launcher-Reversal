using System;
using System.Threading;

namespace Bsg.Launcher.WebSockets;

public interface IWebSocketClientFactory
{
	IWebSocketChannel CreateChannel(Uri channelUri, CancellationToken cancellationToken = default(CancellationToken));
}
