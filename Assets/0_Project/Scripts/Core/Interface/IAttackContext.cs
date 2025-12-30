using System.Collections.Generic;
using UnityEngine;

public interface IAttackContext
{
    public bool IsAttacking {  get; }
    public void EnableAttack();
    public void DisableAttack();
}
