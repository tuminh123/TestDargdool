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
        if (LevelManager.Instance == null) return;

        var lm = LevelManager.Instance;

        SyncUI(lm.CurrentExp, lm.ExpToNext);

        lm.OnExpChanged += OnExpChanged;

    }

    protected virtual void OnDestroy()
    {
        GameEventBus.OnGameRestart -= OnPlayerSpawned;

        if (LevelManager.Instance == null)return;

        LevelManager.Instance.OnExpChanged -= OnExpChanged;
    }

    private void OnPlayerSpawned()
    {
        if (LevelManager.Instance == null) return;

        var lm = LevelManager.Instance;

        // ✅ Sync lại UI sau restart
        SyncUI(lm.CurrentExp, lm.ExpToNext);
    }

    private void OnExpChanged(int current, int max)
    {
        UpdateBar(current, max);
        UpdateText(current, max);
    }

    private void SyncUI(int current, int max)
    {
        fill.fillAmount = max > 0 ? (float)current / max : 0f;
        UpdateText(current, max);
    }

    protected virtual void UpdateBar(float current, float max)
    {
        float targetFill = max <= 0 ? 0f : current / max;

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
