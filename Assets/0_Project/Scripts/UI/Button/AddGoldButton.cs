using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AddGoldButton : MonoBehaviour
{
    
    private void Start()
    {
        if(transform.TryGetComponent(out Button button))
        {
            button.onClick.AddListener(Clicked);
        }
    }

    private async void Clicked()
    {
        bool success = await AdsManager.Instance.RewardAdsHandles();
        if (success)
        {
            SingletonManager.Instance.goldManager.AddGold(100);
        }
        else
        {
            Debug.Log("Loading");
        }
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Click);
    }
}
