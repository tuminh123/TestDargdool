using UnityEngine;

public class LevelService
{
    private LevelRuntimeData runtime;
    private PlayerProgressData saveData;

    public LevelService(PlayerProgressData save, SimpleExpCurve curve)
    {
        saveData = save;
        runtime = new LevelRuntimeData(save, curve);
    }

    public LevelRuntimeData Runtime => runtime;

    public void AddExp(int amount)
    {
        runtime.AddExp(amount);

        while (runtime.CanLevelUp())
        {
            runtime.ApplyLevelUp();
            GameEventBus.RaiseLevelUp(runtime.Level);
        }

        runtime.WriteBack(saveData);
    }

    public void Reset()
    {
        saveData.Reset();
        runtime.LoadFromSave(saveData);
    }
}

