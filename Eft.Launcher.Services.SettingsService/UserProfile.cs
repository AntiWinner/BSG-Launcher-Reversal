using Bsg.Network.MultichannelDownloading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.SettingsService;

public class UserProfile
{
	[JsonProperty(PropertyName = "aid")]
	public string AccountId { get; set; }

	[JsonProperty(PropertyName = "nickname")]
	public string Nickname { get; set; }

	[JsonProperty(PropertyName = "lang")]
	public string UserLanguage { get; set; }

	[JsonProperty(PropertyName = "region")]
	public string UserRegion { get; set; }

	[JsonProperty(PropertyName = "ip_region")]
	public string IpRegion { get; set; }

	[JsonProperty(PropertyName = "gameVersion")]
	public string GameEdition { get; set; }

	[JsonProperty(PropertyName = "branchConfig")]
	public BranchData[] Branches { get; set; }

	[JsonProperty(PropertyName = "checkLegal")]
	public bool? IsLicenseAgreementAlreadyAccepted { get; set; }

	[JsonProperty(PropertyName = "purchaseDate")]
	public string PurchaseDate { get; private set; }

	[JsonProperty(PropertyName = "userAvatar")]
	public string AvatarUrl { get; set; }

	[JsonProperty(PropertyName = "supportUnreadNotifications")]
	public int? SupportUnreadNotifications { get; private set; }

	[JsonProperty(PropertyName = "sitePushStreamChannels")]
	public JArray SitePushStreamChannels { get; private set; }

	[JsonProperty(PropertyName = "geoInfo")]
	public JToken GeoInfo { get; private set; }

	public MultichannelDownloaderOptions MultiChannelDownloadSettings { get; set; }

	public ChannelSettings[] ChannelSettings { get; set; }
}
