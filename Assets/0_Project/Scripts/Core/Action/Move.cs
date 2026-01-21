using System;
using System.Collections;
using UnityEngine;


public class Move :MonoBehaviour
{

    [SerializeField] float speed = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] float bodyForce = 2f;
    //[SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên

    private Coroutine moveCoroutine;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    //get
    public bool IsMovingRight => isMovingRight;
    public bool IsMovingLeft => isMovingLeft;

    #region  Utils


    public void LimitMoving(Rigidbody2D Body,Rigidbody2D LeftLeg,Rigidbody2D RightLeg)
    {
        if (Body == null || LeftLeg == null || RightLeg == null) return;
        LimitVelocity(Body);
        LimitVelocity(LeftLeg);
        LimitVelocity(RightLeg);
    }
    //limit Handle
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (rb == null) return;
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }

    #endregion

    #region Moving handle

    public void MoveHandle(float x, IPostAction action)
    {
        //if (isAttacking) return;
        if(action == null) return;

        Rigidbody2D body = action?.GetBalance(BalanceType.body_up)?.Rb;
        Rigidbody2D rightLeg = action?.GetBalance(BalanceType.right_leg)?.Rb;
        Rigidbody2D leftLeg = action?.GetBalance(BalanceType.left_leg)?.Rb;

        if(body == null ||  rightLeg == null || leftLeg == null) return;

        if (Mathf.Abs(x) != 0)
        {

            if (x > 0)
            {
                body.AddForce(Vector2.right * bodyForce, ForceMode2D.Impulse);
                if (!isMovingRight)
                {

                    isMovingRight = true;
                    isMovingLeft = false;
                    if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                    moveCoroutine = StartCoroutine(MoveRight(legWait,action,rightLeg,leftLeg));
                }
            }
            else
            {
                body.AddForce(Vector2.left * bodyForce, ForceMode2D.Impulse);
                if (!isMovingLeft)
                {

                    isMovingLeft = true;
                    isMovingRight = false;
                    if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                    moveCoroutine = StartCoroutine(MoveLeft(legWait, action, rightLeg, leftLeg)); ;
                }

            }

        }
    }

    public void StopMoveCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        isMovingLeft = false;
        isMovingRight = false;
    }
    IEnumerator MoveRight(float seconds,IPostAction action,Rigidbody2D rightLeg,Rigidbody2D leftLeg)
    {
        while (isMovingRight)
        {
            //Debug.Log("Move right");

            //data.Walk_1();
            //leftLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);

            action.SetAction(StringConst.MOVE_STEP_1);
            SmoothMotionHelper.SmoothMoveTowards(rightLeg, rightLeg.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);

            //data.Walk_2();
            // rightLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);

            action.SetAction(StringConst.MOVE_STEP_2);
            SmoothMotionHelper.SmoothMoveTowards(leftLeg, leftLeg.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds, IPostAction action,Rigidbody2D rightLeg, Rigidbody2D leftLeg)
    {
        while (isMovingLeft)
        {

            //Debug.Log("Move Left");

            //data.Walk_2();
            //rightLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);

            action.SetAction(StringConst.MOVE_STEP_2);
            SmoothMotionHelper.SmoothMoveTowards(rightLeg, rightLeg.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);


            //data.Walk_1();
            //playerData.playerData.LeftLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);

            action.SetAction(StringConst.MOVE_STEP_1);
            SmoothMotionHelper.SmoothMoveTowards(leftLeg, leftLeg.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }

    #endregion
}