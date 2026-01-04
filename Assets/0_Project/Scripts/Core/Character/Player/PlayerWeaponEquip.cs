using UnityEngine;
using UnityEngine.XR;

public enum EquipState
{
    Idle,      // Không cầm gì
    Pulling,   // Đang hút/kéo vũ khí về tay
    Holding    // Đã cầm vũ khí
}
public class PlayerWeaponEquip : MonoBehaviour
{
    public event System.Action<WeaponBase> OnEquip;
    public event System.Action OnDrop;
    public event System.Action<HandController> OnHand;

    [Header("Hands")]
    [SerializeField] HandController leftHand;
    [SerializeField] HandController rightHand;
    [Header("Pull Params")]
    [SerializeField] float grabForce = 2000f; 
    [SerializeField] float grabDistance = 0.05f; 
    EquipState state = EquipState.Idle; 
    HandController activeHand; 
    TargetJoint2D grabJoint;
    public WeaponBase currentWeapon { get; private set; }

    public bool HasWeapon => state == EquipState.Holding;

    void FixedUpdate() 
    {
        if (state == EquipState.Pulling) UpdatePulling(); 
    }

    private void OnTriggerEnter2D(Collider2D collision) { 
        if (state != EquipState.Idle) return; 

        if (!collision.TryGetComponent(out WeaponBase weapon)) return;

        BeginEquip(weapon);

     /*   currentWeapon = weapon;
        HandController currentHand = GetHand();

        if (currentHand == null) return;
        weapon.MoveToHand(currentHand);
        weapon.Equipping(currentHand.Rb);

        HasWeapon = true;

        SetRotWhenEquip(currentHand);*/

    }

    #region Equip Flow

    private void BeginEquip(WeaponBase weapon)
    {
        state = EquipState.Pulling; 
        currentWeapon = weapon; 
        activeHand = ChooseHand(); 
        OnHand?.Invoke(activeHand); 
        grabJoint = weapon.gameObject.AddComponent<TargetJoint2D>(); 
        grabJoint.autoConfigureTarget = false; 
        grabJoint.target = activeHand.transform.position; 
        grabJoint.maxForce = grabForce;
        grabJoint.frequency = 10f;
        grabJoint.dampingRatio = 1f;
    }
    private void UpdatePulling()
    {
        if (!currentWeapon || !activeHand) return;
        grabJoint.target = activeHand.transform.position;
        float dist = Vector2.Distance(currentWeapon.rb.position, activeHand.transform.position);
        if (dist <= grabDistance) CompleteEquip();
    }

    private void CompleteEquip()
    {
        Destroy(grabJoint);

        state = EquipState.Holding;
        activeHand.EquipWeapon(currentWeapon);
        //SetRotWhenEquip(activeHand);
        //currentWeapon.Equipping(activeHand.Rb);
        OnEquip?.Invoke(currentWeapon);
    }
    private HandController ChooseHand()
    {
        if (!leftHand.IsHolding) return leftHand; 
        if (!rightHand.IsHolding) return rightHand; 
        return Random.value > 0.5f ? leftHand : rightHand;
    }
    #endregion

    public void UnEquipWeapon()
    {
        if (currentWeapon == null) return;
        //HasWeapon = false;
        currentWeapon.ResetWeapon();
    }

    #region Weapon Rot Handle
    public void SetRotWhenEquip(HandController hand)
    {
        if(hand == null || currentWeapon == null || hand.HandType == HandType.None) return;
        if (hand.HandType == HandType.Left) currentWeapon.transform.localScale = new Vector3(-1, 1, 1);

        if (hand.HandType == HandType.Right) currentWeapon.transform.localScale = new Vector3(1, 1, 1);
    }
    public void SetRotWhenAttack(Vector2 dir)
    {
        if ( currentWeapon == null) return;
        if (dir.x < 0) currentWeapon.transform.localScale = new Vector3(-1, 1, 1);

        if (dir.x > 0) currentWeapon.transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion
    public HandController GetHand()
    {
        HandController[] hands = new HandController[] { leftHand, rightHand };
        return hands[Random.Range(0, hands.Length)];

    }
}
