using System;
using UnityEngine;

public class HealingPotion : PotionBase
{
    private void Awake()
    {
        effect = new HealEffect(amount);
    }
    public override string GetObjectName()
    {
        return StringConst.HEALINGPOTION;
    }
    
}
