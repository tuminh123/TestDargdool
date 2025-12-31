using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{

    public WeaponEquipController2D equip;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            equip.SetMode(EquipMode2D.StrongHinge);

        if (Input.GetMouseButtonUp(0))
            equip.SetMode(EquipMode2D.HardParent);

        if (Input.GetKeyDown(KeyCode.G))
            equip.DropWeapon();
    }
}