using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField] private Balance rightLeg, leftLeg, body;

    [Header("Settings")]
    [SerializeField] float stepDuration = 0.4f;
    [SerializeField] float moveDistance = 0.25f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float maxSpeed = 5f;

    [Header("Curves")]
    [SerializeField] private AnimationCurve stepCurve;      // 0→1→0
    [SerializeField] private AnimationCurve rotationCurve;  // 0→1→0

    private float moveInput;     // -1 → trái, +1 → phải
    private float stepTime = 0f; // 0→1 của animation curve

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        MoveHandle(moveInput);
        
        HandleStepTimer();
        HandleLegAnimation();
    }

    private void FixedUpdate()
    {
        ApplyStepMovement();
        LimitVelocity(body.Rb);
        LimitVelocity(leftLeg.Rb);
        LimitVelocity(rightLeg.Rb);
    }

    // =======================
    //       INPUT
    // =======================
    public void MoveHandle(float x)
    {
        moveInput = x; // -1, 0, 1
        if (x == 0) stepTime = 0f;
    }

    // =======================
    //   STEP PHASE (0 → 1)
    // =======================
    void HandleStepTimer()
    {
        if (moveInput != 0)
        {
            stepTime += Time.deltaTime / stepDuration;

            if (stepTime > 1f)
                stepTime = 0f;          // bắt đầu bước tiếp theo
        }
        else
        {
            stepTime = 0f;
        }
    }

    // =======================
    //     LEG ANIMATION
    // =======================
    void HandleLegAnimation()
    {
        float rotValue = rotationCurve.Evaluate(stepTime); // 0→1→0

        if (moveInput > 0) // đi phải
        {
            rightLeg.SetTargetRotation(Mathf.Lerp(0, 25, rotValue));
            leftLeg.SetTargetRotation(Mathf.Lerp(0, 80, rotValue));
        }
        else if (moveInput < 0) // đi trái
        {
            rightLeg.SetTargetRotation(Mathf.Lerp(0, -80, rotValue));
            leftLeg.SetTargetRotation(Mathf.Lerp(0, -25, rotValue));
        }
    }

    // =======================
    //    APPLY MOVEMENT
    // =======================
    void ApplyStepMovement()
    {
        if (moveInput == 0) return;

        float stepValue = stepCurve.Evaluate(stepTime); // 0→1→0

        Vector2 move = new Vector2(
            moveInput * moveDistance * stepValue,
            0
        );

        body.Rb.AddForce(move, ForceMode2D.Force);
    }

    // =======================
    //  LIMIT VELOCITY
    // =======================
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(
                Mathf.Sign(rb.linearVelocityX) * maxSpeed,
                rb.linearVelocityY
            );
        }
    }
}
