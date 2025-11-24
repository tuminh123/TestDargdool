using UnityEngine;


public class AttackHandle 
{
    private Balance bodyPart_1,bodyPart_2,body;

    private float rot_1,rot_2;
    private float initRot1, initRot2;
    private float initLinearDamping_1, initLinearDamping_2;
    private float initAngularDamping_1, initAngularDamping_2;
    
    private Vector2 attackDir;

    public AttackHandle(Balance bodyPart_1, Balance bodyPart_2,Balance body, float rot_1, float rot_2,Vector2 attackDir)
    {
        this.bodyPart_1 = bodyPart_1;
        this.bodyPart_2 = bodyPart_2;
        this.body = body;
        
        this.rot_1 = rot_1;
        this.rot_2 = rot_2;
        this.initRot1 = bodyPart_1.Rotation;
        this.initRot2 = bodyPart_2.Rotation;
        
        initLinearDamping_1 = bodyPart_1.Rb.linearDamping;
        initLinearDamping_2 = bodyPart_2.Rb.linearDamping;
        initAngularDamping_1 = bodyPart_1.Rb.angularDamping;
        initAngularDamping_2 = bodyPart_2.Rb.angularDamping;

        this.attackDir = attackDir;
    }

    public void AttackBegin(AttackDataConfigSO configSO)
    {
        // Mục tiêu xoay forward
        float targetRot1 = initRot1 + rot_1;
        float targetRot2 = initRot2 + rot_2;

        // Cài đặt drag vật lý để tránh văng khớp
        bodyPart_1.Rb.linearDamping = configSO.LinearDrag;
        bodyPart_2.Rb.linearDamping = configSO.LinearDrag;
        bodyPart_1.Rb.angularDamping = configSO.AngularDrag;
        bodyPart_2.Rb.angularDamping = configSO .AngularDrag;

        float elapsed = 0f;
        float launchTime = configSO.LaunchTime;
        float proceduralOffset = configSO.ProceduralOffset;
        while (elapsed < configSO.AttackDuration)
        {
            elapsed += Time.fixedDeltaTime;

            // 1️⃣ Xoay tay mượt với giới hạn tốc độ xoay
            float rotateSmoothSpeed = configSO.RotateSmoothSpeed;
            float maxAngularSpeed = configSO.MaxAngularSpeed;
            float t_1 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_1.Rotation, targetRot1, rotateSmoothSpeed, maxAngularSpeed);
            float t_2 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_2.Rotation, targetRot2, rotateSmoothSpeed, maxAngularSpeed);
            bodyPart_1.SetRotation(t_1);
            bodyPart_2.SetRotation(t_2);

            // 2️⃣ Tính target theo momentum cơ thể
            float attackReach = configSO.AttackReach;
            float attackForce = configSO.AttackForce;
            Vector2 targetPos = (Vector2)body.Rb.position + attackDir * attackReach + Vector2.Perpendicular(attackDir) * proceduralOffset;
            if (elapsed < launchTime)
            {
                bodyPart_1.Rb.linearVelocity = attackDir * attackForce;  // Đẩy tay thẳng tới target
                bodyPart_2.Rb.linearVelocity = attackDir * attackForce;
                body.Rb.AddForce(attackDir * attackForce * 0.3f, ForceMode2D.Impulse); // Kéo bodyParent
            }
            // 3️⃣ Di chuyển tay procedural với lực giới hạn và giảm tốc
            float maxSpeed = configSO.MaxSpeed;
            float maxForce = configSO.MaxForce;
            float decelDistance = configSO.DecelDistance;
            SmoothMotionHelper.SmoothMoveTowardsLimited(bodyPart_1.Rb, targetPos, maxSpeed, maxForce, decelDistance);
            SmoothMotionHelper.SmoothMoveTowardsLimited(bodyPart_2.Rb, targetPos, maxSpeed, maxForce, decelDistance);

            // 4️⃣ Đẩy cơ thể bay nhẹ procedural
            SmoothMotionHelper.ApplySoftImpulse(body.Rb, attackDir, attackForce * 0.2f, 0.5f);

        }
    }

    public void AttackEnd()
    {
        
        bodyPart_1.SetRotation(initRot1);
        bodyPart_2.SetRotation(initRot2);
        
        // Restore damping
        bodyPart_1.Rb.linearDamping = initLinearDamping_1;
        bodyPart_2.Rb.linearDamping = initLinearDamping_2;
        bodyPart_1.Rb.angularDamping = initAngularDamping_1;
        bodyPart_2.Rb.angularDamping = initAngularDamping_2;
    }
}
