using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class MainAttackState : MainCharacterState
{
    private IPostAction postAction;
    private VfxBase vfx = null;

    public MainAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
        postAction = new SmoothPostAction(characterCtrl?.ragdollController?.ActionsDataSO, characterCtrl?.ragdollController?.Balances, characterCtrl?.attack?.ConfigSO);
    }

    public override void Enter()
    {

        base.Enter();

        string name = characterCtrl.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;
        characterCtrl.attackContext.EnableAttack();
        characterCtrl?.ragdollController?.postContext.SetPostAction(postAction);

        GameObject hand = characterCtrl.AttackDir.x > 0 ? characterCtrl.RightHand : characterCtrl.LeftHand; 
        characterCtrl.EffectSpawns(hand,out vfx);
        characterCtrl?.attack?.attackContext?.ExecuteAttack(characterCtrl.AttackDir,characterCtrl?.ragdollController?.actionBase,name,characterCtrl.gameObject);
        characterCtrl.SendDamageBase();

        characterCtrl.attack.currentAttack.OnAttackEnd += EndAttack;

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attack.currentAttack.OnAttackEnd -= EndAttack;

    }

    private void EndAttack()
    {
        //Debug.Log("End");
        characterCtrl?.attack?.attackContext?.CancelAttack();
        if (vfx != null)
        {
            characterCtrl.EffectDeSpawns(vfx);
        }
        characterCtrl.attackContext.DisableAttack();
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}