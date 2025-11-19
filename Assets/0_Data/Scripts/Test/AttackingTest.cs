

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackingTest : MonoBehaviour
{
    [SerializeField] Balance rightArm;
    [SerializeField] Balance rightArmDown;
    [SerializeField] Balance rightLeg;
    [SerializeField] Balance rightLegDown;

    [SerializeField] Balance leftArm;
    [SerializeField] Balance leftArmDown;
    [SerializeField] Balance leftLeg;
    [SerializeField] Balance leftLegDown;

    [SerializeField] Balance body;
    [SerializeField] float attackForce;
    [SerializeField] AnimationCurve attackCurve;
    private float attackSpeed;
    float time;

    private Vector2 attackDir;

    private bool isAttacking;
    [SerializeField] private AnimationCurve rotateCurve;

    public bool IsAttacking=>isAttacking;
    private void Start()
    {
        StartAttack();
    }
    private void Update()
    {
        AttackDirHandle();

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(HandleAttack(rightArm, rightArmDown));
            ////StartCoroutine(RightPunch());
            //IEnumerator punchRight = BodyPartHandle(rightArm, rightArmDown, 110, 100, 10, 10, -15, 0, 0, 0,5, 1); 
            //IEnumerator elbowRight = BodyPartHandle(rightArm, rightArmDown, 100, -65, 20, 20, -15, 0, 0, 0,5, 1); 
            //IEnumerator kickRight = BodyPartHandle(rightLeg, rightLegDown, 100, 100, 50, 35, 50, 0, 0, 0,20, 15); 
            //IEnumerator pillowRight = BodyPartHandle(rightLeg, rightLegDown,90,-25,50,35,50, 0, 0, 0,20, 15); 
            //IEnumerator[] coroutines = { punchRight, elbowRight,kickRight, pillowRight };
            //int rand = Random.Range(0, coroutines.Length);
            //StartCoroutine(coroutines[rand]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //IEnumerator punchLeft = BodyPartHandle(leftArm,leftArmDown, -110, -100, 10, 10, 15, 0, 0, 0, 5, 1);
            //IEnumerator elbowLeft = BodyPartHandle(leftArm,leftArmDown, -100, 65, 20, 20, 15, 0, 0, 0, 5, 1);
            //IEnumerator kickLeft = BodyPartHandle(leftLeg,leftLegDown, -100, -100, 50, 35, -50, 0, 0, 0, 20, 15);
            //IEnumerator pillowLeft = BodyPartHandle(leftLeg,leftLegDown, -90, 25, 50, 35, -50, 0, 0, 0, 20, 15);
            //IEnumerator[] coroutines = { punchLeft,elbowLeft,kickLeft,pillowLeft };
            //int rand = Random.Range(0, coroutines.Length);
            //StartCoroutine(coroutines[rand]);
        }
    }
    private void StartAttack()
    {
        time = 0;
        attackSpeed = attackCurve.Evaluate(time);
    }
    private void AttackHandle()
    {
        time += Time.fixedDeltaTime;
        attackSpeed = attackCurve.Evaluate(time);

    }
    private IEnumerator HandleAttack(Balance p1,Balance p2)
    {
        float t = 0f;
        float duration = 0.25f;

        // Lưu giá trị ban đầu
        float p1StartRot = p1.TargetRotation;
        float p2StartRot = p2.TargetRotation;
        float bodyStartRot = body.TargetRotation;

        // Giá trị muốn xoay tới
        float p1EndRot = 100;
        float p2EndRot = -60;
        float bodyEndRot = -50;

        // Toàn bộ chạy trong 1 coroutine
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = t / duration;

            // Easing bằng AnimationCurve
            float eased = rotateCurve.Evaluate(ratio);

            // Rotate
            p1.SetTargetRotation(Mathf.Lerp(p1StartRot, p1EndRot, eased));
            p2.SetTargetRotation(Mathf.Lerp(p2StartRot, p2EndRot, eased));
            body.SetTargetRotation(Mathf.Lerp(bodyStartRot, bodyEndRot, eased));

            yield return null;
        }

        // Apply lực sau khi xoay xong
        float speed = attackCurve.Evaluate(1f);
        p1.Rb.linearVelocity = attackDir * speed;
        body.Rb.linearVelocity = attackDir * (speed / 2f);

        // Giữ một chút
        yield return new WaitForSeconds(0.2f);

        // Trả khớp về ban đầu – cũng chỉ trong 1 coroutine
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = t / duration;
            float eased = rotateCurve.Evaluate(ratio);

            p1.SetTargetRotation(Mathf.Lerp(p1EndRot, p1StartRot, eased));
            p2.SetTargetRotation(Mathf.Lerp(p2EndRot, p2StartRot, eased));
            body.SetTargetRotation(Mathf.Lerp(bodyEndRot, bodyStartRot, eased));

            yield return null;
        }
    }

    private void Init(Balance p1, Balance p2, out float rot1, out float rot2, out float for1, out float for2)
    {
        rot1 = p1.TargetRotation;
        rot2 = p2.TargetRotation;
        for1 = p1.Force;
        for2 = p2.Force;
    }

    private void AttackDirHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;
    }
    #region Left attack
    private IEnumerator LeftPunch()
    {
        isAttacking = true;
        leftArm.SetPropertie(-110, 10);
        leftArmDown.SetPropertie(-100, 10);
        body.SetTargetRotation(15);

        leftArm.Rb.linearVelocity = attackDir * attackForce;
        leftArmDown.Rb.linearVelocity = attackDir * attackForce;
        body.Rb.AddForce(attackDir * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        leftArm.SetPropertie(0, 5);
        leftArmDown.SetPropertie(0, 1);
        isAttacking = false;
    }
    private IEnumerator LeftElbow()
    {
        isAttacking = true;
        leftArm.SetPropertie(-100, 20);
        leftArmDown.SetPropertie(65, 20);
        body.SetTargetRotation(15);

        leftArm.Rb.linearVelocity = Vector2.left * attackForce;
        leftArmDown.Rb.linearVelocity = Vector2.left * attackForce;
        body.Rb.AddForce(Vector2.left * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        leftArm.SetPropertie(0, 5);
        leftArmDown.SetPropertie(0, 1);
        isAttacking = false;
    }
    private IEnumerator LeftKick()
    {
        isAttacking = true;
        leftLeg.SetPropertie(-100, 50);
        leftLegDown.SetPropertie(-100, 35);
        body.SetTargetRotation(-50);

        leftLeg.Rb.linearVelocity = Vector2.left * attackForce;
        leftLegDown.Rb.linearVelocity = Vector2.left * attackForce;
        body.Rb.AddForce(Vector2.left * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        leftLeg.SetPropertie(0, 20);
        leftLegDown.SetPropertie(0, 15);
        isAttacking = false;
    }
    private IEnumerator LeftPillow()
    {
        isAttacking = true;
        leftLeg.SetPropertie(-90, 50);
        leftLegDown.SetPropertie(25, 35);
        body.SetTargetRotation(-50);

        leftLeg.Rb.linearVelocity = Vector2.left * attackForce;
        leftLegDown.Rb.linearVelocity = Vector2.left * attackForce;
        body.Rb.AddForce(Vector2.left * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        leftLeg.SetPropertie(0, 20);
        leftLegDown.SetPropertie(0, 15);
        isAttacking = false;
    }
    #endregion

    #region Right attack
    private IEnumerator RightPunch()
    {
        isAttacking = true;
        rightArm.SetPropertie(110,10);
        rightArmDown.SetPropertie(100,10);
        body.SetTargetRotation(-15);

        rightArm.Rb.linearVelocity = Vector2.right * attackForce;
        rightArmDown.Rb.linearVelocity = Vector2.right * attackForce;
        body.Rb.AddForce(Vector2.right * attackForce,ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        rightArm.SetPropertie(0, 5);
        rightArmDown.SetPropertie(0, 1);
        isAttacking= false;
    }
    private IEnumerator RightElbow()
    {
        isAttacking = true;
        rightArm.SetPropertie(100, 20);
        rightArmDown.SetPropertie(-65, 20);
        body.SetTargetRotation(-15);

        rightArm.Rb.linearVelocity = Vector2.right * attackForce;
        rightArmDown.Rb.linearVelocity = Vector2.right * attackForce;
        body.Rb.AddForce(Vector2.right * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        rightArm.SetPropertie(0, 5);
        rightArmDown.SetPropertie(0, 1);
        isAttacking = false;
    }
    private IEnumerator RightKick()
    {
        isAttacking = true;
        rightLeg.SetPropertie(100, 50);
        rightLegDown.SetPropertie(100, 35);
        body.SetTargetRotation(50);

        rightLeg.Rb.linearVelocity = Vector2.right *attackForce;
        rightLegDown.Rb.linearVelocity = Vector2.right * attackForce;
        body.Rb.AddForce(Vector2.right * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        rightLeg.SetPropertie(0, 20);
        rightLegDown.SetPropertie(0, 15);
        isAttacking = false;
    }
    private IEnumerator RightPillow()
    {
        isAttacking = true;
        rightLeg.SetPropertie(90, 50);
        rightLegDown.SetPropertie(-25, 35);
        body.SetTargetRotation(50);

        rightLeg.Rb.linearVelocity = Vector2.right * attackForce;
        rightLegDown.Rb.linearVelocity = Vector2.right * attackForce;
        body.Rb.AddForce(Vector2.right * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(0);
        rightLeg.SetPropertie(0, 20);
        rightLegDown.SetPropertie(0, 15);
        isAttacking = false;
    }
    #endregion

    private IEnumerator BodyPartHandle(Balance part_1,Balance part_2,float rot_1,float rot_2,float force_1, float force_2,float body_rot, float init_body_rot, float init_rot_1, float init_rot_2, float init_force_1, float init_force_2)
    {
        isAttacking = true;
        part_1.SetPropertie(rot_1,force_1);
        part_2.SetPropertie(rot_2,force_2);
        body.SetTargetRotation(body_rot);

        part_1.Rb.linearVelocity = attackDir*attackForce;
        part_2.Rb.linearVelocity = attackDir * attackForce;
        body.Rb.AddForce(attackDir * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        body.SetTargetRotation(init_body_rot);
        part_1.SetPropertie(init_rot_1,init_force_1);
        part_2.SetPropertie(init_rot_2, init_force_2);

        isAttacking = false;
    }
    //private TypeAttack GetRandom()
    //{
    //    sysArray values = Enum.GetValues(typeof(TypeAttack));
    //    return (TypeAttack)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    //}

}