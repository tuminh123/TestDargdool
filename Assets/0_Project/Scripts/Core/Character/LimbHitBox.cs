using UnityEngine;

public class LimbHitBox : MonoBehaviour
{
    [SerializeField] float damageScale = 1f; // đầu = 2, tay = 1, chân = 0.7
    private CharacterParent parent;
    private void Awake()
    {
        parent = GetComponentInParent<CharacterParent>();
    }
    public void ReceiveHit(float rawDamage, Vector2 force)
    {
        if (parent == null) return;

        float finalDamage = DamageCaculate(rawDamage);

        parent.healthBase.TakeDamaged(finalDamage);

        parent.ragdollController.OnHit(force, rawDamage);
    }

    private float DamageCaculate(float rawDamage)
    {
        if (parent == null || parent.Stats == null) return 0;

        float baseDamage = rawDamage * parent.Stats.DamageBase;

        bool isCrit = Random.value < parent.Stats.CritChane;
        if (isCrit)
        {
            baseDamage *= parent.Stats.CritMultiplier;
        }
        float finalDamage = baseDamage * damageScale;
        return finalDamage;
    }
}
