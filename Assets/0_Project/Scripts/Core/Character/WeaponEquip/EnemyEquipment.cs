using UnityEngine;

public class EnemyEquipment : EquipmentBase
{
    public override void SetAbstractWeaponWhenEquip()
    {
        currentWeapon?.DamageDealer?.SetFaction(Faction.Enemy);

    }
}
