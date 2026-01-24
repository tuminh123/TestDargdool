using UnityEngine;

public class SnowBossShoot : CharacterShoot
{
    public SnowBossShoot(Transform shootPoint) : base(shootPoint)
    {

    }

    public override void Shoot(Vector2 dir)
    {
        if (shootPoint == null) return;

        IceBomb iceBomb = ZenManager.Instance?.itemPoolManager
            ?.Spawn(StringConst.BOMB_ICE, shootPoint.position, Quaternion.identity) as IceBomb;

        if (iceBomb == null) return;

        Rigidbody2D rb = iceBomb.rb;

        if (rb == null) return;
        //rb.AddForce(dir*iceBomb.Force,ForceMode2D.Impulse);
        rb.linearVelocity = dir * iceBomb.Force;
    }
}
