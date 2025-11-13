using System.Collections;
using UnityEngine;

public class Moving : ActionBase
{
    [Header("action index")]
    [SerializeField] float speed = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] float bodyForce = 1f;
    [SerializeField] private float damping = 0.85f;   // giảm tốc khi thả phím
    [SerializeField] private float maxSpeed = 5f;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    private BodyBalance bodyBalance;
    /// <summary>
    /// Lower body
    /// </summary>
    // left
    private LeftFootBalance leftFootBalance;
    private LeftLegBalance leftLegBalance;
    private LeftPillowBalance leftPillowBalance;
    // right
    private RightFootBalance rightFootBalance;
    private RightLegBalance rightLegBalance;
    private RightPillowBalance rightPillowBalance;

    

    protected override void Awake()
    {
        base.Awake();
        bodyBalance = GetComponentInChildren<BodyBalance>();
        // leg
        leftFootBalance = GetComponentInChildren<LeftFootBalance>();
        leftLegBalance = GetComponentInChildren<LeftLegBalance>();
        leftPillowBalance = GetComponentInChildren<LeftPillowBalance>();
        //right
        rightFootBalance = GetComponentInChildren<RightFootBalance>();
        rightLegBalance = GetComponentInChildren<RightLegBalance>();
        rightPillowBalance = GetComponentInChildren<RightPillowBalance>();

    }
    private void FixedUpdate()
    {
        // -----------------------------
        // GIỚI HẠN VẬN TỐC TỐI ĐA
        // -----------------------------
        LimitVelocity(bodyBalance.Rb);
        LimitVelocity(leftFootBalance.Rb);
        LimitVelocity(rightFootBalance.Rb);
    }
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }
    public void MoveHandle()
    {
        float x = InputManager.Instance.xInput;
        if (Mathf.Abs(x) != 0)
        {

            if (x > 0)
            {
                bodyBalance.Rb.AddForce(Vector2.right * bodyForce, ForceMode2D.Impulse);

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
                bodyBalance.Rb.AddForce(Vector2.left * bodyForce, ForceMode2D.Impulse);
                //anim.Play("walk_left");
                if (!isMovingLeft)
                {
                    isMovingLeft = true;
                    isMovingRight = false;
                    StopAllCoroutines();
                    StartCoroutine(MoveLeft(legWait));
                }

            }

        }
        else if (InputManager.Instance.attackInput)
        {
            characterCtrl.ChangeState(state.attack);
        }
        else
        {
            characterCtrl.ChangeState(state.idle);
        }
    }
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            leftFootBalance.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            rightFootBalance.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {
            rightFootBalance.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            leftFootBalance.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }

}
