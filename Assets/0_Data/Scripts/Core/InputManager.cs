using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance{get; private set;}

    [Header("Buffer Settings")]
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float attackRightBufferTime = 0.1f;
    [SerializeField] private float attackLeftBufferTime = 0.1f;

    private float jumpBufferCounter;
    private float attackRightBufferCounter;
    private float attackLeftBufferCounter;

    public float xInput {get; private set;}
    public bool jumpInput{get; private set;}
    public bool attackInput{get; private set;}
    public bool attackRight{get; private set;}
    public bool attackLeft{get; private set;}

    private void Awake()
    {
        Instance = this;
    }
    private void FixedUpdate()
    {
        //SetInput();
        JumpBuffer();
        AttackLeftBuffer();
        AttackRightBuffer();
    }

    public void SetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        attackInput = Input.GetMouseButtonDown(0);
        attackRight = Input.GetKeyDown(KeyCode.E);
        attackLeft = Input.GetKeyDown(KeyCode.Q);
    }
    #region JumpBuffer
    private void JumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        if(jumpBufferTime > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    public bool ConsumeJumpBuffer()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter = 0;
            return true;
        }
        return false;
    }
    #endregion

    #region Attack buffer
   
    private void AttackRightBuffer()
    {
        if (Input.GetKeyDown(KeyCode.E))
            attackRightBufferCounter = attackRightBufferTime;

        if (attackRightBufferCounter > 0)
            attackRightBufferCounter -= Time.deltaTime;
    }
    private void AttackLeftBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            attackLeftBufferCounter = attackLeftBufferTime;

        if (attackLeftBufferCounter > 0)
            attackLeftBufferCounter -= Time.deltaTime;
    }
    public bool ConsumeAttackRightBuffer()
    {
        if (attackRightBufferCounter > 0)
        {
            attackRightBufferCounter = 0;
            return true;
        }
        return false;
    }
    public bool ConsumeAttackLeftBuffer()
    {
        if (attackLeftBufferCounter > 0)
        {
            attackLeftBufferCounter = 0;
            return true;
        }
        return false;
    }
    #endregion
}
