using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour
{
    public event System.Action<WeaponBase> OnEquip;

    [SerializeField] private Balance handRight, handLeft;
    [SerializeField] private bool isEquipping = false;

    private CharacterCtrl ctrl;
    //get
    public bool IsEquipping => isEquipping; 

    private void Awake()
    {
        ctrl = GetComponentInParent<CharacterCtrl>();
    }

    private void EquipHandle(WeaponBase weapon)
    {
        Balance hand = GetHandBalance();

        weapon.Equip(hand.Rb);

        //weapon.weaponDamage.SetTargetLayer(StringConst.ENEMY);

        RotationWeapon(weapon, hand);

        isEquipping = true;

        OnEquip?.Invoke(weapon);
    }

    private void RotationWeapon(WeaponBase weapon, Balance hand)
    {
        if (hand == handLeft)
        {
            WeaponSetup(weapon, hand, -1);
        }
        else if (hand == handRight)
        {
            WeaponSetup(weapon, hand, 1);
        }
    }

    private void WeaponSetup(WeaponBase weapon, Balance hand,float rot)
    {
        weapon.transform.localScale = new Vector3(rot, 1, 1);
        weapon.transform.position = (hand.transform.position+new Vector3(0.1f,-0.4f,0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.TryGetComponent(out WeaponBase weapon))
        {
            if(isEquipping) return;
            EquipHandle(weapon);
        }

    }
    public Balance GetHandBalance()
    {
        Balance[] randHand = new Balance[] { handLeft, handRight };
        int index = Random.Range(0, randHand.Length);
        return randHand[index];
    }
    public void SetIsEquipping(bool isEquipping)
    {
        this.isEquipping = isEquipping;
    }
    public void UnEquipping()
    {
        this.isEquipping=false;
    }
    public void Equipping()
    {
        this.isEquipping = true;
    }
}
