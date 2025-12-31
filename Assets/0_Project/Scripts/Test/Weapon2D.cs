using UnityEngine;

[System.Serializable]
public class WeaponData2D
{
    [Header("Physics")]
    public float mass = 1f;

    [Header("Hinge Limits")]
    public float minAngle = -70f;
    public float maxAngle = 70f;

    [Header("Motor (Attack)")]
    public float attackMotorSpeed = 500f;
    public float attackMotorTorque = 800f;

    [Header("Motor (Weak / Hit)")]
    public float weakMotorSpeed = 150f;
    public float weakMotorTorque = 200f;

    [Header("Damage")]
    public float baseDamage = 5f;
    public float velocityMultiplier = 1.2f;

    [Header("Drop")]
    public float dropForceThreshold = 8f;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon2D : MonoBehaviour
{
    public WeaponData2D data;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public HingeJoint2D joint;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void ApplyData()
    {
        rb.mass = data.mass;
    }
}
