using UnityEngine;

public class MainStunState : MainCharacterState
{
    private float stunTime;

    public MainStunState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stunTime = 2;
        characterCtrl.SetTriggerBalance(false);
        characterCtrl.SetKnockBackBalance();
    }
    public override void Update()
    {
        base.Update();
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        characterCtrl.SetIsStunned(false);
        characterCtrl.SetTriggerBalance(true);
    }
}
