using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum BalanceType
//{
//    none = 0,
//    head = 1,
//    body = 2,
//    right_arm = 3,
//    right_forearm = 4,
//    right_hand = 5,
//    left_arm = 6,
//    left_forearm = 7,
//    left_hand = 8,
//    right_leg = 9,
//    right_lower_leg = 10,
//    right_foot = 11,
//    left_leg = 12,
//    left_lower_leg = 13,
//    left_foot = 14,

//}
public enum Faction { none = 0,player = 1,enemy = 2}

public class Balance : MonoBehaviour, IDamageFaction
{
    //[SerializeField] private BalanceType type;
    [SerializeField] private float targetRotation;
    [SerializeField] private float force;
    [SerializeField] private Rigidbody2D rb;
    private bool isTrigger = true;
    //faction
    [SerializeField] private Faction action;

    //get
    public float TargetRotation=>targetRotation;
    public float Force=>force;
    public Rigidbody2D Rb => rb;

    public Faction GetFaction => action;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isTrigger)
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
    }
    public void SetPropertie(float targetRotation,float force)
    {
        this.targetRotation = targetRotation;
        //this.targetRotation = Mathf.Lerp(this.targetRotation, targetRotation, Time.fixedDeltaTime * 10f);
        this.force = force;
    }
    public void SetTargetRotation(float targetRotation)
    {
        this.targetRotation = targetRotation;
    }
    public void SetForce(float force)
    {
        this.force = force;
    }
    public void SetIsTrigger(bool isTrigger)
    {
        this.isTrigger = isTrigger;
    }
}
