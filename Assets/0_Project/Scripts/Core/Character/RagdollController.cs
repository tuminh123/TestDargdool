using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
using Cysharp.Threading.Tasks;
public class RagdollController : MonoBehaviour
{
    [SerializeField] List<Balance> balances;
    [SerializeField] float knockdownThreshold = 10f;

    [SerializeField] private float explosionForce = 25f;
    [SerializeField] private float torqueForce = 15f;
    [SerializeField] private float cameraBias = 0.6f;

    bool isRagdoll;

    public void OnHit(Vector2 force, float impact)
    {
        if (impact < knockdownThreshold) return;
        EnterRagdoll();
    }

    public void EnterRagdoll()
    {
        if (isRagdoll) return;
        isRagdoll = true;

        foreach (var b in balances)
            b.DisablePose();
    }

    public void Recover()
    {
        foreach (var b in balances)
        {
            b.ResetState();
            b.EnablePose();
        }

        isRagdoll = false;
    }

    

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
