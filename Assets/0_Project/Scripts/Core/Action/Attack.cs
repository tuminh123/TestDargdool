using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class AttackProfileSO 
{
    public float windupTime = 0.05f;
    public float recoveryTime = 0.3f;

    [Header("Root Force")]
    public float rootForce = 8f;
    public float maxRootSpeed = 6f;

    [Header("Pose")]
    public float poseStrengthDuringAttack = 0.4f;

    [Header("Damping")]
    public float attackDrag = 4f;
    public float attackAngularDrag = 6f;
}
public class Attack : MonoBehaviour
{
    public event Action<Vector2> OnAttack;
    public event Action OnAttackEnd;
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
   /* private Coroutine attackRoutine;

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

    public void CancelAttack()
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

    #endregion
   */
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
        OnAttack?.Invoke(attackDir);
        CancelAttack();

        attackCTS = new CancellationTokenSource();
        var token = attackCTS.Token;

        bool isRight = attackDir.x > 0;

        AttackData attack = GetRandomAttack(isRight);

        if (attack == null) return;
        currentAttackData = attack;

        ShowTextEffect(attackDir);

        if (attack == null) return;

        await attack.ExecuteAttack(configSO,attackDir, token);

        OnAttackEnd?.Invoke();
    }

    public void CancelAttack()
    {
        if (attackCTS == null) return;
        attackCTS.Cancel();
        attackCTS.Dispose();
        attackCTS = null;
    }

    public bool CanAttack()
    {
        return rightAttacks.Length > 0 && leftAttacks.Length > 0;
    }

    #endregion


    // ============================================================
    #region Weapon Attack
    // ============================================================

/*    public async UniTask HandleWeaponAttack(Vector2 attackDir)
    {
        CancelAttack();

        attackCTS = new CancellationTokenSource();
        var token = attackCTS.Token;

        bool isRight = attackDir.x > 0;

        AttackData attack = null;
        if (attackDir.x > 0)
            attack = weaponRightAttack;
        else if (attackDir.x < 0)
            attack = weaponLeftAttack;

        if (attack == null) return;
        currentAttackData = attack;

        ShowTextEffect(attackDir);

        if (attack == null) return;

        await attack.Execute(attackDir, token);
    }
*/
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

    //[SerializeField] Rigidbody2D body;
    //[SerializeField] Rigidbody2D arm;
    //[SerializeField] PoseMotor[] poseMotors;
    //[SerializeField] Balance[] balances;
    //public void HandleAttack(Vector2 dir)
    //{
    //    StartCoroutine(AttackHandle(dir));
    //}
    //public IEnumerator AttackHandle(Vector2 dir)
    //{
    //    // 1. Chuẩn bị
    //    foreach (var p in poseMotors) p.Enable();
    //    foreach (var b in balances) b.Recover();

    //    yield return new WaitForSeconds(0.1f);

    //    // 2. Đánh
    //    foreach (var p in poseMotors) p.Disable();
    //    foreach (var b in balances) b.Apply(2f);

    //    body.AddForce(dir * 6f, ForceMode2D.Impulse);
    //    arm.AddForce(dir * 4f, ForceMode2D.Impulse);

    //    yield return new WaitForSeconds(0.15f);

    //    // 3. Hồi
    //    foreach (var b in balances) b.Recover();
    //    foreach (var p in poseMotors) p.Enable();
    //}
}