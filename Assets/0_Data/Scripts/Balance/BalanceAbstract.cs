using System;
using System.Collections;
using UnityEngine;

public abstract class BalanceAbstract : MonoBehaviour
{
    [SerializeField] protected float targetRotation;
    [SerializeField] protected float force;
    [SerializeField] protected bool isActive = true;

    private Coroutine gravityRoutine;
    private Coroutine lerpRoutine;


    protected Rigidbody2D rb;
    //get
    public float TargetRotation => targetRotation;
    public float Force => force;
    public Rigidbody2D Rb => rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isActive) 
        BalanceHandle();
    }

    public virtual void BalanceHandle()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
    }

    public void SetTargetRotation(float targetRotation)
    {
        this.targetRotation = targetRotation;
    }
    public void SetForce(float force)
    {
        this.force = force;
    }
    public void SetIsActive(bool isActive)
    {
        this.isActive = isActive;
    }
    // ======================================================================
    // 🚀 Bổ sung thêm phần ragdoll hỗ trợ vật lý tấn công "bồng bềnh"
    // ======================================================================

    /// <summary>
    /// Thêm một lực đẩy vật lý (dùng khi tấn công)
    /// </summary>
    public virtual void ApplyImpulse(Vector2 direction, float magnitude)
    {
        if (!isActive || rb == null) return;
        rb.AddForce(direction.normalized * magnitude, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Giảm gravity tạm thời để tạo cảm giác bay nhẹ như trong Ragdoll Fists
    /// </summary>
    public virtual void ModifyGravityTemporarily(float multiplier, float duration)
    {
        if (gravityRoutine != null) StopCoroutine(gravityRoutine);

        gravityRoutine = StartCoroutine(GravityRoutine(multiplier, duration));
    }

    private IEnumerator GravityRoutine(float multiplier, float duration)
    {
        float original = rb.gravityScale;
        rb.gravityScale *= multiplier;
        yield return new WaitForSeconds(duration);
        rb.gravityScale = original;
    }

    /// <summary>
    /// Bật lại phần thân một cách mềm mại (thay vì bật cứng)
    /// </summary>
    public virtual void LerpToActive(float duration)
    {
        if (lerpRoutine != null) StopCoroutine(lerpRoutine);
        lerpRoutine = StartCoroutine(LerpActiveRoutine(duration));
    }

    private IEnumerator LerpActiveRoutine(float duration)
    {
        float timer = 0f;
        Vector2 startVel = rb.linearVelocity;
        Vector2 targetVel = Vector2.zero;
        isActive = true; // bật lại chế độ cân bằng

        while (timer < duration)
        {
            rb.linearVelocity = Vector2.Lerp(startVel, targetVel, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = targetVel;
    }
}
