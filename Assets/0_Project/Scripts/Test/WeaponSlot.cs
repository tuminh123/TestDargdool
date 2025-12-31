using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var equip = col.GetComponentInParent<WeaponEquipController2D>();
        if (equip == null) return;

        var weapon = GetComponent<Weapon2D>();
        equip.EquipWeapon(weapon);

        GetComponent<Collider2D>().enabled = false;
    }
}
