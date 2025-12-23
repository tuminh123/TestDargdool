
using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Core;
using HadesSDK.Ads.Runtime;
using HadesSDK.Ads.Runtime.FirebaseServices;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;
using Zenject;
using Zenject.SpaceFighter;

public class GameManager : MonoBehaviour
{
    [InjectOptional]
    private TimeSlow timeSlow;
    [InjectOptional]
    private UIManager uiManager;
    [SerializeField] CharacterCtrl playerPrefab;


    /*private void Awake()
    {
        SingletonManager.Instance.dataManager.DataLoad();
    }*/


    private void OnEnable()
    {
        GameEventBus.OnGameLose += HandleGameLose;
        GameEventBus.OnGameResume += GameEventBus_OnGameResume;
        GameEventBus.OnGamePause += GameEventBus_OnGamePause;
        GameEventBus.OnGameRestart += OnRestartGame;
        GameEventBus.OnPlayerRegeneration += GameEventBus_OnPlayerRegeneration;
        GameEventBus.OnGameWin += GameWinHandle;
    }
    private void OnDisable()
    {
        GameEventBus.OnGameLose -= HandleGameLose;
        GameEventBus.OnGameResume -= GameEventBus_OnGameResume;
        GameEventBus.OnGamePause -= GameEventBus_OnGamePause;
        GameEventBus.OnGameRestart -= OnRestartGame;
        GameEventBus.OnPlayerRegeneration -= GameEventBus_OnPlayerRegeneration;
        GameEventBus.OnGameWin -= GameWinHandle;
        bool isBannerOn = FirebaseService.Instance.GetRemoteConfig<RemoteConfig>().banner_ad_on;
        if (isBannerOn)
        {
            // ShowBanner
        }
        else
        {
            // Hide Banner
        }
    }

    /* private void GameEventBus_OnLoadingFirst()
     {
         Debug.Log("Loading done. Show aoa ad");
         AdsManager.Instance.NotifyLoadingFinished();
     }*/


    #region Game event handle
    private void GameEventBus_OnPlayerRegeneration()
    {
        if (FirebaseService.Instance != null)
        {
            FirebaseService.Instance.LogEvent("Regeneration Event", new EventParameter("Regeneration", "player regeneration"));
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        CharacterCtrl player = CharacterCtrl.Instance;

        if (player == null) return;
        if (player.healthBase == null) return;

        if (player.healthBase.IsDead) 
        {
            player.transform.position = player.LastPositionBeforeDead;

            player.SetTriggerBalance(true);
        }

        player.weaponEquip.UnEquipping();

        ZenManager.Instance.uIManager.PopupLose.ClosePopup();

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
        ZenManager.Instance.uIManager.OpenPopup(ZenManager.Instance.uIManager.PopupLose);
        SingletonManager.Instance.soundManager.PlaySound(SoundType.GameFail);

        if (FirebaseService.Instance != null)
        {
            FirebaseService.Instance.LogEvent("Lose Event", new EventParameter("Lose", "don't complete all wave"));
        }


        if (AdsManager.Instance == null)
        {
            Debug.LogWarning("AdsManager.Instance is NULL – skip ads");
            yield break;
        }
        AdsManager.Instance.InterAdsBegin();


    }
    private void OnRestartGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;  

        ZenManager.Instance.waveSpawner.ResetWave();

        var resettableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var r in resettableObjects)
        {
            if(r == null) continue;
            if (r is IResettable resetObj) resetObj.ResetOnGameRestart();
        }

        StopAllCoroutines();
        ZenManager.Instance.uIManager.CloseCurrentPopup();

        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

    }
    private void GameWinHandle()
    {

        Time.timeScale = 0f;
        ZenManager.Instance.uIManager.OpenPopup(ZenManager.Instance.uIManager.PopupWin);
        SingletonManager.Instance.soundManager.PlaySound(SoundType.WinClap);

        if (FirebaseService.Instance != null)
        {
            FirebaseService.Instance.LogEvent("Win Event", new EventParameter("Win", "complete all wave"));
        }
       

        if (AdsManager.Instance == null)
        {
            Debug.LogWarning("AdsManager.Instance is NULL – skip ads");
        }
        AdsManager.Instance.InterAdsBegin();
    }
    #endregion

    #region Chanage scene
    public void ChangeMenuScene()
    {
        Time.timeScale = 1f;
        SingletonManager.Instance.sceneLoader.LoadHomeScene();
        DataManager.Instance.DataSave();
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
    #endregion
    private void OnApplicationQuit()
    {
        DataManager.Instance.DataSave();
    }
    private async void OnApplicationPause(bool pause)
    {
        if (!pause) // app RESUME
        {
            DataManager.Instance.DataSave();

            if (AdsManager.Instance != null)
            {
                await AdsManager.Instance.AoaAdsHandle();
            }
        }

    }
}
