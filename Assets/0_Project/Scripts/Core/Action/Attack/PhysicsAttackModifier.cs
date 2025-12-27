using UnityEngine;
using System.Collections.Generic;

public class PhysicsAttackModifier : MonoBehaviour
{
    [Header("Attack Physics Buff")]
    public float massMultiplier = 1.5f;
    public float gravityMultiplier = 2f;
    public float impulseForce = 8f;

    private Rigidbody2D[] bodies;
    private Dictionary<Rigidbody2D, float> originMass = new();
    private Dictionary<Rigidbody2D, float> originGravity = new();

    private void Awake()
    {
        bodies = GetComponentsInChildren<Rigidbody2D>();

        foreach (var rb in bodies)
        {
            originMass[rb] = rb.mass;
            originGravity[rb] = rb.gravityScale;
        }
    }

    public void EnableAttackPhysics(Vector2 dir)
    {
        foreach (var rb in bodies)
        {
            rb.mass = originMass[rb] * massMultiplier;
            rb.gravityScale = originGravity[rb] * gravityMultiplier;

            rb.AddForce(dir * impulseForce, ForceMode2D.Impulse);
        }
    }

    public void ResetPhysics()
    {
        foreach (var rb in bodies)
        {
            rb.mass = originMass[rb];
            rb.gravityScale = originGravity[rb];
        }
    }
}
