using System.Collections;
using UnityEngine;

public interface IDamageable 
{
    public void TakeDamaged(float damage);
    public bool IsDead { get;}
} 