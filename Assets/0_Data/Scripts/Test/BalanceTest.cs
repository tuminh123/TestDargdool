using UnityEngine;

public class BalanceTest : MonoBehaviour
{
    [Header("Target Rotation")]
    [SerializeField] private float targetRot = 0f;

    [Header("Stability Settings")]
    [SerializeField] private float stiff = 30f;   // lực kéo về (độ cứng)
    [SerializeField] private float damp = 6f;     // giảm rung

    [Header("Rotation Limit (Optional)")]
    public bool useClamp = false;
    public float minRot = -180f;
    public float maxRot = 180f;

    [Header("Reset Data (Idle Pose)")]
    public float defaultRot = 0f;
    public float defaultStiff = 30f;
    public float defaultDamp = 6f;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        StabilizeRotation(targetRot, stiff, damp);
    }


    // ======================
    //   PD CONTROLLER
    // ======================
    public void StabilizeRotation(float target, float stiff, float damp)
    {
        float currentRot = rb.rotation;

        // Luôn chọn đường xoay NGẮN NHẤT → KHÔNG xoay vòng tròn
        float delta = Mathf.DeltaAngle(currentRot, target);

        // Lực đàn hồi kéo về target
        float torque = delta * stiff;

        // Giảm rung
        torque -= rb.angularVelocity * damp;

        rb.AddTorque(torque);
    }


    // ======================
    //   PUBLIC API
    // ======================
    public void SetRotation(float rot)
    {
        if (useClamp)
            targetRot = Mathf.Clamp(rot, minRot, maxRot);
        else
            targetRot = rot;
    }

    public void SetStiff(float value) => stiff = value;
    public void SetDamp(float value) => damp = value;


    public void ResetData()
    {
        targetRot = defaultRot;
        stiff = defaultStiff;
        damp = defaultDamp;
    }
}
