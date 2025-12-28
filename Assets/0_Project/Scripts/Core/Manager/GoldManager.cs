using System.Collections;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public event System.Action OnGoldAmountChanged;

    public bool AddGold(int count)
    {
        if (DataManager.Instance.PlayerData.AddGold(count))
        {
            OnGoldAmountChanged?.Invoke();
            return true;
        }
        return false;
    }
    public bool MinusGold(int count)
    {
        if (DataManager.Instance.PlayerData.MinusGold(count))
        {
            OnGoldAmountChanged?.Invoke();
            return true;
        }
        return false;
    }

}