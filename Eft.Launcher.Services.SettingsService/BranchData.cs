using Newtonsoft.Json;

namespace Eft.Launcher.Services.SettingsService;

[JsonObject(MemberSerialization.OptIn)]
public class BranchData
{
	[JsonProperty(PropertyName = "isDefault")]
	public bool IsDefault { get; set; }

	[JsonProperty(PropertyName = "isActive")]
	public bool IsActive { get; set; }

	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "clientVersion")]
	public string MatchingTag { get; set; }

	[JsonProperty(PropertyName = "backendUri")]
	public string GameBackendUri { get; set; }

	[JsonProperty(PropertyName = "siteUri")]
	public string SiteUri { get; set; }

	[JsonProperty(PropertyName = "logsUri")]
	public string LogsUri { get; set; }

	[JsonProperty(PropertyName = "feedbackBehavior")]
	public FeedbackBehavior FeedbackBehavior { get; set; }

	[JsonProperty(PropertyName = "selectedDataCenters")]
	public string[] SelectedDataCenters { get; set; }

	[JsonProperty(PropertyName = "participantStatus")]
	public BranchParticipationStatus BranchParticipationStatus { get; set; }

	[JsonProperty(PropertyName = "status")]
	public BranchStatus BranchStatus { get; set; }
}
