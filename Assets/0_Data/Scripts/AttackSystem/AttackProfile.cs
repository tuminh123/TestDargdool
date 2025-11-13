
using UnityEngine;

[CreateAssetMenu(fileName = "AttackProfile", menuName = "Attack/AttackProfile", order = 0)]
public class AttackProfile : ScriptableObject
{
    public enum Category { Arm, Elbow, Leg, Pillow }
    public enum Side { Right, Left, Any }

    [Header("Identity")]
    public string displayName;
    public Category category = Category.Arm;
    public Side side = Side.Any;

    [Header("Timing & Feel")]
    public float attackDuration = 0.25f;
    public float stiffness = 50f;
    public float damping = 0.9f;

    [Header("Angles (degrees)")]
    public float armExt = 115f;
    public float elbowExt = 85f;
    public float handExt = 45f;
    public float armRetract = 45f;
    public float elbowRetract = -30f;

    public float legExt = 40f;
    public float footExt = 25f;
    public float legRetract = 90f;
    public float footRetract = 60f;

    public float pillowLegExt = 30f;
    public float pillowPillowExt = 30f;
    public float pillowLegRetract = 100f;
    public float pillowPillowRetract = 80f;

    [Header("Impulse / Force")]
    public float armImpulse = 100f;
    public float handImpulse = 100f;
    public float legImpulse = 150f;
    public float pillowImpulse = 120f;
    public float bodyImpulse = 70f;
}
