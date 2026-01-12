using UnityEngine;

[CreateAssetMenu(fileName = "Physics Attack Original Profile Data SO", menuName = "Data SO/Attack/ Physics Attack Original Profile")]
public class PhysicsAttackOriginalProfile : ScriptableObject
{
    [Header("Torque (luc xoay)")]
    [Tooltip("Luc torque tác dong lên tay")]
    [SerializeField] private float armTorque = 10f;
    [Tooltip("Luc torque tác dong lên v? khí")]
    [SerializeField] private float weaponTorque = 15f;

    [Header("Timing")]
    [Tooltip("Thoi gian áp luc (giây)")]
    [SerializeField] private float impulseDuration = 0.1f;

    [Header("Recovery")]
    [Header("Translation Force")]
    [SerializeField] private float pushForce = 2.5f; // luc lao toi

    [Header("Recovery")]
    [Tooltip("Angular damping sau khi dánh (giúp tay/vu khí cham dan)")]
    [SerializeField] private float recoveryAngularDamping = 6f;

    //get
    public float ArmTorque => armTorque;
    public float WeaponTorque => weaponTorque;
    public float ImpulseDuration => impulseDuration;
    public float PushForce => pushForce;
    public float RecoveryAngularDamping => recoveryAngularDamping;
}
