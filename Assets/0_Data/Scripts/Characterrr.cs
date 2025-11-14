using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characterrr : MonoBehaviour
{
    #region Object Attribute
    [Header("Body object")]
    [SerializeField] private Balance body;
    [Space]
    [Header("right")]
    [SerializeField] private Balance armRight;
    [SerializeField] private Balance forearmRight;
    [SerializeField] private Balance handRight;
    [SerializeField] private Balance legRight;
    [SerializeField] private Balance lower_leg_Right;
    [SerializeField] private Balance footRight;
    [Space]
    [Header("left")]
    [SerializeField] private Balance armLeft;
    [SerializeField] private Balance forearmLeft;
    [SerializeField] private Balance handLeft;
    [SerializeField] private Balance legLeft;
    [SerializeField] private Balance lower_leg_Left;
    [SerializeField] private Balance footLeft;
    #endregion

    #region Movement Attribute
    [Space]
    [Header("action index")]
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float legWait = .5f;
    [SerializeField] private float maxSpeed = 5f;

    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    #endregion

    #region Attack Attribute
    public float attackForce = 15f;
    public float handForce = 15f;
    public float legForce = 15f;
    public float bodyForce = 2f;
    public float damping = 0.9f;
    public float stiffness = 8f;
    public float attackDuration = 0.25f;
    #endregion

    [Space]
    [Header("Ground check")]
    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;


    private List<IAttackAction> leftActions = new List<IAttackAction>();
    private List<IAttackAction> rightActions = new List<IAttackAction>();
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        InitLeftAttack();
        InitRightAttack();

    }
    private void Start()
    {

        // Giúp vật lý mượt hơn
        body.Rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        legLeft.Rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        legRight.Rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(x) != 0)
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

    }
    private void FixedUpdate()
    {
        AttackHandle();
        MoveHandle();
        JumpHandle();

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

    #region Attack
    //Attack handle
    private void AttackHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 attackDir = (mouseWorld - body.transform.position).normalized;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (attackDir.x < 0) AttackLeft(attackDir);
        //    if (attackDir.x > 0)
        //    {
        //        //AttackRight(attackDir);
        //        rightActions[2].Execute(attackDir);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.E)) AttackLeft(attackDir);
        if (Input.GetKeyDown(KeyCode.Q)) AttackRight(attackDir);
    }

    // Right attack
    #region Right Attack
    private void InitRightAttack()
    {
        rightActions.Add(new PunchAttack(armRight,forearmRight,handRight, body,attackForce, handForce, bodyForce, damping, stiffness, attackDuration, 115f, 85f, 45f));
        rightActions.Add(new KickAttack(legRight,footRight, body, 40f, 25f, legForce, bodyForce * 1.5f, damping, stiffness, attackDuration));
        rightActions.Add(new ElbowAttack(armRight,forearmRight, body, 100f, 120f, attackForce * 0.8f, bodyForce * 0.8f, damping, stiffness, attackDuration));
        rightActions.Add(new KneeAttack(legRight, body, 55f, legForce, bodyForce * 1.2f, damping, stiffness, attackDuration));
    }

    //Random attack right
    private void AttackRight(Vector2 attackDir)
    {
        if (rightActions.Count == 0) return;
        int idx = Random.Range(0, rightActions.Count);
        StartCoroutine(rightActions[idx].Execute(attackDir));
    }

    #endregion

    // Left Attack
    #region Left Attack
    private void InitLeftAttack()
    {
        // Khởi tạo Left actions
        leftActions.Add(new PunchAttack(armLeft,forearmLeft,handLeft, body,attackForce, handForce, bodyForce, damping, stiffness, attackDuration, -115f, -85f, -45f));
        leftActions.Add(new KickAttack(legLeft, footLeft, body, -40f, -25f, legForce, bodyForce * 1.5f, damping, stiffness, attackDuration));
        leftActions.Add(new ElbowAttack(armLeft,forearmLeft, body, -100f, -120f, attackForce * 0.8f, bodyForce * 0.8f, damping, stiffness, attackDuration));
        leftActions.Add(new KneeAttack(legLeft, body, -55f, legForce, bodyForce * 1.2f, damping, stiffness, attackDuration));
    }
    //random
    private void AttackLeft(Vector2 attackDir)
    {
        if (leftActions.Count == 0) return;
        int idx = Random.Range(0, leftActions.Count);
        StartCoroutine(leftActions[idx].Execute(attackDir));
    }
    #endregion

    #endregion

    #region Jump
    //jump
    private void JumpHandle()
    {
        bool isGround = IsGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            body.Rb.AddForce(Vector2.up * bodyForce, ForceMode2D.Impulse);
            legLeft.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
            legRight.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
        }
    }
    #endregion

    #region Moving

    private void MoveHandle()
    {
        float x = Input.GetAxisRaw("Horizontal");
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
            // Thả phím → giảm tốc mượt
            body.Rb.linearVelocity = new Vector2(body.Rb.linearVelocity.x * damping, body.Rb.linearVelocity.y);
        }
    }
    IEnumerator MoveRight(float seconds)
    {
        while (isMovingRight)
        {
            legLeft.Rb. AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            legRight.Rb.AddForce(Vector2.right * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator MoveLeft(float seconds)
    {
        while (isMovingLeft)
        {
            legRight.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
            yield return new WaitForSeconds(seconds);
            legLeft.Rb.AddForce(Vector2.left * (speed * 1000) * Time.fixedDeltaTime);
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
