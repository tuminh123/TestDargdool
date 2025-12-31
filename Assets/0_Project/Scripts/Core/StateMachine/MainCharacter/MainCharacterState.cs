using System.Collections;
using Terresquall;
using UnityEngine;

public class MainCharacterState : IState
{
    protected StateMachine stateMachine;
    protected CharacterCtrl characterCtrl;
    protected float x;
    protected bool isGround;

    private int airTapCount = 0;       // Đếm số lần tap khi đang trên không

    public MainCharacterState(StateMachine stateMachine, CharacterCtrl characterCtrl)
    {
        this.stateMachine = stateMachine;
        this.characterCtrl = characterCtrl;
    }

    public virtual void Enter()
    {
        //InputManager.OnTap += HandleTap;
        SwipeManager.OnTap += HandleTap;
    }

    public virtual void Exit()
    {
        //InputManager.OnTap -= HandleTap;
        SwipeManager.OnTap -= HandleTap;
        //characterCtrl.moveVer2.SetMoveDirection(Vector2.zero);
    }

    private void HandleTap(Vector2 pos)
    {
        if (characterCtrl.healthBase.IsDead) return;

        if (characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.stunnedState);
            return;
        }

        isGround = characterCtrl.groundDetect.IsGround();

        if (!isGround)
        {
           /* if (airTapCount >= 3)
            {
                Debug.Log("Air tap limit reached!");
                return;
            }
*/
            airTapCount++;
            Debug.Log("Air tap count: " + airTapCount);
        }

        characterCtrl.SetAttackDirection(pos);

       /* if (characterCtrl.weaponEquip.IsEquipping)
        {
            stateMachine.ChangeState(characterCtrl.weaponAttackState);
        }
        else*/ if (characterCtrl.attack.CanAttack())
        {
            stateMachine.ChangeState(characterCtrl.attackState);
        }
    }

    public virtual void Update()
    {
        if (characterCtrl.healthBase.IsDead) return;

        if (characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.stunnedState);
            return;
        }

        /*  x = VirtualJoystick.GetAxis(StringConst.HORIZONTAL);
          float y = VirtualJoystick.GetAxis(StringConst.VERTICAL);*/

        x = SwipeManagerTest.MoveDirection;

        if (characterCtrl.groundDetect != null)
        {
            isGround = characterCtrl.groundDetect.IsGround();
        }
        else
        {
            return;
        }

        if (isGround)
        {
            airTapCount = 0;
        }

        /*if (SwipeManager.Tap && characterCtrl.attack.CanAttack() && !characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.attackState);
        }
        else*/
        if (/*y > 0*/ SwipeManagerTest.SwipeUp && isGround)
        {
            stateMachine.ChangeState(characterCtrl.jumpState);
        }
    }

    public virtual void UpdatePhysic()
    {
    }
}