using System;
using System.Collections;
using System.Collections.Generic;
/*using Sirenix.OdinInspector;*/
using UnityEngine;
using Random = UnityEngine.Random;

public class UIRagdoll : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] _allRb;
    [SerializeField] private float _randomForceStrength = 10;
    [SerializeField] private float _randomTorqueStrength = 100;
    private void Reset()
    {
        _allRb = GetComponentsInChildren<Rigidbody2D>();
    }
    private void Awake()
    {
        if (_allRb == null || _allRb.Length == 0)
        {
            Reset();
        }   
    }
    //[Button]
    public void EnableRagdoll()
    {
        foreach (var rb in _allRb)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    //[Button]
    public void DisableRagdoll()
    {
        foreach (var rb in _allRb)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void RandomForce()
    {
        foreach (var rb in _allRb)
        {
            rb.AddForce(new Vector2(Random.Range(-1,1), Random.Range(-1, 1)) * _randomForceStrength);
            rb.AddTorque( Random.Range(-1,1) * _randomTorqueStrength);
        }
    }
}
