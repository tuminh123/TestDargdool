using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour
{
    [SerializeField] private Balance body;
    [SerializeField] Balance rightArm;
    [SerializeField] Balance rightArmDown;
    [SerializeField] Balance rightLeg;
    [SerializeField] Balance rightLegDown;

    [SerializeField] Balance leftArm;
    [SerializeField] Balance leftArmDown;
    [SerializeField] Balance leftLeg;
    [SerializeField] Balance leftLegDown;
    
    [SerializeField]private AttackDataConfigSO configSO;

    private AttackHandle[] attacks;
    // private BaseAttack[] rightAttacks;
    // private BaseAttack[] leftAttacks;
    // private int randAttackRight;
    // private int randAttackLeft;
    private float timeAttack;
    private Vector2 attackDir;
    
    //get
    public AttackDataConfigSO ConfigSo => configSO;
    public Vector2 AttackDir => attackDir;

    private void Start()
    {
        attacks = new[]
        {
            new AttackHandle(rightArm, rightArmDown, body,90, 80,attackDir),
            new AttackHandle(rightLeg, rightLegDown, body,110, 100,attackDir),
            new AttackHandle(rightArm, rightArmDown, body,140, -120,attackDir),
            new AttackHandle(rightLeg, rightLegDown, body,120, -60,attackDir),
            new AttackHandle(leftArm, leftArmDown, body,-90, -80,attackDir),
            new AttackHandle(leftLeg, leftLegDown, body,-110, -100,attackDir),
            new AttackHandle(leftArm, leftArmDown, body,-140, 120,attackDir),
            new AttackHandle(leftLeg, leftLegDown, body,-120, 60,attackDir)
        };
        
        /*rightAttacks = new BaseAttack[]
      {
          new BaseAttack(rightArm, rightArmDown, body,90, 80,attackDir),
          new BaseAttack(rightLeg, rightLegDown, body,110, 100,attackDir),
          new BaseAttack(rightArm, rightArmDown, body,140, -120,attackDir),
          new BaseAttack(rightLeg, rightLegDown, body,120, -60,attackDir)
      };

      leftAttacks = new BaseAttack[]
      {
          new BaseAttack(leftArm, leftArmDown, body,-90, -80,attackDir),
          new BaseAttack(leftLeg, leftLegDown, body,-110, -100,attackDir),
          new BaseAttack(leftArm, leftArmDown, body,-140, 120,attackDir),
          new BaseAttack(leftLeg, leftLegDown, body,-120, 60,attackDir)
      };

      randAttackRight = Random.Range(0, rightAttacks.Length);
      randAttackLeft = Random.Range(0, leftAttacks.Length);*/
    }

    private void FixedUpdate()
    {
        AttackDirHandle();
    }

    //private bool isAttacking;
    private void AttackDirHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;
    }

    public void BeginAttack()
    {
        int rand = Random.Range(0, attacks.Length);
        attacks[rand].AttackBegin(configSO);
    }

    public void EndAttack()
    {
        int rand = Random.Range(0, attacks.Length);
        attacks[rand].AttackEnd();
    }
    // public void AttackExecute()
    // {
    //     if(attackDir.x > 0 ) rightAttacks[randAttackRight].AttackBegin(configSO);
    //     if(attackDir.x < 0 ) leftAttacks[randAttackLeft].AttackBegin(configSO);
    // }
    //
    // public void RandomLeftAttack()
    // {
    //     if(attackDir.x > 0 ) rightAttacks[randAttackRight].AttackBegin(configSO);
    //     if(attackDir.x < 0 ) leftAttacks[randAttackLeft].AttackBegin(configSO);
    // }
}