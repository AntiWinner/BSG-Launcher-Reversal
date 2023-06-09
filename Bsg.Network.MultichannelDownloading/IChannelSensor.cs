namespace Bsg.Network.MultichannelDownloading;

public interface IChannelSensor
{
	IChannel Channel { get; }

	bool IsActive { get; }

	int Speed { get; }
}
