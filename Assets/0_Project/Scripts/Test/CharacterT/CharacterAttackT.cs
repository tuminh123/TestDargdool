using UnityEngine;

public class CharacterAttackT : CharacterMainT
{
    public CharacterAttackT(CharacterT characterT, StateMachine stateMachine) : base(characterT, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterT.attack.DisableBalance();
        characterT.attack.Attack(characterT.AttackDir);

        characterT.attack.currentAttack.OnAttackEnd += CurrentAttack_OnAttackEnd;
    }

    private void CurrentAttack_OnAttackEnd()
    {
        stateMachine.ChangeState(characterT.idleT);
    }

    public override void Exit()
    {
        base.Exit();
        characterT.attack.EnableBalance();
        characterT.attack.currentAttack.OnAttackEnd -= CurrentAttack_OnAttackEnd;
    }
}
