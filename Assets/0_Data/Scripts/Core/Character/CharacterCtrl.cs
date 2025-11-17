
using System.Collections;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{


    #region Object Attribute
    [SerializeField] private Balance body;
    [SerializeField] Balance rightArm;
    [SerializeField] Balance rightArmDown;
    [SerializeField] Balance rightLeg;
    [SerializeField] Balance rightLegDown;

    [SerializeField] Balance leftArm;
    [SerializeField] Balance leftArmDown;
    [SerializeField] Balance leftLeg;
    [SerializeField] Balance leftLegDown;
    #endregion

    #region Movement Attribute
    [Space]
    [Header("action index")]
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên

    private Coroutine moveCoroutine;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    #endregion

    #region Attack Attribute
    [SerializeField] float attackForce = 15f;
    [SerializeField] float attackDuration = 0.5f;

    [SerializeField] private float attackReach = 1f;            // Khoảng cách tay bay trước
    [SerializeField] private float bodyMomentumFactor = 0.5f;   // Tỷ lệ momentum cơ thể kéo tay
    [SerializeField] private float decelDistance = 0.4f;        // Khoảng cách giảm tốc khi gần target
    [SerializeField] private float maxForce = 50f;              // Lực tối đa áp dụng mỗi frame
    [SerializeField] private float maxAngularSpeed = 200f;      // Tốc độ xoay tối đa mỗi frame
    [SerializeField] private float linearDrag = 2f;             // Damping tuyến tính của khớp
    [SerializeField] private float angularDrag = 5f;            // Damping xoay của khớp
    [SerializeField] private float rotateSmoothSpeed = 10f;     // Tốc độ xoay mượt

    private Vector2 attackDir;
    private bool isAttacking;
    #endregion


    //[SerializeField] private float stiffness = 45f;         // mềm mại hơn

    [Space]
    [Header("Ground check")]
    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    private Animator anim;

    //get
    public Vector2 AttackDir => attackDir;
    public float AttackForce => attackForce;


    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        //AttackHandle();
        HandleAttack();

        MoveHandle();
        JumpHandle();
        AttackDirHandle();

        LimitVelocity(body.Rb);
        LimitVelocity(leftLeg.Rb);
        LimitVelocity(rightLeg.Rb);

    }

    private void AttackDirHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;
    }

    #region Utilities
    //limit Handle
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }
    #endregion

    #region Attack
    private void AttackHandle()
    {
        if (InputManager.Instance.ConsumeAttackRightBuffer() && attackDir.x >0)
        {
            //StartCoroutine(RightPunch());
            IEnumerator punchRight = BodyPartHandle(rightArm, rightArmDown, 110, 100, 10, 10, -15, 0, 0, 0, 5, 1);
            IEnumerator elbowRight = BodyPartHandle(rightArm, rightArmDown, 100, -65, 20, 20, -15, 0, 0, 0, 5, 1);
            IEnumerator kickRight = BodyPartHandle(rightLeg, rightLegDown, 100, 100, 50, 35, 50, 0, 0, 0, 20, 15);
            IEnumerator pillowRight = BodyPartHandle(rightLeg, rightLegDown, 90, -25, 50, 35, 50, 0, 0, 0, 20, 15);
            IEnumerator[] coroutines = { punchRight, elbowRight, kickRight, pillowRight };
            int rand = Random.Range(0, coroutines.Length);
            StartCoroutine(coroutines[rand]);
        }
        if (InputManager.Instance.ConsumeAttackLeftBuffer() && attackDir.x <0)
        {
            IEnumerator punchLeft = BodyPartHandle(leftArm, leftArmDown, -110, -100, 10, 10, 15, 0, 0, 0, 5, 1);
            IEnumerator elbowLeft = BodyPartHandle(leftArm, leftArmDown, -100, 65, 20, 20, 15, 0, 0, 0, 5, 1);
            IEnumerator kickLeft = BodyPartHandle(leftLeg, leftLegDown, -100, -100, 50, 35, -50, 0, 0, 0, 20, 15);
            IEnumerator pillowLeft = BodyPartHandle(leftLeg, leftLegDown, -90, 25, 50, 35, -50, 0, 0, 0, 20, 15);
            IEnumerator[] coroutines = { punchLeft, elbowLeft, kickLeft, pillowLeft };
            int rand = Random.Range(0, coroutines.Length);
            StartCoroutine(coroutines[rand]);
        }
    }
    private IEnumerator BodyPartHandle(Balance part_1, Balance part_2, float rot_1, float rot_2, float force_1, float force_2, float body_rot, float init_body_rot, float init_rot_1, float init_rot_2, float init_force_1, float init_force_2)
    {
        isAttacking = true;

        // Cài đặt drag vật lý để tránh văng khớp
        part_1.Rb.linearDamping = linearDrag;
        part_2.Rb.linearDamping = linearDrag;
        part_1.Rb.angularDamping = angularDrag;
        part_2.Rb.angularDamping = angularDrag;

        part_1.SetPropertie(rot_1, force_1);
        part_2.SetPropertie(rot_2, force_2);
        body.SetTargetRotation(body_rot);

        part_1.Rb.linearVelocity = attackDir * attackForce;
        part_2.Rb.linearVelocity = attackDir * attackForce;
        body.Rb.linearVelocity = attackDir * (attackForce / 2);

        yield return new WaitForSeconds(attackDuration);

        body.SetTargetRotation(init_body_rot);
        part_1.SetPropertie(init_rot_1, init_force_1);
        part_2.SetPropertie(init_rot_2, init_force_2);

        isAttacking = false;
    }

    #region Test
    private void HandleAttack()
    {
        //right
        //IEnumerator rightPunch = SmoothAttack(rightArm, rightArmDown, 90, 80);
        //IEnumerator rightKick = SmoothAttack(rightLeg, rightArmDown, 90, 80);
        //IEnumerator rightElbow = SmoothAttack(rightArm, rightArmDown, 90, 80);
        //IEnumerator rightPillow = SmoothAttack(rightArm, rightArmDown, 90, 80);
        if (Input.GetKeyDown(KeyCode.E) && attackDir.x > 0)
            StartCoroutine(SmoothAttack(rightArm, rightArmDown,90,80));
        if (InputManager.Instance.ConsumeAttackLeftBuffer() && attackDir.x < 0)
            StartCoroutine(SmoothAttack(leftArm, leftArmDown,90,80));
    }

    private IEnumerator SmoothAttack(Balance part1, Balance part2,float rot_1,float rot_2)
    {
        isAttacking = true;

        // Lưu rotation ban đầu
        float initRot1 = part1.TargetRotation;
        float initRot2 = part2.TargetRotation;

        // Mục tiêu xoay forward
        float targetRot1 = initRot1 + rot_1;
        float targetRot2 = initRot2 + rot_2;

        // Cài đặt drag vật lý để tránh văng khớp
        part1.Rb.linearDamping = linearDrag;
        part2.Rb.linearDamping = linearDrag;
        part1.Rb.angularDamping = angularDrag;
        part2.Rb.angularDamping = angularDrag;

        float elapsed = 0f;
        float launchTime = 0.3f; // 10% duration dùng lực chính
        float proceduralOffset = 0.2f;
        while (elapsed < attackDuration)
        {
            elapsed += Time.fixedDeltaTime;

            // 1️⃣ Xoay tay mượt với giới hạn tốc độ xoay
            float t_1 = SmoothMotionHelper.SmoothRotateLimited(part1.TargetRotation, targetRot1,rotateSmoothSpeed,maxAngularSpeed);
            float t_2 = SmoothMotionHelper.SmoothRotateLimited(part2.TargetRotation, targetRot2, rotateSmoothSpeed, maxAngularSpeed);
            part1.SetTargetRotation(t_1);
            part2.SetTargetRotation(t_2);
            // 2️⃣ Tính target theo momentum cơ thể
            Vector2 targetPos = (Vector2)body.Rb.position + attackDir * attackReach + Vector2.Perpendicular(attackDir) * proceduralOffset;
            if (elapsed < launchTime)
            {
                part1.Rb.linearVelocity = attackDir * attackForce;  // Đẩy tay thẳng tới target
                part2.Rb.linearVelocity = attackDir * attackForce;
                body.Rb.AddForce(attackDir * attackForce * 0.3f, ForceMode2D.Impulse); // Kéo body
            }
            // 3️⃣ Di chuyển tay procedural với lực giới hạn và giảm tốc
            SmoothMotionHelper.SmoothMoveTowardsLimited(part1.Rb, targetPos, maxSpeed, maxForce, decelDistance);
            SmoothMotionHelper.SmoothMoveTowardsLimited(part2.Rb, targetPos, maxSpeed, maxForce, decelDistance);

            // 4️⃣ Đẩy cơ thể bay nhẹ procedural
            SmoothMotionHelper.ApplySoftImpulse(body.Rb, attackDir, attackForce * 0.2f, 0.5f);

            yield return new WaitForFixedUpdate();
        }

        // Reset rotation mượt
        part1.SetTargetRotation(initRot1);
        part2.SetTargetRotation(initRot2);

        isAttacking = false;
    }


    #endregion

    #endregion

    #region Jump
    //jump
    private void JumpHandle()
    {
        bool isGround = IsGround();

        if (InputManager.Instance.ConsumeJumpBuffer() && isGround)
        {
            body.Rb.AddForce(Vector2.up * bodyForce, ForceMode2D.Impulse);
            leftLeg.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
            rightLeg.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
        }
    }
    #endregion

    #region Moving

    private void MoveHandle()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (isAttacking) return;
        if (Mathf.Abs(x) != 0)
        {

            if (x > 0)
            {
                body.Rb.AddForce(Vector2.right * bodyForce, ForceMode2D.Impulse);
                if (!isMovingRight)
                {

                    isMovingRight = true;
                    isMovingLeft = false;
                    if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                    moveCoroutine = StartCoroutine(MoveRight(legWait));
                }
            }
            else
            {
                body.Rb.AddForce(Vector2.left * bodyForce, ForceMode2D.Impulse);
                if (!isMovingLeft)
                {

                    isMovingLeft = true;
                    isMovingRight = false;
                    if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                    moveCoroutine = StartCoroutine(MoveLeft(legWait)); ;
                }

            }

        }
        else
        {
            leftLeg.SetPropertie(0, 20);
            rightLeg.SetPropertie(0, 20);
            body.Rb.linearVelocity = new Vector2(body.Rb.linearVelocity.x * damping, body.Rb.linearVelocity.y);
        }
    }
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            rightLeg.SetTargetRotation(5);
            leftLeg.SetTargetRotation(90);

            //leftLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(rightLeg.Rb, rightLeg.Rb.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);

            rightLeg.SetTargetRotation(90);
            leftLeg.SetTargetRotation(5);

            // rightLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(leftLeg.Rb, leftLeg.Rb.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {
            rightLeg.SetTargetRotation(-90);
            leftLeg.SetTargetRotation(-5);

            //rightLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(rightLeg.Rb, rightLeg.Rb.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);

            rightLeg.SetTargetRotation(-5);
            leftLeg.SetTargetRotation(-90);

            //leftLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(leftLeg.Rb, leftLeg.Rb.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }
    #endregion

    #region Collistion check
    public bool IsGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
    #endregion
}