using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
using Cysharp.Threading.Tasks;
public class RagdollController : MonoBehaviour
{
    #region Balance System
    [SerializeField] private Balance[] balances ;
    [SerializeField] private ActionDataSO[] actionsDataSO;

    protected LimbHitBox[] limbHitBoxs;
    protected PhysicsDamageDealer[] physicsCharacterDamageDealers;

    public IPostAction actionBase { get; private set; }
    #endregion

    [SerializeField] float knockdownThreshold = 10f;
    [SerializeField] float knockbackForce = 100f;

    #region Exeplosion
    [SerializeField] private float explosionForce = 25f;
    [SerializeField] private float torqueForce = 15f;
    [SerializeField] private float cameraBias = 0.6f;
    #endregion

    bool isRagdoll;

    //get
    public Balance[] Balances => balances;
    public ActionDataSO[] ActionsDataSO => actionsDataSO;
    private void Awake()
    {
        if (limbHitBoxs == null || limbHitBoxs.Length <= 0)
        {
            limbHitBoxs = transform.GetComponentsInChildren<LimbHitBox>();
        }

        if (physicsCharacterDamageDealers == null || physicsCharacterDamageDealers.Length <= 0)
        {
            physicsCharacterDamageDealers = transform.GetComponentsInChildren<PhysicsDamageDealer>();
        }

        if(balances == null || balances.Length <= 0)
        {
            balances = transform.GetComponentsInChildren<Balance>();
        }

        actionBase = new ActionPostBase(actionsDataSO, balances);
    }

    #region Init Limb
    public void InitLimbs(CharacterParent parent)
    {
        foreach (var limb in limbHitBoxs)
        {
            if (limb == null) continue;
            limb.Init(parent);
        }
    }

    public void InitPhysicDamageDeal(IObjSendDamage obj,IAttackContext context)
    {
        //Debug.Log($"[InitWeapons] attackContext = {attackContext}");
        foreach (var dealer in physicsCharacterDamageDealers)
        {
            if (dealer == null) continue;
            dealer.Init(obj,context);
        }
    }
    #endregion


    #region Hit Ragdoll
    public void OnHit(Vector2 force, float impact)
    {
        if (impact < knockdownThreshold) return;
        DisableRagdoll(force);
    }

    public void DisableRagdoll(Vector2 force)
    {
        if (isRagdoll) return;
        isRagdoll = true;

        foreach (var b in balances)
        {
            b.DisablePose();
            b.Rb.AddForce(force*knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void EnableRagdoll()
    {
        foreach (var b in balances)
        {
            //b.ResetState();
            b.EnablePose();
        }

        isRagdoll = false;
    }
    public void ResetRagdoll()
    {
        foreach (var b in balances)
        {
            b.ResetState();
            b.EnablePose();
        }
        isRagdoll = false;
    }
    #endregion

    public async UniTask Explode()
    {
        Vector2 camDir =  ((Vector2)Camera.main.transform.position - (Vector2)transform.position).normalized;

        foreach (Balance bal in balances)
        {
            if(bal == null) continue;

            HingeJoint2D join = bal.hinge;
            if (join == null) continue;

            Destroy(join);
            //transform.parent = null;

            Rigidbody2D rb = bal.Rb;
            if(rb == null) continue;

            Vector2 force = Random.insideUnitCircle.normalized * explosionForce;
            force += camDir * explosionForce * cameraBias;

            rb.AddForce(force, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-torqueForce, torqueForce));

            await UniTask.DelayFrame(1);
        }
        if (ZenManager.Instance == null || ZenManager.Instance.cameraShaker == null) return;
        ZenManager.Instance.cameraShaker.ShakeCam();

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
    }
}
