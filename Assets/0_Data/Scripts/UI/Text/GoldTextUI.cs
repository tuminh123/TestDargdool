using UnityEngine;
using Zenject;

public class GoldTextUI : TextBase
{

    private void Start()
    {
        UpdateText(SingletonManager.Instance.goldManager.GoldCount.ToString());

        SingletonManager.Instance.goldManager.OnGoldAmountChanged += Gold_OnGoldAmountChanged;
    }

    private void OnDestroy()
    {
        SingletonManager.Instance.goldManager.OnGoldAmountChanged -= Gold_OnGoldAmountChanged;
    }

    private void Gold_OnGoldAmountChanged()
    {
        string text = $"{SingletonManager.Instance.goldManager.GoldCount}";
        UpdateText(text);
    }

   
}
