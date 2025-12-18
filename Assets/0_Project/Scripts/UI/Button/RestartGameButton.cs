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
        if (AdsManager.Instance.IsFirstCheck)
        {
            await UniTask.WhenAny(
                AdsManager.Instance.InterAdsHandle(),
                UniTask.Delay(6000)
            );

            Time.fixedDeltaTime = 0.02f;
            Time.timeScale = 1;
        }
    }
}
