using UnityEngine;

public class character_idle : character_state_base
{
    public character_idle(character_test test, StateMachine stateMachine, string stateName, action_type type) : base(test, stateMachine, stateName, type)
    {
        balanceParts = new[]
        {
            BalanceType.left_arm,
            BalanceType.left_forearm,
            BalanceType.left_hand,
            BalanceType.right_arm,
            BalanceType.right_forearm,
            BalanceType.right_hand,
            BalanceType.left_leg,
            BalanceType.left_lower_leg,
            BalanceType.right_leg,
            BalanceType.right_lower_leg
        };
    }

    public override void Enter()
    {
        time = 5;

        base.Enter();
       
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(test.attack_state);
        }
    }
}
