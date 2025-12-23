using UnityEngine;

public class MainStunState : MainCharacterState
{
    private float stunTime;
    private float time;

    public MainStunState(StateMachine stateMachine, CharacterCtrl characterCtrl,float stunTime) : base(stateMachine, characterCtrl)
    {
        this.stunTime = stunTime;
    }

    public override void Enter()
    {
        base.Enter();
        time = stunTime;
        characterCtrl.SetTriggerBalance(false);
        characterCtrl.SetKnockBackBalance();
        
        
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;

        if (time <= 0)
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
