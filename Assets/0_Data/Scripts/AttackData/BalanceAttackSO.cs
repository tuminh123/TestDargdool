using UnityEngine;

public enum AttackType { right_punch, left_punch }

[CreateAssetMenu(fileName = "AttackDataSO", menuName = "ActionDataSO/AttackDataSO")]
public class BalanceAttackSO : BalanceActionSO
{
    public AttackType attackType;
}
