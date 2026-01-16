
using UnityEngine;

public class PlayerWeaponEquip : EquipmentBase
{
    public override void SetAbstractWeaponWhenEquip()
    {
        currentWeapon?.DamageDealer?.SetFaction(Faction.Ally);
    }
}
