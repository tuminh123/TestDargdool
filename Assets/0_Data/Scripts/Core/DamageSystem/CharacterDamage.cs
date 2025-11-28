using UnityEngine;

public class CharacterDamage : DamageBase
{
    private CharacterParent characterParent;

    private void Awake()
    {
        characterParent = GetComponentInParent<CharacterParent>();

        damageBase = characterParent.Stats.DamageBase;
    }
}
