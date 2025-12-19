using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RestartGameButton : MonoBehaviour
{

    private void Start()
    {
        if (transform.TryGetComponent(out Button button))
        {
            button.onClick.AddListener(Clicked);
        }
    }

    private async void Clicked()
    {
        GameEventBus.RaiseGameRestart();

        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1;

        // SAFE CHECK AdsManager
        if (AdsManager.Instance == null)
        {
            Debug.LogWarning("AdsManager.Instance is NULL – skip ads");
            return;
        }

        if (!AdsManager.Instance.IsFirstCheck)
            return;

        await UniTask.WhenAny(
            AdsManager.Instance.InterAdsHandle(),
            UniTask.Delay(6000)
        );

    }
}
