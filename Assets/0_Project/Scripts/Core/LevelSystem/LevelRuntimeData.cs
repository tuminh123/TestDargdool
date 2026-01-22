
using Core;
using System.Security.Cryptography;
using UnityEngine;
public class LevelRuntimeData
{
    public event System.Action<int> OnLevelChange;
    public int Level { get; private set; }
    public int CurrentExp { get; private set; }
    public int ExpToNext { get; private set; }

    private SimpleExpCurve expCurve;

    public LevelRuntimeData(SimpleExpCurve curve)
    {
        expCurve = curve;
        Level = 1;
        CurrentExp = 0;
        ExpToNext = curve.GetExpToNextLevel(Level);
    }

    public void AddExp(int amount)
    {
        CurrentExp += amount;
    }

    public bool CanLevelUp()
    {
        return CurrentExp >= ExpToNext;
    }
    public void ResetLevel(SimpleExpCurve curve)
    {
        Level = 1;
        CurrentExp = 0;
        ExpToNext = curve.GetExpToNextLevel(Level);
        Global.Send(new SignalLevelText { level = 1 });
    }
    public void ApplyLevelUp()
    {
        CurrentExp -= ExpToNext;
        Level++;
        ExpToNext = expCurve.GetExpToNextLevel(Level);
        OnLevelChange?.Invoke(Level);
        Global.Send(new SignalLevelText { level = Level });
    }

}
