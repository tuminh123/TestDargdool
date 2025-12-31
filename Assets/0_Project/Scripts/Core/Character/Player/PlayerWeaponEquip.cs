using UnityEngine;
public enum EquipState
{
    Idle,
    Pulling,
    Holding
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

    WeaponBase currentWeapon;
    HandController activeHand;
    TargetJoint2D grabJoint;

    #region Unity

    void FixedUpdate()
    {
        if (state == EquipState.Pulling)
            UpdatePulling();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (state != EquipState.Idle) return;

        if (col.TryGetComponent(out WeaponBase weapon))
            BeginEquip(weapon);
    }

    #endregion

    #region Equip Flow

    void BeginEquip(WeaponBase weapon)
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

    void UpdatePulling()
    {
        if (!currentWeapon || !activeHand) return;

        grabJoint.target = activeHand.transform.position;

        float dist = Vector2.Distance(
            currentWeapon.rb.position,
            activeHand.transform.position
        );

        if (dist <= grabDistance)
            CompleteEquip();
    }

    void CompleteEquip()
    {
        Destroy(grabJoint);

        activeHand.AttachWeapon(currentWeapon);

        state = EquipState.Holding;
        OnEquip?.Invoke(currentWeapon);
    }

    public void DropWeapon()
    {
        if (state != EquipState.Holding) return;

        activeHand.DetachWeapon();

        currentWeapon = null;
        activeHand = null;

        state = EquipState.Idle;
        OnDrop?.Invoke();
    }

    #endregion

    #region Helpers

    public HandController ChooseHand()
    {
        if (!leftHand.IsHolding) return leftHand;
        if (!rightHand.IsHolding) return rightHand;

        return Random.value > 0.5f ? leftHand : rightHand;
    }

    #endregion
}
