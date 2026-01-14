using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalanceType
{
    none = 0,
    head = 1,
    body_up = 2,
    right_arm = 3,
    right_forearm = 4,
    right_hand = 5,
    left_arm = 6,
    left_forearm = 7,
    left_hand = 8,
    right_leg = 9,
    right_lower_leg = 10,
    right_foot = 11,
    left_leg = 12,
    left_lower_leg = 13,
    left_foot = 14,
    body_bottom = 15,
    hip = 16,

}
public class Balance : MonoBehaviour
{
    #region Balance attribute
    [SerializeField] private float rot;
    [SerializeField] private float force = 30;

    [SerializeField] private float minRot = -180f;
    [SerializeField] private float maxRot = 180f;

    [SerializeField] private BalanceType type;

    private bool isPoseActive = true;
    #endregion

    #region Rigibody2D attribute
    private Vector2 startPos;
    private float startRot;
    private float startLinearDrag;
    private float startAngularDrag;
    private RigidbodyConstraints2D startConstraints;
    private float startGravity;
    private float startMass;
    #endregion

    #region Component State Cache
    public HingeJoint2D hinge { get; private set; }
    public Transform model { get; private set; }
    private Rigidbody2D rb;
    private Collider2D col;
    #endregion

    //get
    public BalanceType Type => type;
    public float Rotation=>rot;
    public float Force=>force;
    public Rigidbody2D Rb => rb;
    public Collider2D Col => col;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        hinge = GetComponent<HingeJoint2D>();
        model = transform.Find(StringConst.MODEL);
    }

    private void Start()
    {
        CacheState();
    }

    private void FixedUpdate()
    {
        HandleBalance();
    }

    private void HandleBalance()
    {
        
        if (isPoseActive)
        {
            float clampedRot = Mathf.Clamp(rot, minRot, maxRot);
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, clampedRot, force * Time.fixedDeltaTime));
        }
    }

    #region Set API
    public void EnablePose() => isPoseActive = true;
    public void DisablePose() => isPoseActive = false;
    public void SetRotation(float targetRotation)
    {
        this.rot = targetRotation;
    }
    public void SetForce(float force)
    {
        this.force = force;
    }
    #endregion

    #region Reset State
  

    public void CacheState()
    {
        startPos = rb.position;
        startRot = rb.rotation;
        startLinearDrag = rb.linearDamping;
        startAngularDrag = rb.angularDamping;
        startConstraints = rb.constraints;
        startGravity = rb.gravityScale;
        startMass = rb.mass;
    }

    public void ResetState()
    {
        /*rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;*/

        rb.position = startPos;
        rb.rotation = startRot;

        rb.linearDamping = startLinearDrag;
        rb.angularDamping = startAngularDrag;
        rb.constraints = startConstraints;
        rb.gravityScale = startGravity;
        rb.mass = startMass;
        // Đảm bảo physics reset
        rb.Sleep();
        rb.WakeUp();
    }
    #endregion

}
