using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float damage = 10f;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private void Update()
    {
        transform.Rotate(_speed * Vector3.forward);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInChildren<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamaged(damage);
        Debug.Log("Rotator deal damage: " + damage);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
