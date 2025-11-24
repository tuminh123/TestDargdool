using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class MoveTest : MonoBehaviour
{
    private HealthBase healthBase;
    private Coroutine damageCoroutine;

    public Transform target;

    private void Awake()
    {
        healthBase = GetComponentInChildren<HealthBase>();
    }

    #region Health event
    private void OnEnable()
    {
        if (healthBase == null) return;
        //healthBase.OnDead += OnEnmyDead;
        healthBase.OnTakeDamage += OnTakeDamage;
    }

    private void OnDisable()
    {
        if (healthBase == null) return;
        //healthBase.OnDead -= OnEnmyDead;
        healthBase.OnTakeDamage += OnTakeDamage;

        StopDamageCoroutine();
    }
    private void OnDestroy()
    {
        StopDamageCoroutine();
    }
    //private void OnEnmyDead()
    //{
    //    EnemyObjectPool.Instance.DeSpawn(this);
    //}
    private void OnTakeDamage()
    {
        Balance body = transform.GetComponent<Balance>();
        Balance[] childBalance = transform.GetComponentsInChildren<Balance>();

        Vector2 dir = transform.position - target.position;
        foreach (var item in childBalance)
        {
            item.Rb.linearVelocity = dir * 3;

        }
        body.Rb.linearVelocity = dir * 3;

        damageCoroutine = StartCoroutine(SetBalanceTrigger(body, childBalance));

    }
    private IEnumerator SetBalanceTrigger(Balance body, Balance[] childBalance)
    {
        

        foreach (var item in childBalance)
        {
            item.SetIsTrigger(false);
           
        }
        body.SetIsTrigger(false);

        yield return new WaitForSeconds(2);

        foreach (var item in childBalance)
        {
            item.SetIsTrigger(true);
        }
        body.SetIsTrigger(true);
    }
    public void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    #endregion
}
