using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private BalanceType type;
    [SerializeField] private float targetRotation;
    [SerializeField] private float force;

    [SerializeField] private Rigidbody2D rb;
    //get
    public float TargetRotation=>targetRotation;
    public float Force=>force;
    public Rigidbody2D Rb => rb;
    public  BalanceType Type =>type;

    private void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.deltaTime));
    }
    public void SetTargetRotation(float targetRotation)
    {
        this.targetRotation = targetRotation;
    }
    public void SetForce(float force)
    {
        this.force = force;
    }
    public void SetType(BalanceType type)
    {
        this.type = type;
    }
}
