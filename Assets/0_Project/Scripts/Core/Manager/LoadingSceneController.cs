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
    #region Test
    /*    [SerializeField] private Slider loadingBar;
        [SerializeField] private CanvasGroup fadeGroup;

        private async void Start()
        {
            fadeGroup.alpha = 1;

            await FadeIn(0.5f);
            await LoadNextScene();
        }

        private async UniTask LoadNextScene()
        {
            string next = SceneLoadData.NextScene;

            AsyncOperation op = SceneManager.LoadSceneAsync(next);
            op.allowSceneActivation = false;

            float minLoadTime = 2f;
            float timer = 0f;

            while (op.progress < 0.9f || timer < minLoadTime)
            {
                timer += Time.deltaTime;

                float progress = Mathf.Clamp01(op.progress / 0.9f);
                float timeProgress = Mathf.Clamp01(timer / minLoadTime);

                // Lấy giá trị lớn nhất giữa load thật và load theo thời gian
                //loadingBar.value = Mathf.Max(progress, timeProgress);

                loadingBar.value = Mathf.Lerp(loadingBar.value, Mathf.Max(progress, timeProgress), 0.15f);

                await UniTask.Yield();
            }

            loadingBar.value = 1f;
            await FadeOut(0.5f);

            op.allowSceneActivation = true;
        }

        *//*private async UniTask FadeIn(float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                fadeGroup.alpha = 1 - (t / duration);
                await UniTask.Yield();
            }
            fadeGroup.alpha = 0;
        }

        private async UniTask FadeOut(float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                fadeGroup.alpha = t / duration;
                await UniTask.Yield();
            }
            fadeGroup.alpha = 1;
        }*//*
        private async UniTask FadeIn(float duration)
        {
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                fadeGroup.alpha = 1 - (t / duration);
                await UniTask.Yield();
            }
            fadeGroup.alpha = 0;
        }

        private async UniTask FadeOut(float duration)
        {
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                fadeGroup.alpha = t / duration;
                await UniTask.Yield();
            }
            fadeGroup.alpha = 1;
        }*/
    #endregion

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

    [SerializeField] private TextMeshProUGUI _textPercent;
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
    }
}
