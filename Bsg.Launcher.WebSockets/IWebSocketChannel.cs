using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Bsg.Launcher.WebSockets;

public interface IWebSocketChannel : IDisposable
{
	string Name { get; }

	Uri ChannelUri { get; }

	event Action OnChannelClosed;

	IDisposable Subscribe<TMessage>(Action<TMessage> onMessageReceived, [CallerMemberName] string subscriberName = "anonymous");

	Task<TMessage> WaitForMessageAsync<TMessage>(CancellationToken cancellationToken = default(CancellationToken));
}
