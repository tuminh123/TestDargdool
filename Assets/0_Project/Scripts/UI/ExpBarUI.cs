using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI expText;
    [Header("Animation")]
    [SerializeField] private float fillDuration = 0.25f;

    private Coroutine fillRoutine;

    protected virtual void Start()
    {
        GameEventBus.OnGameRestart += OnPlayerSpawned;
       
        if (ZenManager.Instance == null || ZenManager.Instance.levelManager == null) return;
        ZenManager.Instance.levelManager.OnExpChanged += LevelManager_OnExpChanged;
        UpdateBar(ZenManager.Instance.levelManager.CurrentExp, ZenManager.Instance.levelManager.ExpToNext);
    }

    protected virtual void OnDestroy()
    {
        GameEventBus.OnGameRestart -= OnPlayerSpawned;

        if (ZenManager.Instance == null || ZenManager.Instance.levelManager == null) return;
        ZenManager.Instance.levelManager.OnExpChanged -= LevelManager_OnExpChanged;
    }

    private void OnPlayerSpawned()
    {
        if (ZenManager.Instance == null || ZenManager.Instance.levelManager == null) return;
        ZenManager.Instance.levelManager.OnExpChanged -= LevelManager_OnExpChanged;
        ZenManager.Instance.levelManager.OnExpChanged += LevelManager_OnExpChanged;
    }

    public void LevelManager_OnExpChanged(int arg1, int arg2)
    {
        UpdateBar(arg1,arg2);
        UpdateText(arg1, arg2);
    }
    protected virtual void UpdateBar(float current, float max)
    {
        float targetFill = max <= 0 ? 0f : (float)current / max;

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillRoutine = StartCoroutine(AnimateFill(targetFill));
    }
    private void UpdateText(float current, float max)
    {
        expText.text = $"{current} / {max}";
    }
    private IEnumerator AnimateFill(float target)
    {
        float start = fill.fillAmount;
        float time = 0f;

        while (time < fillDuration)
        {
            time += Time.deltaTime;
            fill.fillAmount = Mathf.Lerp(start, target, time / fillDuration);
            yield return null;
        }

        fill.fillAmount = target;
    }
}
