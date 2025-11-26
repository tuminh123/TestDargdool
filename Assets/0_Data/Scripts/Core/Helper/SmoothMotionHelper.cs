using UnityEngine;

/// <summary>
/// Class helper hỗ trợ chuyển động mượt cho Rigidbody2D và các khớp
/// Tạo cảm giác bay bổng, mềm mại và procedural
/// </summary>
public static class SmoothMotionHelper
{
    /// <summary>
    /// Xoay khớp mượt từ currentRotation đến rotation
    /// smoothSpeed: tốc độ mượt, càng lớn càng nhanh đuổi target
    /// </summary>
    public static float SmoothRotateTo(float currentRotation, float targetRotation, float smoothSpeed = 10f)
    {
        // Mathf.LerpAngle giúp xoay theo góc, xử lý wrap-around 360°
        return Mathf.LerpAngle(currentRotation, targetRotation, Time.fixedDeltaTime * smoothSpeed);
    }
    /// <summary>
    /// Xoay mượt với giới hạn tốc độ xoay
    /// </summary>
    public static float SmoothRotateLimited(float currentRot, float targetRot, float rotateSmoothSpeed, float maxAngularSpeed)
    {
        float newRot = Mathf.LerpAngle(currentRot, targetRot, Time.fixedDeltaTime * rotateSmoothSpeed);
        float delta = Mathf.DeltaAngle(currentRot, newRot);
        delta = Mathf.Clamp(delta, -maxAngularSpeed * Time.fixedDeltaTime, maxAngularSpeed * Time.fixedDeltaTime);
        return currentRot + delta;
    }

    /// <summary>
    /// Di chuyển Rigidbody2D mượt đến targetPosition
    /// maxSpeed: tốc độ tối đa
    /// accelerate: độ tăng tốc (forceChange áp dụng)
    /// decelDistance: khoảng cách bắt đầu giảm tốc khi gần target
    /// </summary>
    public static void SmoothMoveTowards(Rigidbody2D rb, Vector2 targetPosition, float maxSpeed, float accelerate = 30f, float decelDistance = 0.5f)
    {
        Vector2 dir = (targetPosition - rb.position);
        float distance = dir.magnitude;
        dir.Normalize();

        // Tính hệ số giảm tốc khi gần target
        float speedFactor = Mathf.Clamp01(distance / decelDistance);

        // Tốc độ mong muốn
        float desiredSpeed = maxSpeed * speedFactor;

        // Lực điều chỉnh: steering = (vDesired - vCurrent) * accelerate
        Vector2 desiredVelocity = dir * desiredSpeed;
        Vector2 steering = (desiredVelocity - rb.linearVelocity) * accelerate;

        rb.AddForce(steering);
    }

    /// <summary>
    /// Di chuyển procedural với giới hạn lực và giảm tốc gần target
    /// </summary>
    public static void SmoothMoveTowardsLimited(Rigidbody2D rb, Vector2 targetPos, float maxSpeed, float maxForce, float decelDistance)
    {
        Vector2 dir = targetPos - rb.position;
        float distance = dir.magnitude;
        dir.Normalize();

        float speedFactor = Mathf.Clamp01(distance / decelDistance);
        float desiredSpeed = maxSpeed * speedFactor;

        Vector2 desiredVelocity = dir * desiredSpeed;
        Vector2 steering = (desiredVelocity - rb.linearVelocity) * 20f; // hệ số tăng tốc 20

        // Giới hạn lực tối đa
        steering = Vector2.ClampMagnitude(steering, maxForce);

        rb.AddForce(steering);
    }


    /// <summary>
    /// Tạo lực đẩy mềm thay vì Impulse giật mạnh
    /// direction: hướng đẩy
    /// forceChange: tổng lực
    /// softness: tỷ lệ lực mềm, 0 = toàn lực bình thường, 1 = toàn lực mềm
    /// </summary>
    public static void ApplySoftImpulse(Rigidbody2D rb, Vector2 direction, float force, float softness = 0.3f)
    {
        // Lực mềm (Impulse nhỏ)
        rb.AddForce(direction.normalized * (force * softness), ForceMode2D.Impulse);

        // Lực chính (AddForce bình thường)
        rb.AddForce(direction.normalized * (force * (1f - softness)));
    }
}
#region Comment Attribute
//   Tham số	   :                        Tác dụng
//smoothSpeed  :	Tốc độ xoay khớp, càng cao càng nhanh nhưng quá cao sẽ cứng
//maxSpeed	   :    Tốc độ tối đa của khớp khi di chuyển/đấm
//accelerate   :	Lực áp dụng để đạt tốc độ mong muốn, càng cao càng nhanh nhưng dễ giật
//decelDistance: 	Khoảng cách gần target bắt đầu giảm tốc → tạo cảm giác cú đấm có lực
//softness	   :    Tỷ lệ lực “mềm” khi tạo impulse, giúp cú đấm bay bổng, float nhẹ
#endregion
