using UnityEngine;

public class character_attack : character_state_base
{
    public character_attack(character_test test, StateMachine stateMachine, string stateName, action_type type) : base(test, stateMachine, stateName, type)
    {
        balanceParts = new[]
         {
            BalanceType.right_arm,
            BalanceType.right_forearm,
            BalanceType.right_hand,
            BalanceType.body_bottom,
            BalanceType.left_leg,
            BalanceType.left_lower_leg,
            BalanceType.right_leg,
            BalanceType.right_lower_leg
        };
    }

    public override void Enter()
    {
        time = 1;
        base.Enter();
        
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        test.SetTrigget(false);

        test.SetVelocity();

    }
    public override void Update()
    {
        base.Update();
        if (time < 0)
        {
            stateMachine.ChangeState(test.idle_state);
        }
    }
}
