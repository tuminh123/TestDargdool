using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

#region Attack Data

[System.Serializable]
public class AttackData
{
    //event
    public System.Action OnAttackEnd;
    public System.Action OnAttacking;

    [SerializeField] private Balance bodyPart_1, bodyPart_2;
    [SerializeField] private AttackRotSO attackRotSo;
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public IEnumerator ExecuteAttack(AttackDataConfigSO configSO, Vector2 attackDir, Balance body)
    {
        if (bodyPart_1 == null || bodyPart_2 == null || attackRotSo == null || configSO == null || attackDir == null || body == null) yield return null; 
        isAttacking = true;

        attackRotSo.InitData(bodyPart_1, bodyPart_2);

        // Mục tiêu xoay forward
        float targetRot1 = attackRotSo.Rot_1;
        float targetRot2 = attackRotSo.Rot_2;


        // Cài đặt drag vật lý để tránh văng khớp
        bodyPart_1.Rb.linearDamping = configSO.LinearDrag;
        bodyPart_2.Rb.linearDamping = configSO.LinearDrag;
        bodyPart_1.Rb.angularDamping = configSO.AngularDrag;
        bodyPart_2.Rb.angularDamping = configSO.AngularDrag;
        
        float rotateSmoothSpeed = configSO.RotateSmoothSpeed;
        float maxAngularSpeed = configSO.MaxAngularSpeed;

        // ===== Phase 1: Định hình tay =====
        float poseTime = 0.35f; // thời gian định hình
        float elapsedPose = 0f;

        while (elapsedPose < poseTime)
        {
            elapsedPose += Time.fixedDeltaTime;

            // Xoay tay mượt về target
            float t_1 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_1.Rotation, targetRot1, rotateSmoothSpeed, maxAngularSpeed);
            float t_2 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_2.Rotation, targetRot2, rotateSmoothSpeed, maxAngularSpeed);
            bodyPart_1.SetRotation(t_1);
            bodyPart_2.SetRotation(t_2);

            yield return new WaitForFixedUpdate();
        }
        
        float elapsed = 0f;
        //float poseTime = 0.2f; // thời gian định hình tay (có thể config)
        float launchTime = configSO.LaunchTime;
        float proceduralOffset = configSO.ProceduralOffset;

        while (elapsed < configSO.AttackDuration)
        {
            elapsed += Time.fixedDeltaTime;

            //// 1️⃣ Xoay tay mượt với giới hạn tốc độ xoay
            //// float rotateSmoothSpeed = configSO.RotateSmoothSpeed;
            //// float maxAngularSpeed = configSO.MaxAngularSpeed;
            //float t_1 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_1.Rotation, targetRot1, rotateSmoothSpeed,maxAngularSpeed);
            //float t_2 = SmoothMotionHelper.SmoothRotateLimited(bodyPart_2.Rotation, targetRot2, rotateSmoothSpeed, maxAngularSpeed);
            //bodyPart_1.SetRotation(t_1);
            //bodyPart_2.SetRotation(t_2);
            
            // 2️⃣ Tính target theo momentum cơ thể
            float attackReach = configSO.AttackReach;
            float attackForce = configSO.AttackForce;
            Vector2 targetPos = (Vector2)body.Rb.position + attackDir * attackReach + Vector2.Perpendicular(attackDir) * proceduralOffset;
            if (elapsed < launchTime)
            {
                bodyPart_1.Rb.linearVelocity = attackDir * attackForce; // Đẩy tay thẳng tới target
                bodyPart_2.Rb.linearVelocity = attackDir * attackForce;
                body.Rb.linearVelocity = attackDir * attackForce * 0.3f; // Kéo body
            }

            // 3️⃣ Di chuyển tay procedural với lực giới hạn và giảm tốc
            float maxSpeed = configSO.MaxSpeed;
            float maxForce = configSO.MaxForce;
            float decelDistance = configSO.DecelDistance;
            SmoothMotionHelper.SmoothMoveTowardsLimited(bodyPart_1.Rb, targetPos, maxSpeed, maxForce, decelDistance);
            SmoothMotionHelper.SmoothMoveTowardsLimited(bodyPart_2.Rb, targetPos, maxSpeed, maxForce, decelDistance);

            // 4️⃣ Đẩy cơ thể bay nhẹ procedural
            SmoothMotionHelper.ApplySoftImpulse(body.Rb, attackDir, attackForce * 0.2f, 0.5f);

            OnAttacking?.Invoke();

            yield return new WaitForFixedUpdate();

        }
        OnAttackEnd?.Invoke();
        attackRotSo.ResetData(bodyPart_1,bodyPart_2);
        isAttacking = false;
    }
}

    #endregion

public class Attack : MonoBehaviour
{
    
    [SerializeField] private Balance body;
    //[SerializeField] private AttackDataConfigSO configSO;

    //[SerializeField] private AttackData[] leftAttacks;
    //[SerializeField] private AttackData[] rightAttacks;

    [SerializeField] private AttackBase[] leftAttacks;
    [SerializeField] private AttackBase[] rightAttacks;

    public AttackBase currentAttackData { get; private set; }

    private Coroutine attackRoutine;
    public bool isAttacking {  get; private set; }

    //get
    //public AttackDataConfigSO ConfigSo => configSO;

    //private bool isAttacking;

    public AttackBase GetRandomAttack(bool isRight)
    {
        if (!CanAttack()) return null;
        var list = isRight ? rightAttacks : leftAttacks;
        return list[Random.Range(0, list.Length)];
    }
    public void HandleAttack(Vector2 attackDir)
    {
        bool isRight = attackDir.x > 0; 
        AttackBase attack = GetRandomAttack(isRight);
        if (attack == null) return;
        currentAttackData = attack;
        attackRoutine = StartCoroutine(ExecuteAttack(attack,attackDir));
    }
    private IEnumerator ExecuteAttack(AttackBase attack,Vector2 attackDir)
    {
        isAttacking = true;
        attack.AttackHandle(attackDir);

        yield return new WaitForSeconds(1);

        isAttacking = false;
        attack.AttackEnd();
    }
    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
    public bool CanAttack()
    {
        return rightAttacks.Length > 0 && leftAttacks.Length > 0;
    }
    
}