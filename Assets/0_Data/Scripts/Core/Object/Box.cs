using System;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

public class Box : ObjInGameBase
{
    private Animator ani;
    public BoxHealth boxHealth { get;private set; }
    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        boxHealth = GetComponentInChildren<BoxHealth>();
    }
 
    public void Handle()
    {
        StartCoroutine(ExplosionHandle());
    }

    private IEnumerator ExplosionHandle()
    {
        ani.Play("explosion");
        yield return new WaitForSeconds(0.5f);
        Gold gold = SingletonManager.Instance.objInGamePoolManager.Spawn(StringConst.GOLD, transform.position, Quaternion.identity) as Gold;
        gold.SetVelocity();
        yield return new WaitForSeconds(0.2f);
        SingletonManager.Instance.objInGamePoolManager.DeSpawn(this);
       
    }
    private void OnDestroy()
    {
        StopCoroutine(ExplosionHandle());
    }
    private void OnDisable()
    {
        StopCoroutine(ExplosionHandle());
    }
    public override string GetObjectName()
    {
        return StringConst.BOX;
    }
}
