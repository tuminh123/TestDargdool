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
        x = Input.GetAxisRaw("Horizontal");
        isGround = characterCtrl.groundDetect.IsGround();
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attack");
            stateMachine.ChangeState(characterCtrl.attackState);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            stateMachine.ChangeState(characterCtrl.jumpState);
        }
    }
}