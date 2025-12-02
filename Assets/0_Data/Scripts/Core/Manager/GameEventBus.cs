using System;
using UnityEngine;

public enum GameState
{
    MENU = 0,
    PLAY = 1,
    PAUSE = 2,
    LOSE = 4,
    WIN = 5,
    LOADING = 6,
    RESUME = 7,
    QUITGAME = 9,
    RESTART = 10,

}

public static class GameEventBus
{
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<CharacterCtrl> OnPlayerSpawned;
    public static event Action<CharacterCtrl> OnPlayerUpgrade;
    public static void RaiseGameStateChanged(GameState s) => OnGameStateChanged?.Invoke(s);
    public static void RaisePlayerSpawned(CharacterCtrl ctrl) => OnPlayerSpawned?.Invoke(ctrl);
    public static void RaisePlayerUpgrade(CharacterCtrl ctrl) => OnPlayerUpgrade?.Invoke(ctrl);
}
