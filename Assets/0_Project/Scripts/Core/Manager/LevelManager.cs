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

    private PlayerProgressData progress;

    public int Level => progress.level;
    public int CurrentExp => progress.currentExp;
    public int ExpToNext { get; private set; }

    public event Action<int, int> OnExpChanged;
    public event Action<int> OnLevelUp;

  /*  private void Start()
    {
        if (DataManager.Instance == null) return;
        Init(DataManager.Instance.ProgressData);
    }*/

    public void Init(PlayerProgressData data)
    {
        progress = data;
        ExpToNext = expCurve.GetExpToNextLevel(progress.level);
        OnExpChanged?.Invoke(CurrentExp, ExpToNext);
    }

    public void AddExp(int amount)
    {
        progress.currentExp += amount;

        while (progress.currentExp >= ExpToNext)
        {
            progress.currentExp -= ExpToNext;
            LevelUp();
        }

        OnExpChanged?.Invoke(CurrentExp, ExpToNext);
    }

    private void LevelUp()
    {
        progress.level++;
        ExpToNext = expCurve.GetExpToNextLevel(progress.level);
        OnLevelUp?.Invoke(progress.level);
    }
}
