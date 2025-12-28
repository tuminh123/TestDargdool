
using UnityEngine;
public static class GameEventBus
{
    public static event System.Action<CharacterCtrl> OnGameLose;
    public static event System.Action<CharacterCtrl> OnPlayerUpgrade;
    public static event System.Action<CharacterCtrl> OnPlayerSpawn;
    public static event System.Action OnGamePause;
    public static event System.Action OnGameResume;
    public static event System.Action OnGameRestart;
    public static event System.Action OnPlayerRegeneration;
    public static event System.Action OnLoading;
    public static event System.Action OnLoadingDone;
    public static event System.Action OnGameWin;
    public static event System.Action OnEnemyDead;
    public static event System.Action<int> OnLevelUp;
    public static void RaisePlayerLose(CharacterCtrl ctrl) => OnGameLose?.Invoke(ctrl);
    public static void RaisePlayerUpgrade(CharacterCtrl ctrl) => OnPlayerUpgrade?.Invoke(ctrl);
    public static void RaisePlayerSpawn(CharacterCtrl ctrl) => OnPlayerSpawn?.Invoke(ctrl);
    public static void RaiseGamePause()=> OnGamePause?.Invoke();
    public static void RaiseGameResume()=> OnGameResume?.Invoke();
    public static void RaiseGameRestart()=> OnGameRestart?.Invoke();
    public static void RaisePlayerRegeneration() => OnPlayerRegeneration?.Invoke();
    public static void RaiseLoading() => OnLoading?.Invoke();
    public static void RaiseLoadingDone() => OnLoadingDone?.Invoke();
    public static void RaiseGameWin() => OnGameWin?.Invoke();
    public static void RaiseEnemyDead() => OnEnemyDead?.Invoke();
    public static void RaiseLevelUp(int level) => OnLevelUp?.Invoke(level);

}
