using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRegenerationButton : MonoBehaviour
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
        if (AdsManager.Instance == null)
        {
            Debug.LogWarning("AdsManager.Instance is NULL – skip ads");
            return;
        }
        bool success = await AdsManager.Instance.RewardAdsHandles();
        if (success)
        {
            GameEventBus.RaisePlayerRegeneration();
        }
        else
        {
            Debug.Log("Loading");
        }
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Click);
    }
}