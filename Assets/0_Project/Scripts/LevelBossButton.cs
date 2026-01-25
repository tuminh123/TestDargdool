using UnityEngine;

public class LevelBossButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        SingletonManager.Instance.sceneLoader.LoadBossScene();
    }

}
