namespace Eft.Launcher.Services.GameService;

public enum GameState
{
	Unknown,
	InstallRequired,
	RepairRequired,
	BuyRequired,
	UpdateRequired,
	ReadyToGame,
	InQueue,
	InGame,
	ReinstallRequired,
	PreparingForGame
}
