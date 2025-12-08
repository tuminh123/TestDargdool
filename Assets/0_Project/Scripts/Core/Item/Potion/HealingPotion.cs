using System;
using UnityEngine;

public class HealingPotion : PotionBase
{
    protected override void Awake()
    {
        base.Awake();
        effect = new HealEffect(amount);
    }
    public override string GetObjectName()
    {
        return StringConst.HEALINGPOTION;
    }
    
}
