using Cysharp.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour
{
    [SerializeField] private GameObject weaponArmRight, weaponArmLeft;
    [SerializeField] private bool isEquipping = false;

    public WeaponBase currentWeapon { get; private set; }
    private CharacterCtrl ctrl;
    //get
    public bool IsEquipping => isEquipping;

    private void Awake()
    {
        ctrl = GetComponentInParent<CharacterCtrl>();
    }
    /* private void FixedUpdate()
     {
         if (currentWeapon == null || !currentWeapon.IsEquip) return;
         currentWeapon.RotateByDirection(ctrl.AttackDir);
         isEquipping = currentWeapon.IsEquip;
     }*/
    private void EquipHandle(WeaponBase weapon)
    {
        GameObject handObj = GetHandBalance();
        if (handObj == null) return;
        Rigidbody2D hand = handObj.GetComponent<Rigidbody2D>(); 

        float flipDir = (hand == weaponArmLeft) ? -1f : 1f;
        Vector3 pos = new Vector3(0.1f, -0.4f, 0);
        weapon.EquipWeapon(hand,hand.transform, flipDir, pos);

        isEquipping = true;
        currentWeapon = weapon;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.transform.TryGetComponent(out WeaponBase weapon))
        {
            if (isEquipping) return;
            EquipHandle(weapon);
        }
    }

    public void ResetEquip()
    {
        if (currentWeapon == null) return;
        isEquipping = false;
        currentWeapon.UnEquipWeapon();

    }
    public void SetIsEquip(bool isEquip)
    {
        this.isEquipping = isEquip;
    }
    public GameObject GetHandBalance()
    {
        GameObject[] randHand = new GameObject[] { weaponArmLeft, weaponArmRight };
        int index = Random.Range(0, randHand.Length);
        return randHand[index];
    }
}
