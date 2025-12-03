using System.Collections;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public event System.Action OnGoldAmountChanged;

    public bool AddGold(int count)
    {
        if (count < 0) return false;

        SingletonManager.Instance.dataManager.Data.goldCount += count;
        OnGoldAmountChanged?.Invoke();
        return true;
    }
    public bool MinusGold(int count)
    {
        if (SingletonManager.Instance.dataManager.Data.goldCount == 0) return false;
        if (count > SingletonManager.Instance.dataManager.Data.goldCount) return false;

        SingletonManager.Instance.dataManager.Data.goldCount -= count;
        OnGoldAmountChanged?.Invoke();
        return true;
    }

}