using UnityEngine;

public abstract class PotionBase : ItemBase
{
    [SerializeField] protected float amount = 30;

    protected IEffect effect;

    public void SetEffect(IEffect effect)
    {
        this.effect = effect;
    }

    public void Use(GameObject target)
    {
        effect.Apply(target);
    }
}
