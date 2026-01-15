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

    public IRagdollAttackSystem currentAttack { get; private set; }
    public RagdollAttackContext attackContext { get; private set; }

    //token
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;

    //Text attack effect
    [SerializeField] protected DamageNumber textEffect;
    protected string[] texts = { "Bump", "Bonk", "Baam", "Hit", "Pow", "Pop", "Thunk", "Smack", "Ahh" };

    //get
    public AttackDataConfigSO ConfigSO => configSO;
    public PhysicsAttackOriginalProfile PhysicsProfile => physicsProfile;

    private void Awake()
    {
        currentAttack = new AttackSmoothSystem(configSO);
        attackContext = new RagdollAttackContext(currentAttack);
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