using System.Collections;
using UnityEngine;

public class PlayerRegenerationButton : ButtonBase
{
    public override void Clicked()
    {
        GameEventBus.RaisePlayerRegeneration();
    }
}