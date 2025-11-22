using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Leg Parts")]
    public Balance leftLeg;
    public Balance rightLeg;

    [Header("Body")]
    public Rigidbody2D bodyRb;

    [Header("Move Settings")]
    public float walkForce = 20f;
    public float maxSpeed = 5f;
    public float stepDuration = 0.22f;
    public float stopDamping = 12f;
    public float moveDamping = 1f;

    [Header("Standing Pose")]
    public float leftStandRot = 5f;
    public float rightStandRot = -5f;

    float stepTimer;
    int stepIndex;

    bool moveRight;
    bool moveLeft;

    // ------------------------------------------------------
    void FixedUpdate()
    {
        if (moveRight)
        {
            bodyRb.linearDamping = moveDamping;
            StepLogic(Vector2.right);
        }
        else if (moveLeft)
        {
            bodyRb.linearDamping = moveDamping;
            StepLogic(Vector2.left);
        }
        else
        {
            ApplyStopDamping();
            StandPose();
        }
    }

    // ------------------------------------------------------
    public void MoveRight()
    {
        moveRight = true;
        moveLeft = false;
    }

    public void MoveLeft()
    {
        moveLeft = true;
        moveRight = false;
    }

    public void StopMove()
    {
        moveRight = false;
        moveLeft = false;

        stepTimer = 0;
        stepIndex = 0;
    }

    // ------------------------------------------------------
    void StepLogic(Vector2 dir)
    {
        // hạn chế tốc độ
        if (Mathf.Abs(bodyRb.linearVelocity.x) < maxSpeed)
            bodyRb.AddForce(dir * walkForce, ForceMode2D.Force);

        // bước chân 2 pha
        stepTimer += Time.fixedDeltaTime;
        if (stepTimer >= stepDuration)
        {
            stepTimer = 0;
            stepIndex = 1 - stepIndex;  
            DoStep(dir, stepIndex);
        }
    }

    // ------------------------------------------------------
    void DoStep(Vector2 dir, int idx)
    {
        float forward = dir.x > 0 ? 1 : -1;

        if (idx == 0)
        {
            rightLeg.SetRotation(10 * forward);
            leftLeg.SetRotation(85 * forward);

            SmoothMotionHelper.SmoothMoveTowards(
                rightLeg.Rb,
                rightLeg.Rb.position + dir * 0.3f,
                maxSpeed
            );
        }
        else
        {
            rightLeg.SetRotation(85 * forward);
            leftLeg.SetRotation(10 * forward);

            SmoothMotionHelper.SmoothMoveTowards(
                leftLeg.Rb,
                leftLeg.Rb.position + dir * 0.3f,
                maxSpeed
            );
        }
    }

    // ------------------------------------------------------
    void ApplyStopDamping()
    {
        // tăng damping để dừng cực nhanh, không trượt
        bodyRb.linearDamping = stopDamping;
    }

    // ------------------------------------------------------
    void StandPose()
    {
        // trả chân về thế đứng tự nhiên
        leftLeg.SetRotation(leftStandRot);
        rightLeg.SetRotation(rightStandRot);

        // nếu vẫn còn tốc → giảm từ từ
        if (Mathf.Abs(bodyRb.linearVelocity.x) > 0.1f)
        {
            Vector2 v = bodyRb.linearVelocity;
            v.x *= 0.9f; // giảm nhẹ nhưng mượt
            bodyRb.linearVelocity = v;
        }
        else
        {
            // dừng hẳn
            Vector2 v = bodyRb.linearVelocity;
            v.x = 0;
            bodyRb.linearVelocity = v;
        }
    }
}
