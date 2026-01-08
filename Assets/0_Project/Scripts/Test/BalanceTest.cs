using UnityEngine;

public class BalanceTest : MonoBehaviour
{
    [Header("Pose")]
    public float targetRotation;
    public float poseSpeed = 50f;

    [SerializeField] private float minRot = -180f;
    [SerializeField] private float maxRot = 180f;

    public Rigidbody2D rb { get;private set; }
    bool poseEnabled = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!poseEnabled) return;
        /*
                float clampedRot = Mathf.Clamp(targetRotation, minRot, maxRot);
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, clampedRot, poseSpeed * Time.fixedDeltaTime));*/

        rb.MoveRotation(Mathf.LerpAngle(rb.rotation,targetRotation,poseSpeed * Time.fixedDeltaTime));
    }

    public void EnablePose() => poseEnabled = true;
    public void DisablePose() => poseEnabled = false;
    public void SetRotation(float rot)
    {
        targetRotation = rot;
    }
}
