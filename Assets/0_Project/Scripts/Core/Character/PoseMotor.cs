using UnityEngine;

/// <summary>
/// PoseMotor giữ tư thế khi IDLE / MOVE
/// TẮT khi ATTACK
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PoseMotor : MonoBehaviour
{
    [SerializeField] private float targetRotation;
    [SerializeField] private float poseForce = 15f;
    [SerializeField] private float minRot = -180;
    [SerializeField] private float maxRot = 180;

    private Rigidbody2D rb;
    private bool active = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!active) return;

        float clamped = Mathf.Clamp(targetRotation, minRot, maxRot);
        float newRot = Mathf.LerpAngle( rb.rotation,clamped,poseForce * Time.fixedDeltaTime);

        rb.MoveRotation(newRot);
    }

    public void Enable() => active = true;
    public void Disable() => active = false;

    public void SetTargetRotation(float rot)
    {
        targetRotation = rot;
    }
}
