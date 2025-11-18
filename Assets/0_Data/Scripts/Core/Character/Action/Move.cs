using System;
using System.Collections;
using UnityEngine;

public class Move :MonoBehaviour
{
    [SerializeField]private Balance rightLeg, leftLeg,body;
    
    [SerializeField] float speed = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên
    
    private Coroutine moveCoroutine;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    #region  Utils

    private void FixedUpdate()
    {
        LimitMoving();
    }

    public void LimitMoving()
    {
        LimitVelocity(body.Rb);
        LimitVelocity(leftLeg.Rb);
        LimitVelocity(rightLeg.Rb);
    }
    //limit Handle
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }

    #endregion

    #region Moving handle

      public void MoveHandle(float x)
    {
        //if (isAttacking) return;
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
  
}