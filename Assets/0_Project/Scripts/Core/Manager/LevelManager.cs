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
[DefaultExecutionOrder(-1010)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get;private set; }
    [SerializeField] private SimpleExpCurve expCurve;

    private LevelService service;

    public int Level;
    public int CurrentExp => service.Runtime.CurrentExp;
    public int ExpToNext => service.Runtime.ExpToNext;

    public event Action<int, int> OnExpChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void Init(PlayerProgressData saveData)
    {
        service = new LevelService(saveData, expCurve);

        Level = service.Runtime.Level;

        RaiseExpChanged();
    }

    public void AddExp(int amount)
    {
        service.AddExp(amount);
        Level = service.Runtime.Level;
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


