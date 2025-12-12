
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;
using Zenject;

public class GameManager : MonoBehaviour
{
    [InjectOptional]
    private TimeSlow timeSlow;
    [InjectOptional]
    private UIManager uiManager;
    [SerializeField] private CharacterCtrl player;

    private void Awake()
    {
        SingletonManager.Instance.dataManager.DataLoad();
    }
    private void OnEnable()
    {
        GameEventBus.OnGameLose += HandleGameLose;
        GameEventBus.OnGameResume += GameEventBus_OnGameResume;
        GameEventBus.OnGamePause += GameEventBus_OnGamePause;
        GameEventBus.OnGameRestart += OnRestartGame;
    }

    private void OnDisable()
    {
        GameEventBus.OnGameLose -= HandleGameLose;
        GameEventBus.OnGameResume -= GameEventBus_OnGameResume;
        GameEventBus.OnGamePause -= GameEventBus_OnGamePause;
        GameEventBus.OnGameRestart -= OnRestartGame;
    }
    private void GameEventBus_OnGamePause()
    {
        Time.timeScale = 0;
    }

    private void GameEventBus_OnGameResume()
    {
        Time.timeScale = 1;
    }
    private void HandleGameLose(CharacterCtrl ctrl)
    {
        StartCoroutine(LoseHandle());
    }
    private IEnumerator LoseHandle()
    {
        ZenManager.Instance.timeSlow.gameObject.SetActive(true);
        ZenManager.Instance.timeSlow.DoSlowmotion();
        yield return new WaitForSeconds(1.5f);

        ZenManager.Instance.timeSlow.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        Time.timeScale = 0f;
        ZenManager.Instance.uIManager.PopupLose.OpenPopup();
    }
    private void OnRestartGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;   // giá trị mặc định của Unity

        ZenManager.Instance.waveSpawner.ResetWave();

        var resettableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var r in resettableObjects)
        {
            if(r==null) continue;
            if (r is IResettable resetObj) resetObj.ResetOnGameRestart();
        }

        StopAllCoroutines();
        ZenManager.Instance.uIManager.PopupLose.ClosePopup();
        Instantiate(player, Vector3.zero, Quaternion.identity);

    }
    public void ChangeMenuScene()
    {
        Time.timeScale = 1f;
        SingletonManager.Instance.sceneLoader.LoadHomeScene();
        SingletonManager.Instance.dataManager.DataSave();
    }
    public void ChangePlayScene()
    {
        SingletonManager.Instance.sceneLoader.LoadGamePlayScene();
    }
    public void ChangeUpgradeScene()
    {
        Time.timeScale = 1f;
        SingletonManager.Instance.sceneLoader.LoadUpgradeScene();
    }
    private void OnApplicationQuit()
    {
        SingletonManager.Instance.dataManager.DataSave();
    }
}
