

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

    private Vector2 attackDir;

    private bool isAttacking;
    public bool IsAttacking=>isAttacking;
    private void Update()
    {
        AttackDirHandle();

        if (Input.GetKeyDown(KeyCode.E))
        {
            //StartCoroutine(RightPunch());
            IEnumerator punchRight = BodyPartHandle(rightArm, rightArmDown, 110, 100, 10, 10, -15, 0, 0, 0,5, 1); 
            IEnumerator elbowRight = BodyPartHandle(rightArm, rightArmDown, 100, -65, 20, 20, -15, 0, 0, 0,5, 1); 
            IEnumerator kickRight = BodyPartHandle(rightLeg, rightLegDown, 100, 100, 50, 35, 50, 0, 0, 0,20, 15); 
            IEnumerator pillowRight = BodyPartHandle(rightLeg, rightLegDown,90,-25,50,35,50, 0, 0, 0,20, 15); 
            IEnumerator[] coroutines = { punchRight, elbowRight,kickRight, pillowRight };
            int rand = Random.Range(0, coroutines.Length);
            StartCoroutine(coroutines[rand]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IEnumerator punchLeft = BodyPartHandle(leftArm,leftArmDown, -110, -100, 10, 10, 15, 0, 0, 0, 5, 1);
            IEnumerator elbowLeft = BodyPartHandle(leftArm,leftArmDown, -100, 65, 20, 20, 15, 0, 0, 0, 5, 1);
            IEnumerator kickLeft = BodyPartHandle(leftLeg,leftLegDown, -100, -100, 50, 35, -50, 0, 0, 0, 20, 15);
            IEnumerator pillowLeft = BodyPartHandle(leftLeg,leftLegDown, -90, 25, 50, 35, -50, 0, 0, 0, 20, 15);
            IEnumerator[] coroutines = { punchLeft,elbowLeft,kickLeft,pillowLeft };
            int rand = Random.Range(0, coroutines.Length);
            StartCoroutine(coroutines[rand]);
        }
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