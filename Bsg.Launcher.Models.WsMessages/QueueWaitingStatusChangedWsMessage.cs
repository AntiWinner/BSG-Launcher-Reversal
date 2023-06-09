using Newtonsoft.Json;

namespace Bsg.Launcher.Models.WsMessages;

public class QueueWaitingStatusChangedWsMessage
{
	[JsonProperty("count")]
	public int PassedUsersCount { get; set; }

	[JsonProperty("avg_time")]
	public double AverageWaitingTimeByUserSec { get; set; }
}
