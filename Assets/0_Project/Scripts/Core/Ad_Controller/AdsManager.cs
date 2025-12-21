using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Runtime;
using HadesSDK.Ads.Runtime.FirebaseServices;
using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1110)]
public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private const float AOA_ADS_LOAD_TIMEOUT = 5f;
    private const float REWARD_LOAD_TIMEOUT = 5f;
    private const float INTER_ADS_LOAD_TIMEOUT = 3f;
    private bool isAoaShowSession = false;
    private bool isFirstCheck = false;

    private float _timer => Time.realtimeSinceStartup;
    private float _lastTimeShowInterAd = -999f;

    //public bool IsReward { get; private set; }
    //get
    public bool IsFirstLoad => isAoaShowSession;
    public bool IsFirstCheck => isFirstCheck;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {
        AdManager.Instance.Init();

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) => {
            // Show Mediation Debugger
            MaxSdk.ShowMediationDebugger();
        };
    }
    public void InterAdsBegin()
    {
        isFirstCheck = true;
    }

    #region Reward Ads Handle


  /*  public async UniTask RewardAdsHandle()
    {
        try
        {
            // AdManager.Instance.Init();

            //check conditions load ads
            //bool isRewardReady = IsAdReady(AdManager.Instance.IsRewardReady());
            //var adLoadTask = WaitForAdLoad(isRewardReady);

            var adLoadTask = WaitForAdLoad(() => AdManager.Instance.IsRewardReady());
            var timeoutTask = UniTask.WaitForSeconds(REWARD_LOAD_TIMEOUT);

            //Select the true conditions for use.
            var completed = await UniTask.WhenAny(adLoadTask, timeoutTask);// return value 0 & 1

            if (completed == 0)
            {
                *//* if (FirebaseService.Instance != null)
                 {
                     Debug.Log("ok");
                 }
                 else
                 {
                     Debug.Log("FirebaseService is null");
                 }*//*

                AdManager.Instance.ShowReward(OnSuccess, OnFail, "Inter_Show");
                IsReward = true;
            }
            else
            {
                Debug.Log("Quá Timeout ");
                IsReward = false;
            }
            Debug.Log(IsReward);
        }
        catch (Exception ex)
        {
            Debug.LogError($"lỗi ad : {ex.Message}");
        }
    }*/
    public async UniTask<bool> RewardAdsHandles()
    {
        try
        {
            var adLoadTask = WaitForAdLoad(() => AdManager.Instance.IsRewardReady());
            var timeoutTask = UniTask.WaitForSeconds(REWARD_LOAD_TIMEOUT);

            var completed = await UniTask.WhenAny(adLoadTask, timeoutTask);

            if (completed == 0)
            {
                bool rewardGranted = false;

                AdManager.Instance.ShowReward(
                    () => 
                    {
                        rewardGranted = true;
                        BlockInterstitial();
                    },
                    () => rewardGranted = false,
                    "Reward_Show"
                );

                await UniTask.WaitUntil(() => rewardGranted);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"lỗi ad : {ex.Message}");
            return false;     
        }
    }

    #endregion

    #region Inter Ads Handle

    public async UniTask InterAdsHandle()
    {
        if (!IsInterstitialPassCapping()) return;
        try
        {

            //check conditions load ads
            //bool isInterReady = IsAdReady(AdManager.Instance.IsInterstitialReady());
            var adLoadTask = WaitForAdLoad(() => AdManager.Instance.IsInterstitialReady());
            var timeoutTask = UniTask.WaitForSeconds(INTER_ADS_LOAD_TIMEOUT);

            //Select the true conditions for use.
            var completed = await UniTask.WhenAny(adLoadTask, timeoutTask);// return value 0 & 1

            if (completed == 0)
            {
                /* if (FirebaseService.Instance != null)
                 {
                     Debug.Log("ok");
                 }
                 else
                 {
                     Debug.Log("FirebaseService is null");
                 }*/
                if (!isFirstCheck) return;
                AdManager.Instance.ShowInterstitial(() =>
                {
                    BlockInterstitial();
                }, 
                OnFail, "Inter_Show");
            }
            else
            {
                Debug.Log("Quá Timeout ");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"lỗi ad : {ex.Message}");
        }
    }

    private void BlockInterstitial()
    {
        _lastTimeShowInterAd = _timer;
    }

    public bool IsInterstitialPassCapping()
    {
        float cappingTime = AdManager.Instance.RemoteConfig.inter_ad_capping_time;
        return (_timer - _lastTimeShowInterAd) >= cappingTime;
    }

    #endregion

    #region Banner Ads Handle
    public void ShowBanner()
    {
        // SDK show banner
        AdManager.Instance.ShowBanner();
    }

    public void HideBanner()
    {
        // SDK hide banner
        AdManager.Instance.HideBanner();
    }
    #endregion

    #region Aoa Ads Handle
    public async UniTask AoaAdsHandle()
    {

        if (isAoaShowSession) return;
        try
        {
            // AdManager.Instance.Init();

            //check conditions load ads
            /* bool isAoaReady = IsAdReady(AdManager.Instance.IsAoaReady());
             var adLoadTask = WaitForAdLoad(isAoaReady);*/

            var adLoadTask = WaitForAdLoad(() => AdManager.Instance.IsAoaReady());
            var timeoutTask = UniTask.WaitForSeconds(AOA_ADS_LOAD_TIMEOUT);

            //Select the true conditions for use.
            var completed = await UniTask.WhenAny(adLoadTask, timeoutTask);// return value 0 & 1

            if (completed == 0)
            {
                /* if (FirebaseService.Instance != null)
                 {
                     Debug.Log("ok");
                 }
                 else
                 {
                     Debug.Log("FirebaseService is null");
                 }*/


                isAoaShowSession = true;
                BlockInterstitial();

                AdManager.Instance.ShowAoa();
                
            }
            else
            {
                Debug.Log("Quá Timeout ");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"lỗi ad : {ex.Message}");
        }
    }


    #endregion

    #region Func Utills
   /* private bool IsAdReady(bool isReady)
    {
        try
        {
            return isReady;
        }
        catch
        {
            return false;
        }
    }*/

    private async UniTask WaitForAdLoad(/*bool isReady*/Func<bool> condition)
    {
        while (!condition())//!isReady
        {
            await UniTask.Yield();
        }
    }

    private void OnSuccess()
    {
        Debug.Log("Success");
    }
    private void OnFail()
    {
        Debug.Log("Fail");
    }
    #endregion

}