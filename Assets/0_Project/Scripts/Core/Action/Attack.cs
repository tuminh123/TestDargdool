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

    public IRagdollAttackSystem currentAttack;

    //token
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;

    //Text attack effect
    [SerializeField] protected DamageNumber textEffect;
    protected string[] texts = { "Bump", "Bonk", "Baam", "Hit", "Pow", "Pop", "Thunk", "Smack", "Ahh" };

    private void Awake()
    {
        currentAttack = new AttackSmoothSystem(configSO);
    }

    public void SetAttack(IRagdollAttackSystem attack)
    {
        this.currentAttack = attack;
    }
    public void ExecuteAttack(Vector2 dir,IPostAction postAction,string name,GameObject obj)
    {

        atc = new CancellationTokenSource();
        ltc = CancellationTokenSource.CreateLinkedTokenSource(atc.Token,obj.GetCancellationTokenOnDestroy());
        var token = ltc.Token;

        UniTaskSafe.Forget
        (
            ct => this.currentAttack.ExecuteAttack(ct, dir, postAction, name),
            token,
            "attacking task"
        );

       
    }
    public void CancelAttack()
    {
        if (ltc != null)
        {
            if (!ltc.IsCancellationRequested) ltc.Cancel();

            ltc.Dispose();
            ltc = null;
        }

        if (atc != null)
        {
            if (!atc.IsCancellationRequested) atc.Cancel();

            atc.Dispose();
            atc = null;
        }
    }

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