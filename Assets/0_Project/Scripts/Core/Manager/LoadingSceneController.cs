using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Core;
using HadesSDK.Ads.Runtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneController : MonoBehaviour
{
    #region Test 2
    /*[SerializeField] private Image loadingBar;          // Image fill
    [SerializeField] private TextMeshProUGUI percentText;          // Text hiển thị %
    [SerializeField] private float minLoadTime = 3f;    // thời gian tối thiểu
    private float timer = 0f;

    [Header("Fade UI")]
    [SerializeField] private Image fadeImage;           // Image đen để fade
    [SerializeField] private float fadeDuration = 1f;

    private int count = 0;

    void Start()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        if (string.IsNullOrEmpty(SceneLoadData.NextScene))
        {
            Debug.LogError("NextScene is NULL!");
            yield break;
        }

        string sceneToLoad = SceneLoadData.NextScene;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;

        // Reset UI
        if (loadingBar) loadingBar.fillAmount = 0f;

        if (percentText) percentText.text = "0%";

        while (!async.isDone)
        {
            //timer += Time.deltaTime;
            timer += Time.unscaledDeltaTime;

            // progress Unity tối đa = 0.9
            float loadProgress = Mathf.Clamp01(async.progress / 0.9f);

            // kết hợp thời gian tối thiểu + tiến độ load thật
            float finalProgress = Mathf.Clamp01((timer / minLoadTime) * loadProgress);

            // Cập nhật Image Fill
            if (loadingBar) loadingBar.fillAmount = finalProgress;

            // Cập nhật %
            if (percentText) percentText.text = Mathf.RoundToInt(finalProgress * 100f) + "%";

            // Nếu load xong và đã đủ thời gian
            if (async.progress >= 0.9f && timer >= minLoadTime)
            {
                // Chỗ này dùng để load ADS
                // AdsManager.ShowInterstitial(() => async.allowSceneActivation = true);
                // Bắt đầu fade out

                yield return StartCoroutine(FadeOut());

                AdsManager.Instance.AoaAdHandle();

                yield return new WaitForSecondsRealtime(10f);
                async.allowSceneActivation = true;
            }
           
            yield return null;
        }
    }

    private void LoadingFirstHandle()
    {
        if (count >= 1) return;
        GameEventBus.RaiseLoadingFirst();
        count++;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;
        c.a = 1;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            //t += Time.deltaTime;
            t += Time.unscaledDeltaTime;
            c.a = 1 - (t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0;
        fadeImage.color = c;
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            //t += Time.deltaTime;
            t += Time.unscaledDeltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1;
        fadeImage.color = c;
    }*/
    #endregion

    #region Test 3
    /*[SerializeField] private TextMeshProUGUI _textPercent;
    [SerializeField] private Image _fillImage;

    [Header("Fade UI")]
    [SerializeField] private Image fadeImage;           // Image đen để fade
    [SerializeField] private float fadeDuration = 1f;

    private float currentValue = 0f;
    private float targetValue = 0f;

    private readonly int[] steps = { 8, 15, 55, 80, 92, 98, 100 };


    private void Start()
    {
        FadeIn().Forget();
        FakeLoadingRoutine().Forget();
    }

    private async UniTask FakeLoadingRoutine()
    {
        string sceneToLoad = SceneLoadData.NextScene;
        var a = SceneManager.LoadSceneAsync(sceneToLoad);
        a.allowSceneActivation = false;

        foreach (int step in steps)
        {
            targetValue = step / 100f;

            while (currentValue < targetValue)
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * 0.6f);
                UpdateUI(currentValue);
                await UniTask.Yield();
            }

            await UniTask.WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.25f));
        }

        if (AdsManager.Instance.IsFirstLoad)
        {
            await AdsManager.Instance.AoaAdsHandle();
        }

        if (AdsManager.Instance.IsFirstCheck)
        {
            await AdsManager.Instance.InterAdsHandle();
            Time.fixedDeltaTime = 0.02f;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));

        await FadeOut();

        a.allowSceneActivation = true;
        // SceneManager.LoadScene("GamePlay");
    }

   
    private async UniTask FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;
        c.a = 1;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            //t += Time.unscaledDeltaTime;
            c.a = 1 - (t / fadeDuration);
            fadeImage.color = c;
            await UniTask.Yield();
        }

        c.a = 0;
        fadeImage.color = c;
    }
    private async UniTask FadeOut()
    {
        float t = 0;
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            //t += Time.unscaledDeltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            await UniTask.Yield();
        }

        c.a = 1;
        fadeImage.color = c;
    }
    private void UpdateUI(float value)
    {
        _fillImage.fillAmount = value;
        _textPercent.text = $"{(int)(value * 100)}%";
    }*/
    #endregion
    
    [Header("Loading UI")]
    [SerializeField] private TextMeshProUGUI textPercent;
    [SerializeField] private Image fillImage;

    [Header("Fade UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.8f;

    [Header("Fake Loading Config")]
    [SerializeField] private int[] fakeSteps = { 8, 15, 55, 80, 92, 98, 100 };
    [SerializeField] private float fakeSpeed = 0.6f;

    private float currentValue;
    private float targetValue;

    private AsyncOperation loadOperation;

    private void Start()
    {
        StartAsync().Forget();
    }

    private async UniTaskVoid StartAsync()
    {
        AdsManager.Instance.HideBanner();
        await FadeIn();

        await LoadSceneAsync();

        // Show Ads (an toàn)
        await ShowAdsSafe();

        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));

        await FadeOut();

        loadOperation.allowSceneActivation = true;

        await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        AdsManager.Instance.ShowBanner();
    }

    #region Scene Loading

    private async UniTask LoadSceneAsync()
    {
        string sceneName = SceneLoadData.NextScene;

        loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        // Fake loading UI
        await FakeLoadingRoutine();

        // Đảm bảo scene load xong thật
        await UniTask.WaitUntil(() => loadOperation.progress >= 0.9f);

    }

    #endregion

    #region Fake Loading

    private async UniTask FakeLoadingRoutine()
    {
        foreach (int step in fakeSteps)
        {
            targetValue = step / 100f;

            while (currentValue < targetValue)
            {
                currentValue = Mathf.MoveTowards(
                    currentValue,
                    targetValue,
                    Time.unscaledDeltaTime * fakeSpeed
                );

                UpdateUI(currentValue);
                await UniTask.Yield();
            }

            await UniTask.Delay(
                TimeSpan.FromSeconds(UnityEngine.Random.Range(0.1f, 0.25f)),
                DelayType.UnscaledDeltaTime
            );
        }
    }

    private void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
        textPercent.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    #endregion

    #region Ads Handling (Safe)

    private async UniTask ShowAdsSafe()
    {
        await UniTask.WhenAny(
               AdsManager.Instance.AoaAdsHandle(),
               UniTask.Delay(6000)
        );

        // Interstitial Ads
        if (AdsManager.Instance.IsFirstCheck)
        {
            await UniTask.WhenAny(
                AdsManager.Instance.InterAdsHandle(),
                UniTask.Delay(6000)
            );

            Time.fixedDeltaTime = 0.02f;
        }
    }

    #endregion

    #region Fade

    private async UniTask FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = 1 - (t / fadeDuration);
            fadeImage.color = c;
            await UniTask.Yield();
        }

        c.a = 0;
        fadeImage.color = c;
    }

    private async UniTask FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            await UniTask.Yield();
        }

        c.a = 1;
        fadeImage.color = c;
    }
    /*private async UniTask Fade(float begin,float end)
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = begin;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            await UniTask.Yield();
        }

        c.a = end;
        fadeImage.color = c;
    }
*/

    #endregion
}

