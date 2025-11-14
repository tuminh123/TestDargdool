using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTypes
{
    Punch,
    Kick,
    Elbow,
    Knee
}

public interface IAttackAction
{
    bool IsAttacking { get; }
    IEnumerator Execute(Vector2 attackDir);
}
