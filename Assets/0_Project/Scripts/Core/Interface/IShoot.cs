using System.Collections;
using UnityEngine;

/*public enum RangeWeaponType
{
    None = 0,
    Shuriken = 1,
    Dagger = 2,
    Bow = 3,
} 
*/public interface IShoot
{
    //public RangeWeaponType type { get; }
    public void Shoot(Vector2 dir);
}