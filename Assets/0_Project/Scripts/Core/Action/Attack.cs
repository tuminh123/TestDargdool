using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;


public class Attack : MonoBehaviour
{
    public event Action<Vector2> OnAttack;
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

    #region Coutine 
    /*  private Coroutine attackRoutine;

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
          Vector3 pos = (Vector3)attackDir + transform.position;
          textEffect.Spawn(pos, text);

          attackRoutine = StartCoroutine(attack.ExecuteAttack(configSO, attackDir));
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
          attackRoutine = StartCoroutine(currentAttackData.ExecuteAttack(configSO, attackDir));
      }

      #endregion*/
    #endregion

    #region UniTask Combat attack

    private CancellationTokenSource attackCTS;


    // ============================================================
    #region Basic Combat Attack
    // ============================================================

    public AttackData GetRandomAttack(bool isRight)
    {
        if (!CanAttack()) return null;

        var list = isRight ? rightAttacks : leftAttacks;
        return list[Random.Range(0, list.Length)];
    }

    public async UniTask HandleAttack(Vector2 attackDir)
    {
        if (!CanAttack()) return;

        bool isRight = attackDir.x > 0;
        AttackData attack = GetRandomAttack(isRight);

        if (attack == null) return;
        currentAttackData = attack;

        ShowTextEffect(attackDir);

        // CANCEL ATTACK CŨ
        attackCTS?.Cancel();
        attackCTS = new CancellationTokenSource();
        var linkToken = CancellationTokenSource.CreateLinkedTokenSource(attackCTS.Token, this.GetCancellationTokenOnDestroy()).Token;

        // CHẠY ATTACK BẰNG UNITASK
        await attack.ExecuteAttack(
            configSO,
            attackDir,
            linkToken
        );

        OnAttack?.Invoke(attackDir);
    }

    public void StopAttack()
    {
        if (attackCTS != null)
        {
            attackCTS.Cancel();
            attackCTS = null;
        }
    }

    public bool CanAttack()
    {
        return rightAttacks.Length > 0 && leftAttacks.Length > 0;
    }

    #endregion


    // ============================================================
    #region Weapon Attack
    // ============================================================

    public async UniTask HandleWeaponAttack(Vector2 attackDir)
    {
        if (attackDir.x > 0)
            currentAttackData = weaponRightAttack;
        else if (attackDir.x < 0)
            currentAttackData = weaponLeftAttack;

        if (currentAttackData == null) return;

        ShowTextEffect(attackDir);

        attackCTS?.Cancel();
        attackCTS = new CancellationTokenSource();
        var linkToken = CancellationTokenSource.CreateLinkedTokenSource(attackCTS.Token, this.GetCancellationTokenOnDestroy()).Token;

        await currentAttackData.ExecuteAttack(
            configSO,
            attackDir,
            linkToken
        );
    }

    #endregion

    #endregion
    // ============================================================
    #region UI Text Popup
    // ============================================================

    private void ShowTextEffect(Vector2 attackDir)
    {
        int rand = Random.Range(0, texts.Length);
        string text = texts[rand];
        Vector3 pos = (Vector3)attackDir + transform.position;

        textEffect.Spawn(pos, text);
    }

    #endregion
}