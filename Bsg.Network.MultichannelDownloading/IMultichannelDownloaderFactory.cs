using System;
using System.Collections.Generic;

namespace Bsg.Network.MultichannelDownloading;

public interface IMultichannelDownloaderFactory
{
	event Action<IMultichannelDownloader> OnMultichannelDownloaderCreated;

	IMultichannelDownloader Create(IReadOnlyCollection<ChannelSettings> сhannelSettings);
}
