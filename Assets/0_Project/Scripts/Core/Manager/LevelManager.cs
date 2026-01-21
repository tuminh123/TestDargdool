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
    public event Action<int, int> OnExpChanged;
    public static LevelManager Instance { get;private set; }
    [SerializeField] private SimpleExpCurve expCurve;
    private LevelRuntimeData runtimeData;

    public int Level => runtimeData.Level;
    public int CurrentExp => runtimeData.CurrentExp;
    public int ExpToNext => runtimeData.ExpToNext;

    public LevelRuntimeData RuntimeData => runtimeData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        Init();

        GameEventBus.OnGameRestart += GameEventBus_OnGameRestart;
    }

    private void OnDestroy()
    {
        GameEventBus.OnGameRestart -= GameEventBus_OnGameRestart;
    }
    private void GameEventBus_OnGameRestart()
    {
        Init();
    }

    public void Init()
    {
        runtimeData = new LevelRuntimeData(expCurve);

        RaiseExpChanged();
    }

    public void AddExp(int amount)
    {
        ExpHandle(amount);
        RaiseExpChanged();
    }

    private void RaiseExpChanged()
    {
        OnExpChanged?.Invoke(CurrentExp, ExpToNext);
    }


    public void ExpHandle(int amount)
    {
        runtimeData.AddExp(amount);

        while (runtimeData.CanLevelUp())
        {
            runtimeData.ApplyLevelUp();
            GameEventBus.RaiseLevelUp(runtimeData.Level);
        }
    }
}


