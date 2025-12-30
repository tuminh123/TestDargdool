using Core;
using System;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

public abstract class Box : ObjInGameBase,IPhysicReceiveDamage,IGameElement,IReceive<SignalSendDamage>
{
    public Animator ani { get; private set; }
    public BoxHealth boxHealth { get;private set; }

    public GameObject Owner => transform.gameObject;

    public bool HasSetup { get; private set; }

    private float damage;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        boxHealth = GetComponent<BoxHealth>();
    }
    #region Signal Event
    private void OnEnable()
    {
        EnableSetup();
    }
    private void OnDisable()
    {
        DisableSetup();
    }

    public void EnableSetup()
    {
        if (!HasSetup)
        {
            HasSetup = true;
            Global.Add(this);
        }
        Enable();
    }

    public void DisableSetup()
    {
        if (HasSetup)
        {
            HasSetup = false;
            Global.Remove(this);
        }
        Disable();
    }

    public virtual void Enable()
    {
    }

    public virtual void Disable()
    {
    }
    #endregion


    public void Receive(in SignalSendDamage signal)
    {
        damage = signal.damaged;
    }

    public  void ReceiveHit(float rawDamage, Vector2 force)
    {
        float finalDamage = damage * rawDamage;

        boxHealth.TakeDamaged(finalDamage);
    }
}
