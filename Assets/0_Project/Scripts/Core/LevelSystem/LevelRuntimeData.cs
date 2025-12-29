using UnityEngine;

public class LevelRuntimeData
{
    public int Level { get; private set; }
    public int CurrentExp { get; private set; }
    public int ExpToNext { get; private set; }

    private SimpleExpCurve expCurve;

    public LevelRuntimeData(PlayerProgressData saveData, SimpleExpCurve curve)
    {
        expCurve = curve;
        LoadFromSave(saveData);
    }

    public void LoadFromSave(PlayerProgressData save)
    {
        Level = Mathf.Max(1, save.level);
        CurrentExp = Mathf.Max(0, save.currentExp);
        ExpToNext = expCurve.GetExpToNextLevel(Level);

        if (CurrentExp >= ExpToNext)
            CurrentExp = 0;
    }

    public void AddExp(int amount)
    {
        CurrentExp += amount;
    }

    public bool CanLevelUp()
    {
        return CurrentExp >= ExpToNext;
    }

    public void ApplyLevelUp()
    {
        CurrentExp -= ExpToNext;
        Level++;
        ExpToNext = expCurve.GetExpToNextLevel(Level);
    }

    public void WriteBack(PlayerProgressData save)
    {
        save.level = Level;
        save.currentExp = CurrentExp;
    }
}
