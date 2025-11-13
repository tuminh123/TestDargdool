using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : ActionBase
{
    [Header("Attack Settings")]
    [SerializeField] private float attackForce = 150f;
    [SerializeField] private float upForceRatio = 0.3f;
    [SerializeField] private float recoilForceRatio = 0.3f;
    [SerializeField] private float gravityScaleMultiplier = 0.5f;
    [SerializeField] private float gravityDuration = 0.3f;

    [Header("Ragdoll Parts")]
    [SerializeField] private RightArmBalance rightArmBalance;
    [SerializeField] private RightElbowBalance rightElbowBalance;
    [SerializeField] private RightHandBalance rightHandBalance;

    private BodyBalance body;
    private Vector2 attackDir;
    private bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponentInChildren<BodyBalance>();
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        attackDir = (mousePos - body.transform.position).normalized;

        Debug.DrawLine(body.transform.position, mousePos, Color.cyan);
    }

    public void AttackHandle()
    {
        if (isAttacking) return;
        isAttacking = true;

        // Vô hiệu hóa các phần thân không cần thiết
        foreach (BalanceAbstract b in characterCtrl.Balances)
        {
            if (b == rightArmBalance || b == rightElbowBalance || b == rightHandBalance) continue;
            b.SetIsActive(false);
        }

        // Tính hướng lực có nâng lên
        Vector2 impulseDir = attackDir + Vector2.up * upForceRatio;

        // Apply lực tấn công cho các phần tay
        rightHandBalance.ApplyImpulse(impulseDir, attackForce);
        rightElbowBalance.ApplyImpulse(impulseDir, attackForce * 0.6f);
        rightArmBalance.ApplyImpulse(impulseDir, attackForce * 0.4f);

        // Giảm trọng lực tạm thời để tạo cảm giác “bay bồng bềnh”
        rightHandBalance.ModifyGravityTemporarily(gravityScaleMultiplier, gravityDuration);
        rightElbowBalance.ModifyGravityTemporarily(gravityScaleMultiplier, gravityDuration);
        rightArmBalance.ModifyGravityTemporarily(gravityScaleMultiplier, gravityDuration);

        // Thêm phản lực bật ngược cho thân
        if (body != null)
        {
            body.ApplyImpulse(-attackDir, attackForce * recoilForceRatio);
        }

        StartCoroutine(EndAttackAfter(characterCtrl.TimeDelay));
    }

    private IEnumerator EndAttackAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;

        // Bật lại toàn bộ ragdoll
        foreach (BalanceAbstract b in characterCtrl.Balances)
        {
            b.LerpToActive(0.2f); // bật dần cho mềm mại
        }

        characterCtrl.ChangeState(state.idle);
    }
}
