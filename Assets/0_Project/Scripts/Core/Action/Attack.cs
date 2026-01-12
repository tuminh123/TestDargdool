using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;
public class Attack : MonoBehaviour
{
    //public event Action<Vector2> OnAttack;
    [SerializeField] private AttackDataConfigSO configSO;
    [SerializeField] private PhysicsAttackOriginalProfile physicsProfile;

    private IAttack attack;



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
    private CancellationTokenSource linkedCTS;
    public async UniTask HandleAttack(Vector2 attackDir,ActionPostBase postBase,string nameAttack,GameObject @object)
    {

        CancelAttack();

        attackCTS = new CancellationTokenSource();
        linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(attackCTS.Token,@object.GetCancellationTokenOnDestroy()) ;
        var token = linkedCTS.Token;

        AttackData attackData = new AttackData();

        currentAttackData = attackData;

        ShowTextEffect(attackDir);

        await attackData.ExecuteAttack(configSO,attackDir, token,postBase,nameAttack);
    }

    public void CancelAttack()
    {
        if (attackCTS == null)
        {
            attackCTS.Cancel();
            attackCTS.Dispose();
            attackCTS = null;
        }
        if(linkedCTS != null)
        {
            linkedCTS.Cancel();
            linkedCTS.Dispose();
            linkedCTS = null;
        }
    }

    #endregion

    #region Attack Handle

    public void EnterAttack(CharacterParent c, string leftAttack, string rightAttack)
    {
        if (c == null) return;

        c.ragdollController.actionBase.SetPostAction(new ActionPostSmooth(configSO));

        c.attackContext.EnableAttack();
        c.attackContext.EnableAttackPhysics(c.AttackDir);

        string name = c.AttackDir.x < 0 ? leftAttack : rightAttack;

        HandleAttack(c.AttackDir, c.ragdollController.actionBase, name,c.gameObject).Forget();

        c.SendDamageBase();

        if (currentAttackData == null) return;
        //currentAttackData.EnableEffect(true);

    }
    public void ExitAttack(CharacterParent c)
    {
        CancelAttack();
        if (c == null) return;
        c.attackContext.DisableAttack();
        c.attackContext.ResetPhysics();

        if (currentAttackData == null) return;
        //currentAttackData.EnableEffect(false);

    }
    #endregion

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