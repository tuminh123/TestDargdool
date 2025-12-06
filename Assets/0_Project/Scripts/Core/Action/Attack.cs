using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Attack : MonoBehaviour
{
    
    [SerializeField] private Balance body;
    [SerializeField] private AttackDataConfigSO configSO;

    [SerializeField] private AttackData[] leftAttacks;
    [SerializeField] private AttackData[] rightAttacks;

    [SerializeField] private AttackData weaponLeftAttack;
    [SerializeField] private AttackData weaponRightAttack;

    //Text attack effect
    [SerializeField] protected DamageNumber textEffect;
    protected string[] texts = { "Bump", "Bonk", "Baam", "Hit", "Pow", "Pop", "Thunk", "Smack", "Ahh" };

    public AttackData currentAttackData { get; private set; }

    private Coroutine attackRoutine;

    #region Combat attack
    public AttackData GetRandomAttack(bool isRight)
    {
        if (!CanAttack()) return null;
        var list = isRight ? rightAttacks : leftAttacks;
        return list[Random.Range(0, list.Length)];
    }
    public void HandleAttack(Vector2 attackDir)
    {
        bool isRight = attackDir.x > 0; 
        AttackData attack = GetRandomAttack(isRight);
        if (attack == null) return;
        currentAttackData = attack;

        //TextCombat
        int rand = Random.Range(0, texts.Length);
        string text = texts[rand];
        Vector3 pos = (Vector3) attackDir + transform.position;
        textEffect.Spawn(pos,text);

        attackRoutine = StartCoroutine(attack.ExecuteAttack(configSO,attackDir,body));
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
    public bool CanAttack()
    {
        return rightAttacks.Length > 0 && leftAttacks.Length > 0;
    }
    #endregion

    #region Weapon attack

    public void HandleWeaponAttack(Vector2 attackDir)
    {
        if (attackDir.x > 0)
        {
            currentAttackData = weaponRightAttack;

        }
        else if (attackDir.x < 0) 
        {
            currentAttackData = weaponLeftAttack;
        }
        if (currentAttackData == null) return;
        attackRoutine = StartCoroutine(currentAttackData.ExecuteAttack(configSO, attackDir, body));
    }

    #endregion

}