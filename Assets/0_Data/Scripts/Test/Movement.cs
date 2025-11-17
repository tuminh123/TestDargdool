using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private Balance body;
    [SerializeField] private Balance legLeft;
    [SerializeField] private Balance legRight;

    #region Movement Attribute
    [Space]
    [Header("action index")]
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên

    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    #endregion
    private AttackingTest attackingTest;
    private void Awake()
    {
        attackingTest = GetComponent<AttackingTest>();
    }
    private void FixedUpdate()
    {
        MoveHandle();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            legLeft.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
            legRight.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
        }

        // -----------------------------
        // GIỚI HẠN VẬN TỐC TỐI ĐA
        // -----------------------------
        LimitVelocity(body.Rb);
        LimitVelocity(legLeft.Rb);
        LimitVelocity(legRight.Rb);
    }
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }
    #region Moving

    private void MoveHandle()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (attackingTest.IsAttacking) return;
        if (Mathf.Abs(x) != 0)
        {

            if (x > 0)
            {
                body.Rb.AddForce(Vector2.right * bodyForce, ForceMode2D.Impulse);
                if (!isMovingRight)
                {
                    
                    isMovingRight = true;
                    isMovingLeft = false;
                    StopAllCoroutines();
                    StartCoroutine(MoveRight(legWait));
                }
            }
            else
            {
                body.Rb.AddForce(Vector2.left * bodyForce, ForceMode2D.Impulse);
                if (!isMovingLeft)
                {
                    
                    isMovingLeft = true;
                    isMovingRight = false;
                    StopAllCoroutines();
                    StartCoroutine(MoveLeft(legWait));
                }

            }

        }
        else
        {
            legLeft.SetPropertie(0, 20);
            legRight.SetPropertie(0, 20);
            body.Rb.linearVelocity = new Vector2(body.Rb.linearVelocity.x * damping, body.Rb.linearVelocity.y);
        }
    }
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            legRight.SetTargetRotation(5);
            legLeft.SetTargetRotation(90);

            legLeft.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);

            legRight.SetTargetRotation(90);
            legLeft.SetTargetRotation(5);

            legRight.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {
            legRight.SetTargetRotation(-90);
            legLeft.SetTargetRotation(-5);

            legRight.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);

            legRight.SetTargetRotation(-5);
            legLeft.SetTargetRotation(-90);

            legLeft.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }
    #endregion
}
