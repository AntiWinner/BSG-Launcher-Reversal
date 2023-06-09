namespace Eft.Launcher.Services.UpdateServices;

public enum GameUpdateServiceState
{
	Idle,
	CheckingForUpdate,
	DownloadingUpdate,
	Pause,
	InstallingUpdate,
	RepairingGame,
	ConsistencyChecking,
	UpdateError,
	CreatingUpdate,
	CheckingUpdateHash
}
