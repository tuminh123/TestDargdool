using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Runtime;
using System;
using System.Collections;
using UnityEngine;

public class AdTestManager : MonoBehaviour
{
    public static AdTestManager Instance { get; private set; }

    private const float TIMEOUT = 5f;

    // ===== STATE =====
    public bool IsFirstOpenApp { get; private set; } = true;
    public bool CanShowInter { get; private set; } = false;

    private UniTaskCompletionSource<bool> rewardTcs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AdManager.Instance.Init();
    }

    // ===================== AOA =====================
    public async UniTask TryShowAoa()
    {
        if (!IsFirstOpenApp) return;

        bool ready = await WaitUntilReady(() => AdManager.Instance.IsAoaReady());
        if (!ready) return;

        AdManager.Instance.ShowAoa();
        IsFirstOpenApp = false;
    }

    // ===================== INTER =====================
    public async UniTask TryShowInter()
    {
        if (!CanShowInter) return;

        bool ready = await WaitUntilReady(() => AdManager.Instance.IsInterstitialReady());
        if (!ready) return;

        AdManager.Instance.ShowInterstitial(null, null, "Inter_Show");
    }

    public void EnableInterAfterFirstLose()
    {
        CanShowInter = true;
    }

    // ===================== REWARD =====================
    public UniTask<bool> ShowReward()
    {
        rewardTcs = new UniTaskCompletionSource<bool>();

        try
        {
            AdManager.Instance.ShowReward(
                () => rewardTcs.TrySetResult(true),
                () => rewardTcs.TrySetResult(false),
                "Reward_Show"
            );
        }
        catch
        {
            rewardTcs.TrySetResult(false);
        }

        return rewardTcs.Task.Timeout(TimeSpan.FromSeconds(TIMEOUT));
    }

    // ===================== BANNER =====================
    public void ShowBanner()
    {
        AdManager.Instance.ShowBanner();
    }

    public void HideBanner()
    {
        AdManager.Instance.HideBanner();
    }

    // ===================== UTILS =====================
    private async UniTask<bool> WaitUntilReady(Func<bool> condition)
    {
        float t = 0f;
        while (!condition())
        {
            await UniTask.DelayFrame(1);
            t += Time.unscaledDeltaTime;
            if (t >= TIMEOUT)
                return false;
        }
        return true;
    }
}