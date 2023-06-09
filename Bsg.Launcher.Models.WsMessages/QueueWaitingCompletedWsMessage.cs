using Newtonsoft.Json;

namespace Bsg.Launcher.Models.WsMessages;

public class QueueWaitingCompletedWsMessage
{
	[JsonProperty("time_to_start_game_sec")]
	public int TimeToStartGameSec { get; set; }
}
