using UnityEngine;

public class LimbHitBox : MonoBehaviour
{
    [SerializeField] float damageScale = 1f;
    [SerializeField] private CharacterParent parent;

    //get
    public CharacterParent Parent => parent;
    private void Awake()
    {
        if (parent == null)
        {
            parent = transform.GetComponentInParent<CharacterParent>();
        }
    }
    private void Reset()
    {
        parent = transform.GetComponentInParent<CharacterParent>();
    }
    public void ReceiveHit(float rawDamage, Vector2 force)
    {
        if (parent == null) return;

        float finalDamage = DamageCaculate(rawDamage);

        Debug.Log(finalDamage);
        parent.healthBase.TakeDamaged(finalDamage);

        parent.ragdollController.OnHit(force, rawDamage);
    }

    private float DamageCaculate(float rawDamage)
    {
        if (parent == null || parent.Stats == null) return 0;

        float baseDamage = rawDamage * parent.Stats.DamageBase;

        Debug.Log(parent.Stats.DamageBase);
        Debug.Log(baseDamage);
        bool isCrit = Random.value < parent.Stats.CritChane;
        Debug.Log(isCrit);
        if (isCrit)
        {
            baseDamage *= parent.Stats.CritMultiplier;
        }
        float finalDamage = baseDamage * damageScale;
        return finalDamage;
    }
}
