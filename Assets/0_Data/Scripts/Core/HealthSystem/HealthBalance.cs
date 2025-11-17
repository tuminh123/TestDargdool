using System.Collections;
using UnityEngine;

public class HealthBalance : MonoBehaviour
{
    public System.Action<float> OnDamage;

    [SerializeField] private float damageTaken = 10;
    [SerializeField] private float forceKnockBack = -70;
    private HealthBase healthBase;
    private Balance balance;
    private void Awake()
    {
        healthBase = transform.parent.GetComponent<HealthBase>();
        balance = GetComponent<Balance>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider == null) return;
        IDamageFaction dam = collision.transform.GetComponent<IDamageFaction>();
        if(dam == null) return;
        if(healthBase == null) return;
        if (healthBase.Faction == dam.GetFaction) return;

        StartCoroutine(SetBalanceTrigger(collision));
        OnDamage?.Invoke(damageTaken);
    }
    private IEnumerator SetBalanceTrigger(Collision2D collision)
    {
        Vector2 dir = (collision.transform.position - transform.position).normalized;
        balance.SetIsTrigger(false);
        balance.Rb.AddForce(dir * (forceKnockBack*1000)*Time.fixedDeltaTime);
        yield return new WaitForSeconds(10);
        balance.SetIsTrigger(true);
    }
}
