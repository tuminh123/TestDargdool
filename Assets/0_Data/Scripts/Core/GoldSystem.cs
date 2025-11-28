using System.Collections;
using UnityEngine;

public class GoldSystem : MonoBehaviour
{
    [SerializeField] private int goldCount;

    public void AddGold(int count)
    {
        if (count < 0) return;
        goldCount += count;
    }
    public bool MinusGold(int count)
    {
        if (goldCount == 0) return false;
        if (count > goldCount) return false;
        goldCount -= count;
        return true;
    }

}