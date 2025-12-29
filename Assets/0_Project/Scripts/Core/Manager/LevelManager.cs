using UnityEngine;
using System;

[Serializable]
public class PlayerProgressData
{
    public int level;
    public int currentExp;

    public PlayerProgressData()
    {
        level = 1;
        currentExp = 0;
    }

    public void Reset()
    {
        level = 1;
        currentExp = 0;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SimpleExpCurve expCurve;

    private LevelService service;

    public int Level => service.Runtime.Level;
    public int CurrentExp => service.Runtime.CurrentExp;
    public int ExpToNext => service.Runtime.ExpToNext;

    public event Action<int, int> OnExpChanged;

    public void Init(PlayerProgressData saveData)
    {
        service = new LevelService(saveData, expCurve);
        RaiseExpChanged();
    }

    public void AddExp(int amount)
    {
        service.AddExp(amount);
        RaiseExpChanged();
    }

    private void RaiseExpChanged()
    {
        OnExpChanged?.Invoke(CurrentExp, ExpToNext);
    }

    public void ResetLevel()
    {
        service.Reset();
        RaiseExpChanged();
        GameEventBus.RaiseLevelUp(Level);
    }
}


