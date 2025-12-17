using Cysharp.Threading.Tasks;
using HadesSDK;
using HadesSDK.Ads.Runtime;
using System;
using System.Collections;
using UnityEngine;


public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private const float AOA_ADS_LOAD_TIMEOUT = 5f;
    private const float RWEWARD_LOAD_TIMEOUT = 5f;
    private const float INTER_ADS_LOAD_TIMEOUT = 3f;
    private bool isFirstLoad = true;
    private bool isFirstCheck = false;

    public bool IsReward { get; private set; }
    public bool IsFirstLoad => isFirstLoad;
    public bool IsFirstCheck => isFirstCheck;
    public void SetIsFirstCheck(bool isFirstCheck)
    {
        this.isFirstCheck = isFirstCheck;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        AdManager.Instance.Init();

    }
    #region Reward Ads Handle


    public async UniTask RewardAdsHandle()
    {
        try
        {
            // AdManager.Instance.Init();

            //check conditions load ads
            //bool isRewardReady = IsAdReady(AdManager.Instance.IsRewardReady());
            //var adLoadTask = WaitForAdLoad(isRewardReady);

            var adLoadTask = WaitForAdLoad(() => AdManager.Instance.IsRewardReady());
            var timeoutTask = UniTask.WaitForSeconds(RWEWARD_LOAD_TIMEOUT);

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
    }

    #endregion

    #region Inter Ads Handle

    public async UniTask InterAdsHandle()
    {
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

                AdManager.Instance.ShowInterstitial(OnSuccess, OnFail, "Inter_Show");
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

    #region Banner Ads Handle
    public void BannerAdsHandle()
    {
        ShowBanner().Forget();
    }
    private async UniTask ShowBanner()
    {
        while (true)
        {
            AdManager.Instance.ShowBanner();
            await UniTask.WaitForSeconds(10000);
            AdManager.Instance.LoadBanner();
        }
    }
    #endregion

    #region Aoa Ads Handle
    public async UniTask AoaAdsHandle()
    {
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

                AdManager.Instance.ShowAoa();
                isFirstLoad = false;
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