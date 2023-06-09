using System;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Services.GameService;

public class GameStateChangedEventArgs : EventArgs
{
	[CompilerGenerated]
	private readonly GameState _E000;

	[CompilerGenerated]
	private readonly GameState _E001;

	public GameState OldState
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public GameState NewState
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public GameStateChangedEventArgs(GameState oldState, GameState newState)
	{
		_E000 = oldState;
		_E001 = newState;
	}
}
