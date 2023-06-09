using System.Threading;
using System.Threading.Tasks;
using Bsg.Launcher.Models;

namespace Bsg.Launcher.Queues;

public interface IQueueHandler
{
	event QueueStateChangedEventHandler OnQueueStateChanged;

	event QueueProgressEventHandler OnQueueProgress;

	Task<int> WaitForQueueAsync(QueueInfo queueInfo, CancellationToken cancellationToken);
}
