using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Eft.Launcher.Services.GameService;

public interface IGameService
{
	BsgVersion GameVersion { get; }

	GameState GameState { get; }

	event EventHandler<bool> OnGameInstalledStateChanged;

	event EventHandler<GameStateChangedEventArgs> OnGameStateChanged;

	event EventHandler<BsgVersion> OnGameVersionChanged;

	event Func<ProcessLifecycleInformation, Task> OnGameClosedAsync;

	event Action<Process> OnGameStarted;

	void Init();

	void UpdateInstallationInfo();

	bool CheckGameIsInstalled(string dirToCheck = null);

	void UpdateGameIsInstalled();

	void UpdateGameVersion();

	Task<(string gameSession, int timeToStartGameSec)> PrepareGame(CancellationToken cancellationToken);

	Task RunGame(string gameSession);

	void CloseGame();

	void ClearCache();

	void DeleteLocalIni();

	void DeleteSharedIni();

	void InstallAnticheat();

	void UninstallAnticheat();
}
