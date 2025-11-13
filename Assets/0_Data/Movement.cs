using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    // body object
    [Header("Body object")]
    [SerializeField] GameObject leftLeg;
    [SerializeField] GameObject rightLeg;
    [SerializeField] GameObject down_rightLeg;
    [SerializeField] GameObject down_leftLeg;
    [SerializeField] GameObject body;
    Rigidbody2D leftLegRB;
    Rigidbody2D rightLegRB;
    private Rigidbody2D bodyRB;
    [Space]
    [Header("action index")]
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] float bodyForce = 1f;
    [SerializeField] private float damping = 0.85f;   // giảm tốc khi thả phím
    [SerializeField] private float maxSpeed = 5f;

    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    [Space]
    [Header("Ground check")]
    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    //private HingeJoint2D leftHinge;
    //[SerializeField] private HingeJoint2D down_leftHinge;
    //private HingeJoint2D rightHinge;
    //[SerializeField]private HingeJoint2D down_rightHinge;
    
    //component
    Animator anim;
    private void Awake()
    {
        leftLegRB = leftLeg.GetComponent<Rigidbody2D>();
        rightLegRB = rightLeg.GetComponent<Rigidbody2D>();
        bodyRB = body.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        //leftHinge = leftLeg.GetComponent<HingeJoint2D>();
        //rightHinge = rightLeg.GetComponent <HingeJoint2D>();
        //down_leftHinge = down_leftLeg.GetComponent<HingeJoint2D>();
        //down_rightHinge = down_rightHinge.GetComponent<HingeJoint2D>();

        //leftHinge.useLimits = true;
        //rightHinge.useLimits = true;
        //down_leftHinge.useLimits = true;
        //down_rightHinge.useLimits = true;

        // Giúp vật lý mượt hơn
        bodyRB.interpolation = RigidbodyInterpolation2D.Interpolate;
        leftLegRB.interpolation = RigidbodyInterpolation2D.Interpolate;
        rightLegRB.interpolation = RigidbodyInterpolation2D.Interpolate;
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = InputManager.Instance.xInput;
        if(Mathf.Abs(x) != 0)
        {
            if (isMovingLeft)
                anim.Play("walk_left");
            else if (isMovingRight)
                anim.Play("walk_right");
        }
        else
        {
            anim.Play("idle");
        }
            
        //anim.SetBool("isMove", InputManager.Instance.xInput != 0);
    }

    private void FixedUpdate()
    {
        MoveHandle();
        JumpHandle();
        
        // -----------------------------
        // GIỚI HẠN VẬN TỐC TỐI ĐA
        // -----------------------------
        LimitVelocity(bodyRB);
        LimitVelocity(leftLegRB);
        LimitVelocity(rightLegRB);
    }
    private void LimitVelocity(Rigidbody2D rb)
    {
        if (Mathf.Abs(rb.linearVelocityX) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxSpeed, rb.linearVelocityY);
        }
    }

    //jump
    private void JumpHandle()
    {
        bool jumpInput = InputManager.Instance.jumpInput;
        bool isGround = IsGround();
        
        if (jumpInput && isGround)
        {
            bodyRB.AddForce(Vector2.up*bodyForce,ForceMode2D.Impulse);
            leftLegRB.AddForce(Vector2.up * (jumpHeight*1000));
            rightLegRB.AddForce(Vector2.up * (jumpHeight * 1000));
        }
    }


    #region Moving

    private void MoveHandle()
    {
        float x = InputManager.Instance.xInput;
        if(Mathf.Abs(x) != 0)
        {
            //leftHinge.limits = new JointAngleLimits2D { min = -60, max = 45 };
            //down_leftHinge.limits = new JointAngleLimits2D { min = 0, max = 130 };
            //rightHinge.limits = new JointAngleLimits2D { min = -45, max = 60 };
            //down_rightHinge.limits = new JointAngleLimits2D { min = -130, max = 0 };

            if (x > 0)
            {
                bodyRB.AddForce(Vector2.right*bodyForce,ForceMode2D.Impulse);
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
                bodyRB.AddForce(Vector2.left*bodyForce,ForceMode2D.Impulse);
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
            // Thả phím → giảm tốc mượt
            bodyRB.linearVelocity = new Vector2(bodyRB.linearVelocity.x * damping, bodyRB.linearVelocity.y);
            //leftHinge.limits = new JointAngleLimits2D { min = 0, max = 0 };
            //rightHinge.limits = new JointAngleLimits2D { min = 0, max = 0 };
            //down_leftHinge.limits = new JointAngleLimits2D { min = 0, max = 0 };
            //down_rightHinge.limits = new JointAngleLimits2D { min = 0, max = 0 };
        }
    }
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            leftLegRB.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            rightLegRB.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {
            rightLegRB.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            leftLegRB.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }
    /*IEnumerator MoveRight(float seconds)
    {
        leftLegRB.AddForce(Vector2.right * (speed*1000) * Time.deltaTime);
        yield return new WaitForSeconds(seconds);
        rightLegRB.AddForce(Vector2.right * (speed * 1000) * Time.deltaTime);
    }

    IEnumerator MoveLeft(float seconds)
    {
        rightLegRB.AddForce(Vector2.left * (speed * 1000) * Time.deltaTime);
        yield return new WaitForSeconds(seconds);
        leftLegRB.AddForce(Vector2.left * (speed * 1000) * Time.deltaTime);
    }*/
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
