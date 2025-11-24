using System.Collections;
using UnityEngine;

public class MainCharacterState : IState
{
    protected StateMachine stateMachine;
    protected CharacterCtrl characterCtrl;
    protected float x;
    protected bool isGround;

    public MainCharacterState(StateMachine stateMachine, CharacterCtrl characterCtrl)
    {
        this.stateMachine = stateMachine;
        this.characterCtrl = characterCtrl;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
        if (characterCtrl.healthBase.IsDead) return;
        if (characterCtrl.IsStunned) return;

        x = Input.GetAxisRaw("Horizontal");
        isGround = characterCtrl.groundDetect.IsGround();
        
        if (Input.GetMouseButtonDown(0) && characterCtrl.attack.CanAttack() && characterCtrl.IsStunned == false)
        {
            stateMachine.ChangeState(characterCtrl.attackState);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            stateMachine.ChangeState(characterCtrl.jumpState);
        }
    }
}