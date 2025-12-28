using UnityEngine;
using Zenject;

public class GoldTextUI : TextBase
{

    private void Start()
    {
        UpdateText(DataManager.Instance.PlayerData.GoldCount.ToString());

        SingletonManager.Instance.goldManager.OnGoldAmountChanged += Gold_OnGoldAmountChanged;
    }

    private void OnDestroy()
    {
        SingletonManager.Instance.goldManager.OnGoldAmountChanged -= Gold_OnGoldAmountChanged;
    }

    private void Gold_OnGoldAmountChanged()
    {
        string text = $"{DataManager.Instance.PlayerData.GoldCount}";

        UpdateText(text);
    }

   
}
