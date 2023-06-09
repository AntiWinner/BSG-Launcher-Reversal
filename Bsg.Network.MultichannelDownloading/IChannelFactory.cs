namespace Bsg.Network.MultichannelDownloading;

public interface IChannelFactory
{
	bool CanCreateChannelFor(ChannelSettings channelSettings);

	IChannel CreateChannel(ChannelSettings channelSettings, MultichannelDownloaderContext context);
}
