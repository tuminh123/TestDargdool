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
        InputManager.OnTap += HandleTap;
        //SwipeManager.OnSwipe += HandleSwipe;
    }

    public virtual void Exit()
    {
        InputManager.OnTap -= HandleTap;
        //SwipeManager.OnSwipe -= HandleSwipe;
        //characterCtrl.moveVer2.SetMoveDirection(Vector2.zero);
    }

    private void HandleTap(Vector2 pos)
    {
        if (characterCtrl.healthBase.IsDead) return;

        if (characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.stunnedState);
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

        if (characterCtrl.weaponEquip.IsEquipping)
        {
            stateMachine.ChangeState(characterCtrl.weaponAttackState);
        }
        else if (characterCtrl.attack.CanAttack())
        {
            stateMachine.ChangeState(characterCtrl.attackState);
        }
    }

    //private void HandleSwipe(Vector2 delta)
    //{
    //    if (characterCtrl.healthBase.IsDead || characterCtrl.IsStunned) return;

    //    if (delta.magnitude < 30f) return; // tránh swipe quá nhỏ

    //    Vector2 dir = delta.normalized;

    //    if (dir.y > 0.6f && characterCtrl.groundDetect.IsGround())
    //    {
    //        stateMachine.ChangeState(characterCtrl.jumpState);
    //        return;
    //    }

    //    // chỉ lấy hướng ngang
    //    Vector2 move = new Vector2(dir.x, 0);

    //    characterCtrl.moveVer2.SetMoveDirection(move);
    //    stateMachine.ChangeState(characterCtrl.moveState);
    //}

    public virtual void Update()
    {
        if (characterCtrl.healthBase.IsDead) return;

        if (characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.stunnedState);
        }

        x = VirtualJoystick.GetAxis(StringConst.HORIZONTAL);
        float y = VirtualJoystick.GetAxis(StringConst.VERTICAL);



        isGround = characterCtrl.groundDetect.IsGround();

        if (isGround)
        {
            airTapCount = 0;
        }

        /*if (SwipeManager.Tap && characterCtrl.attack.CanAttack() && !characterCtrl.IsStunned)
        {
            stateMachine.ChangeState(characterCtrl.attackState);
        }
        else*/
        if (y > 0 && isGround)
        {
            stateMachine.ChangeState(characterCtrl.jumpState);
        }
    }

    public virtual void UpdatePhysic()
    {
    }
}