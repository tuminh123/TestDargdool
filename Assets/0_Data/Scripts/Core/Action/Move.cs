using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class MoveData
{
    [SerializeField] private Balance rightLeg,rightThigh,rightUpArm,rightLowArm,rightHand;
    [SerializeField] private Balance leftLeg,leftThigh,leftUpArm,leftLowArm,leftHand;
    [SerializeField] private Balance body; 

    public Balance RightLeg => rightLeg;
    public Balance RightThing => rightThigh;
    public Balance LeftLeg => leftLeg;
    public Balance LeftThing => leftThigh;
    public Balance Body => body;

    //Walk Right
    public void Walk_1()
    {
        //right
        rightLeg.SetRotation(45);
        rightThigh.SetRotation(40);
        rightUpArm.SetRotation(40);
        rightLowArm.SetRotation(40);
        rightHand.SetRotation(35);

        //left
        leftLeg.SetRotation(-45);
        leftThigh.SetRotation(-50);
        leftUpArm.SetRotation(-40);
        leftLowArm.SetRotation(-40);
        leftHand.SetRotation(-40);
    }
    public void Walk_2()
    {
        //right
        rightLeg.SetRotation(-45);
        rightThigh.SetRotation(-50);
        rightUpArm.SetRotation(-40);
        rightLowArm.SetRotation(-40);
        rightHand.SetRotation(-35);

        //left
        leftLeg.SetRotation(45);
        leftThigh.SetRotation(40);
        leftUpArm.SetRotation(40);
        leftLowArm.SetRotation(40);
        leftHand.SetRotation(40);
    }

}

public class Move :MonoBehaviour
{
    [SerializeField] private MoveData data;
    
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

    private void FixedUpdate()
    {
        LimitMoving();
    }

    public void LimitMoving()
    {
        LimitVelocity(data.Body.Rb);
        LimitVelocity(data.LeftLeg.Rb);
        LimitVelocity(data.RightLeg.Rb);
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
                data.Body.Rb.AddForce(Vector2.right * bodyForce, ForceMode2D.Impulse);
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
                data.Body.Rb.AddForce(Vector2.left * bodyForce, ForceMode2D.Impulse);
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
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            //Debug.Log("Move right");

            data.Walk_1();
            //leftLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(data.RightLeg.Rb, data.RightLeg.Rb.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);

            data.Walk_2();
            // rightLeg.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(data.LeftLeg.Rb, data.LeftLeg.Rb.position + Vector2.right * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {

            //Debug.Log("Move Left");

            data.Walk_2();
            //rightLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(data.RightLeg.Rb, data.RightLeg.Rb.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);


            data.Walk_1();
            //data.data.LeftLeg.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            SmoothMotionHelper.SmoothMoveTowards(data.LeftLeg.Rb, data.LeftLeg.Rb.position + Vector2.left * speed * Time.fixedDeltaTime, maxSpeed);

            yield return new WaitForSeconds(seconds);
        }
    }

    #endregion
  
}