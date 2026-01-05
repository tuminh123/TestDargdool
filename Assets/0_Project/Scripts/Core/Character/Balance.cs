using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Balance : MonoBehaviour
{
    [SerializeField] private float rot;
    [SerializeField] private float force;

    [SerializeField] private float minRot = -180f;
    [SerializeField] private float maxRot = 180f;

    //[SerializeField] private float smoothTime = 0.1f; // thời gian mượt
    //private float angularVelocity; // lưu velocity giữa các frame

    [SerializeField] private DefaultBalanceData dataSO;
    public HingeJoint2D hinge { get; private set; }

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isPoseActive = true;


    //get
    public DefaultBalanceData DataSO => dataSO;
    public float Rotation=>rot;
    public float Force=>force;
    public Rigidbody2D Rb => rb;
    public Collider2D Col => col;


    private void Awake()
    {
        ResetData();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        hinge = GetComponent<HingeJoint2D>();
    }
    //private void OnValidate()
    //{
    //    rotChange = dataSO.Torque;
    //    forceChange = dataSO.Force;
    //}
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
        //else
        //{
        //    //float clampedRot = Mathf.Clamp(rotChange, minRot, maxRot);
        //    float newRotation = Mathf.SmoothDampAngle(rb.rotation, clampedRot, ref angularVelocity, smoothTime);
        //    rb.MoveRotation(newRotation);
        //}
    }
    public void EnablePose() => isPoseActive = true;
    public void DisablePose() => isPoseActive = false;
    public void ResetData()
    {
        //Debug.Log("Reset");
        if (dataSO == null) return;
        dataSO.Init(out rot,out force);
    }
    public void SetPropertie(float targetRotation,float force)
    {
        this.rot = targetRotation;
        this.force = force;
    }
    public void SetRotation(float targetRotation)
    {
        this.rot = targetRotation;
    }
    public void SetForce(float force)
    {
        this.force = force;
    }

    ///////////////////////////////////////////////////////

    private Vector2 startPos;
    private float startRot;
    private float startLinearDrag;
    private float startAngularDrag;
    private RigidbodyConstraints2D startConstraints;
    private float startGravity;

    public void CacheState()
    {
        startPos = rb.position;
        startRot = rb.rotation;
        startLinearDrag = rb.linearDamping;
        startAngularDrag = rb.angularDamping;
        startConstraints = rb.constraints;
        startGravity = rb.gravityScale;
    }

    public void ResetState()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.position = startPos;
        rb.rotation = startRot;

        rb.linearDamping = startLinearDrag;
        rb.angularDamping = startAngularDrag;
        rb.constraints = startConstraints;
        rb.gravityScale = startGravity;

        // Đảm bảo physics reset
        rb.Sleep();
        rb.WakeUp();
    }
    /////////////////////////////////////////////////////////////////////


    private float baseMass;
    private float baseGravity;
    private float baseLinearDrag;
    private float baseAngularDrag;

    /// <summary>
    /// Làm bộ phận mất thăng bằng
    /// multiplier càng cao → càng nặng → càng khó kiểm soát
    /// </summary>
    public void Apply(float multiplier)
    {
        rb.mass = baseMass * multiplier;
        rb.gravityScale = baseGravity * multiplier;
        rb.linearDamping = baseLinearDrag * multiplier;
        rb.angularDamping = baseAngularDrag * multiplier;
    }

    /// <summary>
    /// Trả lại trạng thái ổn định
    /// </summary>
    public void Recover()
    {
        rb.mass = baseMass;
        rb.gravityScale = baseGravity;
        rb.linearDamping = baseLinearDrag;
        rb.angularDamping = baseAngularDrag;
    }

}
