
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Attacking : ActionBase
{
    [SerializeField] private float attackForce = 150f;
    [SerializeField] private List<BalanceAbstract> balanceList  = new List<BalanceAbstract>();

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
        Vector3 camDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        camDir.z = 0;
        attackDir = (camDir - body.transform.position).normalized;
        // Debug hướng
        Debug.DrawLine(body.transform.position, camDir, Color.blue);

    }
    public void AttackHandle()
    {
        if (isAttacking) return;
        isAttacking = true;

        BalanceAbstract temp = GetBalance();
        isAttacking = true;

        //BalanceAbstract temp = balanceList[2];
        foreach (BalanceAbstract b in characterCtrl.Balances)
        {
            if (b==temp || b== body) continue;  // body/hand không bị tắt
            b.SetIsActive(false);            // Ngắt điều khiển
        }
        Debug.Log(temp);
        HandleAttackForce(temp);


        StartCoroutine(EndAttackAfter(characterCtrl.TimeDelay));

    }
    private  IEnumerator EndAttackAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
        // Bật lại cân bằng
        foreach (BalanceAbstract b in characterCtrl.Balances)
        {
            StartCoroutine(Set(b));
        }
        characterCtrl.ChangeState(state.idle);
    }
    private void HandleAttackForce(BalanceAbstract attackBalance)
    {
        //rightArm.Rb.AddForce(attackDir * (attackForce * 1000) * Time.fixedDeltaTime);
        //body.Rb.AddForce(attackDir * (attackForce/2 * 1000) * Time.fixedDeltaTime);
        attackBalance.Rb.AddForce(attackDir * attackForce, ForceMode2D.Impulse);
        body.Rb.AddForce(attackDir * (attackForce * 0.5f), ForceMode2D.Impulse);
    }
    private BalanceAbstract GetBalance()
    {
        int index = Random.Range(0,balanceList.Count);
        return balanceList[index];
    }
    private IEnumerator Set(BalanceAbstract balance)
    {
        yield return new WaitForSeconds(0.5f);
        balance.SetIsActive(true);
    }
}   
