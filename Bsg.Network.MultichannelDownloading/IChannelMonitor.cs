using System;
using System.Collections.Generic;

namespace Bsg.Network.MultichannelDownloading;

public interface IChannelMonitor : IDisposable
{
	IReadOnlyCollection<IChannelSensor> Sensors { get; }

	int Speed { get; }

	event Action OnSpareNodeActivationThresholdReached;

	event Action OnSpareNodeActivationThresholdRestored;

	event Action<IChannel> OnConnectionBreakingThresholdReached;
}
