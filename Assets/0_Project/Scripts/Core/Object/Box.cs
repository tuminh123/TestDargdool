using System;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

public class Box : ObjInGameBase
{
    public Animator ani { get; private set; }
    public BoxHealth boxHealth { get;private set; }
    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        boxHealth = GetComponentInChildren<BoxHealth>();
    }

    public override string GetObjectName()
    {
        return StringConst.BOX;
    }
}
