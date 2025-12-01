using System.Collections;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public event System.Action OnGoldAmountChanged;

    private int goldCount;

    public int GoldCount=>goldCount;

    public void SetGoldCount(int goldCount)
    {
        this.goldCount = goldCount;
    }

    public bool AddGold(int count)
    {
        if (count < 0) return false;

        goldCount += count;
        OnGoldAmountChanged?.Invoke();
        return true;
    }
    public bool MinusGold(int count)
    {
        if (goldCount == 0) return false;
        if (count > goldCount) return false;

        goldCount -= count;
        OnGoldAmountChanged?.Invoke();
        return true;
    }

}