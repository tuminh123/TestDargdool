using System;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

public abstract class Box : ObjInGameBase
{
    public Animator ani { get; private set; }
    public BoxHealth boxHealth { get;private set; }
    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        boxHealth = GetComponentInChildren<BoxHealth>();
    }
}
