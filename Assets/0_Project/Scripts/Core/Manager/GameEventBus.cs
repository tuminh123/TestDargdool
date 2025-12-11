
using UnityEngine;
public static class GameEventBus
{
    public static event System.Action<CharacterCtrl> OnGameLose;
    public static event System.Action<CharacterCtrl> OnPlayerUpgrade;
    public static event System.Action OnGamePause;
    public static event System.Action OnGameResume;
    public static void RaisePlayerLose(CharacterCtrl ctrl) => OnGameLose?.Invoke(ctrl);
    public static void RaisePlayerUpgrade(CharacterCtrl ctrl) => OnPlayerUpgrade?.Invoke(ctrl);
    public static void RaiseGamePause()=> OnGamePause?.Invoke();
    public static void RaiseGameResume()=> OnGameResume?.Invoke();
}
