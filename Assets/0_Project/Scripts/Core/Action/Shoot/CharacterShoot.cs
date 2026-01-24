using UnityEngine;

public abstract class CharacterShoot : IShoot
{
    protected Transform shootPoint;

    public CharacterShoot(Transform shootPoint)
    {
        this.shootPoint = shootPoint;
    }
    public void SetShootPoint(Transform shootPoint)
    {
        this.shootPoint = shootPoint;
    }
    public abstract void Shoot(Vector2 dir);
}
