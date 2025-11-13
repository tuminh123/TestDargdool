using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{   public float targetRotation;
    public Rigidbody2D rb;
    public float force;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

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
}
