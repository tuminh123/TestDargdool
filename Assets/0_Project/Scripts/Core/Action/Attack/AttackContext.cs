using UnityEngine;

public class AttackContext : MonoBehaviour
{
    public bool IsAttacking { get; private set; }
    public int CurrentAttackId { get; private set; }
    public void EnableAttack()
    {
        IsAttacking = true;
    }

    public void DisableAttack()
    {
        IsAttacking = false;
    }
}
